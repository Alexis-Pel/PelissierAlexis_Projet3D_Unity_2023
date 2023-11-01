using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Data/Waves/New Wave")]
public class WaveScriptable : ScriptableObject
{
    // Wave Info
    public string m_waveName;

    // Enemies info
    public List<AbstractEnemyScript> ennemies;
    public float enemyRate;
}
