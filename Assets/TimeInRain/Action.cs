public enum ActionType{
    HandleSandland=1,
    Plant=3,
    end=-1
}
public class Action {
    public ActionType type;
    public int cost()
    {
        if (type != ActionType.end) return (int)type;
        else return -1;
    }
}
