using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UI;
using Kino;
using TMPro;

public class EnemyDashScript : AbstractEnemyScript
{

    [SerializeField]
    private float m_MoveSpeed;

    [SerializeField]
    private Material m_dashMaterial;

    [SerializeField]
    private GameObject m_graphic;


    private ShieldScript _shield;
    public Rigidbody _rigidBody;

    public Vector2 _movement;

    // DASH
    [SerializeField]
    private float _dashForce;
    private int timerBeforeDash = 0;

    private new MeshRenderer renderer;
    private Material baseMaterial;

    private ParticleSystem particles;
    public bool _isDashing = false;


    void Awake()
    {
        //TODO : Get rigidbody
        _rigidBody = GetComponent<Rigidbody>();
        _shield = GetComponentInChildren<ShieldScript>();
        particles = GetComponentInChildren<ParticleSystem>();
        renderer = m_graphic.GetComponent<MeshRenderer>();
        baseMaterial = renderer.material;
    }

    private void PreDash()
    {
        _isDashing = true;
        renderer.material = m_dashMaterial;
        Invoke(nameof(Dash), 0.5f);
    }

    void Dash()
    {
        // Get target Position
        Vector3 dir = _shield.GetDirection();
        Vector3 targetPosition = transform.position + dir * _dashForce;

        // Clamp target position
        targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, -8.5f, 8.5f), targetPosition.y, Mathf.Clamp(targetPosition.z, -4.5f, 4.5f));

        // Dash Cooldown
        Invoke(nameof(setDashing), 0.4f);

        // DoMove
        _rigidBody.DOMove(targetPosition, 0.3f).OnComplete(() => { timerBeforeDash = 0; renderer.material = baseMaterial; });
    }

    private void setDashing()
    {
        _isDashing = false;
    }


    public override void OnTimer()
    {
    }

    public override void OnUpdate()
    {
        if (_isAlive)
        {
            RotateToPlayer();
            if (!_isDashing)
            {
                MoveToPlayer();
                if (timerBeforeDash == 0)
                {
                    timerBeforeDash = Random.Range(1, 6);
                    Invoke(nameof(PreDash), timerBeforeDash);
                }
            }
        }

    }

    public override void OnStart()
    {

    }
}
