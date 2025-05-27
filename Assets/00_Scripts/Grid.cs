using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private static Grid _intant;
    public static Grid intant => _intant;

    int maxValue;
    public int maxRow;

    public List<Row> Rows = new List<Row>();
    [SerializeField] private GameObject tile;
    public List<Tile> poolTile = new List<Tile>();

    private void Awake()
    {
        if (_intant == null)
        {
            _intant = this;
        }
        else
        {
            Destroy(this);
        }

        maxRow = Contant.max;
        Rows = this.GetComponentsInChildren<Row>().ToList();
        maxValue = 2;
    }

    private void Update()
    {
        Play();
    }

    public void Play()
    {
        if (!UIManager.intant.StartButton.activeSelf || UIManager.intant.Notify.gameObject.activeSelf)
        {
            if (maxValue == 2048)
            {
                UIManager.intant.Message(Contant.Win);
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(new Vector2Int(1, 0));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(new Vector2Int(-1, 0));
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(new Vector2Int(0, -1));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(new Vector2Int(0, 1));
            }
        }
    }

    public void MoveTiles(Vector2Int direction)
    {
        ScoreManager.intant.turnScore = 0;
        bool isHorizontal = direction.y == 0;
        Queue<int> q = new Queue<int>();
        for(int i = 0; i < maxRow; i++)
        {
            Tile top = null;
            bool b = (direction.x == 1 || direction.y == 1);
            int n = b ? maxRow - 1 : 0;
            for(int j =  n ;  (b ? j >= 0 : j < maxRow); j += (b ? -1 : 1))
            {
                Cell cell = isHorizontal ? Rows[i].Cells[j] : Rows[j].Cells[i];
                if (cell.tile != null) 
                {
                    cell.oldValue = cell.tile.value;
                    cell.tile.canMerge = true;
                    if(q.Count != 0)
                    {
                        if(cell.tile.value == top.value && top.canMerge)
                        {
                            top.value *= 2;
                            top.ChangeState();
                            maxValue = Mathf.Max(maxValue, top.value);
                            ScoreManager.intant.UpdateScore(top.value);
                            cell.tile.canMerge = false;
                        }
                    }
                    top = cell.tile;
                    q.Enqueue(j);
                }
                else
                {
                    cell.oldValue = 0;
                }
            }
            while (q.Count > 0)
            {
                int index = q.Dequeue();
                Cell currentCell = isHorizontal ? Rows[i].Cells[index] : Rows[index].Cells[i];
                Cell TargetCell = isHorizontal ? Rows[i].Cells[n] : Rows[n].Cells[i];

                if (currentCell.tile.canMerge)
                {
                    currentCell.tile.parent = TargetCell;
                    currentCell.tile.Moving();
                    if (index != n)
                    {
                        currentCell.tile = null;
                    }
                    n += (b ? -1 : 1);
                }
                else
                {
                    {
                        currentCell.tile.Moving();
                        Tile t = currentCell.tile;
                        currentCell.tile = null;
                        poolTile.Add(t);
                        t.gameObject.SetActive(false);
                    }
                }
            }
        }
        StartCoroutine(SpawnTile());
    }

    public void ShowTile()
    {
        int index = Random.Range(0, 3);
        int startIndex = index;
        while (true)
        {
            Cell c = Rows[index].CellEmpty();
            if (c != null)
            {
                getTile(c , Random.Range(1, 3) * 2);
                return;
            }
            else
            {
                index++;
                if (index >= Rows.Count)
                {
                    index = 0;
                }
                if (index == startIndex)
                {
                    UIManager.intant.Message(Contant.GameOver);
                    return;
                }
            }
        }

    }

    public GameObject CreateTile()
    {
        foreach (Tile t in poolTile)
        {
            if (!t.gameObject.activeSelf)
                return t.gameObject;
        }
        GameObject g = Instantiate(tile);
        return g;
    }
    
    IEnumerator SpawnTile()
    {
        yield return new WaitForSeconds(0.2f);
        ShowTile();
    }
    
    public void getTile(Cell c , int value)
    {
        
        Tile t = CreateTile().GetComponent<Tile>();
        t.gameObject.SetActive(true);
        t.parent = c;
        c.tile = t;
        t.transform.SetParent(c.transform);
        t.transform.position = c.transform.position;
        t.value = value;
        maxValue = Mathf.Max(maxValue, t.value);
        t.ChangeState();
    }

}