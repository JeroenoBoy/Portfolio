using System;
using JUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using JUtils.Attributes; // My utils library
using JUtils.Extensions;



namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField] private JumpCheck _groundCheck;
        [Range(0, 1)]
        [SerializeField] private float _deadZone = 0.1f;
        [SerializeField] public AudioSource walkAudio;
        [SerializeField] private Transform _cameraPivot;

        [field: Header("Jump")]
        [field: SerializeField] public bool enableJump { get; set; } = true;
        [field: SerializeField] public float jumpForce { get; set; } = 5f;

        [field: Header("Speed")]
        [field: SerializeField] public float speed              { get; set; } = 5f;
        [field: SerializeField] public float airSpeed           { get; set; } = 5f;
        [field: SerializeField] public float gravityMultiplier  { get; set; }
        [field: SerializeField] public float maxSpeed           { get; set; } = 10f;

        [field: Header("Gravity")]
        [field: SerializeField] public bool useGravity             { get; set;  }= true;
        [field: SerializeField] public float gravity               { get; set; } = 9.81f;
        [field: SerializeField] public float downwardsGravity      { get; set; } = 9.81f * 1.5f;
        [field: SerializeField] public float groundedGravityMulti  { get; set; } = 100f;
        [field: SerializeField] public LayerMask groundLayer       { get; set; }

        [field: Header("Drag")]
        [field: SerializeField] public bool slide         { get; set; }
        [field: SerializeField] public float defaultDrag  { get; set; } = 2f;
        [field: SerializeField] public float stopDrag     { get; set; } = 4f;
        [field: SerializeField] public float airDragMulti { get; set; } = 0.5f;

        [Space, Space]
        [SerializeField] private UnityEvent _onJumpFromGroundHuman;
        [SerializeField] private UnityEvent _onJumpFromGroundSlime;
        [SerializeField] private UnityEvent _stopWalkSound;

        //  Input
        [HideInInspector]
        public Vector3  movementDir;
        private bool   _isSprinting;
        private float  _gravity;
        private float  _lastJumped;

        //  Velocity
        private Vector3 _velocity;
        private Vector3 _acceleration;
        private float _jumpVelocity;

        //  Components
        private CharacterController _controller;
        private CameraMovement _cameraMovement;
        private StateController _playerState;


        #region Properties

        public bool isMoving => movementDir != Vector3.zero;
        public float speedMultiplier { get; set; } = 1f;
        public Vector3 velocity
        {
            get => enabled ? _velocity : Vector3.zero;
            set => _velocity = value;
        }

        #endregion


        private void OnEnable()
        {
            _controller.enabled = true;
        }


        private void Awake()
        {
            _controller     = GetComponent<CharacterController>();
            _cameraMovement = GetComponent<CameraMovement>();
            _playerState    = GetComponent<StateController>();
        }


        private void OnDisable()
        {
            _controller.enabled = false;
            if (walkAudio.isPlaying) walkAudio.Stop();
        }


        #region Input


        private void OnMove(InputValue value)
        {
            Vector2 input = value.Get<Vector2>();
            movementDir = input.sqrMagnitude < _deadZone * _deadZone
                ? Vector3.zero
                : input.ToXZVector3();
        }


        private void OnJump()
        {
            if (!enableJump || !_groundCheck.canJump) return;
            if (_lastJumped + 0.2f > Time.time) return;
            _lastJumped = Time.time;
            velocity += _cameraPivot.up * jumpForce;

            if (_playerState.ReturnState() == AnimalType.SLIME)
                _onJumpFromGroundSlime.Invoke();
            else if (_playerState.ReturnState() == AnimalType.HUMAN)
                _onJumpFromGroundHuman.Invoke();

            _stopWalkSound.Invoke();
        }


        private void OnSprint(InputValue value)
        {
            _isSprinting = value.isPressed;
        }


        #endregion


        private void FixedUpdate()
        {
            Vector3 current = velocity;

            //  Getting rotations for wall stick

            Quaternion rotation = _cameraPivot.rotation;
            Quaternion inverseRotation = Quaternion.Inverse(rotation);

            //  Updating the movement

            UpdateMovement(ref current, rotation, inverseRotation);
            UpdateGravity(ref current);

            //  Applying velocity

            current *= 1 - 1 / (defaultDrag * (_groundCheck.isColliding ? 1 : airDragMulti));

            //  Calculating force angles, Splitting the forces to fix issues with the controller

            Vector3 vector = inverseRotation * current;
            Vector3 xz     = rotation * vector.With(y: 0);
            Vector3 y      = rotation * vector.With(x: 0, z: 0);

            Vector3 pos = transform.position;

            //  Moving

            _controller.Move(xz * Time.fixedDeltaTime);
            _controller.Move(y * Time.fixedDeltaTime);

            //  Calculating velocity

            velocity = (transform.position - pos) / Time.fixedDeltaTime;
        }


        /// <summary>
        /// Updates the movement of the player
        /// </summary>
        private void UpdateMovement(ref Vector3 current, Quaternion rotation, Quaternion inverseRotation)
        {
            //  Getting the desired speed

            float targetSpeed = _groundCheck.isColliding ? speed : airSpeed;

            //  Stopping

            if (movementDir == Vector3.zero || current.sqrMagnitude > maxSpeed * maxSpeed)
            {
                //  Applying extra drag on xz movement
                current += rotation * ((inverseRotation * -current).With(y: 0) * 1 / stopDrag);

                if (walkAudio.isPlaying) walkAudio.Stop();
            }

            //  Moving

            else
            {
                Vector3 rotatedMovement = rotation * movementDir;

                current += rotatedMovement * (targetSpeed * Time.fixedDeltaTime);

                //  Playing audio when isn't playing

                if(!walkAudio.isPlaying)
                    walkAudio.Play();

                if (slide) return;

                //  Remove sliding

                Vector3 rotatedCurrent = inverseRotation * -current;

                //  These quaternions help with only removing the X velocity
                Quaternion toMD   = Quaternion.FromToRotation(Vector3.forward, movementDir);
                Quaternion fromMD = Quaternion.FromToRotation(movementDir, Vector3.forward);

                //  Removing the X velocity
                Vector3 wow = (fromMD * rotatedCurrent).With(y: 0, z: 0);
                current += rotation * (toMD * wow) * (1 / stopDrag);
            }
        }


        /// <summary>
        /// Updates the gravity of the player
        /// </summary>
        private void UpdateGravity(ref Vector3 current)
        {
            if (!useGravity) return;

            Transform t = transform;
            bool isGrounded = Physics.Raycast(t.position, -t.up, .1f, groundLayer);

            float tGravity = velocity.y > _jumpVelocity * .2f ? gravity : downwardsGravity;
            if (!slide && isGrounded && _lastJumped + 0.2f < Time.time) tGravity *= groundedGravityMulti;

            current += _cameraMovement.rootRotation * Vector3.down * (tGravity * Time.fixedDeltaTime);
        }
    }
}