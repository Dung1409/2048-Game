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
    private void Start()
    {
        if (_intant == null)
        {
            _intant = this;
        }
        else
        {
            Destroy(this);
        }
        Score = 0;
        ScoreText.text = Score.ToString();
        Rows = this.GetComponentsInChildren<Row>().ToList();
        maxValue = 2;
        turnScore = 0;
        foreach(Row r in Rows)
        {
            foreach(Cell c in r.Cells) {
                if (c.tile != null)
                {
                    maxValue = Mathf.Max(maxValue, c.tile.value);
                }
            }
        }
    }

    private void Update()
    {
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

        if(maxValue == 2048)
        {
            Time.timeScale = 0;
            Debug.Log("You win");
        }

    }

    public void MoveRight()
    {
        for (int i = 0; i <= 3; i++)
        {
            Queue<int> q = new Queue<int>();
            int n = 3;
            Tile top = null;
            for (int j = 3; j >= 0; j--)
            {
                if (Rows[i].Cells[j].tile != null)
                {
                    Rows[i].Cells[j].oldValue = Rows[i].Cells[j].tile.value;
                    Rows[i].Cells[j].tile.canMerge = true;
                    if (q.Count != 0)
                    {
                        if (Rows[i].Cells[j].tile.value == top.value && top.canMerge == true)
                        {
                            top.value *= 2;
                            maxValue = Mathf.Max(maxValue , top.value);
                            UpdateTextScore(top.value);
                            top.ChangeState();
                            Rows[i].Cells[j].tile.canMerge = false;
                        }
                    }
                    top = Rows[i].Cells[j].tile;
                    q.Enqueue(j);
                }
                else
                {
                    Rows[i].Cells[j].oldValue = 0;
                }
            }
            while (q.Count > 0)
            {
                int idx = q.Dequeue();
                if (Rows[i].Cells[idx].tile.canMerge)
                {
                    Rows[i].Cells[idx].tile.parent = Rows[i].Cells[n];
                    Rows[i].Cells[idx].tile.index = new Vector2Int(i, n);
                    Rows[i].Cells[idx].tile.Moving();
                    if (idx != n)
                    {
                        Rows[i].Cells[idx].tile = null;
                    }
                    n--;
                }
                else
                {
                    Rows[i].Cells[idx].tile.index = new Vector2Int(i, n + 1);
                    Rows[i].Cells[idx].tile.Moving();
                    Tile a = Rows[i].Cells[idx].tile;
                    Rows[i].Cells[idx].tile = null;
                    poolTile.Add(a);
                    a.gameObject.SetActive(false);
                }
            }
        }
        StartCoroutine(SpawnTile());
    }

    public void MoveLeft()
    {
        for (int i = 0; i <= 3; i++)
        {
            Queue<int> q = new Queue<int>();
            int n = 0;
            Tile top = null;
            for (int j = 0; j <= 3; j++)
            {
                if (Rows[i].Cells[j].tile != null)
                {
                    Rows[i].Cells[j].oldValue = Rows[i].Cells[j].tile.value;  
                    Rows[i].Cells[j].tile.canMerge = true;
                    if (q.Count != 0)
                    {
                        if (Rows[i].Cells[j].tile.value == top.value && top.canMerge == true)
                        {
                            top.value *= 2;
                            maxValue = Mathf.Max(maxValue, top.value);
                            UpdateTextScore(top.value);
                            top.ChangeState();
                            Rows[i].Cells[j].tile.canMerge = false;
                        }
                    }
                    top = Rows[i].Cells[j].tile;
                    q.Enqueue(j);
                }
                else
                {
                    Rows[i].Cells[j].oldValue = 0;
                }
            }
            while (q.Count > 0) {

                int idx = q.Dequeue();
                if (Rows[i].Cells[idx].tile.canMerge)
                {
                    Rows[i].Cells[idx].tile.parent = Rows[i].Cells[n];
                    Rows[i].Cells[idx].tile.index = new Vector2Int(i, n);
                    Rows[i].Cells[idx].tile.Moving();
                    if (idx != n)
                    {
                        Rows[i].Cells[idx].tile = null;
                    }
                    n++;
                }
                else
                {
                    Rows[i].Cells[idx].tile.index = new Vector2Int(i, n - 1);
                    Rows[i].Cells[idx].tile.Moving();
                    Tile a = Rows[i].Cells[idx].tile;
                    Rows[i].Cells[idx].tile = null;
                    poolTile.Add(a);
                    a.gameObject.SetActive(false);
                }
            }

        }
        StartCoroutine(SpawnTile());
    }

    public void MoveUp()
    {
        for (int i = 0; i <= 3; i++)
        {
            Queue<int> q = new Queue<int>();
            Tile top = null;
            for (int j = 0; j <= 3; j++)
            {
                if (Rows[j].Cells[i].tile != null)
                {
                    Rows[j].Cells[i].oldValue = Rows[j].Cells[i].tile.value;
                    Rows[j].Cells[i].tile.canMerge = true;
                    if (q.Count != 0)
                    {
                        if (Rows[j].Cells[i].tile.value == top.value && top.canMerge == true)
                        {
                            top.value *= 2;
                            maxValue = Mathf.Max(maxValue, top.value);
                            UpdateTextScore(top.value);
                            top.ChangeState();
                            Rows[j].Cells[i].tile.canMerge = false;
                        }
                    }
                    top = Rows[j].Cells[i].tile;
                    q.Enqueue(j);
                }
                else
                {
                    Rows[j].Cells[i].oldValue = 0;
                }
            }
            int n = 0;
            while (q.Count > 0)
            {
                int idx = q.Dequeue();
                if (Rows[idx].Cells[i].tile.canMerge)
                {
                    Rows[idx].Cells[i].tile.parent = Rows[n].Cells[i];
                    Rows[idx].Cells[i].tile.index = new Vector2Int(n, i);
                    Rows[idx].Cells[i].tile.Moving();
                    if (idx != n)
                    {
                        Rows[idx].Cells[i].tile = null;
                    }
                    n++;
                }
                else
                {
                    Rows[idx].Cells[i].tile.index = new Vector2Int(n - 1, i);
                    Rows[idx].Cells[i].tile.Moving();
                    Tile a = Rows[idx].Cells[i].tile;
                    Rows[idx].Cells[i].tile = null;
                    poolTile.Add(a);
                    a.gameObject.SetActive(false);
                }
            }
        }
        StartCoroutine(SpawnTile());
    }

    public void MoveDown()
    {
        for (int i = 0; i <= 3; i++)
        {
            Queue<int> q = new Queue<int>();
            Tile top = null;
            for (int j = 3; j >= 0; j--)
            {
                if (Rows[j].Cells[i].tile != null)
                {
                    Rows[j].Cells[i].oldValue = Rows[j].Cells[i].tile.value;
                    Rows[j].Cells[i].tile.canMerge = true;
                    if (q.Count != 0)
                    {
                        if (Rows[j].Cells[i].tile.value == top.value && top.canMerge == true)
                        {
                            top.value *= 2;
                            maxValue = Mathf.Max(maxValue, top.value);
                            UpdateTextScore(top.value);
                            top.ChangeState();
                            Rows[j].Cells[i].tile.canMerge = false;
                        }
                    }
                    top = Rows[j].Cells[i].tile;
                    q.Enqueue(j);
                }
                else
                {
                    Rows[j].Cells[i].oldValue = 0;
                }
            }
            int n = 3;
            while (q.Count > 0)
            {
                int idx = q.Dequeue();
                if (Rows[idx].Cells[i].tile.canMerge)
                {
                    Rows[idx].Cells[i].tile.parent = Rows[n].Cells[i];
                    Rows[idx].Cells[i].tile.index = new Vector2Int(n, i);
                    Rows[idx].Cells[i].tile.Moving();
                    if (idx != n)
                    {
                        Rows[idx].Cells[i].tile = null;
                    }
                    n--;
                }
                else
                {

                    Rows[idx].Cells[i].tile.index = new Vector2Int(n + 1, i);
                    Rows[idx].Cells[i].tile.Moving();
                    Tile a = Rows[idx].Cells[i].tile;
                    Rows[idx].Cells[i].tile = null;
                    poolTile.Add(a);
                    a.gameObject.SetActive(false);
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
                int value = Random.Range(1, 3);
                Tile t = CreateTile().GetComponent<Tile>();
                t.gameObject.SetActive(true);
                t.parent = c;
                c.tile = t;
                t.transform.SetParent(c.transform);
                t.transform.position = c.transform.position;
                t.value = value * 2;
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
                    UnityEngine.Debug.Log("Game Over");
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
        foreach(Row r in Rows)
        {
            foreach(Cell c in r.Cells)
            {
                c.Back();   
            }
        }
        UpdateTextScore(-turnScore);
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
