public partial class BlkRtInfo {
    public void SandControl() {
        if(HasActionQueued || !BlkParam.Type.In(BlockType.ShaQiu, BlockType.ShaDi, BlockType.PingDi))
            return;
        HasActionQueued = true;
        UpdateGreenDeltaBy(1);
    }


    public void Planting() {
        if(HasActionQueued || BlkParam.Type != BlockType.CaoDi)
            return;
        HasActionQueued = true;
        SetBlkInfo(BlockType.YouMiao);
    }


    /// <remarks> Ues this to prevent double action on same block. </remarks>
    public bool HasActionQueued { get; private set; }

    public int GreenDelta { get; private set; }


    /// <summary>
    /// Update green value delta. Typically, this should only be set by an "action".
    /// </summary>
    /// <param name="inc"> The delta of delta. NOT the value of delta. </param>
    /// <returns> The green value AFTER the delta be applied </returns>
    private int UpdateGreenDeltaBy(int inc) {
        GreenDelta += inc;
        return GreenDelta + BlkParam.GreenValue;
    }


    /// <summary>
    /// Clear buffered delta & queued action. Called at the end of turn by MapMgr.
    /// </summary>
    public void ClearGreenDelta() {
        HasActionQueued = false;
        GreenDelta = 0;
    }


    private MapMgr mapMgr;


    private void Awake() {
        ClearGreenDelta();
        mapMgr = GetComponentInParent<MapMgr>();
        mapMgr.RegisterBlk(this);
    }
}
