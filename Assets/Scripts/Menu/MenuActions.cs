using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuActions : MonoBehaviour
{
    public GameObject album;
    [SerializeField] private CanvasGroup credits;
    [SerializeField] private GameObject creditsObject;

    [Header("Credits Animations")]
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Animator textAnimator;

    public float delayInSeconds = 2.0f;
    public void openAlbum()
    {
        album.SetActive(true);
    }

    //to hide menu and show credits
    public void hideCredits()
    {
       
        creditsObject.SetActive(false);
    }  public void showCredits()
    {
        creditsObject.SetActive(true);
        cameraAnimator.Play("Camera");
        textAnimator.Play("Text");

    }

    public void StartGame()
    {
        StartCoroutine(LoadSceneWithDelay("FaustoScene 1"));
    }


    public void QuitGame()
    {
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

        SceneManager.LoadScene(sceneName);
    }
}



