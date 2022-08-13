using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	[SerializeField] private Transform referenceTransform;
	[SerializeField] private SealieAnimatorHandler animatorHandler;
	[SerializeField] private Animator targetAnimator;
	private float currentAngle;
	private EOrientation currentOrientation;

	[SerializeField] private GameObject sealFront;
	[SerializeField] private GameObject sealSideEast;
	[SerializeField] private GameObject sealSideWest;
	[SerializeField] private GameObject sealBack;

	public void Awake()
	{
		sealSideEast.SetActive(false);
		sealSideWest.SetActive(false);
		sealBack.SetActive(false);
	}

	public void update()
    {
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
			Debug.Log("sealy has been hit");
			targetAnimator.SetTrigger("sealyIsHit");
		}
    }

	public void UpdateOrientation()
	{
		currentAngle = Vector2.SignedAngle(Vector2.up, referenceTransform.up);

		//! Convert signed angle to 360
		if (currentAngle < 0) currentAngle += 360f;

		CheckOrientation();
	}

	public void PlayerStunUpdate(bool _stunned)
	{
		if (_stunned)
			animatorHandler.PlayAnimation(GetStunnedAnimBasedOnOrientation(currentOrientation));
		else
			ToggleDirectionSprite();
	}

	private void CheckOrientation()
	{
		float octantAngle = 360/8;
		int currentOctant = 0;

		while(currentOctant < 8)
		{
			if (currentAngle > 337.5 && currentAngle <= 360)
			{
				currentOrientation = EOrientation.North;
				break;
			}

			//! N (-22.5 > 22.5) NW (22.5 > 67.5) W (67.5 > 112.5) SW (112.5 > 157.5) S (157.5 > 202.5) SE (202.5 > 247.5) E (247.5 > 292.5) NE (292.5 > 337.5)
			if (currentAngle > (octantAngle * currentOctant) - octantAngle/2 && currentAngle <= (octantAngle * (currentOctant + 1)) - octantAngle/2)
			{
				currentOrientation = (EOrientation)currentOctant;
				break;
			}
			currentOctant++;
		}

		ToggleDirectionSprite();
	}

	private void ToggleDirectionSprite()
	{
		//this function is used to activate the correct direction for sealey 
		switch (currentOrientation)
		{
			case EOrientation.North:
				if (sealFront != null)
				{
					targetAnimator = sealFront.GetComponent<Animator>();

					sealFront.gameObject.SetActive(true);
					sealBack.gameObject.SetActive(false);
					sealSideWest.gameObject.SetActive(false);
					sealSideEast.gameObject.SetActive(false);

					// change animation handler 
					animatorHandler = sealFront.gameObject.GetComponent<SealieAnimatorHandler>();
					animatorHandler.PlayAnimation(SealieAnimatorHandler.ESealieAnimationName.SwimNorth);
				}
				break;
			case EOrientation.East:
				if (sealSideEast != null)
				{
					targetAnimator = sealSideEast.GetComponent<Animator>();

					sealFront.gameObject.SetActive(false);
					sealBack.gameObject.SetActive(false);
					sealSideEast.gameObject.SetActive(true);
					sealSideWest.gameObject.SetActive(false);

					// change animation handler
					animatorHandler = sealSideEast.gameObject.GetComponent<SealieAnimatorHandler>();
					animatorHandler.PlayAnimation(SealieAnimatorHandler.ESealieAnimationName.SwimEast);

				}
				break;
			case EOrientation.South:
				if (sealBack != null)
				{
					targetAnimator = sealBack.GetComponent<Animator>();

					sealFront.gameObject.SetActive(false);
					sealBack.gameObject.SetActive(true);
					sealSideEast.gameObject.SetActive(false);
					sealSideWest.gameObject.SetActive(false);

					// change animation handler
					animatorHandler = sealBack.gameObject.GetComponent<SealieAnimatorHandler>();
					animatorHandler.PlayAnimation(SealieAnimatorHandler.ESealieAnimationName.SwimSouth);

				}
				break;
			case EOrientation.West:
				if (sealSideWest != null)
				{
					targetAnimator = sealSideWest.GetComponent<Animator>();

					sealFront.gameObject.SetActive(false);
					sealBack.gameObject.SetActive(false);
					sealSideWest.gameObject.SetActive(true);
					sealSideEast.gameObject.SetActive(false);

					// change animation handler
					animatorHandler = sealSideWest.gameObject.GetComponent<SealieAnimatorHandler>();
					animatorHandler.PlayAnimation(SealieAnimatorHandler.ESealieAnimationName.SwimWest);

				}
				break;
			case EOrientation.NorthEast:
				break;
			case EOrientation.SouthEast:
				Debug.Log("south east");
				break;
			case EOrientation.SouthWest:
				Debug.Log("south west");
				break;
			case EOrientation.NorthWest:
				break;
		}
	}

	private SealieAnimatorHandler.ESealieAnimationName GetStunnedAnimBasedOnOrientation(EOrientation _orientation)
	{
		switch (_orientation)
		{
			case EOrientation.East:
			case EOrientation.SouthEast:
			case EOrientation.NorthEast:
				targetAnimator.SetTrigger("sealyIsHit");
			    return SealieAnimatorHandler.ESealieAnimationName.StunnedEast;
			case EOrientation.West:
			case EOrientation.SouthWest:
			case EOrientation.NorthWest:
				targetAnimator.SetTrigger("sealyIsHit");
				return SealieAnimatorHandler.ESealieAnimationName.StunnedWest;
			case EOrientation.North:
				targetAnimator.SetTrigger("sealyIsHit");
				return SealieAnimatorHandler.ESealieAnimationName.StunnedNorth;
			case EOrientation.South:
				targetAnimator.SetTrigger("sealyIsHit");
				return SealieAnimatorHandler.ESealieAnimationName.StunnedSouth;
			default:
				return SealieAnimatorHandler.ESealieAnimationName.StunnedNorth;
		}
	}

	private enum EOrientation
	{
		North		= 0,
		NorthEast	= 7,
		East		= 6,
		SouthEast	= 5,
		South		= 4,
		SouthWest	= 3,
		West		= 2,
		NorthWest	= 1
	}

	private void container()
    {

    }
}
