using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using JUtils;   // My utils library
using JUtils.Attributes;
using Managers;
using Buildings.BuildSystem;



namespace Buildings
{
    public abstract class PowerBasedBuilding : Building
    {
        private static readonly Vector3[] _connectors =
        {
            new (0, 0, 1),
            new (0, 0, -1),
            new (1, 0, 0),
            new (-1, 0, 0),
        };


        [Header("Materials")]
        [SerializeField] protected Material _defaultMaterial;
        [SerializeField] protected Material _unpoweredMaterial;

        [Header("Power Options")]
        [SerializeField] private   bool _isPowered;
        [SerializeField] protected int  _updateIndex;

        private MeshRenderer[] _renderers;


        [HideInInspector] public List<PowerBasedBuilding> connections = new ();

        /// <summary>
        /// Get or set the current power state of the building.
        /// </summary>
        public virtual bool isPowered
        {
            get {
                if (!this) return false;
                if (MainBase.updateIndex == _updateIndex)
                    return _isPowered;
                return isPowered = false;
            }
            internal set {
                _updateIndex = MainBase.updateIndex;
                if (value == _isPowered || !this) return;
                _isPowered = value;
                SetMaterial();
            }
        }

        /// <summary>
        /// Check if the building is up to date with the main base.
        /// </summary>
        public bool isUpToDate => MainBase.updateIndex == _updateIndex;


        /// <summary>
        /// Get all the components
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _renderers = GetComponentsInChildren<MeshRenderer>();
        }


        /// <summary>
        /// Updates the power state of the building
        /// </summary>
        protected override void Start()
        {
            base.Start();

            //  Find all connectors

            foreach (Vector3 connector in _connectors) {
                Vector2Int pos = BuildManager.GirdPosition(transform.position + rotation * connector);

                //  Getting the tile at the connector

                Tile tile = BuildManager.GetTile(pos);
                if (!tile || !tile.hasObject) continue;

                if (tile.building == this)
                    throw new Exception("Building cannot be connected to itself " + pos);

                //  Checking if the tile is a power based building and not a pipe

                if (tile.building is not Pipe pipe || !pipe.CanConnectTo(this.tile))
                    continue;

                //  Adding this to the pipe

                connections.Add(pipe);
                pipe.connections.Add(this);
            }

            //  Setting the power state

            PipeManager.onPipeDestroyed += ConnectionUpdated;
            ConnectionUpdated();
            SetMaterial();
        }


        /// <summary>
        /// Check if the pipe is connected to the main base
        /// </summary>
        public virtual void ConnectionUpdated()
        {
            if (isPowered) return;
            isPowered = connections.Any(x => x is Pipe && x.isPowered);
        }


        /// <summary>
        /// Changes the material of the building based on the power state
        /// </summary>
        protected void SetMaterial()
        {
            Material target = isPowered ? _defaultMaterial : _unpoweredMaterial;
            bool     update = false;

            //  Changing all the mesh renderers

            foreach (MeshRenderer meshRenderer in _renderers) {

                //  This case rarely happens, but can cause some nasty errors
                if (!meshRenderer) {
                    update = true;
                    continue;
                }

                meshRenderer.material = target;
            }

            //  Updating the mesh renderers if needed

            if (update) _renderers = GetComponentsInChildren<MeshRenderer>();
        }


        #region Debug

        [Button("Show Connectors")]
        public void ShowConnections()
        {
            foreach (PowerBasedBuilding connection in connections)
                Debug.Log(connection.tile.position);
        }


        protected virtual void OnDrawGizmos()
        {
            if (!MainBase.instance) return;

            Gizmos.color = isPowered ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + Vector3.up, 0.05f);
        }

        #endregion
    }
}