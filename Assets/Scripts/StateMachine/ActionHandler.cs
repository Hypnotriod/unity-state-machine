using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.StateMachine
{
    public class ActionHandler
    {
        private bool isCompleted = false;
        private Action completeAction = null;
        private bool isAborted = false;
        private Action abortAction = null;

        public bool IsCompleted
        {
            get
            {
                return this.isCompleted;
            }
        }

        public bool IsAborted
        {
            get
            {
                return this.isAborted;
            }
        }

        public ActionHandler WithComplete(Action completeAction)
        {
            if (this.isCompleted)
            {
                completeAction?.Invoke();
            }
            else if (!this.isAborted)
            {
                this.completeAction = completeAction;
            }
            return this;
        }

        public ActionHandler WithAbort(Action abortAction)
        {
            if (!this.isCompleted && !this.isAborted)
            {
                this.abortAction = abortAction;
            }
            return this;
        }

        public ActionHandler Complete()
        {
            isCompleted = true;
            InvokeCompleted();
            return this;
        }

        public ActionHandler Abort()
        {
            isAborted = true;
            InvokeAborted();
            return this;
        }

        public ActionHandler Set()
        {
            isAborted = false;
            isCompleted = false;
            InvokeCompleted();
            return this;
        }

        private void InvokeCompleted()
        {
            abortAction = null;
            var tmp = completeAction;
            completeAction = null;
            tmp?.Invoke();
        }

        private void InvokeAborted()
        {
            completeAction = null;
            var tmp = abortAction;
            abortAction = null;
            tmp?.Invoke();
        }
    }
}
