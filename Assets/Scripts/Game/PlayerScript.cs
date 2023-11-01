using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UI;
using Kino;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    [SerializeField]
    private GameManager Instance;

    [SerializeField]
    private WaveManager WaveManager;

    [SerializeField]
    private float m_MoveSpeed;

    [SerializeField]
    private Scrollbar m_DashBar;

    [SerializeField]
    private TMP_Text combo_text;

    private ShieldScript _shield;
    public Rigidbody _rigidBody;

    public Vector2 _movement;

    private bool _isAlive = true;
    private float timer = 0;

    // DASH
    [SerializeField]
    private float _dashForce;
    private bool _dashCooldown = false;
    public bool _isDashing = false;
    private AnalogGlitch glitch;

    private ParticleSystem particles;
    public int combo = 0;


    void Awake()
    {
        //TODO : Get rigidbody
        _rigidBody = GetComponent<Rigidbody>();
        _shield = GetComponentInChildren<ShieldScript>();
        particles = GetComponentInChildren<ParticleSystem>();
        glitch = Camera.main.GetComponent<AnalogGlitch>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (_dashCooldown && m_DashBar.size < 1)
        {
            if (timer > 0.1)
            {
                m_DashBar.size += 0.17f;
                timer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        //TODO : Move
        _rigidBody.velocity = new Vector3(_movement.x * m_MoveSpeed, _rigidBody.velocity.y, _movement.y * m_MoveSpeed);
    }

    void OnMove(InputValue value)
    {
        if (_isAlive)
        {
            _movement = value.Get<Vector2>();
            if (WaveManager.invert)
            {
                _movement = -_movement;
            }
        }
        else
            _movement = Vector2.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isAlive)
        {
            if (collision.collider != null)
            {
                // If an ennemy touch the player
                AbstractEnemyScript enemy = collision.collider.GetComponent<AbstractEnemyScript>();
                if (enemy != null)
                {
                    combo += 1;
                    if(combo > 1)
                    {
                        string a;
                        switch (combo)
                        {
                            case 2:
                                a = "Double";
                                break;
                            case 3:
                                a = "Triple";
                                break;
                            case 4:
                                a = "Quadriple";
                                break;
                            default:
                                a = "Monster";
                                break;
                        }
                        combo_text.text = string.Format("{0} kill !!!", a);
                        combo_text.enabled = true;
                    }
                }
            }
        }
    }

    void OnDash()
    {
        if(!_dashCooldown && _isAlive)
        {
            glitch.colorDrift = 0.15f;
            glitch.scanLineJitter = 0.1f;
            // Get target Position
            Vector3 dir = _shield.GetDirection();
            Vector3 targetPosition = transform.position + dir * _dashForce;

            // Clamp target position
            targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, -8.5f, 8.5f), targetPosition.y, Mathf.Clamp(targetPosition.z, -4.5f, 4.5f));

            m_DashBar.size = 0;

            // Dash Cooldown
            _dashCooldown = true;
            _isDashing = true;
            Invoke(nameof(setCooldown), 0.6f);
            Invoke(nameof(setDashing), 0.4f);

            // DoMove
            _rigidBody.DOMove(targetPosition, 0.4f);

        }
    }

    private void SetText()
    {
        combo_text.enabled = false;
    }

    private void setCooldown()
    {
        _dashCooldown = false;
        Invoke(nameof(SetText), 0.4f);
    }

    private void setDashing()
    {
        _isDashing = false;
        combo = 0;
        glitch.colorDrift = 0f;
        glitch.scanLineJitter = 0;

    }


    public void takeBullet()
    {
        if (!_isDashing )
        {
            preKill();
        }
    }
    public void Kill()
    {
        Instance.GameOver();
        Invoke(nameof(postKill), 1f);
    }

    private void postKill()
    {
        Destroy(gameObject);
    }

    public void preKill()
    {
        if (!_isAlive)
            return;
        if (!GameManager.IsEvent)
        {
            _isAlive = false;
            _movement = Vector2.zero;
            Destroy(_shield.gameObject);
            particles.Play();
            Camera.main.GetComponent<CameraController>().TriggerShake();
            transform.DOScale(0, 0.5f).SetEase(Ease.InElastic).OnComplete(() => Kill());
        }
        else
        {
            WaveManager.StopWave();
            WaveManager.clearEnnemies();
            WaveManager.showText();
            Invoke(nameof(a), 5f);
        }
    }

    private void a()
    {
        WaveManager.newWave();
    }
}
