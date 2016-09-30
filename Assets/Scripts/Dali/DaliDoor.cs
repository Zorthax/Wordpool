using UnityEngine;
using System.Collections;

public class DaliDoor : MonoBehaviour {

    public float distance = 3;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        TestLevelLoad();
    }

    void TestLevelLoad()
    {
        if (Input.GetButtonDown("Pick Up"))
        {
            RaycastHit[] all = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, distance);

            foreach (RaycastHit h in all)
            {
                if (h.transform == transform)
                {
                    OnLoad();
                }
            }
        }
    }

    public virtual void OnLoad()
    {
        SaveSystem.SetLoadIndex(-1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
