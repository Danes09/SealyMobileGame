using System;
using UnityEngine;
using UnityEngine.Events;

/*
 * Script: CollisionEventHandler
 * Purpose: Expose Collision2D events to other components, so that colliders does not have to be put in the same gameobject as whichever script that reacts to the collision.
 * Example: Check Player character gameobject, this script can be found in the WallCollider child object
*/
[RequireComponent(typeof(Collider2D))]
public class CollisionEventHandler : MonoBehaviour
{
	public EventCollisionEnter2D CollisionEnter2D;
	public EventCollisionExit2D CollisionExit2D;
	public EventCollisionExit2D CollisionStay2D;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		CollisionEnter2D?.Invoke(collision);
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		CollisionExit2D?.Invoke(collision);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		CollisionStay2D?.Invoke(collision);
	}
}

[Serializable]
public class EventCollisionEnter2D : UnityEvent<Collision2D>
{

}

[Serializable]
public class EventCollisionExit2D : UnityEvent<Collision2D>
{

}

[Serializable]
public class EventCollisionStay2D : UnityEvent<Collision2D>
{

}