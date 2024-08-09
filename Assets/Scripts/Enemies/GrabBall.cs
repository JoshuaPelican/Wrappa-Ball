using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBall : EnemyBehaviour
{
    [SerializeField] float Speed = 5f;
    [SerializeField] float DestinationReachedThreshold = 0.1f;
    [SerializeField] Vector2 PauseDurationRange = new Vector2(5, 7);

    Transform ballTransform;
    Rigidbody2D ball;

    bool isGrabbing;
    bool isPursuing = true;
    bool isPaused;
    Vector2 currentDestination = new Vector2(-9999, -9999);

    private void OnDestroy()
    {
        if (isGrabbing)
            ballTransform.parent = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGrabbing)
            return;

        if (isPaused)
            return;

        if (!collision.CompareTag("Ball"))
            return;

        if (!collision.TryGetComponent(out ball))
            return;

        Grab();
    }

    protected override void Init()
    {
        base.Init();

        ballTransform = GameObject.FindWithTag("Ball").transform;
    }

    protected override void Move()
    {
        if (isPaused)
            return;

        if (isPursuing)
            FindBall();

        Vector2 direction = Vector2.Lerp(transform.up, (currentDestination - rig.position).normalized, 0.75f);
        Vector2 movement = Speed * Time.deltaTime * direction;

        rig.MovePosition(rig.position + movement);
        transform.up = direction;

        if (Vector2.Distance(currentDestination, rig.position) > DestinationReachedThreshold)
            return;

        Drop();
        Invoke(nameof(StartPursuit), Random.Range(PauseDurationRange.x, PauseDurationRange.y));
    }

    void ChooseDestination()
    {
        Vector2 cameraWorldBounds = CameraUtility.CameraWorldBounds(transform.localScale);

        currentDestination = new Vector2(Random.Range(-cameraWorldBounds.x, cameraWorldBounds.x), Random.Range(-cameraWorldBounds.y, cameraWorldBounds.y));
    }

    void StartPursuit()
    {
        isPursuing = true;
        isPaused = false;
    }

    void FindBall()
    {
        currentDestination = ballTransform.position;
    }

    void Grab()
    {
        ball.isKinematic = true;
        ball.velocity = Vector2.zero;
        ball.simulated = false;
        ballTransform.parent = this.transform;

        isGrabbing = true;
        isPursuing = false;

        ChooseDestination();
    }

    void Drop()
    {
        ball.isKinematic = false;
        ballTransform.parent = null;
        ball.simulated = true;

        isGrabbing = false;
        isPaused = true;
    }
}
