using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    // World
    public TMP_Text _waveText;
    public WaveScriptable[] _waves;
    public EnemySpawner[] m_spawners;

    // Settings
    private WaveScriptable _currentWave;
    private List<AbstractEnemyScript> _ennemies = new List<AbstractEnemyScript>();
    private int _currentWaveIndex = 0;
    private bool last_wave = false;

    // Var
    private bool show = true;
    private bool start_newWave = false;

    public int _remainingEnemies;
    public bool invert = false;

    
    private void Awake()
    {
        PrepareWave();
    }

    static public void clearEnnemies()
    {
        AbstractEnemyScript[] ennemies = FindObjectsOfType<AbstractEnemyScript>();
        foreach (AbstractEnemyScript item in ennemies)
        {
            item.PreKill();
        }
    }

    private void FixedUpdate()
    {
        if(_remainingEnemies == 0 && show)
        {
            if (last_wave)
            {
                GameManager.winner = true;
            }
            else
            {
                showText();
            }
        }
        if (start_newWave)
        {
            //TODO: Change wave
            Invoke(nameof(newWave), 5f);
            start_newWave = false;
        }
    }

    public void showText()
    {
        _waveText.enabled = true;
        show = false;
        Invoke(nameof(setText), 3f);
        start_newWave = true;
    }

    public void newWave()
    {
        PrepareWave();
        StartWave(_currentWave);
    }

    private void setText()
    {
        _waveText.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartWave(_currentWave);
    }

    // Start the wave
    private void StartWave(WaveScriptable wave)
    {
        InvokeRepeating(nameof(Spawn), 0f, _currentWave.enemyRate);
    }

    // Set wave index and text
    public void SetWave()
    {
        _currentWaveIndex += 1;
        _waveText.text = string.Format("Wave {0}", _waves[_currentWaveIndex].m_waveName);
    }

    // Prepare the wave
    private void PrepareWave()
    {
        _currentWave = _waves[_currentWaveIndex];
        foreach (var item in _currentWave.ennemies)
        {
            _ennemies.Add(item);
        }
        _remainingEnemies = _ennemies.Count;
        show = true;

        if (_currentWaveIndex + 1 == _waves.Length)
        {
            last_wave = true;
        }
        else
        {
            last_wave = false;
        }

        // Special Waves
        switch (_currentWave.m_waveName)
        {
            case "Special":
                StartEvent();
                break;
            default:
                EndEvent();
                break;
        }
    }

    private void StartEvent()
    {
        GameManager.IsEvent = true;
        string e = EventScript.StartEvent();

        switch (e)
        {
            case "Invert":
                invert = true;
                break;
            default:
                break;
        }
    }

    private void EndEvent()
    {
        invert = false;
        GameManager.IsEvent = false;
    }

    // Spawn enemy
    private void Spawn()
    {
        int index = Random.Range(0, m_spawners.Length);
        int index_enemy = Random.Range(0, _ennemies.Count);
        AbstractEnemyScript e = _ennemies[index_enemy];
        if(e.name == "Enemy_Shoot")
        {
            e.m_shootPeriod -= _currentWaveIndex / 10;
        }
        m_spawners[index].SetPrefab(e);
        m_spawners[index].Spawn();
        _ennemies.RemoveAt(index_enemy);
        /**
        if(_ennemies.Count < 5)
        {

        }
        **/
        if(_ennemies.Count == 0)
        {
            StopWave();
        }

    }

    // Stop Wave
    public void StopWave()
    {
        // Cancel Spawn
        CancelInvoke(nameof(Spawn));
        if (!last_wave)
        {
            //Change Wave Text
            SetWave();
        }
    }
}
