using System;
using System.Collections;
using UnityEngine;

namespace AIActions
{
    public class DroneLeaveScreenAction : CoroutineAction
    {
        private DroneController _drone;
        private float _cooldown;

        public DroneLeaveScreenAction(DroneController drone, float cooldown)
        {
            _drone = drone;
            _cooldown = cooldown;
        }

        protected override IEnumerator DoAction()
        {
            Vector3 dest = CoordsUtils.RandomWorldPointOffScreen();
            _drone.FlyToDest(dest);
            while (_drone.IsMoving)
            {
                yield return null;
            }
            yield return new WaitForSeconds(_cooldown);
        }

        protected override void CancelAction()
        {
            _drone.ResetDest();
        }
    }
}