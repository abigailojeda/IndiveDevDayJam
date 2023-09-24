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
     public TextMeshProUGUI endGameAmountText;
    public static bool isPaused = false;
    public static bool viewingAlbum = false;
    public static bool inEndGameScreen = false;

    public Slider _musicSlider;
    public Slider _sfxSlider;
    public Slider _ambienceSlider;

    public void MusicVolume() { AudioManager.Instance.MusicVolume(_musicSlider.value); }
    public void sfxVolume() { AudioManager.Instance.SfxVolume(_sfxSlider.value); }
    public void ambienceVolue() { AudioManager.Instance.AmbienceVolume(_ambienceSlider.value); }

    private void OnEnable() {
        AlbumController.amountCaptured += updateAmount;
    }
    private void OnDisable() {
        AlbumController.amountCaptured -= updateAmount;
    }

    private void Start() {
        _musicSlider.value = AudioManager.Instance.musicSource.volume;
        _sfxSlider.value = AudioManager.Instance.sfxSource.volume;
        _ambienceSlider.value = AudioManager.Instance.ambienceSource.volume;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            if (!isPaused && !inEndGameScreen && !viewingAlbum)
            {
                playerMovementScript.updateAlbum();
                albumcontrollerScript.LoadPhotographedObjects();
                ShowAlbum();
            }
        }

        if(!isPaused && !viewingAlbum && !inEndGameScreen)
        {
            if (Input.GetKey(KeyCode.E))
            {
                buttonCameraE.enabled = false;
                buttonCameraClick.enabled = true;
            } else {
                buttonCameraE.enabled = true;
                buttonCameraClick.enabled = false;
            }
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !isPaused && !viewingAlbum && !inEndGameScreen){
            pauseGame();
        } else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && isPaused){
            unPauseGame();
        }
    }
    public void pauseGame(){
        PlayerMovement.walking = false;
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
        PlayerMovement.walking = true;
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
        PlayerMovement.walking = false;
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
        PlayerMovement.walking = true;
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
        PlayerMovement.walking = false;
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
            endGameAmountText.text = amount.ToString() + "/24";
        }
    }
}
