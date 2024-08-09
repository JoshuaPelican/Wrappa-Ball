using UnityEngine;

public abstract class ActionOnDestroy : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Health>().OnDie.AddListener(DestroyAction);
    }

    protected abstract void DestroyAction();
}
