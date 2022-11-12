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

    private State Initial() => new(InParallel(
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


    private State Next() => new(InSequence(
        () => component1.ActionInstant(),
        () => component2.ActionDelayed(2f),
        () => component1.ActionDelayed(3f),
        () => component2.ActionInstant()
    ), () =>
    {
        Debug.Log("Done");
    });
}
