private void FindTarget() {

    //  Caching

    Vector3 position = transform.position;

    //  Running query

    float sqrDistance = Mathf.Infinity;
    MovableUnit closest = null;

    List<MovableUnit> targetsToRemove = new ();

    foreach (MovableUnit target in _targets) {
        if (!target) {
            targetsToRemove.Add(target);
            continue;
        }

        float currSqrDistance = (position - target.position).sqrMagnitude;
        if (currSqrDistance >= sqrDistance) continue;

        if (!_externalAimAtTargetCheck) {
            Ray ray = new (position, target.GetComponent<Collider>().ClosestPoint(position) - position);
            if (!Raycast(ray, out RaycastHit hit) || hit.transform != target.transform) continue;
        }

        sqrDistance = currSqrDistance;
        closest = target;
    }

    //  Removing invalid targets

    foreach (MovableUnit target in targetsToRemove) {
        _targets.Remove(target);
    }

    //  Applying

    _target = closest;
}