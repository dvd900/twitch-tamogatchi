
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
        private int _splineInd;

        private Vector3 _spottedPos;
        private Vector3 _hivePos;
        private bool _spotted;
        private bool _shouldChase;
        private float _spottedTimer;

        private float _speedMod;

        public BeeIdleAction(BeeController bee, BezierSpline spline)
        {
            _bee = bee;
            _spline = spline;
            _hivePos = bee.Hive.transform.position;

            InitSpline(_hivePos, 1);
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
                    _t = 0;

                    InitSpline(_spottedPos, .2f);

                    _spottedTimer = _bee.TimeUntilChase + (Random.value - .5f) * _bee.TimeUntilChase;
                }

                _bee.transform.position = _spline.GetPoint(_t);
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
                    _t = 0;
                    _splineInd = 0;

                    InitSpline(_hivePos, 1.0f);
                }

                _bee.transform.position = _spline.GetPoint(_t);
                _bee.transform.rotation = Quaternion.LookRotation(_spline.GetTangent(_t), Vector3.up);

                if(_bee.Hive != null)
                {
                    _hivePos = _bee.Hive.transform.position;
                }

                int newInd = CurrSplineInd();
                if(newInd != _splineInd)
                {
                    _splineInd = newInd;
                    int indToChange = (_splineInd + 2) % NUM_POINTS;
                    SetPointPos(indToChange, _hivePos, 1.0f);
                }
            }
        }

        private void UpdateT(float speed)
        {
            _t += speed * Time.deltaTime / 10.0f;
            _t = _t % 1.0f;
        }

        private void InitSpline(Vector3 center, float rangeMod)
        {
            _spline.Initialize(NUM_POINTS);

            _spline[0].position = _bee.transform.position;
            for (int i = 1; i < NUM_POINTS; i++)
            {
                SetPointPos(i, center, rangeMod);
            }

            _spline.AutoConstructSpline();
        }

        private void SetPointPos(int ind, Vector3 center, float rangeMod)
        {
            var pos = rangeMod * SWARM_RANGE * Random.insideUnitSphere;
            pos.y *= .5f;
            _spline[ind].position = center + pos + 7 * Vector3.up;
        }

        private int CurrSplineInd()
        {
            return (int)(_t * NUM_POINTS);
        }
    }
}
