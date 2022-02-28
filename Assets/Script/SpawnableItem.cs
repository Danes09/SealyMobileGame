using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableItem : MonoBehaviour
{
	[SerializeField] private bool destroyWhenOutOfScreen;

	private void OnBecameInvisible()
	{
		if (destroyWhenOutOfScreen)
		{
			Destroy(this.gameObject);
		}
	}
}
