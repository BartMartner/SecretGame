using UnityEngine;
using System.Collections;

public class SaveTrigger : MonoBehaviour
{
    public bool playerPresent;
    public AudioClip sound;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        playerPresent = true;
        Player.instance.preventAttack = true;
        UIMain.instance.ShowTextBar("Press X To Save", 0);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        playerPresent = false;
        Player.instance.preventAttack = false;
        UIMain.instance.HideTextBar();
    }

    public void Update()
    {
        if(playerPresent && Input.GetButton("Attack"))
        {
            StartCoroutine(Save());
        }
    }

    public IEnumerator Save()
    {
        playerPresent = false;


        UIMain.instance.ShowTextBar("Saving");

        AudioSource.PlayClipAtPoint(sound, transform.position);
        Player.instance.DisableMovement();
        var newPosition = Player.instance.transform.position;
        newPosition.x = transform.position.x;
        Player.instance.transform.position = newPosition;
        Player.instance.health = Player.instance.maxHealth;
        Player.instance.currentBombs = Player.instance.maxBombs;
        SaveGameManager.instance.saveGameData.savePosition = Player.instance.transform.position.ToVector3Data();
        SaveGameManager.instance.saveGameData.lastRoom = transform.root.name;
        SaveGameManager.instance.SaveGame();

        yield return StartCoroutine(Player.instance.Flash(8, 0.1f, Color.cyan, 0.5f));

        yield return null;
        UIMain.instance.ShowTextBar("Save Successful");
        Player.instance.EnableMovement();
        Player.instance.preventAttack = false;
    }
}
