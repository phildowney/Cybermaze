using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{

    public float timeTakenDuringLerp = 1f;
    private GameObject playerObject;

    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;
    private bool _isTransitioning;
    private Vector3 cameraStartingPosition;
    private Vector3 playerStartingPosition;
    private Vector3 playerEndPosition;
    private Vector3 cameraEndPosition = new Vector3(0.5f, 6.5f, -10f);

    public bool isNorthTransition = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (_isTransitioning)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;
            Debug.Log($"percentageComplete: {percentageComplete}");

            Camera.main.transform.position = Vector3.Lerp(cameraStartingPosition, cameraEndPosition, percentageComplete);
            playerObject.transform.position = Vector3.Lerp(playerStartingPosition, playerEndPosition, percentageComplete);
            if (percentageComplete >= 1.0f)
            {
                _isTransitioning = false;
                GlobalData.IsTransitioning = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !GlobalData.IsTransitioning)
        {
            playerObject = GlobalData.PlayerObject;

            Debug.Log("whaddup");
            cameraStartingPosition = Camera.main.transform.position;
            playerStartingPosition = playerObject.transform.position;

            var direction = isNorthTransition ? 1 : -1;

            cameraEndPosition = cameraStartingPosition + new Vector3(0, 6f * direction);
            playerEndPosition = playerStartingPosition + new Vector3(0, 1.55f * direction);

            _timeStartedLerping = Time.time;

            _isTransitioning = true;
            GlobalData.IsTransitioning = true;
        }
    }
}
