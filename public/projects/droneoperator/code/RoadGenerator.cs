public void Bake()
{
    if (_fileName == "") _fileName = Random.Range(0, 10000000).ToString();

    //  Getting data and arrays

    Vector3[] points = _path.CalculateLocalEquallySpacedPointsRelative(_pointDistance);

    int[]        triangles = new int[(points.Length-1)*6];
    List<Vector3> vertices = new (Mathf.CeilToInt(points.Length*2 + points.Length / _uvWrapDistance));
    List<Vector3> normals  = new (Mathf.CeilToInt(points.Length*2 + points.Length / _uvWrapDistance));
    List<Vector2> uvs      = new (Mathf.CeilToInt(points.Length*2 + points.Length / _uvWrapDistance));

    float uvTimer = 0;
    float uvAdder = _pointDistance / _uvWrapDistance;

    Vector3 upAdder = transform.up * _roadYOffset;

    //  Looping through all the points

    Vector3 previous = points[0] + (points[0] - points[1]);
    for (int i = 0; i < points.Length; i++) {
        Vector3 current = points[i];

        int ti = (i-1) * 6;                                        // Triangles Index
        int vi = vertices.Count;                                  //  Vertices Index
        current = points[i];

        //  Calculating vertices

        Vector3 direction = current - previous;
        Vector3 cross     = Vector3.Cross(direction, Vector3.up).normalized * _roadWith;

        void AddPoints(float uv)
        {
            vertices.Add(current + cross + upAdder);
            vertices.Add(current - cross + upAdder);

            //  Calculate normals

            normals.Add(Vector3.Cross(cross, direction));
            normals.Add(Vector3.Cross(direction, -cross));

            //  uv's

            uvs.Add(new Vector2(uv,  0));
            uvs.Add(new Vector2(uv,  1));
        }

        //  Uv fancy calculations

        uvTimer += uvAdder;

        if (uvTimer >= 1) {
            uvTimer = 0;
            AddPoints(1);
            AddPoints(0);
        }
        else {
            AddPoints(uvTimer);
        }

        //  Setting previous to current

        previous = current;

        if (i == 0) continue;

        //  Calculating triangles

        triangles[ti]   = vi;
        triangles[ti+1] = vi -1;
        triangles[ti+2] = vi -2;
        triangles[ti+3] = vi +1;
        triangles[ti+4] = vi -1;
        triangles[ti+5] = vi;
    }

    //  Creating the mesh

    _meshFilter.mesh = new Mesh()
    {
        vertices  = vertices.ToArray(),
        normals   = normals.ToArray(),
        uv = uvs.ToArray(),
        triangles = triangles,
    };
}