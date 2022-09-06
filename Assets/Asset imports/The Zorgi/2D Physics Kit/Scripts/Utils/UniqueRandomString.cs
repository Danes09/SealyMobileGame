using UnityEngine;

namespace TheZorgi {	
	public class UniqueRandomString : MonoBehaviour {
		public static string RandomString {
			get {
				var chars = "0123456789abcdefghijklmnopqrstuvwxABCDEFGHIJKLMNOPQRSTUVWXYZ";
				string result = string.Empty;
				for (int i = 0; i < 20; i++) {
					int a = Random.Range(0, chars.Length);
					result = result + chars[a];
				}
				return result;
			}
		}
	}
}
