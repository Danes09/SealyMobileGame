using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheZorgi.Traps {
	
	public interface ITrapDamage {

		void DamagePlayer(float countDamage);
		void InflictPermanentDamage();
		IEnumerator DamageCoroutine(float damageEvery);
	}
}
