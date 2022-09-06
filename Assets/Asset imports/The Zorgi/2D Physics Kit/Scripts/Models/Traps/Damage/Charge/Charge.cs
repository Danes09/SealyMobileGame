using System.Collections;
using System.Collections.Generic;
using TheZorgi.Utils;
using UnityEngine;

namespace TheZorgi.Traps.Damage {
	
	public class Charge : MonoBehaviour, ITrapDamage, IInteractable, IGameSound, IGameAnimation {
		
		[Header("Trap Settings")]
		[SerializeField] private float m_initialDamage; //The damage which player will get after falling into a trap
		[SerializeField] private float m_speed; //speed of the charge

		[Header("System settings")]
		[SerializeField] private GameObject m_particlesObject;
		[SerializeField] private List<GameObject> m_borders; //Direction points
		[SerializeField] Custom2DEventTrigger m_eventsTrigger;
		[SerializeField] private List<string> m_interactedTags;

		//Additional fields are using by ChargeEditor

		[HideInInspector][SerializeField] private bool m_showBorders = true; //Hides default and destination points
		[HideInInspector][SerializeField] private MovementType m_movementType; // Is responsible for movement type
		[HideInInspector][SerializeField] private Direction m_directionType; // Is responsible for direction type

		private const float LERP_ERROR = 0.01f; //magnitude const

		private Vector3 m_defaultPosition; //first border of the trap behaviour
		private	Vector3 m_nextPosition; //second border of the trap behaviour

		private float m_interpolationT; //trap time interpolation
		private float m_offset;//is responsible for offset of the charge motion

		private void Awake() {
            SetDefaultValues();
        }

