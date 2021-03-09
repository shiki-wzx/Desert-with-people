using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class MapMgr : MonoBehaviour {
    /// <summary> Update green value. Call at end of turn. </summary>
    /// todo: optimization
    [ContextMenu("Propagate GreenValue")]
    public MapUpdInfo PropagateGreenValue(bool skipDesertify = false) {
        var muInfo = new MapUpdInfo { SeedingTriggered = false, DegradationTriggered = false };

        // double buffer green value
        var dbBufGv = new (int cur, int buf)[childBlks.Count];

        // calculate delta caused by action (sand control & planting)
        foreach(var blk in childBlks) {
            if(blk.BlkParam.Type.Greenable())
                dbBufGv[blk.BlkIdx].cur = dbBufGv[blk.BlkIdx].buf = blk.BlkParam.GreenValue + blk.GreenDelta;
            blk.ClearGreenDelta();
        }

        // desertification
        if(!skipDesertify) {
            foreach(var blk in childBlks) {
                if(!blk.BlkParam.Type.Greenable())
                    continue;

                var adjBlk = blk.GetAdjacentBlk();
                var sandCnt = adjBlk.Count(
                    adj => adj.BlkParam.Type.Greenable() && dbBufGv[adj.BlkIdx].cur < 0
                );

                dbBufGv[blk.BlkIdx].buf = dbBufGv[blk.BlkIdx].cur;
                if(sandCnt > adjBlk.Count - sandCnt)
                    --dbBufGv[blk.BlkIdx].buf;
            }
            for(var i = 0; i < dbBufGv.Length; ++i)
                dbBufGv[i].cur = dbBufGv[i].buf;
        }

        // seeding
        foreach(var blk in childBlks) {
            if(!blk.BlkParam.Type.Greenable() || dbBufGv[blk.BlkIdx].cur < BlockType.ZhiWu.ToGreenValue())
                continue;
            var adjBlk = blk.GetAdjacentBlk();
            foreach(var adj in adjBlk) {
                if(adj.BlkParam.Type.Greenable() &&
                   dbBufGv[adj.BlkIdx].cur.ToBlockType().In(BlockType.PingDi, BlockType.CaoDi)) {
                    ++dbBufGv[adj.BlkIdx].buf;
                    muInfo.SeedingTriggered = true;
                }
            }
        }
        for(var i = 0; i < dbBufGv.Length; ++i)
            dbBufGv[i].cur = dbBufGv[i].buf;

        // apply delta & Sibling => Plant
        foreach(var blk in childBlks) {
            if(!blk.BlkParam.Type.Greenable())
                continue;
            var newType = dbBufGv[blk.BlkIdx].cur.ToBlockType(clamp: true);
            if(newType == BlockType.YouMiao)
                newType = BlockType.ZhiWu;
            if(newType.ToGreenValue() < blk.BlkParam.GreenValue)
                muInfo.DegradationTriggered = true;
            blk.SetBlkInfo(newType);
        }

        return muInfo;
    }


    /// <summary> Helper function for achievements </summary>
    public int CountBlk(params BlockType[] types) => childBlks.Count(blk => blk.BlkParam.Type.In(types));


    private readonly List<BlkRtInfo> childBlks = new List<BlkRtInfo>();


    /// <remarks> Called by BlkRtInfo. </remarks>
    public void RegisterBlk(BlkRtInfo rtInfo) {
        childBlks.Add(rtInfo);
    }
}
