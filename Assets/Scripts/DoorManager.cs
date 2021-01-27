using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts;

public class DoorManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (GlobalData.KeyCount > 0)
        {
            print("unlock door");
            GlobalData.KeyCount--;

            var foundDoor = GlobalData.GridTilesByCoordinates.Where(kvp => kvp.Value.GameObject == gameObject).ToArray();

            if (foundDoor.Length == 1)
            {
                GlobalData.GridTilesByCoordinates.Remove(foundDoor.Single().Key);
                Destroy(gameObject);
            }
            else if (foundDoor.Length == 0)
            {
                print("DoorManager error: door not found");
            }
            else
            {
                print(string.Format("DoorManager error: duplicate door found ({0})", foundDoor.Length));
            }

            Destroy(gameObject);
        }
        else
        {
            print("moar keyz plz");
        }
    }

}
