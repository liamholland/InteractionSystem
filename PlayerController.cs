using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject groundCheck;
    public float GCRadius;
    public LayerMask whatIsGround;
    public float jumpForce;
    public float movementSmoothing;
    public float moveSpeed;
    public bool airControl;

    public static float isFacing;

    private float canJump;
    private Rigidbody2D playerRigidBody;
    private Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private bool CheckIfGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, GCRadius, whatIsGround);
        if (colliders.Length > 0)
            return true;
        else
            return false;
    }

    private void Move()
    {
        if (CheckIfGrounded() || airControl)
        {
            isFacing = Input.GetAxisRaw("Horizontal");
            Vector3 targetVelocity = new Vector2(moveSpeed * isFacing, playerRigidBody.velocity.y);
            playerRigidBody.velocity = Vector3.SmoothDamp(playerRigidBody.velocity, targetVelocity, 
                ref currentVelocity, movementSmoothing);
        }

        if (CheckIfGrounded() && Input.GetAxisRaw("Jump") > 0)
        {
            playerRigidBody.AddForce(new Vector2(0f, jumpForce));
        }
    }
}
