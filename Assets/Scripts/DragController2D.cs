using UnityEngine;

public class DragController2D : MonoBehaviour
{
    Rigidbody2D rig;
    Camera mainCam;

    Vector2 mousePosition => mainCam.ScreenToWorldPoint(Input.mousePosition);

    Vector2 screenWorldBounds;

    private void Awake()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
            Application.targetFrameRate = 60;

        Init();
    }

    private void Start()
    {
        screenWorldBounds = CameraUtility.CameraWorldBounds(new Vector2(1, 0.5f));
    }

    private void OnMouseDrag()
    {
        Vector2 clampedPosition = mousePosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenWorldBounds.x, screenWorldBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenWorldBounds.y, screenWorldBounds.y);
        rig.MovePosition(clampedPosition);
    }

    void Init()
    {
        rig = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        rig.sleepMode = RigidbodySleepMode2D.NeverSleep; //Prevents non-moving objects from not updating rigidbody collisions!
    }
}
