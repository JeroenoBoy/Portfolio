using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using JUtils.Attributes; // My own utils library



namespace Tools
{
    public class RotateChaotically : MonoBehaviour
    {
        [SerializeField] private RotateOption[] _rotateOptions;

        private bool _isActive;

        private int   _currentIndex;
        private float _currentRotation;

        private float _startRotation;
        private float _targetRotation;
        private float _speed;
        
        private float _timeSpeedMulti;

        public float rotation
        {
            get => _currentRotation;
            set => transform.localEulerAngles = new Vector3(_currentRotation = value,0,0);
        }


        [Button("Activate", true)]
        public void Activate()
        {
            if (_isActive) return;
            _isActive = true;
            _currentIndex = -1;
            _currentRotation = transform.eulerAngles.x;
            _targetRotation = 0;
            _speed = 0;
            _timeSpeedMulti = 0;
            
            Switch(_rotateOptions[0]);
        }


        public void Stop()
        {
            if (!_isActive) return;
            _isActive = false;
        }


        private void Update()
        {
            if (!_isActive) return;

            RotateOption current = _rotateOptions[_currentIndex];

            switch (current.type)
            {
                //  Accelerate towards the target speed
                
                case RotateOption.RotateType.Accelerate:
                    _speed = Mathf.MoveTowards(
                        _speed,
                        current.targetSpeed,
                        Time.deltaTime * _timeSpeedMulti
                    );
                    
                    rotation += _speed * Time.deltaTime; 
                    if (_speed == current.targetSpeed) Switch(current);
                    break;
                
                //  Move at a constant speed for a couple of rotations
                
                case RotateOption.RotateType.Constant:
                    rotation = Mathf.MoveTowards(rotation, _targetRotation, Mathf.Abs(_speed) * Time.deltaTime);
                    if (rotation == _targetRotation) Switch(current);
                    break;
                
                //  Stop at a certain rotation
                
                case RotateOption.RotateType.StopAtRotation:
                    //  Rotating
                    rotation += _speed * Time.deltaTime;
                    
                    //  Changing the speed based on the current angle, This works surprisingly well
                    float multi = Mathf.InverseLerp(_targetRotation, _startRotation, rotation);
                    _speed = _timeSpeedMulti * Mathf.Sqrt(multi);
                    
                    //  Stopping 
                    if (Mathf.Abs(_speed) < .1f)
                    {
                        _speed = 0;
                        rotation = _targetRotation;
                        Switch(current);
                    }
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }


        public void Switch(RotateOption current)
        {
            if (_currentIndex >= 0) current.events.onFinish.Invoke();
            
            //  Checking if finished
            
            _currentIndex++;
            if (_currentIndex >= _rotateOptions.Length)
            {
                Stop();
                return;
            }
            
            //  Setting values for next
            
            current = _rotateOptions[_currentIndex];
            
            switch (current.type)
            {
                case RotateOption.RotateType.Accelerate:
                    _timeSpeedMulti = Mathf.Abs(current.targetSpeed - _speed) * (1/current.time);
                    break;
                
                case RotateOption.RotateType.Constant:
                    _targetRotation = rotation + Mathf.Sign(_speed) * current.rotations * 360;
                    break;
                
                case RotateOption.RotateType.StopAtRotation:
                    
                    //  This will be the rotation where the speed it maxed out
                    _startRotation  = rotation;
                    
                    //  Calculating the target rotation
                    _targetRotation = current.targetRotation * Mathf.Sign(_speed) + (_speed > 0
                        ? Mathf.Ceil(rotation / 360) 
                        : Mathf.Floor(rotation / 360)) * 360;

                    //  This helps with the speed calculation
                    _timeSpeedMulti = _speed;
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
            
            current.events.onStart.Invoke();
        }



        [System.Serializable]
        public struct RotateOption
        {
            public enum RotateType { Accelerate, Constant, StopAtRotation }

            public RotateType type;

            public float rotations;
            public float time;
            public float targetSpeed;
            public float targetRotation;

            public Events events;

            
            
            [System.Serializable]
            public struct Events
            {
                public UnityEvent onStart;
                public UnityEvent onFinish;
            }
            
            
            
            #if UNITY_EDITOR
            [CustomPropertyDrawer(typeof(RotateOption))]
            private class MyClass : PropertyDrawer
            {
                private bool _isExpanded;
                
                public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
                {
                    SerializedProperty typeProp = property.FindPropertyRelative("type");
                    RotateType type = (RotateType)typeProp.enumValueIndex;
                    
                    float height = base.GetPropertyHeight(typeProp, label);

                    switch (type)
                    {
                        case RotateType.Accelerate:
                            height += 4 + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("time"), label);
                            height += 4 + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targetSpeed"), label);
                            break;

                        case RotateType.Constant:
                            height += 4 + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("rotations"), label);
                            break;

                        case RotateType.StopAtRotation:
                            height += 4 + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targetRotation"), label);
                            break;
                        
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    height += 4 + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("events"), label);

                    return height;
                }


                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                    SerializedProperty typeProp = property.FindPropertyRelative("type");
                    RotateType type = (RotateType)typeProp.enumValueIndex;

                    position.height = 20;
                    EditorGUI.PropertyField(position, typeProp);

                    switch (type)
                    {
                        case RotateType.Accelerate:
                            position.y += 22;
                            EditorGUI.PropertyField(position, property.FindPropertyRelative("time"));
                            position.y += 22;
                            EditorGUI.PropertyField(position, property.FindPropertyRelative("targetSpeed"));
                            break;

                        case RotateType.Constant:
                            position.y += 22;
                            EditorGUI.PropertyField(position, property.FindPropertyRelative("rotations"));
                            break;

                        case RotateType.StopAtRotation:
                            position.y += 22;
                            EditorGUI.PropertyField(position, property.FindPropertyRelative("targetRotation"));
                            break;
                        
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    position.y += 22;
                    SerializedProperty events = property.FindPropertyRelative("events");
                    
                    _isExpanded = EditorGUI.PropertyField(position, events, true);
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            #endif
        }
    }
}
