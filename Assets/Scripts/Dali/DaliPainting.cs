using UnityEngine;
using System.Collections;

public class DaliPainting : DaliDoor
{
    public int index = 0;

    public override void OnLoad()
    {
        SaveSystem.readyToSave = true;
        SaveSystem.SetLoadIndex(index);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
