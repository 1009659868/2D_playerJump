using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_1 : MonoBehaviour
{
    private GroundCheck gCheck;
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;

    public float speed, jumpForce;
    //获取地面检测
    [Header("地面检测")]
    public Transform groundCheck;
    public LayerMask ground;

    [Header("状态")]
    public bool isJump;
    private bool jumpPressed;
    private int jumpCount;
    public bool IsGround => gCheck.IsGround;

    private void Awake()
    {
        gCheck = GetComponentInChildren<GroundCheck>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")&&jumpCount>0)
        {
            jumpPressed = true;
        }
        

    }
    private void FixedUpdate()
    {
        //地面检测
        //isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        Debug.Log(IsGround);
        Jump();

        GroundMovement();
        
    }
    void GroundMovement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        if (horizontalMove != 0)
        {
            transform.localScale=new Vector3(horizontalMove,1,1);
        }
    }
    void Jump()
    {
        if (IsGround)
        {
            jumpCount = 2;//二段跳
            
        }
        if(jumpPressed&&IsGround)
        {
            isJump=true;
            rb.velocity=new Vector2(rb.velocity.x,jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount>0&&isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }
}
