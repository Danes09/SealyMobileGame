using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Utils {

	public class PhysicsTrajectory {
		private float _dL;

		private const float _dT = 0.1f;
		private const float _maxTrajectoryLength = 100f;
		private const uint _maxTrajectoryIterations = 1000;
		private const int _groundLayerID = 8; //by default

		public static List<Vector3> GetTrajectory (Vector2 initialSpeed, Vector3 initialPosition) {
			List<Vector3> trajectory = new List<Vector3> ();
			float trajectoryLength = 0f;

			float time = 0;
			Vector2 horizont;
			if (initialSpeed.x > 0) {
				horizont = Vector2.right;
			} else {
				horizont = Vector2.left;
			}
			float angle = Vector2.Angle (horizont, initialSpeed) * Mathf.Deg2Rad;
			float Xpart = initialSpeed.magnitude * Mathf.Cos (angle) * Mathf.Sign(initialSpeed.x);
			float Ypart = initialSpeed.magnitude * Mathf.Sin (angle) * Mathf.Sign(initialSpeed.y);

			float x = 0f;
			float y = 0f;

			Vector3 fromPosition = initialPosition;
			Vector3 toPosition = initialPosition;

			RaycastHit2D hit;

			for (int l = 0; l < _maxTrajectoryIterations; l++) {

				if (trajectoryLength > _maxTrajectoryLength)
					break;

				x = initialPosition.x + Xpart * time;
				y = initialPosition.y + Ypart * time + 0.5f * Physics2D.gravity.y * (time * time);
				toPosition.Set (x, y, 0f);
				trajectoryLength += (toPosition - fromPosition).magnitude;

				hit = Physics2D.Raycast ((Vector2)fromPosition, (Vector2)toPosition - (Vector2)fromPosition, 10f, _groundLayerID);
				if (hit.collider != null) {
					toPosition = hit.point;
					trajectory.Add (toPosition);
					break;
				} else {
					trajectory.Add (toPosition);
				}

				fromPosition = toPosition;
				time = time + _dT;
			}

			return trajectory;
		}
	}
}
