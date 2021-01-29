using Assets.Scripts;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseInputController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO: Not a static method, etc.
    public static void HandePlayerMouseInput()
    {
    }

    public static void HandleMazeBuilderMouseInput(Vector3 mouseScreenPosition, Camera PlayerCamera, Vector3 press, Vector3 delta, IMazeBuilder mazeBuilder)
    {
        #region Tile Creation

        if (Input.GetMouseButtonDown(2))
        {
            var mouseWorldPosition = PlayerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, PlayerCamera.nearClipPlane));
            press = mouseWorldPosition + delta;
        }

        if (Input.GetMouseButton(2))
        {
            var mouseWorldPosition = PlayerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, PlayerCamera.nearClipPlane));

            delta = press - mouseWorldPosition;

            print("Delta" + delta);
        }

        PlayerCamera.transform.position = PlayerCamera.transform.position.Add(delta);

        if (Input.GetMouseButton(0))
        {
            var mouseWorldPosition = PlayerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, PlayerCamera.nearClipPlane));

            mazeBuilder.AddTileAtWorldPosition(mouseWorldPosition);
        }

        if (Input.GetMouseButton(1))
        {
            var mouseWorldPosition = PlayerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, PlayerCamera.nearClipPlane));
            mazeBuilder.RemoveTileAtWorldPosition(mouseWorldPosition);
        }

        #endregion

        #region Zooming

        var scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize -= scroll * 5;
        }

        #endregion
    }
}
