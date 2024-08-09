using UnityEngine;

public class RandomRoam : EnemyBehaviour
{
    [SerializeField] float Speed = 2f;
    [SerializeField] float DestinationReachedThreshold = 0.1f;
    [SerializeField] Vector2 PauseDurationRange = new Vector2(2, 5);

    Vector2 currentDestination;

    bool isPaused;

    protected override void Init()
    {
        base.Init();

        ChooseDestination();
    }

    protected override void Move()
    {
        if (isPaused)
            return;

        Vector2 direction = (currentDestination - rig.position).normalized;
        Vector2 movement = Speed * Time.deltaTime * direction;

        rig.MovePosition(rig.position + movement);

        if (Vector2.Distance(currentDestination, rig.position) > DestinationReachedThreshold)
            return;

        Invoke(nameof(ChooseDestination), Random.Range(PauseDurationRange.x, PauseDurationRange.y));
        isPaused = true;
    }

    void ChooseDestination()
    {

        Vector2 cameraWorldBounds = CameraUtility.CameraWorldBounds(transform.localScale);

        currentDestination = new Vector2(Random.Range(-cameraWorldBounds.x, cameraWorldBounds.x), Random.Range(-cameraWorldBounds.y, cameraWorldBounds.y));

        isPaused = false;
    }
}
