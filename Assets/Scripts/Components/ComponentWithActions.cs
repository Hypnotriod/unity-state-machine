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
    public StateHandler ActionInstant()
    {
        Debug.LogFormat("Action on {0}: Instant", gameObject.name);
        return CompletedHandler();
    }

    public StateHandler ActionDelayed(float seconds)
    {
        Debug.LogFormat("Action on {0}: Begun", gameObject.name);
        Delay(seconds, () =>
        {
            Debug.LogFormat("Action on {0}: Completed after {1} seconds", gameObject.name, seconds);
            CompleteState();
        });

        return EnqueueHandler();
    }
}
