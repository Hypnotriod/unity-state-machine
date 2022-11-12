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
            HandleNextState();
        }

        private void HandleNextState(Queue<Func<ActionHandler>> queue = null, ActionHandler handler = null)
        {
            if (isCompleted || isAborted) { return; }

            queue?.Dequeue();

            int queuedActionsCount = actionQueues.Sum(q => q.Count);

            if (queue != null && handler != null)
            {
                activeHandlers.Remove(handler);
                if (queue.Count > 0)
                {
                    var h = queue.Peek()();
                    activeHandlers.Add(h);
                    h.WithComplete(() => HandleNextState(queue, h));
                }
                else if (queuedActionsCount == 0)
                {
                    Complete();
                }
                return;
            }

            if (queuedActionsCount == 0)
            {
                Complete();
                return;
            }

            foreach (var q in actionQueues)
            {
                if (q.Count > 0)
                {
                    var h = q.Peek()();
                    activeHandlers.Add(h);
                    h.WithComplete(() => HandleNextState(q, h));
                }
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
