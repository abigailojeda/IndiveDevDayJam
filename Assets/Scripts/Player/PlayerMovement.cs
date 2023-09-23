using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public GridGenerator gridGeneratorScript;
    List<GameObject> pathElements;

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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Menu");
        }
    }
}
