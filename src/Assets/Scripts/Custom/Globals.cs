using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public enum UnitType { King, Warlord, Cavalry, Spearman, Swordsman, Archer };
    public enum Player { One, Two };
    public enum ActionType { None, Move, NormalAttack, QuickAttack };
    public static int[,] ActionPhases = { {0,4,6,5} , {0,4,6,5} , {0,4,5,1} , {0,3,5,1} , {0,3,5,1} , {0,3,7,2} };
}
