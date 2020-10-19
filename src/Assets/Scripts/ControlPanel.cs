using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    private GameController gameController;
    public GameObject P1UnitsScrollPanel;
    public GameObject P2UnitsScrollPanel;
    public GameObject SelectedUnitPanel;
    public TextMeshProUGUI Time, TurnText, RoundText;
    private PlayerObjects playerOptions;
    public string P1Name = "First Player", P2Name = "Second Player";

    GameObject[] tiles;

    public Camera P1Camera, P2Camera, BattleCamera, P1BirdseyeCamera, P2BirdseyeCamera, BattleBirdseyeCamera;
    private bool birdseyeView = false;

    private Vector3 oldPos;

    GameObject selectedUnit;

    // Start is called before the first frame update
    void Start()
    {                    
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        playerOptions = GameObject.Find("PlayerOptions")?.GetComponent<PlayerObjects>();

        if (playerOptions != null)
        {
            P1Name = playerOptions.P1Name;
            P2Name = playerOptions.P2Name;
            playerOptions.ChangeUnitMaterials();
        }

        P1UnitsScrollPanel.transform.Find("PanelHeader").GetComponent<TextMeshProUGUI>().text = P1Name + "'s Units";
        P2UnitsScrollPanel.transform.Find("PanelHeader").GetComponent<TextMeshProUGUI>().text = P2Name + "'s Units";

        TurnText.text = P1Name + "'s Turn";

        tiles = GameObject.FindGameObjectsWithTag("Tile");     
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.text = ((int) gameController.time).ToString();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeView();
        }
        
    }

    public void ShowSelectedUnitPanel(GameObject unit)
    {
        selectedUnit = unit;
        P1UnitsScrollPanel.SetActive(false);
        P2UnitsScrollPanel.SetActive(false);
        SelectedUnitPanel.SetActive(true);
        SelectedUnitPanel.GetComponent<SelectedUnitPanel>().RefreshPanel(unit);

        //if (!birdseyeView)
        //{
        //    Camera.main.gameObject.SetActive(false);
        //    unit.GetComponent<Unit>().focusCamera.SetActive(true);
        //}
            
        
    }

    public void ShowScrollPanel()
    {
        selectedUnit = null;
        if (GameController.whoseTurn == Globals.Player.One)
        {
            P2UnitsScrollPanel.SetActive(false);
            P1UnitsScrollPanel.SetActive(true);
            TurnText.text = P1Name+"'s Turn";

            Camera.main.gameObject.SetActive(false);
            if(birdseyeView)
                P1BirdseyeCamera.gameObject.SetActive(true);
            else
                P1Camera.gameObject.SetActive(true);
        }            
        else
        {
            P1UnitsScrollPanel.SetActive(false);
            P2UnitsScrollPanel.SetActive(true);
            TurnText.text = P2Name + "'s Turn";

            Camera.main.gameObject.SetActive(false);
            if (birdseyeView)
                P2BirdseyeCamera.gameObject.SetActive(true);
            else
                P2Camera.gameObject.SetActive(true);
        }

        UnmarkAllTiles();
        RoundText.text = "Round " + gameController.turnNo.ToString();
        SelectedUnitPanel.SetActive(false);
    }

    public void BattleTurnPanel()
    {
        selectedUnit = null;
        P2UnitsScrollPanel.SetActive(false);
        P1UnitsScrollPanel.SetActive(false);
        SelectedUnitPanel.SetActive(false);
        TurnText.text = "Battle Turn";


        Camera.main.gameObject.SetActive(false);
        if (birdseyeView)
            BattleBirdseyeCamera.gameObject.SetActive(true);
        else
            BattleCamera.gameObject.SetActive(true);       

    }

    private void ChangeView()
    {
        birdseyeView = !birdseyeView;

        if (P1Camera.gameObject.activeSelf)
        {
            Camera.main.gameObject.SetActive(false);
            P1BirdseyeCamera.gameObject.SetActive(true);
        }
        else if (P2Camera.gameObject.activeSelf)
        {
            Camera.main.gameObject.SetActive(false);
            P2BirdseyeCamera.gameObject.SetActive(true);
        }
        else if (P1BirdseyeCamera.gameObject.activeSelf)
        {
            Camera.main.gameObject.SetActive(false);
            //if (selectedUnit != null)
            //{
            //    selectedUnit.GetComponent<Unit>().focusCamera.SetActive(true);
            //}
            //else
            {
                P1Camera.gameObject.SetActive(true);
            }
        }
        else if (P2BirdseyeCamera.gameObject.activeSelf)
        {
            Camera.main.gameObject.SetActive(false);
            //if (selectedUnit != null)
            //{
            //    selectedUnit.GetComponent<Unit>().focusCamera.SetActive(true);
            //}
            //else
            {
                P2Camera.gameObject.SetActive(true);
            }
        }
        else if (BattleBirdseyeCamera.gameObject.activeSelf)
        {
            Camera.main.gameObject.SetActive(false);
            BattleCamera.gameObject.SetActive(true);            
        }
        else if (BattleCamera.gameObject.activeSelf)
        {
            Camera.main.gameObject.SetActive(false);
            BattleBirdseyeCamera.gameObject.SetActive(true);
        }
        //else
        //{
        //    Camera.main.gameObject.SetActive(false);
        //    if (selectedUnit.GetComponent<Unit>().Owner == Globals.Player.One)
        //    {
        //        P1BirdseyeCamera.gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        P2BirdseyeCamera.gameObject.SetActive(true);
        //    }
        //}
    }

    public void UnmarkAllTiles()
    {
        foreach (var tile in tiles)
        {
            tile.GetComponent<Tile>().Unmark();
        }
    }

}
