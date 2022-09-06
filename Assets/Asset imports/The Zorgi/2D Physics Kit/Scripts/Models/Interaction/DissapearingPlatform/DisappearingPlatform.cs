using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Interaction {
	
	public class DisappearingPlatform : MonoBehaviour, IInteractable {
		
		[Header("Platform settings")]
		[SerializeField] private List<string> m_interactedTags;
		[SerializeField] private float m_blinkingTime; //life time of the platform
		[SerializeField] private float m_blinkingRate; //less value increasing blinking speed

		private Renderer m_renderer;
		private float m_time;
		private bool m_isConnected;
		
		private void Awake() {
			m_renderer = GetComponent<Renderer>();
		}

		private void Update() {
			if (!m_isConnected)
				return;
			
			m_renderer.enabled = !(Time.fixedTime % 0.5 < m_blinkingRate);
			
			if (m_time >= m_blinkingTime) {
				DestroyPlatform();
			}
			
			m_time += Time.deltaTime;
		}

		private void OnCollisionEnter2D(Collision2D other) {
			if (!CanInteract(other.collider.tag))
				return;

			m_isConnected = true;
		}

		private void DestroyPlatform() {
			Destroy(gameObject);
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
