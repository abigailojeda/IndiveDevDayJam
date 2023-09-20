
using UnityEngine;
using TMPro;

[RequireComponent(typeof(InputController))]

public class PlayerControler : MonoBehaviour
{
    [SerializeField] float speed = 0f;
    [SerializeField] float rotationSpeed = 0f;
   

    private new Transform camera;
    public float rayDistance;


    InputController inputController = null;
  

    private void Awake()
    {
        inputController = GetComponent<InputController>();
        GameObject cameraObject = GameObject.FindWithTag("MainCamera"); 

        if (cameraObject != null)
        {
            camera = cameraObject.transform; 
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'MainCamera'.");
        }

    

 
    }

    private void Update()
    {
        Move();
     

    
      }

    void Move()
    {
        Vector2 input = inputController.MoveInput();

        transform.position += transform.forward * input.y * speed * Time.deltaTime;
     
    }

    //DETECT TARGET TO PHOTO

    public bool ChecklIsTarget()
    {

        Debug.DrawRay(camera.position, camera.forward * rayDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, rayDistance))
        {
            Debug.Log(hit.transform.tag);

            if (hit.transform.tag == "target")
            {

                Debug.Log("Es posible hacer foto");
                return true;
            }
            else
            {
                Debug.Log("NO foto");
                return false;
            }
        }
        return false;
    }
}
