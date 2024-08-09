using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreLoader : MonoBehaviour
{
    [SerializeField] TextMeshPro HighscoreText;

    private void Awake()
    {
        HighscoreText.SetText("High Score: " + PlayerPrefs.GetInt("Highscore").ToString());
    }
}
