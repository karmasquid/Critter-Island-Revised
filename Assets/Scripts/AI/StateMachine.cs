using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {

    IState currRunningState;

    IState prevState;

    public void ChangeState(IState newState)
    {
        if (this.currRunningState != null)
        {
            this.currRunningState.Exit();
        }

        this.prevState = currRunningState;

        this.currRunningState = newState;

        this.currRunningState.Enter();
    }

    public void ExecuteStateUpdate()
    {
        var runningState = this.currRunningState;

        if (runningState!= null)
        {
            runningState.Execute();
        }
    }

    public void SwitchToPrevState()
    {
        this.currRunningState.Exit();
        this.currRunningState = this.prevState;
        this.currRunningState.Enter();
    }
}// Stina Hedman
