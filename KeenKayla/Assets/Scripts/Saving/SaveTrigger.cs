using UnityEngine;
using System.Collections;

public class SaveTrigger : MonoBehaviour
{
    public bool playerPresent;

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

        UIMain.instance.ShowTextBar("Save Successful");

        //TODO: Play Some Animation For the Save Station
        //TODO: Play Some Animation For the Player
        var newPosition = Player.instance.transform.position;
        newPosition.x = transform.position.x;
        Player.instance.transform.position = newPosition;
        Player.instance.health = Player.instance.maxHealth;
        Player.instance.currentBombs = Player.instance.maxBombs;
        SaveGameManager.instance.saveGameData.savePosition = Player.instance.transform.position.ToVector3Data();
        SaveGameManager.instance.saveGameData.lastRoom = transform.root.name;
        SaveGameManager.instance.SaveGame();

        yield return null;

        Player.instance.preventAttack = false;
    }
}
