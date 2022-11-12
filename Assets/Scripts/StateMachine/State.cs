using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachine
{
    public class State
    {
        public List<Queue<Func<ActionHandler>>> ActionQueues { get; }
        private Action Completed { get; set; }

        public State(List<Queue<Func<ActionHandler>>> actionQueues, Action completed)
        {
            ActionQueues = actionQueues;
            Completed = completed;
        }

        public State(Queue<Func<ActionHandler>> queue, Action completed)
        {
            ActionQueues = new List<Queue<Func<ActionHandler>>> { queue };
            Completed = completed;
        }

        public void Complete()
        {
            Completed?.Invoke();
            Completed = null;
        }
    }
}
