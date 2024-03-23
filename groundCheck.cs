using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{

    //�����ڽ�ɫ���»���ͼ�εķ�ʽ�����е�����
    [Header("��������")]
    public LayerMask mask;
    [Range(0, 1)] public float boxHeight = 0.01f;
    [Range(0, 1)] public float boxWidth = 0.36f;
    public Vector2 boxCenter;
    //��ȡ��ɫ��size,���������ɫ��"����"����
    private Vector2 playerSize;
    private Vector2 boxSize;

    [Header("������")]
    [SerializeField] public bool isGround;

    private Rigidbody2D _rigidbody2D;

    //���ڶ�̬�ı�����Ŀ��,��Ϊһ���м��ݴ����
    private float boxWidthTemp;
    private float boxHeightTemp;

    // Start is called before the first frame update
    void Start()
    {
        //��ȡ��ɫ����
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //ͨ��������Ⱦ������ȡ��ɫ�ı߿��С
        playerSize= GetComponent<SpriteRenderer>().bounds.size;
        //��������Ĵ�С,�����Ŀ�Ĭ��Ϊ��ɫ�Ŀ�,��Ϊһ����С��ֵ,���ö�ά����(��ж�ά����)Vector2������
        boxSize = new Vector2(playerSize.x * boxWidth, boxHeight);
        //�����ʼ��ײ���λ��
        boxCenter = (Vector2)transform.position + (Vector2.down * playerSize.y * 0.5f);
        //����ʱ������ֵ
        boxWidthTemp = boxWidth;
        boxHeightTemp = boxHeight;
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }
    //��ײ��size,postion��check��ͬ����˵�����
    void Check()
    {
        boxSizeUpdate();
        boxPostionUpdate();
        GroundCheck();
    }
    //��ײ��size���º���
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
        //����������box�ڽ�ɫ�·�,����box����Vector2������ʵ��
        //����transform��ȡ��ǰ��ɫ��λ����Ϣ
        //Vector2�����ݽṹΪ{��:(0,1),��:(1,0),��:(0,-1),��:(-1,0)}
        //ͬ����������������ݽṹ��ʵ�ֽ�ɫ�����ҷ�ת
        //transform.position{��ʾ��ɫλ��:(ˮƽ����,��ֱ����)��λ��}
        //����boxCenter��λ��:ˮƽλ�ò���(��Ϊ�µ����ݽṹ"��:(0,-1)",x�ǳ���0,������Ӻ�ˮƽλ�ò���),�߶�λ��Ϊtransform.position.y��ȥ��ɫ�߶ȵ�һ��
        boxCenter = (Vector2)transform.position + (Vector2.down * playerSize.y * 0.5f);
    }
    //�����⺯��
    void GroundCheck()
    {
        //Physics2D.OverlapBox���ص�ֵ����boolҲ��collider2D
        //����:��һ�������� PhysicsScene2D �е���ײ����бȶԣ������ص�һ������
        //�ο��ĵ�:https://docs.unity3d.com/cn/2020.2/ScriptReference/PhysicsScene2D.OverlapBox.html
        //Ҳ����˵�����boxCenter�ϼ���Ƿ���mask�н���,�оͱ�ʾ������ײ,û�оͱ�ʾû�з�����ײ
        if (Physics2D.OverlapBox(boxCenter, boxSize, 0, mask) != null)
        {
            isGround=true;
        }
        else
        {
            isGround=false;
        }
    }

    //������box���Ƴ���,���ӻ�
    //�ο��ĵ�:https://docs.unity3d.com/cn/2020.2/ScriptReference/Gizmos.html
    //����һ���̳з�����(�������),���������Զ�����,����Ҫ��ӵ�update������ִ��
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
        //DrawWireCube	ʹ�� center �� size ����һ���߿���塣
        //�����center����֮ǰ�����boxcenter,��һ��vector2����
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
