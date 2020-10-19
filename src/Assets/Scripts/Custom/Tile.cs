using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{

    // Our coordinates in the map array
    public int x;
    public int y;
    public GameObject Unit;
    public List<Tile> neighbourList;
    public bool marked;
    public int markDistance = 0;
    

    void Start()
    {
        FindNeighbours();

    }

    #region METHODS

    public void MarkNeighbours(int distance, Color color)
    {
        marked = true;
        markDistance = distance;
        Mark(color);

        if (distance != 0)
        {
            foreach (var neighbour in neighbourList)
            {
                neighbour.Mark(color);
                if (neighbour.markDistance < distance)
                {
                    neighbour.marked = true;
                    neighbour.markDistance = distance;
                    neighbour.MarkNeighbours(distance - 1, color);
                }                
            }
        }         
    }    

    public void Mark(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }    

    public void Unmark()
    {
        GetComponent<Renderer>().material.color = new Color(125.0f / 255.0f, 170.0f / 255.0f, 70.0f / 255.0f, 255.0f / 255.0f);
        marked = false;
        markDistance = 0;
    }


    public void FindNeighbours()
    {       
        Tile neigbour = null;

        //front neighbour
        neigbour = GameObject.Find("Tile_" + x + "_" + (y + 1))?.GetComponent<Tile>() ?? null;
        if(neigbour != null)
        {
            neighbourList.Add(neigbour);            
        }

        //rear neighbour
        neigbour = GameObject.Find("Tile_" + x + "_" + (y - 1))?.GetComponent<Tile>() ?? null;
        if (neigbour != null)
        {
            neighbourList.Add(neigbour);            
        }

        //right neighbour 1
        neigbour = GameObject.Find("Tile_" + (x + 1) + "_" + y)?.GetComponent<Tile>() ?? null;
        if (neigbour != null)
        {
            neighbourList.Add(neigbour);
        }

        //right neighbour 2
        neigbour = GameObject.Find("Tile_" + (x + 1) + "_" + (x % 2 == 0 ? y + 1 : y - 1))?.GetComponent<Tile>() ?? null;
        if (neigbour != null)
        {
            neighbourList.Add(neigbour);
        }

        //left neighbour 1
        neigbour = GameObject.Find("Tile_" + (x - 1) + "_" + y)?.GetComponent<Tile>() ?? null;
        if (neigbour != null)
        {
            neighbourList.Add(neigbour);
        }

        //left neighbour 2
        neigbour = GameObject.Find("Tile_" + (x - 1) + "_" + (x % 2 == 0 ? y + 1 : y - 1))?.GetComponent<Tile>() ?? null;
        if (neigbour != null)
        {
            neighbourList.Add(neigbour);
        }
    }

    #endregion
}
