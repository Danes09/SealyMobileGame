using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace TheZorgi.Interaction {
	
    [CustomEditor(typeof(MovingObject))]
    public class MovingObjectEditor : Editor {
        
        private MovingObject m_movingObject;
        private PositionType m_positionType;

        public override void OnInspectorGUI() {
            m_movingObject = (MovingObject)target;

            if (m_movingObject.Positions == null)
                m_movingObject.Positions = new List<MovingObjectPosition>();
            if (m_movingObject.PositionsType == PositionType.World)
                if (m_movingObject.Positions.Count == 0)
                    m_movingObject.Positions.Add(new MovingObjectPosition { Name = "Start Position", Position = m_movingObject.transform.localPosition });
                else
                    m_movingObject.Positions[0] = new MovingObjectPosition { Name = "Start Position", Position = m_movingObject.transform.localPosition };
            else
                if (m_movingObject.Positions.Count == 0)
                m_movingObject.Positions.Add(new MovingObjectPosition { Name = "Start Position", Position = Vector3.zero });
            else
                m_movingObject.Positions[0] = new MovingObjectPosition { Name = "Start Position", Position = Vector3.zero };

            DrawDefaultInspector();
            DrawPositionsInspector();
        }

        private void DrawPositionsInspector() {
            GUILayout.Space(5);
            GUILayout.Label("Positions", EditorStyles.boldLabel);

            for (var p = 0; p < m_movingObject.Positions.Count; p++) {
                DrawPosition(p);
            }
            DrawAddPositionButton();
        }

        private void DrawPosition(int positionIndex) {
            if (positionIndex < 0 || positionIndex >= m_movingObject.Positions.Count)
                return;

            GUILayout.BeginHorizontal(); {
                EditorGUI.BeginChangeCheck();
                var newName = GUILayout.TextField(m_movingObject.Positions[positionIndex].Name, GUILayout.Width(120));
                var newPosition = EditorGUILayout.Vector3Field("", m_movingObject.Positions[positionIndex].Position);

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(m_movingObject, "Modify Position");
                    m_movingObject.Positions[positionIndex].Name = newName;
                    m_movingObject.Positions[positionIndex].Position = newPosition;
                    EditorUtility.SetDirty(m_movingObject);
                }

                if (positionIndex != 0 && GUILayout.Button("Remove")) {
                    if (EditorUtility.DisplayDialog("Warning", "Do you really want to remove position " + m_movingObject.Positions[positionIndex].Name + " ?", "Yes", "No")) {
                        Undo.RecordObject(m_movingObject, "Delete Position");
                        m_movingObject.Positions.RemoveAt(positionIndex);
                        EditorUtility.SetDirty(m_movingObject);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawAddPositionButton() {
            if (GUILayout.Button("Add new Position", GUILayout.Height(30))) {
                Undo.RecordObject(m_movingObject, "Add new Position");
                m_movingObject.Positions.Add(new MovingObjectPosition { Name = "New Position" });
                EditorUtility.SetDirty(m_movingObject);
            }
        }
    }
}
#endif