using UnityEngine;

namespace TheZorgi
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineBuilder : MonoBehaviour {

        [SerializeField] LineRenderer m_lineRenderer;

        void Awake() {
            if(m_lineRenderer == null) {
                m_lineRenderer = GetComponent<LineRenderer>();
            }
        }

        public void DrawLine(Vector3[] positions, bool isWorldSpace) {
            if (!m_lineRenderer.enabled)
                m_lineRenderer.enabled = true;

            m_lineRenderer.useWorldSpace = isWorldSpace;
            m_lineRenderer.positionCount = positions.Length;
            m_lineRenderer.SetPositions(positions);
        }

        public void Hide() {
            if (m_lineRenderer.enabled)
                m_lineRenderer.enabled = false;
        }
    }
}
