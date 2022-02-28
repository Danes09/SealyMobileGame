using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxSpawner : MonoBehaviour
{
	[SerializeField] private GameObject vfxPrefab;

	public void SpawnVfxOnOrigin()
	{
		Instantiate(vfxPrefab, this.transform.position, Quaternion.identity);
	}

	public void SpawnVfxOnCollisionPoint(Collision2D _collision)
	{
		Instantiate(vfxPrefab, _collision.GetContact(0).point, Quaternion.identity);
	}
}
