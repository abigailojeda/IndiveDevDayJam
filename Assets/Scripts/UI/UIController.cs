using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    public AlbumController albumcontrollerScript;
    public GameObject album;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            if (Time.timeScale < 1)
            {
               

            }
            else
            {
                playerMovementScript.updateAlbum();
                albumcontrollerScript.LoadPhotographedObjects();
                ShowAlbum();
            }
        }
    }

    public void ShowAlbum()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        album.SetActive(true);
    } 
    
    public void HideAlbum()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        album.SetActive(false);
    }



   
}
