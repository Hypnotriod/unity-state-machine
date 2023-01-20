using System.Collections.Generic;

namespace Assets.Scripts.StateMachine
{
    public class ActionHandlersQueue
    {
        private readonly Queue<ActionHandler> actionHandlers = new();

        public ActionHandler Enqueue()
        {
            ActionHandler handler = new();
            actionHandlers.Enqueue(handler);
            return handler;
        }

        public void Complete()
        {
            if (actionHandlers.Count == 0) { return; }
            actionHandlers.Dequeue().Complete();
        }

        public void Abort()
        {
            if (actionHandlers.Count == 0) { return; }
            actionHandlers.Dequeue().Abort();
        }
    }
}
