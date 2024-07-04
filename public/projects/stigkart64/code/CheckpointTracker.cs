namespace Game.Scripts.Laps
{
    public class CheckpointTracker : MonoBehaviour
    {
        private static int _deathLayer;


        [Header("Events")]
        [SerializeField] private UnityEvent<int>   _checkpointReachedEvent;
        [SerializeField] private UnityEvent<int>   _lapFinishedEvent;
        [SerializeField] private UnityEvent<float> _finishedEvent;

        [Header("Debug")]
        [SerializeField] private int _currentLap = 0;
        [SerializeField] private int _currentCheckpoint = 0;

        private bool _atStartCheckpoint => _currentCheckpoint == 0;
        private bool _isFinished => _currentLap == LapsManager.instance.lapAmount;


        private CharacterController _character;


        private void Start()
        {
            if (_deathLayer == 0) _deathLayer = LayerMask.NameToLayer("Death");
            _character = GetComponent<CharacterController>();
        }


        /**
         * Reset the tracker to its base values
         */
        public void ResetTracker()
        {
            _currentCheckpoint = 0;
            _currentLap = 0;
            startTime = Time.time;
        }


        /**
         * Detects and updates the checkpoint
         */
        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out Checkpoint checkpoint)) return;

            if (checkpoint != nextCheckpoint)
            {
                if(checkpoint != currentCheckpoint) Respawn();
                return;
            }
            _currentCheckpoint = nextCheckpointIndex;
            _checkpointReachedEvent.Invoke(_currentCheckpoint);

            if (!_atStartCheckpoint) return;
            _currentLap++;
            _lapFinishedEvent.Invoke(_currentLap);

            if(!_isFinished) return;
            _finishedEvent.Invoke(elapsedTime);
        }


        /**
         * Detecting death
         */
        private void OnControllerColliderHit(ControllerColliderHit other)
        {
            if(other.gameObject.layer != _deathLayer) return;
            Respawn();
        }


        /**
         * Makes me respawn to the last reached checkpoint
         */
        public void Respawn()
        {
            //  Disabling because else i go back
            _character.enabled = false;

            transform.position = currentCheckpoint.position;
            transform.forward  = currentCheckpoint.forward;
            SendMessage("OnRespawn", SendMessageOptions.DontRequireReceiver);

            //  Re enabling because else things go wrong
            _character.enabled = true;
        }


        #region Getters, setters and utility functions

        /**
         * Get the index of the next checkpoint
         */
        public int currentLap
        {
            get => _currentLap;
            set => _currentLap = value;
        }

        /**
         * Get the index of the current checkpoint
         */
        public int currentCheckpointIndex
        {
            get => _currentCheckpoint;
            set => _currentCheckpoint = value;
        }

        /**
         * Get the time this tracker last reset
         */
        public float startTime { get; private set; }

        /**
         * Get the elapsed time
         */
        public float elapsedTime => Time.time - startTime;

        /**
         * Get the index of the next checkpoint
         */
        public int nextCheckpointIndex
            => (_currentCheckpoint + 1) % LapsManager.instance.totalNotes;

        /**
         * Get the current checkpoint
         */
        public Checkpoint currentCheckpoint
        {
            get => LapsManager.instance.GetNode(_currentCheckpoint);
        }

        /**
         * Get the next checkpoint
         */
        public Checkpoint nextCheckpoint
            => LapsManager.instance.GetNode(nextCheckpointIndex);


        //
        //  Event getters
        //

        public UnityEvent<int>   checkpointReachedEvent => _checkpointReachedEvent;
        public UnityEvent<int>   lapFinishedEvent       => _lapFinishedEvent;
        public UnityEvent<float> finishedEvent          => _finishedEvent;

        #endregion
    }
}