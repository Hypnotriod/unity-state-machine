using Assets.Scripts.StateMachine;
using UnityEngine;

internal class ComponentWithActions : GameComponent
{
    public ActionHandler ActionInstant()
    {
        Debug.LogFormat("Action on {0}: Instant", gameObject.name);
        return CompletedHandler();
    }

    private readonly ActionHandlersQueue actionDelayed = new();
    public ActionHandler ActionDelayed(float seconds)
    {
        Debug.LogFormat("Action on {0}: Wait for {1} seconds", gameObject.name, seconds);
        Coroutine delayCoroutine = Delay(seconds, () =>
        {
            Debug.LogFormat("Action on {0}: Completed after {1} seconds", gameObject.name, seconds);
            actionDelayed.Complete();
        });

        return actionDelayed.Enqueue().WithAbort(() =>
        {
            Debug.LogFormat("Action on {0}: Wait for {1} seconds: Aborted", gameObject.name, seconds);
            StopCoroutine(delayCoroutine);
        });
    }
}
