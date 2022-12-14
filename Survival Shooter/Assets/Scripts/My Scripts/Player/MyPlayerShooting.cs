using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;              // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;    // The time between each shot.
    public float range = 100f;                  // The distance the gun can fire.


    float timer;                                // A timer to determin when to fire.
    Ray shootRay = new Ray();                   // A ray from the gun end forwards.
    RaycastHit shootHit;                        // A raycast hit to get information about what was hit.
    int shootableMask;                          // A layer mask so the raycast only hits things on the shootable layer.
    LineRenderer gunLine;                       // Reference to the line renderer.
    AudioSource gunAudio;                       // Reference to the audio source.
    Light gunLight;                             // Reference to the light component.
    float effectsDisplayTime = 0.2f;            // The proportion of the timeBetweenBullets that the effects will display for.


    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");

        // Set up the references.
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being pressed and it's time to fire
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            // Shoot the gun.
            Shoot();
        }

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            // Disable the effects.
            DisableEffects();
        }
    }


    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot()
    {
        // Reset the timer.
        timer = 0f;

        // Play the gun shot audioclip.
        gunAudio.Play();

        // Enable the light.
        gunLight.enabled = true;

        // Enable the line renderer and set it's first position to the end of the gun
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            // Try and find an MyEnemyHealth scripton the gameobject hit.
            MyEnemyHealth enemyHealth = shootHit.collider.GetComponent<MyEnemyHealth>();

            // If the MyEnemyHealth component exists
            if (enemyHealth != null)
            {
                // The enemy should take damage.
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }

            // Set the second position of the line renderer to the point the raycast hit.
            gunLine.SetPosition(1, shootHit.point);
        }

        // If the raycast didn't hit anything on the shootable layer.
        else
        {
            // Set the second position of the line renderer to the fullest extent of the gun's range.
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
