using System;

namespace Assets.Scripts.StateMachine
{
    public class ActionHandler
    {
        private bool isCompleted = false;
        private Action completeAction = null;
        private bool isAborted = false;
        private Action abortAction = null;
        private bool isSuspended = false;
        private Action suspendAction = null;
        private Action resumeAction = null;

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

        public bool IsSuspended
        {
            get
            {
                return this.isSuspended;
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

        public ActionHandler WithSuspendResume(Action suspendAction, Action resumeAction)
        {
            if (isCompleted || isAborted) { return this; }
            this.suspendAction = suspendAction;
            this.resumeAction = resumeAction;
            return this;
        }

        public ActionHandler Suspend()
        {
            if (isCompleted || isAborted || isSuspended) { return this; }
            isSuspended = true;
            suspendAction?.Invoke();
            return this;
        }

        public ActionHandler Resume()
        {
            if (isCompleted || isAborted || !isSuspended) { return this; }
            isSuspended = false;
            resumeAction?.Invoke();
            return this;
        }

        public ActionHandler Complete()
        {
            if (isCompleted || isAborted) { return this; }
            isCompleted = true;
            InvokeCompleted();
            return this;
        }

        public ActionHandler Abort()
        {
            if (isCompleted || isAborted) { return this; }
            isAborted = true;
            InvokeAborted();
            return this;
        }

        private void InvokeCompleted()
        {
            abortAction = null;
            suspendAction = null;
            resumeAction = null;
            var tmp = completeAction;
            completeAction = null;
            tmp?.Invoke();
        }

        private void InvokeAborted()
        {
            completeAction = null;
            suspendAction = null;
            resumeAction = null;
            var tmp = abortAction;
            abortAction = null;
            tmp?.Invoke();
        }
    }
}
