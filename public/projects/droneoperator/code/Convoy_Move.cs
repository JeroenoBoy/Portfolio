protected virtual void Update()
{
    if (isFinished) return;

    Vector3 position = transform.position;

    //  Calculating speed multi

    bool isProperParent = parent != null;

    float moveSpeedMulti   = isProperParent ? ((position - parent.position).magnitude - _options.minDistance) / (_options.targetDistance - _options.minDistance) : 1;
    float cornerSpeedMulti = 1-Mathf.Clamp01(angle / _options.stopAngle);

    if (isProperParent && parent is InvisibleConvoyLeader) {
        moveSpeedMulti = Mathf.Min(moveSpeedMulti, 1);
    }

    float totalMulti = (moveSpeedMulti + cornerSpeedMulti) * .5f;

    //  Calculating new position

    Vector3         newPos = Vector3.MoveTowards(position, targetNode, _options.moveSpeed * totalMulti * Time.deltaTime);
    Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeedMulti * totalMulti * Time.deltaTime);

    transform.SetPositionAndRotation(newPos, newRotation);

    //  Checking distance

    CalculateTarget(newPos);
}


protected void CalculateTarget(Vector3 newPos)
{
    if (!((newPos - targetNode).magnitude < _options.moveSpeed * .3f)) return;

    if (++_pathIndex >= path.Count) {
        isFinished = true;
        _onFinish.Invoke();
        return;
    }

    targetNode      = path[_pathIndex];
    Vector3 nextNode = path[Mathf.Min(_pathIndex+1, path.Count-1)];

    Quaternion a = Quaternion.LookRotation(nextNode - targetNode);
    Quaternion b = Quaternion.LookRotation(targetNode - transform.position);

    targetRotation = Quaternion.Slerp(a,b,.5f);
    angle          = Quaternion.Angle(transform.rotation, targetRotation);
    rotSpeedMulti  = angle / (nextNode - targetNode).magnitude * _options.moveSpeed;
}