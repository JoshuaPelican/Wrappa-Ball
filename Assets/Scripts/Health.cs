using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] int MaxHealth;
    [SerializeField] Faction DamagedBy;
    [SerializeField] int ExtraDamageTaken;
    [SerializeField] AudioClip[] DeathClips;

    public UnityEvent OnDie = new UnityEvent();

    int currentHealth;
    EnemyData enemy;
    bool isElite;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        GetComponent<Impact2D>().OnImpact.AddListener(ProcessImpact);

        ResetHealth();
    }

    void ResetHealth()
    {
        currentHealth = MaxHealth;
    }

    void ProcessImpact(ImpactData impact)
    {
        if (impact.Faction != DamagedBy)
            return;

        TakeDamage(impact.ImpactTier);
    }

    public void DoubleHealth()
    {
        currentHealth *= 2;
    }

    public void SetEnemyData(EnemyData enemy, bool isElite)
    {
        this.enemy = enemy;
        this.isElite = isElite;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage + ExtraDamageTaken;
        //Debug.Log($"{this.name} took {damage + ExtraDamageTaken} damage!");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        OnDie.Invoke();

        if(DeathClips.Length > 0)
            AudioManager.PlayAudioRandom(DeathClips, 1, 1 - (MaxHealth * 0.05f));

        Destroy(gameObject);

        if (enemy == null)
            return;

        GameManager.AddScore(isElite ? enemy.Score * 3 : enemy.Score);
        GameManager.AddKillCount(enemy, isElite);
    }
}
