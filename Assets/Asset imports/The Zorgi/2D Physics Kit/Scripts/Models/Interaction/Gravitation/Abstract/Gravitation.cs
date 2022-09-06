using UnityEngine;

namespace TheZorgi.Interaction.Gravitation {

	public abstract class Gravitation : MonoBehaviour, IPreviewElement {
		
		[Header("Main Settings")]
		[Tooltip ("Force value depends on a sign. Negative - inside, positive - outside.")]
		[SerializeField] protected float m_force;
		[SerializeField] protected bool m_drawTrajectory;

		[Header("System settings")]
		[SerializeField] protected LineBuilder m_lineBuilder;
		[SerializeField] protected SpriteRenderer m_waves;
		
		protected Color m_positiveColor;
		protected Color m_negativeColor;

		protected abstract void UpdateAreaEffector();
		protected abstract void DrawOwnTrajectory();
		protected abstract Vector3[] GetPositionsOfDrawArea(Vector3 position);
			
		private void Awake () {
			m_positiveColor = Color.cyan;
			m_negativeColor = Color.red;
		}

		private void Start() {
			UpdateAreaEffector();

			if (m_drawTrajectory)
				DrawTrajectory();
		}
		
		private void Update() {
			UpdateAreaEffector();

			if (m_drawTrajectory) {
				DrawTrajectory();
			}
			else {
				m_lineBuilder.Hide();
			}
		}

		public void DrawTrajectory() {
			DrawOwnTrajectory();
		}
		
		public float Force {
			get {
				return m_force;
			}
			set {
				m_force = value;
				UpdateAreaEffector ();
			}
		}
	}
}