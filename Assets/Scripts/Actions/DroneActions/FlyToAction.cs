using System;
using UnityEngine;

namespace AIActions
{
    public class FlyToAction : AIAction
    {
        private DroneController _controller;
        private Vector3 _dest;

        public FlyToAction(DroneController controller, Vector3 dest)
        {
            _controller = controller;
            _dest = dest;
        }

        public override void Interrupt()
        {
            _controller.ResetDest();
        }

        public override bool IsFinished()
        {
            return !_controller.IsMoving;
        }

        public override void StartAction()
        {
            _controller.FlyToDest(_dest);
        }

        public override void UpdateAction()
        {

        }
    }
}
