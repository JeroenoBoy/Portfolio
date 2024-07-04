namespace Game.Scripts.SplitScreen
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private bool  _destroyOnLoad = false;
        [SerializeField] private float _deadzone = 0.2f;

        public float   turnAxis     { get; private set; }
        public float   acceleration { get; private set; }
        public Gamepad gamepad      { get; private set; }


        public  bool holdingDrift { get; private set; }
        private bool _pressedDriftThisFrame;
        public  bool driftPressed => holdingDrift && !_pressedDriftThisFrame;
        public  bool driftReleased => !holdingDrift && _pressedDriftThisFrame;
        public  PlayerInput input { get; private set; }


        private bool _holdingPause;
        private bool _isPressingPause;
        public  bool pauseButtonPressed => _holdingPause && !_isPressingPause;


        public bool destroyOnLoad
        {
            get => _destroyOnLoad;
            set => _destroyOnLoad = value;
        }

        private RumbleAmount _rumble;


        /**
         * Initiate the component
         */
        private void Start()
        {
            if(!_destroyOnLoad) DontDestroyOnLoad(gameObject);
            input = GetComponent<PlayerInput>();

            gamepad = Gamepad.all.FirstOrDefault(g => g.deviceId == input.devices.First().deviceId);
        }


        /**
         * Updates the controller rubmle
         */
        private void Update()
        {
            if (_rumble == null || Time.time < _rumble.end) return;

            _rumble = null;
            gamepad?.SetMotorSpeeds(0,0);
        }


        /**
         * Setting the acceleration & turn axis input
         */
        private void OnTurn(InputValue input)
        {
            var value = input.Get<Vector2>();
            turnAxis = Mathf.Abs(value.x) > _deadzone
                ? value.x
                : 0;
        }


        /**
         * Setting the acceleration & turn axis input
         */
        private void OnAccelerate(InputValue input)
        {
            var value = input.Get<Vector2>();
            acceleration = Mathf.Abs(value.y) > _deadzone
                ? value.y
                : 0;
        }


        /**
         * Setting the paused butten input
         */
        private void OnPauseToggle(InputValue input)
        {
            _holdingPause = input.isPressed;
        }


        /**
         * Setting the drift input
         */
        private void OnDrift(InputValue input)
        {
            holdingDrift = input.isPressed;
        }


        private void LateUpdate()
        {
            _pressedDriftThisFrame = holdingDrift;
            _isPressingPause = _holdingPause;
        }


        /**
         * Starts the shaking of the controller
         */
        public void ShakeController(float low, float high, float duration)
        {
            gamepad?.SetMotorSpeeds(low, high);
            _rumble = new RumbleAmount
            {
                low  = low,
                high = high,
                end  = Time.time + duration
            };
        }


        /**
         * Reset controller speed
         */
        private void OnDestroy()
        {
            if(gamepad != null)
                gamepad.SetMotorSpeeds(0,0);
        }
    }



    public class RumbleAmount
    {
        public float end { get; internal set; }
        public float low { get; internal set; }
        public float high { get; internal set; }
    }
}