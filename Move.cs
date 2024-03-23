using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [Header("ÒÆ¶¯²ÎÊý")]
    public float speed = 2f;

    private float xVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        GroundMovement();
    }

    private void GroundMovement()
    {
        xVelocity = Input.GetAxis("Horizontal");//-1f~1f

        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        animator.SetFloat("Speed", new Vector2(xVelocity * speed,0).magnitude);
        FilpDirction();
    }
    void FilpDirction()
    {
        if (xVelocity < 0)
            transform.localScale = new Vector2(-1, 1);
        if (xVelocity > 0)
            transform.localScale = new Vector2(1, 1);
    }

}
