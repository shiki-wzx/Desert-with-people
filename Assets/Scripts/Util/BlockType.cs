using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum BlockType {
    None,
    ShaQiu,
    ShaDi,
    PingDi,
    CaoDi,
    YouMiao,
    ZhiWu,
    ZhiShaZhan
}

public static class BlockTypeExt {
    public static bool In<T>(this T item, IEnumerable<T> sequence) {
        if(sequence == null)
            throw new ArgumentNullException(nameof(sequence));
        return sequence.Contains(item);
    }

    public static bool In<T>(this T item, params T[] sequence) {
        if(sequence == null)
            throw new ArgumentNullException(nameof(sequence));
        return sequence.Contains(item);
    }

    public static BlockType ToBlockType(this int num, bool clamp = false) {
        if(clamp)
            num = Mathf.Clamp(num, -2, 3);
        return num switch {
            -2 => BlockType.ShaQiu,
            -1 => BlockType.ShaDi,
            0 => BlockType.PingDi,
            1 => BlockType.CaoDi,
            2 => BlockType.YouMiao,
            3 => BlockType.ZhiWu,
            _ => BlockType.None
        };
    }

    public static int ToGreenValue(this BlockType type) {
        return type switch {
            BlockType.ShaQiu => -2,
            BlockType.ShaDi => -1,
            BlockType.PingDi => 0,
            BlockType.CaoDi => 1,
            BlockType.YouMiao => 2,
            BlockType.ZhiWu => 3,
            _ => 0
        };
    }

    public static bool Greenable(this BlockType type) {
        //return type.In(
        //    BlockType.ShaQiu,
        //    BlockType.ShaDi,
        //    BlockType.PingDi,
        //    BlockType.CaoDi,
        //    BlockType.YouMiao,
        //    BlockType.ZhiWu
        //);

        // sufficient for now
        return type != BlockType.ZhiShaZhan;
    }
}
