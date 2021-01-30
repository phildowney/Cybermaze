using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Room
{
    public string id;

    public string backgroundImage;

    public Point cameraLocation;
    
    public Point keyLocation;

    public Point duckLocation;

    // TODO: Convert to enum
    public string wallUp;

    public string wallLeft;

    public string wallDown;

    public string wallRight; 

    public String roomUp;

    public String roomLeft;

    public String roomDown;

    public String roomRight;

    // fuckit I'm outta time!
    public string toString() {
        try {
            return "id: " + id + " backgroundImage: " + backgroundImage + " cameraLocation: " + toStringOrNull(cameraLocation) + " keyLocation: " +
                toStringOrNull(keyLocation) + " duckLocation " + toStringOrNull(duckLocation) + " wallUp " + wallUp + " wallLeft " + wallLeft + " wallDown " +
                wallDown + " wallRight " + wallRight + " roomUp " + uuidOrNull(roomUp) + " roomLeft " + uuidOrNull(roomLeft) +
                " roomDown " + uuidOrNull(roomDown) + " roomRight " + uuidOrNull(roomRight);
        } catch (NullReferenceException e) {
            Debug.Log(e);
            return "not loaded";
        }
    }

    public Room SparseClone() {
        Room room = new Room();

        room.id = (int.Parse(id) + 1).ToString();
        room.backgroundImage = "background_r_" + room.id + ".png";
        room.cameraLocation = cameraLocation;
        room.keyLocation = keyLocation;
        room.duckLocation = duckLocation;
        return room;
    }

    private string toStringOrNull(object obj) {
        return obj == null ? "null" : obj.ToString();
    }

    private string uuidOrNull(String room) {
        return room == null ? "null" : room;
    }
}
