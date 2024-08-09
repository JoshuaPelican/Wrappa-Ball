using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedIndicator : MonoBehaviour
{
    [SerializeField] Slider Slider;

    Rigidbody2D ballRig;

    private void Start()
    {
        ballRig = GameObject.FindWithTag("Ball").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Slider.value = Mathf.Lerp(Slider.value, Mathf.Sqrt(ballRig.GetPointVelocity(ballRig.position).magnitude) / 15f, 0.5f);
    }
}
