using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private Transform m_Target;

    [SerializeField]
    private float m_Speed = 0f;

    [SerializeField]
    private Vector3 m_Offset;

    // Desired duration of the shake effect
    [SerializeField]
    public float desiredShakeDuration = 1.0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    [SerializeField]
    public float shakeMagnitude = 0.1f;

    // A measure of how quickly the shake effect should evaporate
    [SerializeField]
    public float dampingSpeed = 1.0f;

    private float shakeDuration = 0f;

    [SerializeField]
    private Vector3 initPos;


    public void TriggerShake()
    {
        shakeDuration = desiredShakeDuration;
    }

    public void ResetSettings()
    {
        shakeMagnitude = 0.2f;
        desiredShakeDuration = 1f;
        dampingSpeed = 1f;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.position = initPos + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.position = initPos;
        }
    }
}