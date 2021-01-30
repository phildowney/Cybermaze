using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

// This loads both the Map rooms and the Layout, reconciles them, and we'll want to call it when the scene loads.
public class MapLoader
{
    public void LoadMap()
    {
        MapRoomLayout mapRoomLayout = MapRoomLayout.ReadFromCSV();
        Map map = Map.ReadMapFile();
        var roomsById = map.getRoomByIdMap();

        Debug.Log("Done loading map.");
    }
}
