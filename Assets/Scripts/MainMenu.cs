using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;

    //energy system variables
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration;
    [SerializeField] private Button playButton;
    private int energy;

    private const string energyKey = "Energy";
    private const string energyReadyKey = "EnergyReady";

    //android notification

    [SerializeField] AndroidNotificationHandler androidNotificationHandler;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt(ScoreHandler.highScoreKey, 0);
        highScoreText.text = $"High Score: {highScore}";

        //energy system
        energy = PlayerPrefs.GetInt(energyKey, maxEnergy);//Default value max energy
        if (energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(energyReadyKey, string.Empty);
            if (energyReadyString == string.Empty) { return; }

            DateTime energyReady = DateTime.Parse(energyReadyString);
            if (DateTime.Now > energyReady)
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(energyKey, energy);
#if UNITY_ANDROID
                androidNotificationHandler.ScheduleNotification(energyReady);
#endif
            }
            else
            {
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);

            }
        }
        energyText.text = $"Play ({energy})";
    }

    void EnergyRecharged()
    {
        playButton.interactable = true;
        energy = maxEnergy;
        PlayerPrefs.SetInt(energyKey, energy);
        energyText.text = $"Play ({energy})";
    }
    public void Play()
    {
        if (energy < 1) { return; }
        energy--;
        PlayerPrefs.SetInt(energyKey, energy);

        if (energy == 0)
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(energyReadyKey, energyReady.ToString());


        }

        SceneManager.LoadScene(1);
    }


}
