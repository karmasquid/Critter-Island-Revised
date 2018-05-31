using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {

    IState currRunningState;

    IState prevState;

    //used to change state in statemachine.
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

    //used to execute state update.
    public void ExecuteStateUpdate()
    {
        var runningState = this.currRunningState;

        if (runningState!= null)
        {
            runningState.Execute();
        }
    }

    //used to switch to previous state.
    public void SwitchToPrevState()
    {
        this.currRunningState.Exit();
        this.currRunningState = this.prevState;
        this.currRunningState.Enter();
    }
}// Stina Hedman
