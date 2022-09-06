using System;
using UnityEngine;

namespace TheZorgi.Utils {
	
	public class CustomCollision2DEventTrigger : MonoBehaviour {

		public event Action<Collision2D> OnCustomCollisionEnter2D = delegate {};
		public event Action<Collision2D> OnCustomCollisionExit2D = delegate {};
		public event Action<Collision2D> OnCustomCollisionStay2D = delegate {};

		void OnCollisionEnter2D (Collision2D collision) {
			if (OnCustomCollisionEnter2D != null)
				OnCustomCollisionEnter2D (collision);
		}

		void OnCollisionStay2D (Collision2D collision) {
			if (OnCustomCollisionStay2D != null)
				OnCustomCollisionStay2D (collision);
		}

		void OnCollisionExit2D (Collision2D collision) {
			if (OnCustomCollisionExit2D != null)
				OnCustomCollisionExit2D (collision);
		}
	}
}
