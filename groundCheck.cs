using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{

    //利用在角色底下绘制图形的方式来进行地面检测
    [Header("地面检测箱")]
    public LayerMask mask;
    [Range(0, 1)] public float boxHeight = 0.01f;
    [Range(0, 1)] public float boxWidth = 0.36f;
    public Vector2 boxCenter;
    //获取角色的size,用来计算角色的"底下"在哪
    private Vector2 playerSize;
    private Vector2 boxSize;

    [Header("地面检测")]
    [SerializeField] public bool isGround;

    private Rigidbody2D _rigidbody2D;

    //用于动态改变检测箱的宽高,作为一个中间暂存变量
    private float boxWidthTemp;
    private float boxHeightTemp;

    // Start is called before the first frame update
    void Start()
    {
        //获取角色刚体
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //通过精灵渲染器来获取角色的边框大小
        playerSize= GetComponent<SpriteRenderer>().bounds.size;
        //计算检测箱的大小,检测箱的宽默认为角色的宽,高为一个较小的值,利用二维向量(或叫二维载体)Vector2来绘制
        boxSize = new Vector2(playerSize.x * boxWidth, boxHeight);
        //计算初始碰撞箱的位置
        boxCenter = (Vector2)transform.position + (Vector2.down * playerSize.y * 0.5f);
        //给临时变量赋值
        boxWidthTemp = boxWidth;
        boxHeightTemp = boxHeight;
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }
    //碰撞箱size,postion和check共同组成了地面检测
    void Check()
    {
        boxSizeUpdate();
        boxPostionUpdate();
        GroundCheck();
    }
    //碰撞箱size更新函数
    void boxSizeUpdate()
    {
        if (boxWidthTemp != boxWidth)
        {
            boxSize = new Vector2(playerSize.x * boxWidth, boxHeight / 10);
            boxWidthTemp = boxWidth;
        }
        if (boxHeightTemp != boxHeight)
        {
            boxSize = new Vector2(playerSize.x * boxWidth, boxHeight / 10);
            boxHeightTemp = boxHeight;
        }
    }
    void boxPostionUpdate()
    {
        //绘制虚拟检测box在角色下方,虚拟box利用Vector2容器来实现
        //利用transform获取当前角色的位置信息
        //Vector2的数据结构为{上:(0,1),右:(1,0),下:(0,-1),左:(-1,0)}
        //同样可以利用这个数据结构来实现角色的左右翻转
        //transform.position{表示角色位置:(水平居中,垂直居中)的位置}
        //所以boxCenter的位置:水平位置不变(因为下的数据结构"下:(0,-1)",x是乘以0,所以相加后水平位置不变),高度位置为transform.position.y减去角色高度的一半
        boxCenter = (Vector2)transform.position + (Vector2.down * playerSize.y * 0.5f);
    }
    //地面检测函数
    void GroundCheck()
    {
        //Physics2D.OverlapBox返回的值既是bool也是collider2D
        //描述:将一个盒体与 PhysicsScene2D 中的碰撞体进行比对，仅返回第一个交点
        //参考文档:https://docs.unity3d.com/cn/2020.2/ScriptReference/PhysicsScene2D.OverlapBox.html
        //也就是说如果在boxCenter上检测是否与mask有交点,有就表示发送碰撞,没有就表示没有发生碰撞
        if (Physics2D.OverlapBox(boxCenter, boxSize, 0, mask) != null)
        {
            isGround=true;
        }
        else
        {
            isGround=false;
        }
    }

    //将虚拟box绘制出来,可视化
    //参考文档:https://docs.unity3d.com/cn/2020.2/ScriptReference/Gizmos.html
    //这是一个继承方法吧(还不清除),反正可以自动运行,不需要添加到update里面来执行
    private void OnDrawGizmos()
    {
        if (isGround)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color= Color.red;
        }
        //DrawWireCube	使用 center 和 size 绘制一个线框盒体。
        //这里的center就是之前计算的boxcenter,是一个vector2容器
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
