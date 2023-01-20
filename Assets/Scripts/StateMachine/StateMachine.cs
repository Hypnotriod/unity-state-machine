﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        private State state = null;

        protected void NextState(State state)
        {
            this.state = state;
            this.state.Begin();
        }

        protected void SuspendCurrentState()
        {
            this.state?.Suspend();
        }

        protected void ResumeCurrentState()
        {
            this.state?.Resume();
        }

        protected List<Queue<Func<ActionHandler>>> InParallel(params Queue<Func<ActionHandler>>[] list)
        {
            return new List<Queue<Func<ActionHandler>>>(list);
        }

        protected List<Queue<Func<ActionHandler>>> InParallel(params Func<ActionHandler>[] list)
        {
            return list.Select(h =>
            {
                Queue<Func<ActionHandler>> queue = new();
                queue.Enqueue(h);
                return queue;
            }).ToList();
        }

        protected Queue<Func<ActionHandler>> InSequence(params Func<ActionHandler>[] list)
        {
            return new Queue<Func<ActionHandler>>(list);
        }

        protected ActionHandler InParallelNested(params Queue<Func<ActionHandler>>[] list)
        {
            return InParallelNested(InParallel(list));
        }

        protected ActionHandler InParallelNested(params Func<ActionHandler>[] list)
        {
            return InParallelNested(InParallel(list));
        }

        protected ActionHandler InParallelNested(List<Queue<Func<ActionHandler>>> list)
        {
            ActionHandler nestedHandler = new();
            ActionHandler abortHandler = new();
            nestedHandler.WithAbort(() => abortHandler.Complete());
            State state = new(list,
                (State state) => state.RegisterAbortHandler(abortHandler, () => { }),
                () => nestedHandler.Complete());
            state.Begin();
            return nestedHandler;
        }
    }
}
