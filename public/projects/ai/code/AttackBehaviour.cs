using Controllers;
using NPC.UnitData;
using NPC.Utility;
using UnityEngine;
using Util;

namespace NPC.Behaviours.Unit
{
    public class AttackBehaviour : UnitBehaviour
    {
        private float _nextStrike;


        protected override void Enter()
        {
            animator.speed = stateInfo.length * (1/unitSettings.AttackSpeed);
            _nextStrike    = Time.time + unitSettings.AttackSpeed*.5f;
        }


        public override void PhysicsUpdate()
        {
            if(Time.time < _nextStrike) return;
            _nextStrike += unitSettings.AttackSpeed;

            //  Actually dealing the damage

            if (target
                && target.TryGetComponent(out HealthController health)
                && target.TryGetComponent(out Collider col)
                && col.Raycast(transform.ForwardRay(), out var info, unitSettings.SightRange))
                health.Damage((int)unitSettings.AttackDamage, info.point);
        }


        protected override void Exit()
        {
            animator.speed = 1;
        }
    }
}