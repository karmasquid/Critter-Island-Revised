using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState {

    public void Enter()
    {
        Debug.Log("pew1");  
    }

    public void Execute()
    {
        Debug.Log("chilling like a baws");
    }

    public void Exit()
    {
        Debug.Log("pewwawaeaweapew");
    }
}
