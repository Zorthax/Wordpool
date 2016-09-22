using UnityEngine;
using System.Collections;

public class SaveSystem : MonoBehaviour {

    static SaveSystem saveSystem;

    public bool sizeOfScreen;
    public int paintingWidth;
    public int paintingHeight;

    static bool searchForPaintings;
    static bool setTexturesFirstTime = true;

    const int saveSlots = 4;
    int currentSave = 0;

    static Texture2D[] daliTextures;
    GameObject[] daliPaintings;

    public Texture2D savedTexture;

	// Use this for initialization
	void Start ()
    {      
        if (saveSystem == null)
        {
            saveSystem = this;
        }
        else if (saveSystem != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        searchForPaintings = true;   
    }
	
	// Update is called once per frame
	void Update ()
    {
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
                Debug.Log("Set dali painting");
                daliPaintings[i] = gb;
                i++;
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
}
