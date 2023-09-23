using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateCells : MonoBehaviour
{
    public static Action finishedPopulatingCells;

    public GridGenerator gridGeneratorScript;

    public GameObject[] treesPrefabs;
    public GameObject[] bushesPrefabs;
    public GameObject[] flowersAndWeedPrefabs;
    public GameObject[] rocksAndTrunksPrefabs;
    public GameObject[] fungusPrefabs;
    public GameObject[] emptyCellTrees;
    public float xSpawnOffsetRange = 0.250f; // Range of random offset in the X-axis

    // Losetas para instanciar decoraciones
    List<GameObject> pathElements;
    List<GameObject> tier1Elements;
    List<GameObject> tier2Elements;
    List<GameObject> tier3Elements;
    List<GameObject> tier4Elements;
    List<GameObject> everythingElse;
    
    // Losetas para instanciar criaturas despues T1
    public List<GameObject> insectIdleObjectsT1 = new List<GameObject>();
    public List<GameObject> smallIdleObjectsT1 = new List<GameObject>();
    public List<GameObject> insectVerticalObjectsT1 = new List<GameObject>();
    public List<GameObject> flyingIdleObjectsT1 = new List<GameObject>();
    public List<GameObject> backSpotObjectsT1 = new List<GameObject>();

    // Losetas para instanciar criaturas despues T2
    public List<GameObject> insectIdleObjectsT2 = new List<GameObject>();
    public List<GameObject> smallIdleObjectsT2 = new List<GameObject>();
    public List<GameObject> insectVerticalObjectsT2 = new List<GameObject>();
    public List<GameObject> flyingIdleObjectsT2 = new List<GameObject>();
    public List<GameObject> backSpotObjectsT2 = new List<GameObject>();

    // Losetas para instanciar criaturas despues T3
    public List<GameObject> insectIdleObjectsT3 = new List<GameObject>();
    public List<GameObject> smallIdleObjectsT3 = new List<GameObject>();
    public List<GameObject> insectVerticalObjectsT3 = new List<GameObject>();
    public List<GameObject> flyingIdleObjectsT3 = new List<GameObject>();
    public List<GameObject> backSpotObjectsT3 = new List<GameObject>();

    // Losetas para instanciar criaturas despues T4
    public List<GameObject> insectIdleObjectsT4 = new List<GameObject>();
    public List<GameObject> smallIdleObjectsT4 = new List<GameObject>();
    public List<GameObject> insectVerticalObjectsT4 = new List<GameObject>();
    public List<GameObject> flyingIdleObjectsT4 = new List<GameObject>();
    public List<GameObject> backSpotObjectsT4 = new List<GameObject>();

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
            everythingElse = gridGeneratorScript.everythingElse;
        }

        // Loop through each GameObject in the array
        foreach (GameObject cell in tier1Elements)
        {
            populateCell(cell, "T1");
        }
        foreach (GameObject cell in tier2Elements)
        {
            populateCell(cell, "T2");
        }
        foreach (GameObject cell in tier3Elements)
        {
            populateCell(cell, "T3");
        }
        foreach (GameObject cell in tier4Elements)
        {
            populateCell(cell, "T4");
        }
        foreach (GameObject cell in everythingElse)
        {
            populateEmptyCells(cell);
        }

        finishedPopulatingCells?.Invoke();
    }

    void populateCell(GameObject cell, string tier)
    {
        // ------------------ Main Object -----------------------
        Transform childTransform = cell.transform.Find("MainObject");

        if (childTransform != null)
        {
            Vector3 offSet = getRandomOffset();
            Vector3 spawnPosition = childTransform.position + offSet;
            GameObject prefab = getRandomPrefabFor("MainObject", tier);
            if (prefab != null)
            {
                // Instantiate the prefab and make it a child of the specified child object
                GameObject newObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
                newObj.transform.parent = childTransform;
                assingSpotsToTiersArray(newObj, tier);
            }
        }
        else
        {
            Debug.LogWarning("Child object not found in " + cell.name);
        }

        // ------------------ Front 1 -----------------------
        Transform childTransform1 = cell.transform.Find("FrontObject1");

        if (childTransform1 != null)
        {
            GameObject prefab = getRandomPrefabFor("Front", tier);
            if (prefab != null)
            {
                // Instantiate the prefab and make it a child of the specified child object
                GameObject newObj = Instantiate(prefab, childTransform1);
                newObj.transform.parent = childTransform1;
                assingSpotsToTiersArray(newObj, tier);
            }
            
        }
        else
        {
            Debug.LogWarning("Child object not found in " + cell.name);
        }

        // ------------------ Front 2 -----------------------
        Transform childTransform2 = cell.transform.Find("FrontObject2");

        if (childTransform2 != null)
        {
            GameObject prefab = getRandomPrefabFor("Front", tier);
            if (prefab != null)
            {
                // Instantiate the prefab and make it a child of the specified child object
                GameObject newObj = Instantiate(prefab, childTransform2);
                newObj.transform.parent = childTransform2;
                assingSpotsToTiersArray(newObj, tier);
            }
        }
        else
        {
            Debug.LogWarning("Child object not found in " + cell.name);
        }

        // ------------------ BackObject 1-----------------------
        Transform backObject = cell.transform.Find("BackObject1");

        if (backObject != null)
        {
            GameObject prefab = getRandomPrefabFor("Back", tier);
            if (prefab != null)
            {
                GameObject newObj = Instantiate(prefab, backObject);
                newObj.transform.parent = backObject;
                assingSpotsToTiersArray(newObj, tier);
            } else {
                switch (tier)
                {
                    case "T1":
                        backSpotObjectsT1.Add(backObject.gameObject);
                        break;
                    case "T2":
                        backSpotObjectsT2.Add(backObject.gameObject);
                        break;
                    case "T3":
                        backSpotObjectsT3.Add(backObject.gameObject);
                        break;
                    case "T4":
                        backSpotObjectsT4.Add(backObject.gameObject);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            Debug.LogWarning("Child object not found in " + cell.name);
        }

        // ------------------ BackObject 2-----------------------
        Transform backObject2 = cell.transform.Find("BackObject2");

        if (backObject2 != null)
        {
            GameObject prefab = getRandomPrefabFor("Back", tier);
            if (prefab != null)
            {
                GameObject newObj = Instantiate(prefab, backObject2);
                newObj.transform.parent = backObject2;
                assingSpotsToTiersArray(newObj, tier);
            } else {
                switch (tier)
                {
                    case "T1":
                        backSpotObjectsT1.Add(backObject2.gameObject);
                        break;
                    case "T2":
                        backSpotObjectsT2.Add(backObject2.gameObject);
                        break;
                    case "T3":
                        backSpotObjectsT3.Add(backObject2.gameObject);
                        break;
                    case "T4":
                        backSpotObjectsT4.Add(backObject2.gameObject);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            Debug.LogWarning("Child object not found in " + cell.name);
        }
    }

    void populateEmptyCells(GameObject cell)
    {
        // ------------------ Main Object -----------------------
        Transform childTransform = cell.transform.Find("MainObject");

        if (childTransform != null)
        {
            Vector3 offSet = getRandomOffset();
            Vector3 spawnPosition = childTransform.position + offSet;
            int randomChance = UnityEngine.Random.Range(1, 100);
            if (randomChance <= 20)
            {
                GameObject prefab = GetRandomObjectFromArray(emptyCellTrees);
                if (prefab != null)
                {
                    // Instantiate the prefab and make it a child of the specified child object
                    GameObject newObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
                    newObj.transform.parent = childTransform;
                    newObj.transform.LookAt(Camera.main.transform.position);
                }
            }
        }
        else
        {
            Debug.LogWarning("Child object not found in " + cell.name);
        }
    }

    GameObject getRandomPrefabFor(string position, string tier)
    {
        switch (position)
                {
                    // Tree, Bush, Flower, Rock, Fungus
                    case "MainObject":
                        switch (tier)
                        {
                            case "T1":
                                return GetRandomObjectByProbability(0.30f,0.65f,0,0.05f,0);
                            case "T2":
                                return GetRandomObjectByProbability(0.50f,0.45f,0,0.05f,0);
                            case "T3":
                                return GetRandomObjectByProbability(0.50f,0.50f,0,0,0);
                            case "T4":
                                return GetRandomObjectByProbability(1f,0,0,0,0);
                            default:
                                return null;
                        }
                    case "Front":
                        switch (tier)
                        {
                            case "T1":
                                return GetRandomObjectByProbability(0,0.20f,0.74f,0.05f,0.01f);
                            case "T2":
                                return GetRandomObjectByProbability(0f,0.25f,0.54f,0.19f,0.02f);
                            case "T3":
                                return GetRandomObjectByProbability(0f,0.40f,0.40f,0.20f,0);
                            case "T4":
                                return GetRandomObjectByProbability(0.20f,0.30f,0.10f,0.30f,0);
                            default:
                                return null;
                        }
                    case "Back":
                        switch (tier)
                        {
                            case "T1":
                                return GetRandomObjectByProbability(0,0.20f,0.40f,0.30f,0.02f);
                            case "T2":
                                return GetRandomObjectByProbability(0.10f,0.15f,0.15f,0.15f,0.02f);
                            case "T3":
                                return GetRandomObjectByProbability(0.10f,0.10f,0f,0.20f,0);
                            case "T4":
                                return GetRandomObjectByProbability(0.50f,0,0f,0,0);
                            default:
                                return null;
                        }
                    default:
                        return null;
                }
    }
    private GameObject GetRandomObjectByProbability(float treeProb, float bushesProb, float flowerWeedProb, float rockTrunksProb, float fungusProb)
    {
        float randomValue = UnityEngine.Random.value;
        float cumulativeProbability = 0.0f;

        // Define the probabilities for each array
        float treesProbability = treeProb; // 20%
        float bushesProbability = bushesProb; // 5%
        float flowersAndWeedProbability = flowerWeedProb; // 50%
        float rocksAndTrunksProbability = rockTrunksProb; // 25%
        float fungusProbability = fungusProb;

        // Include fungusPrefabs with a chance
        if (randomValue < cumulativeProbability + fungusProbability)
        {
            return GetRandomObjectFromArray(fungusPrefabs);
        }
        cumulativeProbability += fungusProbability;
        // Check the random value against the cumulative probabilities
        if (randomValue < cumulativeProbability + treesProbability)
        {
            return GetRandomObjectFromArray(treesPrefabs);
        }
        cumulativeProbability += treesProbability;

        if (randomValue < cumulativeProbability + bushesProbability)
        {
            return GetRandomObjectFromArray(bushesPrefabs);
        }
        cumulativeProbability += bushesProbability;

        if (randomValue < cumulativeProbability + flowersAndWeedProbability)
        {
            return GetRandomObjectFromArray(flowersAndWeedPrefabs);
        }
        cumulativeProbability += flowersAndWeedProbability;

        
        // Include rocksAndTrunksPrefabs with a chance
        if (randomValue < cumulativeProbability + rocksAndTrunksProbability)
        {
            return GetRandomObjectFromArray(rocksAndTrunksPrefabs);
        }


        // If none of the above conditions are met, return null
        return null;
    }

    void assingSpotsToTiersArray(GameObject go, string tier)
    {
        // Find the "rotate" child
        Transform rotateChild = go.transform.Find("rotate");

        if (rotateChild != null)
        {
            // Iterate through the child objects and add them to the corresponding list
            foreach (Transform child in rotateChild)
            {
                string childName = child.name;
                
                switch (childName)
                {
                    case "insectIdle":
                        switch (tier)
                        {
                            case "T1":
                                insectIdleObjectsT1.Add(child.gameObject);
                                break;
                            case "T2":
                                insectIdleObjectsT2.Add(child.gameObject);
                                break;
                            case "T3":
                                insectIdleObjectsT3.Add(child.gameObject);
                                break;
                            case "T4":
                                insectIdleObjectsT4.Add(child.gameObject);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "smallIdle":
                        switch (tier)
                        {
                            case "T1":
                                smallIdleObjectsT1.Add(child.gameObject);
                                break;
                            case "T2":
                                smallIdleObjectsT2.Add(child.gameObject);
                                break;
                            case "T3":
                                smallIdleObjectsT3.Add(child.gameObject);
                                break;
                            case "T4":
                                smallIdleObjectsT4.Add(child.gameObject);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "insectVertical":
                        switch (tier)
                        {
                            case "T1":
                                insectVerticalObjectsT1.Add(child.gameObject);
                                break;
                            case "T2":
                                insectVerticalObjectsT2.Add(child.gameObject);
                                break;
                            case "T3":
                                insectVerticalObjectsT3.Add(child.gameObject);
                                break;
                            case "T4":
                                insectVerticalObjectsT4.Add(child.gameObject);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "flyingIdle":
                        switch (tier)
                        {
                            case "T1":
                                flyingIdleObjectsT1.Add(child.gameObject);
                                break;
                            case "T2":
                                flyingIdleObjectsT2.Add(child.gameObject);
                                break;
                            case "T3":
                                flyingIdleObjectsT3.Add(child.gameObject);
                                break;
                            case "T4":
                                flyingIdleObjectsT4.Add(child.gameObject);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private GameObject GetRandomObjectFromArray(GameObject[] array)
    {
        if (array.Length == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, array.Length);
        return array[randomIndex];
    }

    Vector3 getRandomOffset()
    {
        // Calculate random offsets within the specified ranges
        float randomXOffset = UnityEngine.Random.Range(-xSpawnOffsetRange, xSpawnOffsetRange);

        // Create a random offset Vector3
        return new Vector3(randomXOffset, 0f, 0f);
    }
}
