using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO; //for seriliazation

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public static string CurrentUserName = "";
    public static string BestScoreUserName = "";
    public static int BestScoreResult = 0;

    private void Awake()
    {
        //fetch saved data from file during initialization of game
        LoadBestScoreData();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        BestScoreText.text = $"Best Score : {BestScoreUserName} : {BestScoreResult}";
        ScoreText.text = $"Score : {m_Points} : Player: {CurrentUserName}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points} : Player: {CurrentUserName}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        //TODO check best score
        if (BestScoreResult < m_Points) {
            SaveCurrentPlayerScore();
        }
    }

    [System.Serializable] //it's needed for JsonUtility
    class SaveBestScoreData
    {
        public string BestPlayerName;
        public int BestScore;
    }

    public void SaveCurrentPlayerScore()
    {
        SaveBestScoreData data = new SaveBestScoreData();
        data.BestPlayerName = CurrentUserName;
        data.BestScore = m_Points;

        string json = JsonUtility.ToJson(data);
    
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public static void LoadBestScoreData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveBestScoreData data = JsonUtility.FromJson<SaveBestScoreData>(json);

            BestScoreUserName = data.BestPlayerName;
            BestScoreResult = data.BestScore;
        }
    }
}
