using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuActions : MonoBehaviour
{
    public BlackScreen blackScreenScript;
    public GameObject album;
    [SerializeField] private CanvasGroup credits;
    [SerializeField] private GameObject creditsObject;

    [Header("Credits Animations")]
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Animator textAnimator;

    public float delayInSeconds = 2.5f;
    public void openAlbum()
    {
        AudioManager.Instance.PlayRandomAlbumPickup();
        album.SetActive(true);
    }

    //to hide menu and show credits
    public void hideCredits()
    {
        creditsObject.SetActive(false);
    } 
    
    public void showCredits()
    {
        AudioManager.Instance.PlayCreditCameraSFX("CreditsCamera2");
        creditsObject.SetActive(true);
        cameraAnimator.Play("Camera");
        textAnimator.Play("Text");
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayExtraCameraSFX("QuitCamera");
        AudioManager.Instance.PlayExtraCameraSFX("StartCamera");

        StartCoroutine(LoadSceneWithDelay("FaustoScene 1"));
    }


    public void QuitGame()
    {
        AudioManager.Instance.PlayExtraCameraSFX("QuitCamera");
        //GameObject.Find("SoundManager").GetComponent<SoundManager>().playAudio("button");
        Application.Quit();
    }

    public void GoToMenu()
    {
        //GameObject.Find("SoundManager").GetComponent<SoundManager>().playAudio("button");
        SceneManager.LoadScene("Menu");

    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(delayInSeconds);
        blackScreenScript.FadeIn(1f);
        yield return new WaitForSeconds(0.5f);
        // SceneManager.LoadScene(sceneName);
        LoadLevel(sceneName);
    }

    // ----------------------------------------------------
    // CODIGO PARA CARGAR LA SCENA ASYNCRONAMENTE
    // ----------------------------------------------------
    public void LoadLevel(string scene){
        // manMenu.SetActive(false);
        // loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync(scene));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            // loadingSlider.value = progressValue;
            yield return null;
        }
    }
    // ----------------------------------------------------
    // CODIGO PARA CARGAR LA SCENA ASYNCRONAMENTE
    // ----------------------------------------------------
}



