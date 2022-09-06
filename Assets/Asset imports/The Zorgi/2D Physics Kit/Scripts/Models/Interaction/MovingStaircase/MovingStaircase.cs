using UnityEngine;

namespace TheZorgi.Interaction {

	public class MovingStaircase : MonoBehaviour, IGameSound, IGameAnimation {

		[Header("Main Settings")]
        [SerializeField] private float m_force;
        [SerializeField] private float m_rate;

		[HideInInspector][SerializeField] private Direction m_directionType; // Is responsible for direction type

		private AnimateMaterial m_anim;
		private float m_tempRate;
		private float m_tempForce;

        private void Awake() {
			SetDefaultValues ();
			ApplyAnimationMaterial ();
        }

		#region Customer's usage

		/// <summary>
		/// Add Force to the Object
		/// </summary>
		void OnCollisionStay2D(Collision2D collisionInfo) {
			collisionInfo.rigidbody.AddForce(transform.forward * m_tempForce);

			PlayAnimation ();//TODO IN THIS PLACE USE YOUR ANIMATION IF IT NEEDED

			PlaySound ();//TODO IN THIS PLACE PLAY YOUR SOUND IF IT NEEDED
		}

		/// <summary>
		/// Play animation
		/// </summary>
		public void PlayAnimation(){
			//TODO IN THIS PLACE USE YOUR ANIMATION IF IT NEEDED
		}

		/// <summary>
		/// Stop animation
		/// </summary>
		public void StopAnimation(){
			//TODO IN THIS PLACE STOP USE YOUR ANIMATION IF IT NEEDED
		}

		/// <summary>
		/// Play sound
		/// </summary>
		public void PlaySound(){
			//TODO IN THIS PLACE PLAY YOUR SOUND IF IT NEEDED
		}

		/// <summary>
		/// Stop sound
		/// </summary>
		public void StopSound(){
			//TODO IN THIS PLACE STOP YOUR SOUND IF IT NEEDED
		}

		#endregion

		#region Internal usage only

		/// <summary>
		/// Sets the default values.
		/// </summary>
		private void SetDefaultValues() {
			m_tempRate = m_directionType.Equals (Direction.FORWARD) ? -m_rate : m_rate;
			m_tempForce = m_directionType.Equals (Direction.FORWARD) ? m_force : -m_force;
		}

		/// <summary>
		/// Applies the animation material.
		/// </summary>
		private void ApplyAnimationMaterial() {
			m_anim = GetComponentInChildren<AnimateMaterial>();
			m_anim.UVAnimationRateY = m_tempRate;
		}

		/// <summary>
		/// Gets or sets the type of the direction.
		/// </summary>
		public Direction DirectionType {
			get { return m_directionType; }
			set { m_directionType = value; }
		}

		#endregion
    }
}
