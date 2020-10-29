
using System.Collections;
using BezierSolution;
using UnityEngine;

namespace AIActions
{
    public class BeeIdleAction : AIAction
    {
        private const int NUM_POINTS = 7;
        private const float SWARM_RANGE = 20.0f;

        private BeeController _bee;
        private BezierSpline _spline;

        private float _t;
        private Vector3 _spottedPos;
        private Vector3 _hivePos;
        private bool _spotted;
        private bool _shouldChase;
        private float _spottedTimer;

        public BeeIdleAction(BeeController bee, BezierSpline spline)
        {
            _bee = bee;
            _spline = spline;
            _hivePos = bee.Hive.transform.position;

            _spline.Initialize(NUM_POINTS);

            _spline[0].position = _bee.transform.position - _bee.Hive.transform.position;
            for (int i = 1; i < NUM_POINTS; i++)
            {
                var pos = SWARM_RANGE * Random.insideUnitSphere;
                pos.y *= .5f;
                _spline[i].position = pos + 7 * Vector3.up;
            }

            _spline.AutoConstructSpline();
        }

        public override void Interrupt()
        {
            
        }

        public override bool IsFinished()
        {
            return _shouldChase;
        }

        public override void StartAction()
        {
            
        }

        public override void UpdateAction()
        {
            UpdateT((_spotted) ? 1.4f * _bee.Speed : _bee.Speed);

            float dmag = float.PositiveInfinity;

            if(Skin.CurrentTango != null)
            {
                Vector3 d = _bee.transform.position - Skin.CurrentTango.transform.position;
                dmag = d.magnitude;
            }

            if(!_spotted && dmag < _bee.ChaseRange * 1.5f || _spotted && dmag < _bee.ChaseRange * 1.75f)
            {
                if(!_spotted)
                {
                    _spotted = true;
                    _spottedPos = _bee.transform.position;
                    _spline[0].position = Vector3.zero;
                    _t = 0;

                    _spottedTimer = _bee.TimeUntilChase + (Random.value - .5f) * _bee.TimeUntilChase;
                }

                _bee.transform.position = _spottedPos + .2f * _spline.GetPoint(_t);
                Vector3 target = Skin.CurrentTango.transform.position;
                target.y = _bee.transform.position.y;
                _bee.transform.LookAt(target);

                _spottedTimer -= Time.deltaTime;
                if(_spottedTimer < 0)
                {
                    _shouldChase = true;
                }
            }
            else
            {
                if (_spotted)
                {
                    _spotted = false;
                    _spline[0].position = _bee.transform.position - _hivePos;
                    _t = 0;
                }

                _bee.transform.position = _hivePos + _spline.GetPoint(_t);
                _bee.transform.rotation = Quaternion.LookRotation(_spline.GetTangent(_t), Vector3.up);
            }
        }

        private void UpdateT(float speed)
        {
            _t += speed * Time.deltaTime / 10.0f;
            _t = _t % 1.0f;
        }
    }
}
