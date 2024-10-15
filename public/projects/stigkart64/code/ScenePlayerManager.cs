namespace Game.Scripts.SplitScreen
{
    public abstract class ScenePlayerManager : Singleton<ScenePlayerManager>
    {
        protected readonly Dictionary<PlayerInput, GameObject> _players = new Dictionary<PlayerInput, GameObject>();


        /**
         * Binds events when the scene starts
         */
        protected virtual void Start()
        {
            PlayerInputManager.instance.onPlayerJoined += PlayerJoined;
            PlayerInputManager.instance.onPlayerLeft   += PlayerLeft;
            PlayerInputManager.instance.EnableJoining();

            foreach (var input in PlayerInput.all)
                PlayerJoined(input);
        }


        /**
         * Unbinds all events when the object gets destroyed
         */
        private void OnDestroy()
        {
            if(!PlayerInputManager.instance) return;
            PlayerInputManager.instance.onPlayerJoined -= PlayerJoined;
            PlayerInputManager.instance.onPlayerLeft   -= PlayerLeft;
        }


        /**
         * Adds object to the dictionary when a player joined
         */
        private void PlayerJoined(PlayerInput input)
        {
            _players.Add(input, OnPlayerJoined(input));
        }


        /**
         * Removes the object when the player left
         */
        private void PlayerLeft(PlayerInput input)
        {
            OnPlayerLeft(input);
            _players.Remove(input);
        }


        /**
         * Get the GameObject bound to an input
         */
        protected GameObject GetGameObject(PlayerInput _input)
        {
            return _players[_input];
        }


        /**
         * Get all players in this manager
         */
        public GameObject[] GetObjects()
        {
            return _players.Values.ToArray();
        }


        //  Returning a GameObject so i can add it to the list
        protected abstract GameObject OnPlayerJoined(PlayerInput input);
        protected abstract void OnPlayerLeft(PlayerInput input);
    }
}