using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    public Text text;
    public void UpdateText(string newText)
    {
        text.text = newText;
    }
}
