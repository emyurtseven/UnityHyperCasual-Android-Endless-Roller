using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is attached to objects that detect the player coming close and drop from above.
/// CURRENTLY UNUSED
/// </summary>
public class Dropper : MonoBehaviour
{
    CapsuleCollider myTriggerCollider;
    Rigidbody myRigidbody;

    private void Start() 
    {
        myTriggerCollider = GetComponent<CapsuleCollider>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            myRigidbody.useGravity = true;      // Turn on gravity so object drops
            Destroy(myTriggerCollider);     // Destroy the trigger collider once it's done the job
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StartCoroutine(FixObjectPosition());
        }
    }

    /// <summary>
    /// Coroutine that fixes the object to the ground after it drops
    /// </summary>
    IEnumerator FixObjectPosition()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(myRigidbody);
    }


}
