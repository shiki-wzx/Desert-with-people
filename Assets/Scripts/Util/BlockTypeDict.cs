using System;
using UnityEngine;


// dirty code, really dirty
[CreateAssetMenu(fileName = "BlockTypeDict", menuName = "Map/BlockType Dict")]
public class BlockTypeDict : ScriptableObject {
    [SerializeField] private BlockParam shaQiu, shaDi, pingDi, caoDi, youMiao, zhiWu, zhiShaZhan;

    public BlockParam this[BlockType type] {
        get {
            return type switch {
                BlockType.ShaQiu => shaQiu,
                BlockType.ShaDi => shaDi,
                BlockType.PingDi => pingDi,
                BlockType.CaoDi => caoDi,
                BlockType.YouMiao => youMiao,
                BlockType.ZhiWu => zhiWu,
                BlockType.ZhiShaZhan => zhiShaZhan,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
