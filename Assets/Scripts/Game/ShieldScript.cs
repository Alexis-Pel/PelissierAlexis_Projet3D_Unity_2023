using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class ShieldScript : MonoBehaviour
{
    public Transform player;  // Reference to the player GameObject.
    public float orbitDistance = 1.5f;  // Desired orbit distance.
    public float rotationSpeed; // Rotation speed in degrees per second.
    public bool isPlayer;
    public Material material;

    private Vector3 direction;
    // private bool stop = false;

    private void Start()
    {
       material.SetColor("_EmissionColor", new Color(255f, 255f, 255f) * 0.006f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition;
        if (isPlayer)
        {
            targetPosition = MouseToWorldPoint(Camera.main, 10f);
            direction = targetPosition - player.position;
        }
        else
        {
            direction = transform.forward;
        }

        // Calculate the direction from the player to the target position.
        direction.y = 0;
        direction = Vector3.Normalize(direction);
        Quaternion targetRotation = TargetRotation(direction);

        // Set the position based on the desired orbit distance.
        transform.SetPositionAndRotation(player.position + direction * orbitDistance, Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed));
    }

    /// <summary>
    /// Get the mouse position as a point of the world
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="distance">distance from the camera to the point</param>
    /// <returns></returns>
    private Vector3 MouseToWorldPoint(Camera cam, float distance)
    {
        var m = Input.mousePosition;
        m.z = distance;
        return cam.ScreenToWorldPoint(m);
    }

    /// <summary>
    /// Get the target rotation of the shield
    /// </summary>
    /// <param name="direction"></param>
    /// <returns> targetRotation </returns>
    private Quaternion TargetRotation(Vector3 direction)
    {
        // Rotate the GameObject towards the target.
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation.x = 0;
        targetRotation.z = 0;
        return targetRotation;
    }

    /// <summary>
    /// Getter for direction
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDirection()
    {
        return direction;
    }

    /// <summary>
    /// Reflection visual Effect
    /// </summary>
    /// <param name="stop"></param>
    public void Reflection(bool stop)
    {
        float intensity;
        if (stop)
            intensity = 0.006f;
        else
        {
            intensity = 0.02f;
            Invoke(nameof(ReflectionOff), 0.05f);
        }

        material.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1f) * intensity);
    }

    private void ReflectionOff()
    {
        Reflection(true);
    }
}
