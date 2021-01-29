using Assets.Scripts;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// HAY READ THIS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// You need to have a specific aspect ratio with pixel dimensions set for this to work AT ALL.

public interface IMazeBuilder
{
    void AddTileAtWorldPosition(Vector3 mouseWorldPosition);
    void RemoveTileAtWorldPosition(Vector3 mouseWorldPosition);
}

public class MazeBuilder : MonoBehaviour, IMazeBuilder
{
    public int count = 0;
    public bool Locked = true;

    public GameObject WallTilePrefab;
    public GameObject ExitTilePrefab;
    public Camera PlayerCamera;
    public GameObject PlayerPrefab;
    public GameObject KeyPrefab;
    public GameObject DoorPrefab;
    public GameObject StartPrefab;
    public GameObject TileButtonPrefab;
    public GameObject GroundPrefab;
    public GameObject Ground2Prefab;
    public Canvas Canvas;
    public PlaceableTile[] PlaceableTiles;

    // TODO: a better way to switch plz?
    public bool Platformer = false;

    private const int TileScreenSize = 64;

    private GameObject _playerTile;

    // Use this for initialization
    void Start()
    {
        GlobalData.SelectedTileType = PlaceableTiles.First().TileType;

        for (int i = 0; i < PlaceableTiles.Length; i++)
        {
            var tile = PlaceableTiles[i];
            var tileButton = Instantiate(TileButtonPrefab, new Vector3(0f, 0f, 0f), new Quaternion(), Canvas.transform);

            var size = 75f;
            var spacing = 15f;
            var offset = spacing + size / 2f;

            //tileButton.transform.localScale = new Vector3(size, size, 1f);

            ((RectTransform)tileButton.transform).anchoredPosition = new Vector3(offset + (size + spacing) * i, offset);
            tileButton.GetComponent<Image>().sprite = tile.ButtonSprite;
            tileButton.GetComponent<Button>().onClick.AddListener(() => { GlobalData.SelectedTileType = tile.TileType; });
            tileButton.GetComponentInChildren<Text>().text = tile.ButtonText; // TODO:  Newline support? Mebbe?
            tileButton.name = tile.TileType.ToString() + "TileButton";
            print(tileButton.GetComponentInChildren<Text>().transform.localScale);
        }

        // Load the level

        if (GlobalData.SerializedLevel != null)
        {
            ClearLevel();

            print(GlobalData.SerializedLevel);
            var unescaped = GlobalData.SerializedLevel.Replace(@"\", string.Empty); // TODO: Find a better way to unescape this?
            GlobalData.GridTilesByCoordinates = JsonUtility.FromJson<TileDictionary>(unescaped);

            DrawLevel();
        }
        else
        {
            if (GlobalData.GridTilesByCoordinates == null)
            {
                GlobalData.GridTilesByCoordinates = new SerializableDictionary<Point, CachedTile>();
            }
            else
            {
                DrawLevel();
            }
        }

        _playerTile = (GameObject)Instantiate(PlayerPrefab, GlobalData.PlayerLocation, new Quaternion());

        Instantiate(StartPrefab, new Vector3(0f, 0f, 0f), new Quaternion());

        FindObjectOfType<Camera>().GetComponentInChildren<SmoothCamera2D>().target = _playerTile.transform;

        var KeysText = GameObject.FindGameObjectWithTag("KeysTextTag").GetComponent<Text>();
        KeysText.text = string.Format("Keys: {0}", GlobalData.KeyCount);
    }

    private void DrawLevel()
    {
        foreach (var kvp in GlobalData.GridTilesByCoordinates)
        {
            print("Found tile at X: " + kvp.Key.X + " Y: " + kvp.Key.Y);

            var worldPoint = new Vector3((float)kvp.Key.X, (float)kvp.Key.Y);
            var toCreate = GetPrefabFromTileType(kvp.Value.TileType);

            kvp.Value.GameObject = (GameObject)Instantiate(toCreate, worldPoint, new Quaternion());
        }
    }

    private GameObject GetPrefabFromTileType(TileTypes selectedSprite)
    {
        switch (selectedSprite)
        {
            case TileTypes.Exit:
                return ExitTilePrefab;
            case TileTypes.Floor:
                return WallTilePrefab;
            case TileTypes.Key:
                return KeyPrefab;
            case TileTypes.Door:
                return DoorPrefab;
            case TileTypes.Ground:
                return GroundPrefab;
            case TileTypes.Ground2:
                return Ground2Prefab;
            default:
                throw new ArgumentOutOfRangeException("selectedSprite", selectedSprite, null);
        }
    }

    private Vector3 press;
    private Vector3 delta = new Vector3(0, 0);

    // Update is called once per frame
    void Update()
    {
        // Apparently this will cause problems in the future: http://answers.unity3d.com/questions/784617/how-do-i-block-touch-events-from-propagating-throu.html#answer-885898
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        var mouseScreenPosition = Input.mousePosition;

        MouseInputController.HandleMazeBuilderMouseInput(mouseScreenPosition, PlayerCamera, press, delta, this);
    }

    void FixedUpdate()
    {
        PlayerController.HandePlayerMoveInput(_playerTile);

        var KeysText = GameObject.FindGameObjectWithTag("KeysTextTag").GetComponent<Text>();
        KeysText.text = string.Format("Keys: {0}", GlobalData.KeyCount);
    }

    public void AddTileAtWorldPosition(Vector3 mouseWorldPosition)
    {
        // This leaves a padding around our floor tiles because the scaling factor nonsense is all based around our fancy 
        // PPU calculation, which doesn't work if the resolution isn't exactly 720p. Can we make this more dynamic?
        var newTileCoordinates = ScreenPositionToCoordinates(mouseWorldPosition);

        if (GlobalData.GridTilesByCoordinates.Keys.Contains(newTileCoordinates)) return;

        print("Adding new tile at X: " + newTileCoordinates.X + " Y: " + newTileCoordinates.Y);

        var worldPoint = new Vector3((float)newTileCoordinates.X, (float)newTileCoordinates.Y);
        // TODO: Retire TileTypes, replace this with a PlaceableTileType? That'd be nifty...
        var newTile = (GameObject)Instantiate(GetPrefabFromTileType(GlobalData.SelectedTileType), worldPoint, new Quaternion());
        var cached = new CachedTile
        {
            GameObject = newTile,
            TileType = GlobalData.SelectedTileType
        };

        GlobalData.GridTilesByCoordinates.Add(newTileCoordinates, cached);
    }

    public void RemoveTileAtWorldPosition(Vector3 mouseWorldPosition)
    {
        var coordinates = ScreenPositionToCoordinates(mouseWorldPosition);

        if (!GlobalData.GridTilesByCoordinates.Keys.Contains(coordinates)) return;

        print("Remove tile at X: " + coordinates.X + " Y: " + coordinates.Y);

        var tile = GlobalData.GridTilesByCoordinates[coordinates];

        Destroy(tile.GameObject);

        GlobalData.GridTilesByCoordinates.Remove(coordinates);
    }

    private Point ScreenPositionToCoordinates(Vector3 newTileScreenPosition)
    {
        var worldDistance = ScreenToWorldDistance(TileScreenSize);

        var newTileCoordinatesX = RoundToNearest(newTileScreenPosition.x, 1);
        var newTileCoordinatesY = RoundToNearest(newTileScreenPosition.y, 1);

        return new Point { X = newTileCoordinatesX, Y = newTileCoordinatesY };
    }

    private static double RoundToNearest(float value, double nearest)
    {
        return Math.Round(value / nearest) * (int)nearest;
    }

    private float ScreenToWorldDistance(int pixels)
    {
        return /*PlayerCamera.orthographicSize*/ 5.625f / (Screen.height / 2.0f) * pixels;
    }

    // Used by tile selecting buttons
    public void SelectTile(string tile)
    {
        var tileType = (TileTypes)Enum.Parse(typeof(TileTypes), tile);

        GlobalData.SelectedTileType = tileType;
    }

    [Serializable]
    public class PlaceableTile
    {
        public TileTypes TileType = TileTypes.Floor;
        public Sprite TileSprite;
        public Sprite ButtonSprite;
        public string ButtonText;
    }

    public void ToggleCamera()
    {
        FindObjectOfType<Camera>().GetComponentInChildren<SmoothCamera2D>().target = !Locked ? _playerTile.transform : null;

        Locked = !Locked;
    }

    public void ResetZoom()
    {
        Camera.main.orthographicSize = 5.625f;
    }

    public void SaveLevel()
    {
        GlobalData.GridTilesByCoordinates.OnBeforeSerialize();

        var levelData = JsonUtility.ToJson(GlobalData.GridTilesByCoordinates);
        var level = GlobalData.LoadedLevel;

        var tw = new StreamWriter(Application.persistentDataPath + @"/file.sav");

        // When we want to start implementing a server for saving levels, this link will be handy! GET/POST examples: http://answers.unity3d.com/questions/11021/how-can-i-send-and-receive-data-to-and-from-a-url.html
        Debug.Log(Application.persistentDataPath);
        tw.Write(levelData);

        tw.Close();
    }

    public void LoadLevel()
    {
        var levelPath = Application.persistentDataPath + @"/file.sav";

        print(levelPath);

        if (!File.Exists(levelPath)) return;

        var tr = new StreamReader(levelPath);
        var json = tr.ReadToEnd();

        tr.Close();

        ClearLevel();

        GlobalData.GridTilesByCoordinates = JsonUtility.FromJson<TileDictionary>(json);

        DrawLevel();
    }

    private void ClearLevel()
    {
        if (GlobalData.GridTilesByCoordinates == null) return;

        foreach (var kvp in GlobalData.GridTilesByCoordinates)
        {
            print("Clearing tile at X: " + kvp.Key.X + " Y: " + kvp.Key.Y);

            Destroy(kvp.Value.GameObject);
        }

        GlobalData.GridTilesByCoordinates.Clear();
    }
}

[Serializable]
public struct Point
{
    [SerializeField] public double X;
    [SerializeField] public double Y;
}

[Serializable]
public class CachedTile
{
    [SerializeField]
    public TileTypes TileType;
    public GameObject GameObject;
}