using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public static UIMain instance;
    public Image textBar;
    private Text _textBarText;
    public GameObject MaruMari;
    public GameObject Pogo;

    public void Awake()
    {
        instance = this;
        _textBarText = textBar.GetComponentInChildren<Text>();
        textBar.canvasRenderer.SetAlpha(0);
        _textBarText.canvasRenderer.SetAlpha(0);
    }

    public void NewGame()
    {
        SaveGameManager.instance.NewGame();
        SceneManager.LoadScene(0);
    }

    public void ShowTextBar(string text, float fadeTime = 4)
    {
        _textBarText.text = text;
        textBar.CrossFadeAlpha(1, 1, true);
        _textBarText.CrossFadeAlpha(1, 1, true);

        if (fadeTime != 0)
        {
            StartCoroutine(WaitHideTextBar(fadeTime));
        }
    }

    public IEnumerator WaitHideTextBar(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideTextBar();
    }

    public void HideTextBar()
    {
        textBar.CrossFadeAlpha(0, 1, true);
        _textBarText.CrossFadeAlpha(0, 1, true);
    }
}
