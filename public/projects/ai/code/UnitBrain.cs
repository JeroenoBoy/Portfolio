using System;
using System.Collections;
using System.Linq;
using Controllers;
using NPC.UnitData;
using NPC.Utility;
using Platoons;
using Spawners;
using UnityEngine;

namespace NPC.Brains
{
    public class UnitBrain : StateController
    {
        #region Animator hashes

        private static readonly int _healthHash     = Animator.StringToHash("Health");
        private static readonly int _stateHash      = Animator.StringToHash("State");
        private static readonly int _distanceHash   = Animator.StringToHash("Distance");
        private static readonly int _platoonHash    = Animator.StringToHash("PlatoonSize");
        private static readonly int _targetHash     = Animator.StringToHash("HasTarget");
        private static readonly int _nearCastleHash = Animator.StringToHash("NearCastle");
        private static readonly int _isDeadHash     = Animator.StringToHash("IsDead");
        private static readonly int _diedHash       = Animator.StringToHash("Died");
        private static readonly int _nearUpgrade    = Animator.StringToHash("NearUpgrade");
        private static readonly int _isUpgraded     = Animator.StringToHash("IsUpgraded");

        #endregion


        public float     upgradeArea;
        public Transform castleTarget;


        public HealthController HealthComponent { get; private set; }
        public Eyes                        Eyes { get; private set; }
        public UnitSettings        UnitSettings { get; private set; }


        private bool              _hasTarget;
        private PlatoonController _platoonController;


        /**
         * Initiate the script
         */
        protected override void Awake()
        {
            HealthComponent    = GetComponent<HealthController>();
            Eyes               = GetComponent<Eyes>();
            UnitSettings       = GetComponent<UnitSettings>();
            _platoonController = GetComponent<PlatoonController>();
            base.Awake();
        }


        /**
         * Mainly to assign platoons
         */
        private void Start() => Bind();


        /**
         * Remove the unit from its platoon
         */
        private void OnDisable()
        {
            Platoon?.RemoveUnit(_platoonController);
            Target = null;
            ResetUnit();
        }


        #region Update


        /**
         * Manage the updates
         */
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            UpdateCastleInRange();
            UpdateNearUpgradeArea();
            if(Eyes.Hits != null) UpdateTarget();
        }


        /**
         * Passes if the users is near the castle to the animator
         */
        private void UpdateCastleInRange()
        {
            Animator.SetBool(_nearCastleHash, (castleTarget.position - transform.position).sqrMagnitude < Settings.castleDistance * Settings.castleDistance);

            if (_hasTarget) return;
            Animator.SetFloat(_distanceHash, (castleTarget.position - transform.position).magnitude);
        }


        /**
         * Updates the target
         */
        private void UpdateTarget()
        {
            var position = transform.position;

            //  Getting the closest unit in the other team

            var closest = Eyes.Hits
                .Where  (t => t.transform.TryGetComponent(out UnitBrain brain) && brain.Team != Team)
                .OrderBy(t => (t.transform.position - position).sqrMagnitude)
                .FirstOrDefault().transform;

            //  Setting new target

            if(!TrySetTarget(closest)) return;

            //  Updating animator values

            Animator.SetFloat(_distanceHash, (Target.position - position).magnitude);
        }


        /**
         * Updates when I get near the upgrade area
         */
        public void UpdateNearUpgradeArea()
        {
            var upPos = UpgradeArea.Instance.transform.position;
            Animator.SetBool(_nearUpgrade, (upPos - transform.position).sqrMagnitude < upgradeArea * upgradeArea);
        }

        #endregion


        #region Custom Events


        /**
         * Gets run when the health changes
         */
        private void HealthChange()
        {
            Animator.SetInteger(_healthHash, HealthComponent.health);
        }


        /**
         * Gets run when the health changes
         */
        private void OnDeath()
        {
            Animator.SetTrigger(_diedHash);
            Animator.SetBool(_isDeadHash, true);

            if (!SpawnManager.managers.TryGetValue(Team, out var manager)) return;
            if ((manager.transform.position - transform.position).sqrMagnitude > manager.PenaltyDistance * manager.PenaltyDistance) return;
            manager.CurrentPenalty++;
        }


        /**
         * Gets run when the UnitState changes
         */
        private void OnStateChange()
        {
            Animator.SetInteger(_stateHash, (int)UnitSettings.state);
        }


        /**
         * Gets fired when the platoon changes
         */
        private void OnPlatoonUpdate()
        {
            Animator.SetInteger(_platoonHash, Platoon.Count);
        }


        /**
         * Gets fired when we found an enemy
         */
        private void OnEnemySpotted(Transform enemy)
        {
            TrySetTarget(enemy.transform);
        }

        #endregion


        #region Helper functions

        /**
         * Try to set the target to the closest
         */
        public bool TrySetTarget(Transform closest)
        {
            var position = transform.position;

            switch (closest != null)
            {

                //  Sets the new target
                case true when !Target:
                    Target = closest;
                    Platoon.SendMessage("OnEnemySpotted", Target);
                    return true;

                //  Sets the target to which one is closest
                case true when Target:
                    var closestDistance = (closest.position - position).sqrMagnitude;
                    var currentDistance =  (Target.position - position).sqrMagnitude;

                    if (closestDistance < currentDistance) Target = closest;
                    return true;

                //  Resets the target when its out of sight
                case false when Target:
                    if (Eyes.CanSee(Target)) return true;
                    Target = null;
                    return false;

                default: return false;
            }
        }


        /**
         * Rebind all settings to the controllers
         */
        public void Bind()
        {
            MovementController.MaxSpeed = UnitSettings.MovementSpeed;
            Eyes.RayLength              = (int)UnitSettings.SightRange;
            HealthComponent.maxHealth   = (int)UnitSettings.Defense;
            HealthComponent.health      = HealthComponent.maxHealth;
            HealthComponent.baseHealth  = UnitSettings.BaseDefence;

            Target = null;

            SendMessage("OnBind", SendMessageOptions.DontRequireReceiver);
        }


        /**
         * Invoke the upgrading variables
         */
        public void Upgrade()
        {
            Animator.SetBool(_isUpgraded, true);
        }


        #endregion


        #region properties


        /**
         * Get my team
         */
        public UnitTeam Team
        {
            get => UnitSettings.team;
            set => UnitSettings.team = value;
        }


        /**
         * Get & set the target
         */
        public override Transform Target
        {
            get
            {
                //  Sets the target to null when the target is destroyed
                if (!base.Target && _hasTarget)
                    Target = null;

                //  Resets the target when its deactivated
                if (_hasTarget && !base.Target.gameObject.activeInHierarchy)
                    Target = null;

                return base.Target ? base.Target : castleTarget;
            }
            set
            {
                //  Say to the animator that I lost my target
                if (_hasTarget && !value)
                    Animator.SetBool(_targetHash, _hasTarget = false);

                //  Say to the animator that I have a target
                else if (!_hasTarget && value)
                    Animator.SetBool(_targetHash, _hasTarget = true);

                base.Target = value;
            }
        }


        /**
         * Get my platoon
         */
        public Platoon Platoon
        {
            get => _platoonController.Platoon;
            set => _platoonController.Platoon = value;
        }

        #endregion
    }
}