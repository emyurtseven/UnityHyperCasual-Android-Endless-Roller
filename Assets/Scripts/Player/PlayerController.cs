using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float dodgeSpeed = 8f;
    [SerializeField] float jumpSpeed = 12f;
    [SerializeField] float movementMargins = 0.5f;

    TouchActions touchActions;    // provide general access to TouchActions

    InputAction move;
    InputAction jump;

    Rigidbody myRigidbody;
    MeshCollider platformCollider;
    Vector2 moveDirection;

    float xAxisInput;
    float maxMovementX;
    int groundLayerMask;
    bool jumpInput;
    bool isGrounded;


    void Awake()
    {
        platformCollider = GameObject.FindGameObjectWithTag("Ground").GetComponent<MeshCollider>();
        myRigidbody = GetComponent<Rigidbody>();
        touchActions = new TouchActions();
    }

    private void Start()
    {
        maxMovementX = platformCollider.bounds.max.x - movementMargins;
        groundLayerMask = LayerMask.NameToLayer("Ground");
        myRigidbody.maxAngularVelocity = GameManager.Instance.GameSpeed * 1.3f;
        myRigidbody.angularVelocity = new Vector3(myRigidbody.maxAngularVelocity, 0, 0);
    }

    private void OnEnable()
    {
        move = touchActions.Player.Move;
        move.performed += UIManager.Instance.ModifyMoveButtonColors;
        move.Enable();

        jump = touchActions.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    void FixedUpdate()
    {
        myRigidbody.velocity = new Vector3(xAxisInput, myRigidbody.velocity.y, 0);
        myRigidbody.angularVelocity = new Vector3(myRigidbody.maxAngularVelocity, 0, -xAxisInput);
        RestrictPlayerToPlatform();
    }

    private void Update()
    {
        xAxisInput = move.ReadValue<Vector2>().x * dodgeSpeed;
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            myRigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            AudioManager.PlaySfx(AudioClipName.Jump, 1);
        }
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
            UIManager.Instance.ModifyJumpButtonColor(isGrounded);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == groundLayerMask)
        {
            isGrounded = false;
            UIManager.Instance.ModifyJumpButtonColor(isGrounded);
        }
    }
}