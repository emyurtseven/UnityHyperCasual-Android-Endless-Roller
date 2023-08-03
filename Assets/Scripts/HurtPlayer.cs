using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is attached to lethal game objects.
/// </summary>
public class HurtPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player dead");
        }
    }
}
