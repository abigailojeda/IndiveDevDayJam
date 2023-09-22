using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CameraScript : MonoBehaviour
{
    public static Action<GameObject> photographedObject;
    
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;

    [Header("Photo Feader Effect")]
    [SerializeField] private Animator fadingAnimation;

    public GameObject cameraUsable;
    public GameObject cameraUnusable;

    private bool viewingPhoto;
    
    List<GameObject> validTargets = new List<GameObject>();
    private Dictionary<GameObject, bool> anyValidHitDic = new Dictionary<GameObject, bool>();
    public GameObject[] raycasterGrid;
    public int rayDistance = 100; // Adjust the distance as needed
    public GameObject visualTestObj;
    public Vector3 boxSize = new Vector3(3f, 3f, 3f);
    bool aiming = false;
    public bool canTakePhoto = false;
    public float photoCd = 4.0f;
    private Texture2D screenCapture;

    [Header("Debug Options")]
    [SerializeField] private bool FIREMYLASER = false;
    [SerializeField] private bool drawLine = false;


    private void Start() {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        foreach (var caster in raycasterGrid)
        {
            anyValidHitDic[caster] = false;
        }
    }

    private void Update() {
        if (!checkIfAnyValidHit())
        {
            validTargets.Clear();
        }
        if (Input.GetKey(KeyCode.E) && !viewingPhoto)
        {
            aiming = true;
            if (!viewingPhoto && validTargets.Count > 0)
            {
                cameraUsable.SetActive(true);
                cameraUnusable.SetActive(false);
            } else {
                cameraUsable.SetActive(false);
                cameraUnusable.SetActive(true);
            }
        } else {
            aiming = false;
            cameraUsable.SetActive(false);
            cameraUnusable.SetActive(false);
        }

        if (aiming)
        {
            foreach (var caster in raycasterGrid)
            {
                // Get the GameObject's forward direction
                Vector3 forwardDirection = caster.transform.forward;
                // Calculate the destination position at a certain distance in the forward direction
                Vector3 destination = caster.transform.position + forwardDirection * rayDistance;
                makeBoxCast(caster, destination);
                visualizeCast(caster, destination);
            }
        } else {
            validTargets.Clear();
        }

        if (Input.GetMouseButtonDown(0) && aiming && !viewingPhoto && validTargets.Count > 0)
        {
            cameraUsable.SetActive(false);
            StartCoroutine(CapturePhoto());
            foreach (var vTarget in validTargets)
            {
                photographedObject?.Invoke(vTarget);
            }
        }
    }

    bool checkIfAnyValidHit()
    {
        bool anyHit = false;
        foreach (KeyValuePair<GameObject, bool> entry in anyValidHitDic)
        {
            if (entry.Value == true) {
                anyHit = true;
            }
        }
        return anyHit;
    }

    void makeBoxCast(GameObject caster, Vector3 destination)
    {
        // Perform the boxcast
        RaycastHit hit;
        if (Physics.BoxCast(caster.transform.position, boxSize / 2f, caster.transform.forward, out hit, Quaternion.identity))
        {
            // If the boxcast hits something, you can handle the hit object here
            GameObject collidedObj = hit.collider.gameObject.transform.parent.gameObject;
            Debug.Log(collidedObj.name);
            if (collidedObj.tag == "target")
            {
                anyValidHitDic[caster] = true;
                if (!validTargets.Contains(collidedObj))
                {
                    validTargets.Add(collidedObj);
                }
            } else {
                anyValidHitDic[caster] = false;
            }
        } else {
            anyValidHitDic[caster] = false;
        }
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

    private IEnumerator photoTakenCoroutine()
    {
        canTakePhoto = false;
        yield return new WaitForSeconds(photoCd);
        canTakePhoto = true;
    }

    void visualizeCast(GameObject caster, Vector3 destination)
    {
        if (visualTestObj != null && FIREMYLASER)
            {
                GameObject tempObj = Instantiate(visualTestObj, caster.transform.position, Quaternion.identity);
                tempObj.transform.DOMove(destination, 1, false).SetEase(Ease.Linear).OnComplete(() => { Destroy(tempObj); });
            }
        if (drawLine) Debug.DrawLine(caster.transform.position, destination, Color.red, 30.0f);
    }
}
