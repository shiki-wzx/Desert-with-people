using System;
using UnityEngine;


// dirty code, really dirty
[CreateAssetMenu(fileName = "BlockTypeDict", menuName = "Map/BlockType Dict")]
public class BlockTypeDict : ScriptableObject
{
    [SerializeField] private BlockParam shaQiu, shaDi, pingDi, caoDi, youMiao, zhiWu, zhiShaZhan;

    public BlockParam this[BlockType type]
    {
        get
        {
            switch (type)
            {
                case BlockType.ShaQiu:
                    return shaQiu;
                case BlockType.ShaDi:
                    return shaDi;
                case BlockType.PingDi:
                    return pingDi;
                case BlockType.CaoDi:
                    return caoDi;
                case BlockType.YouMiao:
                    return youMiao;
                case BlockType.ZhiWu:
                    return zhiWu;
                case BlockType.ZhiShaZhan:
                    return zhiShaZhan;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}