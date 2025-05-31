using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _intant;
    public static UIManager intant => _intant;
    public GameObject Notify;
    void Start()
    {
        if(_intant == null)
        {
            _intant = this;
        }
        else
        {
            Destroy(this);
        }
        Notify.gameObject.SetActive(false);
    }

   

    public void Message(string message)
    {
        Notify.GetComponentInChildren<TextMeshProUGUI>().text = message;
        Notify.SetActive(true);
        Grid.intant.enabled = false;
    }

    public void Restart()
    {   
        ScoreManager.intant.score = 0;
        ScoreManager.intant.UpdateUI();
        foreach (Row r in Grid.intant.Rows)
        {
            foreach (Cell c in r.Cells)
            {
                c.Restart();
            }
        }
        PlayerPrefs.DeleteKey(Contant.BoardData + Contant.max.ToString());
        PlayerPrefs.DeleteKey(Contant.Score + Contant.max.ToString());
        Grid.intant.maxValue = 2;
        Notify.gameObject.SetActive(false);
        Grid.intant.ShowTile();
        Grid.intant.ShowTile();
    }


    public void Undo()
    {
        foreach (Row r in Grid.intant.Rows)
        {
            foreach (Cell c in r.Cells)
            {
                c.Undo();
            }
        }
        ScoreManager.intant.UpdateScore(-ScoreManager.intant.turnScore);
    }

    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
