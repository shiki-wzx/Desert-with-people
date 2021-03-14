using PlayerActions;


/// <summary>
/// 当要执行 move 的时候调用这个接口
/// </summary>
public class PlayerMovement : SingletonMono<PlayerMovement> {
    public void ForceRelease() {
        MapMgr.Instance.UnGreyAll();
        currAction = ActionType.None;
    }


    private void MapBlkClickHandle(BlkRtInfo info) {
        if(currAction == ActionType.None)
            return;
        if(info.HasActionQueued)
            return;

        int cost;
        if(currAction == ActionType.DesertHandle) {
            info.SandControl();
            AudioMgr.Instance.PlayFx(AudioFxType.ControlSand);
            cost = 10;
        }
        else {
            info.Planting();
            AudioMgr.Instance.PlayFx(AudioFxType.PlantTree);
            cost = 30;
        }

        GameFlowCtrler.Instance.labourForce -= (int)(cost * GameFlowCtrler.Instance.labourForceCostCoef);

        MapMgr.Instance.UnGreyAll();
        currAction = ActionType.None;
    }


    //protected override void OnInstanceAwake()
    //{}

    private void OnEnable() => CameraControl.Instance.ClickCallback += MapBlkClickHandle;
    private void OnDisable() => CameraControl.Instance.ClickCallback -= MapBlkClickHandle;

    private ActionType currAction = ActionType.None;


    public void Move(ActionType action) {
        switch(action) {
        case ActionType.DesertHandle:
            if(currAction == ActionType.None) {
                currAction = ActionType.DesertHandle;
                MapMgr.Instance.GreyExceptType(BlockType.ShaQiu, BlockType.ShaDi, BlockType.PingDi);
            }
            break;

        case ActionType.Plant:
            if(currAction == ActionType.None) {
                currAction = ActionType.Plant;
                MapMgr.Instance.GreyExceptType(BlockType.CaoDi);
            }
            break;
        }
    }
}
