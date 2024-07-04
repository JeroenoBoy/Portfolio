using Signals.Blimp;
using UnityEditor;
using UnityEngine;



namespace Signals
{
    [RequireComponent(typeof(SignalEmitter))]
    public class SignalController : MonoBehaviour
    {
        private static readonly int _activate = Animator.StringToHash("Activate");

        [SerializeField] private float       _cooldown;
        [SerializeField] private SignalBlimp _blimpPrefab;
        [SerializeField] private Animator    _animator;
        [SerializeField] private AudioSource _audio;

        private float _canEmitAt;
        private float _startEmit;

        private SignalEmitter _signalEmitter;


        /// <summary>
        /// Initialize the script
        /// </summary>
        private void Awake()
        {
            _signalEmitter = GetComponent<SignalEmitter>();
        }


        /// <summary>
        /// Start the cooldown
        /// </summary>
        private void Start()
        {
            _canEmitAt = _startEmit = Time.time;
        }


        /// <summary>
        /// Emits the signal based on the input
        /// </summary>
        private void Update()
        {
            if (!Input.GetButton("Jump") || !(Time.time > _canEmitAt)) return;

            EmitSignal();

            _canEmitAt = _cooldown + (_startEmit = Time.time);
        }


        /// <summary>
        /// Emit the signal
        /// </summary>
        private void EmitSignal()
        {
            _animator.SetTrigger(_activate);
            _audio.Play();
            _audio.pitch = Random.Range(0.95f, 1.05f);

            foreach (var signalHit in _signalEmitter.EmitSignal()) {

                if (signalHit.Distance < 10) signalHit.Target.SendMessage("OnPing", SendMessageOptions.DontRequireReceiver);

                var obj = Instantiate(
                    _blimpPrefab.gameObject,
                    transform.position,
                    Quaternion.identity
                );

                obj.GetComponent<SignalBlimp>().SignalHit = signalHit;
            }
        }



#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(Time.time <= 0) return;
            if(Time.time > _canEmitAt) return;

            Handles.color = Color.yellow;

            var p = Mathf.InverseLerp(_canEmitAt, _startEmit, Time.time);
            Handles.DrawWireDisc(transform.position, transform.forward, .5f + p);
        }
#endif
    }
}