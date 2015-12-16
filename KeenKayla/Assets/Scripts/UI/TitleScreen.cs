using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour 
{
    public void NewGame()
    {
        SaveGameManager.instance.NewGame();
        Application.LoadLevel("Level1");
    }

    public void Continue()
    {
        //TODO: Decide Appropriate Scene to Go To
        //TODO: Create Loading Screen (Probably)
        Application.LoadLevel("Level1");
    }

    public void DebugMenu()
    {
        Application.LoadLevel("DebugMenu");
    }
}
