using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 0.1f;

    public Vector3 direction;
    private bool isReflecting = false;
    public bool canFF = false;
    private bool active = false;

    public Material material_ff;
    public MeshRenderer child_renderer;
    private Material material_origin;

    public Rigidbody rb;

    private void Start()
    {
        //transform.position = new Vector3(transform.position.x, 1.05f, transform.position.z);
        direction = Vector3.forward;
        material_origin = child_renderer.material;
        Invoke(nameof(Kill), 3.5f);
        Invoke(nameof(setActive), 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        rb.velocity = m_speed * transform.forward;
    }

    // On trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.CompareTag("Shield"))
            {
                isReflecting = false;
                ReflectBullet(other, material_ff);
                canFF = true;
            }
            else if (other.CompareTag("Player"))
            {
                if (!canFF)
                {
                    PlayerScript player = other.GetComponent<PlayerScript>();
                    player.takeBullet();
                    Kill();
                }
            }
            else if (other.CompareTag("Enemy"))
            {
                if (canFF)
                {
                    AbstractEnemyScript enemy = other.GetComponent<AbstractEnemyScript>();
                    enemy.PreKill();
                    enemy.SendScore(1);
                    Kill();
                }
            }
            else if (other.CompareTag("EnemyShield"))
            {
                isReflecting = false;
                ReflectBullet(other, material_origin);
            }
        }

    }
    // Reflect Bullet
    private void ReflectBullet(Collider other, Material material)
    {
        if (!isReflecting)
        {
            isReflecting = true;

            child_renderer.material = material;

            Vector3 normal = (other.transform.position - transform.position).normalized;

            Vector3 deviationDirection = Vector3.Reflect(transform.forward.normalized, normal);

            transform.forward = deviationDirection;

            other.GetComponent<ShieldScript>().Reflection(false);
        }
    }

    // Kill
    public void Kill()
    {
        CancelInvoke();
        Destroy(gameObject);
    }

    private void setActive()
    {
        active = true;
    }
}
