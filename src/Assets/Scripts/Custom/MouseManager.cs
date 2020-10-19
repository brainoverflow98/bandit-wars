using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {

	Unit selectedUnit;
    GameObject[] tiles;
    GameObject[] units;

    public float zoomSensitivity = 10.0f;

    // Use this for initialization
    void Start () {

       tiles = GameObject.FindGameObjectsWithTag("Tile");
        units = GameObject.FindGameObjectsWithTag("Unit");
	
	}    
	
	// Update is called once per frame
	void Update () {

        if (!EventSystem.current.IsPointerOverGameObject())        
            Camera.main.transform.position += Camera.main.transform.forward.normalized * Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;


        if (Input.GetMouseButtonDown(0))
        {
            //if (EventSystem.current.IsPointerOverGameObject())
            //    return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            int layerMask = 1 << 9;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
            {
                GameObject ourHitObject = hitInfo.collider.transform.gameObject;

                //Debug.Log("Clicked On: " + ourHitObject.name);

                // So...what kind of object are we over?
                if (ourHitObject.tag == "Tile")
                {
                    // Ah! We are over a hex!                
                    SelectTile(ourHitObject);
                }
                else
                {
                    UnmarkTiles();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            //if (EventSystem.current.IsPointerOverGameObject())
            //    return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            int layerMask = 1 << 10;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
            {
                GameObject ourHitObject = hitInfo.collider.transform.gameObject;

                //Debug.Log("Clicked On: " + ourHitObject.name);

                // So...what kind of object are we over?
                if (ourHitObject.tag == "Unit")
                {
                    // We are over a unit!                    
                    SelectUnit(ourHitObject);

                }
                else
                {
                    UnmarkTiles();
                }
            }
        }




    }

	public void SelectTile(GameObject ourHitObject) {

        if (ourHitObject.GetComponent<Tile>().marked)
        {
            // If we have a unit selected set target tile
            if (selectedUnit != null)
            {
                if(selectedUnit.Action == Globals.ActionType.None || selectedUnit.Action == Globals.ActionType.Move)
                {
                    foreach (var unit in units)
                    {
                        if (ourHitObject == unit.GetComponent<Unit>().TargetTile && unit.GetComponent<Unit>().Action == Globals.ActionType.Move)
                            return;
                    }
                }
                
                selectedUnit.TargetTile = ourHitObject;
                if (selectedUnit.Action == Globals.ActionType.None)
                    selectedUnit.Action = Globals.ActionType.Move;
                selectedUnit.MarkAction();
            }
        }
    }
    

    public void SelectUnit(GameObject ourHitObject)
    {       
        // We have click on the unit
        var unit = ourHitObject.GetComponent<Unit>();
        if(unit.Owner == GameController.whoseTurn)
        {
            selectedUnit = unit;
            selectedUnit.MarkAction();

            selectedUnit.GUILabel.SelectUnitWorld();
        }
        
    }

    public void UnselectUnits()
    {
        // We have click on the unit
        selectedUnit = null;
    }

    public void UnmarkTiles()
    {
        foreach (var tile in tiles)
        {
            tile.GetComponent<Tile>().Unmark();
        }
    }
}
