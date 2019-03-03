using UnityEngine;

namespace Logic
{
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        private Vector3 m_Offset;

        private void Start()
        {
            m_Offset = transform.position - GameManager.Instance.Player.Position;
        }

        private void LateUpdate()
        {
            Vector3 m_LastPosition = transform.position;
            Vector3 m_TargetPosition = GameManager.Instance.Player.Position + m_Offset;
            transform.position = Vector3.Lerp(m_LastPosition, m_TargetPosition, Time.deltaTime * 8);
        }
    }
}
