using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GlobalData
    {
        public static bool IsKilled = false;

        public static TileTypes SelectedTileType { get; set; }
        public static SerializableDictionary<Point, CachedTile> GridTilesByCoordinates { get; set; }
        public static Vector3 PlayerLocation { get; set; }

        public static Character Player { get; set; }

        public static int KeyCount { get; set; }
        public static string SerializedLevel { get; set; }

        public static Level LoadedLevel { get; set; }
        public static Level[] Levels { get; set; }
        public static string SecureToken { get; set; }

        // TODO: Be consistent about TopDown vs Topdown (I suggest the former)
        public static MazeBuilder.PlaceableTile[] TopDownTileset
        {
            get
            {
                var allSprites = Resources.LoadAll<Sprite>("Sprites");

                if (allSprites.Length == 0) return null;

                var floor = allSprites.First(s => s.name == "floor");
                var exitTile = allSprites.First(s => s.name == "exit");
                var key = allSprites.First(s => s.name == "blueKey");
                var keyButton = allSprites.First(s => s.name == "blueKeyButton");
                var door = allSprites.First(s => s.name == "door");

                return new[]
                {
                    new MazeBuilder.PlaceableTile
                    {
                        ButtonSprite = floor,
                        ButtonText = "Floor Tile",
                        TileSprite = floor,
                        TileType = TileTypes.Floor
                    },
                    new MazeBuilder.PlaceableTile
                    {
                        ButtonSprite = exitTile,
                        ButtonText = "Exit\nTile",
                        TileSprite = exitTile,
                        TileType = TileTypes.Exit
                    },
                    new MazeBuilder.PlaceableTile
                    {
                        ButtonSprite = keyButton,
                        ButtonText = string.Empty,
                        TileSprite = key,
                        TileType = TileTypes.Key
                    },
                    new MazeBuilder.PlaceableTile
                    {
                        ButtonSprite = door,
                        ButtonText = string.Empty,
                        TileSprite = door,
                        TileType = TileTypes.Door
                    }
                };
            }
        }

        public static MazeBuilder.PlaceableTile[] PlatformerTileset
        {
            get
            {
                var allSprites = Resources.LoadAll<Sprite>("Sprites");

                if (allSprites.Length == 0) return null;

                var white = allSprites.First(s => s.name == "tile");
                var g2 = allSprites.First(s => s.name == "tile2");

                return new[]
                {
                    new MazeBuilder.PlaceableTile
                    {
                        ButtonSprite = white,
                        ButtonText = "Ground Tile",
                        TileSprite = white,
                        TileType = TileTypes.Ground
                    },
                    new MazeBuilder.PlaceableTile
                    {
                        ButtonSprite = g2,
                        ButtonText = "Ground Tile 2",
                        TileSprite = g2,
                        TileType = TileTypes.Ground2
                    }
                };
            }
        }
    }
}
