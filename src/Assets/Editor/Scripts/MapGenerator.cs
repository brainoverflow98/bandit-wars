using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public GameObject TilePrefab;

	// Size of the map in terms of number of hex tiles
	// This is NOT representative of the amount of 
	// world space that we're going to take up.
	// (i.e. our tiles might be more or less than 1 Unity World Unit)
	public int width = 11;
	public int height = 13;

    public float xBegin = 12f;
    public float yBegin = 13.91f;
    public float zBegin = 79.1f;

    public float angle = 30f;

    public float xOffset = 4.4f;
	public float zOffset = 3.85f;

	// Use this for initialization
	void Start () {

        float yPos = yBegin;

		for (int x = 0; x < width; x++) {            
			for (int y = 0; y < height; y++) { 

				float xPos = x * xOffset + xBegin;

				// Are we on an odd row?
				if( y % 2 == 1 ) {
                    if (x == width - 1)
                        continue;
					xPos += xOffset/2f;
				}
                float zPos = y * zOffset + zBegin;

                GameObject hex_go = (GameObject)PrefabUtility.InstantiatePrefab(TilePrefab);

                hex_go.transform.position = new Vector3(xPos, yPos, zPos);
                hex_go.transform.rotation = Quaternion.Euler(0, angle, 0);                

				// Name the gameobject something sensible.
				hex_go.name = "Tile_" + (y+1) + "_" + (x+1);

				// Make sure the hex is aware of its place on the map
				hex_go.GetComponent<Tile>().x = y + 1;
				hex_go.GetComponent<Tile>().y = x + 1;

				// For a cleaner hierachy, parent this hex to the map
				hex_go.transform.SetParent(this.transform);

				// TODO: Quill needs to explain different optimization later...
				hex_go.isStatic = true;

			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
