using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;    // The amount of health the enemy starts the game with.
    public int currentHealth;           // The current health the enemy has.
    public float sinkSpeed = 2.5f;      // The speed at which the enemy sinks through the floor when dead.
    public int scoreValue = 10;         // The amount added to the player's score when the enemy dies.
    public AudioClip deathClip;         // The sound to play when the enemny dies.


    Animator anime;                     // References to the animator.
    AudioSource enemyAudio;             // References to the audio source.
    CapsuleCollider capsuleCollider;    // References to the capsule collider.
    bool isDead;                        // Whether the enemy is dead.
    bool isSinking;                     // Whether the enemy has started sinking through the floor.


    void Awake()
    {
        // Setting up the references.
        anime = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Setting the current health when the enemy first spawns.
        currentHealth = startingHealth;
    }


    void Update()
    {
        // If the enemy should be sinking.
        if (isSinking)
        {
            // move the enemy down by the sinkSpeed per second.
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        // If the enemy is dead
        if (isDead)
            // no need to take damage so exit the function.
            return;

        // Play the hurt sound effect.
        enemyAudio.Play();

        // Reduce the current health by the amount of damage sustained.
        currentHealth -= amount;

        // If the current health is less than or equal to zero
        if (currentHealth <= 0)
        {
            // the enemy is dead.
            Death();
        }
    }


    void Death()
    {
        // The enemy is dead.
        isDead = true;

        // Turn the collider into a trigger so shots can pass throogh it.
        capsuleCollider.isTrigger = true;

        // Tell the animator that the enemy is dead.
        anime.SetTrigger("Dead");

        // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
        enemyAudio.clip = deathClip;
        enemyAudio.Play();
    }


    public void StartSinking()
    {
        // Find and disable the Nav Mesh Agent.
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

        // Find the rigidbody component and make it kinematic (since we use translate to sink the enemy).
        GetComponent<Rigidbody>().isKinematic = true;

        // The enemy should sink
        isSinking = true;

        // After 2 seconds destroy the enemy.
        Destroy(gameObject, 2f);
    }
}
