using UnityEngine;

public class ScreenExtender : MonoBehaviour
{
    private void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            Camera.main.orthographicSize *= 1.5f;
    }
}
