using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class BlkRtInfo : MonoBehaviour {
    /// <remarks> This is set in prefab. </remarks>
    [SerializeField] private BlockTypeDict blockTypeDict;

    /// <summary>
    /// Block index. Set by GenBlkPh. Starts from 0. Usually no need to change.
    /// </summary>
    public int BlkIdx { get; private set; }

    public BlockParam BlkParam { get; private set; }


    private void Start() {
        var subs = gameObject.name.Split(' ');
        BlkIdx = int.Parse(subs[1]);
    }


    /// <summary>
    /// Update block. This will update UI at the same time.
    /// </summary>
    public void SetBlkInfo(BlockType type) => SetBlkInfo(type, BlkIdx);

    /// <summary>
    /// Typically called by GenBlkPh.
    /// </summary>
    public void SetBlkInfo(BlockType type, int idx) {
        BlkIdx = idx;
        BlkParam = blockTypeDict[type];
        gameObject.name = $"Hexagon {BlkIdx}";
        var children = (from Transform child in transform select child.gameObject).ToList();
        foreach(var child in children)
            Destroy(child.gameObject);
        Instantiate(BlkParam.BlockPrefab, transform, false);
    }


    // todo: hard coded; refer to same source instead
    private const float BlkWidth = .578f, BlkHeight = .2f;

    private readonly RaycastHit[] hitRst = new RaycastHit[1];

    /// <summary>
    /// Get adjacent blocks.
    /// </summary>
    public List<BlkRtInfo> GetAdjacentBlk() {
        var adjBlks = new List<BlkRtInfo>();

        var ori = transform.position + BlkHeight / 2 * Vector3.up;
        var dir = Vector3.forward;
        var rot = Quaternion.AngleAxis(60, Vector3.up);
        var mask = LayerMask.GetMask("BlkLayer");

        for(var tt = 0; tt < 6; ++tt) {
            // will not hit self (probably)
            var hits = Physics.RaycastNonAlloc(ori, dir, hitRst, BlkWidth, mask);
            dir = rot * dir;
            if(hits < 1)
                continue;
            // will not be null (probably)
            var rtInfo = hitRst[0].collider.GetComponentInParent<BlkRtInfo>();
            adjBlks.Add(rtInfo);
        }

        return adjBlks;
    }
}
