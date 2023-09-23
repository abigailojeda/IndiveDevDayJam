using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AnimalPhoto
{
    public string name;
    public bool photographed;

    public AnimalPhoto(string name)
    {
        this.name = name;
        this.photographed = false;
    }
}

[System.Serializable]
public class AnimalsData
{
    public List<AnimalPhoto> animales;
}


public class PlayerMovement : MonoBehaviour
{
 
    //public List<Animal> AnimalList = new List<Animal>();

    public GridGenerator gridGeneratorScript;
    List<GameObject> pathElements;
    public CameraScript cameraScript;
    public float moveDuration = 2.0f;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;

    private void OnEnable() {
        PopulateCells.finishedPopulatingCells += movePlayer;
        PopulateCells.finishedPopulatingCells += teleportPlayer;
    }
    private void OnDisable() {
        PopulateCells.finishedPopulatingCells -= movePlayer;
        PopulateCells.finishedPopulatingCells -= teleportPlayer;
    }

    void setPathArray()
    {
        if (gridGeneratorScript != null)
        {
            pathElements = gridGeneratorScript.pathElements;
        }
    }
    void teleportPlayer()
    {
        setPathArray();
        transform.position = pathElements[0].transform.position;
    }
    void movePlayer()
    {
        setPathArray();
        if (currentWaypointIndex < pathElements.Count)
        {
            GameObject nextWaypoint = pathElements[currentWaypointIndex];
            Transform childTransform = nextWaypoint.transform.Find("MainObject");
            if (childTransform != null)
            {
                Vector3 destination = childTransform.transform.position;
                // Ensure the player can't start another move while already moving
                if (!isMoving)
                {
                    isMoving = true;

                    // Move the player to the next waypoint
                    transform.DOMove(destination, moveDuration).SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            // When the move is complete, increment the waypoint index and move to the next waypoint
                            currentWaypointIndex++;
                            isMoving = false; // Allow the next move
                            movePlayer();
                        });
                }
            }
            else
            {
                Debug.LogWarning("Child object not found in " + nextWaypoint.name);
            }
        } else {
            endPhase();
        }
        
        
    }

    public void endPhase()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
      
        CameraScript cameraScript = FindObjectOfType<CameraScript>();
        var photographedObjectsList = cameraScript.GetPhotographedObjectsList();
        List<string> animalNames = new List<string>();

        Debug.Log(photographedObjectsList);
        foreach (GameObject animalGameObject in photographedObjectsList)
        {
            // Accede a los componentes o propiedades específicas del objeto GameObject
            // Por ejemplo, puedes obtener el nombre del animal así:
            string animalName = animalGameObject.name.Replace("(Clone)", "");
            animalNames.Add(animalName);
            // Luego, puedes hacer lo que necesites con esta información
            Debug.Log("Animal fotografiado: " + animalName);
        }

        updateAnimalsData(animalNames);
        SceneManager.LoadScene("Menu");

    }

    public void updateAnimalsData(List<string> animalNamesList)
    {
        Debug.Log("ANIMALITOS: "+animalNamesList);
        //GET DATA FROM PLAYERPREFS
        if (PlayerPrefs.HasKey("AnimalsData"))
        {
            string json = PlayerPrefs.GetString("AnimalsData");
            Debug.Log("AnimalsData JSON: " + json);
            AnimalsData data = JsonUtility.FromJson<AnimalsData>(json);
            foreach (AnimalPhoto animal in data.animales)
            {
                if (animalNamesList.Contains(animal.name))
                {
                    animal.photographed = true;
                }
            }
            Debug.Log("ANTES DE updated: " + data);
            string updatedJson = JsonUtility.ToJson(data);

            Debug.Log("updated: " + updatedJson);

            PlayerPrefs.SetString("AnimalsData", updatedJson);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("No se encontró la clave 'AnimalsData' en PlayerPrefs.");
        }
    }
}

