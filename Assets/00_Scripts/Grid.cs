using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private static Grid _intant;
    public static Grid intant => _intant;

    public List<Row> Rows = new List<Row>();
    [SerializeField] private GameObject tile;
    public List<Tile> poolTile = new List<Tile>();

    public int Score;
    [SerializeField] int turnScore;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] int maxValue;

    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject notify;
    int max;
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
        max = Contant.max;
        Rows = this.GetComponentsInChildren<Row>().ToList();

        Observer.AddListener(Contant.Win , Win);
        Observer.AddListener(Contant.GameOver, GameOver);
        Observer.AddListener(Contant.Restart , Restart);  
    }

    private void Start()
    {
        Score = 0;
        ScoreText.text = Score.ToString();
        maxValue = 2;
        turnScore = 0;
        foreach (Row r in Rows)
        {
            foreach (Cell c in r.Cells)
            {
                if (c.tile != null)
                {
                    maxValue = Mathf.Max(maxValue, c.tile.value);
                }
            }
        }
        notify.SetActive(false);
    }

    private void Update()
    {
        if (!StartButton.activeSelf)
        {
            if (maxValue == 2048)
            {
                Observer.Notify(Contant.Win);
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                turnScore = 0;
                MoveRight();
            }

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                turnScore = 0;
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                turnScore = 0;
                MoveUp();
            }

            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                turnScore = 0;
                MoveDown();
            }
        }
    }
    #region
    public void MoveTiles(Vector2Int direction)
    {
        bool isHorizontal = direction.y == 0;
        for (int i = 0; i < max; i++)
        {
            Queue<int> q = new Queue<int>();
            int n = (direction.x == 1 || direction.y == 1) ? max - 1 : 0;
            Tile top = null;

            for (int j = (direction.x == 1 || direction.y == 1) ? max - 1 : 0;
                 (direction.x == 1 || direction.y == 1) ? j >= 0 : j < max;
                 j += (direction.x == 1 || direction.y == 1) ? -1 : 1)
            {
                Cell cell = isHorizontal ? Rows[i].Cells[j] : Rows[j].Cells[i];
                if (cell.tile != null)
                {
                    cell.oldValue = cell.tile.value;
                    cell.tile.canMerge = true;
                    if (q.Count != 0)
                    {
                        if (cell.tile.value == top.value && top.canMerge)
                        {
                            top.value *= 2;
                            maxValue = Mathf.Max(maxValue, top.value);
                            UpdateTextScore(top.value);
                            top.ChangeState();
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
                int idx = q.Dequeue();
                Cell currentCell = isHorizontal ? Rows[i].Cells[idx] : Rows[idx].Cells[i];
                Cell targetCell = isHorizontal ? Rows[i].Cells[n] : Rows[n].Cells[i];

                if (currentCell.tile.canMerge)
                {
                    currentCell.tile.parent = targetCell;
                    currentCell.tile.Moving();
                    if (idx != n)
                    {
                        currentCell.tile = null;
                    }
                    n += (direction.x == 1 || direction.y == 1) ? -1 : 1;
                }
                else
                {
                    currentCell.tile.Moving();
                    Tile a = currentCell.tile;
                    currentCell.tile = null;
                    poolTile.Add(a);
                    a.gameObject.SetActive(false);
                }
            }
        }
        StartCoroutine(SpawnTile());
    }

    public void MoveLeft() => MoveTiles(new Vector2Int(-1, 0));
    public void MoveRight() => MoveTiles(new Vector2Int(1, 0));
    public void MoveUp() => MoveTiles(new Vector2Int(0, -1));
    public void MoveDown() => MoveTiles(new Vector2Int(0, 1));
    #endregion
       
    public void ShowTile()
    {
        int index = Random.Range(0, 3);
        int startIndex = index;
        while (true)
        {
            Cell c = Rows[index].CellEmpty();
            if (c != null)
            {
                Tile t = CreateTile().GetComponent<Tile>();
                t.gameObject.SetActive(true);
                t.parent = c;
                c.tile = t;
                t.transform.SetParent(c.transform);
                t.transform.position = c.transform.position;
                t.value = Random.Range(1 , 3) * 2;
                t.ChangeState();
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
                     Observer.Notify(Contant.GameOver);
                    return;
                }
            }
        }

    }
    public void UpdateTextScore(int value)
    {
        turnScore += value;
        Score += value;
        ScoreText.text = Score.ToString();
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
        yield return new WaitForSeconds(0.1f);
        ShowTile();
    }
    public void Back()
    {
        foreach (Row r in Rows)
        {
            foreach (Cell c in r.Cells)
            {
                c.Back();
            }
        }
        UpdateTextScore(-turnScore);
    }
    public void Restart()
    {
        Score = 0;
        turnScore = 0;
        ScoreText.text = turnScore.ToString();
        this.enabled = true;    
        foreach(Row r in Rows)
        {
            foreach (Cell c in r.Cells)
            {
                c.Restart();
            }
        }
        StartGame();
    }
    public void StartGame()
    {
        ShowTile();
        ShowTile();
        StartButton.SetActive(false);
        notify.SetActive(false);    
    }

    #region
    public void Notify(string message)
    {
        notify.SetActive(true); 
        notify.GetComponentInChildren<TextMeshProUGUI>().text = message;
        this.enabled = false;
    }
    public void Win() => Notify(Contant.Win);
    public void GameOver() => Notify(Contant.GameOver);
    #endregion

}