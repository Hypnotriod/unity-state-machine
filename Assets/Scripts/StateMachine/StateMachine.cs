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
            HandleNextState(state);
        }

        private void HandleNextState(State state, Queue<Func<ActionHandler>> queue = null)
        {
            queue?.Dequeue();

            int queuedActionsCount = state.ActionQueues.Sum(q => q.Count);

            if (queue != null)
            {
                if (queue.Count > 0)
                {
                    var handler = queue.Peek();
                    handler().Completed = () => HandleNextState(state, queue);
                }
                else if (queuedActionsCount == 0)
                {
                    state.Complete();
                }
                return;
            }

            if (queuedActionsCount == 0)
            {
                state.Complete();
                return;
            }

            foreach (var q in state.ActionQueues)
            {
                if (q.Count > 0)
                {
                    var handler = q.Peek();
                    handler().Completed = () => HandleNextState(state, q);
                }
            }
        }

        protected List<Queue<Func<ActionHandler>>> InParallel(params Queue<Func<ActionHandler>>[] list)
        {
            return new List<Queue<Func<ActionHandler>>>(list);
        }

        protected Queue<Func<ActionHandler>> InSequence(params Func<ActionHandler>[] list)
        {
            return new Queue<Func<ActionHandler>>(list);
        }
    }
}
