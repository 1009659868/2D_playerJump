using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("跳跃参数")]
    public float jumpForce = 3f;          //跳跃力
    public float jumpHoldForce = 1.2f;      //长按跳跃附加力
    public float jumpHoldDuration = 0.1f;   //长按跳跃窗口时长
    public float crouchJumpBoost = 6f; //蹲下跳跃力

    private float jumpTime;//计算跳跃时间

    [Header("移动参数")]
    public float speed = 2f;
    public float crouchSpeedDivisor = 0.5f; //下蹲的速度

    private float xVelocity;

    [Header("状态")]
    public bool IsCrouch=false;
    public bool IsOnGround;
    public bool IsJump;

    [Header("环境检测")]
    public LayerMask groundLayer;

    [Header("按键")]
    //按键设置
    public bool jumpPressed=false;
    public bool jumpHeld=false;
    public bool crouchHeld = false;

    //碰撞体尺寸
    private Vector2 colliderStandSize;
    private Vector2 colliderStandOffset;
    private Vector2 colliderCrouchSize;
    private Vector2 colliderCrouchOffset;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb= GetComponent<Rigidbody2D>();

        //保存原有尺寸
        colliderStandSize = coll.size;
        colliderStandOffset=coll.offset;

        //蹲下后尺寸
        colliderCrouchSize=new Vector2(coll.size.x, coll.size.y/2f);
        colliderCrouchOffset=new Vector2(coll.offset.x, coll.offset.y/2f);

    }

    // Update is called once per frame
    void Update()
    {
        if (!jumpPressed)
        {
            jumpPressed = Input.GetButtonDown("Jump");
        }
        
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
    }
    private void FixedUpdate()
    {
       
        PhysicsCheck(); 
        GroundMovement();
        MidAirMovement();
    }
    void PhysicsCheck()
    {
        //地面碰撞检测
        if (coll.IsTouchingLayers(groundLayer))
        {
            IsOnGround = true;
        }
        else
            IsOnGround = false;
    }
    private void GroundMovement()
    {
        if (crouchHeld)
            Crouch();
        else if (!crouchHeld && IsCrouch)
            StandUp();

        xVelocity = Input.GetAxis("Horizontal");//-1f~1f

        if (IsCrouch)
            xVelocity /= crouchSpeedDivisor;

        rb.velocity=new Vector2(xVelocity*speed,rb.velocity.y);
        FilpDirction();
    }
    
    void MidAirMovement()
    {
        Debug.Log("jumpPressed:" + jumpPressed);
        Debug.Log("IsOnGround:" + IsOnGround);
        if (jumpPressed&&IsOnGround)
        {
            IsOnGround = false;
            IsJump = true;

            jumpTime= Time.time+jumpHoldDuration;

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);//Impulse冲力
            jumpPressed = false;
            Debug.Log("jump_Pressed");
        }
        else if (IsJump)
        {
            if (jumpHeld&&Time.time-(jumpTime-jumpHoldDuration)>jumpHoldDuration/5)
            {
                Debug.Log("High jump");
                rb.AddForce(new Vector2(0f,jumpHoldForce),ForceMode2D.Impulse);
            }
            if (jumpTime < Time.time)
            {
                IsJump = false;
            }
            
        }
    }
    
    void FilpDirction()
    {
        if (xVelocity < 0)
            transform.localScale = new Vector2(-1, 1);
        if (xVelocity > 0)
            transform.localScale = new Vector2(1, 1);
    }
    //下蹲
    void Crouch()
    {
        IsCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }
    //站立
    void StandUp()
    {
        IsCrouch= false;
        coll.size=colliderStandSize; 
        coll.offset = colliderStandOffset;
    }
}
