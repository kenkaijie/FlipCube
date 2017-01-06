using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void StateCallback<StateEnum>(StateEnum currentState, StateTransitionError errorCode);

// these are states meant for logic only controllers
public enum LogicControllerState
{
    CREATED,
    IDLE,
    BUSY,
    ERROR
}

public enum StateTransitionError
{
    NOERROR,
    ERROR_INVALID_TRANSITION
}

public interface IStateful<StateEnum>
{
    void SetOnStateChangeHandler(StateCallback<StateEnum> newState);
    void TransitionToState(StateEnum newState);
    StateEnum GetState();
}
