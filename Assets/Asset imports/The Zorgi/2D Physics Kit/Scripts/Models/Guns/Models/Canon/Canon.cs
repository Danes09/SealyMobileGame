using TheZorgi.Utils;
using UnityEngine;

namespace TheZorgi.Guns {
	
	public class Canon : Gun, IPreviewElement {

        [Header("Additional Gun Settings")]
        [SerializeField] private Transform m_firePoint;
        [SerializeField] private Transform m_fireDirection;
		[SerializeField] private Collider2D m_collider;
		[SerializeField] private bool m_isAutoFire;

		[Header("System settings")]
        [SerializeField] private LineBuilder m_lineBuilder;

		private void Update() {
            if (m_isAutoFire && m_fireTime >= m_fireRate) {
                Reload();
                m_fireTime = 0f;
            }
            m_fireTime += Time.deltaTime;

            if(m_drawTrajectory) {
                DrawTrajectory();
            } else {
                m_lineBuilder.Hide();
            }
        }
		
		/// <summary>
		/// Reload the Canon
		/// </summary>
		protected override void Reload() {
			var shell = InstantiateNewShell(m_firePoint.position, Quaternion.identity).GetComponent<CanonShell>();
			InitializeNewShell(shell);
			m_isReadyToFire = true;
		}
		
		/// <summary>
		/// Initializes new shell
		/// </summary>
		/// <param name="canonShell"></param>
		private void InitializeNewShell(CanonShell canonShell) {
			canonShell.Init(m_firePoint.position, m_fireDirection.position, m_shellSpeed, m_shellExplodeDelay, m_shellDamage, m_interactedTags);
			var bulletCollider = canonShell.GetComponent<Collider2D>();
			Physics2D.IgnoreCollision(m_collider, bulletCollider, true);
		}
		
		/// <summary>
		/// Processing Click on Canon(reload / fire)
		/// </summary>
		public void OnMouseDown() {
			if (m_isAutoFire && !m_isReadyToFire)
				return;

			Reload();
		}

		/// <summary>
		/// Draws the trajectory(Effect Area) 
		/// WARNING! Trajectory is visible only in play mode. Also you should enable Draw Trajectory in Inspector Window
		/// </summary>
		public void DrawTrajectory() {
			Vector2 currentSpeed = (m_fireDirection.position - m_firePoint.position).normalized * m_shellSpeed;
			var trajectory = PhysicsTrajectory.GetTrajectory(currentSpeed, m_firePoint.position);

			m_lineBuilder.DrawLine(trajectory.ToArray(), true);
			m_lineBuilder.enabled = true;
		}

		/// <summary>
		/// Draws Effect Area when play mode OFF. 
		/// WARNING! You can see GIZMOS only on Scene Window
		/// </summary>
        private void OnDrawGizmos() {
			if (Application.isPlaying || !m_drawTrajectory) 
				return;
			
			var positions = PhysicsTrajectory.GetTrajectory((m_fireDirection.position - m_firePoint.position).normalized * m_shellSpeed, m_firePoint.position);
			if (positions == null || positions.Count <= 1) 
				return;
	            
			Gizmos.color = Color.green;
			var lastPosition = positions[0];

			for (var p = 1; p < positions.Count; p++) {
				if (p % 2 != 0)
					Gizmos.DrawLine(lastPosition, positions[p]);

				lastPosition = positions[p];
			}
		}
    }
}
