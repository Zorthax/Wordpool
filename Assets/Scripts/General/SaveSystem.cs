using UnityEngine;
using System.Collections;

public class SaveSystem : MonoBehaviour {

    static SaveSystem saveSystem;
    Camera screenshotCamera;

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
    static bool daliPhasesSet;
    public GameObject daliObject;
    static GameObject realDali;
    GameObject[] daliPaintings;

    static Texture2D[] escherTextures;
    static bool[,] escherPhases;
    static bool escherPhasesSet;
    public GameObject escherObject;
    static GameObject realEscher;
    GameObject[] escherPaintings;

    public static Texture2D savedTexture;
    static bool[] savedPhases;

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
        realDali = daliObject;
        realEscher = escherObject;
        DontDestroyOnLoad(gameObject);
        if (realDali != null && readyToSave && daliPhases != null)
        {
            Debug.Log("Loading objects in dali level");
            realDali.GetComponent<DaliPhases>().LoadPhases(daliPhases, currentSave);
            readyToSave = false;
        }
        else if (realEscher != null && readyToSave && escherPhases != null)
        {
            Debug.Log("Loading objects in dali level");
            realEscher.GetComponent<EscherPhases>().LoadPhases(escherPhases, currentSave);
            readyToSave = false;
        }


    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("Current Save Slot: " + currentSave);
        //daliCamera = Camera.main;
        
        if (searchForPaintings)
        {
            SetTextures();
            SetPaintings(); 
            searchForPaintings = false;
        }
    }

    IEnumerator What(int levelIndex)
    {
        Camera cam = Camera.main;
        screenshotCamera.gameObject.SetActive(true);
        cam.gameObject.SetActive(false);
        screenshotCamera.gameObject.tag = "MainCamera";
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

        if (levelIndex == 0) { if (currentSave < 0) ShiftDaliSaves(); daliTextures[currentSave] = savedTexture; SaveDaliPhases(); }
        if (levelIndex == 1) { if (currentSave < 0) ShiftEscherSaves(); escherTextures[currentSave] = savedTexture; SaveEscherPhases(); }

        cam.gameObject.SetActive(true);
        screenshotCamera.gameObject.SetActive(false);
        cam.gameObject.tag = "MainCamera";

        Application.LoadLevel(0);
           
    }

    public void TakeScreenshot(int levelIndex)
    {
        StartCoroutine(What(levelIndex));
    }

    void SetTextures()
    {
        if (setTexturesFirstTime)
        {
            //Texture2D tex = new Texture2D(1, 1);
            //Color[] white = new Color[1] { new Color(0, 1, 1) };
            //tex.SetPixels(white);
            //tex.Apply();
            //
            daliTextures = new Texture2D[saveSlots];
            escherTextures = new Texture2D[saveSlots];
            //for (int i = 0; i < saveSlots; i++)
            //{
            //    daliTextures[i] = tex;
            //}
            setTexturesFirstTime = false;
        }


        //if (savedTexture != null)
        //{
        //    daliTextures[currentSave] = savedTexture;
        //    savedTexture = null;
        //    if (currentSave < saveSlots - 1) currentSave++;
        //    else currentSave = 0;
        //}    
    }

    void SetPaintings()
    {
        GameObject[] allPaintings;
        allPaintings = GameObject.FindGameObjectsWithTag("SaveFrame");

        int i = 0;
        daliPaintings = new GameObject[saveSlots];
        escherPaintings = new GameObject[saveSlots];
        foreach (GameObject gb in allPaintings)
        {
            if (gb.GetComponent<DaliPainting>() != null)
            {
                //Debug.Log("Set dali painting");
                //daliPaintings[i] = gb;
                //i++;
                daliPaintings[gb.GetComponent<DaliPainting>().index] = gb;
            }
            else if (gb.GetComponent<EscherPainting>() != null)
            {
                //Debug.Log("Set dali painting");
                //daliPaintings[i] = gb;
                //i++;
                escherPaintings[gb.GetComponent<EscherPainting>().index] = gb;
            }
        }
        PaintingTextures();
       
    }

    void PaintingTextures()
    {
        
        for (int x = 0; x < daliPaintings.Length && daliPaintings[x] != null && daliTextures[x] != null; ++x)
        {
            MeshRenderer mr = daliPaintings[x].GetComponent<MeshRenderer>();
            Destroy(mr.material);
            mr.materials = new Material[1];
            mr.materials[0].mainTexture = daliTextures[x]; 
        }

        for (int x = 0; x < escherPaintings.Length && escherPaintings[x] != null && escherTextures[x] != null; ++x)
        {
            MeshRenderer mr = escherPaintings[x].GetComponent<MeshRenderer>();
            Destroy(mr.material);
            mr.materials = new Material[1];
            mr.materials[0].mainTexture = escherTextures[x];
        }


    }

    public void SaveDaliPhases()
    {
        if (daliPhases == null)
        {
            Debug.Log("Resetting save array");
            daliPhases = new bool[saveSlots, realDali.GetComponent<DaliPhases>().SavePhases().Length];
        }

        if (realDali != null)
        {
            bool[] phases = realDali.GetComponent<DaliPhases>().SavePhases();
            for (int i = 0; i < phases.Length; ++i)
                daliPhases[currentSave, i] = phases[i];
        }
    }

    public void SaveEscherPhases()
    {
        if (escherPhases == null)
        {
            Debug.Log("Resetting save array");
            escherPhases = new bool[saveSlots, realEscher.GetComponent<EscherPhases>().SavePhases().Length];
        }

        if (realEscher != null)
        {
            bool[] phases = realEscher.GetComponent<EscherPhases>().SavePhases();
            for (int i = 0; i < phases.Length; ++i)
                escherPhases[currentSave, i] = phases[i];
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

    void ShiftDaliSaves()
    {
        currentSave = 0;
        for (int i = saveSlots - 1; i > 0; i--)
        {
            if (daliTextures[i - 1] != null && daliPhases != null)
            {
                daliTextures[i] = daliTextures[i - 1];
                int length = daliPhases.GetLength(1); //bug testing

                //Move each save up
                for (int j = 0; j < length; j++)
                    daliPhases[i, j] = daliPhases[i - 1, j];
            }
        }        
    }

    void ShiftEscherSaves()
    {
        currentSave = 0;
        for (int i = saveSlots - 1; i > 0; i--)
        {
            if (escherTextures[i - 1] != null && escherPhases != null)
            {
                escherTextures[i] = escherTextures[i - 1];
                int length = escherPhases.GetLength(1); //bug testing

                //Move each save up
                for (int j = 0; j < length; j++)
                    escherPhases[i, j] = escherPhases[i - 1, j];
            }
        }

    }

    public void SetScreenshotCamera(Camera cam)
    {
        screenshotCamera = cam;
    }
}
