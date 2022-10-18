using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTransitionManager : MonoBehaviour
{
	public SimpleEvent ZoneTransitionStarted;
	public GameObject player;
	public GameObject playerball;
	public GameObject levelBounds;
	public GameObject waterBody;
	public GameObject waterSurface;
	public float waterFillTime;
	public float waterFillYOffset = 4f;
	public float playerSwimOffset = 3f;
	public Transform playerSwimToRef;
	public Camera mainCam;
	public float cameraTransitionTime;
	public float cameraHeight;
	private BouncingBall bouncingBall;
	private PlayerMovement playerMovement;
	private Rigidbody2D ballRb2d;

	[ContextMenu("TransitionNextZone")]
	public void ExecuteZoneTransition()
	{
		StartCoroutine(DoTransitionSequence());
	}

	public void ExecuteZoneTransition(System.Action transitionCompleteAction)
	{
		StartCoroutine(DoTransitionSequence(transitionCompleteAction));
	}

	private IEnumerator DoTransitionSequence(System.Action transitionCompleteAction = null)
    {

        ZoneTransitionStarted?.Invoke();
		TapReceiver.gameInputBlocked = true;
		playerMovement.StopPlayer();
        yield return StartCoroutine(DisableBallRoutine());
        yield return StartCoroutine(FillScreen());
        yield return StartCoroutine(SetPlayerLocation());
        StartCoroutine(RepositionPlayerBallWater());
        yield return StartCoroutine(MoveCameraAndBounds());

        bouncingBall.enabled = true;
		ballRb2d.bodyType = RigidbodyType2D.Dynamic;
		playerMovement.enabled = true;
		TapReceiver.gameInputBlocked = false;
        
        transitionCompleteAction?.Invoke();
	}

	IEnumerator DisableBallRoutine()
	{
		bouncingBall.Reset();

		while (ballRb2d.velocity != Vector2.zero)
		{
			yield return null;
		}

		bouncingBall.enabled = false;
		ballRb2d.bodyType = RigidbodyType2D.Static;
	}

	IEnumerator FillScreen()
	{
		float startY = waterBody.transform.position.y;
		
		while (waterBody.transform.position.y < startY + waterFillYOffset)
		{
			waterBody.transform.Translate(Vector3.up * (waterFillYOffset / waterFillTime) * Time.deltaTime);
			waterSurface.transform.Translate(Vector3.up * (waterFillYOffset / waterFillTime) * Time.deltaTime);
			bouncingBall.transform.Translate(Vector3.up * (waterFillYOffset/waterFillTime) * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator SetPlayerLocation()
	{
		Vector3 targetPos = new Vector2(playerSwimToRef.position.x, player.transform.position.y);
		Vector3 startPos = player.transform.position;
		//float alpha = 0;
		
		playerMovement.SetPlayerMoveDirection(targetPos);

		float expectedTimeTaken = Vector3.Magnitude(targetPos - startPos) / 3;

		// while (player.transform.position.x != 0)
		// {
		// 	alpha = 1 - (Mathf.Clamp(Vector3.SqrMagnitude((targetPos - player.transform.position)/Vector3.SqrMagnitude(targetPos - startPos)), 0, 1) * Time.deltaTime * 5);
		// 	player.transform.position = Vector3.Lerp(startPos, targetPos, alpha);
		// 	yield return null;
		// }
		yield return new WaitForSeconds(expectedTimeTaken);

		targetPos = new Vector3(player.transform.position.x, playerSwimToRef.position.y);
		startPos = player.transform.position;

		playerMovement.SetPlayerMoveDirection(targetPos);

		expectedTimeTaken = Vector3.Magnitude(targetPos - startPos) / 3;

		// while (player.transform.position.y != targetPos.y)
		// {
		// 	alpha = 1 - (Mathf.Clamp(Vector3.SqrMagnitude((targetPos - player.transform.position) / Vector3.SqrMagnitude(targetPos - startPos)), 0, 1) * Time.deltaTime * 5);
		// 	player.transform.position = Vector3.Lerp(startPos, targetPos, alpha);
		// 	yield return null;
		// }
		yield return new WaitForSeconds(expectedTimeTaken);
		
		playerMovement.StopPlayer();
		playerMovement.enabled = false;
	}

	IEnumerator MoveCameraAndBounds()
	{
		Vector3 targetCameraPos = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + cameraHeight, mainCam.transform.position.z);

        while (Vector3.Magnitude(targetCameraPos - mainCam.transform.position) > 0.05f)
		{
            mainCam.transform.Translate(Vector2.up * (cameraHeight/cameraTransitionTime) * Time.deltaTime);
			levelBounds.transform.Translate(Vector2.up * (cameraHeight / cameraTransitionTime) * Time.deltaTime);
			player.transform.Translate(Vector2.up * (cameraHeight / cameraTransitionTime) * Time.deltaTime);
			playerball.transform.Translate(Vector2.up * (cameraHeight / cameraTransitionTime) * Time.deltaTime);

			yield return null;
		}
        
        mainCam.transform.position = targetCameraPos;
	}

	IEnumerator RepositionPlayerBallWater()
	{
		float startY = waterBody.transform.localPosition.y;

		while (waterBody.transform.localPosition.y > startY - waterFillYOffset)
		{
			waterBody.transform.Translate(Vector3.down * (waterFillYOffset / waterFillTime) * Time.deltaTime);
			waterSurface.transform.Translate(Vector3.down * (waterFillYOffset / waterFillTime) * Time.deltaTime);
			bouncingBall.transform.Translate(Vector3.down * (waterFillYOffset / waterFillTime) * Time.deltaTime);
			player.transform.Translate(Vector3.down * (waterFillYOffset / waterFillTime) * Time.deltaTime);
			yield return null;
		}
	}

	private void Start()
	{
		bouncingBall = playerball.GetComponent<BouncingBall>();
		playerMovement = player.GetComponent<PlayerMovement>();
		ballRb2d = playerball.GetComponent<Rigidbody2D>();
		mainCam = Camera.main;
	}
}

/*
 * 1. Wait for ball to stop bouncing and turn off ball physics
 * 2. Move water and ball upwards until water fills the screen
 * 3. Move player character horizontally to the center of the screen and then upwards
 * 4. Move camera and level bounds upwards and move player character, water, and ball 
 		downwards to the original y level
 * 5. Enable ball physics, activate next area
 */
