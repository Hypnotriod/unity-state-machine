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

        private void HandleNextState(State state, Queue<Func<StateHandler>> queue = null)
        {
            queue?.Dequeue();

            int queuedTasksCount = state.TaskQueues.Sum(q => q.Count);

            if (queue != null)
            {
                if (queue.Count > 0)
                {
                    var handler = queue.Peek();
                    handler().Completed = () => HandleNextState(state, queue);
                }
                else if (queuedTasksCount == 0)
                {
                    state.Complete();
                }
                return;
            }

            if (queuedTasksCount == 0)
            {
                state.Complete();
                return;
            }

            foreach (var q in state.TaskQueues)
            {
                if (q.Count > 0)
                {
                    var handler = q.Peek();
                    handler().Completed = () => HandleNextState(state, q);
                }
            }
        }

        protected List<Queue<Func<StateHandler>>> InParallel(params Queue<Func<StateHandler>>[] list)
        {
            return new List<Queue<Func<StateHandler>>>(list);
        }

        protected Queue<Func<StateHandler>> InSequence(params Func<StateHandler>[] list)
        {
            return new Queue<Func<StateHandler>>(list);
        }
    }
}
