using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData {
    //角色属性信息

    public float m_Health { set; get; }
    public float m_Attack { set; get; }
    public float m_Defend { set; get; }

    public bool m_IsDead = false;

    // Use this for initialization
    public void OnInitialize()
    {
        m_Health = 100;
        m_Attack = 30;
        m_Defend = 10;
    }
    public void OnUpdate(float deltaTime)
    {
        if (m_Health <= 0)
        {
            m_IsDead = true;
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
