using UnityEngine;
using System.Collections;

public class SwappableMaterial : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void ChangeMaterial(Material mat)
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
            rend.material = mat;
        else
            Debug.Log("Can not change material due to missing renderer");
    }
}
