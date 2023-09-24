using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumController : MonoBehaviour
{
    public static Action<int> amountCaptured;

    [System.Serializable]
    public class Animal
    {
        public string name;
        public bool photographed;

        public Animal(string name)
        {
            this.name = name;
            this.photographed = false;
        }
    }
   
    public List<Animal> menuAnimals = new List<Animal>();

    public GameObject album; 
    public GameObject[] pages; 
    public GameObject btnBack; 
    public GameObject btnNext;

    [Header("Photos to show or hide in album")]
    public List<GameObject> animalSprites;
    private int currentPageIndex = 0;
    //public  List<GameObject> photographedObjectsList = new List<GameObject>();

    private void OnEnable() {
        CameraScript.photographedObject += photoTaken;
    }
    private void OnDisable() {
        CameraScript.photographedObject -= photoTaken;
    }

    void Start()
    {
        /* Debug.Log("STARTTTTTT"); */
        InitializeMenuAnimals();

        pages[currentPageIndex].SetActive(true);
        btnBack.SetActive(false);
        //LoadPhotographedObjects();
    }
   
    //TO CHANGE PAGES
    public void ChangePage(int offset)
    {
     
        if(offset == 1)
        {
            AudioManager.Instance.PlayRandomAlbumArrowR();
        } else {
            AudioManager.Instance.PlayRandomAlbumArrowL();
        }
        pages[currentPageIndex].SetActive(false);

     
        currentPageIndex += offset;

      
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, pages.Length - 1);

        // Debug.Log("pagina: " + currentPageIndex);

        pages[currentPageIndex].SetActive(true);

   
        btnBack.SetActive(currentPageIndex > 0);
        btnNext.SetActive(currentPageIndex < pages.Length - 1);
    }

   //TO CREATE A JSON FOR ANIMALS DATA
    [System.Serializable]
    public class AnimalsData
    {
        public List<Animal> animales;
    }

    public void InitializeMenuAnimals()
    {
        // Verifica si ya existe un JSON en PlayerPrefs
        if (PlayerPrefs.GetInt("Initialized") != 1)
        //if (true) 
        {
            List<Animal> menuAnimals = new List<Animal>();
            //PAGE 1
            menuAnimals.Add(new Animal("ardilla"));
            menuAnimals.Add(new Animal("zorro"));
            menuAnimals.Add(new Animal("raton"));
            menuAnimals.Add(new Animal("mapache"));
            menuAnimals.Add(new Animal("luciernaga"));
            menuAnimals.Add(new Animal("mariposa"));
            menuAnimals.Add(new Animal("lobo"));
            menuAnimals.Add(new Animal("abeja"));

            //PAGE 2
            menuAnimals.Add(new Animal("fungus1"));
            menuAnimals.Add(new Animal("fungus2"));
            menuAnimals.Add(new Animal("fungus3"));
            menuAnimals.Add(new Animal("fungus4"));
            menuAnimals.Add(new Animal("fungus5"));
            menuAnimals.Add(new Animal("escarabajo1"));
            menuAnimals.Add(new Animal("escarabajo2"));
            menuAnimals.Add(new Animal("honguitorrinco"));

            //PAGE3
            menuAnimals.Add(new Animal("unicornio"));
            menuAnimals.Add(new Animal("chthulhu"));
            menuAnimals.Add(new Animal("dragon"));
            menuAnimals.Add(new Animal("buho"));
            menuAnimals.Add(new Animal("stitch"));
            menuAnimals.Add(new Animal("lechuza"));
            menuAnimals.Add(new Animal("polilla"));
            menuAnimals.Add(new Animal("murcielago"));

            AnimalsData data = new AnimalsData();
            data.animales = menuAnimals;

            //Debug.Log("estas intentando guardar: " + data);

            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("AnimalsData", json);
            PlayerPrefs.SetInt("Initialized", 1);
            PlayerPrefs.Save();

        }
        LoadPhotographedObjects();
    }


 

    public void LoadPhotographedObjects()
    {
        int counter = 0;
        if (PlayerPrefs.HasKey("AnimalsData"))
        {
            string json = PlayerPrefs.GetString("AnimalsData");
            //Debug.Log("AnimalsData JSON: " + json);
            AnimalsData data = JsonUtility.FromJson<AnimalsData>(json);

            // Itera a trav�s de los animales y activa los sprites correspondientes
            foreach (Animal animal in data.animales)
            {
                if (animal.photographed)
               {
                  ActivateAnimalSprite(animal.name);
                  counter++;
               }
            }
            amountCaptured?.Invoke(counter);
        }
    }

    public void photoTaken(GameObject captured){
        StartCoroutine(ExecuteDelayedFunction());
    }
    private IEnumerator ExecuteDelayedFunction()
    {
        InitializeMenuAnimals();
        yield return new WaitForSeconds(0.5f);
        int counter = 0;
        if (PlayerPrefs.HasKey("AnimalsData"))
        {
            string json = PlayerPrefs.GetString("AnimalsData");
            AnimalsData data = JsonUtility.FromJson<AnimalsData>(json);
            foreach (Animal animal in data.animales)
            {
                if (animal.photographed)
               {
                  counter++;

                }
            }
            amountCaptured?.Invoke(counter);
        }
    }

    private void ActivateAnimalSprite(string animalName)
    {
        foreach (GameObject spriteObj in animalSprites)
        {
            if (spriteObj.name == animalName)
            {
                spriteObj.SetActive(true);
                
            }
        }
    }

    //TO CLOSE ALBUM
    public void closeAlbum()
    {
        AudioManager.Instance.PlayRandomAlbumPickup();
        album.SetActive(false);
    }
}
