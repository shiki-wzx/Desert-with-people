using System.Collections.Generic;
using UnityEngine;


public partial class BlkRtInfo {
    [SerializeField] private GameObject workerPrefab;

    private List<GameObject> workerGos;


    // todo: use spawn point
    private void SpawnWorkers() {
        var magicHide = Random.Range(0, 3);
        if(magicHide != 0)
            workerGos.Add(Instantiate(workerPrefab,
                                      transform.position + new Vector3(-0.09f, 0.25f, 0.34f),
                                      Quaternion.Euler(0, -116, 0)));
        if(magicHide != 1)
            workerGos.Add(Instantiate(workerPrefab,
                                      transform.position + new Vector3(0.42f, 0.25f, -0.08f),
                                      Quaternion.Euler(0, 0, 0)));
        if(magicHide != 2)
            workerGos.Add(Instantiate(workerPrefab,
                                      transform.position + new Vector3(-0.08f, 0.25f, -0.48f),
                                      Quaternion.Euler(0, 169, 0)));
    }


    private void ClearWorkers() {
        foreach(var w in workerGos)
            Destroy(w);
        workerGos.Clear();
    }


    /// <returns> whether action applied </returns>
    public bool SandControl() {
        if(HasActionQueued || !BlkParam.Type.In(BlockType.ShaQiu, BlockType.ShaDi, BlockType.PingDi))
            return false;
        HasActionQueued = true;
        UpdateGreenDeltaBy(1);
        SpawnWorkers();
        return true;
    }


    /// <returns> whether action applied </returns>
    public bool Planting() {
        if(HasActionQueued || BlkParam.Type != BlockType.CaoDi)
            return false;
        HasActionQueued = true;
        SetBlkType(BlockType.YouMiao);
        //SpawnWorkers();
        return true;
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
    /// Clear buffered delta & queued action & model. Called at the end of turn by MapMgr.
    /// </summary>
    public void ClearGreenDelta() {
        HasActionQueued = false;
        GreenDelta = 0;
        ClearWorkers();
    }


    private void Awake() {
        workerGos = new List<GameObject>();
        ClearGreenDelta();
        var mapMgr = GetComponentInParent<MapMgr>();
        mapMgr.RegisterBlk(this);
    }
}
