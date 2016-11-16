using UnityEngine;
using System.Collections;

public class EscherPainting : EscherDoor {

    public int index = 0;

    public override void OnLoad()
    {
        SaveSystem.readyToSave = true;
        SaveSystem.SetLoadIndex(index);
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
