using UnityEngine;

//! All animation calls should go through a handler for the ease of keeping track.
public class SealieAnimatorHandler : MonoBehaviour
{
	[SerializeField] private Animator targetAnimator;

	public void PlayAnimation(ESealieAnimationName _sealieAnimationName)
	{
		targetAnimator.Play(GetAnimationStringName(_sealieAnimationName));
	}

	public void SetInt(ESealieAnimParameter _animParameter, int value)
	{

	}

	private string GetAnimationStringName(ESealieAnimationName _sealieAnimationName)
	{
		switch(_sealieAnimationName)
		{
			case ESealieAnimationName.Idle:
				return "Idle";
			case ESealieAnimationName.SwimEast:
				return "SwimEast";
			case ESealieAnimationName.SwimWest:
				return "SwimWest";
			case ESealieAnimationName.SwimSouth:
				return "SwimSouth";
			case ESealieAnimationName.SwimNorth:
				return "SwimNorth";
			case ESealieAnimationName.StunnedNorth:
				return "StunnedNorth";
			case ESealieAnimationName.StunnedEast:
				return "StunnedEast";
			case ESealieAnimationName.StunnedWest:
				return "StunnedWest";
			case ESealieAnimationName.StunnedSouth:
				return "StunnedSouth";
			default:
				return "ERROR";
		}
	}

	public enum ESealieAnimationName
	{
		Idle,
		SwimEast,
		SwimWest,
		SwimNorth,
		SwimSouth,
		StunnedNorth,
		StunnedEast,
		StunnedWest,
		StunnedSouth
	}

	public enum ESealieAnimParameter
	{

	}
}
