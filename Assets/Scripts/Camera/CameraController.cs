
using UnityEngine;

[RequireComponent(typeof(InputController))]

public class CameraController : MonoBehaviour
{

    [SerializeField] float mouseSensivity = 0f;
    InputController inputController = null;

    private void Awake()
    {
        inputController = GetComponent<InputController>();
        Debug.Log(inputController);
    }

        void Update()
    {
        MouseCamera();
    }

    void MouseCamera()
    {
        Vector2 input = inputController.MoveInput();
        //Debug.Log(input);
        transform.Rotate(Vector3.up * input.x * mouseSensivity * Time.deltaTime);
    }
}
