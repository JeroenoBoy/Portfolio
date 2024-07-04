using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using JUtils.Attributes; // My own utils library
using JUtils.Singletons;



namespace Player
{
    public class StateController : Singleton<StateController>
    {
        [SerializeField, Required] private VisualAnimalSwitchScript _vfxScript;
        [SerializeField, Required] private PlayerMovement _movement;
        [SerializeField, Required] private CameraMovement _cam;

        [field: SerializeField]
        public bool canSwitch { get; set; } = true;

        [Space]
        [SerializeField] private AnimalType _currentAnimal;
        [SerializeField] private State[] _states;
        [Space]
        [SerializeField] private UnityEvent _onStateChanged;

        public AnimalType currentAnimal => _currentAnimal;



        private void OnValidate()
        {
            _states[0].type = AnimalType.HUMAN;
            _states[1].type = AnimalType.SLIME;
        }


        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.5f);
            foreach (State state in _states)
                state.Disable();

            GetState(_currentAnimal).Enable(_movement);
        }


        //  Input event functions
        #region Inputs


        private void OnNextAnimal()
        {
            if (!enabled || _cam.lockCamera || !canSwitch) return;
            SwitchTo(_currentAnimal.Next());
        }


        private void OnPreviousAnimal()
        {
            if (!enabled || _cam.lockCamera || !canSwitch) return;
            SwitchTo(_currentAnimal.Previous());
        }


        private void OnFirstAnimal()
        {
            if (!enabled || _cam.lockCamera || !canSwitch) return;
            SwitchTo(AnimalType.HUMAN);
        }


        private void OnSecondAnimal()
        {
            if (!enabled || _cam.lockCamera || !canSwitch) return;
            SwitchTo(AnimalType.SLIME);
        }


        #endregion


        // State GetState(AnimalType)
        // void SwitchTo(AnimalType)
        #region Util


        private State GetState(AnimalType target) => target switch
        {
            AnimalType.HUMAN => _states[0],
            AnimalType.SLIME => _states[1],
            _ => throw new Exception("Invalid animal type")
        };


        public void SwitchTo(int index) => SwitchTo((AnimalType)index);


        /// <summary>
        /// Switch to the specified animal type.
        /// </summary>
        public void SwitchTo(AnimalType nextState)
        {
            if (_currentAnimal == nextState) return;

            //  Get the states from their enum values
            State current = GetState(_currentAnimal);
            State next    = GetState(nextState);

            //  Change the current states
            current.Disable();
            next.Enable(_movement);

            //  Changing the VFX (Height, Post Processing, etc.)
            _vfxScript.ChangeVisualEffect((int)nextState);
            _currentAnimal = nextState;

            //  Call the event
            _onStateChanged.Invoke();
        }


        public void SetScale(Vector3 target, Transform transform)
            => StartCoroutine(ScaleTransition(target, transform));


        /// <summary>
        /// Scales the transform to the target scale during the duration.
        /// </summary>
        private IEnumerator ScaleTransition(Vector3 target, Transform transform, float animationSpeed = .5f)
        {
            float speed = 1/animationSpeed;
            Vector3 start = transform.localScale;

            for (float i = 0; i < 1; i += Time.deltaTime * speed)
            {
                transform.localScale = Vector3.Lerp(start, target, i);
                yield return null;
            }

            transform.localScale = target;
        }


        public void SetSwitch(bool value) => canSwitch = value;


        #endregion


        [Obsolete("Use StateController.currentAnimal instead")]
        public AnimalType ReturnState() => _currentAnimal;
    }



    [System.Serializable]
    public struct State
    {
        public AnimalType type;
        [SerializeField] private Behaviour[]     _behaviours;
        [SerializeField] private Events          _events;
        [SerializeField] private MovementOptions _movement;


        /// <summary>
        /// Enable the state
        /// </summary>
        public void Enable(PlayerMovement movement)
        {
            foreach (Behaviour behaviour in _behaviours)
                behaviour.enabled = true;

            _events.onEnable.Invoke();
            _movement.Apply(movement);
        }


        /// <summary>
        /// Disable the state
        /// </summary>
        public void Disable()
        {
            foreach (Behaviour behaviour in _behaviours)
                behaviour.enabled = false;

            _events.onDisable.Invoke();
        }


        [System.Serializable]
        private struct Events
        {
            public UnityEvent onEnable;
            public UnityEvent onDisable;
        }


        [System.Serializable]
        private struct MovementOptions
        {
            [Header("Jump")]
            public bool  jumpEnabled;
            public float jumpStrength;
            public float gravityMultiplier;

            [Header("Movement")]
            public float movementSpeed;
            public float airSpeed;

            [Header("Drag")]
            public bool  slide;
            public float defaultDrag;
            public float airDragMulti;
            public float stopDrag;

            [Header("Collider")]
            public float colliderOffset;
            public float colliderWidth;
            public float colliderHeight;

            [Header("HeadBob")]
            public HeadBob.Calculation calculation;
            public float headBobSpeed;
            public float headBobIntensity;
            public float minHeadBobSpeed;
            public float maxHeadBobSpeed;

            [Header("Audio")]
            public AudioClip _soundClip;


            /// <summary>
            /// Apply the movement options to the given movement script
            /// </summary>
            public void Apply(PlayerMovement movement)
            {
                //  Jump
                movement.enableJump        = jumpEnabled;
                movement.jumpForce         = jumpStrength;
                movement.gravityMultiplier = gravityMultiplier;

                //  Speed
                movement.speed            = movementSpeed;
                movement.airSpeed         = airSpeed;

                //  Drag
                movement.slide        = slide;
                movement.defaultDrag  = defaultDrag;
                movement.stopDrag     = stopDrag;
                movement.airDragMulti = airDragMulti;

                //  Walk sound
                movement.GetComponent<AudioSource>().clip = _soundClip;

                //  Collider
                CharacterController controller = movement.GetComponent<CharacterController>();
                controller.center = new Vector3(0, colliderOffset, 0);
                controller.height = colliderHeight;
                controller.radius = colliderWidth;

                //  HeadBob
                HeadBob headBob = movement.GetComponentInChildren<HeadBob>();
                headBob.calculation = calculation;
                headBob.speed       = headBobSpeed;
                headBob.intensity   = headBobIntensity;
                headBob.minSpeed    = minHeadBobSpeed;
                headBob.maxSpeed    = maxHeadBobSpeed;
            }
        }
    }
}
