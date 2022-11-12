using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.StateMachine
{
    public class ActionHandler
    {
        private bool isCompleted = false;
        private Action completed = null;

        public Action Completed
        {
            set
            {
                if (this.isCompleted)
                {
                    value?.Invoke();
                }
                else
                {
                    this.completed = value;
                }
            }
        }

        public bool IsCompleted()
        {
            return isCompleted;
        }

        public ActionHandler Complete()
        {
            isCompleted = true;
            InvokeCompleted();
            return this;
        }

        public ActionHandler Reset()
        {
            isCompleted = false;
            InvokeCompleted();
            return this;
        }

        private void InvokeCompleted()
        {
            var tmp = completed;
            completed = null;
            tmp?.Invoke();
        }
    }
}
