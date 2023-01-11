using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        protected void NextState(State state)
        {
            state.Begin();
        }

        protected List<Queue<Func<ActionHandler>>> InParallel(params Queue<Func<ActionHandler>>[] list)
        {
            return new List<Queue<Func<ActionHandler>>>(list);
        }

        protected Queue<Func<ActionHandler>> InSequence(params Func<ActionHandler>[] list)
        {
            return new Queue<Func<ActionHandler>>(list);
        }

        protected ActionHandler InParallelNested(params Queue<Func<ActionHandler>>[] list)
        {
            ActionHandler nestedHandler = new();
            ActionHandler abortHandler = new();
            nestedHandler.WithAbort(() => abortHandler.Complete());
            State state = new(InParallel(list),
                (State state) => state.RegisterAbortHandler(abortHandler, () => { }),
                () => nestedHandler.Complete());
            state.Begin();
            return nestedHandler;
        }
    }
}
