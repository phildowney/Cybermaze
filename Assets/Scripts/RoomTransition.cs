using Assets.Scripts;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{

    public float timeTakenDuringLerp = 0.5f;
    private GameObject playerObject;

    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;
    private bool _isTransitioning;
    private Vector3 cameraStartingPosition;
    private Vector3 playerStartingPosition;
    private Vector3 playerEndPosition;
    private Vector3 cameraEndPosition = new Vector3(0.5f, 6.5f, -10f);

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public Direction direction = Direction.North;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (_isTransitioning)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = direction == Direction.North || direction == Direction.South
                ? (timeSinceStarted / timeTakenDuringLerp)
                : (timeSinceStarted / timeTakenDuringLerp) * (9f / 16f);
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

            float cameraDistanceY = 6f;
            float cameraDistanceX = cameraDistanceY * 16f / 9f;
            float playerDistanceY = 1.55f;
            float playerDistanceX = playerDistanceY + 0.666667f;
            switch (direction)
            {
                case Direction.North:
                    cameraEndPosition = cameraStartingPosition + new Vector3(0, cameraDistanceY);
                    playerEndPosition = playerStartingPosition + new Vector3(0, playerDistanceY);
                    break;
                case Direction.East:
                    cameraEndPosition = cameraStartingPosition + new Vector3(cameraDistanceX, 0);
                    playerEndPosition = playerStartingPosition + new Vector3(playerDistanceX, 0);
                    break;
                case Direction.South:
                    cameraEndPosition = cameraStartingPosition + new Vector3(0, -cameraDistanceY);
                    playerEndPosition = playerStartingPosition + new Vector3(0, -playerDistanceY);
                    break;
                case Direction.West:
                    cameraEndPosition = cameraStartingPosition + new Vector3(-cameraDistanceX, 0);
                    playerEndPosition = playerStartingPosition + new Vector3(-playerDistanceX, 0);
                    break;
            }

            _timeStartedLerping = Time.time;

            _isTransitioning = true;
            GlobalData.IsTransitioning = true;
        }
    }
}
