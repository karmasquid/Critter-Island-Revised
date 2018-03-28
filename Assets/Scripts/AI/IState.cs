using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState{

    // three actions within each state.

    void Enter();

    void Execute();

    void Exit();

}
// Stina Hedman
