using System;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Linq;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

// HAY READ THIS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// You need to have a specific aspect ratio with pixel dimensions set for this to work AT ALL.

public class MazeBuilder : MonoBehaviour
{
    public int count = 0;
    public bool Locked = true;
    private int CombatLikelihood = 1000; // This is named poorly.

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
	void Start ()
    {
        Debug.Log("Hello World");
        var allPrefabs = Resources.LoadAll<GameObject>("Prefabs");

	    //if (!Platformer)
	    //{
	    //    PlayerPrefab = allPrefabs.FirstOrDefault(p => p.name == "TopdownPlayer");
     //       PlaceableTiles = GlobalData.TopDownTileset;
            
     //   }
	    //else
	    //{
     //       PlayerPrefab = allPrefabs.FirstOrDefault(p => p.name == "PlatformerPlayer");
	    //    PlaceableTiles = GlobalData.PlatformerTileset;
	    //}

	    GlobalData.SelectedTileType = PlaceableTiles.First().TileType;

        print(allPrefabs.Length);
        //var allSprites = Resources.LoadAll<Sprite>("Sprites");
        //print(allSprites.Length);
        // Set up the UI
        //PlaceableTiles = GlobalData.TopDownTileset;

        for (int i = 0; i < PlaceableTiles.Length; i++)
	    {
	        var tile = PlaceableTiles[i];
	        var tileButton = Instantiate(TileButtonPrefab, new Vector3(0f, 0f, 0f), new Quaternion(), Canvas.transform);
            
	        var size = 75f;
	        var spacing = 15f;
	        var offset = spacing + size / 2f;

            //tileButton.transform.localScale = new Vector3(size, size, 1f);

            ((RectTransform) tileButton.transform).anchoredPosition = new Vector3(offset + (size + spacing) * i, offset);
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
	    
	    //GlobalData.SelectedTileType = TileTypes.Floor;
        //AddTileAtWorldPosition(new Vector3(0f, 0f, 0f));

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

            var worldPoint = new Vector3((float) kvp.Key.X, (float) kvp.Key.Y);
            var toCreate = GetPrefabFromTileType(kvp.Value.TileType);

            kvp.Value.GameObject = (GameObject) Instantiate(toCreate, worldPoint, new Quaternion());
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
    private Vector3 delta = new Vector3(0,0);

    // Update is called once per frame
    void Update()
    {
        // Apparently this will cause problems in the future: http://answers.unity3d.com/questions/784617/how-do-i-block-touch-events-from-propagating-throu.html#answer-885898
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        var mouseScreenPosition = Input.mousePosition;

        #region Tile Creation

        if (Input.GetMouseButtonDown(2))
        {
            var mouseWorldPosition = PlayerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, PlayerCamera.farClipPlane));
            press = mouseWorldPosition + delta;
        }

        if (Input.GetMouseButton(2))
        {
            var mouseWorldPosition = PlayerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, PlayerCamera.farClipPlane));

            delta = press - mouseWorldPosition;

