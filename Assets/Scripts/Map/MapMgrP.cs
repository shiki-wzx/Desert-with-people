using System.Linq;
using UnityEngine;


public partial class MapMgr {
    /// <summary> Helper function for event </summary>
    public bool RandomDegrade(int num, BlockType dstType, params BlockType[] srcTypes) {
        var candidates = childBlks.Where(blk => blk.BlkParam.Type.In(srcTypes)).ToArray();
        if(candidates.Length < num)
            return false;

        var numReq = num;
        for(var numAva = candidates.Length; numAva > 0 && numReq > 0; --numAva) {
            var prob = (float)numReq / numAva;
            if(Random.value > prob)
                continue;
            --numReq;
            candidates[numAva - 1].SetBlkInfo(dstType);
        }
        return true;
    }


    /// <summary> Helper function for achievements </summary>
    /// todo: optimization
    public MapStatInfo GetStatus() {
        var msInfo = new MapStatInfo {
            AnyBlkWhoseAdjDesertLt4 = false,
            AnyBlkWhoseAdjDesertEq0 = false,
            AnyGreenBlkWhoseAdjAllGreen = false
        };

        foreach(var blk in childBlks) {
            if(msInfo.AnyBlkWhoseAdjDesertLt4 &&
               msInfo.AnyBlkWhoseAdjDesertEq0 &&
               msInfo.AnyGreenBlkWhoseAdjAllGreen)
                break;
            if(blk.BlkParam.Type.In(BlockType.ShaQiu, BlockType.ShaDi))
                continue;

            var desertCnt = 0;
            var notGreenCnt = 0;
            foreach(var adj in blk.GetAdjacentBlk()) {
                if(adj.BlkParam.Type.In(BlockType.ShaQiu, BlockType.ShaDi)) {
                    ++desertCnt;
                    ++notGreenCnt;
                }
                else if(adj.BlkParam.Type == BlockType.PingDi)
                    ++notGreenCnt;
            }

            // TODO: fix 积少成多
            if(desertCnt == 0)
                msInfo.AnyBlkWhoseAdjDesertLt4 = msInfo.AnyBlkWhoseAdjDesertEq0 = true;
            else if(desertCnt < 4)
                msInfo.AnyBlkWhoseAdjDesertLt4 = true;
            if(blk.BlkParam.Type != BlockType.PingDi && notGreenCnt == 0)
                msInfo.AnyGreenBlkWhoseAdjAllGreen = true;
        }

        return msInfo;
    }
}


public struct MapStatInfo {
    /// <summary> 积少成多 I </summary>
    public bool AnyBlkWhoseAdjDesertLt4;

    /// <summary> 积少成多 II </summary>
    public bool AnyBlkWhoseAdjDesertEq0;

    /// <summary> 积少成多 III </summary>
    public bool AnyGreenBlkWhoseAdjAllGreen;
}


public struct MapUpdInfo {
    /// <summary> 牧绿者 I </summary>
    public bool SeedingTriggered;

    /// <summary> 慎重治沙者 I </summary>
    public bool DegradationTriggered;
}
