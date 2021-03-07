using UnityEngine;


[CreateAssetMenu(fileName = "BlockParam", menuName = "Map/Block Param")]
public class BlockParam : ScriptableObject {
    [SerializeField] private BlockType type;
    [SerializeField] private int greenValue;
    [SerializeField] private GameObject blockPrefab;

    public BlockType Type => type;

    /// <remarks> CAUTION: ZhiShaZhan has GreenValue of 0. </remarks>
    public int GreenValue => greenValue;

    public GameObject BlockPrefab => blockPrefab;
}
