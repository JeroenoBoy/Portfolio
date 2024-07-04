using UnityEngine;



namespace Parallax
{
    public class RepeatingParallax : MonoBehaviour
    {
        [SerializeField] private float   _multi    = 0.4f;
        [SerializeField] private int     _gridSize = 3;
        [SerializeField] private Vector2 _offsetMulti;

        [Header("Debug")]
        [SerializeField] private Vector2 _offset;

        private Transform _target;
        private Vector2   _targetSize;


        private void Start()
        {
            var cam = Camera.main;

            Debug.Assert(cam != null, nameof(cam) + " != null");

            _target = cam.transform;
            _offset = _target.position - transform.position;

            _targetSize = cam.sensorSize * _gridSize * .5f;

            _offsetMulti *= _gridSize;
        }


        private void Update()
        {
            var pos = transform.position = (Vector2)_target.position * _multi - _offset;

            if      (pos.x > _xMax) _offset.x += _offsetMulti.x;
            else if (pos.x < _xMin) _offset.x -= _offsetMulti.x;
            else if (pos.y > _yMax) _offset.y += _offsetMulti.y;
            else if (pos.y < _yMin) _offset.y -= _offsetMulti.y;
        }


        private float _xMin => _target.position.x - _targetSize.x;
        private float _xMax => _target.position.x + _targetSize.x;
        private float _yMin => _target.position.y - _targetSize.y;
        private float _yMax => _target.position.y + _targetSize.y;
    }
}