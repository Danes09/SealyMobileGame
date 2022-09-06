using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TheZorgi.Utils {

	public class Custom2DEventTrigger : MonoBehaviour {

		public event Action<Collider2D> OnCustomTriggerEnter2D = delegate {};
		public event Action<Collider2D> OnCustomTriggerStay2D = delegate {};
		public event Action<Collider2D> OnCustomTriggerExit2D = delegate {};

        private Collider2D _collider;

        public Collider2D Collider {
            get { 
				return _collider;
			}
        }

		void Awake () {
            _collider = GetComponent<Collider2D>();
        }

		void OnTriggerEnter2D (Collider2D collider) {
			if (OnCustomTriggerEnter2D != null)
				OnCustomTriggerEnter2D (collider);
		}

		void OnTriggerStay2D (Collider2D collider) {
			if (OnCustomTriggerStay2D != null)
				OnCustomTriggerStay2D (collider);
		}

		void OnTriggerExit2D (Collider2D collider) {
			if (OnCustomTriggerExit2D != null)
				OnCustomTriggerExit2D (collider);
		}
	}
}
