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

    private const string EMPTY_CSV_FIELD = "__";

    // [0,0]...[0,14] is row 1 (empty)
    // [16,0]...[16,14] is the last row (empty)
    // So starting room (11) is at [12,9].
    public string[,] mapLayout { get; set; }

    public Point findRoomPosition(string roomId)
    {
        Point index = new Point();

        int w = mapLayout.GetLength(0); // width
        int h = mapLayout.GetLength(1); // height

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
            {
                if (mapLayout[x, y].Equals(roomId))
                {
                    index.X = x;
                    index.Y = y;
                    return index;
                }
            }
        }

        index.X = -1;
        index.Y = -1;

        return index;
    }

    public static MapRoomLayout ReadFromCSV()
    {
        string path = Application.dataPath + CSV_FILE_PATH;
        string[][] mapLayoutJagged = File.ReadAllLines(path).Select(line => line.Split(',').ToArray()).ToArray();
        string[,] mapLayout = To2D(mapLayoutJagged);

        Debug.Log("map csv is: " + mapLayout.ToString());

        sanitizeSingleDigitNumbers(mapLayout);

        MapRoomLayout mapRoomLayout = new MapRoomLayout();
        mapRoomLayout.mapLayout = mapLayout;

        return mapRoomLayout;
    }

    private static void sanitizeSingleDigitNumbers(string[,] map)
    {
        for (int i=0; i < map.GetLength(0); i++)
        {
            for (int j=0; j < map.GetLength(1); j++)
            {
                string cell = map[i,j];
                if (cell != EMPTY_CSV_FIELD)
                {
                    map[i,j] = int.Parse(cell).ToString();
                }
            }
        }
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
