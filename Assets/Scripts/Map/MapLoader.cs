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
    private Sprite[] allSprites;
    private const string FLOOR_NOT_FOUND_SPRITE_NAME = "not-found";
    
    // How big are the Room Prefabs supposed to be, and where do we place them next to each other?
    // e.g. prefab 1 at 0, prefab 2 at 1921, prefab 3 at 1920*3+2, etc.
    // TODO: Scale appropriately?
    private const int ROOM_SIZE_X = 11; // 10 units in editor
    private const int ROOM_SIZE_Y = 6;

    // Place first prefab at these coordinates, then the rest are oriented around that one based on the map CSV.
    private const string MAP_ORIGIN_GAMEOBJECT_NAME = "EntireMap";

    private const string DUCK_CHILD_GAMEOBJECT_NAME = "Kid";

    private const string BORDER_GAMEOBJECT_NAME = "border";

    public GameObject roomPrefab; 

    public void LoadMap()
    {
        // Load floor sprites.
        this.allSprites = Resources.LoadAll<Sprite>("Sprites/REVISED");

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

        rooms.ForEach(room =>
        {
            var prefabInstance = UnityEngine.Object.Instantiate<GameObject>(roomPrefab);
            prefabInstance.transform.SetParent(originGameObject.transform);
            prefabInstance.name = "Room " + room.id;
            prefabInstance.tag = "Room Clone";
            activateWalls(room, prefabInstance);

            // Here's where we configure the prefab to have the room data, the correct image, position it correctly in the world, etc.
            // We could also set the tiles (walls/doors/ducks) here, or do that later.
            RoomBehaviour roomBehaviour = prefabInstance.GetComponent<RoomBehaviour>();
            roomBehaviour.room = room;
            roomBehaviour.roomIdCopyForEditor = room.id;
            roomBehaviour.mapArrayPosition = mapRoomLayout.findRoomPosition(room.id);
            AssignCorrectPrefabPosition(roomBehaviour, originPosition, firstRoomPosition);

            CalculateAndSetBackgroundImage(roomBehaviour);
            CalculateAndSetBackgroundImageBorder(roomBehaviour);
            roomPrefabs.Add(room.id, prefabInstance);
        });

        ConditionalSpawnDuckOnPrefabs(roomPrefabs);

        return roomPrefabs;
    }

    private void CalculateAndSetBackgroundImage(RoomBehaviour roomBehaviour)
    { 
        string imageKey = roomBehaviour.CalculateRevisedBackgroundImageName();
        GameObject roomPrefab = roomBehaviour.gameObject;
        SpriteRenderer renderer = roomPrefab.GetComponent<SpriteRenderer>();
        Sprite floorSprite = Array.Find<Sprite>(
            this.allSprites,
            sprite => sprite.name == imageKey
        );

        if (floorSprite == null) {
            floorSprite = Array.Find<Sprite>(
                this.allSprites,
                sprite => sprite.name == FLOOR_NOT_FOUND_SPRITE_NAME
            );
        }

        renderer.sprite = floorSprite;

        roomBehaviour.backgroundImageKeyForEditor = imageKey;
    }

    private void CalculateAndSetBackgroundImageBorder(RoomBehaviour roomBehaviour)
    {
        string imageKey = roomBehaviour.CalculateRevisedBackgroundImageBorderName();
        
        GameObject roomPrefab = roomBehaviour.gameObject;
        Transform borderTransform = roomPrefab.transform.Find(BORDER_GAMEOBJECT_NAME);

        if (borderTransform == null) {
            Debug.LogError("Could not find border for room " + roomBehaviour.room);
            return;
        }

        GameObject borderObject = borderTransform.gameObject;
        SpriteRenderer renderer = borderObject.GetComponent<SpriteRenderer>();
        
        Sprite borderSprite = Array.Find<Sprite>(
            this.allSprites,
            sprite => sprite.name == imageKey
        );

        if (borderSprite == null) {
            borderSprite = Array.Find<Sprite>(
                this.allSprites,
                sprite => sprite.name == FLOOR_NOT_FOUND_SPRITE_NAME
            );
        }

        renderer.sprite = borderSprite;

        borderTransform.localScale -= new Vector3(0.45f, 0.45f, 1);

        roomBehaviour.borderImageKeyForEditor = imageKey;
    }

    private void activateWalls(Room room, GameObject prefabInstance)
    {
        var door = prefabInstance.transform.Find("DoorWest");
        door.gameObject.SetActive(room.wallLeft != "door");
        door = prefabInstance.transform.Find("DoorEast");
        door.gameObject.SetActive(room.wallRight != "door");
        door = prefabInstance.transform.Find("DoorSouth");
        door.gameObject.SetActive(room.wallDown != "door");
        door = prefabInstance.transform.Find("DoorNorth");
        door.gameObject.SetActive(room.wallUp != "door");
    }

    private void ConditionalSpawnDuckOnPrefabs(Dictionary<string, GameObject> roomPrefabs)
    {
        foreach (var roomPrefab in roomPrefabs.Values)
        {
            bool isActive = roomPrefab.GetComponent<RoomBehaviour>().getDuckLocation();
            Transform duckTransform = roomPrefab.transform.Find(DUCK_CHILD_GAMEOBJECT_NAME);
            if (duckTransform != null)
            {
                GameObject duck = duckTransform.gameObject;
                duck.SetActive(isActive);
            }
        }
    }

    /**
    * Alright, so here's what's going on.
    *
    * The map CSV is an array where the top-left is 0,0, the bottom left is 0,16, the top-right is 15,0
    * and the bottom-right is 15,16.
    *
    * However, the 2D array in csharp is actually [y,x].
    * Then, finally, the Unity coordinate system is bottom-left = 0,0, and top-right = 15,16.
    *
    * So to convert our array coordinates to Unity coordinates, we need to flip the array (Y,X) to X,Y,
    * and then we need to invert the Y-values by subtracting from 1 (because in our array, increasing Y goes 'down',
    * but in Unity, increasing Y goes upwards.)
    *
    * We multiply the "Y in array, but it's actually X" by the X-multiplier, and the "X in the array, but it's actually Y"
    * by the Y multiplier.
    *
    * You can instead thing of roomArrayPosition.X as roomArrayPosition.firstArray, which is the row/y-value array.
    * Vice-versa for X.
    */
    public Point CalculateUnityPosition(Point roomArrayPosition)
    {
        // NOTE: The array is actually [Y,X] so we need to flip them when converting to Unity space!

        // A room at 3,5 is 3 map sizes to the right of 0, 5 map sizes up from 0, 
        float desiredRoomOriginX = (float)(roomArrayPosition.Y * ROOM_SIZE_X);// + mapArrayPosition.X; for off-by-one offset?
        float desiredRoomOriginY = (float)((1 - roomArrayPosition.X) * ROOM_SIZE_Y);// + mapArrayPosition.Y;
        return new Point { X = desiredRoomOriginX, Y = desiredRoomOriginY };
    }

    // The CSV is a 0-indexed array, topleft [0,0] to bottom right [0,0],
    // but the Unity coordinate system is bottom left [0,0] to top-right [99,99]
    // We need to invert our CSV y-value when calculating where to place the rooms.
    private void AssignCorrectPrefabPosition(
        RoomBehaviour roomBehaviour,
        Vector3 originPosition,
        Point firstRoomPosition
    ) {
        Point mapArrayPosition = roomBehaviour.mapArrayPosition;
        Point unityPosition = CalculateUnityPosition(mapArrayPosition);

        GameObject roomPrefab = roomBehaviour.gameObject;
        Vector3 newPosition = new Vector3((float)unityPosition.X, (float)unityPosition.Y, roomPrefab.transform.position.z);
        roomPrefab.transform.position = newPosition;

        roomBehaviour.xPositionCopyForEditor = roomBehaviour.mapArrayPosition.X;
        roomBehaviour.yPositionCopyForEditor = roomBehaviour.mapArrayPosition.Y;
    }
}
