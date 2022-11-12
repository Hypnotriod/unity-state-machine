using Assets.Scripts.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComponent : MonoBehaviour
{
    protected ActionHandler CompletedHandler()
    {
        return new ActionHandler().Complete();
    }

    protected Coroutine Delay(float seconds, Action action)
    {
        return StartCoroutine(DelayRoutine(seconds, action));
    }

    private IEnumerator DelayRoutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}
