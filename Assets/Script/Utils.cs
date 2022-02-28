using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
	public static Vector3 CreateNoiseVector(float minAngle, float maxAngle)
	{
		float xNoise = Random.Range(minAngle, maxAngle);
		float yNoise = Random.Range(minAngle, maxAngle);
		float zNoise = Random.Range(minAngle, maxAngle);

		Vector3 noise = new Vector3(
			Mathf.Sin(2 * Mathf.PI * xNoise / 360),
			Mathf.Sin(2 * Mathf.PI * yNoise / 360),
			Mathf.Sin(2 * Mathf.PI * zNoise / 360)
  		);
		return noise;
	}
}
