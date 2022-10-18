using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelProgression
{
    // Number of points needed to progress to this new LevelProgression data.
    public int endPointsToProgress;
    public List<GameObject> prevLevelObjects = new List<GameObject>();
    public List<GameObject> levelObjects = new List<GameObject>();
}

public class CameraControlScript : MonoBehaviour
{
    public static CameraControlScript instance;
    public ZoneTransitionManager zoneTransitionManager;
    public GameObject playerBall;
    public GameObject playerObject;
    public GameObject levelBoundary;
    public List<LevelProgression> levelLayers = new List<LevelProgression>();

    public float transitionDuration;

    private Camera mainCam;
    private Vector3 lerpedCamPos;
    private Vector3 lerpedPlayerPos;
    private Vector3 lerpedBoundaryPos;
    private int currentLevelProgression = 0;
    
    // Values are checked from line to line in the scene itself.
    private float cameraHeight = 10.0f;
    private float cameraWidth = 5.62f;

    void Start()
    {
        // Set the main camera for reference.
        mainCam = Camera.main;
    }

    public void CheckReqForProgression(int gameEndPoints)
    {
        if (currentLevelProgression >= levelLayers.Count)
            return;

        if (levelLayers[currentLevelProgression].endPointsToProgress == gameEndPoints)
        {

            // Progress the level upwards because requirements are met.
            ProgressCameraUpwards();
        } 
    }

    void ProgressCameraUpwards()
    {

        // Progress the camera upwards by a set amount (cameraHeight) each time the player...
        // ... completes one part of the level and is ready to progress to the next part of the level.
        currentLevelProgression++;

        // Only progress the level upwards if there are layers in the level.
        if (currentLevelProgression <= levelLayers.Count)
        {
            // Only deactivate if there are objects to deactivate.
            if (levelLayers[currentLevelProgression - 1].prevLevelObjects.Count > 0)
            {
                // Deactivate certain objects in the prev section before level camera moves upwards to next part.
                foreach (GameObject i in levelLayers[currentLevelProgression - 1].prevLevelObjects)
                    i.SetActive(false);
            }

            /*
            // #Critical:
            // Need to activate it when the camera is about to transition upwards.
            // Activate & enable the objects in the next level progression data to begin functioning.
            foreach (GameObject i in levelLayers[currentLevelProgression - 1].levelObjects)
            {
                // Set the deactivated objects to begin functioning.
                i.SetActive(true);
            }
            */

			zoneTransitionManager.ExecuteZoneTransition(()=> ActivateNextArea());
            //StartCoroutine(CameraTransitionUpwards());
        }
    }

    void ActivateNextArea()
    {
        if (levelLayers.Count != 0 || currentLevelProgression <= levelLayers.Count)
        {
            // Need to activate it only when camera has finished lerping upwards.
            // Activate & enable the objects in the next level progression data to begin functioning.
            foreach (GameObject i in levelLayers[currentLevelProgression - 1].levelObjects)
            {
                // Set the deactivated objects to begin functioning.
                i.SetActive(true);
            }
        }
    }

    IEnumerator CameraTransitionUpwards()
    {
        lerpedCamPos = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + cameraHeight, mainCam.transform.position.z);
        lerpedPlayerPos = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y + cameraHeight, playerObject.transform.position.z);
        lerpedBoundaryPos = new Vector3(levelBoundary.transform.position.x, levelBoundary.transform.position.y + cameraHeight, levelBoundary.transform.position.z);
		//! Wait for ball to stop moving
		playerBall.GetComponent<BouncingBall>().Reset();
		yield return new WaitUntil(()=>playerBall.GetComponent<Rigidbody2D>().velocity == Vector2.zero);
		Vector3 lerpedBallPos = new Vector3(playerBall.transform.position.x, playerBall.transform.position.y + cameraHeight, playerBall.transform.position.z);

		// Disable ball bounce here while the water "rises" upwards.
		playerBall.GetComponent<BouncingBall>().enabled = false;
		//! No gravity on ball
		playerBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Don't disable the player movement. Stun the player instead then disable the player movement after the player floats down a lil.
        playerObject.GetComponent<PlayerMovement>().ApplyStun(1.0f);
        yield return new WaitForSeconds(1.2f);
        playerObject.GetComponent<PlayerMovement>().enabled = false;

        float time = 0.0f;
        while (mainCam.transform.position.y < (lerpedCamPos.y - 0.01f))
        {
            Debug.Log(lerpedCamPos.y + " - " + mainCam.transform.position.y);
            time += Time.deltaTime * (Time.timeScale / transitionDuration);
            
            playerObject.transform.position = Vector3.Lerp(playerObject.transform.position, lerpedPlayerPos, time);
            levelBoundary.transform.position = Vector3.Lerp(levelBoundary.transform.position, lerpedBoundaryPos, time);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, lerpedCamPos, time);
			playerBall.transform.position = Vector3.Lerp(playerBall.transform.position, lerpedBallPos, time);
            
            yield return null;
		}
		//! Gravity on ball
		playerBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        
        // Activate the objects in the next area.
        ActivateNextArea();
        yield return 0;
    }
}
