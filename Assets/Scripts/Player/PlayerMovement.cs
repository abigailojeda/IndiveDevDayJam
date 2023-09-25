using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AnimalPhoto
{
    public string name;
    public string description;
    public bool photographed;

    public AnimalPhoto(string name, string description)
    {
        this.name = name;
        this.description = description;
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

    public CursorScript cursorScript;
    public GridGenerator gridGeneratorScript;
    List<GameObject> pathElements;
    public CameraScript cameraScript;
    public float moveDuration = 2.0f;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    public GameObject endMenu;
    public BlackScreen blackScreenScript;
    public AudioSource footstepAudioSource;
    private int currentFoot = 0;
    public static bool walking = false;

    private void OnEnable() {
        /* PopulateCells.finishedPopulatingCells += movePlayer; */
        PopulateCells.finishedPopulatingCells += teleportPlayer;
    }
    private void OnDisable() {
        /* PopulateCells.finishedPopulatingCells -= movePlayer; */
        PopulateCells.finishedPopulatingCells -= teleportPlayer;
        StopCoroutine(PlayFootstepsRoutine());
    }
    private void OnDestroy() {
        StopCoroutine(PlayFootstepsRoutine());
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
        AudioManager.Instance.PlayMusic("GameTheme50");
        AudioManager.Instance.PlayAmbience("AmbientForestNight");
        blackScreenScript.FadeOut(1f);
        setPathArray();
        transform.position = pathElements[0].transform.position;
        movePlayer();
        // Start playing the first footstep sound immediately.
        PlayFootstepSound();
        StopCoroutine(PlayFootstepsRoutine());
        
        // Start a repeating coroutine to play footstep sounds every second.
        StartCoroutine(PlayFootstepsRoutine());
    }
    void movePlayer()
    {
        walking = true;
        /* setPathArray(); */
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

            CameraScript cameraScript = FindObjectOfType<CameraScript>();
            var photographedObjectsList = cameraScript.GetPhotographedObjectsList();
            List<string> animalNames = new List<string>();

            Debug.Log(photographedObjectsList);
            foreach (GameObject animalGameObject in photographedObjectsList)
            {

                string animalName = animalGameObject.name.Replace("(Clone)", "");
                animalNames.Add(animalName);

                Debug.Log("Animal fotografiado: " + animalName);
            }

            updateAnimalsData(animalNames);
            cursorScript.showCursor();
            /* Cursor.visible = true; */
            Cursor.lockState = CursorLockMode.None;
            //Time.timeScale = 1;

            endMenu.SetActive(true);
            walking = false;
            UIController.inEndGameScreen = true;
        }
    }

    private void PlayFootstepSound()
    {
        AudioClip s = (currentFoot == 0) ? AudioManager.Instance.getRightFootSound() : AudioManager.Instance.getLeftFootSound();
        footstepAudioSource.PlayOneShot(s);
        currentFoot = 1 - currentFoot; // Toggle between 0 (right) and 1 (left).
    }

    private IEnumerator PlayFootstepsRoutine()
    {
        while (walking)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("espero 1 segundos en teoria");
            PlayFootstepSound();
        }
    }

    public void endPhase()
    {
        walking = false;
        UIController.inEndGameScreen = false;
        AudioManager.Instance.PlayExtraCameraSFX("QuitCamera");
        AudioManager.Instance.PlayMusic("MenuTheme");
        AudioManager.Instance.PlayAmbience("AmbientForestDay");
        SceneManager.LoadScene("Menu");

    }

    public void updateAlbum()
    {


        CameraScript cameraScript = FindObjectOfType<CameraScript>();
        var photographedObjectsList = cameraScript.GetPhotographedObjectsList();
        List<string> animalNames = new List<string>();

        Debug.Log(photographedObjectsList);
        foreach (GameObject animalGameObject in photographedObjectsList)
        {

            string animalName = animalGameObject.name.Replace("(Clone)", "");
            animalNames.Add(animalName);

            //Debug.Log("Animal fotografiado: " + animalName);
        }

        updateAnimalsData(animalNames);

    }

    public void updateAnimalsData(List<string> animalNamesList)
    {
        //Debug.Log("ANIMALITOS: "+animalNamesList);
        //GET DATA FROM PLAYERPREFS
        if (PlayerPrefs.HasKey("AnimalsData"))
        {
            string json = PlayerPrefs.GetString("AnimalsData");
            //Debug.Log("AnimalsData JSON: " + json);
            AnimalsData data = JsonUtility.FromJson<AnimalsData>(json);
            foreach (AnimalPhoto animal in data.animales)
            {
                if (animalNamesList.Contains(animal.name))
                {
                    animal.photographed = true;
                }
            }
            string updatedJson = JsonUtility.ToJson(data);

            //Debug.Log("updated: " + updatedJson);

            PlayerPrefs.SetString("AnimalsData", updatedJson);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("No se encontr� la clave 'AnimalsData' en PlayerPrefs.");
        }
    }
}

