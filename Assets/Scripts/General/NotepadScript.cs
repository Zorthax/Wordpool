using UnityEngine;
using System.Collections;

public class NotepadScript : MonoBehaviour {

    public string sentence;
    public UnityEngine.UI.Text text;
    public float fadeSpeed;
    public float timeOnScreen;

    bool fadingIn;
    bool stay;
    bool fadingOut;
    float timer = 0;
    Color c;

	// Use this for initialization
	void Start ()
    {
        c = text.color;
        c.a = 0;
        fadingIn = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (fadingIn) fadeIn();
        if (stay) stayOnScreen();
        if (fadingOut) fadeOut();
        text.text = sentence;
        text.color = c;

        if (Input.GetButtonDown("Notepad")) fadingIn = true;
	}

    void fadeIn()
    {
        c.a += Time.deltaTime * fadeSpeed;
        if (c.a >= 1) { fadingIn = false; stay = true; }
    }

    void stayOnScreen()
    {
        timer += Time.deltaTime;
        if (timer >= timeOnScreen) { stay = false; timer = 0; fadingOut = true; }
    }

    void fadeOut()
    {
        c.a -= Time.deltaTime * fadeSpeed;
        if (c.a <= 0) { fadingOut = false; }
    }

    public void ShowText()
    { fadingIn = true; }

}
