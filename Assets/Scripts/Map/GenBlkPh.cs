using System.Linq;
using UnityEngine;


public class GenBlkPh : MonoBehaviour {
    [SerializeField] private GameObject blkPhPfb;
    [SerializeField] private float blkWidth = .66f;

    private readonly Vector3 basePos = Vector3.zero;


    private void Awake() {
        if(transform.childCount == 0)
            DoGenBlkPh();
    }


    [ContextMenu("Generate Block Placeholders")]
    private void DoGenBlkPh() {
        // deltaX, deltaZ, blkCnt
        var deltas = new[] {
            (-3, 1, 4), (-2, 1, 5), (-1, 0, 6), (0, 0, 6), (1, 0, 5), (2, 1, 4)
        };

        var types = new[] {
            BlockType.ShaQiu, BlockType.ShaQiu, BlockType.ShaDi, BlockType.ShaDi,
            BlockType.ShaDi, BlockType.ShaDi, BlockType.PingDi, BlockType.ShaQiu, BlockType.ShaQiu,
            BlockType.ShaDi, BlockType.ShaQiu, BlockType.ZhiShaZhan, BlockType.ShaDi, BlockType.ShaDi, BlockType.ShaDi,
            BlockType.ShaQiu, BlockType.ShaDi, BlockType.PingDi, BlockType.ShaDi, BlockType.PingDi, BlockType.ShaQiu,
            BlockType.ShaDi, BlockType.ShaDi, BlockType.ShaDi, BlockType.ShaQiu, BlockType.ShaDi,
            BlockType.ShaQiu, BlockType.ShaDi, BlockType.ShaDi, BlockType.ShaQiu
        };

        // debug test code
        //var types = new[] {
        //    BlockType.ShaQiu, BlockType.ShaQiu, BlockType.PingDi, BlockType.CaoDi,
        //    BlockType.YouMiao, BlockType.ZhiWu, BlockType.PingDi, BlockType.ShaQiu, BlockType.ShaQiu,
        //    BlockType.ShaDi, BlockType.ShaQiu, BlockType.ZhiShaZhan, BlockType.ShaDi, BlockType.ShaDi, BlockType.ShaDi,
        //    BlockType.ShaQiu, BlockType.ShaDi, BlockType.PingDi, BlockType.ShaDi, BlockType.PingDi, BlockType.ShaQiu,
        //    BlockType.ShaDi, BlockType.ShaDi, BlockType.ShaDi, BlockType.ShaQiu, BlockType.ShaDi,
        //    BlockType.ShaQiu, BlockType.ShaDi, BlockType.ShaDi, BlockType.ShaQiu
        //};

        var blkIdx = 0;

        foreach(var (deltaX, deltaZ, blkCnt) in deltas) {
            var pos = basePos;
            pos.x += deltaX * blkWidth * 1.5f;
            if(deltaX % 2 != 0)
                pos.z += blkWidth * Mathf.Sqrt(3) / 2;
            pos.z += deltaZ * blkWidth * Mathf.Sqrt(3);

            for(var i = 0; i < blkCnt; ++i) {
                var go = Instantiate(blkPhPfb, transform, false);
                go.transform.localPosition = pos;
                go.GetComponent<BlkRtInfo>().SetBlkInfo(types[blkIdx], blkIdx);

                ++blkIdx;
                pos.z += blkWidth * Mathf.Sqrt(3);
            }
        }
    }


    [ContextMenu("Clear Block Placeholders")]
    private void DoClearBlkPh() {
        var children = (from Transform child in transform select child.gameObject).ToList();
        foreach(var child in children)
            DestroyImmediate(child.gameObject);
    }
}
