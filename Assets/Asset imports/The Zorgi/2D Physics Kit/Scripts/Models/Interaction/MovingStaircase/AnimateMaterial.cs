using UnityEngine;

namespace TheZorgi.Interaction {

    public class AnimateMaterial : MonoBehaviour {

		private Vector2 m_uvAnimationRate;
		private Vector2 m_uvOffset = Vector2.zero;
		private Renderer m_rend;

        void Awake() {
            m_rend = GetComponent<Renderer>();
        }

        void LateUpdate() {
			m_uvOffset += (m_uvAnimationRate * Time.deltaTime);

            if (m_rend.enabled)  
                m_rend.materials[0].SetTextureOffset("_MainTex", m_uvOffset);
        }

		/// <summary>
		/// Gets or sets the UV animation rate.
		/// </summary>
		public float UVAnimationRateY {
			set { m_uvAnimationRate.y = value; }
		}
    }
}
