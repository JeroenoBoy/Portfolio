[CustomEditor(typeof(BezierPathEditor))]
private class PathEditor : Editor
{
    private BezierPathEditor _creator;
    private BezierPath _path;

    private bool _pointSelected;
    private int _selectedHandle = -1;
    private int _activeCurve;

    private Vector3[] _calculatedPoints = Array.Empty<Vector3>();


    private void OnEnable()
    {
        _creator = (BezierPathEditor)target;
        _path = _creator._path ??= _creator.CreatePath();
    }


    private void OnSceneGUI()
    {
        Handles.matrix = Matrix4x4.TRS(_creator.position, _creator.rotation, _creator.scale);

        Draw();
        DrawEqualPoints();
        Input();
    }


    private void Input()
    {
        Event guiEvent = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        if (!Physics.Raycast(mouseRay, out RaycastHit hit, 100f)) return;

        //  Adding segment

        if (guiEvent is {type: EventType.MouseDown, button: 0, control: true}) {
            Event.current.Use();

            Undo.RecordObject(_creator, "Add Segment");
            _path.AddSegment(_creator.RelativePoint(hit.point));
            _creator.editorOnUpdate?.Invoke();
        }

        //  Inserting segment

        if (guiEvent is {shift: true}) {
            //  Getting closest gui point

            float minDist = _creator._selectCurveDistance;
            int selectedIndex = -1;

            for (int i = _path.segmentCount; i --> 0;) {
                BezierNode node = _creator.WorldPositionNode(i);

                float distance = HandleUtility.DistancePointBezier(hit.point, node.pointA, node.pointB, node.handleA, node.handleB);
                if (distance > minDist) continue;

                selectedIndex = i;
                minDist = distance;
            }

            _activeCurve = selectedIndex;

            //  Adding segment

            if (guiEvent is {type: EventType.MouseDown, button: 0, control: false} && _activeCurve != -1) {
                Event.current.Use();

                Undo.RecordObject(_creator, "Added segment");
                _path.InsertSegment(selectedIndex, _creator.RelativePoint(hit.point));
                _creator.editorOnUpdate?.Invoke();
            }
        }
        else {
            _activeCurve = -1;
        }
    }


    /// <summary>
    /// Draws the bezier curve and all its handles
    /// </summary>
    private void Draw()
    {
        Handles.zTest = CompareFunction.Less;
        if (_creator._autoSetControlPoints) {

            //  Drawing the curve

            Handles.color = Color.white;
            for (int i = 0; i < _path.segmentCount; i++) {
                BezierNode node = _path.GetNode(i);

                Color color = i == _activeCurve ? Color.yellow : Color.black;
                Handles.DrawBezier(node.pointA, node.pointB, node.handleA, node.handleB, color, null, 2);
            }

            //  Drawing handles

            Handles.color = Color.red;

            for (int i = 0; i < _path.totalPoints; i+=3) {

                //  Points that are selectable

                if (_selectedHandle != i) {
                    if (!Handles.Button(_path[i], Quaternion.identity, _creator._handleSize, _creator._handleSize, Handles.SphereHandleCap)) continue;
                    Event.current.Use();

                    if (Event.current.shift && i % 3 == 0) {
                        Undo.RecordObject(_creator, "Segment Deleted");
                        _path.RemoveSegment(i / 3);
                        _creator.editorOnUpdate?.Invoke();
                    }
                    else _selectedHandle = i;

                    continue;
                }


                //  Bezier path arrows

                Vector3 newPos = Handles.DoPositionHandle(_path[i], Quaternion.identity);

                if (_path[i] == newPos) continue;
                Undo.RecordObject(_creator, "Move Bezier Point");
                _path.AutoSetControlPoints(i/3);
                _path[i] = newPos;
                _creator.editorOnUpdate?.Invoke();
            }
        }
        else {

            //  Drawing the curve

            Handles.color = Color.white;
            for (int i = 0; i < _path.segmentCount; i++) {
                BezierNode node = _path.GetNode(i);

                Handles.DrawLine(node.handleA, node.pointA);
                Handles.DrawLine(node.handleB, node.pointB);

                Color color = i == _activeCurve ? Color.yellow : Color.black;
                Handles.DrawBezier(node.pointA, node.pointB, node.handleA, node.handleB, color, null, 2);
            }

            //  Drawing handles

            Handles.color = Color.red;

            for (int i = 0; i < _path.totalPoints; i++) {

                //  Points that are selectable

                if (_selectedHandle != i) {
                    if (!Handles.Button(_path[i], Quaternion.identity, _creator._handleSize, _creator._handleSize, Handles.SphereHandleCap)) continue;
                    Event.current.Use();

                    if (Event.current.shift && i % 3 == 0) {
                        Undo.RecordObject(_creator, "Segment Deleted");
                        _path.RemoveSegment(i/3);
                        _creator.editorOnUpdate?.Invoke();
                    }
                    else _selectedHandle = i;

                    continue;
                }


                //  Bezier path arrows

                Vector3 newPos = Handles.DoPositionHandle(_path[i], Quaternion.identity);

                if (_path[i] == newPos) continue;
                Undo.RecordObject(_creator, "Move Bezier Point");
                _path[i] = newPos;
                _creator.editorOnUpdate?.Invoke();
            }
        }
    }


    /// <summary>
    /// Draws the equally generated points
    /// </summary>
    private void DrawEqualPoints()
    {
        Handles.matrix = Matrix4x4.identity;

        Handles.color = Color.green;
        for (int i = 0; i < _calculatedPoints.Length; i++) {
            Handles.DrawSolidDisc(_calculatedPoints[i], Vector3.up, .5f);
        }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Reset Path")) {
            Undo.RecordObject(_creator, "Reset Path");
            _path.Reset();
            _creator.editorOnUpdate?.Invoke();
        }


        if (GUILayout.Button("Auto Set Control Points")) {
            Undo.RecordObject(_creator, "Auto Set Path");
            _path.AutoSetAllControlPoints();
            _creator.editorOnUpdate?.Invoke();
        }


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Visualize Equal Points")) {
            Undo.RecordObject(_creator, "Visualize Equal Points");
            _calculatedPoints = _creator.CalculateEquallySpacedPoints();
        }
        if (GUILayout.Button("Delete Equal Points")) {
            Undo.RecordObject(_creator, "Visualize Equal Points");
            _calculatedPoints = Array.Empty<Vector3>();
        }
        GUILayout.EndHorizontal();
    }
}