using System;
using UnityEngine;



namespace Parallax
{
    public class StationaryParallax: MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float     _multi = 0.4f;

        [Header("Debug")]
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _originalPos;
        [SerializeField] private Vector2 _centerPos;


        private void OnValidate()
        {
            if(_multi is > 1 or < -1)
                _multi = Mathf.Clamp(_multi, -1, 1);
        }


        private void Start()
        {
            if(!_target) _target = Camera.main.transform;

            _originalPos = transform.position;
            _offset      = _originalPos - (Vector2)_target.position;
            _centerPos   = _originalPos - _offset * _multi;
        }


        private void Update()
        {
            transform.position = (Vector2)_target.position * _multi + _centerPos;
        }


        /**
         *  Not very efficient but its only in the editor, so I
         *  don't really mind for now
         */
        private void OnDrawGizmosSelected()
        {
            if(!Application.isPlaying) Start();

            var bounds = GetComponent<SpriteRenderer>().bounds;

            Gizmos.DrawWireSphere(_originalPos, bounds.size.x*.5f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_centerPos, bounds.size.x*.5f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_originalPos, bounds.size.x / (1-_multi) / 1.5f);
        }
    }
}