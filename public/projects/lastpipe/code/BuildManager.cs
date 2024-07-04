using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;
using JUtils.Extensions; // My own utils library
using JUtils.Singletons;
using Buildings;
using Buildings.BuildSystem;
using UI.Inventory;
using Data;



namespace Managers
{
    public class BuildManager : Singleton<BuildManager>
    {
        [SerializeField] private InventoryItem   _selectedObject;
        [SerializeField] private BuildableObject _buildableObject;

        private Tile _selectedTile;
        private Dictionary<Vector2Int, Tile> _tiles = new ();

        private Building _selectedBuilding;


        /// <summary>
        /// Change the selected object
        /// </summary>
        public Building selectedBuilding
        {
            get => _selectedBuilding;
            set {

                //  if the value is null, we reset the selected object

                if (_selectedBuilding == value && value is not null) {
                    value.ShowStats(false);
                    _selectedBuilding = null;
                    return;
                }

                //  Hide the stats menu of the previous selected object

                if (_selectedBuilding) {
                    _selectedBuilding.ShowStats(false);
                }

                //  Setting the new selected object

                _selectedBuilding = value;
                if (value) value.ShowStats(true);
            }
        }


        /// <summary>
        /// Clear the tile map
        /// </summary>
        private void Awake()
        {
            _tiles.Clear();
        }


        /// <summary>
        /// QOL: Also set the buildable object
        /// </summary>
        private void OnValidate()
        {
            if (Application.isPlaying
                && isActiveAndEnabled
                && _buildableObject
                && _selectedObject != _buildableObject.selectedObject)
                _buildableObject.SetObject(_selectedObject);
        }


        /// <summary>
        /// Sets the position of the buildable object
        /// </summary>
        private void Update()
        {
            //  Rotate the buildable object
            if (Input.GetKeyDown(KeyCode.R))
                _buildableObject.Rotate(90);

            //  Cache the transform
            Transform t = _buildableObject.transform;

            //  Moving and updating the buildable object

            if (selectedTile && !EventSystem.current.IsPointerOverGameObject()) {
                t.position = selectedTile.buildingPosition;

                MeshRenderer ren = _buildableObject.renderer;

                //  Letting the item decide which color the ring should be
                if (selectedObject) {
                    ren.material.color = selectedObject.item.HandleColor(selectedTile);
                }

                //  When over a building, the ring should be white when nothing selected
                else if (selectedTile && selectedTile.hasObject && selectedTile.building.statsMenu) {
                    ren.material.color = Item.activeColor;

                    if (Input.GetMouseButtonDown(0))
                        selectedBuilding = selectedTile.building;
                }

                // When over nothing, the ring should be disabled
                else {
                    ren.material.color = Item.disabledColor;

                    if (Input.GetMouseButtonDown(0))
                        selectedBuilding = null;
                }
            }
            else {
                t.position = Vector3.up * 10000;
            }
        }


        #region Statics

        public static bool CanBuildOn(Tile tile)
        {
            if (!tile) return false;
            if (selectedObject == null) return false;
            if (EventSystem.current.IsPointerOverGameObject()) return false;

            Item item = selectedObject.item;

            if (item is DeleteItem) return tile.hasObject && tile.building is not MainBase;
            if (tile.hasObject) return false;

            return item.canBuildOnCrystal
                       ? tile.hasCrystal
                       : !tile.hasCrystal;
        }


        public static Tile selectedTile
        {
            get => instance._selectedTile;
            set => instance._selectedTile = value;
        }


        public static InventoryItem selectedObject
        {
            get => instance?._buildableObject?.selectedObject;
            set => instance._buildableObject.SetObject(value);
        }


        /// <summary>
        /// Add a tile to the build manager
        /// </summary>
        /// <param name="tile">The tile to add</param>
        public static void AddTile(Tile tile)
        {
            if (HasTile(tile)) return;
            instance._tiles.Add(tile.position, tile);
        }


        /// <summary>
        /// Add a tile to the build manager
        /// </summary>
        /// <param name="tile">The tile to add</param>
        public static bool HasTile(Tile tile)
        {
            return instance._tiles.ContainsKey(tile.position);
        }


        /// <summary>
        /// Get a tile from the build manager
        /// </summary>
        /// <param name="tilePosition">Position of the tile</param>
        /// <returns>Tile at that position</returns>
        public static Tile GetTile(Vector2Int tilePosition)
        {
            return instance._tiles.ContainsKey(tilePosition) ? instance._tiles[tilePosition] : null;
        }


        /// <summary>
        /// Remove a tile from the build manager
        /// </summary>
        /// <param name="tile">The tile to remove</param>
        public static void RemoveTile(Tile tile)
        {
            if (instance && instance._tiles[tile.position] == tile) {
                instance._tiles.Remove(tile.position);
            }
        }


        /// <summary>
        /// Build the current selected tile
        /// </summary>
        public static void Build([NotNull] GameObject building, [NotNull] Tile target)
        {
            instance._buildableObject.Build(building, target);
        }


        /// <summary>
        /// Build the current selected tile
        /// </summary>
        public static void Build()
        {
            if (!CanBuildOn(instance._selectedTile)) return;
            instance.selectedBuilding = null;
            instance._buildableObject.Build(selectedTile);
        }


        /// <summary>
        /// Build the current selected tile
        /// </summary>
        public static void Build(Tile tile)
        {
            if (!CanBuildOn(tile)) return;
            instance._buildableObject.Build(tile);
        }


        /// <summary>
        /// Create tiles from a list of positions
        /// </summary>
        /// <param name="cells"></param>
        public static void CreateTilesFromMap(Transform origin, float[,] cells, float minHeight)
        {
            int width  = cells.GetLength(1);
            int height = cells.GetLength(0);

            Vector3 originPos = origin.position.Round() + Vector3.up;

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (cells[y,x] < minHeight) continue;
                    Vector3 position = originPos + new Vector3(x, 0, y);
                    Tile tile = new Tile(origin, position);

                    AddTile(tile);
                }
            }
        }


        /// <summary>
        /// Reset the build manager
        /// </summary>
        public static void ResetGrid()
        {
            instance._tiles.Clear();
        }


        #region Utility

        public static Vector2Int GirdPosition(Vector3 vector)
        {
            return Vector2Int.RoundToInt(vector.XZToVector2());
        }

        #endregion


        #endregion


        #region Dev
#if UNITY_EDITOR

        private IEnumerator SetAllTiles()
        {
            foreach (Tile tile in _tiles.Values) {
                Build(tile);
                yield return null;
            }
        }

#endif
        #endregion
    }
}
