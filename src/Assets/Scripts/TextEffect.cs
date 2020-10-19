using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (text.alpha > 0.0f)
        {
            text.alpha -= 0.005f;
        }
    }

    public void SetText(string newText)
    {
        text.text = newText;
        text.alpha = 1.0f;
    }

}
