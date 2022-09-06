using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Interaction {

	public class UnlockManager : MonoBehaviour, IGameSound, IGameAnimation
	{
		[Header("Main Settings")]
		[SerializeField] private int m_unlockedKeysLimit;
		[SerializeField] private List<string> m_interactedTags;
		[SerializeField] private List<UnlockKey> m_keys = new List<UnlockKey>();

		[HideInInspector][SerializeField] private GameObject m_hiddenGameObject;
		[HideInInspector][SerializeField] private float m_disappereanceDelay;

		private const string UNLOCK_KEY_PATH = "Prefabs/Physics Elements/UnlockKey";

		private int m_collectedKeysCount;

		private void Awake() {
			if (!ValidOptions()) {
				Debug.LogError ("Unlocked keys limit isn't equal keys count!");
			}
			foreach (var key in m_keys) {
				key.AddListener (collectedKey => {//Adds listener for each key. When some key will be collected, event will be fired and code come here
					if(AllKeysCollected())//check if all keys are collected
						return;

					IncrementCollectedKeysCount(); //increment collected keys count
					RemoveCollectedKeyFromList(collectedKey); //removing collected key from the list.
					
					if(AllKeysCollected())//check if all keys are collected. if Yes, reward player, open door, etc.
						Reward();					
				});
			}
		}

		#region Customer's usage

		/// <summary>
		/// Determines whether this instance are all keys collected.
		/// </summary>
		private bool AllKeysCollected() {
			return m_collectedKeysCount >= m_unlockedKeysLimit;
		}

		/// <summary>
		/// Increments the collected keys count.
		/// </summary>
		private void IncrementCollectedKeysCount() {
			m_collectedKeysCount++;

			//TODO IN THIS PLACE YOU CAN UPDATE KEYS COUNTER IN UI, GET ACHIEVEMENT, ETC.
		}

		/// <summary>
		/// Rewards the player.
		/// </summary>
		private void Reward() {
			PlayAnimation ();//TODO IN THIS PLACE USE YOUR ANIMATION IF IT NEEDED

			PlaySound ();//TODO IN THIS PLACE PLAY YOUR SOUND IF IT NEEDED

			Debug.Log("Woohoo! We got all keys!");

			if (m_hiddenGameObject != null)				
				HideGameObject (m_hiddenGameObject);//IN THIS PLACE YOU SHOULD OPEN SOME DOOR, DISABLE TRAP, ETC.
		}

		/// <summary>
		/// Starts animation after player collects key
		/// </summary>
		public void PlayAnimation(){
			//TODO IN THIS PLACE USE YOUR ANIMATION IF IT NEEDED
		}

		/// <summary>
		/// Stops animation when player collects key
		/// </summary>
		public void StopAnimation(){
			//TODO IN THIS PLACE STOP USE YOUR ANIMATION IF IT NEEDED
		}

		/// <summary>
		/// Starts playing sound when player collects the key
		/// </summary>
		public void PlaySound(){
			//TODO IN THIS PLACE PLAY YOUR SOUND IF IT NEEDED
		}

		/// <summary>
		/// Pauses playing sound when player collects the key 
		/// </summary>
		public void PauseSound(){
			//TODO IN THIS PLACE PAUSE YOUR SOUND IF IT NEEDED
		}

		/// <summary>
		/// Stops playing sound when player collects the key
		/// </summary>
		public void StopSound(){
			//TODO IN THIS PLACE STOP YOUR SOUND IF IT NEEDED
		}

		#endregion

		#region Internal usage only

		/// <summary>
		/// Validate the options.
		/// </summary>
		private bool ValidOptions() {
			return m_unlockedKeysLimit == m_keys.Count;				
		}

		/// <summary>
		/// Removes the collected key from list.
		/// </summary>
		private void RemoveCollectedKeyFromList(UnlockKey collectedKey) {
			foreach(var resultKey in m_keys) {
				if(resultKey == null)
					continue;

				if (!resultKey.UniqueName.Equals(collectedKey.UniqueName)) 
					continue;
				
				m_keys.Remove (resultKey);
				break;
			}
		}

		/// <summary>
		/// Registers the new key.
		/// </summary>
		public void RegisterNewKey() {
			var unlockKeyPrefab = Instantiate(Resources.Load(UNLOCK_KEY_PATH)) as GameObject;
			var key = unlockKeyPrefab.GetComponent<UnlockKey>();
			if (key == null) {
				Debug.Log ("Prefab UnlockKey not found!");
				return;
			}
			
			key.transform.parent = transform;
			key.Create (UniqueRandomString.RandomString, m_disappereanceDelay, m_interactedTags);
			m_keys.Add (key);
		}

		/// <summary>
		/// Removes all gameObjects keys from the scene & clear list of keys.
		/// </summary>
		public void RemoveAllKeys() {
			foreach(var resultKey in m_keys) {
				if (resultKey == null)
					continue;
				
				DestroyImmediate (resultKey.gameObject);
			}

			m_keys.Clear ();
		}

		public void HideGameObject(GameObject go) {
			if (go != null)
				go.SetActive (false);
			else
				Debug.LogError ("Hidden object is null! Attach the object to UnlockManager, please!");
		}

		/// <summary>
		/// Gets the keys array.
		/// </summary>
		public List<UnlockKey> Keys {
			get { return m_keys; }
		}

		/// <summary>
		/// Gets or sets the disappereance delay.
		/// </summary>
		public float DisappereanceDelay {
			get { return m_disappereanceDelay; }
			set { m_disappereanceDelay = value; }
		}

		/// <summary>
		/// Gets or sets the hidden game object. It will be hidden when all keys will be collected. 
		/// </summary>
		public GameObject HiddenGameObject {
			get { return m_hiddenGameObject; }
			set { m_hiddenGameObject = value; }
		}

		#endregion
	}
}
