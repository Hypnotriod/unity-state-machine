using Assets.Scripts.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComponent : MonoBehaviour
{
    private readonly Queue<StateHandler> stateHandlers = new();

    protected StateHandler EnqueueHandler()
    {
        var handler = new StateHandler();
        stateHandlers.Enqueue(handler);
        return handler;
    }

    protected StateHandler CompletedHandler()
    {
        return new StateHandler().Complete();
    }

    protected void CompleteState()
    {
        if (stateHandlers.Count == 0) return;
        stateHandlers.Dequeue()?.Complete();
    }

    protected void Delay(float seconds, Action action)
    {
        StartCoroutine(DelayRoutine(seconds, action));
    }

    private IEnumerator DelayRoutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}
