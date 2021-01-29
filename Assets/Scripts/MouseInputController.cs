using Assets.Scripts;
using UnityEngine;

public class MouseInputController
{
    private Vector3 press;
    private Vector3 delta = new Vector3(0, 0);

    public void HandleMazeBuilderMouseInput(IMazeBuilder mazeBuilder, Camera playerCamera, Vector3 mouseScreenPosition)
    {

        if (Input.GetMouseButtonDown(2))
        {
            var mouseWorldPosition = playerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, playerCamera.nearClipPlane));
            press = mouseWorldPosition + delta;
        }

        if (Input.GetMouseButton(2))
        {
            var mouseWorldPosition = playerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, playerCamera.nearClipPlane));

            delta = press - mouseWorldPosition;
        }

        playerCamera.transform.position = playerCamera.transform.position.Add(delta);

        if (Input.GetMouseButton(0))
        {
            var mouseWorldPosition = playerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, playerCamera.nearClipPlane));

            mazeBuilder.AddTileAtWorldPosition(mouseWorldPosition);
        }

        if (Input.GetMouseButton(1))
        {
            var mouseWorldPosition = playerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, playerCamera.nearClipPlane));
            mazeBuilder.RemoveTileAtWorldPosition(mouseWorldPosition);
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize -= scroll * 5;
        }
    }
}
