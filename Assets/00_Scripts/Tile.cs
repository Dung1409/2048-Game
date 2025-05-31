using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public List<TileState> tileStates = new List<TileState>();
    public int value;
    public Vector2Int pos;
    public TileState dataState;
    public Cell parent;
    public bool canMerge;
    
    private Animator _animator;


    [SerializeField] private TextMeshProUGUI text;
    public Dictionary<Vector2Int, bool> dir = new Dictionary<Vector2Int, bool>() {  {Vector2Int.right,  true},
                                                                                    {Vector2Int.left,   true},
                                                                                    {Vector2Int.up,     true}, 
                                                                                    {Vector2Int.down,   true}};
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        ChangeState();
        canMerge = true;
        _animator = GetComponent<Animator>();
    }

    public void Moving()
    {
        StartCoroutine(move());
    }

    IEnumerator move()
    {

        if (parent.tile == null)
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

    public void ChangeAnimation(bool isMerge)
    {
        _animator.SetBool("Merge" , isMerge);
    }



    public void AnimationIdle() => ChangeAnimation(false);
    public void AnimationMerge() => ChangeAnimation(true);
    public void ChangeState()
    {
        int idx = (int)Mathf.Log(value, 2) - 1;
        dataState = tileStates[idx];
        this.GetComponent<Image>().color = dataState.backgroundColor;
        text.text = value.ToString();
        text.color = dataState.textColor;
    }    
}
