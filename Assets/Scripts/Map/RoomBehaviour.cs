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

    public string getId() {
        return room.id;
    }

    public string getBackgroundImage() {
        return room.backgroundImage;
    }

    public Point getCameraLocation() {
        return room.cameraLocation;
    }

    public Point getKeyLocation() {
        return room.keyLocation;
    }

    public Point getDuckLocation() {
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
