using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyLightSwitch : MonoBehaviour
{
public Light lightSource; // Reference to the light source
    public float minOnDuration = 1f; // Minimum duration in seconds when the light is on
    public float maxOnDuration = 4f; // Maximum duration in seconds when the light is on
    public float minOffDuration = 1f; // Minimum duration in seconds when the light is off
    public float maxOffDuration = 4f; // Maximum duration in seconds when the light is off

    private float timer = 0f;
    private bool isLightOn = false;
    private float nextDuration = 0f;

    private void Start()
    {
        // Initially, turn off the light
        lightSource.enabled = false;

        // Set the initial duration for the off state
        nextDuration = Random.Range(minOffDuration, maxOffDuration);
    }

    private void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to toggle the light
        if (isLightOn && timer >= nextDuration)
        {
            // Turn off the light
            lightSource.enabled = false;
            isLightOn = false;

            // Set the duration for the next off state
            nextDuration = Random.Range(minOffDuration, maxOffDuration);
            timer = 0f; // Reset the timer
        }
        else if (!isLightOn && timer >= nextDuration)
        {
            // Turn on the light
            lightSource.enabled = true;
            isLightOn = true;

            // Set the duration for the next on state
            nextDuration = Random.Range(minOnDuration, maxOnDuration);
            timer = 0f; // Reset the timer
        }
    }
}
