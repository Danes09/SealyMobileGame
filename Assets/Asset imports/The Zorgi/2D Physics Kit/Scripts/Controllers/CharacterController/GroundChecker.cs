using UnityEngine;

namespace TheZorgi {
    [RequireComponent(typeof(Collider2D))]
    public class GroundChecker : MonoBehaviour
	{
		[SerializeField] LayerMask m_groundLayer;
		[SerializeField] bool m_isGrounded;

        public bool IsGrounded {
            get { return m_isGrounded; }
        }

        void OnTriggerEnter2D(Collider2D collider) {
            if (collider.gameObject.layer == Mathf.Log(m_groundLayer.value, 2))
                m_isGrounded = true;
        }

        void OnTriggerStay2D(Collider2D collider) {
            if (collider.gameObject.layer == Mathf.Log(m_groundLayer.value, 2))
                m_isGrounded = true;
        }

        void OnTriggerExit2D(Collider2D collider) {
            if (collider.gameObject.layer == Mathf.Log(m_groundLayer.value, 2))
                m_isGrounded = false;
        }
    }
}