            print("Delta" + delta);
        }

        PlayerCamera.transform.position = PlayerCamera.transform.position.Add(delta);

        if (Input.GetMouseButton(0))
        {
            var mouseWorldPosition = PlayerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, PlayerCamera.farClipPlane));

            AddTileAtWorldPosition(mouseWorldPosition);
        }

        if (Input.GetMouseButton(1))
        {
            var mouseWorldPosition = PlayerCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, PlayerCamera.farClipPlane));
            RemoveTileAtWorldPosition(mouseWorldPosition);
        }

        #endregion

        #region Zooming

        var scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize -= scroll*5;
        }

        #endregion

        if(Input.GetKeyUp(KeyCode.T))
        {
            _playerTile.transform.position = new Vector3(0f, 0f, 0f);
        }
    }

    void FixedUpdate()
    {
        // TODO: move this to the PlayerController?
        // TODO: rename the PlayerController to TopDownPlayerController
        if (!Platformer) { 
        #region Player Movement

        var moveConstant = 0.12f;
        var moved = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _playerTile.transform.position = _playerTile.transform.position.Add(-1f*moveConstant, 0f, 0f);
            moved = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _playerTile.transform.position = _playerTile.transform.position.Add(moveConstant, 0f, 0f);
            moved = true;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _playerTile.transform.position = _playerTile.transform.position.Add(0f, moveConstant, 0f);
            moved = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _playerTile.transform.position = _playerTile.transform.position.Add(0f, -1f*moveConstant, 0f);
            moved = true;
        }

        if (moved)
        {
            if (Random.Range(0, CombatLikelihood) == 0)
            {
                //LoadCombat();
            }
        }
        }

        #endregion

        var KeysText = GameObject.FindGameObjectWithTag("KeysTextTag").GetComponent<Text>();
        KeysText.text = string.Format("Keys: {0}", GlobalData.KeyCount);
    }

    private void AddTileAtWorldPosition(Vector3 mouseWorldPosition)
    {
        // This leaves a padding around our floor tiles because the scaling factor nonsense is all based around our fancy 
        // PPU calculation, which doesn't work if the resolution isn't exactly 720p. Can we make this more dynamic?
        var newTileCoordinates = ScreenPositionToCoordinates(mouseWorldPosition);

        if (GlobalData.GridTilesByCoordinates.Keys.Contains(newTileCoordinates)) return;

        print("Adding new tile at X: " + newTileCoordinates.X + " Y: " + newTileCoordinates.Y);

        var worldPoint = new Vector3((float) newTileCoordinates.X, (float) newTileCoordinates.Y);
        // TODO: Retire TileTypes, replace this with a PlaceableTileType? That'd be nifty...
        var newTile = (GameObject)Instantiate(GetPrefabFromTileType(GlobalData.SelectedTileType), worldPoint, new Quaternion());
        var cached = new CachedTile
        {
            GameObject = newTile,
            TileType = GlobalData.SelectedTileType
        };

        GlobalData.GridTilesByCoordinates.Add(newTileCoordinates, cached);
    }

    private void RemoveTileAtWorldPosition(Vector3 mouseWorldPosition)
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

        var newTileCoordinatesX = RoundToNearest(newTileScreenPosition.x, worldDistance);
        var newTileCoordinatesY = RoundToNearest(newTileScreenPosition.y, worldDistance);

        return new Point {X = newTileCoordinatesX, Y = newTileCoordinatesY};
    }

    private static double RoundToNearest(float value, double nearest)
    {
        return Math.Round(value/nearest)*(int) nearest;
    }

    private float ScreenToWorldDistance(int pixels)
    {
        return /*PlayerCamera.orthographicSize*/ 5.625f/(Screen.height/2.0f)*pixels;
    }

    // Used by tile selecting buttons
    public void SelectTile(string tile)
    {
        var tileType = (TileTypes) Enum.Parse(typeof (TileTypes), tile);

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

    public void LoadCombat()
    {
        GlobalData.GridTilesByCoordinates = GlobalData.GridTilesByCoordinates;
        GlobalData.PlayerLocation = _playerTile.transform.position;
        SceneManager.LoadScene("combat");
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


        // First attempt 

        //var url = "http://silentmindgames.com/leveleditor/saveLevel.php";

        //var form = new WWWForm();
        //form.AddField("json", levelData);
        //form.AddField("secureToken", "SuperSecurePassword");
        //var www = new WWW(url, form);

        //StartCoroutine(WaitForRequest(www));

        // Newest

        //var url = "http://silentmindgames.com/leveleditor-dev/api.php";

        //var form = new WWWForm();

        //Debug.Log("Save1");
        //form.AddField("action", "saveLevel");
        //Debug.Log("Save2");
        //form.AddField("token", GlobalData.SecureToken);

        //form.AddField("levelId", level.Id);
        //form.AddField("levelName", level.Name);

        //form.AddField("levelDescription", level.Description);
        //form.AddField("levelData", levelData);
        //form.AddField("userId", 1);

        //Debug.Log("Save3");

        //var www = new WWW(url, form);

        //StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

         // check for errors
        if (www.error == null)
        {
            //Debug.Log("WWW Ok!: " + www.data);
        }
        else {
            Debug.Log("WWW Error: " + www.error);
        }
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

        //StartCoroutine(DoWWW());
    }

    private IEnumerator DoWWW()
    {
        var json = new WWW("http://silentmindgames.com/leveleditor/file.sav");
        yield return json;
        print(json.text);

        ClearLevel();

        GlobalData.GridTilesByCoordinates = JsonUtility.FromJson<TileDictionary>(json.text);

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