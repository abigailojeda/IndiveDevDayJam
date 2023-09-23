using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumController : MonoBehaviour
{

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

    void Start()
    {
        Debug.Log("STARTTTTTT");
        InitializeMenuAnimals();

        pages[currentPageIndex].SetActive(true);
        btnBack.SetActive(false);
        //LoadPhotographedObjects();
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
        {

            // Si no existe, inicializa la lista de animales con la propiedad 'photographed' en false
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
            menuAnimals.Add(new Animal("chtulu"));
            menuAnimals.Add(new Animal("dragon"));
            menuAnimals.Add(new Animal("buho"));
            menuAnimals.Add(new Animal("stitch"));
            menuAnimals.Add(new Animal("lechuza"));
            menuAnimals.Add(new Animal("polilla"));

            // Crea una instancia de la clase AnimalsData y asigna la lista de animales
            AnimalsData data = new AnimalsData();
            data.animales = menuAnimals;

            Debug.Log("estas intentando guardar: " + data);

            // Convierte la instancia de AnimalsData en JSON y guárdala en PlayerPrefs
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("AnimalsData", json);
            PlayerPrefs.SetInt("Initialized", 1);
            PlayerPrefs.Save();
  
        } 

        LoadPhotographedObjects();

    }


    //TO GET PHOTOGRAPHED DATA
    //public void LoadPhotographedObjects()
    //{
    //    if (PlayerPrefs.HasKey("PhotographedObjects"))
    //    {

    //        string objectsString = PlayerPrefs.GetString("PhotographedObjects");

    //        // Separar el string en partes usando "|" como delimitador
    //        string[] objectNames = objectsString.Split('|');

    //        // Crear objetos GameObject a partir de los nombres
    //        foreach (string name in objectNames)
    //        {

    //            {
    //                Debug.Log("Objeto encontrado: " + name);
    //            }
    //        }
    //    }
    //}


    public void LoadPhotographedObjects()
    {
        Debug.Log("SI QUE ENTROOOOOO");
        if (PlayerPrefs.HasKey("AnimalsData"))
        {
            string json = PlayerPrefs.GetString("AnimalsData");
            Debug.Log("AnimalsData JSON: " + json);
            AnimalsData data = JsonUtility.FromJson<AnimalsData>(json);

            // Itera a través de los animales y activa los sprites correspondientes
            foreach (Animal animal in data.animales)
            {
                if (animal.photographed)
               {
                  ActivateAnimalSprite(animal.name);
               }
            }
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
        album.SetActive(false);
    }
}
