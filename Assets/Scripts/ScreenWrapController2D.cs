using System.Collections;
using UnityEngine;

public class ScreenWrapController2D : MonoBehaviour
{
    [SerializeField] float wrapDelay = 0.1f;
    [SerializeField] float maxVelocity = 5f;

    [SerializeField] SpawnManager AFKSpawnManager;

    Rigidbody2D rig;

    static Vector2 screenWorldBounds;

    float myRadius;
    bool isWrapping;

    float idleTimer = 60f;

    ScreenWrapData MyWrapData =>
        new ScreenWrapData(
        rig.position.x - myRadius > screenWorldBounds.x,
        rig.position.x + myRadius < -screenWorldBounds.x,
        rig.position.y - myRadius > screenWorldBounds.y,
        rig.position.y + myRadius < -screenWorldBounds.y);

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        screenWorldBounds = CameraUtility.CameraWorldBounds(0.25f * Vector2.one);
    }

    private void Update()
    {
        if(!isWrapping && MyWrapData.Offscreen)
            StartCoroutine(WrapScreen());


        if (Mathf.Abs(rig.velocity.x) <= 0.01f)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0)
            {
                AFKSpawnManager.SpawnAFKKiller();
                idleTimer = 60f;
            }

        }
        else
            idleTimer = 60f;
    }

    private void FixedUpdate()
    {
        rig.velocity = Vector2.ClampMagnitude(rig.velocity, maxVelocity);
    }

    void Init()
    {
        rig = GetComponent<Rigidbody2D>();

        myRadius = transform.localScale.magnitude / 2;
    }

    IEnumerator WrapScreen()
    {
        isWrapping = true;

        Vector2 savedVelocity = rig.velocity;
        float savedAngularVelocity = rig.angularVelocity;
        rig.simulated = false;

        yield return new WaitForSeconds(wrapDelay);

        rig.simulated = true;
        rig.velocity = savedVelocity;
        rig.angularVelocity = savedAngularVelocity;

        FlipPosition();

        isWrapping = false;
    }

    void FlipPosition()
    {
        Vector2 newPosition = rig.position;

        if (MyWrapData.Left) newPosition.x = screenWorldBounds.x + myRadius;
        else if (MyWrapData.Right) newPosition.x = -screenWorldBounds.x - myRadius;

        if (MyWrapData.Bottom) newPosition.y = screenWorldBounds.y + myRadius;
        else if (MyWrapData.Top) newPosition.y = -screenWorldBounds.y - myRadius;

        rig.position = newPosition;
    }

    public struct ScreenWrapData 
    {
        public bool Offscreen;
        public bool Left;
        public bool Right;
        public bool Top;
        public bool Bottom;

        public ScreenWrapData(bool right, bool left, bool top, bool bottom) //True is offscreen
        {
            Right = right;
            Left = left;
            Top = top;
            Bottom = bottom;

            Offscreen = left || right || top || bottom;
               
        }
    }
}
