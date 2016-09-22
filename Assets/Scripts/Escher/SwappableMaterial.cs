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
        {
            Material[] temp = rend.materials;
            for (int i = 0; i < rend.sharedMaterials.Length; i++)
            {
                temp[i] = mat;
            }
            rend.materials = temp;
        }
        else
            Debug.Log("Can not change material due to missing renderer");
    }
}
