using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Interaction {
	
	[Serializable]
	public class UnlockKey : MonoBehaviour, IInteractable, IGameSound, IGameAnimation {

		[HideInInspector][SerializeField] private float m_disappereanceDelay;
		[HideInInspector][SerializeField] private string m_uniqueName = string.Empty;
		[HideInInspector][SerializeField] private List<string> m_interactedTags = new List<string>(); 
			
		public event Action<UnlockKey> OnCollected = delegate {};

		private bool m_isAlreadyPickedUp;

		#region Customer's usage

		/// <summary>
		/// Expects to be collected
		/// </summary>
		void OnTriggerEnter2D(Collider2D other) {
			if (!CanInteract(other.tag))
				return;

			if(m_isAlreadyPickedUp) //check on duplicate OnTriggerEnter2D
				return;			

			PlayAnimation ();//TODO IN THIS PLACE USE YOUR ANIMATION IF IT NEEDED

			PlaySound ();//TODO IN THIS PLACE PLAY YOUR SOUND IF IT NEEDED

			CollectItem ();

			m_isAlreadyPickedUp = true; //prevents OnTrigger2DEnter from being processed again
		}

		/// <summary>
		/// Player collects the key.
		/// </summary>
		private void CollectItem () {
			OnCollected.Invoke (this);// Firing event, that key was collected. Increment gathered keys count in the UnlockKeyManager.

			//Destroys gameObject after delay.
			//For immediately destroying unlockKey set m_disappereanceDelay = 0f; 
			Destroy (gameObject, m_disappereanceDelay); 
		}

		/// <summary>
		/// Can gameObject interact with? gameObject will interact with player.
		/// </summary>
		/// <param name="tag"></param>
		public bool CanInteract(string tag) {
			return m_interactedTags.Contains(tag);
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
		/// Stops playing sound when player collects the key
		/// </summary>
		public void StopSound(){
			//TODO IN THIS PLACE STOP YOUR SOUND IF IT NEEDED
		}

		#endregion

		#region Internal usage only

		/// <summary>
		/// Creates key with the specified uniqueName and delay.
		/// </summary>
		public void Create(string uniqueName, float delay,  List<string> interactedTags) {
			m_uniqueName = uniqueName;
			m_disappereanceDelay = delay;
			m_interactedTags = interactedTags;
		}

		/// <summary>
		/// Adds the listener.
		/// </summary>
		public void AddListener(Action<UnlockKey> callback) {
			OnCollected = callback;
		}

		/// <summary>
		/// Gets unique name of the Key for deleting it from UnlockManager keys' list.
		/// </summary>
		public string UniqueName {
			get { return m_uniqueName; }
			set { m_uniqueName = value; }
		}

		#endregion
	}
}
