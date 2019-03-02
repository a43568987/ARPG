using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData{

    //敌人属性信息
    private EnemyController m_EnemyAI = new EnemyController();

    public float m_Health { set; get; }
    public float m_Attack { set; get; }
    public float m_Defend { set; get; }

    public bool m_IsDead = false;

    private float m_Timer = 0;

    // Use this for initialization
    public void OnInitialize()
    {
        m_Health = 100;
        m_Attack = 5;
        m_Defend = 10;
    }

    public void OnUpdate(float deltaTime)
    {
        if(m_Health <= 0)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer > 3)
            {
                m_IsDead = true;
            }
        }
    }

    public float GetHealth()
    {
        return m_Health;
    }
    public float GetAttack()
    {
        return m_Attack;
    }
    public float GetDefend()
    {
        return m_Defend;
    }
    public bool IsDead()
    {
        return m_IsDead;
    }
}
