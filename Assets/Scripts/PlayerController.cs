using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float movementMargins;

    Rigidbody myRigidbody;
    MeshCollider platformCollider;

    float xAxisInput;
    float maxMovementX;
    int groundLayerMask;
    bool jumpInput;
    bool isGrounded;

    void Awake()
    {
        platformCollider = GameObject.FindGameObjectWithTag("Ground").GetComponent<MeshCollider>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void Start() 
    {
        maxMovementX = platformCollider.bounds.max.x - movementMargins;
        groundLayerMask = LayerMask.NameToLayer("Ground");
        myRigidbody.maxAngularVelocity = 14;
        myRigidbody.angularVelocity = new Vector3(14, 0, 0);
    }

    void FixedUpdate()
    {
        myRigidbody.velocity = new Vector3(xAxisInput, myRigidbody.velocity.y, 0);
        RestrictPlayerToPlatform();
    }

    private void RestrictPlayerToPlatform()
    {
        if (transform.position.x > maxMovementX && myRigidbody.velocity.x > 0)
        {
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        }
        else if (transform.position.x < -maxMovementX && myRigidbody.velocity.x < 0)
        {
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == groundLayerMask)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == groundLayerMask)
        {
            isGrounded = false;
        }
    }

    void OnMove(InputValue value)
    {
        xAxisInput =  value.Get<Vector2>().x * moveSpeed;
    }

    void OnJump(InputValue value)
    {
        if (isGrounded)
        {
            myRigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
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
