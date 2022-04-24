using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private float scoreMultiplier = 5f;
    private float score;

    public const string highScoreKey = "HighScore";
    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * scoreMultiplier;
        scoreText.text =Mathf.FloorToInt(score).ToString();
    }

    private void OnDestroy()
    {

        int currentHighScore=PlayerPrefs.GetInt(highScoreKey, 0);

        if (score>currentHighScore)
        {
            PlayerPrefs.SetInt(highScoreKey, Mathf.FloorToInt(score));
        }
    }
}
