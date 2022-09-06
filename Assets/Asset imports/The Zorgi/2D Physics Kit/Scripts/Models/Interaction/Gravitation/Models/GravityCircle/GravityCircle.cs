using UnityEngine;

namespace TheZorgi.Interaction.Gravitation {

	public class GravityCircle : Gravitation {

		[Header("Additional Gravity Settings")]
		[SerializeField] private PointEffector2D m_effectArea;
		
		private CircleCollider2D m_circleCollider;

		/// <summary>
		/// Updates Effect Area
		/// </summary>
        protected override void UpdateAreaEffector () {
			m_effectArea.forceMagnitude = m_force;
			m_waves.color = m_force > 0 ? m_positiveColor : m_negativeColor;
		}

		/// <summary>
		/// Draws the trajectory(Effect Area) 
		/// WARNING! Trajectory is visible only in play mode. Also you should enable Draw Trajectory in Inspector Window
		/// </summary>
		protected override void DrawOwnTrajectory() {
			if (m_circleCollider == null)
				m_circleCollider = m_effectArea.gameObject.GetComponent<CircleCollider2D>();
			
			var positions = GetPositionsOfDrawArea(m_circleCollider.transform.localPosition);
			m_lineBuilder.DrawLine(positions, false);
		}

		/// <summary>
		/// Gets array of positions for drawing effect area
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		protected override Vector3[] GetPositionsOfDrawArea(Vector3 position) {
			var positionsCount = (int)(m_circleCollider.radius * 20f);
			var positions = new Vector3[positionsCount];
			var angle = 0f;
			var step = 360f / (positions.Length - 1);
			for (var i = 0; i < positions.Length; i++) {
				positions[i] = new Vector3(position.x + m_circleCollider.radius * Mathf.Cos(angle * Mathf.Deg2Rad),
					position.y + m_circleCollider.radius * Mathf.Sin(angle * Mathf.Deg2Rad),
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

			if (m_circleCollider == null)
				m_circleCollider = m_effectArea.gameObject.GetComponent<CircleCollider2D>();
			
			var positions = GetPositionsOfDrawArea(m_circleCollider.transform.position);
			Gizmos.color = Color.green;
			var lastPosition = positions[0];
			for (var p = 1; p < positions.Length; p++) {
				Gizmos.DrawLine(lastPosition, positions[p]);
				lastPosition = positions[p];
			}
		}
	}

}
