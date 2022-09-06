using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Guns {

	public class RocketTurret : Gun, IInteractable, IPreviewElement {

		[Header("Additional Settings")]
		[SerializeField] private GameObject m_target;
		
		[Header("Shell Settings")]
		[SerializeField] private Transform m_firePoint; //start point for turret shooting
		[SerializeField] private float m_shellTurnSensitive; //shell turn sensitive. Recommended value is 3
		[SerializeField] private List<string> m_shellInteractedTags; //shell interacted tags
		
		[Header("System settings")]
		[SerializeField] private CircleCollider2D m_effectArea;
		[SerializeField] private LineBuilder m_lineBuilder;
		[SerializeField] private float m_radius; //attack radius
		
		private List<RocketShell> m_rocketShells = new List<RocketShell>();
		private bool m_isFirstLaunch = true;
		
		private void Awake() {
			m_effectArea.enabled = true;
		}

		private void Update() {
			if (m_drawTrajectory) {
				DrawTrajectory();
			}
			else {
				m_lineBuilder.Hide();
			}
		}

		private void FixedUpdate() {
			var hitColliders = Physics2D.OverlapCircleAll(transform.position, m_radius);
			foreach (var collider in hitColliders) {
				if (!CanInteract(collider.tag)) 
					continue;
				
				if (m_isFirstLaunch) {
					Reload();
					m_fireTime = 0f;
					m_isFirstLaunch = false;
				}
					
				if (m_fireTime >= m_fireRate) {
					Reload();
					m_fireTime = 0f;
				}
			}
			
			m_fireTime += Time.fixedDeltaTime;
			
			UpdateTargetPosition();
		}
		
		/// <summary>
		/// Reloads the rocket turret
		/// </summary>
		protected override void Reload() {
			var shell = InstantiateNewShell(m_firePoint.position, m_firePoint.rotation).GetComponent<RocketShell>();
			RegisterShell(shell);
			InitializeNewShell(shell);
		}

		/// <summary>
		/// Registers new shell
		/// </summary>
		/// <param name="rocketShell"></param>
		private void RegisterShell(RocketShell rocketShell) {
			m_rocketShells.Add(rocketShell);
		}

		/// <summary>
		/// Initializes new shell
		/// </summary>
		/// <param name="rocketShell"></param>
		private void InitializeNewShell(RocketShell rocketShell) {
			rocketShell.Init(m_firePoint.position, m_target.transform.position, m_shellSpeed, m_shellTurnSensitive, m_shellExplodeDelay, m_shellDamage, m_shellInteractedTags);
		}

		/// <summary>
		/// Updates target position for each shell
		/// </summary>
		private void UpdateTargetPosition() {
			m_rocketShells.ForEach(shell => {
				shell.UpdateTargetPosition(m_target.transform.position);
			});
		}
		
		/// <summary>
		/// Can gameObject interact with? gameObject will interact with each object with speficied tag.
		/// </summary>
		/// <param name="tag"></param>
		public bool CanInteract(string tag) {
			return m_interactedTags.Contains(tag);
		}
		
		/// <summary>
		/// Draws the trajectory(Effect Area) 
		/// WARNING! Trajectory is visible only in play mode. Also you should to enable Draw Trajectory in Inspector Window
		/// </summary>
		public void DrawTrajectory() {
			var positions = GetPositionsOfDrawArea(m_effectArea.transform.localPosition);
			m_lineBuilder.DrawLine(positions, false);
		}

		/// <summary>
		/// Gets array of positions for drawing effect area
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		private Vector3[] GetPositionsOfDrawArea(Vector3 position) {
			var positionsCount = (int)(m_radius * 20f);
			var positions = new Vector3[positionsCount];
			var angle = 0f;
			var step = 360f / (positions.Length - 1);
			for (var i = 0; i < positions.Length; i++) {
				positions[i] = new Vector3(position.x + m_radius * Mathf.Cos(angle * Mathf.Deg2Rad),
					position.y + m_radius * Mathf.Sin(angle * Mathf.Deg2Rad),
					position.z);
				angle += step;
			}
			
			return positions;
		}
		
		/// <summary>
		/// Draws Effect Area when play mode OFF. 
		/// WARNING! You can see GIZMOS only on Scene Window
		/// </summary>
		private void OnDrawGizmos() {
			if (Application.isPlaying || !m_drawTrajectory) 
				return;
			
			var positions = GetPositionsOfDrawArea(m_effectArea.transform.position);
			Gizmos.color = Color.green;
			var lastPosition = positions[0];
			for (var p = 1; p < positions.Length; p++) {
				Gizmos.DrawLine(lastPosition, positions[p]);
				lastPosition = positions[p];
			}
		}
	}
}
