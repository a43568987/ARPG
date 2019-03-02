using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character{

    private CharacterData m_CharacterData = new CharacterData();
    private CharacterController m_CharacterController = new CharacterController();

    // Use this for initialization
    public void OnInitialize()
    {
        m_CharacterData.OnInitialize();
        m_CharacterController.OnInitialize();
        m_CharacterController.SetData(m_CharacterData);
	}

    // Update is called once per frame
    public void OnUpdate(float deltaTime)
    {
        m_CharacterController.OnUpdate(deltaTime);
        m_CharacterData.OnUpdate(deltaTime);
    }
    public CharacterController GetController()
    {
        return m_CharacterController;
    }
    public CharacterData GetData()
    {
        return m_CharacterData;
    }
}
