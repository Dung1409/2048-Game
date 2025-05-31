using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _intant;
    public static ScoreManager intant => _intant;

    public int turnScore = 0;
    public int score = 0;
    public int highScore;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Start()
    {
        if(_intant == null)
        {
            _intant = this;
        }
        else
        {
            Destroy(this);
        }
        score = PlayerPrefs.GetInt(Contant.Score + Contant.max.ToString(), 0);
        highScore = PlayerPrefs.GetInt(Contant.HighScore + Contant.max.ToString() , 0);
        Debug.Log(highScore);
        UpdateUI();
    }


    public void UpdateScore(int value)
    {
        turnScore += value;
        score += value;
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(Contant.HighScore + Contant.max.ToString() ,highScore);
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }
}
