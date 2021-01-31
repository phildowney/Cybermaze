using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Must match Room.cs
// Sadly, there's no C# equivalent of Lombok's @delegate, so we can't just have a Room member and forward/delegate the methods in one line.
// EDIT - Actually, these accessors may not be needed. Keeping for now.
// TODO: This would be better as a ScriptableObject.
public class RoomBehaviour : MonoBehaviour
{
    public Room room { get; set; }

    public Point mapArrayPosition { get; set; }

    // Instantiation copies for inspecting in unityeditor. Don't assign to these. Don't rely on these.
    // See MapLoader.cs#createRoomPrefabsForData()
    public double xPositionCopyForEditor;
    public double yPositionCopyForEditor;
    public string roomIdCopyForEditor;
    public string backgroundImageKeyForEditor;
    public string borderImageKeyForEditor;

    public string getId() {
        return room.id;
    }

    public string CalculateBackgroundImage()
    {
        var north = room.wallUp != string.Empty ? "n" : "";
        var east = room.wallRight != string.Empty ? "e" : "";
        var south = room.wallDown != string.Empty ? "s" : "";
        var west = room.wallLeft != string.Empty ? "w" : "";

        return north + east + south + west;
    }

    public string CalculateRevisedBackgroundImageName()
    {
        string baseImageKey = this.CalculateBackgroundImage();
        return "new_" + baseImageKey;
    }

    public string CalculateRevisedBackgroundImageBorderName()
    {
        string baseImageKey = this.CalculateBackgroundImage();
        return "border_" + baseImageKey;
    }

    public Point getCameraLocation() {
        return room.cameraLocation;
    }

    public Point getKeyLocation() {
        return room.keyLocation;
    }

    public bool getDuckLocation() {
        return room.duckLocation;
    }

    public string getWallUp() {
        return room.wallUp;
    }

    public string getWallLeft() {
        return room.wallLeft;
    }

    public string getWallDown() {
        return room.wallDown;
    }

    public string getWallRight() {
        return room.wallRight;
    }

    public string getRoomUp() {
        return room.roomUp;
    }

    public string getRoomLeft() {
        return room.roomLeft;
    }

    public string getRoomDown() {
        return room.roomDown;
    }

    public string getRoomRight() {
        return room.roomRight;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
