
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhotoCapture : MonoBehaviour
{
 
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame; 
    
    [Header("Photo Feader Effect")]
    [SerializeField] private Animator fadingAnimation;

    private Texture2D screenCapture;
    private bool viewingPhoto;
    private bool isTarget;
    private bool CameraIsActive;

    private PlayerControler playercontroller;

    public GameObject cameraWorks;
    public GameObject cameraBroken;

    void Awake()
    {
        /* playercontroller = FindObjectOfType<PlayerControler>();
        if (playercontroller == null)
        {
            Debug.LogError("No se encontr� el script PlayerControler.");
        } */
    }

    private void Start()
    {
   
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
             isTarget = playercontroller.ChecklIsTarget();
            

            Debug.Log("es???" + isTarget);
            if (isTarget)
            {
                cameraWorks.SetActive(true);
                CameraIsActive = true;



            }
            else
            {
                cameraBroken.SetActive(true);
                StartCoroutine(DisableCameraBroken());
                
            }
          
        }

        if (Input.GetMouseButtonDown(0) && !viewingPhoto && isTarget && CameraIsActive)
        {
            Debug.Log("aquiiii");
            cameraWorks.SetActive(false);
            CameraIsActive = false;


            StartCoroutine(CapturePhoto());
        }
    }


    IEnumerator CapturePhoto()
    {

        viewingPhoto = true;
        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        ShowPhoto();

        yield return new WaitForSeconds(2f);
        RemovePhoto();
    }

    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100f);
        photoDisplayArea.sprite = photoSprite;

        photoFrame.SetActive(true);
        fadingAnimation.Play("PhotoFade");
    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);
    }

    IEnumerator DisableCameraBroken()
    {
        yield return new WaitForSeconds(1f);
        cameraBroken.SetActive(false); 
    }
}
