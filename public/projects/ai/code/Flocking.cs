namespace NPC.Behaviours.Avoidance
{
    public class Flocking : PermanentBehavior
    {
        private const float minForce = 0.8f;


        public override void PhysicsUpdate()
        {
            var targets = FindTargets(settings.flockCohesionDistance);

            //  Filtering targets

            var center = transform.position;
            var sqrDist  = settings.flockSeparationDistance * settings.flockSeparationDistance;
            var separationTargets = targets.Where(target => (target.position - center).sqrMagnitude < sqrDist).ToArray();

            //  Calculating forces

            var cohesionForce   = CalculateForce(targets,           settings.flockCohesionDistance,    settings.flockCohesionMaxForce);
            var separationForce = CalculateForce(separationTargets, settings.flockSeparationDistance, -settings.flockSeparationMaxForce);

            //  Checking if min force is smaller than a certain value else return force

            var force = cohesionForce + separationForce;

            movement.AddForce(force.sqrMagnitude < minForce * minForce
                ? Vector3.zero
                : force);
        }


        private Vector3 CalculateForce(Transform[] targets,float distance, float maxForce)
        {
            var transform = this.transform;

            var center = transform.position;

            //  Calculate the force based on distance

            Vector3 Calculate(Transform target)
            {
                if (target == transform) return Vector3.zero;

                var direction = (target.position - center).With(y: 0);

                var vecDistance     = direction.FastMag();
                var percentageForce = vecDistance / distance;

                return (1- percentageForce) * maxForce * direction.normalized;
            }

            var vector = targets.Aggregate(Vector3.zero, (vector, transform) => vector + Calculate(transform));

            //  Returning the force

            return Vector3.ClampMagnitude(vector, Mathf.Abs(maxForce));
        }


        /// <summary>
        /// Find targets near the player
        /// </summary>
        private Transform[] FindTargets(float distance)
        {
            return Physics.OverlapSphere(transform.position, distance, settings.flockMask)
                .Select(c => c.transform)
                .ToArray();
        }
    }
}