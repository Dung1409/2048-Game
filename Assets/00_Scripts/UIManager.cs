using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _intant;
    public static UIManager intant => _intant;

    public GameObject StartButton;
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
    }

    public void StartGame()
    {
        StartButton.SetActive(false);
        Notify.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey(Contant.BoardData))
        {
            LoadBoard();
            return;
        }
        Grid.intant.ShowTile();
        Grid.intant.ShowTile();
        Grid.intant.enabled = true;
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
        PlayerPrefs.DeleteKey(Contant.BoardData);
        PlayerPrefs.DeleteKey(Contant.Score);
        foreach (Row r in Grid.intant.Rows)
        {
            foreach (Cell c in r.Cells)
            {
                c.Restart();
            }
        }
        StartGame();
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

    public void LoadBoard()
    {
        BoardData boardData = JsonUtility.FromJson<BoardData>(PlayerPrefs.GetString(Contant.BoardData));
        for(int i = 0; i < boardData.board.Length; i++)
        {
            Cell c = Grid.intant.Rows[i / Contant.max].Cells[i % Contant.max];
            if (boardData.board[i] != 0)
            {
                Grid.intant.getTile(c , boardData.board[i]);
            }
        }
        ScoreManager.intant.score = PlayerPrefs.GetInt(Contant.Score);
        ScoreManager.intant.highScore = PlayerPrefs.GetInt(Contant.HighScore);
        ScoreManager.intant.UpdateUI();
    }
}
