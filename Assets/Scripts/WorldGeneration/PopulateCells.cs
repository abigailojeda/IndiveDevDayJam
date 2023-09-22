using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateCells : MonoBehaviour
{
    public static Action finishedPopulatingCells;

    public GridGenerator gridGeneratorScript;

    public GameObject[] treePrefabTest;

    public float xSpawnOffsetRange = 0.250f; // Range of random offset in the X-axis

    List<GameObject> pathElements;
    List<GameObject> tier1Elements;
    List<GameObject> tier2Elements;
    List<GameObject> tier3Elements;
    List<GameObject> tier4Elements;

    private void OnEnable() {
        GridGenerator.finishedGeneratingGrid += PopulateTiers;
    }

    private void OnDisable() {
        GridGenerator.finishedGeneratingGrid -= PopulateTiers;
    }

    void PopulateTiers()
    {   
        if (gridGeneratorScript != null)
        {
            pathElements = gridGeneratorScript.pathElements;
            tier1Elements = gridGeneratorScript.tier1Elements;
            tier2Elements = gridGeneratorScript.tier2Elements;
            tier3Elements = gridGeneratorScript.tier3Elements;
            tier4Elements = gridGeneratorScript.tier4Elements;
        }

        // Loop through each GameObject in the array
        foreach (GameObject cell in tier1Elements)
        {
            populateMain(cell);
            populateFront(cell);
        }
        foreach (GameObject cell in tier2Elements)
        {
            populateMain(cell);
            populateFront(cell);
        }
        foreach (GameObject cell in tier3Elements)
        {
            populateMain(cell);
            populateFront(cell);
        }
        foreach (GameObject cell in tier4Elements)
        {
            populateMain(cell);
            populateFront(cell);
        }

        finishedPopulatingCells?.Invoke();
    }

    void populateMain(GameObject cell)
    {
            // Find the specified child object by name
            Transform childTransform = cell.transform.Find("MainObject");

            if (childTransform != null)
            {
                Vector3 offSet = getRandomOffset();
                Vector3 spawnPosition = childTransform.position + offSet;
                GameObject prefab = treePrefabTest[UnityEngine.Random.Range(0, treePrefabTest.Length)];
                // Instantiate the prefab and make it a child of the specified child object
                GameObject newObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
                newObj.transform.parent = childTransform;
            }
            else
            {
                Debug.LogWarning("Child object not found in " + cell.name);
            }
    }
    void populateFront(GameObject cell)
    {
        // Find the specified child object by name
            Transform childTransform1 = cell.transform.Find("FrontObject1");

            if (childTransform1 != null)
            {
                GameObject prefab = treePrefabTest[UnityEngine.Random.Range(0, treePrefabTest.Length)];
                // Instantiate the prefab and make it a child of the specified child object
                Instantiate(prefab, childTransform1);
            }
            else
            {
                Debug.LogWarning("Child object not found in " + cell.name);
            }
            // Find the specified child object by name
            Transform childTransform2 = cell.transform.Find("FrontObject2");

            if (childTransform2 != null)
            {
                GameObject prefab = treePrefabTest[UnityEngine.Random.Range(0, treePrefabTest.Length)];
                // Instantiate the prefab and make it a child of the specified child object
                Instantiate(prefab, childTransform2);
            }
            else
            {
                Debug.LogWarning("Child object not found in " + cell.name);
            }
            Transform backObject = cell.transform.Find("BackObject");

            if (backObject != null)
            {
                GameObject prefab = treePrefabTest[UnityEngine.Random.Range(0, treePrefabTest.Length)];
                // Instantiate the prefab and make it a child of the specified child object
                Instantiate(prefab, backObject);
            }
            else
            {
                Debug.LogWarning("Child object not found in " + cell.name);
            }
    }

    Vector3 getRandomOffset()
    {
        // Calculate random offsets within the specified ranges
        float randomXOffset = UnityEngine.Random.Range(-xSpawnOffsetRange, xSpawnOffsetRange);

        // Create a random offset Vector3
        return new Vector3(randomXOffset, 0f, 0f);
    }
}
