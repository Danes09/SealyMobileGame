using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Guns {

	public class ForceGunShell : Shell {
		
		[Header("Shell settings")]
		[SerializeField] private ParticleSystem m_explodeParticles;
		
		private CircleCollider2D m_collider;
		
		protected override void Awake() {
			base.Awake();
			m_collider = GetComponent<CircleCollider2D>();
		}
		
		/// <summary>
		/// Initialize shell
		/// </summary>
		public void Init(Vector3 startFirePoint, float shellExplodeDelay, float damage, List<string> interactedTags) {
			transform.position = startFirePoint;
			m_collider.enabled = false;
			m_destroyDelay = shellExplodeDelay;
			m_damage = damage;
			m_interactedTags = interactedTags;
		}

		/// <summary>
		/// Fires the shell by increasing velocity
		/// </summary>
		/// <param name="forceVector"></param>
		public void Fire(Vector2 forceVector) {
			m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
			m_rigidbody.velocity = forceVector;
			m_collider.enabled = true;
		}

		public void UpdatePosition(Vector3 newPosition) {
			transform.position = newPosition;
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
		/// Starts animation in the current shell
		/// </summary>
		public override void PlayAnimation() {
			PlayParticles();
			Hide();
		}

		/// <summary>
		/// Disable Movement
		/// </summary>
		protected override void DisableMovement() {
			m_rigidbody.velocity = Vector2.zero;
			m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
		}

		/// <summary>
		/// Stops animation in the current shell
		/// </summary>
		public override void StopAnimation(){}

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
