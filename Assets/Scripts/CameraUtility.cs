using UnityEngine;

public class CameraUtility : MonoBehaviour
{
    public static Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public static Vector2 CameraWorldBounds(Vector2 padding)
    {
        return (Vector2)mainCam.ScreenToWorldPoint(new Vector2(mainCam.pixelWidth, mainCam.pixelHeight)) - padding;
    }

    public static Vector2 RandomCameraPosition(Vector2 padding)
    {
        Vector2 cameraWorldBounds = CameraWorldBounds(padding);

        return new Vector2(Random.Range(-cameraWorldBounds.x, cameraWorldBounds.x), Random.Range(-cameraWorldBounds.y, cameraWorldBounds.y));
    }
}
