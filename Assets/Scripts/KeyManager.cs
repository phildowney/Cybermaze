using Assets.Scripts;
using System.Linq;
using UnityEngine;

public class KeyManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GlobalData.KeyCount++;

        var foundKey = GlobalData.GridTilesByCoordinates.Where(kvp => kvp.Value.GameObject == gameObject).ToArray();

        if (foundKey.Length == 1)
        {
            GlobalData.GridTilesByCoordinates.Remove(foundKey.Single().Key);
            Destroy(gameObject);
        }
        else if (foundKey.Length == 0)
        {
            print("KeyManager error: key not found");
        }
        else
        {
            print(string.Format("KeyManager error: duplicate keys found ({0})", foundKey.Length));
        }
    }
}
