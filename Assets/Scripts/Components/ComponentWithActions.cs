using Assets.Scripts.StateMachine;
using UnityEngine;

internal class ComponentWithActions : GameComponent
{
    public ActionHandler ActionInstant()
    {
        Debug.LogFormat("Action on {0}: Instant", gameObject.name);
        return CompletedHandler();
    }

    public ActionHandler ActionDelayed(float seconds)
    {
        ActionHandler handler = new();
        Debug.LogFormat("Action on {0}: Wait for {1} seconds", gameObject.name, seconds);
        Coroutine delayCoroutine = Delay(seconds, () =>
        {
            Debug.LogFormat("Action on {0}: Completed after {1} seconds", gameObject.name, seconds);
            handler.Complete();
        });

        return handler.WithAbort(() =>
        {
            Debug.LogFormat("Action on {0}: Wait for {1} seconds: Aborted", gameObject.name, seconds);
            StopCoroutine(delayCoroutine);
        });
    }
}
