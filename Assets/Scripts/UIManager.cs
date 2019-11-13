using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    
    [SerializeField]
    Text victoryText;
    

	public void VictoryCondition(string text)
    {
        victoryText.text = text;
       
    }
}
