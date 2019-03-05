using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy{

    private EnemyData m_EnemyData = new EnemyData();
    private EnemyController m_EnemyController = new EnemyController();

    // Use this for initialization
    public void OnInitialize(){
        m_EnemyData.OnInitialize();
        m_EnemyController.OnInitialize();
        m_EnemyController.SetData(m_EnemyData);
    }

    // Update is called once per frame
    public void OnUpdate(float deltaTime) { 
        m_EnemyController.OnUpdate(deltaTime);
        m_EnemyData.OnUpdate(deltaTime);
    }
    
    public EnemyController GetController()
    {
        return m_EnemyController;
    }
    public EnemyData GetData()
    {
        return m_EnemyData;
    }
}
