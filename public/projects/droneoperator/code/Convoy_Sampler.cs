public Vector3[] CalculateEquallySpacedPoints(float pointDistance, BezierNode[] nodes)
{
    List<Vector3> points = new () { nodes[0].pointA };

    //  Caching

    float pDistance = pointDistance * pointDistance;
    float deadZone = _deadZone * _deadZone;
    float startIncrementAmount = _incrementAmount;
    int   maxTime = nodes.Length;

    //  loop variables

    float time = 0;

    //  Looping through all the nodes

    while (time < maxTime) {
        Vector3 startPoint = points[^1];
        Vector3 currPoint  = Vector3.zero;
        float distance = float.NegativeInfinity;

        float incrementAmount = startIncrementAmount;
        int direction = 1;

        //  Getting next point time

        while (math.abs(distance - pDistance) > deadZone) {

            //  Halving incrementer when swapping direction

            if (pDistance - distance > 0 && direction == -1) {
                direction = 1;
                incrementAmount *= .5f;
            }
            else if (pDistance - distance < 0 && direction == 1) {
                direction = -1;
                incrementAmount *= .5f;
            }

            //  Changing time

            time += direction * incrementAmount;

            //  Setting point

            if (time >= maxTime && distance < pDistance) {
                currPoint = nodes[^1].pointB;
                break;
            }

            currPoint = nodes[Mathf.FloorToInt(time)].Sample(time % 1);
            distance = (startPoint - currPoint).sqrMagnitude;
        }

        points.Add(currPoint);
    }

    //  Returning

    return points.ToArray();
}