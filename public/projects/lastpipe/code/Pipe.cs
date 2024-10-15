using System;
using System.Linq;
using UnityEngine;
using JUtils.Extensions; // My own utility library
using Buildings.BuildSystem;
using Managers;



namespace Buildings
{
    public class Pipe : PowerBasedBuilding
    {
        [SerializeField] private Vector2[] _connectors;


        /// <summary>
        /// Get or set the power status of this pipe.
        /// </summary>
        public override bool isPowered
        {
            get => base.isPowered;
            internal set {
                base.isPowered = value;
                if (!base.isPowered) UnpowerNeighbors();
            }
        }


        /// <summary>
        /// Find all connectors and fun stuff
        /// </summary>
        protected override void Start()
        {
            FindTile();
            PipeManager.PipeCreated(this);

            //  Find all connectors

            foreach (Vector2 connector in _connectors) {
                Vector2Int pos = BuildManager.GirdPosition(transform.position + rotation * connector.ToXZVector3());

                //  Find the tile at the connector

                Tile tile = BuildManager.GetTile(pos);
                if (!tile || !tile.hasObject) continue;

                if (tile.building == this)
                    throw new Exception("Pipe cannot be connected to itself " + pos);

                //  Checking if the building can be connected to the pipe

                if (tile.building is not PowerBasedBuilding consumer) continue;
                consumer.connections.Add(this);

                //  Checking if the pipe can be connected to the building

                if (tile.building is Pipe other && !other.CanConnectTo(this.tile))
                    continue;

                connections.Add(consumer);
            }

            //  Updating the power status of the pipe and its neighbors

            base.ConnectionUpdated();
            if (isPowered)
                UpdateNeighbors();
            else
                SetMaterial();
        }


        /// <summary>
        /// Checks if this pipe can connect to the tile
        /// </summary>
        public bool CanConnectTo(Tile pipe)
        {
            foreach (Vector2 connector in _connectors) {
                Vector2Int pos = BuildManager.GirdPosition(transform.position + rotation * connector.ToXZVector3());
                if (pos == pipe.position) return true;
            }

            return false;
        }


        /// <summary>
        /// Gets executed when the pipe is connected to another pipe
        /// </summary>
        public override void ConnectionUpdated()
        {
            base.ConnectionUpdated();
            if (isPowered) UpdateNeighbors();
            else UnpowerNeighbors();
        }


        /// <summary>
        /// Updates the power status of neighbors
        /// </summary>
        protected void UpdateNeighbors()
        {
            foreach (PowerBasedBuilding connection in connections) {
                if (connection.isPowered) continue;
                connection.ConnectionUpdated();
            }
        }


        /// <summary>
        /// Unpowers the neighbors
        /// </summary>
        private void UnpowerNeighbors()
        {
            foreach (PowerBasedBuilding connection in connections) {
                if (connection.isUpToDate) continue;
                connection.ConnectionUpdated();
            }
        }


        /// <summary>
        /// Cleans up the pipe
        /// </summary>
        protected virtual void OnDestroy()
        {
            foreach (PowerBasedBuilding child in connections)
                child.connections.Remove(this);

            PipeManager.PipeDestroyed(this);
            UnpowerNeighbors();
        }


        #region Debug


        protected override void OnDrawGizmos()
        {
            if (_connectors == null)
                return;

            base.OnDrawGizmos();

            Gizmos.color = Color.blue;
            foreach (Vector2 connector in _connectors) {
                Quaternion rot = Quaternion.Euler(transform.eulerAngles.With(0, z: 0));
                Gizmos.DrawSphere(transform.position + rot * connector.ToXZVector3(), .05f);
            }
        }


        #endregion
    }
}
