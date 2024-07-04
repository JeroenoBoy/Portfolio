namespace Game.Scripts.SplitScreen.PlayerManagers
{
    public class GamePlayerManager : ScenePlayerManager
    {
        [SerializeField] private InputHandler   _handler;
        [SerializeField] private KartController _kartPrefab;
        [SerializeField] private RenderTexture  _texture;
        [SerializeField] private string         _changeScene;
        [SerializeField] private float          _changeSceneDelay;

        public UnityEvent loadedEvent;
        private Dictionary<GameObject, RenderTexture> _textures = new Dictionary<GameObject, RenderTexture>();
        private List<GameObject> _finishedPlayers = new List<GameObject>();


        /**
         * Initiate the manager
         */
        protected override void Start()
        {
            if (PlayerManager.instance == null || PlayerInput.all.Count == 0)
                 JoinSinglePlayer();
            else base.Start();

            loadedEvent.Invoke();
        }


        /**
         * Creates a new kart object and adds a new RenderTexture &
         */
        protected override GameObject OnPlayerJoined(PlayerInput input)
        {
            var obj = Instantiate(_kartPrefab.gameObject, transform.position, transform.rotation);
            var texture = Instantiate(_texture);
            var handler = input.GetComponent<InputHandler>();

            _textures.Add(obj, texture);

            obj.GetComponent<PlayerInterface>().Initiate(texture, handler);
            return obj;
        }


        /**
         * Removes the players from the list
         */
        protected override void OnPlayerLeft(PlayerInput input)
        {
            _textures.Remove(input.gameObject);
            Destroy(GetGameObject(input));
        }


        /**
         * Join with 1 player at a time
         */
        private void JoinSinglePlayer()
        {
            var handler = Instantiate(_handler.gameObject).GetComponent<InputHandler>();
            var input   = handler.GetComponent<PlayerInput>();
            handler.destroyOnLoad = true;

            _players.Add(input, OnPlayerJoined(input));
        }


        /**
         * Detects when a player finished the map
         */
        public void PlayerFinished(GameObject obj)
        {
            _finishedPlayers.Add(obj);
            if (_finishedPlayers.Count >= PlayerInput.all.Count)
            {
                StartCoroutine(newScene());
            }
        }


        /**
         * Coroutine to start loading a new scene
         */
        private IEnumerator newScene()
        {
            yield return new WaitForSeconds(_changeSceneDelay);
            SceneManager.LoadScene(_changeScene);
        }


        /**
         * Get all the textures in the manager
         */
        public IEnumerable<RenderTexture> GetTextures()
        {
            return _textures.Values;
        }


        /**
         * Get all the textures in the manager
         */
        public GameObject GetKey(RenderTexture value)
        {
            return _textures.FirstOrDefault(KVPair => KVPair.Value == value).Key;
        }
    }
}