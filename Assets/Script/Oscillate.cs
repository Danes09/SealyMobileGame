using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private Transform startPoint;
	[SerializeField] private Transform endPoint;
	private Vector2 currentDirection;
	private bool reflected;

	private void Start()
	{
		currentDirection = ((Vector2)endPoint.position - (Vector2)startPoint.position).normalized;
		this.transform.position = startPoint.position;
	}

	private void Update()
	{
		this.transform.Translate(currentDirection * movementSpeed * Time.deltaTime, Space.World);

		CheckToReflect(reflected?startPoint.position:endPoint.position);
	}

	private void CheckToReflect(Vector2 targetPosition)
	{
		if (Vector2.Distance(targetPosition, this.transform.position) < movementSpeed * Time.deltaTime)
		{
			reflected = !reflected;
			currentDirection = -currentDirection;
		}
	}
}
