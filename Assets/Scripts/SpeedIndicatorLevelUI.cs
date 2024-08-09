using UnityEngine;

public class SpeedIndicatorLevelUI : MonoBehaviour
{
    [SerializeField] int SpeedLevel;

    private void Awake()
    {
        transform.localScale = new Vector3(Mathf.Sqrt(Impact2D.ImpactTiers[SpeedLevel]) / 15f, transform.localScale.y, transform.localScale.z);
    }
}
