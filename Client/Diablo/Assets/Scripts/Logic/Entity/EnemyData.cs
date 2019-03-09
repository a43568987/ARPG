using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData{

    //敌人属性信息

    public float m_Health { set; get; }
    public float m_Attack { set; get; }
    public float m_Defend { set; get; }

    public bool m_IsDead = false;

    private int m_Timer = 0;

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
            if(m_Timer == 0)
            {
                m_Timer = Framework.TimerManager.Instance.AddTimer(3, null);
            }
            if (Framework.TimerManager.Instance.IsOver(m_Timer))
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
