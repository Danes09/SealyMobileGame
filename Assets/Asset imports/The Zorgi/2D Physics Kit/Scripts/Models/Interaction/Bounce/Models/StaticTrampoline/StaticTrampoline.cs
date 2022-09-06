using UnityEngine;

namespace TheZorgi.Interaction.Bounce {

	public class StaticTrampoline : Bounce {
	
		/// <summary>
		/// Adding velocity by jumping
		/// </summary>
		/// <param name="collision">Collision.</param>
		protected override void OnCustomCollisionEnter2D(Collision2D collision) {
			m_collisionObjectRigidbody = collision.transform.GetComponent<Rigidbody2D> ();
			if (m_collisionObjectRigidbody == null) 
				return;
			
			Vector2 newVelocity = (2 * transform.up * (((Vector3)collision.relativeVelocity).magnitude * 1) 
			                       * Mathf.Cos (Vector2.Angle (transform.up, (Vector3)collision.relativeVelocity) * Mathf.Deg2Rad) - (Vector3)collision.relativeVelocity) * (-1f);
			m_collisionObjectRigidbody.velocity = newVelocity * m_bounce;
		}
		
		/// <summary>
		/// Inheritance fake realization. Probably will be used in future
		/// </summary>
		/// <param name="collision">Collision.</param>
		protected override void OnCustomCollisionExit2D(Collision2D collision) {}
	}
}
