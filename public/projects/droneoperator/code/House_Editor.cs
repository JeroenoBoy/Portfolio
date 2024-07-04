[Button(clickableInEditor = true)]
private void Cleanup()
{
    GameObject[] objects = GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();

    Undo.RecordObjects(objects, "Cleanup");

    foreach (GameObject o in objects) {
        if (o == gameObject) continue;
        if (!o) continue;
        if (o.TryGetComponent(out MeshCollider collider)) DestroyImmediate(collider);
        if (!o.GetComponentInChildren<MeshFilter>()) DestroyImmediate(o);
    }

    SavePrefab();
}


[Button(clickableInEditor = true)]
private void GenerateCollider()
{
    Matrix4x4 matrix = transform.worldToLocalMatrix;

    CombineInstance[] instances = GetComponentsInChildren<MeshFilter>()
        .Select(t => new CombineInstance() { mesh = t.sharedMesh, transform = matrix * t.transform.localToWorldMatrix})
        .ToArray();

    MeshCollider collider = GetComponent<MeshCollider>();

    Mesh mesh = new ();
    mesh.CombineMeshes(instances);

    Physics.BakeMesh(mesh.GetInstanceID(), true);

    AssetDatabase.CreateAsset(mesh, AssetPath() + "/Meshes/" + gameObject.name + ".fbx");
    AssetDatabase.SaveAssets();

    //  Saving Mesh

    collider.sharedMesh = mesh;
    SavePrefab();
}


[Button(clickableInEditor = true)]
private void GenerateExplodedVariant()
{
    GameObject[] objects = GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
    Undo.RecordObjects(objects, "Generate Exploding Variant");

    //  Creatiny copy

    GameObject clone = Instantiate(gameObject);

    DestroyImmediate(clone.GetComponent<BoxCollider>());

    //  Adding components

    foreach (Transform o in clone.transform) {
        if (!o.TryGetComponent(out MeshFilter filter)) continue;

        MeshCollider collider = o.AddComponent<MeshCollider>();

        o.AddComponent<Rigidbody>();

        collider.convex = true;
        collider.sharedMesh = filter.sharedMesh;
    }

    //  Removing Static

    foreach (Transform component in clone.GetComponentsInChildren<Transform>()) {
        component.gameObject.isStatic = false;
    }

    //  Cleanup and creating prefab

    clone.name = "Exploded_" + gameObject.name;
    DestroyImmediate(clone.GetComponent<ExplodingHouse>());
    DestroyImmediate(clone.GetComponent<HealthComponent>());

    string path = AssetPath() + "/Exploded/" + clone.name + ".prefab";
    GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(clone, path, InteractionMode.AutomatedAction);

    DestroyImmediate(clone);
    _targetHouse = prefab;
    SavePrefab();
}


private string AssetPath() => string.Join('/', PrefabStageUtility.GetPrefabStage(gameObject).assetPath.Split('/').SkipLast(1).ToArray());
private void SavePrefab() => PrefabUtility.SaveAsPrefabAsset(gameObject, PrefabStageUtility.GetPrefabStage(gameObject).assetPath);