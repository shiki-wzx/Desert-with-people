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

        if(currAction == ActionType.DesertHandle) {
            var success = info.SandControl();
            if(success) {
                AudioMgr.Instance.PlayFx(AudioFxType.ControlSand);
                var cost = GameFlowCtrler.Instance.mp[ActionType.DesertHandle];
                GameFlowCtrler.Instance.labourForce -= (int)(cost * GameFlowCtrler.Instance.labourForceCostCoef);
            }
        }
        else {
            var success = info.Planting();
            if(success) {
                AudioMgr.Instance.PlayFx(AudioFxType.PlantTree);
                var cost = GameFlowCtrler.Instance.mp[ActionType.Plant];
                GameFlowCtrler.Instance.labourForce -= (int)(cost * GameFlowCtrler.Instance.labourForceCostCoef);
            }
        }

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
