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
