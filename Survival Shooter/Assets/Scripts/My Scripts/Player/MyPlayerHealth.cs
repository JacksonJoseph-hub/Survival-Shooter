using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerHealth : MonoBehaviour
{
    public int startingHeatlth = 100;                           // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);      // The color the damageImage is set to, to flash.

    Animator anime;                                             // Reference to the Animator component.
    AudioSource playerAudio;                                    // Reference to the AudioSource component.
    MyPlayerMovement playerMovement;                            // Reference to the player's movement.
    //PlayerShooting playerShooting;                            // Reference to the PlayerShooting script.
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged

    // Start is called before the first frame update
    void Awake()
    {
        // Setting up the references
        anime = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<MyPlayerMovement>();
        //playerShooting = GetComponent<MyPlayerShooting>();

        // Set the initial heath of the player
        currentHealth = startingHeatlth;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player has just been damaged
        if(damaged)
        {
            // Set the color of the damageImage to the flash color.
            damageImage.color = flashColor;
        }

        // Otherwise
        else 
        {
            // transition the color back to clear
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damage flag
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // Play the hurt sound effect.
        playerAudio.Play();

        // If the player has lost all it's health and the death flag hasn't been set yet
        if(currentHealth <= 0 && !isDead)
        {
            // it should die.
            Death();
        }
    }

    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Turn off any remianing shooting effects.
        //playerShooting.DisableEffect();

        // Tell the animator that the player is dead.
        anime.SetTrigger("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        playerAudio.clip = deathClip;
        playerAudio.Play();

        // Turn off the movement and shooting scripts.
        playerMovement.enabled = false;
        //playerShooting.enabled = false;
    }

}
