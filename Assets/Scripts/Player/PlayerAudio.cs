using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CURRENTLT UNUSED
/// </summary>
public class PlayerAudio : MonoBehaviour
{
    Rigidbody myRigidbody;
    AudioSource audioSource;

    [SerializeField] AnimationCurve volumeCurve;
    [SerializeField] AnimationCurve pitchCurve;

    [Range(0.5f, 1f)]
    [SerializeField] float minRollPitch;
    [Range(0.5f, 1f)]
    [SerializeField] float maxRollPitch;
    [Range(0f, 1f)]
    [SerializeField] float maxVolume;

    [SerializeField] float minimumNoiseSpeed = 0.5f;
    float currentSpeed;
    float maxSpeed = 4f;   // Got the value from testing in editor

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        MakeRollingSound();
    }

    /// <summary>
    /// Produces the rolling sound of the player ball. Pitch and volume are tied to players speed.
    /// </summary>
    private void MakeRollingSound()
    {
        currentSpeed = Mathf.Clamp(Mathf.Abs(myRigidbody.angularVelocity.x) + Mathf.Abs(myRigidbody.angularVelocity.z), 0, 4);

        // If below threshold speed, no sound is played
        if (currentSpeed > minimumNoiseSpeed)
        {
            // normalize speed value into 0-1, pitch into 0.6-0.8
            float scaledVelocity = RemapValue(currentSpeed, 0, maxSpeed, 0, maxVolume);
            float scaledPitch = RemapValue(currentSpeed, 0, maxSpeed, minRollPitch, maxRollPitch);

            // set volume based on volume curve set in editor
            audioSource.volume = volumeCurve.Evaluate(scaledVelocity);

            // set pitch based on pitch curve set in editor
            audioSource.pitch = pitchCurve.Evaluate(scaledPitch);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Remaps the value that in the range from1-to1 into the new range from2-to2
    /// Example: 
    /// originalValue = 0.8 with first range 0-1, second range 10-20
    /// then RemapValue(originalValue, 0, 1, 10, 20) returns 18
    /// </summary>
    /// <returns> New value </returns>
    float RemapValue(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
