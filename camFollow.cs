using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public Transform player;         // Assign in inspector or dynamically
    public float smoothSpeed = 5f;   // Adjust for speed of interpolation
    public Vector3 offset;           // The offset from the player (can set in inspector)

    // Camera shake parameters
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.2f;
    private float dampingSpeed = 1.0f;
    private Vector3 initialPosition;

    void Awake()
    {
        initialPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            if (shakeDuration > 0)
            {
                // shake logic
                transform.position = smoothedPosition + Random.insideUnitSphere * shakeMagnitude;
                shakeDuration -= Time.deltaTime * dampingSpeed;

                // Ensure shake doesn't go negative
                if (shakeDuration <= 0)
                {
                    shakeDuration = 0f;
                }
            }
            else
            {
                transform.position = smoothedPosition;
            }
        }
    }

    // Call this from another script to start the shake
    public void ShakeCamera(float duration, float magnitude = 0.2f, float damping = 1.0f)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        dampingSpeed = damping;
    }
}
