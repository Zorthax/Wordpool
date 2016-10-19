using UnityEngine;
using System.Collections;

public class SaveSystem : MonoBehaviour {

    static SaveSystem saveSystem;

    public bool sizeOfScreen;
    public int paintingWidth;
    public int paintingHeight;

    static bool searchForPaintings;
    static bool setTexturesFirstTime = true;
    static bool inHubWorld = false;

    const int saveSlots = 4;
    static int currentSave = 0;
    static int currentLoad = 0;
    public static bool readyToSave;

    static Texture2D[] daliTextures;
    static bool[,] daliPhases;
    static GameObject daliObject;
    GameObject[] daliPaintings;

    public Texture2D savedTexture;

	// Use this for initialization
	void Start ()
    {
        inHubWorld = IsInHubWorld();
        if (saveSystem == null)
        {
            saveSystem = this;
        }
        else if (saveSystem != this)
        {
            searchForPaintings = true;
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        daliObject = GameObject.Find("Dali Phases");
        if (daliObject != null && readyToSave && daliPhases != null)
        {
            Debug.Log("Loading objects in dali level");
            daliObject.GetComponent<DaliPhases>().LoadPhases(daliPhases, currentSave);
            readyToSave = false;
        }

           
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("Current Save Slot: " + currentSave);
        StartCoroutine(What());

        if (searchForPaintings)
        {
            SetTextures();
            SetPaintings(); 
            searchForPaintings = false;
        }
    }

    IEnumerator What()
    {
        if (Input.GetKeyDown("l"))
        {

            yield return new WaitForEndOfFrame();
            Texture2D tex;
            if (!sizeOfScreen)
            {
                tex = new Texture2D(paintingWidth, paintingHeight, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(Screen.width / 2 - paintingWidth / 2, Screen.height / 2 - paintingHeight / 2, paintingWidth, paintingHeight), 0, 0, false);
            }
            else
            {
                tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            }
            tex.Apply();
            savedTexture = tex;
            if (currentSave < 0)
                ShiftSaves();
        }
    }

    void SetTextures()
    {
        if (setTexturesFirstTime)
        {
            Texture2D tex = new Texture2D(1, 1);
            Color[] white = new Color[1] { new Color(0, 1, 1) };
            tex.SetPixels(white);
            tex.Apply();

            daliTextures = new Texture2D[saveSlots];
            for (int i = 0; i < saveSlots; i++)
            {
                daliTextures[i] = tex;
            }
            setTexturesFirstTime = false;
        }


        if (savedTexture != null)
        {
            daliTextures[currentSave] = savedTexture;
            savedTexture = null;
            if (currentSave < saveSlots - 1) currentSave++;
            else currentSave = 0;
        }    
    }

    void SetPaintings()
    {
        GameObject[] allPaintings;
        allPaintings = GameObject.FindGameObjectsWithTag("SaveFrame");

        int i = 0;
        daliPaintings = new GameObject[saveSlots];
        foreach(GameObject gb in allPaintings)
        {
            if (gb.GetComponent<DaliPainting>() != null)
            {
                //Debug.Log("Set dali painting");
                //daliPaintings[i] = gb;
                //i++;
                daliPaintings[gb.GetComponent<DaliPainting>().index] = gb;
            }
        }
        PaintingTextures();
       
    }

    void PaintingTextures()
    {
        
        for (int x = 0; x < daliPaintings.Length && daliPaintings[x] != null; ++x)
        {
            MeshRenderer mr = daliPaintings[x].GetComponent<MeshRenderer>();
            Destroy(mr.material);
            mr.materials = new Material[1];
            mr.materials[0].mainTexture = daliTextures[x]; 
        }

        
    }

    public void SavePhases()
    {
        
        if (daliObject != null)
        {
            if (daliPhases == null)
            {
                Debug.Log("Resetting save array");
                daliPhases = new bool[saveSlots, daliObject.GetComponent<DaliPhases>().SavePhases().Length];
            }
            bool[] phases = daliObject.GetComponent<DaliPhases>().SavePhases();
            for (int i = 0; i < phases.Length; i++)
                daliPhases[currentSave, i] = phases[i];
        }
    }

    bool IsInHubWorld()
    {
        DaliDoor door = FindObjectOfType<DaliDoor>();
        if (door != null)
            return true;
        return false;
    }

    public static void SetLoadIndex(int index)
    {
        currentSave = index;
    }

    void ShiftSaves()
    {
        currentSave = 0;
        for (int i = 0; i < saveSlots; i++)
        {
            if (i > 0)
            {
                if (daliTextures[i - 1] != null)
                {
                    daliTextures[i] = daliTextures[i - 1];
                    
                    //Move each save up
                    for (int j = 0; j < daliPhases.GetLength(i); i++)
                        daliPhases[currentSave, j] = daliPhases[currentSave - 1, j];
                }
            }
        }    
    }
}
