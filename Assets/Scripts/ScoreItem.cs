using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] Image EnemyImage;
    [SerializeField] EnemyData Enemy;
    [SerializeField] bool IsElite;

    private void Start()
    {

        if(Enemy == null && !IsElite)
        {
            ScoreText.SetText($"{GameManager.GameTime.ToString("F0")} X {10} = {Mathf.FloorToInt(GameManager.GameTime) * 10}");
            return;
        }

        if (Enemy == null && IsElite)
        {
            ScoreText.SetText($"Final Score: {GameManager.Score}");
            return;
        }

        string enemyString = Enemy.name + (IsElite ? "E" : "");
        int KillCount = 0;
        if (GameManager.EnemyKillCounts.ContainsKey(enemyString))
            KillCount = GameManager.EnemyKillCounts[enemyString];

        SpriteRenderer rend = Enemy.EnemyPrefab.GetComponentInChildren<SpriteRenderer>();
        EnemyImage.sprite = rend.sprite;
        EnemyImage.color = rend.color;

        if (IsElite) EnemyImage.transform.localScale *= 2;
        ScoreText.SetText($"{KillCount} X {(IsElite ? Enemy.Score * 3 : Enemy.Score)} = {KillCount * (IsElite ? Enemy.Score * 3 : Enemy.Score)}");
    }
}
