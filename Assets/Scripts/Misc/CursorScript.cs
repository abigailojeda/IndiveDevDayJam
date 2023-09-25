using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    [SerializeField] Camera cursorCamera;
    [SerializeField] GameObject cursorImage;


    void Update()
    {
        
        Vector2 cursorPos = cursorCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(cursorPos.x, cursorPos.y, 1f);

        if (Input.GetMouseButton(0))
        {
            // Simulate click animation for 1 frame
            transform.localScale = new Vector3(1.1f, 1.1f, 1);
        }else {
            transform.localScale = new Vector3(1f, 1f, 1);
        } 
    }

    private void Start() {
        Cursor.visible = false;
    }

    public void showCursor() {
        Cursor.visible = false;
        cursorImage.SetActive(true);
    }
    public void hideCursor() {
        Cursor.visible = false;
        cursorImage.SetActive(false);
    }
}
