using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Interaction.Bounce {

	[RequireComponent(typeof(WheelJoint2D))]
	public class Trampoline : Bounce {

		private Dictionary<int, KeyValuePair<Rigidbody2D, Vector2>> m_bounceObjects = new Dictionary<int, KeyValuePair<Rigidbody2D, Vector2>> ();

		/// <summary>
		/// Add object to the list on which additional velocity will act
		/// </summary>
		/// <param name="collision">Collision.</param>
		protected override void OnCustomCollisionEnter2D(Collision2D collision) {
			m_collisionObjectRigidbody = collision.transform.GetComponent<Rigidbody2D> ();
			if (m_collisionObjectRigidbody == null) 
				return;
			
			var objectID = collision.gameObject.GetInstanceID ();
			if (!m_bounceObjects.ContainsKey (objectID)) {
				m_bounceObjects.Add(collision.gameObject.GetInstanceID(), new KeyValuePair<Rigidbody2D, Vector2>(m_collisionObjectRigidbody, collision.relativeVelocity));
			}
		}

		/// <summary>
		/// Adding velocity and remove from the list each object which contacts with Trampoline
		/// </summary>
		/// <param name="collision">Collision.</param>
		protected override void OnCustomCollisionExit2D(Collision2D collision) {
			var objectID = collision.gameObject.GetInstanceID ();
			if (!m_bounceObjects.ContainsKey(objectID)) 
				return;
			
			KeyValuePair<Rigidbody2D, Vector2> pair = m_bounceObjects [objectID]; //where key - Rigidbody2D, value - collision enter velocity
			m_bounceObjects.Remove (objectID);
			var prevVelocity  = pair.Value;
			var newVelocity = (2 * (Vector2)transform.up * (prevVelocity.magnitude * 1) * Mathf.Cos (Vector2.Angle (transform.up, prevVelocity) * Mathf.Deg2Rad) - prevVelocity) * (-1f);

			Rigidbody2D bounceObjectRigidbody = pair.Key;
			bounceObjectRigidbody.velocity = newVelocity * m_bounce;
		}
	}
}
