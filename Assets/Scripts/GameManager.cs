using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Audio;

using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private TMP_Text m_ScoreText;

    [SerializeField]
    private AudioClip[] m_enemyDiesEffect;

    [SerializeField]
    private AudioClip[] m_musics;

    [SerializeField]
    private AudioSource m_audioSource;

    [SerializeField]
    private AudioSource m_musicSource;

    [SerializeField]
    private GameObject m_GameOverScreen;

    [SerializeField]
    private GameObject m_WinScreen;

    [SerializeField]
    private AudioMixer m_audioMixer;

    [SerializeField]
    private GameObject m_Player;

    [SerializeField]
    private WaveManager m_waveManager;

    private TimeScript timer;
    private bool setLowPass = false;
    private float lowPass = 22000f;
    private int _score;
    private bool _isPlaying = true;
    public TMP_Text time_text;

    static public bool winner = false;

    static private bool isSpecialWave = false;

    private void Awake()
    {
        m_waveManager.enabled = true;
        m_audioMixer.SetFloat("LowPass", 22000f);
    }

    private void FixedUpdate()
    {
        if (!m_musicSource.isPlaying)
        {
            playMusic();
        }
        if (setLowPass)
        {
            lowPass -= 800f;
            if(lowPass <= 500)
            {
                lowPass = 500f;
                setLowPass = false;
            }
            m_audioMixer.SetFloat("LowPass", lowPass);
        }
        if(winner)
        {
            GameOver();
            winner = false;
        }
    }

    private void playMusic()
    {
        int index = Random.Range(0, m_musics.Length);
        m_musicSource.PlayOneShot(m_musics[index]);
    }

    private void Start()
    {
        // Screen.SetResolution(640, 480, FullScreenMode.ExclusiveFullScreen, new RefreshRate() { numerator = 30, denominator = 1 });
        playMusic();
        timer = gameObject.AddComponent<TimeScript>();
        timer.StartTimer();
    }

    public int ScoreGetSet
    {
        get { return _score; }
        set
        {
            _score = value;
            m_ScoreText.text = string.Format("{0}", _score);
        }
    }

    /// <summary>
    /// The game is over
    /// </summary>
    public void GameOver()
    {
        setLowPass = true;
        //TODO : Stop playing
        _isPlaying = false;
        if (m_waveManager)
        {
            m_waveManager.CancelInvoke();
            Destroy(m_waveManager);
        }

        // Check Highscore
        float highscore;
        if (GameSettings.infiniteGameMode)
        {
            highscore = Score.PlayerPrefScoreInfinite;

        }
        else
        {
            highscore = Score.PlayerPrefScore;
        }

        if ((_score > highscore) || highscore == 0)
            if(GameSettings.infiniteGameMode)
                Score.PlayerPrefScoreInfinite = _score;
            else
                Score.PlayerPrefScore = _score;

        if (winner)
        {
            timer.StopTimer();
            float bestTime = TimeScript.PlayerPrefTime;
            if((timer.TimeGetSet < bestTime) || bestTime == 0)
            {
                TimeScript.PlayerPrefTime = timer.TimeGetSet;
            }
            Invoke(nameof(ShowWinScreen), 1f);
        }
        else
        {
            Invoke(nameof(ShowGameOverScreen), 1f);

        }
    }

    static public bool IsEvent
    {
        get { return isSpecialWave; }
        set { isSpecialWave = value; }
    }

    /// <summary>
    /// Show game over screen
    /// </summary>
    public void ShowGameOverScreen()
    {
        // Show Game Over screen
        m_GameOverScreen.SetActive(true);
    }

    /// <summary>
    /// Show win screen
    /// </summary>
    private void ShowWinScreen()
    {
        time_text.text = TimeScript.GetFormattedTime(timer.TimeGetSet);
        // Show Win screen
        m_WinScreen.SetActive(true);
    }

    /// <summary>
    /// Reload scene
    /// </summary>
    public void TryAgain()
    {
        // Reload scene
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    /// <summary>
    /// Go to menu
    /// </summary>
    public void Menu()
    {
        // Load menu
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    /// <summary>
    /// Play audio
    /// </summary>
    /// <param name="clip"></param>
    public void PlayerAudio(AudioClip clip)
    {
        if (!m_audioSource.isPlaying)
        {
            m_audioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Play enemy dies audio
    /// </summary>
    public void EnemyDiesAudio()
    {
        PlayerAudio(m_enemyDiesEffect[Random.Range(0, m_enemyDiesEffect.Length)]);
    }

}
