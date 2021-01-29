using Assets.Scripts;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public const float MoveConstant = 7.2f;

    // Use this for initialization
    void Start()
    {
        print("Player alive");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO: Not a static method, etc.
    public static void HandePlayerMoveInput(GameObject _playerTile)
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _playerTile.transform.position = _playerTile.transform.position.Add(-1f * MoveConstant * Time.deltaTime, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _playerTile.transform.position = _playerTile.transform.position.Add(MoveConstant * Time.deltaTime, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            _playerTile.transform.position = _playerTile.transform.position.Add(0f, MoveConstant * Time.deltaTime, 0f);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            _playerTile.transform.position = _playerTile.transform.position.Add(0f, -1f * MoveConstant * Time.deltaTime, 0f);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            _playerTile.transform.position = new Vector3(0f, 0f, 0f);
        }
    }
}
