using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Row : MonoBehaviour
{
    public  List<Cell> Cells = new List<Cell>();
    private void Start()
    {
        int row = Grid.intant.Rows.IndexOf(this);   
        Cells = this.GetComponentsInChildren<Cell>().ToList();
        for (int i = 0; i < Cells.Count; i++) {
            Vector2Int pos = new Vector2Int(row, i);
            Cells[i].pos = new Vector2Int(row, i);
            /*
            if (Cells[i].tile != null ) 
            {
                Cells[i].tile.index = pos;
            }
            */
        }
    }

    public Cell CellEmpty()
    {
        int index = Random.Range(0, 3);
        int startingIndex = index;
        while (Cells[index].tile)
        {
            index++;

            if(index >= Cells.Count)
            {
                index = 0;
            }
            if(index == startingIndex)
            {
                return null;
            }
        }
        return Cells[index];    
    }
}
