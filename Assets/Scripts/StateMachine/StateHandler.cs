using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.StateMachine
{
    public class StateHandler
    {
        private bool done = false;
        private Action completed = null;

        public Action Completed
        {
            get
            {
                return this.completed;
            }
            set
            {
                if (this.done)
                {
                    value?.Invoke();
                }
                else
                {
                    this.completed = value;
                }
            }
        }

        public StateHandler Complete()
        {
            done = true;
            Completed?.Invoke();
            Completed = null;
            return this;
        }
    }
}
