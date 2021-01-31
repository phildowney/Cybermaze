using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

// This loads both the Map rooms and the Layout, reconciles them, and we'll want to call it when the scene loads.
// TODO: This seems like a good fit for ScriptableObject, but there's no time for that now.
// https://docs.unity3d.com/Manual/class-ScriptableObject.html
public class MapLoader : MonoBehaviour
{
    public GameObject roomPrefab; 

    public void LoadMap()
    {
        // Load data files
        MapRoomLayout mapRoomLayout = MapRoomLayout.ReadFromCSV();
        Map map = Map.ReadMapFile();
        var roomsById = map.getRoomByIdMap();

        Debug.Log("Done loading map files.");

        // TODO: Where to store this data?

        // Construct behaviours/prefabs from data files
        Dictionary<string, GameObject> roomPrefabs = createRoomPrefabsForData(map.rooms, mapRoomLayout);

        Debug.Log("Done creating Map behaviours.");
    }

    // TODO: Layout on map.
    // TODO: Set positions internally (Walls, etc.)
    // TODO: Anchor to parent gameobject (camera? Scene?)
    // TODO: Set prefab's background image correctly.
    // https://stackoverflow.com/questions/49186166/unity-how-to-instantiate-a-prefab-by-string-name-to-certain-location
    // prefabInstance.transform.position = new Vector3(100, 200, 100);
    // prefabInstance.SetParent(someGameObject.transform);
    // TODO: If a Room prefab is somewhere on a scene, we could pass that in to this class instead of requiring it in Resources
    private Dictionary<string, GameObject> createRoomPrefabsForData(List<Room> rooms, MapRoomLayout mapRoomLayout) {
        Dictionary<string, GameObject> roomPrefabs = new Dictionary<string, GameObject>();

        rooms.ForEach(room => {
            var prefabInstance = UnityEngine.Object.Instantiate<GameObject>(roomPrefab);
            RoomBehaviour roomBehaviour = prefabInstance.GetComponent<RoomBehaviour>();
            roomBehaviour.room = room;
            roomBehaviour.mapArrayPosition = mapRoomLayout.findRoomPosition(room.id);
            roomPrefabs.Add(room.id, prefabInstance);
        });

        return roomPrefabs;
    }
}
