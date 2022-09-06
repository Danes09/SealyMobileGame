using System.Collections;
using System.Collections.Generic;
using TheZorgi.Utils;
using UnityEngine;

namespace TheZorgi.Traps.Damage {

	public class Ticker : MonoBehaviour, ITrapDamage, IInteractable, IPreviewElement {

        [Header("Main Settings")]
        [SerializeField] private float m_speed;
		[SerializeField] private bool m_drawTrajectory;
		[SerializeField] private float m_initialDamage; //The damage which player will get after falling into a trap
		[SerializeField] private List<string> m_interactedTags;

        [Header("System")]
		[SerializeField] private LineBuilder m_lineBuilder;
		[SerializeField] private Transform m_center;
		[SerializeField] private Transform m_sinkerEnd;
		[SerializeField] private Transform m_posA;
		[SerializeField] private Transform m_posB;
		[SerializeField] Custom2DEventTrigger m_eventsTrigger;

        private float m_startAngle;
        private float m_endAngle;
        private float m_curAngle;
        private float m_destinationAngle;
        private float m_interpolationT;
        private int m_currentDirection;

        private void Start() {
            var angle1 = Angle(m_posA.position - m_center.position, Vector3.down);
            var angle2 = Angle(m_posB.position - m_center.position, Vector3.down);

            if (angle1 > angle2) {
                m_startAngle = angle1;
                m_endAngle = angle2;
                m_currentDirection = -1;
            } else {
                m_endAngle = angle1;
                m_startAngle = angle2;
                m_currentDirection = 1;
            }

            m_destinationAngle = m_endAngle;
            m_curAngle = m_center.eulerAngles.z;
        }

		private void Update() {
            if (m_drawTrajectory)
                DrawTrajectory();
            else
                m_lineBuilder.Hide();
        }

		private void FixedUpdate() {
            m_curAngle = Mathf.Lerp(m_curAngle, m_destinationAngle, (m_interpolationT += Time.fixedDeltaTime * m_speed));

            if (m_currentDirection < 0 && m_curAngle <= m_endAngle) {
                m_curAngle = m_endAngle;
                m_destinationAngle = m_startAngle;
                m_currentDirection = 1;
                m_interpolationT = 0f;
            } else if (m_curAngle >= m_startAngle) {
                m_curAngle = m_startAngle;
                m_destinationAngle = m_endAngle;
                m_currentDirection = -1;
                m_interpolationT = 0f;
            }

            m_center.eulerAngles = new Vector3(m_center.eulerAngles.x, m_center.eulerAngles.y, m_curAngle);
        }

		/// <summary>
		/// Add event listeners 
		/// </summary>
		private void OnEnable () {
			m_eventsTrigger.OnCustomTriggerEnter2D += OnCustomTriggerEnter2DHandler;
		}

		/// <summary>
		/// Remove event listeners
		/// </summary>
		private void OnDisable () {
			m_eventsTrigger.OnCustomTriggerEnter2D -= OnCustomTriggerEnter2DHandler;
		}

		/// <summary>
		/// Adding an object to the list on which the Directed force will act
		/// </summary>
		/// <param name="obj">Object.</param>
		private void OnCustomTriggerEnter2DHandler (Collider2D other) {
			if (!CanInteract(other.tag))
				return;
			
			DamagePlayer (m_initialDamage);
		}
		
		/// <summary>
		/// Can gameObject interact with? If leave m_interactedTags empty, gameObject will interact with everything.
		/// </summary>
		/// <param name="tag"></param>
		public bool CanInteract(string tag) {
			return m_interactedTags.Contains(tag);
		}

		/// <summary>
		/// Angle the specified vectorA and vectorB.
		/// </summary>
		/// <param name="vectorA">Vector a.</param>
		/// <param name="vectorB">Vector b.</param>
        private float Angle(Vector3 vectorA, Vector3 vectorB) {
            var angle = Vector3.Angle(vectorA, vectorB);
            var cross = Vector3.Cross(vectorA, vectorB);
            if (cross.z > 0)
	            angle = -angle;

            return angle;
        }

		#region Interface's methods

		/// <summary>
		/// Inflicts the damage to Player.
		/// </summary>
		/// <param name="countDamage">Count damage.</param>
		public void DamagePlayer(float countDamage) {
			Debug.Log("Player get " + countDamage + " damage!");

			/////////////////////////////////////////////////////////
			//TODO IN THIS PLACE YOU SHOULD REDUCE PLAYER'S LIFES ///
			/////////////////////////////////////////////////////////
		}

		/// <summary>
		/// Preparing to deal damage to Player every (specify delay -> m_damageEvery) seconds
		/// </summary>
		public void InflictPermanentDamage() {}//Doesn't do anything because Trap -> Ticker doesn't do permanent damage to the Player

		/// <summary>
		/// Deal damage after (specify damageEvery) seconds
		/// </summary>
		/// <returns>The damage Coroutine.</returns>
		/// <param name="damageEvery">Delay before deal damage.</param>
		public IEnumerator DamageCoroutine(float damageEvery) {
			yield return null; //Doesn't do anything because Trap -> Ticker doesn't do permanent damage to the Player
		}

		#endregion

		/// <summary>
		/// Draws the trajectory(Effect Area) 
		/// WARNING! Trajectory is visible only in play mode. Also you should enable Draw Trajectory in Inspector Window
		/// </summary>
		public void DrawTrajectory() {
			var positions = new List<Vector3> {Vector3.zero};
			var radius = (transform.position - m_sinkerEnd.position).magnitude;
			var positionsCount = (int)(radius * 20f * Mathf.Abs(m_startAngle - m_endAngle) / 360f);
			var angle = m_endAngle - 90f;
			var step = Mathf.Abs(m_startAngle - m_endAngle) / (positionsCount - 1);
			for (var i = 0; i < positionsCount; i++) {
				positions.Add(new Vector3(radius * Mathf.Cos(angle * Mathf.Deg2Rad), radius * Mathf.Sin(angle * Mathf.Deg2Rad), 0f));
				angle += step * Mathf.Sign(m_startAngle - m_endAngle);
			}
			positions.Add(Vector3.zero);

			m_lineBuilder.DrawLine(positions.ToArray(), false);
		}

		/// <summary>
		/// Draws Effect Area when play mode OFF. 
		/// WARNING! You can see GIZMOS only on Scene Window
		/// </summary>
		private void OnDrawGizmos () {			
			//Center
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, 0.1f);
			//Sinker
			Gizmos.DrawWireSphere(m_sinkerEnd.position, 0.1f);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, m_sinkerEnd.position);
			//Range
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, m_posA.position);
			Gizmos.DrawLine(transform.position, m_posB.position);
		}
    }
}
