using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BlockParam", menuName = "Map/Block Param")]
public class BlockParam : ScriptableObject
{
    [SerializeField] private BlockType type;
    [SerializeField] private int greenValue;
    [SerializeField] private GameObject blockPrefab;

    public BlockType Type => type;
    public int GreenValue => greenValue;
    public GameObject BlockPrefab => blockPrefab;
}