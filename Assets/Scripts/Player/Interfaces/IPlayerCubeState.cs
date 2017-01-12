using System;
using UnityEngine;

public enum TransitionStatus
{
    NO_ERROR,
    ERROR
}

public class StateTransition<T>
{
    public T nextState { get; private set; }
    public TransitionStatus status { get; private set; }

    public StateTransition(T ns, TransitionStatus ts)
    {
        nextState = ns;
        status = ts;
    }
}

public interface IPlayerCubeState
{
    void OnStateEntry(Vector3 position);

    StateTransition<IPlayerCubeState> StartTumble (Vector3 direction);

    StateTransition<IPlayerCubeState> UpdateState ();
    StateTransition<IPlayerCubeState> FixedUpdateState ();


}
