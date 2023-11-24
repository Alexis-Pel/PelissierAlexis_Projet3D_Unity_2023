using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyShooterScript : AbstractEnemyScript
{
    [SerializeField]
    private BulletScript m_prefab;

    [SerializeField]
    private AudioClip m_shootEffect;

    // Shooter Logic
    override public void OnUpdate()
    {
        RotateToPlayer();
        MoveToPlayer();
    }

    private void Shoot()
    {
        BulletScript b = Instantiate(m_prefab, transform);
        b.transform.localPosition = new Vector3(0, 0, 0);
        b.transform.parent = null;

        Instance.PlayerAudio(m_shootEffect);
        //transform.DetachChildren();
    }

    public override void OnTimer()
    {
    }

    public override void OnStart()
    {
        InvokeRepeating(nameof(Shoot), m_shootPeriod, m_shootPeriod);
    }
}
