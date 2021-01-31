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
    // How big are the Room Prefabs supposed to be, and where do we place them next to each other?
    // e.g. prefab 1 at 0, prefab 2 at 1921, prefab 3 at 1920*3+2, etc.
    // TODO: Scale appropriately?
    private const int ROOM_SIZE_X = 11; // 10 units in editor
    private const int ROOM_SIZE_Y = 6;

    // Place first prefab at these coordinates, then the rest are oriented around that one based on the map CSV.
    private const string MAP_ORIGIN_GAMEOBJECT_NAME = "EntireMap";

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

        GameObject originGameObject = GameObject.Find(MAP_ORIGIN_GAMEOBJECT_NAME);
        Vector3 originPosition = originGameObject.transform.position;
        Point firstRoomPosition = mapRoomLayout.findRoomPosition("1");

        rooms.ForEach(room => {
            var prefabInstance = UnityEngine.Object.Instantiate<GameObject>(roomPrefab);

            // Here's where we configure the prefab to have the room data, the correct image, position it correctly in the world, etc.
            // We could also set the tiles (walls/doors/ducks) here, or do that later.
            RoomBehaviour roomBehaviour = prefabInstance.GetComponent<RoomBehaviour>();
            roomBehaviour.room = room;
            roomBehaviour.mapArrayPosition = mapRoomLayout.findRoomPosition(room.id);
            AssignCorrectPrefabPosition(roomBehaviour, originPosition, firstRoomPosition);

            roomPrefabs.Add(room.id, prefabInstance);
        });

        return roomPrefabs;
    }

    // Orient prefabs around the position of room 1 
    private void AssignCorrectPrefabPosition(
        RoomBehaviour roomBehaviour,
        Vector3 originPosition,
        Point firstRoomPosition
    ) {
        Point mapArrayPosition = roomBehaviour.mapArrayPosition;

        // A room at 3,5 is 3 map sizes to the right of 0, 5 map sizes up from 0, 
        float desiredRoomOriginX = (float)(mapArrayPosition.X * ROOM_SIZE_X);// + mapArrayPosition.X; for off-by-one offset?
        float desiredRoomOriginY = (float)(mapArrayPosition.Y * ROOM_SIZE_Y);// + mapArrayPosition.Y;

        GameObject roomPrefab = roomBehaviour.gameObject;
        Vector3 newPosition = new Vector3(desiredRoomOriginX, desiredRoomOriginY, roomPrefab.transform.position.z);
        roomPrefab.transform.position = newPosition; 
    }
}
