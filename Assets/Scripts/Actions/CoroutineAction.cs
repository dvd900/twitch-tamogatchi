using System;
using System.Collections;
using UnityEngine;

namespace AIActions
{
    public abstract class CoroutineAction : AIAction
    {
        private CoroutineTask _task;

        protected abstract IEnumerator DoAction();
        protected virtual void CancelAction() { }

        public sealed override void Interrupt()
        {
            CancelAction();
            _task.Stop();
        }

        public sealed override bool IsFinished()
        {
            return !_task.Running;
        }

        public sealed override void StartAction()
        {
            _task = new CoroutineTask(DoAction());
        }

        public sealed override void UpdateAction()
        {

        }
    }
}
