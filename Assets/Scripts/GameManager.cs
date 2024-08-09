using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static float GameTime;
    public static int Score;

    float scoreTimer;
    bool gameOver;

    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject EndScreen;

    public static Dictionary<string, int> EnemyKillCounts;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        GameTime += Time.deltaTime;

        scoreTimer -= Time.deltaTime;
        if(scoreTimer <= 0)
        {
            AddScore(10);
            scoreTimer = 1f;
        }

        ScoreText.SetText(Score.ToString());

        if (gameOver)
        {
            if (Input.anyKeyDown)
                LoadMenu();
        }
    }

    void Init()
    {
        GameObject.FindWithTag("Paddle").GetComponent<Health>().OnDie.AddListener(GameOver);

        EnemyKillCounts = new Dictionary<string, int>();
        Score = 0;
        GameTime = 0;

        scoreTimer = 1f;
    }

    void GameOver()
    {
        if (PlayerPrefs.GetInt("Highscore") < Score)
            PlayerPrefs.SetInt("Highscore", Score);

        //Display end screen
        EndScreen.SetActive(true);
        HUD.SetActive(false);

        gameOver = true;
    }

    void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public static void AddScore(int score)
    {
        Score += score;
    }

    public static void AddKillCount(EnemyData data, bool isElite)
    {
        string enemyString = data.name + (isElite ? "E" : "");

        if (!EnemyKillCounts.ContainsKey(enemyString))
            EnemyKillCounts.Add(enemyString, 1);
        else
            EnemyKillCounts[enemyString]++;
    }
}
