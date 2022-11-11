using Assets.Scripts.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine
{
    public GameObject gameObject1;
    public GameObject gameObject2;

    private ComponentWithActions component1;
    private ComponentWithActions component2;

    public void Start()
    {
        component1 = gameObject1.GetComponent<ComponentWithActions>();
        component2 = gameObject2.GetComponent<ComponentWithActions>();

        NextState(Initial());
    }

    private State Initial()
    {
        return new State(InParallel(
            InSequence(
                () => component1.ActionInstant(),
                () => component1.ActionDelayed(1f),
                () => component1.ActionDelayed(1.5f),
                () => component1.ActionDelayed(2f)),
            InSequence(
                () => component2.ActionDelayed(2f),
                () => component2.ActionDelayed(1.5f),
                () => component2.ActionDelayed(0.5f),
                () => component2.ActionInstant())
        ), () =>
        {
            Debug.Log("Moved to next state");
            NextState(Next());
        });
    }

    private State Next()
    {
        return new State(InParallel(
            InSequence(
                () => component1.ActionInstant(),
                () => component1.ActionDelayed(3f)),
            InSequence(
                () => component2.ActionDelayed(2f))
        ), () =>
        {
            Debug.Log("Done");
        });
    }
}
