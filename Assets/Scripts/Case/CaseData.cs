using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Case Data", menuName = "Case/New Case Data")]
public class CaseData : ScriptableObject
{
    public string titleCase;
    public int idCase;
    public string descCase;

    public Sprite imageCase;
}
