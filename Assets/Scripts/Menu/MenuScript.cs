using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource AudioSource;

    [SerializeField]
    private AudioClip[] musics;

    [SerializeField]
    private TMP_Text score_text;

    [SerializeField]
    private TMP_Text time_text;

    private void Awake()
    {
        int index = Random.Range(0, musics.Length);
        AudioSource.PlayOneShot(musics[index]);
        score_text.text = string.Format("{0}", Score.PlayerPrefScore);
        float time = TimeScript.PlayerPrefTime;
        string text;
        if (time == 0)
            text = "No Win";
        else
            text = TimeScript.GetFormattedTime(TimeScript.PlayerPrefTime);
        time_text.text = text;
    }

    public void OnStartButton()
    {
        // Load game scene
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
    public void OnQuitButton()
    {
        // Quit Application
        Application.Quit();
    }

    public void OnSettingsButton()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }
}
