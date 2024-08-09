using UnityEngine;

public class ChasePaddle : EnemyBehaviour
{
    [SerializeField] float Speed = 2f;
    [SerializeField] bool FaceTarget = true;

    Transform paddle;

    private void Awake()
    {
        Init();
    }

    private void FixedUpdate()
    {
        Move();
    }


    protected override void Init()
    {
        base.Init();

        paddle = GameObject.FindWithTag("Paddle").transform;
    }

    protected override void Move()
    {
        if (!paddle)
            return;

        Vector2 direction = ((Vector2)paddle.position - rig.position).normalized;
        Vector2 movement = Speed * direction * Time.deltaTime;

        if(FaceTarget)
            transform.up = direction;

        rig.MovePosition(rig.position + movement);
    }
}
