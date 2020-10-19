using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedUnitPanel : MonoBehaviour
{
    
    private TextMeshProUGUI infoText, unitNameText, actionText;
    private Unit unit;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;

    // Start is called before the first frame update
    void Awake()
    {
        unitNameText = GameObject.Find("UnitNameText").GetComponent<TextMeshProUGUI>();
        infoText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
        actionText = GameObject.Find("ActionText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshPanel(GameObject selectedUnit)
    {
        unit = selectedUnit.GetComponent<Unit>();
        unitNameText.text = unit.gameObject.name;

        healthSlider.value = unit.CurrentHealth / unit.Health;
        healthText.text = unit.CurrentHealth + "/" + unit.Health;

        SetInfoText();
    }

    public void SetInfoText()
    {
        actionText.text = unit.Action.ToString();
        if (unit.Action == Globals.ActionType.Move || unit.Action == Globals.ActionType.None)
        {  
            infoText.text = "Chose a tile to move in the next turn. You can move " + unit.MoveRange + " tile. Action Phase: "+unit.MovePhase+"";            
        }
        else if (unit.Action == Globals.ActionType.NormalAttack)
        {
            infoText.text = "Chose a tile to attack in the next turn. Range: "+unit.NormalAttcakRange+" Damage: "+unit.NormalAttcakDamage+ "Action Phase: "+ unit.NormalAttcakPhase + " Can Break Block: "+ unit.canBreakBlock;
        }
        else if (unit.Action == Globals.ActionType.QuickAttack)
        {
            infoText.text = "Chose a tile to attack in the next turn. Range: " + unit.QuickAttackRange + " Damage: " + unit.QuickAttackDamage + "Action Phase: " + unit.QuickAttackPhase;
        }
    }

    public void SetMove()
    {
        unit.TargetTile = null;
        unit.Action = Globals.ActionType.Move;
        unit.MarkAction();
        SetInfoText();
    }

    public void SetNormalAttack()
    {
        unit.TargetTile = null;
        unit.Action = Globals.ActionType.NormalAttack;
        unit.MarkAction();
        SetInfoText();
    }

    public void SetQuickAttack()
    {
        unit.TargetTile = null;
        unit.Action = Globals.ActionType.QuickAttack;
        unit.MarkAction();
        SetInfoText();
    }

    
}
