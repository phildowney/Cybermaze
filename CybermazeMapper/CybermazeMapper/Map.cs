using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

// This calss is the rooms.json file. It is the set of data in the map, but it is not the Layout of that data!

public class Map
{
    public string startRoom;

    public List<Room> rooms;

    private Dictionary<string, Room> roomById = new Dictionary<string, Room>();

    // public string toString() {
    //    StringBuilder sb = new StringBuilder();

    //    foreach (var room in rooms)
    //    {
    //        sb.Append(room.toString());
    //    }

    //    return "Start room: " + startRoom + ", rooms: " + sb.ToString();
    //}

    //// Assumption: This is all copy-by-reference so we're referring to the same Room objects.
    //public Dictionary<string, Room> getRoomByIdMap() {
    //    if (roomById.Count == 0) {
    //        rooms.ForEach(room => roomById.Add(room.id, room));
    //    }

    //    return roomById;
    //}

    //public static Map ReadMapFile() {
    //    string path = Application.dataPath + "/Data/rooms.json";
    //    StreamReader reader = new StreamReader(path); 
    //    string jsonData = reader.ReadToEnd();
    //    reader.Close();

    //    Debug.Log("json is: " + jsonData);

    //    Map map = JsonUtility.FromJson<Map>(jsonData);

    //    // Fix the single-digit room indexes. DON'T @ ME NERDS.
    //    map.rooms.ForEach(room => room.id = int.Parse(room.id).ToString());

    //    return map;
    //}

    ////
    //// Ignore these, for testing/data gen.
    // public static void WriteToDataJson(Map map, string fileName) {
    //    Debug.Log("Want to write: " + map.toString());
    //    string path = Application.dataPath + fileName;

    //    Room room = map.rooms.First();

    //    for (int i = 2; i < 99; ++i) {
    //        room = room.SparseClone();
    //        map.rooms.Add(room);
    //    }

    //    string outputJson = JsonUtility.ToJson(map, true);

    //    Debug.Log("Output json is " + outputJson);

    //    StreamWriter writer = new StreamWriter(path);
    //    writer.Write(outputJson);
    //    writer.Close();

    //    Debug.Log("Done writing.");
    //}

    //public static void WriteARoom(Room room) {
    //    string path = Application.dataPath + "/Data/room.json";

    //    string outputJson = JsonUtility.ToJson(room, true);
    //     StreamWriter writer = new StreamWriter(path);
    //    writer.Write(outputJson);
    //    writer.Close();
    //}

    //public static Map ReadFromDataJson(string fileName) {
    //    string path = Application.dataPath + fileName;
    //    StreamReader reader = new StreamReader(path); 
    //    string jsonData = reader.ReadToEnd();
    //    reader.Close();

    //    Debug.Log("json is: " + jsonData);

    //    return JsonUtility.FromJson<Map>(jsonData);
    //}
}
