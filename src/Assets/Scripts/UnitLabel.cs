using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitLabel : MonoBehaviour
{
    public GameObject unitObj;
    public Unit unit;
    private GameController gameController;
    private MouseManager mouseManager;
    private ControlPanel controlPanel;

    public Slider healthSlider;
    public TextMeshProUGUI healthText, nameText;

    public GameObject canBlock, breakBlock, normalAttack, quickAttack, move;
    

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Text").GetComponent<TextMeshProUGUI>().text = unitObj.name;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        mouseManager = GameObject.Find("MouseManager").GetComponent<MouseManager>();

        controlPanel = GameObject.Find("ControlPanel").GetComponent<ControlPanel>();

        unit = unitObj.GetComponent<Unit>();
        unit.GUILabel = this;
    }

    // Update is called once per frame
    void Update()
    {
        nameText.text = unit.name;
        healthSlider.value = unit.CurrentHealth / unit.Health;
        healthText.text = unit.CurrentHealth+"/"+unit.Health;

        if (unit.canBlock)
        {
            canBlock.SetActive(true);
        }
        else
        {
            canBlock.SetActive(false);
        }

        if (unit.canBreakBlock)
        {
            breakBlock.SetActive(true);
        }
        else
        {
            breakBlock.SetActive(false);
        }

        if (unit.Action == Globals.ActionType.Move)
        {
            move.SetActive(true);
        }
        else
        {
            move.SetActive(false);
        }

        if (unit.Action == Globals.ActionType.NormalAttack)
        {
            normalAttack.SetActive(true);
        }
        else
        {
            normalAttack.SetActive(false);
        }

        if (unit.Action == Globals.ActionType.QuickAttack)
        {
            quickAttack.SetActive(true);
        }
        else
        {
            quickAttack.SetActive(false);
        }
    }

    public void SelectUnitGUI()
    {
        mouseManager.SelectUnit(unitObj);
    }

    public void SelectUnitWorld()
    {

        controlPanel.ShowSelectedUnitPanel(unitObj);
    }

    
}
