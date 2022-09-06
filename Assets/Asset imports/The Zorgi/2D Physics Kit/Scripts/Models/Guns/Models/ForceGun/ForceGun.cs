using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using TheZorgi.Utils;
using UnityEngine;

namespace TheZorgi.Guns {

	public class ForceGun : Gun {

		[Header("Additional ForceGun Settings")]
		[SerializeField] private float m_maxStretch = 1f;
		[SerializeField] private bool m_isAutoFire;
		[SerializeField] private Transform m_firePoint;
		[SerializeField] private Transform m_autoFireDirection;

        [Header("System settings")]
		[SerializeField] private Collider2D m_objectCollider;
		[SerializeField] private CustomCollider2DButton m_collider;
		[SerializeField] private Transform m_center;
		[SerializeField] private LineRenderer m_frontSwingLine;
		[SerializeField] private LineRenderer m_backSwingLine;
		[SerializeField] private LineRenderer m_trajectoryLine;

		private Ray m_rayToDragPosition;
		private Vector3 m_swingPosition;
		private Vector3 m_touchPosition;
		private Vector3 m_touchPositionCorrected;
		private Vector3 m_stretchVector;

		private const int STRETCH_STEPS = 50;
		private const int FRONT_SWING_LINE_SORTING_ORDER = 16;
		private bool m_isClicked;
		private float m_maxStretchSqr;

		private ForceGunShell m_lastShell;

		/// <summary>
		/// Add event listeners 
		/// </summary>
		private void OnEnable() {
            m_collider.OnButtonDown += OnButtonDownHandler;
            m_collider.OnButtonUp 	+= OnButtonUpHandler;
            m_collider.OnButtonHold += OnDragHandler;
        }

		/// <summary>
		/// Remove event listeners
		/// </summary>
		private void OnDisable() {
            m_collider.OnButtonDown -= OnButtonDownHandler;
            m_collider.OnButtonUp 	-= OnButtonUpHandler;
            m_collider.OnButtonHold -= OnDragHandler;
        }

		private void Start () {
	        m_frontSwingLine.sortingOrder = FRONT_SWING_LINE_SORTING_ORDER;
		}

		private void Update () {
			if (m_isAutoFire && m_fireTime >= m_fireRate) {
				Reload ();
				AutoFire ();
				m_fireTime = 0f;
			}
			m_fireTime += Time.deltaTime;

			if (!m_isAutoFire) {
				StopCoroutine("AutoFireCoroutine");
			}

			if (!m_isClicked || !m_isReadyToFire) 
				return;
			
			OnDragHandler ();
			UpdateSlings(); 
			if (m_drawTrajectory)
				UpdateLineRenderer ();
		}
		
		/// <summary>
		/// Start Auto Fire mode
		/// </summary>
		private void AutoFire () {
			StartCoroutine (AutoFireCoroutine (m_fireRate / 2f));
		}

		/// <summary>
		/// Auto the fire coroutine.
		/// </summary>
		/// <returns>The fire coroutine.</returns>
		/// <param name="fireEvery">Move duration.</param>
		private IEnumerator AutoFireCoroutine (float fireEvery) {
			var shellPosition = m_center.position - (m_autoFireDirection.position - m_center.position).normalized * m_maxStretch;
			OnButtonDownHandler();
			var moveVector = (Vector2)(shellPosition - m_firePoint.position);
			for (var i = 0; i < STRETCH_STEPS; i++) {
				var newFirePoint = (Vector2)m_firePoint.position + moveVector / STRETCH_STEPS;
				m_firePoint.position = newFirePoint;
				m_lastShell.UpdatePosition(m_firePoint.position);
				m_stretchVector = (Vector2)(m_firePoint.position - m_center.position);
				
				yield return new WaitForSeconds (fireEvery / STRETCH_STEPS);
			}
			m_firePoint.position = shellPosition;
			m_stretchVector = (Vector2)m_firePoint.position - (Vector2)m_center.position;
			OnButtonUpHandler ();
		}
		
		private void Fire () {
			var forceVector = (m_center.position - m_firePoint.position).normalized * (m_stretchVector.magnitude / m_maxStretch) * m_shellSpeed;
			m_lastShell.Fire(forceVector);
			m_firePoint.position = m_center.position;
		}
		
		/// <summary>
		/// Reload the ForceGun
		/// </summary>
		protected override void Reload () {
			var shell = InstantiateNewShell(m_firePoint.position, Quaternion.identity).GetComponent<ForceGunShell>();
			InitializeNewShell(shell);
			Reset ();
			m_isReadyToFire = true;
		}

		/// <summary>
		/// Initializes new shell
		/// </summary>
		/// <param name="forceGunShell"></param>
		private void InitializeNewShell(ForceGunShell forceGunShell) {
			forceGunShell.Init(m_firePoint.position, m_shellExplodeDelay, m_shellDamage, m_interactedTags);
			var bulletCollider = forceGunShell.GetComponent<Collider2D>();
			Physics2D.IgnoreCollision(m_objectCollider, bulletCollider, true);
			m_lastShell = forceGunShell;
		}
		
