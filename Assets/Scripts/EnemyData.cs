using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public GameObject EnemyPrefab;
    public int CreditCost;
    public int Score;
    public int Weight;
    public int TimeRequired;
}
