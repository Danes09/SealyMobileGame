using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace TheZorgi.Interaction 
{

	[CustomEditor(typeof(UnlockManager))]
	public class UnlockManagerEditor : Editor
	{
		private UnlockManager m_unlockManager;

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			DrawCustomInspector();
		}

		/// <summary>
		/// Draws the custom inspector with additional settings
		/// </summary>
		private void DrawCustomInspector() {
			m_unlockManager = (UnlockManager)target;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Unlock key's settings", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal(); 
			{
				EditorGUILayout.LabelField ("Hidden GameObject");
				m_unlockManager.HiddenGameObject = (GameObject)EditorGUILayout.ObjectField(m_unlockManager.HiddenGameObject, typeof(GameObject), true);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField ("Delay before disappearance");
				m_unlockManager.DisappereanceDelay = EditorGUILayout.FloatField (m_unlockManager.DisappereanceDelay);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Register new key", GUILayout.Height(25))) {
					m_unlockManager.RegisterNewKey ();
				}

				if (GUILayout.Button("Remove all keys", GUILayout.Height(25))) {
					m_unlockManager.RemoveAllKeys ();
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorUtility.SetDirty(m_unlockManager);
		}
	}
}
#endif