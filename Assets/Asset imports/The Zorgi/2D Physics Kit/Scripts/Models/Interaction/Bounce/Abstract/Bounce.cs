using TheZorgi.Utils;
using UnityEngine;

namespace TheZorgi.Interaction.Bounce {

	public abstract class Bounce : MonoBehaviour {

		[Header("Main settings")] 
		[SerializeField] protected float m_bounce;

		[Header("System settings")] 
		[SerializeField] protected CustomCollision2DEventTrigger m_topPartEventsTrigger;

		protected Rigidbody2D m_collisionObjectRigidbody;

		protected abstract void OnCustomCollisionEnter2D(Collision2D collision);
		protected abstract void OnCustomCollisionExit2D(Collision2D collision);
		
		/// <summary>
		/// Add event listeners 
		/// </summary>
		private void OnEnable () {
			if (!m_topPartEventsTrigger) 
				return;
			
			m_topPartEventsTrigger.OnCustomCollisionEnter2D += OnCustomCollisionEnter2D;
			m_topPartEventsTrigger.OnCustomCollisionExit2D 	+= OnCustomCollisionExit2D;
		}

		/// <summary>
		/// Remove event listeners
		/// </summary>
		private void OnDisable () {
			if (!m_topPartEventsTrigger) 
				return;
			
			m_topPartEventsTrigger.OnCustomCollisionEnter2D -= OnCustomCollisionEnter2D;
			m_topPartEventsTrigger.OnCustomCollisionExit2D 	-= OnCustomCollisionExit2D;
		}
	}
}