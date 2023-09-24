using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature3DAudioScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] sounds;
    public float minInterval = 4f; // Minimum time between sound plays
    public float maxInterval = 7f; // Maximum time between sound plays

    private void Start()
    {
        // Start the repeating method with a random delay between minInterval and maxInterval
        InvokeRepeating("PlayRandomSound", Random.Range(minInterval, maxInterval), Random.Range(minInterval, maxInterval));
        audioSource.volume = AudioManager.Instance.ambienceSource.volume;
    }

    private void OnDestroy() {
        CancelInvoke();
    }
    private void OnDisable() {
        CancelInvoke();
    }
    private void PlayRandomSound()
    {
        // Check if there are sounds in the array
        if (sounds.Length > 0)
        {
            // Select a random sound from the array
            int randomIndex = Random.Range(0, sounds.Length);
            AudioClip randomSound = sounds[randomIndex];

            // Play the selected sound on the AudioSource
            audioSource.clip = randomSound;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No sounds in the array.");
        }
    }
}
