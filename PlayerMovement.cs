using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("��Ծ����")]
    public float jumpForce = 3f;          //��Ծ��
    public float jumpHoldForce = 1.2f;      //������Ծ������
    public float jumpHoldDuration = 0.1f;   //������Ծ����ʱ��
    public float crouchJumpBoost = 6f; //������Ծ��

    private float jumpTime;//������Ծʱ��

    [Header("�ƶ�����")]
    public float speed = 2f;
    public float crouchSpeedDivisor = 0.5f; //�¶׵��ٶ�

    private float xVelocity;

    [Header("״̬")]
    public bool IsCrouch=false;
    public bool IsOnGround;
    public bool IsJump;

    [Header("�������")]
    public LayerMask groundLayer;

    [Header("����")]
    //��������
    public bool jumpPressed=false;
    public bool jumpHeld=false;
    public bool crouchHeld = false;

    //��ײ��ߴ�
    private Vector2 colliderStandSize;
    private Vector2 colliderStandOffset;
    private Vector2 colliderCrouchSize;
    private Vector2 colliderCrouchOffset;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb= GetComponent<Rigidbody2D>();

        //����ԭ�гߴ�
        colliderStandSize = coll.size;
        colliderStandOffset=coll.offset;

        //���º�ߴ�
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
        //������ײ���
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

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);//Impulse����
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
    //�¶�
    void Crouch()
    {
        IsCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }
    //վ��
    void StandUp()
    {
        IsCrouch= false;
        coll.size=colliderStandSize; 
        coll.offset = colliderStandOffset;
    }
}
