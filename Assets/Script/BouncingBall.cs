using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBall : MonoBehaviour
{
	[SerializeField] private int maximumBounces;
	[Tooltip("The strength of bounce applied to ball when the seal collides with it.")]
	[SerializeField] private float initialMoveSpeed = 10.0f;
	[Tooltip(" For every time the ball hits the water surface, the bounce strength would be scaled by this value geometrically until maximum bounces are reached")]
	[SerializeField] private float bounceForceScaling = 0.7f;
	[SerializeField] private Collider2D damageCollider;
	[SerializeField] private Rigidbody2D rb2d;
	private int currentBounceCount;
	private Vector2 travelDir = Vector2.zero;
	private float curSpeed;
	private bool bounceFromWater;

	public void CollidedWithObstacles(Collision2D collision)
	{
		bounceFromWater = collision.transform.CompareTag("Water");

		if (bounceFromWater && currentBounceCount >= maximumBounces)
		{
			//Debug.Log("Stop");
			ApplyTravelDir(Vector2.zero);
			rb2d.velocity = Vector2.zero;
			currentBounceCount = 0;
			damageCollider.enabled = false;
			bounceFromWater = false;

			return;
		}

		Bounce(collision.contacts[0].normal);

		//! If bounce on water, increment current bounce count
		if (bounceFromWater)
		{
			currentBounceCount++;
		}

		bounceFromWater = false;
	}

	public void CollidedWithSeal(Collision2D collision)
	{
		currentBounceCount = 0;
		ApplyTravelDir((this.transform.position - collision.transform.position).normalized, true);
		damageCollider.enabled = true;

        PlayerManagerScript.Instance.BounceBallEnergy();
    }

	public void CollidedWithDamageable(Collision2D collision)
	{
		Bounce(collision.contacts[0].normal);

        if (collision.gameObject.tag == "Enemy")
        {
            // Damage the enemy if hits enemy. Enemy layer is Damagable.
            collision.gameObject.GetComponent<EnemyManagerScript>().EnemyIsHit(1);
        }
        else if (collision.gameObject.tag == "Object")
        {
            // Collided with any object.
            collision.gameObject.GetComponent<ItemDataScript>().CheckItemData();
        }
    }

	//! Apply force to ball towards the defined direction. 
	//! byScaledVelocity : Should the force applied be based on initialMoveSpeed scaled by number of bounces?
	public void ApplyTravelDir(Vector2 direction, bool byScaledVelocity = false)
	{
		curSpeed = byScaledVelocity ? initialMoveSpeed * (Mathf.Pow(bounceForceScaling, currentBounceCount)) : rb2d.velocity.magnitude;

		//! Force velocity to zero so that the bounce force is consistent
		rb2d.velocity = Vector2.zero;

		rb2d.AddForce(direction * curSpeed, ForceMode2D.Impulse);
	}

	public void Reset()
	{
		currentBounceCount = maximumBounces;
	}

	private void Bounce(Vector2 normal)
	{
		Vector2 dir = (Vector3.Reflect(travelDir, normal) + Utils.CreateNoiseVector(-15,15)).normalized;
		ApplyTravelDir(dir, bounceFromWater);
	}

	private void Start()
	{
		currentBounceCount = maximumBounces;
	}

	private void Update()
	{
		travelDir = rb2d.velocity.normalized;
        CheckForZeroVelo();

    }

    void CheckForZeroVelo()
    {
        if (rb2d.IsSleeping())
        {
            //Debug.Log("BALL STOPPED MOVING SO FORCE IT TO MOVE");
            rb2d.AddForce(new Vector2(Random.Range(-20.0f, 20.0f), 0.0f));
        }
    }
}
