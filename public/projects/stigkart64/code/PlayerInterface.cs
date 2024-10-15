namespace Game.Scripts.Controls
{
    public class PlayerInterface : MonoBehaviour
    {
        [SerializeField] private GameObject         _ai;
        [SerializeField] private UnityEngine.Camera _camera;

        [Header("Controller rumble")]
        [SerializeField] private float _rumbleLow;
        [SerializeField] private float _rumbleHigh;
        [SerializeField] private float _rumbleDuration;

        private KartController    _controller;
        private CheckpointTracker _cpTracker;
        private SignTracker       _signTracker;
        private InputHandler      _inputHandler;
        private CameraShaker      _cameraShake;
        private UIManager         _uiManager;


        private void Awake()
        {
            _controller  = GetComponent<KartController>();
            _cpTracker   = GetComponent<CheckpointTracker>();
            _signTracker = GetComponent<SignTracker>();
            _cameraShake = _camera.GetComponent<CameraShaker>();
        }


        /**
         * Detecting if the pause button is pressed
         */
        private void Update()
        {
            if (_inputHandler.pauseButtonPressed)
                UIInput.instance.PauseButtonPressed();
        }


        /**
         * Initiates the controller and camera
         */
        public void Initiate(RenderTexture texture, InputHandler handler)
        {
            _inputHandler = handler;

            _camera.targetTexture = texture;
            _controller.input = handler;

            _cpTracker.finishedEvent.AddListener(PlayerFinished);
        }


        /**
         * Initiates the UIManager
         */
        public void InitiateUI(UIManager ui)
        {
            ui.kart    = _controller;
            _uiManager = ui;

            //  Binding events
            _cpTracker  .lapFinishedEvent .AddListener(ui.WhenLapEnd);
            _signTracker.textureFoundEvent.AddListener(ui.turnSignUI.ChangeTexture);
            UIInput.instance.onRaceStart  .AddListener(ui.StartGame);
        }


        /**
         * Initiates the UIManager
         */
        public void RumbleController()
        {
            _inputHandler.ShakeController(_rumbleLow, _rumbleHigh, _rumbleDuration);
            _cameraShake.ShakeOnce(1f, 1f, .3f, 2f);
        }


        /**
         * Executed when a player finished
         */
        private void PlayerFinished(float time)
        {
            var manager = (GamePlayerManager)GamePlayerManager.instance;
            manager.PlayerFinished(gameObject);
            _uiManager.Finish();

            //  Changing the player to an AI

            var ai = Instantiate(_ai, transform.position, transform.rotation);
            var playerCam = _cameraShake.transform.parent.GetComponent<PlayerCameraController>();
            var spawnKart = ai.GetComponentInChildren<SpawnKart>();
            var basicAI   = ai.GetComponent<BasicAi>();
            var cpTracker = ai.GetComponent<CheckpointTracker>();

            spawnKart.Respawn(_inputHandler.input.playerIndex+1);

            playerCam.cube             = spawnKart.transform.parent;
            playerCam.transform.parent = ai.transform;

            cpTracker.currentLap             = _cpTracker.currentLap;
            cpTracker.currentCheckpointIndex = 0;

            //basicAI.m_TargetSpeed = _controller.m_TargetSpeed;
            basicAI.m_StartFrozen = false;
            basicAI.UnFreeze();

            //  Unbinding events
            UIInput.instance.onRaceStart.RemoveListener(_uiManager.StartGame);

            //  Destroying old gameObject
            Destroy(gameObject);
        }
    }
}