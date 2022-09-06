using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace TheZorgi.Traps.Damage {
	
    [CustomEditor(typeof(Charge))]
    public class ChargeEditor : Editor {
		private Charge m_charge;

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			DrawCustomInspector();
        }

        private void DrawCustomInspector() {
            m_charge = (Charge)target;

            EditorGUILayout.Space();
			EditorGUILayout.LabelField("Additional trap settings", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUI.BeginChangeCheck ();
				{
					m_charge.ShowBorders = EditorGUILayout.Toggle ("Show Borders", m_charge.ShowBorders);
								
					if (GUI.changed)
						m_charge.ChangePointsVisibility ();
				}
				EditorGUI.EndChangeCheck ();
			}
			EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
			{
				m_charge.MovementType = (MovementType)EditorGUILayout.EnumPopup ("Movement Type", m_charge.MovementType);
			}
            EditorGUILayout.EndHorizontal();

            if (m_charge.MovementType.Equals(MovementType.CIRCLE)) {
                EditorGUILayout.BeginHorizontal();
				{
					m_charge.DirectionType = (Direction)EditorGUILayout.EnumPopup ("Direction Type", m_charge.DirectionType);
				}
                EditorGUILayout.EndHorizontal();
            }

			if (GUI.changed)
           		EditorUtility.SetDirty(m_charge);
        }       
    }
}
#endif