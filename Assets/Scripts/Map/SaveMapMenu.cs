using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Used for testing functions without running the game.
public class SaveMapMenu : MonoBehaviour
{
    /// Add a context menu named "Do Something" in the inspector
    /// of the attached script.
    [MenuItem("Window/Load Data")]
    public static Map LoadData()
    {
        Debug.Log("Loading map json into memory");
        Map map = Map.ReadFromDataJson("/Data/example.json");
        Debug.Log("Map is: " + map.toString());
        return map;
    }

    [MenuItem("Window/Save Data")]
    public static void SaveData()
    {
        Debug.Log("Saving map data to example_written.json");
        Map map = LoadData();
        Map.WriteToDataJson(map, "/Data/example_written.json");
        Debug.Log("Reloaded map is: " + Map.ReadFromDataJson("/Data/example_written.json").toString());
    }

    [MenuItem("Window/Load CSV")]
    public static void LoadCSV()
    {
        Debug.Log("Loading CSV");
        MapRoomLayout.ReadFromCSV();
    }

    [MenuItem("Window/Load Whole Map")]
    public static void LoadWholeMap()
    {
        Debug.Log("Loading Whole Map");
        new MapLoader().LoadMap();
    }

    [MenuItem("Window/write a room")]
    public static void WriteARoom()
    {
//         Debug.Log("Loading Whole Map");
//         Room room = new Room();
//         Assets.Scripts.NullablePoint nullable = new Assets.Scripts.NullablePoint();
// nullable.x = 100.9f;
// nullable.y = 101.2f;
//         room.keyLocation = nullable;
//         Map.WriteARoom(room);
    }

}
