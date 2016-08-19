using UnityEngine;
using System.Collections;

public class SaveSystem : MonoBehaviour {

    public bool sizeOfScreen;
    public int paintingWidth;
    public int paintingHeight;
    GameObject notepad;


	// Use this for initialization
	void Start ()
    {
        notepad = GameObject.FindGameObjectWithTag("Notepad");
	}
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(What());
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
                tex.ReadPixels(new Rect(0, 0, paintingHeight, paintingHeight), 0, 0, false);
            }
            else
            {
                tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            }
            tex.Apply();
            notepad.GetComponent<MeshRenderer>().material.mainTexture = tex;
        }
    }
}
