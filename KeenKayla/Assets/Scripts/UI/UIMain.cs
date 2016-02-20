using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public static UIMain instance;
    public Image textBar;
    private Text _textBarText;
    public GameObject maruMari;
    public GameObject pogo;
    public GameObject coldSuit;
    public GameObject redLazer;
    public GameObject hoverBoots;
    public GameObject powerSuit;
    public GameObject purpleLazer;
    public GameObject heartPopUp;
    public GameObject bombPopUp;
    public GameObject lazerPopUp;
    public AudioClip itemCollectJingle;

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

    public void ShowPopUp(PowerUpID type)
    {
        switch(type)
        {
            case PowerUpID.ColdSuit:
                StartCoroutine(ItemCollect(coldSuit));
                break;
            case PowerUpID.HoverBoots:
                StartCoroutine(ItemCollect(hoverBoots));
                break;
            case PowerUpID.MaruMari:
                StartCoroutine(ItemCollect(maruMari));
                break;
            case PowerUpID.PogoStick:
                StartCoroutine(ItemCollect(pogo));
                break;
            case PowerUpID.PowerSuit:
                StartCoroutine(ItemCollect(powerSuit));
                break;
            case PowerUpID.PurpleLazer:
                StartCoroutine(ItemCollect(purpleLazer));
                break;
            case PowerUpID.RedLazer:
                StartCoroutine(ItemCollect(redLazer));
                break;
        }
    }

    public void ShowHeartPopup()
    {
        StartCoroutine(ItemCollect(heartPopUp));
    }

    public void ShowBombPopup()
    {
        StartCoroutine(ItemCollect(bombPopUp));
    }

    public void ShowLazerPopup()
    {
        StartCoroutine(ItemCollect(lazerPopUp));
    }

    private IEnumerator ItemCollect(GameObject popUp)
    {
        AudioSource.PlayClipAtPoint(itemCollectJingle, Player.instance.transform.position);

        popUp.SetActive(true);
        Time.timeScale = 0;

        var timer = 0f;

        bool press = false;
        while (timer < 6)
        {
            press = Input.anyKey;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        while(!(press || Input.anyKey))
        {
            yield return null;
        }

        Time.timeScale = 1;
        popUp.SetActive(false);
    }

    public void HideTextBar()
    {
        textBar.CrossFadeAlpha(0, 1, true);
        _textBarText.CrossFadeAlpha(0, 1, true);
    }
}
