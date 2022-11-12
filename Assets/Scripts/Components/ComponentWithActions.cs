using Assets.Scripts.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;

internal class ComponentWithActions : GameComponent
{
    private readonly ActionHandler actionDelayedHandler = new();

    public ActionHandler ActionInstant()
    {
        Debug.LogFormat("Action on {0}: Instant", gameObject.name);
        return CompletedHandler();
    }

    public ActionHandler ActionDelayed(float seconds)
    {
        Debug.LogFormat("Action on {0}: Wait for {1} seconds", gameObject.name, seconds);
        Delay(seconds, () =>
        {
            Debug.LogFormat("Action on {0}: Completed after {1} seconds", gameObject.name, seconds);
            actionDelayedHandler.Complete();
        });

        return actionDelayedHandler.Reset();
    }
}
