using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    int minValue = 4;
    int maxValue = 5;
    int currentValue = 4;
    [SerializeField] TextMeshProUGUI info;
    [SerializeField] Image img;

    Dictionary<string , Sprite> GameScenes = new Dictionary<string , Sprite>();
    
    private void Start()
    {
        Contant.max = PlayerPrefs.GetInt(Contant.Row, 4);
        currentValue = Contant.max;
        info.text = currentValue.ToString() + "X" + currentValue.ToString();
        Sprite[] spites = Resources.LoadAll<Sprite>("GameScenes");
        for(int i = 0; i < spites.Length; i++)
        {
            GameScenes.Add(spites[i].name.ToLower(), spites[i]);
        }
        if (GameScenes.ContainsKey(currentValue.ToString()))
        {
            img.sprite = GameScenes[currentValue.ToString()];
        }
        
    }

    public void ChangeValue(int x)
    {
        currentValue += x;
        if(currentValue > maxValue)
        {
            currentValue = minValue;
        }
        if (currentValue < minValue) 
        {
            currentValue = maxValue;
        }
        info.text = currentValue.ToString() + "X" + currentValue.ToString();
        if (GameScenes.ContainsKey(currentValue.ToString()))
        {
            img.sprite = GameScenes[currentValue.ToString()];
            Contant.max = currentValue;
            PlayerPrefs.SetInt(Contant.Row, currentValue);
        }
    }

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(info.text.ToLower());
        Debug.Log(Contant.max);
    }


}
