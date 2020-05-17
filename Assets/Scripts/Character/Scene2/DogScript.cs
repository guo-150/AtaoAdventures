using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class Dogscript : BaseObject
{
    Animator anim;
    CharacterController controller;

    public float MoveSpeed;//移动速度
    public float Jumphigh;//跳跃速度
    public float Gravity = 20f;//跳下来的重力

    public Image Pot;
    public Text PotDistance;
    RaycastHit hit;

    Vector3 MoveDir = Vector3.zero;//移动向量

    public LineRenderer lineRenderer;
    Vector3[] pos = new Vector3[2];//点的数组
    Vector3 targetPos;//中间点
    Vector3 StartPos;//开始

    public GameObject LeftHand;
    //public GameObject RightHand;
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        lineRenderer.material.color = Color.black;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.01f;
    }
    void Update()
    /*动画控制器开关 
     * attack1：条件：attack1的taggle开关打开                                                                                 Attack1
     * attack2： 2.idle状态到attack2  条件为attack2 的taggle开关打开       Attack2
     * getHit：条件：int值=1；                                                                                                Int
     * Walk：条件：int值=2，退出walk：int值<2；int=0
     * run：条件int值=3；  退出条件：int值>3  int=0
     * die:条件：int=5，不可退出
     * 
     * 
     * q键为攻击1，e键为攻击2
     * 死亡和被攻击为被动
     * 空格为跳跃，wasd为前后左右移动方向
     * 
     * 
     * 
     * 攻击逻辑
     * 日常开启盾和剑的开关
     * 当冲击滑翔时，关闭开关
     * 
     */
    {
        #region 动画控制
        if (anim && Input.GetMouseButtonDown(0)){//鼠标左键键为攻击1，右键为攻击2
            anim.SetTrigger("Attack1");
        }
        if (anim && Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Attack2");
        }
        #endregion
        #region 跑步和跳跃

        if (controller.isGrounded)//是否时条件没有进入 非，查看walk动画，已关闭播放完整退出，还是有问题
        {
            if (Input.GetKey(KeyCode.W))//长按w  走路
            {
                anim.SetInteger("Int", 2);
            }
            else
                anim.SetInteger("Int", 0);       
            MoveDir = new Vector3(0, 0, Input.GetAxis("Vertical"));
            transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * 90 * Time.deltaTime);
            MoveDir = transform.TransformDirection(MoveDir);//屏幕坐标转化成世界坐标
            MoveDir *= MoveSpeed;
            if (Input.GetKeyDown(KeyCode.Space))//跳跃的时候关闭跑步开关，落地之后开启跑步开关
            {
                MoveDir.y += Jumphigh;
            }
        }
        MoveDir.y -= Gravity * Time.deltaTime;
        controller.Move(MoveDir * Time.deltaTime);      
        #endregion
        #region 射击
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit, 50))
        {
            if ((hit.point - transform.position).magnitude<13)
            {          
            Pot.color = Color.red;
            PotDistance.color = Color.red;
            PotDistance.text = (hit.point-transform.position).magnitude.ToString("F2");//保留2位小数   
            if (Input.GetKeyDown(KeyCode.Q))
            {
                clearLine();
                    pos[0] = LeftHand.transform.position;
                    lineRenderer.SetPosition(0, pos[0]);
                    targetPos = LeftHand.transform.position + transform.forward * 10.0f;
                   // targetPos +=(hit.point-StartPos)*10*Time.deltaTime;
                     StartPos = LeftHand.transform.position;            
                 StartLine();//开始划线
                post = StartPos;               
            }
            }
        else
        {
            Pot.color = Color.white;
            PotDistance.color = Color.white;
            PotDistance.text = "???";
        }
        }

        if (RTrigger)
        {
            RMove(transform.position, targetPos);
        }

        if (LineTrigger)
        {
            LineMove(StartPos, targetPos);
        }
        #endregion
    }
    #region 发射  
    public float Rspeed = 5.0f;//移动速度
    public bool RTrigger = false;//移动开关
    public int linePointIndex = 1;//线的点

    public bool LineTrigger = false;//线的开关
    public void StartLine()
    {
        LineTrigger = true;
    }
    Vector3 post;
    public void LineMove(Vector3 Start, Vector3 End)//射线移动
    {
        float dis = (End - post).magnitude;
        Debug.Log("dis   " + dis);
        Vector3 dir = (End - Start);

     dir.y = 0;
        if (dis > 1.5f)
        {
            
            post += dir * Rspeed * 0.5f * Time.deltaTime;
            pos[1] = post;
            Debug.Log("linePointIndex" + linePointIndex);
            lineRenderer.positionCount = linePointIndex + 1;
            lineRenderer.SetPosition(linePointIndex, pos[1]);
            linePointIndex++;

            //  transform.position += dir * Rspeed * Time.deltaTime;
        }
        else
        {
            LineTrigger = false;
            StartRMove();
        }
    }
    public void StartRMove()//移动的开关
    {
        RTrigger = true;
    }
    public void RMove(Vector3 Start, Vector3 End)//人物的移动
    {

        float dis = (End - transform.position).magnitude;
        Debug.Log("dis   " + dis);
        Vector3 dir = (End - Start);
        dir.y = 0;
        if (dis > 1.5f)
        {
            pos[1] = transform.position;
            lineRenderer.SetPosition(1, pos[1]);
            controller.Move(dir * Rspeed * 0.5f * Time.deltaTime);
            //  transform.position += dir * Rspeed * Time.deltaTime;
        }
        else
        {
            RTrigger = false;
            linePointIndex = 1;
            lineRenderer.positionCount = 1;
            Debug.LogError("eeeeeeeeeee");
        }
    }
    public void clearLine()//清楚线的痕迹
    {
        RTrigger = false;
        LineTrigger = false;
        linePointIndex = 1;

        lineRenderer.positionCount = 1;
    }
    #endregion
}
