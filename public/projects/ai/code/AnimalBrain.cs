namespace NPC.Brains
{
    [RequireComponent(typeof(HungerController), typeof(HealthController))]
    public class AnimalBrain : BaseBrain, IEatable
    {
        private static readonly int _hunger   = Animator.StringToHash("hunger");
        private static readonly int _seesFood = Animator.StringToHash("seesFood");
        private static readonly int _distance = Animator.StringToHash("distance");
        private static readonly int _died     = Animator.StringToHash("died");

        [Header("Hunger options")]
        [SerializeField] private string _foodLayer;

        [Header("Healing options")]
        [SerializeField] private int   _startHealingHungerAbove;
        [SerializeField] private float _healingInterval;
        [SerializeField] private int   _healAmount;
        [SerializeField] private float _minMaxAge;
        [SerializeField] private float _maxMaxAge;

        [Header("Nutrition options")]
        [SerializeField] private float _totalNutrition;

        private int   _foodLayerBit;
        private bool  _seedFood;
        private float _healAt;
        private float _dieAt, _bornAt;

        private Transform        _closestFood;
        public  HealthController health { get; private set; }
        public  HungerController hunger { get; private set; }
        private AudioSource      _audioPlayer;
        private static readonly int _age = Animator.StringToHash("age");


        #region properties


        public Transform closestFood
        {
            get
            {
                if (!_closestFood && _seedFood)
                    animator.SetBool(_seesFood, _seedFood = false);
                return _closestFood;
            }
            set
            {
                if (!_seedFood && value) animator.SetBool(_seesFood, _seedFood = value);
                if (_seedFood && !value) animator.SetBool(_seesFood, _seedFood = value);
                _closestFood = value;
            }
        }


        public override Transform target => base.target ? base.target : _closestFood;


        #endregion


        /// <summary>
        /// Initialize the component
        /// </summary>
        protected override void Start()
        {
            _audioPlayer = GetComponent<AudioSource>();
            base.Start();

            _foodLayerBit = LayerMask.NameToLayer(_foodLayer);

            health = GetComponent<HealthController>();
            hunger = GetComponent<HungerController>();

            _dieAt = (_bornAt = Time.time) + Random.Range(_minMaxAge, _maxMaxAge);
        }


        /// <summary>
        /// Check if we see food
        /// </summary>
        protected override void OtherUpdate(Transform[] targets)
        {
            base.OtherUpdate(targets);

            FindFood(targets);
            UpdateTarget();
            UpdateHealing();
            UpdateAging();
        }


        /// <summary>
        /// Finds the nearest food and set it to our food target
        /// </summary>
        private void FindFood(Transform[] targets)
        {
            var targetFood = FindClosest(targets, _foodLayerBit);
            if (!targetFood) return;

            //  Checking if the new target food is closer

            var position = transform.position;
            bool isCloser() => (closestFood.position - position).sqrMagnitude < (targetFood.position - position).sqrMagnitude;
            if(closestFood && isCloser()) return;

            //  Settings closest food to target food

            closestFood = targetFood;
        }


        /// <summary>
        /// Checks if the current target exists & if i can see it
        /// </summary>
        private void UpdateTarget()
        {
            if (!closestFood) return;
            if (!eyes.CanSee(_closestFood)) closestFood = null;
            else animator.SetFloat(_distance, (_closestFood.position - transform.position).FastMag());
        }


        /// <summary>
        /// Update the hunger regeneration
        /// </summary>
        private void UpdateHealing()
        {
            if(hunger.hunger < _startHealingHungerAbove) return;
            if(_healAt > Time.time) return;

            _healAt = Time.time + _healingInterval;
            health.Heal(_healAmount);
        }


        /// <summary>
        /// Manages the aging
        /// </summary>
        private void UpdateAging()
        {
            animator.SetFloat(_age, Time.time - _bornAt);

            if(Time.time < _dieAt) return;
            health.Damage(health.health);
        }


        /// <summary>
        /// Detects when hunger changes
        /// </summary>
        private void OnHungerChange()
        {
            animator.SetFloat(_hunger, hunger.hunger);
        }


        /// <summary>
        /// Sets the death state in the animator
        /// </summary>
        private void OnDeath()
        {
            movementController.velocity = Vector3.zero;
            animator.SetBool(_died, true);
            _audioPlayer.Play();
            Destroy(gameObject, 1f);
        }


        /// <summary>
        /// Take a bite out of me
        /// </summary>
        public float Bite(int biteStrength)
        {
            float damaged = health.Damage(biteStrength);
            return damaged / health.maxHealth * _totalNutrition;
        }


        /// <summary>
        /// Check if i'm dead for the IEatable interface
        /// </summary>
        public bool Eaten()
        {
            return health.isDead;
        }
    }
}