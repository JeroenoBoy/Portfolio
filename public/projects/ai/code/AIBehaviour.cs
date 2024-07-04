namespace NPC
{
    public abstract class AIBehavior : StateMachineBehaviour
    {
        protected bool started { get; private set; }


        #region Usefull properties for the behaviour

        protected StateController stateController { get; private set; }
        protected Animator        animator        { get; private set; }

        protected Transform target            => stateController.target;
        protected Transform transform         => stateController.transform;
        protected NPCSettings settings        => stateController.settings;
        protected MovementController movement => stateController.movementController;

        #endregion


        //
        // These functions manage the state of this behaviour
        //


        public override void OnStateEnter(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!started) Init(anim);

            stateController.AddBehaviour(this);
;           Enter();
        }


        public override void OnStateExit(Animator anim, AnimatorStateInfo stateInfo, int stateMachinePathHash)
        {
            Exit();
            stateController.RemoveBehaviour(this);
        }


        /// <summary>
        /// Assigns the correct controller to this behaviour
        /// </summary>
        protected void Init(Animator anim)
        {
            started = true;
            stateController = anim.GetComponent<StateController>();
            animator = anim;

            if(!stateController) Debug.LogError("Animator does not have a StateController!");

            Start();
        }


        /// <summary>
        /// Get a component on the controller
        /// </summary>
        public T GetComponent<T>() => stateController.GetComponent<T>();


        #region Movement Utils

        /// <summary>
        /// Calculates the seek direction based on minSeekDistance
        /// </summary>
        protected Vector3 GetDirection(Vector3 direction, float speed = -1)
        {
            speed = speed < 0 ? movement.maxSpeed : speed;
            direction = direction.With(y: 0);

            //  Checking if the player is close enough to the target

            var distance = direction.sqrMagnitude;
            return distance < settings.minSeekDistance * settings.minSeekDistance
                ? Vector3.zero
                : direction.ClampMagnitude(speed);
        }


        /// <summary>
        /// Move to a position with the arrive feature
        /// </summary>
        protected Vector3 MoveWithArrive(Vector3 targetPosition, float targetSpeed = -1)
        {
            targetSpeed = targetSpeed < 0 ? movement.maxSpeed : targetSpeed;

            var position = transform.position;

            //  Checking if the player is in stopping distance

            var direction = (targetPosition - position).With(y: 0);
            var distance = direction.FastMag();

            if(distance < settings.stopDistance)
                return Vector3.zero;

            //  Calculating the force based on slowing distance

            var slowDistance   = settings.slowDistance + settings.stopDistance;
            var currentSpeed = movement.velocity;//.magnitude;

            // var percentageDistance = distance / (slowDistance * slowDistance);
            // var desiredSpeed       = percentageDistance * targetSpeed;
            // var desiredVel       = desiredSpeed - currentSpeed;
            //
            // return direction.normalized * Mathf.Clamp(desiredVel , 0, targetSpeed);

            var percentageDistance = distance / slowDistance;
            var desiredSpeed       = percentageDistance * targetSpeed;
            var desiredVel       = direction.normalized * desiredSpeed;

            return (desiredVel - currentSpeed).ClampMagnitude(targetSpeed);
        }

        #endregion




        //
        //  Events
        //


        protected virtual void Start() {}
        protected virtual void Enter() {}
        public virtual void PhysicsUpdate() {}
        public virtual void OnDrawGizmos() {}
        protected virtual void Exit() {}
    }
}