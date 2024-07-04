using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JUtils.Extensions; // My utils library
using World;

using Random = UnityEngine.Random;



namespace Player.Abilities
{
    public class WallStick : MonoBehaviour
    {
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private float     _stickDistance = 1f;
        [SerializeField] private float     _wallJumpForce;
        [SerializeField] private float     _sameWallStickCooldown = 1f;

        private bool    _isActive;
        private float   _jumpForce;
        private Vector3 _lastNormal;

        private ClimbableWall _lastStickedWall;
        private float         _lastStickedWallTime;
        private readonly List<ClimbableWall> _colliders = new ();

        private CameraMovement _cameraMovement;
        private PlayerMovement _playerMovement;


        private void Awake()
        {
            _cameraMovement = GetComponent<CameraMovement>();
            _playerMovement = GetComponent<PlayerMovement>();
        }


        private void OnEnable()
        {
            _isActive  = true;
            _jumpForce = _playerMovement.jumpForce;
        }


        private void OnDisable()
        {
            _lastNormal = Vector3.zero;
            _isActive   = false;
            _cameraMovement.rootRotation = Quaternion.identity;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.HasLayer(_wallLayer) || !other.TryGetComponent(out ClimbableWall wall)) return;

            //  Checking if we can stick to a new wall

            if (_lastStickedWallTime + _sameWallStickCooldown > Time.time &&
                wall == _lastStickedWall) return;

            //  Check if the wall is already in the list

            _playerMovement.jumpForce = _wallJumpForce;
            _colliders.Add(wall);
        }


        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out ClimbableWall wall)) return;
            _colliders.Remove(wall);

            //  If there are still walls, don't reset the rotations
            if (_colliders.Count != 0) return;

            //  Setting cooldown
            _lastStickedWall     = wall;
            _lastStickedWallTime = Time.time;

            //  Resetting rotations
            _lastNormal                  = Vector3.zero;
            _playerMovement.jumpForce    = _jumpForce;
            _cameraMovement.rootRotation = Quaternion.identity;
        }


        private void FixedUpdate()
        {
            //  Getting the last wall
            ClimbableWall wall = _colliders.LastOrDefault();
            if (!wall || !_isActive) return;

            //  Getting the delta of the wall and setting the rootRotation to it

            Quaternion delta = Quaternion.FromToRotation(_cameraMovement.rootRotation * Vector3.up, wall.normal);
            _cameraMovement.rootRotation = delta * _cameraMovement.rootRotation;
        }
    }
}
