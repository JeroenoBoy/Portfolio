using System;
using UnityEngine;
using JUtils.Attributes; // My own utils library



namespace World
{
    public enum DynamicNormalHandler { UP, FORWARD, BACK, RIGHT, LEFT }


    public class ClimbableWall : MonoBehaviour
    {
        [SerializeField] private bool _dynamicNormal;
        [SerializeField] private DynamicNormalHandler _dynamicNormalHandler;

        [Space]
        [SerializeField] private Vector3 _normal;

        public Collider collider { get; private set; }
        public Vector3  normal => _normal;


        private void OnValidate()
        {
            if (_dynamicNormal) FixedUpdate();
        }


        private void Awake()
        {
            collider = GetComponent<Collider>();
        }


        private void FixedUpdate()
        {
            if (!_dynamicNormal) return;

            //  Useful for when the wall needs to rotate, only used in Puzzle 3

            switch (_dynamicNormalHandler)
            {
                case DynamicNormalHandler.UP:
                    SetFromUp();
                    break;

                case DynamicNormalHandler.FORWARD:
                    SetFromForward();
                    break;

                case DynamicNormalHandler.BACK:
                    SetFromDecal();
                    break;

                case DynamicNormalHandler.RIGHT:
                    SetFromRight();
                    break;

                case DynamicNormalHandler.LEFT:
                    SetFromLeft();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        [Button("Set Normal (UP)")]
        private void SetFromUp()
        {
            _dynamicNormalHandler = DynamicNormalHandler.UP;
            _normal = transform.up;
        }


        [Button("Set Normal (FORWARD)")]
        private void SetFromForward()
        {
            _dynamicNormalHandler = DynamicNormalHandler.FORWARD;
            _normal = transform.forward;
        }


        [Button("Set Normal (BACK)")]
        private void SetFromDecal()
        {
            _dynamicNormalHandler = DynamicNormalHandler.BACK;
            _normal = -transform.forward;
        }



        [Button("Set Normal (RIGHT)")]
        private void SetFromRight()
        {
            _dynamicNormalHandler = DynamicNormalHandler.RIGHT;
            _normal = transform.right;
        }



        [Button("Set Normal (LEFT)")]
        private void SetFromLeft()
        {
            _dynamicNormalHandler = DynamicNormalHandler.LEFT;
            _normal = -transform.right;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                transform.position,
                transform.position + Quaternion.FromToRotation(Vector3.up, _normal) * Vector3.up
            );
        }
    }
}
