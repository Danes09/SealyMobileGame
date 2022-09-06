using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Utils {
	
	public class CustomCollider2DButton : MonoBehaviour {

		public event Action OnButtonDown = delegate {};
		public event Action OnButtonHold = delegate {};
		public event Action OnButtonUp = delegate {};

		void OnMouseDown () {
			if (OnButtonDown != null)
				OnButtonDown ();
		}

		void OnMouseDrag () {
			if (OnButtonHold != null)
				OnButtonHold ();
		}

		void OnMouseUp () {
			if (OnButtonUp != null)
				OnButtonUp ();
		}
	}
}
