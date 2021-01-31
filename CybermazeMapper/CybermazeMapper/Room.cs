using System;
using System.Collections.Generic;
using System.Drawing;

public class Room
{
    public string id;

    public string backgroundImage;

    public CybermazeMapper.UnityPoint cameraLocation;
    
    public CybermazeMapper.UnityPoint keyLocation;

    public CybermazeMapper.UnityPoint duckLocation;

    // TODO: Convert to enum
    public string wallUp;

    public string wallLeft;

    public string wallDown;

    public string wallRight; 

    public string roomUp;

    public string roomLeft;

    public string roomDown;

    public string roomRight;

    // fuckit I'm outta time!
    public string toString() {
        try {
            return "id: " + id + " backgroundImage: " + backgroundImage + " cameraLocation: " + toStringOrNull(cameraLocation) + " keyLocation: " +
                toStringOrNull(keyLocation) + " duckLocation " + toStringOrNull(duckLocation) + " wallUp " + wallUp + " wallLeft " + wallLeft + " wallDown " +
                wallDown + " wallRight " + wallRight + " roomUp " + roomUp + " roomLeft " + roomLeft +
                " roomDown " + roomDown + " roomRight " + roomRight;
        } catch (NullReferenceException e) {
            Console.WriteLine(e);
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

    public string GetBackgroundImage()
    {
        var north = wallUp != string.Empty ? "n" : "";
        var east = wallRight != string.Empty ? "e" : "";
        var south = wallDown != string.Empty ? "s" : "";
        var west = wallLeft != string.Empty ? "w" : "";

        return north + east + south + west;
    }

    //if (wallUp == string.Empty && wallRight == string.Empty && wallDown == string.Empty && wallLeft == string.Empty)
    //{
    //    return "";
    //}

    //if (wallUp != string.Empty && wallRight == string.Empty && wallDown == string.Empty && wallLeft == string.Empty)
    //{
    //    return "n.png";
    //}

    //if (wallUp != string.Empty && wallRight != string.Empty && wallDown == string.Empty && wallLeft == string.Empty)
    //{
    //    return "ne.png";
    //}

    //if (wallUp != string.Empty && wallRight != string.Empty && wallDown != string.Empty && wallLeft == string.Empty)
    //{
    //    return "new.png";
    //}

    //if (wallUp != string.Empty && wallRight != string.Empty && wallDown != string.Empty && wallLeft != string.Empty)
    //{
    //    return "news.png";
    //}

    //if (wallUp != string.Empty && wallRight == string.Empty && wallDown != string.Empty && wallLeft == string.Empty)
    //{
    //    return "ns.png";
    //}

    //if (wallUp != string.Empty && wallRight == string.Empty && wallDown != string.Empty && wallLeft != string.Empty)
    //{
    //    return "nsw.png";
    //}

    //if (wallUp != string.Empty && wallRight == string.Empty && wallDown != string.Empty && wallLeft != string.Empty)
    //{
    //    return "nsw.png";
    //}
}
