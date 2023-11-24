using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class AbstractEnemyScript : MonoBehaviour
{
    [SerializeField]
    private string m_name;

    [SerializeField]
    private float m_speed;

    [SerializeField]
    private float m_rotationSpeed;

    private Collider _collider;

    // Effect
    public ParticleSystem _particle;
    public AudioSource _audioSource;

    // GameObjects
    public GameManager Instance;
    private WaveManager waveManager;
    private CameraController _camera;

    //TODO: Private
    public GameObject m_player;

    // Settings
    public bool _isAlive = true;
    private float m_timer = 0f;
    public int m_ScorePoint;
    public float m_shootPeriod = 1.3f;



    public Vector3 direction;


    // Start is called before the first frame update
    void Start()
    {
        waveManager = FindFirstObjectByType<WaveManager>();
        _collider = GetComponent<SphereCollider>();
        if (!_collider)
            _collider = GetComponent<MeshCollider>();
        _camera = Camera.main.GetComponent<CameraController>();
        OnStart();
        //_particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_isAlive)
        {
            m_timer += Time.deltaTime;
            if(m_timer >= 1)
            {
                OnTimer();
                m_timer = 0;
            }
            // Calculate the direction from the enemy to the player.
            if (m_player)
            {
                direction = m_player.transform.position - transform.position;
            }
            // Normalize the direction vector to make it unit length.
            direction.Normalize();

            OnUpdate();
        }
    }

    /// <summary>
    /// Abstract function called each second
    /// </summary>
    abstract public void OnTimer();

    /// <summary>
    /// Abstract function called each frame
    /// </summary>
    abstract public void OnUpdate();

    /// <summary>
    /// Abstract function called on start
    /// </summary>
    abstract public void OnStart();

    /// <summary>
    /// On collision enter
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (_isAlive)
        {
            if (collision.collider != null)
            {
                // If an ennemy touch the player
                PlayerScript player = collision.collider.GetComponent<PlayerScript>();
                if (player != null)
                {
                    // If the player is dashing
                    if (player._isDashing)
                    {
                        if (player.combo > 0)
                            SendScore(player.combo);
                        SendScore(1);
                    }
                    else
                    {
                        player.preKill();
                    }
                    PreKill();
                }
            }
        }
    }

    /// <summary>
    /// Send score to game manager
    /// </summary>
    public void SendScore(int multiplier)
    {
        // Set score
        int score = Instance.ScoreGetSet;
        Instance.ScoreGetSet = score + (m_ScorePoint * multiplier);
    }

    /// <summary>
    /// Pre Kill
    /// </summary>
    public void PreKill()
    {
        CancelInvoke();
        _isAlive = false;
        CameraShake();

        waveManager._remainingEnemies -= 1;
        try
        {
            Destroy(GetComponent<ShieldScript>());
        }
        catch (System.Exception ex)
        {
            print(ex);
        }
        _particle.Play();
        transform.DOScale(0, 0.4f).SetEase(Ease.InElastic).OnComplete(() => Instance.EnemyDiesAudio()) ;

        Invoke(nameof(Kill), 1.5f);
    }

    private void CameraShake()
    {
        _collider.enabled = false;
        _camera.shakeMagnitude = 0.3f;
        _camera.desiredShakeDuration = 0.1f;
        _camera.TriggerShake();
        _camera.ResetSettings();
    }

    private void Kill()
    {
        // Play Audio
        Destroy(gameObject);

    }

    public GameObject PlayerGetSet
    {
        get { return m_player; }
        set { m_player = value; }
    }

    public void SetGameManager(GameManager gm)
    {
        Instance = gm;
    }

    /// <summary>
    /// Move the enemy to the player
    /// </summary>
    public void MoveToPlayer()
    {
        if (m_player)
        {
            // Move the enemy towards the player.
            transform.Translate(m_speed * Time.deltaTime * Vector3.forward);
        }
    }

    /// <summary>
    /// Rotate the enemy to the player
    /// </summary>
    public void RotateToPlayer()
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        transform.DORotateQuaternion(targetRotation, m_rotationSpeed);
    }
}
