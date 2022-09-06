using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Guns {
	
	public class RocketShell : Shell {

		[Header("Shell settings")]
		[SerializeField] private ParticleSystem m_smokeParticles;
		[SerializeField] private ParticleSystem m_explodeParticles;
		
		private Vector3 m_targetPosition;
		private float m_speed;
		private float m_turn;
		private float m_lastTurn;
		private float m_turnSensitive;
	
		private bool m_isInited;
		private bool m_destroyed;
	
		private void FixedUpdate() {
			if (!m_isInited || m_destroyed)
				return;
			
			var newRotation = Quaternion.LookRotation(transform.position - m_targetPosition, Vector3.forward);
			newRotation.x = 0.0f;
			newRotation.y = 0.0f;
			transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.fixedDeltaTime * m_turn);
			m_rigidbody.velocity = transform.up * m_speed;
			
			if (!(m_turn < m_turnSensitive)) 
				return;
			
			m_lastTurn += Time.fixedDeltaTime;
			m_turn += m_lastTurn;
		}

		/// <summary>
		/// Initialize shell
		/// </summary>
		public void Init(Vector3 startFirePoint, Vector2 targetPosition, float speed, float maxTurnSpeed, float shellExplodeDelay, float damage, List<string> interactedTags) {
			transform.position = startFirePoint;
			m_targetPosition = targetPosition;
			m_speed = speed;
			m_turnSensitive = maxTurnSpeed;
			m_destroyDelay = shellExplodeDelay;
			m_damage = damage;
			m_interactedTags = interactedTags;
			
			m_isInited = true;
		}

		/// <summary>
		/// Updates target position
		/// </summary>
		public void UpdateTargetPosition(Vector2 targetPosition) {
			if (m_destroyed)
				return;
			
			m_targetPosition = targetPosition;
		}
		
		#region Customer's usage
		
		/// <summary>
		/// Detects contact shell with gameobject
		/// </summary>
		/// <param name="collision"></param>
		protected override void SetShellEffect(Collider2D collision) {
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
		protected override void StopParticles() {
			m_smokeParticles.Stop();
		}

		/// <summary>
		/// Disable Movement
		/// </summary>
		protected override void DisableMovement() {
			m_destroyed = true;
			m_rigidbody.velocity = Vector2.zero;
		}
		
		/// <summary>
		/// Starts animation in the current shell
		/// </summary>
		public override void PlayAnimation() {
			PlayParticles();
			Hide();
		}

		/// <summary>
		/// Play sound
		/// </summary>
		public override void PlaySound(){}

		/// <summary>
		/// Stop sound
		/// </summary>
		public override void StopSound(){}

		#endregion
	}
}