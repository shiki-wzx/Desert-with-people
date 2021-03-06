using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public int movementsAvalable;
    public int movementsMaximun;
    public void NewRound(int newMovementsMaximun)
    {
        movementsMaximun = newMovementsMaximun;
        movementsAvalable = movementsMaximun;
    }

    public void Action(Action action)
    {
        GameFlowCtrler ctrler = FindObjectOfType<GameFlowCtrler>()._instance;
        switch (action.type)
        {
            case ActionType.HandleSandland:
                {
                    if (movementsAvalable - action.cost() > 0)
                    {
                        //handlesand func
                        movementsAvalable -= action.cost();
                    }
                    else
                    {
                        Debug.Log("Not enough action point.");
                        //do something like UI events
                    }
                    break;
                }
            case ActionType.Plant:
                {
                    if (movementsAvalable - action.cost() > 0)
                    {
                        //plant func
                        movementsAvalable -= action.cost();
                    }
                    else
                    {
                        Debug.Log("Not enough action point.");
                        //do something
                    }
                    break;
                }
            case ActionType.end:
                {
                    ctrler.NewRound();
                    break;
                }
        }

    }

}
