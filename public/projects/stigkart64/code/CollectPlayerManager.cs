namespace Game.Scripts.SplitScreen.PlayerManagers
{
    public class CollectPlayerManager : ScenePlayerManager
    {
        [SerializeField] private GameObject _playerObject;
        [SerializeField] private KartList   _karts;
        [SerializeField] private Transform  _parent;


        protected override GameObject OnPlayerJoined(PlayerInput input)
        {
            var obj  = Instantiate(_playerObject, _parent);
            var kart = Instantiate(_karts.m_Prefabs[input.playerIndex+1], obj.transform);
            var handler = input.GetComponent<InputHandler>();

            kart.transform.localScale  = Vector3.one * 100;
            kart.transform.position   += Vector3.down * 0.5f;
            kart.transform.rotation    = Quaternion.Euler(0, 180, 0);
            kart.AddComponent<CollectPlayerControls>().Initiate(handler);

            obj.GetComponentInChildren<TMP_Text>().text = $"Player {input.playerIndex + 1}";

            return obj;
        }


        protected override void OnPlayerLeft(PlayerInput obj)
        {
            Destroy(GetGameObject(obj));
        }


        public void SwitchScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}