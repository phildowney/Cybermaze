using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Room
{
    public string id;

    public string backgroundImage;

    [SerializeReference]
    public Assets.Scripts.NullablePoint cameraLocation;
    
    [SerializeReference]
    public Assets.Scripts.NullablePoint keyLocation;

    [SerializeReference]
    public Assets.Scripts.NullablePoint duckLocation;

    // TODO: Convert to enum
    public string wallUp;

    public string wallLeft;

    public string wallDown;

    public string wallRight; 

    [SerializeReference]
    public Room roomUp;

    [SerializeReference]
    public Room roomLeft;

    [SerializeReference]
    public Room roomDown;

    [SerializeReference]
    public Room roomRight;

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

    public Room Clone() {
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

    private string uuidOrNull(Room room) {
        return room == null ? "null" : room.id;
    }
}
