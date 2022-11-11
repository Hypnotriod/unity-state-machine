using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachine
{
    public class State
    {
        public List<Queue<Func<StateHandler>>> TaskQueues { get; }
        private Action Completed { get; set; }

        public State(List<Queue<Func<StateHandler>>> taskQueues, Action completed)
        {
            TaskQueues = taskQueues;
            Completed = completed;
        }

        public State(Queue<Func<StateHandler>> queue, Action completed)
        {
            TaskQueues = new List<Queue<Func<StateHandler>>> { queue };
            Completed = completed;
        }

        public void Complete()
        {
            Completed?.Invoke();
            Completed = null;
        }
    }
}
