using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.XR;

namespace Assets.Scripts.StateMachine
{
    public class State
    {
        private readonly List<ActionHandler> abortHandlers = new();
        private readonly List<ActionHandler> activeHandlers = new();
        private readonly List<Queue<Func<ActionHandler>>> actionQueues;
        private Action completeAction;
        private Action<State> beginAction;
        private bool isCompleted = false;
        private bool isAborted = false;
        private bool isStarted = false;

        public State(List<Queue<Func<ActionHandler>>> actionQueues, Action completeAction)
        {
            this.actionQueues = actionQueues;
            this.completeAction = completeAction;
        }

        public State(Queue<Func<ActionHandler>> queue, Action completeAction)
        {
            this.actionQueues = new List<Queue<Func<ActionHandler>>> { queue };
            this.completeAction = completeAction;
        }

        public State(List<Queue<Func<ActionHandler>>> actionQueues, Action<State> beginAction, Action completeAction)
        {
            this.actionQueues = actionQueues;
            this.completeAction = completeAction;
            this.beginAction = beginAction;
        }

        public State(Queue<Func<ActionHandler>> queue, Action<State> beginAction, Action completeAction)
        {
            this.actionQueues = new List<Queue<Func<ActionHandler>>> { queue };
            this.completeAction = completeAction;
            this.beginAction = beginAction;
        }

        public void RegisterAbortHandler(ActionHandler actionHandler, Action action)
        {
            abortHandlers.Add(actionHandler.WithComplete(() => Abort(action)));
        }

        public void Begin()
        {
            if (isStarted) { return; }
            isStarted = true;
            beginAction?.Invoke(this);
            HandleStates();
            TryToComplete();
        }

        private void HandleStates()
        {
            if (isCompleted || isAborted) { return; }

            foreach (var q in actionQueues)
            {
                while (q.Count > 0)
                {
                    var h = q.Dequeue()();
                    if (!h.IsCompleted)
                    {
                        activeHandlers.Add(h);
                        h.WithComplete(() => HandleNextQueuedState(q, h));
                        break;
                    }
                }
            }
        }

        private void HandleNextQueuedState(Queue<Func<ActionHandler>> queue, ActionHandler handler)
        {
            if (isCompleted || isAborted) { return; }

            activeHandlers.Remove(handler);

            while (queue.Count > 0)
            {
                var h = queue.Dequeue()();
                if (!h.IsCompleted)
                {
                    activeHandlers.Add(h);
                    h.WithComplete(() => HandleNextQueuedState(queue, h));
                    return;
                }
            }
            TryToComplete();
        }

        private void TryToComplete()
        {
            if (isCompleted || isAborted) { return; }
            if (activeHandlers.Count == 0 && actionQueues.Sum(q => q.Count) == 0)
            {
                Complete();
            }
        }

        private void Complete()
        {
            isCompleted = true;
            Drain();
            var tmp = completeAction;
            completeAction = null;
            tmp.Invoke();
        }

        private void Abort(Action action)
        {
            isAborted = true;
            Drain();
            action?.Invoke();
        }

        private void Drain()
        {
            foreach (var handler in abortHandlers)
            {
                handler.Abort();
            }
            foreach (var handler in activeHandlers)
            {
                handler.Abort();
            }
            beginAction = null;
            abortHandlers.Clear();
            activeHandlers.Clear();
            actionQueues.Clear();
        }
    }
}
