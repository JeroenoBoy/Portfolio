using System.Collections.Generic;
using System.Linq;
using JUtils;
using UnityEditor;
using UnityEngine;



namespace Signals
{
    public class SignalSource : SignalEmitter
    {
        [SerializeField] private LayerMask _repeaterLayer;


        /// <summary>
        /// Emits the signal
        /// </summary>
        public override SignalHit[] EmitSignal(Vector2 forward)
        {

            //  Iterating through all the colliders

            var signals = new List<SignalHit>();

            foreach (var hit in CastHits(forward))
            {
                //  Looping thru all the colliders and checking if they are in the repeater layer

                if (hit.Target.HasLayer(_repeaterLayer))
                    signals.AddRange(hit.Target.GetComponent<SignalEmitter>().EmitSignal(hit.Direction)
                        .Select(signal => new SignalHit(signal.Target, transform)));
                else
                    signals.Add(hit);
            }

            return signals.Distinct().ToArray();
        }



#if UNITY_EDITOR
        [CustomEditor(typeof(SignalSource))]
        public class Button : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                GUILayout.Space(10);
                if (!GUILayout.Button("Emit Signal")) return;

                var emitter = target as SignalEmitter;

                foreach (var signalHit in emitter.EmitSignal()) {
                    Debug.Log(signalHit.Point);
                }
            }
        }
#endif
    }
}