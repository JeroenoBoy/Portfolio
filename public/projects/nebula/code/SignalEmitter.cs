using System.Collections.Generic;
using JUtils;
using UnityEditor;
using UnityEngine;



namespace Signals
{
    public class SignalEmitter : MonoBehaviour
    {
        [SerializeField] protected int       Radius;
        [SerializeField] protected int       MaxAngle;
        [SerializeField] protected LayerMask DetectionLayer;

        protected virtual Vector3 Direction => transform.up;
        
        
        //  Pretty much just a clamp
        private void OnValidate()
        {
            Radius   = Mathf.Abs(Radius);
            MaxAngle = Mathf.Clamp(MaxAngle, 0, 360);
        }


        public SignalHit[] EmitSignal() => EmitSignal(Direction);
        public abstract SignalHit[] EmitSignal(Vector2 forward);



        /// <summary>
        /// Get all 
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<SignalHit> CastHits(Vector2 forward)
        {
            //  Collecting all the colliders in the sphere

            var hits = Physics2D.OverlapCircleAll(
                transform.position,
                Radius,
                DetectionLayer
            );

            if (hits.Length == 0) yield break;
            
            //  Setting variables for slight optimization
            
            var center = transform.position;
            var angle    = MaxAngle * .5f;

            //  Looping through all the colliders and checking if they are in the right direction

            for (var i = 0; i < hits.Length; i++) {
                var hit = hits[i].transform;

                if (hit == transform) continue;

                //  Calculating and comparing angle

                var direction = (hit.position - center).normalized;
                var shipAngle = Mathf.Abs(
                    Vector2.SignedAngle(forward, direction)
                );

                if (shipAngle > angle) continue;

                //  Returning SignalHit

                yield return new SignalHit(hit, transform);
            }
        }


#if UNITY_EDITOR
        /// <summary>
        /// Just draws the gizmos, nothing else
        /// </summary>
        private void OnDrawGizmos()
        {
            var forward  = Direction;
            var position = transform.position;
            
            var angle = MaxAngle * .5f;

            var left  = Utils.Quaternion(z: angle) * forward;
            var right = Utils.Quaternion(z: -angle) * forward;

            if (MaxAngle < 360) {
                Gizmos.DrawLine(position, position + left * Radius);
                Gizmos.DrawLine(position, position + right * Radius);
            }

            Handles.DrawWireArc(
                position,
                transform.forward,
                right,
                MaxAngle,
                Radius
            );
        }
#endif
    }



    public struct SignalHit
    {
        public float     Distance;
        public Vector2   Point;
        public Transform Origin;
        public Vector2   Direction;
        public Transform Target;


        public SignalHit(Transform target, Transform origin)
        {
            var targetPos = (Vector2)target.position;
            var originPos = (Vector2)origin.position;
            var direction = targetPos - originPos;

            Distance  = direction.magnitude;
            Point     = originPos;
            Origin    = origin;
            Direction = direction;
            Target    = target;
        }
        
        
        public static bool operator ==(SignalHit a, object b)
        {
            if (b is not SignalHit hit) return false;
            return a.Target == hit.Target;
        }

        
        public static bool operator !=(SignalHit a, object b) =>  !(a == b);
    }
}