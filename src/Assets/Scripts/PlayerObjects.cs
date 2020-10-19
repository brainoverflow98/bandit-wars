using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObjects : MonoBehaviour
{
    public string P1Name = "One", P2Name = "Two";
    private int P1Color = 1, P2Color = 5;
    public Material[] materials;
    public GameObject P1Model, P2Model;


    public static GameObject created = null;
    // Start is called before the first frame update
    void Awake()
    {
        if (created != null)
        {
            Destroy(created);            
        }

        DontDestroyOnLoad(this.gameObject);
        created = gameObject;

    }

    public void ChangeP1Color(int color)
    {
        P1Color = color;
        foreach (Transform child in P1Model.transform)
        {
            var skinnedRenderer = child.GetComponent<SkinnedMeshRenderer>();
            if (skinnedRenderer != null)
                skinnedRenderer.material = materials[P1Color];

            var renderer = child.GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
                renderer.material = materials[P1Color];
        }
    }

    public void ChangeP2Color(int color)
    {
        P2Color = color;
        foreach (Transform child in P2Model.transform)
        {
            var skinnedRenderer = child.GetComponent<SkinnedMeshRenderer>();
            if (skinnedRenderer != null)
                skinnedRenderer.material = materials[P2Color];

            var renderer = child.GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
                renderer.material = materials[P2Color];
        }
    }

    public void ChangeUnitMaterials()
    {
        var units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var unit in units)
        {
            Material material = null;
            if (unit.GetComponent<Unit>().Owner == Globals.Player.One)
            {
                material = materials[P1Color];
            }
            else
            {
                material = materials[P2Color];
            }
            foreach (Transform child in unit.transform)
            {
                var skinnedRenderer = child.GetComponent<SkinnedMeshRenderer>();
                if (skinnedRenderer != null)
                    skinnedRenderer.material = material;

                var renderer = child.GetComponentInChildren<MeshRenderer>();
                if (renderer != null)
                    renderer.material = material;
            }
            
        }
    }

}



