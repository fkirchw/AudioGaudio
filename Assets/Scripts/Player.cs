using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerVelocity = 2;
    private Vector2 moveDir = Vector2.zero;
    public Rigidbody2D rigidBody;
    private readonly HashSet<Vector2> contacts = new HashSet<Vector2>();
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.A))
        {
            moveX += -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveX += 1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveY += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveY += -1;
        }

        moveDir = new Vector2(moveX, moveY).normalized;
    }

    public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rigidBody.velocity = 3 * PlayerVelocity * moveDir;
        }
        else
        {
            rigidBody.velocity = moveDir * PlayerVelocity;
        }
        
    }
}