		private void Reset () {
			m_maxStretchSqr = m_maxStretch * m_maxStretch;
			m_touchPosition = m_center.position;
			m_touchPositionCorrected = m_touchPosition;
		}

		/// <summary>
		/// Shows the Trajectory.
		/// </summary>
		private void ShowLineRenderer () {
			m_trajectoryLine.enabled = true;
		}

		/// <summary>
		/// Hides the Trajectory
		/// </summary>
		private void HideLineRenderer () {
			m_trajectoryLine.enabled = false;
		}

		/// <summary>
		/// Updates the Trajectory.
		/// </summary>
		private void UpdateLineRenderer () {
			var trajectory = PhysicsTrajectory.GetTrajectory (CurrentForce, m_firePoint.position);
			m_trajectoryLine.positionCount = trajectory.Count;
			m_trajectoryLine.SetPositions (trajectory.ToArray ());
		}

		/// <summary>
		/// Shows the slings of slingshot.
		/// </summary>
        private void ShowSlings() {
            m_frontSwingLine.enabled = true;
            m_backSwingLine.enabled = true;
        }

		/// <summary>
		/// Hides the slings of slingshot.
		/// </summary>
        private void HideSlings() {
            m_frontSwingLine.enabled = false;
            m_backSwingLine.enabled = false;
        }

		/// <summary>
		/// Update position of the slings
		/// </summary>
        private void UpdateSlings() {
            if (m_firePoint.position == m_swingPosition)
                return;

            m_swingPosition = m_firePoint.position;

            //Swing Front line
            m_frontSwingLine.SetPosition(0, m_frontSwingLine.transform.position);
            m_frontSwingLine.SetPosition(1, m_swingPosition);

            //Swing Back line
            m_backSwingLine.SetPosition(0, m_backSwingLine.transform.position);
            m_backSwingLine.SetPosition(1, m_swingPosition);
        }

		/// <summary>
		/// Raises the button down handler event.
		/// </summary>
		private void OnButtonDownHandler () {
            if (!m_isReadyToFire) 
                return;

			m_isClicked = true;

            ShowSlings();
            if(m_drawTrajectory)
			    ShowLineRenderer ();
		}

		/// <summary>
		/// Raises the button up handler event.
		/// </summary>
		private void OnButtonUpHandler() {
            if (!m_isReadyToFire) {
                Reload();
                return;
            }

			if (m_stretchVector.magnitude < m_maxStretch * 0.1f) {
				m_firePoint.position = m_center.position;
                m_isClicked = false;
                HideLineRenderer();
                HideSlings();
                return;
			}

			m_isClicked = false;
			Fire ();
			HideLineRenderer ();
            HideSlings();
			m_isReadyToFire = false;
		}

		/// <summary>
		/// Drag the slings and release the shell
		/// </summary>
		private void OnDragHandler() {
			if (m_isAutoFire)
				return;

            if (!m_isReadyToFire)
                return;

			m_touchPosition = CameraUtility.GetWorldPositionOnPlane(Input.mousePosition, transform.position.z);
			m_touchPosition.Set (m_touchPosition.x, m_touchPosition.y, 0f);
			m_stretchVector = m_touchPosition - m_center.position;

			m_touchPositionCorrected = m_touchPosition;
			if (m_stretchVector.sqrMagnitude > m_maxStretchSqr) {
				m_rayToDragPosition.origin = m_center.position;
				m_rayToDragPosition.direction = m_stretchVector;
				m_touchPositionCorrected = m_rayToDragPosition.GetPoint (m_maxStretch);
				m_stretchVector = m_touchPositionCorrected - m_center.position;
			}

			m_firePoint.position = m_touchPositionCorrected;
			m_lastShell.UpdatePosition(m_firePoint.position);
		}

		/// <summary>
		/// Gets CurrentForce for drawing trajectory
		/// </summary>
		private Vector2 CurrentForce {
			get {
				return -m_stretchVector.normalized * (m_stretchVector.magnitude / m_maxStretch) * m_shellSpeed;
			}
		}
		
		/// <summary>
		/// Draws Effect Area when play mode OFF. 
		/// WARNING! You can see GIZMOS only on Scene Window
		/// </summary>
		private void OnDrawGizmos() {
            if (!Application.isPlaying && m_drawTrajectory) {
                var positions = PhysicsTrajectory.GetTrajectory(new Vector3(1,1).normalized * m_shellSpeed, m_center.position);
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

            //Drag
            if (Application.isPlaying && m_isReadyToFire) {
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere (m_touchPosition, 0.1f);
				Gizmos.color = Color.green;
				Gizmos.DrawLine (m_center.position, m_touchPositionCorrected);
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine (m_touchPositionCorrected, m_touchPosition);
			}

			//Fire direction
			if (!m_isAutoFire) 
				return;
			
			Gizmos.color = Color.red;
			Gizmos.DrawLine (m_center.position, m_autoFireDirection.position);
		}
    }
}