		#region Customer's usage

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
		private void OnCustomTriggerEnter2DHandler (Collider2D other) {
			if (!CanInteract(other.tag))
				return;

			PlayAnimation ();//TODO IN THIS PLACE USE YOUR ANIMATION IF IT NEEDED

			PlaySound ();//TODO IN THIS PLACE PLAY YOUR SOUND IF IT NEEDED

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
		/// Inflicts the damage to Player.
		/// </summary>
		public void DamagePlayer(float countDamage) {
			Debug.Log("Player get " + countDamage + " damage!");

			//TODO IN THIS PLACE YOU SHOULD REDUCE PLAYER'S LIFES
		}

		/// <summary>
		/// Preparing to deal damage to Player every (specify delay -> m_damageEvery) seconds
		/// </summary>
		public void InflictPermanentDamage() {}//Doesn't do anything because Trap -> Charge doesn't do permanent damage to the Player

		/// <summary>
		/// Deal damage after (specify damageEvery) seconds
		/// </summary>
		public IEnumerator DamageCoroutine(float damageEvery) {
			yield return null; //Doesn't do anything because Trap -> Charge doesn't do permanent damage to the Player
		}

		/// <summary>
		/// Starts animation after player collects key
		/// </summary>
		public void PlayAnimation(){
			//TODO IN THIS PLACE USE YOUR ANIMATION IF IT NEEDED
		}

		/// <summary>
		/// Stops animation when player collects key
		/// </summary>
		public void StopAnimation(){
			//TODO IN THIS PLACE STOP USE YOUR ANIMATION IF IT NEEDED
		}

		/// <summary>
		/// Starts playing sound when player collects the key
		/// </summary>
		public void PlaySound(){
			//TODO IN THIS PLACE PLAY YOUR SOUND IF IT NEEDED
		}

		/// <summary>
		/// Pauses playing sound when player collects the key 
		/// </summary>
		public void PauseSound(){
			//TODO IN THIS PLACE PAUSE YOUR SOUND IF IT NEEDED
		}

		/// <summary>
		/// Stops playing sound when player collects the key
		/// </summary>
		public void StopSound(){
			//TODO IN THIS PLACE STOP YOUR SOUND IF IT NEEDED
		}

		#endregion

		#region Internal usage only

		/// <summary>
		/// Sets the default values of the x, y and offset
		/// WARNING! Don't try to move left border manually! If you want to change position, change position whole Charge object. Then change right border position.
		/// </summary>
		private void SetDefaultValues() {
			if (m_movementType.Equals(MovementType.BETWEEN_TO_POINTS)) {
				m_defaultPosition = m_borders[0].transform.localPosition;
			}
			if(m_movementType.Equals(MovementType.CIRCLE)) {
				m_defaultPosition = m_borders[1].transform.localPosition - m_borders[0].transform.localPosition;//calculate start position between two points(borders)

				//set offset(radius) in order to Charge goes through each point position
				m_offset = m_defaultPosition.magnitude / 2;

				//set center of the Circle
				m_defaultPosition.x = m_defaultPosition.x / 2;
				m_defaultPosition.y = m_defaultPosition.y / 2;
			}
		}

		/// <summary>
		/// Changes the points visibility.
		/// </summary>
		public void ChangePointsVisibility() {
			m_borders [0].SetActive (m_showBorders);
			m_borders [1].SetActive (m_showBorders);
		}

		/// <summary>
		/// Determines charge behaviour
		/// </summary>
		private void FixedUpdate() {
			m_interpolationT += Time.fixedDeltaTime;

			if (m_movementType.Equals (MovementType.BETWEEN_TO_POINTS)) {
				if (m_particlesObject.transform.localPosition == m_defaultPosition) {
					m_defaultPosition = m_borders [0].transform.localPosition;
					m_nextPosition = m_borders [1].transform.localPosition - m_borders [0].transform.localPosition;
				}

				m_particlesObject.transform.localPosition = Vector3.Lerp (m_particlesObject.transform.localPosition, m_nextPosition, m_interpolationT * m_speed);

				if ((m_particlesObject.transform.localPosition - m_nextPosition).magnitude < LERP_ERROR) {
					m_particlesObject.transform.localPosition = m_nextPosition;
					m_interpolationT = 0f;
					m_nextPosition = m_defaultPosition;
				}
			}
			else { //m_movementType.Equals(MovementType.CIRCLE)
				var x = m_defaultPosition.x;
				var y = m_defaultPosition.y;
				//Doesn't calculate Z because We are using 2D game

				if (m_directionType.Equals(Direction.FORWARD)) {
					//multiply on offset(radius) in order to make circle bigger + offset m_defaultPosition.x
					x = m_offset * Mathf.Sin(m_interpolationT * m_speed) + m_defaultPosition.x;
					y = m_offset * Mathf.Cos (m_interpolationT * m_speed) + m_defaultPosition.y;
				}
				else {
					//multiply on offset(radius) in order to make circle bigger + offset m_defaultPosition.x and y
					x = m_offset * Mathf.Cos (m_interpolationT * m_speed) + m_defaultPosition.x;
					y = m_offset * Mathf.Sin (m_interpolationT * m_speed) + m_defaultPosition.y;
				}
				//Set new Vector3(without Z) position
				m_particlesObject.transform.localPosition = new Vector3(x, y);
			}
		}

		/// <summary>
		/// Draws Effect Area when play mode OFF. 
		/// WARNING! You can see GIZMOS only on Scene Window
		/// </summary>
		private void OnDrawGizmos() {
			if (!Application.isPlaying) {
				Gizmos.color = Color.green;

				if (m_movementType.Equals (MovementType.BETWEEN_TO_POINTS)) {
					Gizmos.DrawLine (m_borders [1].transform.position, m_borders [0].transform.position);
				} 
				else if (m_movementType.Equals (MovementType.CIRCLE)) {

					m_defaultPosition = m_borders[1].transform.position - m_borders[0].transform.position;

					//set offset(radius) in order to Charge GIZMOS goes through each point position
					m_offset = m_defaultPosition.magnitude / 2;

					//set center of the Circle
					m_defaultPosition.x = m_defaultPosition.x / 2;
					m_defaultPosition.y = m_defaultPosition.y / 2;

					float theta = 0;
					//multiply on offset(radius) in order to make circle bigger + offset m_defaultPosition x and y
					float x = m_offset * Mathf.Cos(theta) + m_defaultPosition.x;
					float y = m_offset * Mathf.Sin(theta) + m_defaultPosition.y;

					var pos = transform.position + new Vector3 (x, y);
					var newPos = pos;
					var lastPos = pos;
					for(theta = 0.1f; theta < Mathf.PI * 2f; theta += 0.1f) {
						//multiply on offset(radius) in order to make circle bigger + offset m_defaultPosition.x
						x = m_offset * Mathf.Cos (theta) + m_defaultPosition.x;
						y = m_offset * Mathf.Sin(theta) + m_defaultPosition.y;

						newPos = transform.position + new Vector3(x, y);
						Gizmos.DrawLine(pos, newPos);
						pos = newPos;
					}
					Gizmos.DrawLine(pos, lastPos);
				}
			}
		}

		/// <summary>
		/// Is responsible for displaying borders
		/// </summary>
		public bool ShowBorders {
			get { return m_showBorders; }
			set { m_showBorders = value; }
		}

		/// <summary>
		/// Gets or sets the type of the movement.
		/// </summary>
		public MovementType MovementType {
			get { return m_movementType; }
			set { m_movementType = value; }
		}

		/// <summary>
		/// Gets or sets the type of the direction.
		/// </summary>
		public Direction DirectionType {
			get { return m_directionType; }
			set { m_directionType = value; }
		}

		#endregion
    }
}
