using UnityEngine;

namespace TheZorgi.Interaction.Gravitation {

	public class GravityPlatform : Gravitation {

		[Header("Additional Gravity Settings")]
		[SerializeField] private AreaEffector2D m_effectArea;

        private BoxCollider2D m_boxCollider;

		/// <summary>
		/// Updates Effect Area
		/// </summary>
		protected override void UpdateAreaEffector () {
			m_effectArea.forceMagnitude = Mathf.Abs (m_force);
			if (Mathf.Sign (m_force) > 0) {
				m_effectArea.forceAngle = 90f;
				m_waves.color = m_positiveColor;
			} else {
				m_effectArea.forceAngle = -90f;
				m_waves.color = m_negativeColor;
			}
		}

		/// <summary>
		/// Draws the trajectory(Effect Area) 
		/// WARNING! Trajectory is visible only in play mode. Also you should enable Draw Trajectory in Inspector Window
		/// </summary>
		protected override void DrawOwnTrajectory() {
			if(m_boxCollider == null)
				m_boxCollider = m_effectArea.gameObject.GetComponent<BoxCollider2D>();
			
			var positions = GetPositionsOfDrawArea(m_boxCollider.transform.localPosition);
            m_lineBuilder.DrawLine(positions, false);
        }
		
		/// <summary>
		/// Gets array of positions for drawing effect area
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		protected override Vector3[] GetPositionsOfDrawArea(Vector3 position) {
			var positions = new[] {
				new Vector3(position.x - m_boxCollider.size.x/2f, position.y + m_boxCollider.size.y/2f, position.z),
				new Vector3(position.x + m_boxCollider.size.x/2f, position.y + m_boxCollider.size.y/2f, position.z),
				new Vector3(position.x + m_boxCollider.size.x/2f, position.y - m_boxCollider.size.y/2f, position.z),
				new Vector3(position.x - m_boxCollider.size.x/2f, position.y - m_boxCollider.size.y/2f, position.z),
				new Vector3(position.x - m_boxCollider.size.x/2f, position.y + m_boxCollider.size.y/2f, position.z)
			};

			return positions;
		}

		/// <summary>
		/// Draws Effect Area when play mode OFF. 
		/// WARNING! You can see GIZMOS only on Scene Window
		/// </summary>
		private void OnDrawGizmos() {
			if (Application.isPlaying || !m_drawTrajectory) 
				return;
			
			if(m_boxCollider == null)
				m_boxCollider = m_effectArea.gameObject.GetComponent<BoxCollider2D>();
			
			var positions = GetPositionsOfDrawArea(m_boxCollider.transform.position);
			Gizmos.color = Color.green;
			var lastPosition = positions[0];
			for (var p = 1; p < positions.Length; p++) {
				Gizmos.DrawLine(lastPosition, positions[p]);
				lastPosition = positions[p];
			}
		}
    }

}
