using System;
using JetBrains.Annotations;
using UnityEngine;
using JUtils; // My own utils library
using JUtils.Extensions;
using Managers;
using UI.Inventory;
using Data;

using Random = UnityEngine.Random;



namespace Buildings.BuildSystem
{
    public class BuildableObject : MonoBehaviour
    {
        [SerializeField] private Material       _material;
        [SerializeField] private ParticleSystem _placeParticle;
        [SerializeField] private ParticleSystem _destroyParticle;
        [SerializeField] private Transform      _rangeRing;

        public InventoryItem selectedObject { get; private set; }
        public new MeshRenderer renderer { get; private set; }


        private void Awake()
        {
            renderer = GetComponent<MeshRenderer>();
        }


        /// <summary>
        /// Change the item that is selected
        /// </summary>
        public void SetObject([CanBeNull] InventoryItem item)
        {
            //  Cleanup

            ClearChildren();
            _rangeRing.gameObject.SetActive(false);

            //  Delete item if null

            if (item == null) { selectedObject = null; return; }

            //  Set item

            if (item.item is DeleteItem) renderer.material.color = Color.red;
            else {
                renderer.material.color = Color.white;
                PopulateChildren(item.item.itemPrefab);

                //  Set range ring

                if (item.item.itemPrefab.TryGetComponent(out SphereCollider c) &&
                    c.isTrigger)
                {
                    _rangeRing.gameObject.SetActive(true);
                    float radius = c.radius * 2;
                    _rangeRing.localScale = new Vector3(radius, .1f, radius);
                }
            }

            //  Set selected object

            selectedObject = item;
        }


        /// <summary>
        /// Rotate the ghost of the item
        /// </summary>
        public void Rotate(float degrees)
        {
            transform.eulerAngles += new Vector3(0, degrees, 0);
        }


        /// <summary>
        /// Build the selected item item on the tile
        /// </summary>
        public void Build(Tile tile)
        {
            if (!BuildManager.CanBuildOn(tile)) return;

            if (selectedObject.item is DeleteItem) {

                //  Particles
                Instantiate(_destroyParticle.gameObject, tile.buildingPosition - Vector3.down*.5f, Quaternion.identity);

                //  Checking if the building should be destroyed or not

                HealthComponent hc = tile.building.GetComponent<HealthComponent>();
                int selector = Random.Range(0, hc.maxHealth);

                if (selector > hc.health) { hc.Kill(); return; }

                //  Adding the item to the inventory

                InventoryManager.AddItem(tile.building.item, 1);
                Destroy(tile.building.gameObject);
            }

            else {
                //  Build the item
                Instantiate(_placeParticle.gameObject, tile.buildingPosition - Vector3.down*.5f, Quaternion.identity);
                Build(selectedObject.item, tile);
                selectedObject.amount--;
            }
        }


        /// <summary>
        /// Build a specific item on a specific tile
        /// </summary>
        public void Build(Item item, Tile tile)
        {
            Building building = Build(item.itemPrefab, tile);
            building.item = item;
        }


        /// <summary>
        /// Build the gameObject on the tile
        /// </summary>
        public Building Build(GameObject target, Tile tile)
        {
            Building building = Instantiate(
                target,
                tile.buildingPosition,
                transform.rotation,
                tile.transform
            ).GetComponent<Building>();

            return tile.building = building;
        }


        /// <summary>
        /// Populate the children of the item
        /// </summary>
        private void PopulateChildren(GameObject prefab)
        {
            foreach (MeshRenderer renderer in prefab.GetComponentsInChildren<MeshRenderer>()) {
                Mesh mesh = renderer.GetComponent<MeshFilter>().sharedMesh;

                //  Creating component

                GameObject child = new GameObject(renderer.name);
                child.AddComponent<MeshFilter>().mesh = mesh;
                child.AddComponent<MeshRenderer>().material = _material;

                //  Caching properties

                Transform targetTransform = renderer.transform;
                Transform childTransform  = child.transform;

                //  Setting properties

                childTransform.parent = transform;
                childTransform.localPosition = targetTransform.position;
                childTransform.localRotation = targetTransform.rotation;
                childTransform.localScale    = targetTransform.lossyScale;

            }
        }


        private void ClearChildren()
        {
            foreach (Transform child in transform.GetChildren())
                Destroy(child.gameObject);
        }
    }
}
