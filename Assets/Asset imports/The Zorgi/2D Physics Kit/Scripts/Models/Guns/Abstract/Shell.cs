using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Guns {

	public abstract class Shell : MonoBehaviour, IDestroyable, IInteractable, IGameSound, IGameAnimation {

		[Header("Shell main settings")] 
		[SerializeField] protected SpriteRenderer m_sprite;
		
		protected List<string> m_interactedTags;
		protected Rigidbody2D m_rigidbody;
		protected float m_destroyDelay;
		protected float m_damage;
		
		protected abstract void Hide();
		protected abstract void PlayParticles();
		protected abstract void StopParticles();
		protected abstract void DisableMovement();

		protected virtual void Awake() {
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		/// <summary>
		/// Shell trigger enter
		/// </summary>
		private void OnTriggerEnter2D(Collider2D collider) {
			if (!CanInteract(collider.tag))
				return;

			SetShellEffect(collider); //depending on the type of shell, set an effect on the Player
		}

		/// <summary>
		/// Shell collision enter
		/// </summary>
		/// <param name="collision"></param>
		private void OnCollisionEnter2D(Collision2D collision) {
			if (!CanInteract(collision.collider.tag))
				return;
			
			SetShellEffect(collision); //depending on the type of shell, set an effect on the Player
		}

		/// <summary>
		/// Can gameObject interact with? If leave m_interactedTags empty, gameObject will interact with everything.
		/// </summary>
		/// <param name="tag"></param>
		public bool CanInteract(string tag) {
			return m_interactedTags.Count == 0 || m_interactedTags.Contains(tag);
		}

		/// <summary>
		/// Invoke destroying shell after "time" seconds
		/// </summary>
		/// <param name="destroyDelay"></param>
		public virtual void Explode(float destroyDelay) {
			Destroy(gameObject, destroyDelay);
		}

		/// <summary>
		/// Set shell effect by collider
		/// </summary>
		/// <param name="collider"></param>
		protected virtual void SetShellEffect(Collider2D collider) {
			PlayAnimation();
			PlaySound();
		}

		/// <summary>
		/// Set shell effect by collision
		/// </summary>
		/// <param name="collision"></param>
		protected virtual void SetShellEffect(Collision2D collision) {
			PlayAnimation();
			PlaySound();
		}

		/// <summary>
		/// Starts animation in the current shell
		/// </summary>
		public virtual void PlayAnimation() {}

		/// <summary>
		/// Stops animation in the current shell
		/// </summary>
		public virtual void StopAnimation() {}

		/// <summary>
		/// Starts playing sound
		/// </summary>
		public virtual void PlaySound() {}

		/// <summary>
		/// Stops playing sound
		/// </summary>
		public virtual void StopSound() {}
	}
}
