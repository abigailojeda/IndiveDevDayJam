using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSpriteOnSpawn : MonoBehaviour
{
    public SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        // Generate a random float value between 0 (inclusive) and 1 (exclusive)
        float randomValue = Random.Range(0f, 1f);

        // Set a threshold (0.5f) for a 50/50 chance
        float threshold = 0.5f;

        // Check if the random value is less than the threshold
        if (randomValue < threshold)
        {
            sprite.flipX = true;
        }
    }
}
