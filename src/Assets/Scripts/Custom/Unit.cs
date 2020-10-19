using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    public Globals.UnitType UnitType;
    public Globals.Player Owner;
    public Globals.ActionType Action;

	float speed = 8;
    [HideInInspector]
    public GameObject CurrentTile, TargetTile = null;

    private GameController gameController;
    [HideInInspector]
    public Animator anim;

    public float Health, CurrentHealth;
    public float MovePhase, NormalAttcakPhase, QuickAttackPhase;
    public int MoveRange, NormalAttcakRange, QuickAttackRange;
    public int NormalAttcakDamage, QuickAttackDamage;
    public bool canBlock, canBreakBlock;
    private bool blockedThisRound, attackedThisRound;

    public UnitLabel GUILabel;

    [HideInInspector]
    public GameObject focusCamera;
    private GameObject[] tiles;

    private bool died = false;

    public GameObject hitEffect, dieEffect;

    public TextEffect effectText;

    // Use this for initialization
    void Start () {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        anim = GetComponent<Animator>();
        focusCamera = transform.Find("UnitFocusCamera").gameObject;
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        CurrentHealth = Health;

        effectText = transform.GetComponentInChildren<TextEffect>();
    }
	
	// Update is called once per frame
	void Update () {

        if (CurrentHealth > 0)
        {
            if (TargetTile != null)
            {
                Vector3 dir = TargetTile.transform.position - transform.position;
                Vector3 velocity = dir.normalized * speed * Time.deltaTime;

                // Make sure the velocity doesn't actually exceed the distance we want.
                velocity = Vector3.ClampMagnitude(velocity, dir.magnitude);

                Vector3 newDir = Vector3.RotateTowards(transform.forward, velocity, speed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
            else
            {
                float face = Owner == Globals.Player.One ? 90 : -90;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, face, 0), speed * Time.deltaTime * 40);
            }


            var bonus = 0.0f;
            if (Owner == gameController.offensive)
                bonus = 0.5f;

            if (Action == Globals.ActionType.Move && MovePhase - bonus <= gameController.phase && TargetTile != null)
            {
                var unit = TargetTile.GetComponent<Tile>().Unit;
                if (unit == null || unit == gameObject)
                {
                    Vector3 dir = TargetTile.transform.position - transform.position;
                    Vector3 velocity = dir.normalized * speed * Time.deltaTime;

                    // Make sure the velocity doesn't actually exceed the distance we want.
                    velocity = Vector3.ClampMagnitude(velocity, dir.magnitude);

                    if (velocity != new Vector3(0, 0, 0))
                    {
                        transform.Translate(velocity, Space.World);
                        anim.Play("Run");
                    }
                    else
                    {
                        anim.Play("Idle");
                    }
                }

            }
            else if (Action == Globals.ActionType.NormalAttack && NormalAttcakPhase - bonus <= gameController.phase && TargetTile != null)
            {
                var targetUnit = TargetTile.GetComponent<Tile>().Unit;
                if (targetUnit != null && !attackedThisRound)
                {
                    var unit = targetUnit.GetComponent<Unit>();
                    if (unit.Owner != Owner)
                    {
                        anim.Play("Attack");
                        attackedThisRound = true;

                        unit.anim.Play("Take Damage");
                        Instantiate(hitEffect, unit.transform);
                        if (unit.canBlock && !unit.blockedThisRound && !canBreakBlock)
                        {
                            unit.blockedThisRound = true;
                            unit.effectText.SetText("BLOCKED");
                        }
                        else
                        {
                            unit.blockedThisRound = true;
                            unit.CurrentHealth -= NormalAttcakDamage;
                            unit.effectText.SetText("-" + NormalAttcakDamage);
                        }
                    }
                }
            }
            else if (Action == Globals.ActionType.QuickAttack && QuickAttackPhase - bonus <= gameController.phase && TargetTile != null)
            {
                var targetUnit = TargetTile.GetComponent<Tile>().Unit;
                if (targetUnit != null && !attackedThisRound)
                {
                    var unit = targetUnit.GetComponent<Unit>();
                    if (unit.Owner != Owner)
                    {
                        anim.Play("Attack");
                        attackedThisRound = true;

                        unit.anim.Play("Take Damage");
                        Instantiate(hitEffect, unit.transform);
                        if (unit.canBlock && !unit.blockedThisRound)
                        {
                            unit.blockedThisRound = true;
                            unit.effectText.SetText("BLOCKED");
                        }
                        else
                        {
                            unit.blockedThisRound = true;
                            unit.CurrentHealth -= QuickAttackDamage;
                            unit.effectText.SetText("-"+QuickAttackDamage);
                        }
                    }
                }
            }
        }
        else
        {            
            if (!died)
            {
                StartCoroutine("DieCoroutine");
                died = true;
            }            
        }

        



    }  

    void OnTriggerEnter(Collider collider)
    {        
        var hitObject = collider.gameObject;
        Debug.Log("Over: " + hitObject.name);

        if (hitObject.tag == "Tile")
        {
            if (CurrentTile != null)            
            {
                CurrentTile.GetComponent<Tile>().Unit = null;
            }

            CurrentTile = hitObject;
            CurrentTile.GetComponent<Tile>().Unit = gameObject;

        }
        
    }

    IEnumerator DieCoroutine()
    {
        anim.Play("Die");        
        CurrentTile.GetComponent<Tile>().Unit = null;
        GUILabel.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        Instantiate(dieEffect, CurrentTile.transform);
        if (UnitType == Globals.UnitType.King)
        {
            gameController.GameEnd(Owner);
        }
        gameObject.SetActive(false);
    }

    private void MarkRange()
    {
        Color color;
        int range;
        var tile = CurrentTile.GetComponent<Tile>();
        if (Action == Globals.ActionType.Move || Action == Globals.ActionType.None)
        {
            color = new Color(77.0f / 255.0f, 94.0f / 255.0f, 205.0f / 255.0f);
            range = MoveRange;
            tile.MarkNeighbours(range, color);
        }
        else if (Action == Globals.ActionType.NormalAttack)
        {
            color = new Color(188.0f / 255.0f, 47.0f / 255.0f, 54.0f / 255.0f);
            range = NormalAttcakRange;
            tile.MarkNeighbours(range, color);
        }
        else if (Action == Globals.ActionType.QuickAttack)
        {
            color = new Color(239.0f / 255.0f, 108.0f / 255.0f, 241.0f / 255.0f);
            range = QuickAttackRange;
            tile.MarkNeighbours(range, color);
        }
    }

    private void MarkTarget()
    {
        Color color;
        
        if (TargetTile != null)
        {
            var tile = TargetTile.GetComponent<Tile>();
            if (Action == Globals.ActionType.Move)
            {
                color = new Color(240.0f / 255.0f, 240.0f / 255.0f, 70.0f / 255.0f);
                tile.Mark(color);
            }
            else if (Action == Globals.ActionType.NormalAttack)
            {
                color = new Color(240.0f / 255.0f, 240.0f / 255.0f, 70.0f / 255.0f);
                tile.Mark(color);
            }
            else if (Action == Globals.ActionType.QuickAttack)
            {
                color = new Color(240.0f / 255.0f, 240.0f / 255.0f, 70.0f / 255.0f);
                tile.Mark(color);
            }
        }        
    }

    public void MarkAction()
    {
        foreach (var tile in tiles)
        {
            tile.GetComponent<Tile>().Unmark();
        }
        MarkRange();
        MarkTarget();
    }

    public void ResetTurnVariables()
    {
        Action = Globals.ActionType.None;
        TargetTile = null;
        attackedThisRound = false;
        blockedThisRound = false;
    }
}
