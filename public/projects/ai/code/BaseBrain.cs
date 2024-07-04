namespace NPC.Brains
{
    public class BaseBrain : StateController
    {
        private static readonly int _hasTargetHash      = Animator.StringToHash("hasTarget");
        private static readonly int _enemyFound         = Animator.StringToHash("enemyFound");
        private static readonly int _distanceFromTarget = Animator.StringToHash("distance");

        [SerializeField] private string _enemyMask;

        private int  _enemyLayer;
        private bool _hasTarget;
        protected Eyes eyes { get; private set; }


        public override Transform target
        {
            get
            {
                if (!base.target && _hasTarget) target = null;
                return base.target;
            }
            set
            {
                if (!base.target && value)
                {
                    animator.SetBool(_hasTargetHash, true);
                    animator.SetTrigger(_enemyFound);
                }

                if (!value && base.target)
                {
                    animator.SetBool(_hasTargetHash, false);
                    animator.ResetTrigger(_enemyFound);
                }

                base.target = value;
            }
        }


        /// <summary>
        /// Initializes the script
        /// </summary>
        protected virtual void Start()
        {
            _enemyLayer = LayerMask.GetMask(_enemyMask);
            eyes = GetComponent<Eyes>();
        }


        /// <summary>
        /// Mainly checks if we can see targets
        /// </summary>
        protected override void FixedUpdate()
        {
            var targets = eyes.FindTargets().Distinct().ToArray();
            OtherUpdate(targets);
            base.FixedUpdate();
        }


        /// <summary>
        /// Extra update meant to be overridden, here it sets the new target
        /// </summary>
        protected virtual void OtherUpdate(Transform[] targets)
        {
            var newTarget = FindClosest(targets, _enemyLayer);
            if(newTarget) target = newTarget;

            //  Updating behaviours & returning if target is not set

            if(!target) return;

            //  Checking if we can see the target

            if (!eyes.CanSee(target)) LostTarget();
            else animator.SetFloat(_distanceFromTarget, (transform.position - target.position).FastMag());
        }


        /// <summary>
        /// Find the closest target in a layer
        /// </summary>
        protected Transform FindClosest(Transform[] targets, int layer)
        {
            var position  = transform.position;
            var sqrDistance = float.MaxValue;
            Transform closest = null;

            //  Looping thru all targets & finding closest

            foreach (var target in targets)
            {
                //  Comparing layer in here since I don't want to see targets behind walls
                if (((1 << target.gameObject.layer) & layer) == 0)
                    if (layer != target.gameObject.layer) continue;

                //  Checking distance

                var distance = (position - target.position).sqrMagnitude;
                if(distance > sqrDistance) continue;

                //  Settings closest

                closest = target;
                sqrDistance = distance;
            }

            return closest;
        }


        /// <summary>
        /// Resets the state to seeking
        /// </summary>
        private void LostTarget()
        {
            target = null;
        }
    }
}