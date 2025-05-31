using UnityEditor;
using UnityEngine;

[System.Serializable]
public class BoardData 
{
    public int[] board = new int[ Contant.max * Contant.max];
   
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class PlayModeExitHandle
{
    static PlayModeExitHandle()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChange;
    }

    private static void OnPlayModeChange(PlayModeStateChange state)
    {
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (state == PlayModeStateChange.ExitingPlayMode && currentScene != 0)
        {
            Debug.Log("Exit Play Mode");
            SaveData();
        }
    }

    private static void SaveData()
    {
        BoardData currentBoard = new BoardData();
        int n = Contant.max;
        for (int i = 0; i < n * n; i++)
        {
            Cell c = Grid.intant.Rows[i / n].Cells[i % n];
            currentBoard.board[i] = c.tile == null ? 0 : c.tile.value;
        }
        string json = JsonUtility.ToJson(currentBoard);
        PlayerPrefs.SetString(Contant.BoardData + n.ToString(), json);
        PlayerPrefs.SetInt(Contant.Score + n.ToString(), ScoreManager.intant.score);
    }
}
#endif
