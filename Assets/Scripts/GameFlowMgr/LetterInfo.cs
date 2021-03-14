using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName ="LettersData",menuName ="CreateEvent/CreateLettersCollection")]
public class LetterInfo : ScriptableObject
{
    public int currentLetterCount;
    [ShowInInspector]
    public List<Letter> letter;
}
public struct Letter
{
    public string letterContent;
    public int letterIndex;
}
