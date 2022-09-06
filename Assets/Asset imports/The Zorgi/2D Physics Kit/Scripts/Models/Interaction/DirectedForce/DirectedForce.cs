using System.Collections.Generic;
using TheZorgi.Utils;
using UnityEngine;

namespace TheZorgi.Interaction {

	public class DirectedForce : MonoBehaviour, IPreviewElement {

		[Header("Main Settings")]
		[SerializeField] private float m_force;
		[SerializeField] private Vector2 m_direction;
		[SerializeField] private bool m_drawTrajectory;

        [Header("System")]
        [SerializeField] private LineBuilder m_lineBuilder;
        [SerializeField] private Custom2DEventTrigger m_eventsTrigger;
		[SerializeField] private CustomCollider2DButton m_clickArea;
        [SerializeField] private Animation m_animation;

		private Dictionary<int, Rigidbody2D> m_objectsInEffectArea = new Dictionary<int, Rigidbody2D> ();
        private BoxCollider2D m_boxCollider;

		private void Update() {
            if (m_drawTrajectory)
				DrawTrajectory();//Show Effect Area
            else
				m_lineBuilder.Hide();//Hide Effect Area
        }

		/// <summary>
		/// Add event listeners 
		/// </summary>
		private void OnEnable () {
			m_eventsTrigger.OnCustomTriggerEnter2D  += OnCustomTriggerEnter2DHandler;
			m_eventsTrigger.OnCustomTriggerExit2D 	+= OnCustomTriggerExit2DHandler;
			m_clickArea.OnButtonDown 				+= OnButtonDownHandler;
		}

		/// <summary>
		/// Remove event listeners
		/// </summary>
		private void OnDisable () {
			m_eventsTrigger.OnCustomTriggerEnter2D 	-= OnCustomTriggerEnter2DHandler;
			m_eventsTrigger.OnCustomTriggerExit2D 	-= OnCustomTriggerExit2DHandler;
			m_clickArea.OnButtonDown 				-= OnButtonDownHandler;
		}

		/// <summary>
		/// Adds the force to each object from list in Effect Area
		/// </summary>
        public void AddForce() {
            foreach (var rigidbody in m_objectsInEffectArea.Values) {
                rigidbody.AddForce(m_direction * m_force, ForceMode2D.Impulse);
            }
        }

		/// <summary>
		/// Adding an object to the list on which the Directed force will act
		/// </summary>
		/// <param name="obj">Object.</param>
		private void OnCustomTriggerEnter2DHandler (Collider2D obj) {
			var rigidbody = obj.GetComponent<Rigidbody2D> ();
			
			if (rigidbody != null)
				m_objectsInEffectArea.Add (obj.GetInstanceID (), rigidbody);
		}

		/// <summary>
		/// Remove object from the list on which the Directed Force will act
		/// </summary>
		/// <param name="obj">Object.</param>
		private void OnCustomTriggerExit2DHandler (Collider2D obj) {
			m_objectsInEffectArea.Remove (obj.GetInstanceID ());
		}

		/// <summary>
		/// Raises the button down handler event.
		/// </summary>
		private void OnButtonDownHandler () {
			AddForce ();
			m_animation.Stop();
			m_animation.Play();
		}

		/// <summary>
		/// Draws the trajectory(Effect Area) 
		/// WARNING! Trajectory is visible only in play mode. Also you should enable Draw Trajectory in Inspector Window
		/// </summary>
        public void DrawTrajectory() {
            if (m_boxCollider == null)
                m_boxCollider = (BoxCollider2D)m_eventsTrigger.Collider;

            var positions = new[] {
                    new Vector3(m_boxCollider.transform.localPosition.x - m_boxCollider.size.x/2f, m_boxCollider.transform.localPosition.y + m_boxCollider.size.y/2f, m_boxCollider.transform.localPosition.z),
                    new Vector3(m_boxCollider.transform.localPosition.x + m_boxCollider.size.x/2f, m_boxCollider.transform.localPosition.y + m_boxCollider.size.y/2f, m_boxCollider.transform.localPosition.z),
                    new Vector3(m_boxCollider.transform.localPosition.x + m_boxCollider.size.x/2f, m_boxCollider.transform.localPosition.y - m_boxCollider.size.y/2f, m_boxCollider.transform.localPosition.z),
                    new Vector3(m_boxCollider.transform.localPosition.x - m_boxCollider.size.x/2f, m_boxCollider.transform.localPosition.y - m_boxCollider.size.y/2f, m_boxCollider.transform.localPosition.z),
                    new Vector3(m_boxCollider.transform.localPosition.x - m_boxCollider.size.x/2f, m_boxCollider.transform.localPosition.y + m_boxCollider.size.y/2f, m_boxCollider.transform.localPosition.z),
                 };
            m_lineBuilder.DrawLine(positions, false);
        }

		/// <summary>
		/// Draws Effect Area when play mode OFF. 
		/// WARNING! You can see GIZMOS only on Scene Window
		/// </summary>
        private void OnDrawGizmos() {
			if (Application.isPlaying || !m_drawTrajectory) 
				return;
			
			if(m_boxCollider == null)
				m_boxCollider = m_eventsTrigger.GetComponent<BoxCollider2D>();

			var positions = new[] {
				new Vector3(m_boxCollider.transform.position.x - m_boxCollider.size.x/2f, m_boxCollider.transform.position.y + m_boxCollider.size.y/2f, m_boxCollider.transform.position.z),
				new Vector3(m_boxCollider.transform.position.x + m_boxCollider.size.x/2f, m_boxCollider.transform.position.y + m_boxCollider.size.y/2f, m_boxCollider.transform.position.z),
				new Vector3(m_boxCollider.transform.position.x + m_boxCollider.size.x/2f, m_boxCollider.transform.position.y - m_boxCollider.size.y/2f, m_boxCollider.transform.position.z),
				new Vector3(m_boxCollider.transform.position.x - m_boxCollider.size.x/2f, m_boxCollider.transform.position.y - m_boxCollider.size.y/2f, m_boxCollider.transform.position.z),
				new Vector3(m_boxCollider.transform.position.x - m_boxCollider.size.x/2f, m_boxCollider.transform.position.y + m_boxCollider.size.y/2f, m_boxCollider.transform.position.z),
			};

			Gizmos.color = Color.green;
			var lastPosition = positions[0];
			for (var p = 1; p < positions.Length; p++) {
				Gizmos.DrawLine(lastPosition, positions[p]);
				lastPosition = positions[p];
			}
		}
    }
}
