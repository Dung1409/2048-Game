using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public List<TileState> tileStates = new List<TileState>(); 
    public int value;
    public TileState state;
    public Cell parent;

    private TextMeshProUGUI text; 

    public bool canMerge;
    void Awake()
    {
        text = this.GetComponentInChildren<TextMeshProUGUI>();
        text.color = state.textColor;
        this.GetComponent<Image>().color = state.backgroundColor;
        canMerge = true;
    }


    public void Moving()
    {
        StartCoroutine(move());
    }

    IEnumerator move()
    {
        
        if(parent.tile == null)
        {
            parent.tile = this;
        }
        
        this.transform.SetParent(parent.transform);
        float elapsed = 0f;
        float duration = 0.1f;
        Vector3 from = transform.position;
        Vector3 to = parent.transform.position;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;  
        }
        this.transform.position = parent.transform.position;
    }

    public void ChangeState()
    {
        int idx = (int) Mathf.Log(value , 2) - 1;
        state = tileStates[idx];
        this.GetComponent<Image>().color = state.backgroundColor;
        text.text = value.ToString();
        text.color = state.textColor;
    }
}
