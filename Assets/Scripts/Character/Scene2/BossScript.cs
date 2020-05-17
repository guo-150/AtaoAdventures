using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BossScript : Reference
{
    /*人型Boss战斗逻辑    
     * boss触发器范围触碰到人物，boss追击人物，主要靠手抓
     * 
     */
   public GameObject Player;
   //public Collider AlertCollider;//警戒范围
   //public Collider AttackCollider;//攻击范围
    Animator Bossanim;
    public float MoveSpeed;
    //public bool Alertistrue;

    float DisTemp;

    void Start()
    {
        //Player = GameObject.FindGameObjectWithTag(TAG_PLAYER);
        Bossanim = GetComponent<Animator>();

    }
    void Update()
    {    

              
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("触碰开始");
    }
    private void OnTriggerStay(Collider other)//如果人物在怪物警戒范围内，boss跑向人物
    {
        /*在3<x的时候，boss追击dog   Bossint=2 
         * x<3的时候，boss攻击dog（直接出walk状态，进入攻击状态，攻击状态持续）       bossint=0（int<1） attack=true， 
         * dog逃跑的时候，BOss退出攻击追dog（退出攻击状态，进入walk）              attack=flase bossint =2;
         * 出了范围（walk状态过度到victory状态），                               bossint=3
         */
        Debug.Log("持续触碰");
        Vector3 dir = other.transform.position - transform.position;
        DisTemp = dir.magnitude;
        if (other.tag==TAG_PLAYER)
        {
            Debug.Log("是玩家");
             if (dir.magnitude>3)
             {            
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
            transform.position += dir.normalized * MoveSpeed * Time.deltaTime;
                Bossanim.SetInteger("BossInt", 2);
                Bossanim.SetBool("Attack",false);
            }                      
             else if(dir.magnitude<=3)
            {
                Bossanim.SetTrigger("Attack");
                Bossanim.SetInteger("BossInt", 0);
            }
        
        }
    }
    private void OnTriggerExit(Collider other)//出碰撞器的时候，boss Victory
    {
        Debug.Log("脱离boss");
        Bossanim.SetInteger("BossInt",3);                   
    }






}
