using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIMain : MonoBehaviour
{
    public static UIMain instance;

    public void Awake()
    {
        instance = this;
    }

    public void NewGame()
    {
        SaveGameManager.instance.NewGame();
        SceneManager.LoadScene(0);
    }
}
