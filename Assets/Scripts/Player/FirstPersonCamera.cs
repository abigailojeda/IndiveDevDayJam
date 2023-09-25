using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform player;
    public float mouseSenitivity = 2f;
    float cameraVerticalRotation = 0f;
    public CursorScript cursorScript;

    /* bool lockedCursor = true; */
    // Start is called before the first frame update
    void Start()
    {
        cursorScript.hideCursor();
        /* Cursor.visible = false; */
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMoveCamera())
        {
            float inputX = Input.GetAxis("Mouse X") * mouseSenitivity;
            float inputY = Input.GetAxis("Mouse Y") * mouseSenitivity;
            
            cameraVerticalRotation -= inputY;
            cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -45f, 50f);
            transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

            player.Rotate(Vector3.up * inputX);
        }
    }

    bool canMoveCamera(){
        return !UIController.isPaused && !UIController.viewingAlbum && !UIController.inEndGameScreen;
    }
}
