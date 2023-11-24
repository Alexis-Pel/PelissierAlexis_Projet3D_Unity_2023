using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyFollowScript : AbstractEnemyScript
{
    public override void OnStart()
    {
        return;
    }

    public override void OnTimer()
    {
        return;
    }

    // Follower Logic
    override public void OnUpdate()
    {
        RotateToPlayer();
        MoveToPlayer();
    }
}
