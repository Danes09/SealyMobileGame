using UnityEngine;

namespace TheZorgi
{
    public class CharacterController : MonoBehaviour {

        [Header("Main Settings")]
        [SerializeField] float m_speed;
        [SerializeField] float m_jumpPower;

        [Header("System")]
		[SerializeField] Rigidbody2D m_rigidbody;
        [SerializeField] Collider2D m_collider;
        [SerializeField] GroundChecker m_groundChecker;
        [SerializeField] bool m_isGrounded;

        void FixedUpdate() {
            m_isGrounded = m_groundChecker.IsGrounded;

            if (m_isGrounded && Input.GetButtonDown("Jump"))
                Jump();

            var horizontalAxis = Input.GetAxis("Horizontal");
            Move(horizontalAxis);
        }

        private void Jump() {
			m_rigidbody.AddForce(Vector2.up * m_jumpPower, ForceMode2D.Impulse);
        }

        private void Move(float horizontalAxis) {
            if (Mathf.Abs(horizontalAxis) < 0.1f) 
	            return;

			var newVelocity = new Vector2(horizontalAxis * m_speed, m_rigidbody.velocity.y);
            m_rigidbody.velocity = newVelocity;
            transform.localScale = new Vector3(Mathf.Sign(horizontalAxis), transform.localScale.y, transform.localScale.z);
        }

		/// <summary>
		/// Get / Set character speed
		/// </summary>
		public float Speed {
			get { return m_speed; }
			set { m_speed = value; }
		}
    }
}
