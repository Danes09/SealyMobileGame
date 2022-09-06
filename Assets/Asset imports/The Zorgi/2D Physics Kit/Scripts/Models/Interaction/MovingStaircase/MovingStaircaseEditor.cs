using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace TheZorgi.Interaction {

	[CustomEditor(typeof(MovingStaircase))]
	public class MovingStaircaseEditor : Editor
	{
		private MovingStaircase m_movingStaircase;

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			DrawCustomInspector();
		}

		private void DrawCustomInspector() {
			m_movingStaircase = (MovingStaircase)target;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Additional settings", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();
			{
				m_movingStaircase.DirectionType = (Direction)EditorGUILayout.EnumPopup ("Direction Type", m_movingStaircase.DirectionType);
			}
			EditorGUILayout.EndHorizontal();

			if (GUI.changed)
				EditorUtility.SetDirty(m_movingStaircase);
		}       
	}
}
#endif