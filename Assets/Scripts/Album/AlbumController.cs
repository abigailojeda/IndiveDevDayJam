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

    void Start()
    {
        
        pages[currentPageIndex].SetActive(true);
        btnBack.SetActive(false); 
    }
    public void test()
    {
        Debug.Log("tests");
    }
    public void ChangePage(int offset)
    {
        Debug.Log("probando: "+offset);
        pages[currentPageIndex].SetActive(false);

     
        currentPageIndex += offset;

      
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, pages.Length - 1);

      
        pages[currentPageIndex].SetActive(true);

   
        btnBack.SetActive(currentPageIndex > 0);
        btnNext.SetActive(currentPageIndex < pages.Length - 1);
    }

    public void closeAlbum()
    {
        album.SetActive(false);
    }
}
