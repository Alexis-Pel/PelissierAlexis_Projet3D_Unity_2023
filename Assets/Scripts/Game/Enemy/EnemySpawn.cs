using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private AbstractEnemyScript m_Prefab;

    [SerializeField]
    private float m_Width;

    [SerializeField]
    private bool horizontal;

    private GameObject m_Player;

    private GameManager gm;

    private void Awake()
    {
        m_Player = FindFirstObjectByType<PlayerScript>().gameObject;
        gm = FindFirstObjectByType<GameManager>();
    }

    /// <summary>
    /// Do spawn an enemy on a spawner
    /// </summary>
    public void Spawn()
    {
        AbstractEnemyScript instance = Instantiate(m_Prefab, transform);
        instance.PlayerGetSet = m_Player;
        instance.SetGameManager(gm);

        if (horizontal)
        {
            instance.transform.localPosition = new Vector3(Random.Range(transform.position.x - (m_Width / 2), transform.position.x + (m_Width / 2)), 0f, 0f);
        }
        else
        {
            instance.transform.localPosition = new Vector3(0f, 0f, Random.Range(transform.position.z - (m_Width / 2), transform.position.z + (m_Width / 2)));
        }

    }

    /// <summary>
    /// Draw gizmo in editor
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Vector3 vector;
        if (horizontal)
        {
            vector = new Vector3(m_Width * 0.5f, 0, 0);
        }
        else
        {
            vector = new Vector3(0, 0, m_Width * 0.5f);

        }
        Gizmos.DrawLine(transform.position - vector, transform.position + vector);
    }

    public void SetPrefab(AbstractEnemyScript enemy)
    {
        m_Prefab = enemy;
    }
}
