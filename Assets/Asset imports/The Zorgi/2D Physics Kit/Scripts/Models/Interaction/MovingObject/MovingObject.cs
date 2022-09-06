using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Interaction {
	
    public class MovingObject : MonoBehaviour, IPreviewElement {

        [Header("Main Settings")]
		[SerializeField] private float m_speed = 1f;
		[SerializeField] private bool m_loopPositions;
		[SerializeField] private PositionType m_positionsType = PositionType.Local;
		[SerializeField] private bool m_drawTrajectory;

        [Header("System")]
		[SerializeField] private LineBuilder m_lineBuilder;
        [SerializeField] private Rigidbody2D m_rigidbody;

        [HideInInspector]
        public List<MovingObjectPosition> Positions = new List<MovingObjectPosition>();

		private const float LERP_ERROR = 0.01f;

		private Vector3 m_nextPosition;
		private int m_currentPositionID;
		private int m_currentDirection = 1;
		private float m_interpolationT;

        public PositionType PositionsType {
            get { 
				return m_positionsType;
			}
        }

        private void Start() {
            m_nextPosition = Positions[0].Position;

            if (m_drawTrajectory) {
                DrawTrajectory();
            }
        }

        private void Update() {
			if (m_drawTrajectory) {
				DrawTrajectory();
			} else {
				m_lineBuilder.Hide();
			}
		}

        private void FixedUpdate() {
            m_interpolationT += Time.fixedDeltaTime;
	        if (m_positionsType == PositionType.World) {
		        m_rigidbody.transform.position = CalculateNextPosition(m_rigidbody.transform.position);

		        if (!((m_rigidbody.transform.position - m_nextPosition).magnitude < LERP_ERROR)) 
			        return;
		        
		        m_rigidbody.transform.position = m_nextPosition;
		        m_interpolationT = 0f;
		        SetNextPosition();
	        }
	        else {
		        m_rigidbody.transform.localPosition = CalculateNextPosition(m_rigidbody.transform.localPosition);

		        if (!((m_rigidbody.transform.localPosition - m_nextPosition).magnitude < LERP_ERROR)) 
			        return;
		        
		        m_rigidbody.transform.localPosition = m_nextPosition;
		        m_interpolationT = 0f;
		        SetNextPosition();
	        }
        }

        private Vector3 CalculateNextPosition(Vector3 position) {
	        return Vector3.Lerp(position, m_nextPosition, m_interpolationT * m_speed);
        }

        /// <summary>
		/// Sets the next position where object should move.
		/// </summary>
        private void SetNextPosition() {
            if (m_loopPositions) {
                if (m_currentPositionID + m_currentDirection < Positions.Count && m_currentPositionID + m_currentDirection > -1) {
                    m_currentPositionID++;
                } else {
                    m_currentPositionID = 0;
                }
            } else {
                if (m_currentDirection > 0) {
                    if (m_currentPositionID + m_currentDirection < Positions.Count) {
                        m_currentPositionID++;
                    } else {
                        m_currentPositionID = Positions.Count - 1;
                        m_currentDirection = -1;
                    }
                } else {
                    if (m_currentPositionID + m_currentDirection >= 0) {
                        m_currentPositionID--;
                    } else {
                        m_currentPositionID = 0;
                        m_currentDirection = 1;
                    }
                }
            }

            m_nextPosition = Positions[m_currentPositionID].Position;
        }

		/// <summary>
		/// Draws the trajectory(Effect Area) 
		/// WARNING! Trajectory is visible only in play mode. Also you should enable Draw Trajectory in Inspector Window
		/// </summary>
        public void DrawTrajectory() {
            var positions = new List<Vector3>();
		    Positions.ForEach(element => {
		        positions.Add(element.Position);
		    });
			
            if (m_loopPositions && Positions.Count > 0) {
                positions.Add(Positions[0].Position);
            }
			
            m_lineBuilder.DrawLine(positions.ToArray(), m_positionsType == PositionType.World);
        }

		/// <summary>
		/// Draws Effect Area when play mode OFF. 
		/// WARNING! You can see GIZMOS only on Scene Window
		/// </summary>
        private void OnDrawGizmos() {
		    if (Application.isPlaying || !m_drawTrajectory || Positions == null || Positions.Count <= 1) 
			    return;
			
		    var positions = new Vector3[Positions.Count];
		    for (var i = 0; i < Positions.Count; i++) {
		        if (m_positionsType == PositionType.Local) {
		            positions[i] = new Vector3(transform.position.x + Positions[i].Position.x * transform.localScale.x, transform.position.y + Positions[i].Position.y * transform.localScale.y, transform.position.y + Positions[i].Position.y * transform.localScale.y);
		        } else {
		            positions[i] = Positions[i].Position;
		        }
		    }

		    Gizmos.color = Color.green;
		    var lastPosition = positions[0];
		    Gizmos.DrawSphere(positions[0], 0.1f);
		    for (var p = 1; p < positions.Length; p++) {
		        Gizmos.DrawLine(lastPosition, positions[p]);
		        lastPosition = positions[p];
		        Gizmos.DrawSphere(positions[p], 0.1f);
		    }
		}
    }
}
