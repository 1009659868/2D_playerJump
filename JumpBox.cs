using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    private Animator animator;

    [Header("跳跃参数")]
    [Range(0, 10)] public float jumpVelocity = 5f;

    [Header("地面检测箱")]
    public LayerMask mask;
    [Range(0, 1)] public float boxHeight=0.1f/10;
    [Range(0, 1)] public float boxWidth=0.36f;

    private Vector2 playerSize;
    private Vector2 boxSize;

    private bool jumpRequest=false;
    private bool grounded = false;
    private bool isFalling = false;
    private bool isDoubleJumping=false;
    private bool isJumping=false;

    private Rigidbody2D _rigidbody2D;

    private float boxWidthTemp;
    private float boxHeightTemp;

    private int jumpCount;
    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
        _rigidbody2D=GetComponent<Rigidbody2D>();
        playerSize = GetComponent<SpriteRenderer>().bounds.size;
        boxSize = new Vector2(playerSize.x * boxWidth, boxHeight/10);
        boxWidthTemp = boxWidth;
        boxHeightTemp = boxHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && (grounded||jumpCount>0))
        {
            jumpRequest = true;
        }
        boxSizeUpdate();
    }
    void boxSizeUpdate()
    {
        if (boxWidthTemp != boxWidth)
        {
            boxSize = new Vector2(playerSize.x * boxWidth, boxHeight/10);
            boxWidthTemp = boxWidth;
        }
        if (boxHeightTemp != boxHeight)
        {
            boxSize = new Vector2(playerSize.x * boxWidth, boxHeight / 10);
            boxHeightTemp = boxHeight;
        }
    }
    private void FixedUpdate()
    {
        

        Jump();
        
        Debug.Log(_rigidbody2D.velocity.y);
    }
   
    void Jump()
    {
        if (grounded)
        {
            jumpCount = 2;
            isJumping = false;
            isDoubleJumping = false;
            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsDoubleJumping", isDoubleJumping);
        }
        else
        {
            //FallingCheck();
        }
        if (jumpRequest&&grounded)
        {
            Debug.Log("jump");
            _rigidbody2D.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            jumpRequest = false;
            grounded = false;
            isJumping = true;
            jumpCount--;
            animator.SetBool("IsJumping", isJumping);
        }
        else if (jumpRequest && jumpCount > 0 && isJumping)
        {
            Debug.Log("double");
            _rigidbody2D.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            jumpRequest=false;
            grounded = false;
            jumpCount--;
            isJumping=true;
            isDoubleJumping=true;
            animator.SetBool("IsDoubleJumping", isDoubleJumping);
        }
        else
        {
            GroundCheck();
        }
    }
    void FallingCheck()
    {
        if (_rigidbody2D.velocity.y < 0&&(!isJumping||!isDoubleJumping)&&!grounded)
        {
            isFalling = true;
            isJumping=false;
            isDoubleJumping = false;
            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsDoubleJumping", isDoubleJumping);
            animator.SetBool("IsFalling", isFalling);
        }
        else
        {
            isFalling= false;
            animator.SetBool("IsFalling", isFalling);
        }
    }
    void GroundCheck()
    {
        //绘制虚拟检测box,在角色下方,
        Vector2 boxCenter = (Vector2)transform.position + (Vector2.down * playerSize.y * 0.5f);
        if (Physics2D.OverlapBox(boxCenter, boxSize, 0, mask) != null)
        {
            grounded = true;
            isJumping = false;
            isDoubleJumping = false;
            animator.SetBool("IsJumping", false);
        }
        else
        {
            grounded = false;
            animator.SetBool("IsJumping", true);
        }
    }
    private void OnDrawGizmos()
    {
        if (grounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Vector2 boxCenter = (Vector2)transform.position + (Vector2.down * playerSize.y * 0.5f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
