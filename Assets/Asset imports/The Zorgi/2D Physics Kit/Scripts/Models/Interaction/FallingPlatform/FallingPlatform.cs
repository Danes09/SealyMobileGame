using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Interaction {
	
	public class FallingPlatform : MonoBehaviour, IInteractable {
		
		[Header("Platform settings")]		
		[SerializeField] private List<string> m_interactedTags;
		[SerializeField] private float m_fallingSpeed;
		[SerializeField] private float m_destroyDelay;
		
		[Header("System settings")]
		[SerializeField] private Rigidbody2D m_rigidbody;

		private void OnCollisionEnter2D(Collision2D other) {
			if (!CanInteract(other.collider.tag))
				return;
			
			m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
			m_rigidbody.velocity = -transform.up * m_fallingSpeed;

			DestroyPlatform();
		}

		private void DestroyPlatform() {
			Destroy(gameObject, m_destroyDelay);
		}
		
		/// <summary>
		/// Can gameObject interact with? If leave m_interactedTags empty, gameObject will interact with everything.
		/// </summary>
		/// <param name="tag"></param>
		public bool CanInteract(string tag) {
			return m_interactedTags.Contains(tag);
		}

	}
}
