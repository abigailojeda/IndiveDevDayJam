using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateCreatures : MonoBehaviour
{
    public PopulateCells populateCellsScript;

    public GameObject[] insectsVerticalPrefabs;
    public GameObject[] smallIdlePrefabs;
    public GameObject[] flyingIdlePrefabs;
    public GameObject[] flyingInsecsPrefabs;
    public GameObject[] largeAnimalsPrefabs;
    

    // Losetas para instanciar criaturas despues T1
    public List<GameObject> smallIdleObjectsT1 = new List<GameObject>();
    public List<GameObject> insectVerticalObjectsT1 = new List<GameObject>();
    public List<GameObject> flyingIdleObjectsT1 = new List<GameObject>();
    public List<GameObject> backSpotObjectsT1 = new List<GameObject>();

    // Losetas para instanciar criaturas despues T2
    public List<GameObject> smallIdleObjectsT2 = new List<GameObject>();
    public List<GameObject> insectVerticalObjectsT2 = new List<GameObject>();
    public List<GameObject> flyingIdleObjectsT2 = new List<GameObject>();
    public List<GameObject> backSpotObjectsT2 = new List<GameObject>();

    // Losetas para instanciar criaturas despues T3
    public List<GameObject> smallIdleObjectsT3 = new List<GameObject>();
    public List<GameObject> insectVerticalObjectsT3 = new List<GameObject>();
    public List<GameObject> flyingIdleObjectsT3 = new List<GameObject>();
    public List<GameObject> backSpotObjectsT3 = new List<GameObject>();

    // Losetas para instanciar criaturas despues T4
    public List<GameObject> smallIdleObjectsT4 = new List<GameObject>();
    public List<GameObject> insectVerticalObjectsT4 = new List<GameObject>();
    public List<GameObject> flyingIdleObjectsT4 = new List<GameObject>();
    public List<GameObject> backSpotObjectsT4 = new List<GameObject>();

    private void OnEnable() {
        PopulateCells.finishedPopulatingCells += startPopulating;
    }
    private void OnDisable() {
        PopulateCells.finishedPopulatingCells -= startPopulating;
    }

    void startPopulating() {
        if (populateCellsScript != null) {
            smallIdleObjectsT1  = populateCellsScript.smallIdleObjectsT1;
            insectVerticalObjectsT1 = populateCellsScript.insectVerticalObjectsT1;
            flyingIdleObjectsT1 = populateCellsScript.flyingIdleObjectsT1;
            backSpotObjectsT1 = populateCellsScript.backSpotObjectsT1;

            smallIdleObjectsT2 = populateCellsScript.smallIdleObjectsT2;
            insectVerticalObjectsT2 = populateCellsScript.insectVerticalObjectsT2;
            flyingIdleObjectsT2 = populateCellsScript.flyingIdleObjectsT2;
            backSpotObjectsT2 = populateCellsScript.backSpotObjectsT2;

            smallIdleObjectsT3 = populateCellsScript.smallIdleObjectsT3;
            insectVerticalObjectsT3 = populateCellsScript.insectVerticalObjectsT3;
            flyingIdleObjectsT3 = populateCellsScript.flyingIdleObjectsT3;
            backSpotObjectsT3 = populateCellsScript.backSpotObjectsT3;

            smallIdleObjectsT4 = populateCellsScript.smallIdleObjectsT4;
            insectVerticalObjectsT4 = populateCellsScript.insectVerticalObjectsT4;
            flyingIdleObjectsT4 = populateCellsScript.flyingIdleObjectsT4;
            backSpotObjectsT4 = populateCellsScript.backSpotObjectsT4;

            populateSmall();
            populateLarge();
            populateVerticalInsects();
            populateFlyingInsects();
            populateFlyingIdle();
        }
    }

    void populateSmall()
    {
        ProcessListPorcentage(smallIdleObjectsT1, smallIdlePrefabs, 10);
        ProcessListPorcentage(smallIdleObjectsT2, smallIdlePrefabs, 15);
        ProcessListPorcentage(smallIdleObjectsT3, smallIdlePrefabs, 5);
        ProcessListPorcentage(smallIdleObjectsT4, smallIdlePrefabs, 1);
    }
    void populateLarge(){
        ProcessListPorcentage(backSpotObjectsT1, largeAnimalsPrefabs, 1);
        ProcessListPorcentage(backSpotObjectsT2, largeAnimalsPrefabs, 5);
        ProcessListPorcentage(backSpotObjectsT3, largeAnimalsPrefabs, 15);
        ProcessListPorcentage(backSpotObjectsT4, largeAnimalsPrefabs, 20);
    }
    void populateVerticalInsects(){
        ProcessListPorcentage(insectVerticalObjectsT1, insectsVerticalPrefabs, 10);
        ProcessListPorcentage(insectVerticalObjectsT2, insectsVerticalPrefabs, 15);
        ProcessListPorcentage(insectVerticalObjectsT3, insectsVerticalPrefabs, 15);
        ProcessListPorcentage(insectVerticalObjectsT4, insectsVerticalPrefabs, 0);
    }
    void populateFlyingInsects(){
        ProcessListPorcentage(flyingIdleObjectsT1, flyingInsecsPrefabs, 5);
        ProcessListPorcentage(flyingIdleObjectsT2, flyingInsecsPrefabs, 10);
        ProcessListPorcentage(flyingIdleObjectsT3, flyingInsecsPrefabs, 5);
        ProcessListPorcentage(flyingIdleObjectsT4, flyingInsecsPrefabs, 5);
    }
    void populateFlyingIdle(){
        ProcessListPorcentage(flyingIdleObjectsT1, flyingIdlePrefabs, 10);
        ProcessListPorcentage(flyingIdleObjectsT2, flyingIdlePrefabs, 15);
        ProcessListPorcentage(flyingIdleObjectsT3, flyingIdlePrefabs, 2);
        ProcessListPorcentage(flyingIdleObjectsT4, flyingIdlePrefabs, 1);
    }
    private void ProcessListPorcentage(List<GameObject> listTo, GameObject[] listFrom, int percentage)
    {
        int itemsToProcess = Mathf.CeilToInt(listTo.Count * percentage / 100f);

        List<int> indices = new List<int>(listTo.Count);
        for (int i = 0; i < listTo.Count; i++)
        {
            indices.Add(i);
        }

        for (int i = 0; i < itemsToProcess && indices.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, indices.Count);
            int selectedIndex = indices[randomIndex];
            indices.RemoveAt(randomIndex);

            // Perform your action on myList[selectedIndex]
            GameObject newGo = Instantiate(GetRandomObjectFromArray(listFrom), listTo[selectedIndex].transform);
            Billboard parentScript = listTo[selectedIndex].transform.GetComponent<Billboard>();
            SpriteRenderer newGoSr = newGo.GetComponentInChildren<SpriteRenderer>();
            if (parentScript != null && newGoSr)
            {
                parentScript.setSortingLayers(newGoSr);
            }
            if (listFrom == flyingInsecsPrefabs)
            {
                FlyingCreaturesIA newGoScript = newGo.GetComponent<FlyingCreaturesIA>();
                Transform endPatrol = listTo[Random.Range(0, listTo.Count-1)].transform;
                if (newGoScript != null && endPatrol != null)
                {
                    newGoScript.MoveObjectBetweenTransforms(listTo[selectedIndex].transform, endPatrol);
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
}
