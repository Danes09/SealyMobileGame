using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Guns {
	
	public class CanonShell : Shell {
		
		[Header("Shell settings")]
		[SerializeField] private ParticleSystem m_explodeParticles;
		
		private Vector3 m_directionPosition;
		private float m_speed;
		
		/// <summary>
		/// Initialize shell
		/// </summary>
		public void Init(Vector3 startFirePoint, Vector3 directionPosition, float speed, float shellExplodeDelay, float damage, List<string> interactedTags) {
			transform.position = startFirePoint;
			m_directionPosition = directionPosition;
			m_speed = speed;
			m_destroyDelay = shellExplodeDelay;
			m_damage = damage;
			m_interactedTags = interactedTags;

			Fire();
		}
		
		/// <summary>
		/// Fires the shell by increasing velocity
		/// </summary>
		private void Fire() {
			m_rigidbody.velocity = (m_directionPosition - transform.position).normalized * m_speed;
		}
		
		#region Customer's usage
		
		/// <summary>
		/// Detects, if shell contacts with some game object
		/// </summary>
		/// <param name="other"></param>
		protected override void SetShellEffect(Collision2D collision) {
			base.SetShellEffect(collision);
			DisableMovement();
			Explode(m_destroyDelay);
			//TODO Use m_damage during interaction with Player for dealing damage
		}
		
		/// <summary>
		/// Disable visual elements
		/// </summary>
		protected override void Hide() {
			m_sprite.enabled = false;
			StopParticles();
		}

		/// <summary>
		/// Play particles
		/// </summary>
		protected override void PlayParticles() {
			m_explodeParticles.Play();
		}

		/// <summary>
		/// Stop particles
		/// </summary>
		protected override void StopParticles() {}

		/// <summary>
		/// Disable Movement
		/// </summary>
		protected override void DisableMovement() {
			m_rigidbody.velocity = Vector2.zero;
			m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
		}
		
		/// <summary>
		/// Starts animation in the current shell
		/// </summary>
		public override void PlayAnimation() {
			PlayParticles();
			Hide();
		}

		/// <summary>
		/// Starts playing sound
		/// </summary>
		public override void PlaySound(){}

		/// <summary>
		/// Stops playing sound
		/// </summary>
		public override void StopSound(){}

		#endregion
	}
}
