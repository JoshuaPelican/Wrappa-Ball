using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected Rigidbody2D rig;

    private void Awake()
    {
        Init();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();


    protected virtual void Init()
    {
        rig = GetComponent<Rigidbody2D>();
    }
}
