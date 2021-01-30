using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

// See map.csv
// Represents a 2D array of either a room id or __ (no room)
// Corresponding rooms and their ids are defined in rooms.json, Map.cs and Room.cs
public class MapRoomLayout
{
    private const string CSV_FILE_PATH = "/Data/map.csv";

    // [0,0]...[0,14] is row 1 (empty)
    // [16,0]...[16,14] is the last row (empty)
    // So starting room (11) is at [12,9].
    private string[,] mapLayout;

    public static MapRoomLayout ReadFromCSV()
    {
        string path = Application.dataPath + CSV_FILE_PATH;
        string[][] mapLayoutJagged = File.ReadAllLines(path).Select(line => line.Split(',').ToArray()).ToArray();
        string[,] mapLayout = To2D(mapLayoutJagged);

        Debug.Log("map csv is: " + mapLayout.ToString());

        MapRoomLayout mapRoomLayout = new MapRoomLayout();
        mapRoomLayout.mapLayout = mapLayout;

        return mapRoomLayout;
    }

    private static T[,] To2D<T>(T[][] source)
    {
        // lazy https://stackoverflow.com/questions/26291609/converting-jagged-array-to-2d-array-c-sharp
        try
        {
            int FirstDim = source.Length;
            int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

            var result = new T[FirstDim, SecondDim];
            for (int i = 0; i < FirstDim; ++i)
                for (int j = 0; j < SecondDim; ++j)
                    result[i, j] = source[i][j];

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("The given jagged array is not rectangular.");
        } 
    }
}
