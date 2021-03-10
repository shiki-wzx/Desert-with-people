using System.Linq;
using UnityEngine;


public partial class MapMgr {
    /// <summary> Helper function for event </summary>
    /// <returns> whether degradation happens </returns>
    public bool RandomDegrade(int num, BlockType dstType, params BlockType[] srcTypes) {
        var candidates = childBlks.Where(blk => blk.BlkParam.Type.In(srcTypes)).ToArray();
        if(candidates.Length == 0 || num < 1)
            return false;

        var numReq = Mathf.Min(num, candidates.Length);
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
            AnyNotDesertBlkWhoseAdjDesertLt4 = false,
            AnyNotDesertBlkWhoseAdjDesertEq0 = false,
            AnyGreenBlkWhoseAdjAllGreen = false
        };

        foreach(var blk in childBlks) {
            if(msInfo.AnyNotDesertBlkWhoseAdjDesertLt4 &&
               msInfo.AnyNotDesertBlkWhoseAdjDesertEq0 &&
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

            if(desertCnt == 0)
                msInfo.AnyNotDesertBlkWhoseAdjDesertLt4 = msInfo.AnyNotDesertBlkWhoseAdjDesertEq0 = true;
            else if(desertCnt < 4)
                msInfo.AnyNotDesertBlkWhoseAdjDesertLt4 = true;
            if(blk.BlkParam.Type != BlockType.PingDi && notGreenCnt == 0)
                msInfo.AnyGreenBlkWhoseAdjAllGreen = true;
        }

        return msInfo;
    }
}


public struct MapStatInfo {
    /// <summary> 积少成多 I </summary>
    public bool AnyNotDesertBlkWhoseAdjDesertLt4;

    /// <summary> 积少成多 II </summary>
    public bool AnyNotDesertBlkWhoseAdjDesertEq0;

    /// <summary> 积少成多 III </summary>
    public bool AnyGreenBlkWhoseAdjAllGreen;
}


public struct MapUpdInfo {
    /// <summary> 牧绿者 I </summary>
    public bool SeedingTriggered;

    /// <summary> 慎重治沙者 I </summary>
    public bool DegradationTriggered;
}
