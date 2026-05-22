using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item") ]  
public class ItemSO : ScriptableObject
{
    public int ID;
    public string itemName;
    [TextArea]public string description;
    public Sprite Icon;

    [Header("пео╒")]
    public float Weight;

}
