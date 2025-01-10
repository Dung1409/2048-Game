using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell : MonoBehaviour
{
    public Tile tile;
    public Vector2Int pos;

    public int oldValue;

    private void Start()    
    {
        AddTile();
        oldValue = 0;  
    }

    public void AddTile()
    {
        try
        {
            tile = this.GetComponentInChildren<Tile>();
            tile.parent = this;
            tile.index = pos;

        }
        catch (System.Exception ex)
        {
            tile = null;
        }
    }

    public void Back()
    {
        if(oldValue == 0)
        {
            if(tile != null)
            {
                tile.gameObject.SetActive(false);
            }
            this.tile = null;
            return;
        }

        if(tile != null)
        {
            tile.value = oldValue;
            tile.ChangeState();
        }
        else
        {
            Tile t = Grid.intant.CreateTile().GetComponent<Tile>();
            t.gameObject.SetActive(true);
            t.parent = this;
            this.tile = t;
            t.transform.SetParent(this.transform);
            t.transform.position = this.transform.position;
            t.value = oldValue;
            t.ChangeState();
        }
    }
}

