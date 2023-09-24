using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    public AlbumController albumcontrollerScript;
    public GameObject album, pauseMenu;
    public Image buttonCameraE;
    public Image buttonCameraClick;
    public Image buttonAlbum;
    public TextMeshProUGUI amountText;
    bool isPaused = false;
    bool viewingAlbum = false;

    public Slider _musicSlider;
    public Slider _sfxSlider;
    public Slider _ambienceSlider;

    public void MusicVolume() { AudioManager.Instance.MusicVolume(_musicSlider.value); }
    public void sfxVolume() { AudioManager.Instance.SfxVolume(_sfxSlider.value); }
    public void ambienceVolue() { AudioManager.Instance.SfxVolume(_ambienceSlider.value); }

    private void OnEnable() {
        AlbumController.amountCaptured += updateAmount;
    }
    private void OnDisable() {
        AlbumController.amountCaptured -= updateAmount;
    }


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

        if(!isPaused && !viewingAlbum)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("apreto E");
                buttonCameraE.enabled = false;
                buttonCameraClick.enabled = true;
            } else {
                buttonCameraE.enabled = true;
                buttonCameraClick.enabled = false;
            }
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !isPaused && !viewingAlbum){
            pauseGame();
        } else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && isPaused){
            unPauseGame();
        }
    }
    public void pauseGame(){
        Time.timeScale = 0;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        AudioManager.Instance.PlayExtraCameraSFX("QuitCamera");
        buttonCameraE.enabled = false;
        buttonCameraClick.enabled = false;
        buttonAlbum.enabled = false;
        amountText.gameObject.SetActive(false);
    }

    public void unPauseGame(){
        Time.timeScale = 1;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        AudioManager.Instance.PlayExtraCameraSFX("QuitCamera");

        buttonCameraE.enabled = true;
        buttonCameraClick.enabled = false;
        buttonAlbum.enabled = true;
        amountText.gameObject.SetActive(true);
    }
    public void ShowAlbum()
    {
        Time.timeScale = 0;
        viewingAlbum = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        album.SetActive(true);
        AudioManager.Instance.PlayRandomAlbumPickup();

        buttonCameraE.enabled = false;
        buttonCameraClick.enabled = false;
        buttonAlbum.enabled = false;
        amountText.gameObject.SetActive(false);
    } 
    
    public void HideAlbum()
    {
        Time.timeScale = 1;
        viewingAlbum = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        album.SetActive(false);

        buttonCameraE.enabled = true;
        buttonCameraClick.enabled = false;
        buttonAlbum.enabled = true;
        amountText.gameObject.SetActive(true);
    }

    public void returnToMainMenu(){
        Time.timeScale = 1;
        isPaused = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(false);
        AudioManager.Instance.PlayExtraCameraSFX("QuitCamera");
        AudioManager.Instance.PlayMusic("MenuTheme");
        AudioManager.Instance.PlayAmbience("AmbientForestDay");
        SceneManager.LoadScene("Menu");
    }

    public void updateAmount(int amount)
    {
        if (amountText != null){
            amountText.text = amount.ToString() + "/24";
        }
    }
}
