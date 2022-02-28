using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapReceiver : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
	public static bool gameInputBlocked;
	public PlayerMovement playerMoveComponent;
	private InputData latestInput = new InputData();

	private Coroutine inputPauseRoutine;

	private void FixedUpdate()
	{
		ProcessThisFrameInput();
	}

	private void ProcessThisFrameInput()
	{
		if (latestInput.hasNewInput)
		{
			latestInput.ConsumeInput();

			if (gameInputBlocked)
				return;

			if (playerMoveComponent == null)
				return;

			var targetPos = Camera.main.ScreenToWorldPoint(latestInput.inputEventData.position);

			playerMoveComponent.SetPlayerMoveDirection(targetPos);
		}
	}

    public void OnPointerDown(PointerEventData eventData)
    {
		//! This needs to be implemented so that OnPointerUp can work
    }

    public void OnPointerUp(PointerEventData eventData)
	{
		latestInput.UpdateInput(eventData);
	}

	public void SuspendPlayerInput(float duration)
	{
		inputPauseRoutine = StartCoroutine(SuspendPlayerInputRoutine(duration));
	}

	public void ResetInputSuspension()
	{
		latestInput.ConsumeInput();
		if (inputPauseRoutine != null)
			StopCoroutine(inputPauseRoutine);
	}

	private IEnumerator SuspendPlayerInputRoutine(float duration)
	{
		gameInputBlocked = true;

		yield return new WaitForSeconds(duration);

		gameInputBlocked = false;
	}

	private class InputData
	{
		public PointerEventData inputEventData;
		public bool hasNewInput {get; private set;}

		public void UpdateInput(PointerEventData _eventData)
		{
			if(_eventData != null)
			{
				inputEventData = _eventData;
				hasNewInput = true;
			}
		}

		public void ConsumeInput()
		{
			hasNewInput = false;
		}
	}
}
