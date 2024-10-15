namespace NPC
{
    [RequireComponent(typeof(Animator))]
    public abstract class StateController : MonoBehaviour
    {
        [SerializeField] private Transform      _target;
        [SerializeField] private PathController _path;
        [SerializeField] private NPCSettings    _settings;

        [SerializeField] private List<AIBehavior> _behaviours = new List<AIBehavior>();


        #region Getters & Setters

        public MovementController movementController { get; private set; }
        public Animator animator { get; private set; }

        /// <summary>
        /// Get the target of this controller
        /// </summary>
        public virtual Transform target
        {
            get => _target;
            set => _target = value;
        }

        /// <summary>
        /// Get the path of this controller
        /// </summary>
        public PathController path
        {
            get => _path;
            set => _path = value;
        }

        /// <summary>
        /// Get the settings of this controller
        /// </summary>
        public NPCSettings settings => _settings;

        /// <summary>
        /// Get the current behaviours
        /// </summary>
        public List<AIBehavior> behaviours => _behaviours;

        #endregion


        private void Awake()
        {
            movementController = GetComponent<MovementController>();
            animator = GetComponent<Animator>();
        }


        /// <summary>
        /// Add a behaviour & start a behaviour
        /// </summary>
        public void AddBehaviour(AIBehavior behavior)
        {
            _behaviours.Add(behavior);
        }


        /// <summary>
        /// Remove a behaviour
        /// </summary>
        public void RemoveBehaviour(AIBehavior behavior)
        {
            if(!_behaviours.Remove(behavior))
                Debug.LogError("Failed to remove behaviour");
        }


        /// <summary>
        /// Send PhysicsUpdates to all behaviors
        /// </summary>
        protected virtual void FixedUpdate()
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.PhysicsUpdate();
            }
        }


        /// <summary>
        /// Execute the OnDrawGizmos in the behaviours
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.OnDrawGizmos();
            }
        }
    }
}