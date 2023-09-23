using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumController : MonoBehaviour
{
    public GameObject album; 
    public GameObject[] pages; 
    public GameObject btnBack; 
    public GameObject btnNext;

    private int currentPageIndex = 0;
    public  List<GameObject> photographedObjectsList = new List<GameObject>();

    void Start()
    {
        pages[currentPageIndex].SetActive(true);
        btnBack.SetActive(false);
        LoadPhotographedObjects();
    }
   
    //TO CHANGE PAGES
    public void ChangePage(int offset)
    {
     
        pages[currentPageIndex].SetActive(false);

     
        currentPageIndex += offset;

      
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, pages.Length - 1);

      
        pages[currentPageIndex].SetActive(true);

   
        btnBack.SetActive(currentPageIndex > 0);
        btnNext.SetActive(currentPageIndex < pages.Length - 1);
    }

    //TO GET PHOTOGRAPHED DATA
    public void LoadPhotographedObjects()
    {
        if (PlayerPrefs.HasKey("PhotographedObjects"))
        {
            string json = PlayerPrefs.GetString("PhotographedObjects");
            photographedObjectsList = JsonUtility.FromJson<List<GameObject>>(json);
            Debug.Log(photographedObjectsList);
        }
    }


    //TO CLOSE ALBUM
    public void closeAlbum()
    {
        album.SetActive(false);
    }
}
