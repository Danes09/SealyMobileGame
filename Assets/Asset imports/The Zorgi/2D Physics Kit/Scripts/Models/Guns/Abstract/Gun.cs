using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Guns {
	
	public abstract class Gun : MonoBehaviour {
		
		[Header("Gun main Settings")]
		[SerializeField] private GameObject m_shellPrefab; //prefab of each shell type
		[SerializeField] private Transform m_shellsContainer; //shell container for keeping all shells
		[SerializeField] protected List<string> m_interactedTags; //gun interacted tags
		
		[Header("Gun Settings")]
		[SerializeField] protected float m_fireRate; //in seconds
		
		[Header("Shells parameters")]
		[SerializeField] protected float m_shellSpeed;
		[SerializeField] protected float m_shellExplodeDelay; // delay time before exploding the object(in seconds)
		[SerializeField] protected float m_shellDamage;
		
		[Header("UI settings")]
		[SerializeField] protected bool m_drawTrajectory = true; //show / hide trajectory view
		
		protected bool m_isReadyToFire;
		protected float m_fireTime;
		
		protected abstract void Reload();

		private void Awake() {
			if (m_shellPrefab == null)
				Debug.LogError("ShellPrefab is missing!");
		}

		protected GameObject InstantiateNewShell(Vector3 position, Quaternion rotation) {
			return Instantiate(m_shellPrefab, position, rotation, m_shellsContainer);
		}
	}
}
