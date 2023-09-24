using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlbumController : MonoBehaviour
{
    public static Action<int> amountCaptured;
    public int prueba;
    [Header("Hovers to show or hide in album")]
    public GameObject[] hoversToShowOnPage1;
    public GameObject[] hoversToShowOnPage2; 
    public GameObject[] hoversToShowOnPage3; 

    [Header("To Save descriptions")]
    public TextMeshProUGUI[] descriptionsTexts;


    [System.Serializable]
    public class Animal
    {
        public string name;
        public string description;
        public bool photographed;

        public Animal(string name, string description)
        {
            this.name = name;
            this.description = description;
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
        changeHovers();

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
        for (int i = 0; i < pages.Length; i++)
        {
            if (i != currentPageIndex)
            {
                //foreach (GameObject obj in hoversToShowOnPage[i])
                //{
                //    obj.SetActive(false);
                //}
            }
        }

        changeHovers();

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
            menuAnimals.Add(new Animal("ardilla", "Los dientes de las ardillas crecen constantemente durante sus vidas y por eso tienen que morderlo todo, como la cámara de los fotógrafos descuidados"));
            menuAnimals.Add(new Animal("zorro", "Los zorros son canidos con nariz puntiaguda, patas pequeñas y que cavan hoyos. Todo esto es bien sabidos, la única pregunta es ¿Que dice el zorro?."));
            menuAnimals.Add(new Animal("raton", "Los ratones tienen un muy buen sentido del oído… por si no era obvio. Algunas especies son capaces de usar su cola para agarrarse y balancearse en las ramas de los árboles."));
            menuAnimals.Add(new Animal("mapache", "Los panda rojos o también conocidos como “red pandas”, estan mas cerca de los mapaches que de los pandas. A diferencia de la creencia común, sus emociones no afectan su tamaño."));
            menuAnimals.Add(new Animal("luciernaga", "Las luciérnagas producen luz por tres razones : para atraer pareja, advertir a depredadores y avisar a otras luciérnagas de un peligro. ¿Por que razón habrá brillado las que hemos fotografiado?"));
            menuAnimals.Add(new Animal("mariposa", "Existen alrededor de 150.000 especies de mariposas, con diferentes formas, tamaños y colores. Parece que en este bosque solo habita una especie."));
            menuAnimals.Add(new Animal("lobo", "Inteligentes y poderosos depredadores en manada. Antepasados de los perros y con un aullido particular. Se dice que las lobas no están pa tipos como tu."));
            menuAnimals.Add(new Animal("abeja", "Sus dos ojos están formados por miles de ojos más pequeños llamados omatidios. Sabiendo esto, ¿Hemos visto una abeja o la abeja nos ha bzzto a nosotros?"));

            //PAGE 2
            menuAnimals.Add(new Animal("fungus1", "Su color es similar al azul del pájaro kokako, por ello los maoríes llaman a esta seta werewere-kokako, nombre realmente divertido de pronunciar. "));
            menuAnimals.Add(new Animal("fungus2", "Se vuelven más brillantes cuando se encuentran en temperaturas mayores a los 25º, pero lo realmente curioso es que en unas islas de Japón es conocida como “Pepe Verde”."));
            menuAnimals.Add(new Animal("fungus3", "También llamada Flor de Coco, puesto que la descubrieron en los restos de unas palmeras muertas. ¡Se dice que son las setas más luminosas de todas!"));
            menuAnimals.Add(new Animal("fungus4", "Estas setas aparte de ser venenosas y alucinógenas, se dice que son el hogar de hadas, duendes y otras criaturas mágicas. ¡Qué pena que no se vea ningún hada en nuestras fotos!"));
            menuAnimals.Add(new Animal("fungus5", "Estas setas son peludas y viscosas, pero lo más llamativo que tienen es que solo duran unas pocas horas antes de morir. ¡Qué suerte toparse con una!"));
            menuAnimals.Add(new Animal("escarabajo1", "No os dejéis intimidar por su enorme mandíbula, este escarabajo es super majo. Está realmente concienciado con cuidar el medio ambiente ,solo comen maderas en descomposición y no atacan a árboles sanos. "));
            menuAnimals.Add(new Animal("escarabajo2", "Este escarabajo es usado en los juegos de azar. Se colocan dos escarabajos machos en un tronco. Los dos escarabajos lucharán para echar al otro, el que se quede en el tronco es el ganador."));
            menuAnimals.Add(new Animal("honguitorrinco", "Hasta el dia de hoy los científicos no han logrado descifrar si el honguitorrinco es un ornitorrinco con setas o una seta que ha tomado un cuerpo de ornitorrinco"));

            //PAGE3
            menuAnimals.Add(new Animal("unicornio", "El unicornio tiene poderes curativos y con su cuerno purificaba las aguas para que otros animales puedan beber"));
            menuAnimals.Add(new Animal("chthulhu", "Lejos de su hábitat normal en las profundidades del mar, los pequeños tulus se acercan a la civilización para causar horrores más allá de la comprensión."));
            menuAnimals.Add(new Animal("dragon", "Antes de convertirse en grandes reptiles capaces de escupir fuego y aterrorizar montañas, los dragones suelen ser tiernos… Y bastantes fotogenicos."));
            menuAnimals.Add(new Animal("buho", "Majestuosa y enigmática ave, con una capacidad única de girar su cabeza hasta 270 grados lo que le permite buscar presas con precisión, capturando cada detalle en la oscuridad como cual fotografo."));
            menuAnimals.Add(new Animal("stitch", "El Stychicus hawaiano es una extraña criatura omnívora que durante el día duerme en agujeros húmedos y oscuros. Algunos estudios afirman que no es de este planeta."));
            menuAnimals.Add(new Animal("lechuza", "Antiguamente utilizadas para la labor manual de entregar cartas en escuelas de Inglaterra. Las Lechuzas ahora pueden ser encontradas más frecuentemente en su hábitat natural."));
            menuAnimals.Add(new Animal("polilla", "¿Tienes el Flash encendido?, Entonces tienes una polilla al lado tuyo. Mariposa nocturna y amantes de la luz."));
            menuAnimals.Add(new Animal("murcielago", "La mayoría de los murciélagos emiten sonidos llamados ecolocalización para navegar en la oscuridad, algunos no lo hacen tan bien y a veces terminan volando en reverso."));

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

            //ASIGN ANIMAL DESCRIPTION TO TEXT:
            foreach (Animal animal in data.animales)
            {
                // Busca el TextMeshProUGUI con el mismo nombre que el animal y establece su texto.
                foreach (TextMeshProUGUI textMesh in descriptionsTexts)
                {
                    if (textMesh.name == animal.name)
                    {
                        textMesh.text = animal.photographed? animal.description : "Aun no encontrado";
                        break; // Sal del bucle una vez que se encuentre el TextMeshProUGUI correspondiente.
                    }
                }
            }

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

    //TO CHANGE HOVERS
    public void changeHovers()
    {

        if (currentPageIndex == 0)
        {
            foreach (GameObject obj in hoversToShowOnPage2)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in hoversToShowOnPage3)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in hoversToShowOnPage1)
            {
                obj.SetActive(true);
            }
        }
        else if (currentPageIndex == 1)
        {
            foreach (GameObject obj in hoversToShowOnPage1)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in hoversToShowOnPage3)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in hoversToShowOnPage2)
            {
                obj.SetActive(true);
            }
        }
        else if (currentPageIndex == 2)
        {
            foreach (GameObject obj in hoversToShowOnPage1)
           {
                obj.SetActive(false);
          }
            foreach (GameObject obj in hoversToShowOnPage2)
            {
               obj.SetActive(false);
            }
            foreach (GameObject obj in hoversToShowOnPage3)
           {
                obj.SetActive(true);
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
