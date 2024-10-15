using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using JUtils.Attributes; // My own utility library
using JUtils.Singletons;
using Managers;
using UI.Cards;

using Random = System.Random;



namespace Enemies
{
    public class WaveSpawner : Singleton<WaveSpawner>
    {
        [SerializeField] private int _currentWave;
        [SerializeField] private int _difficulty;
        [SerializeField] private int _minDifficultyPerWave;
        [SerializeField] private int _maxDifficultyPerWave;
        [SerializeField] private float _spawnRadius;

        [SerializeField] private Boat _boatPrefab;

        [Space]
        [SerializeField] private UnityEvent _onWaveFinish;

        private Random         _randomizer;
        private List<Enemy>    _enemies = new ();
        private WaveTemplate[] _waveTemplates;

        public int currentWave => _currentWave;


        /// <summary>
        /// Load the templates & randomizer
        /// </summary>
        private void Start()
        {
            _randomizer    = new Random(SeedProvider.seed / 10 * 7);
            _waveTemplates = Resources.LoadAll<WaveTemplate>("Waves");
        }


        /// <summary>
        /// Spawn a wave of enemies.
        /// </summary>
        [Button(playModeOnly: true)]
        public void SpawnWave()
        {
            _currentWave++;
            _difficulty += _randomizer.Next(_minDifficultyPerWave, _maxDifficultyPerWave);

            StartCoroutine(SpawnWaveCoroutine());
        }


        /// <summary>
        /// Spawn a wave of enemies.
        /// </summary>
        private IEnumerator SpawnWaveCoroutine()
        {
            //  Filter out all templates that don't match the current wave.

            WaveTemplate[] availableTemplates = _waveTemplates
                .Where(w =>  _currentWave >= w.minWaveNumber && (w.maxWaveNumber == -1 || _currentWave <= w.maxWaveNumber)).ToArray();

            //  Loop until the difficulty is met.

            int difficulty = _difficulty;
            while (difficulty > 0) {
                int target = 0;
                float targetDiff = float.PositiveInfinity;

                //  Find a template that is within the difficulty

                while (difficulty - targetDiff < 0) {
                    target     = _randomizer.Next(availableTemplates.Length);
                    targetDiff = availableTemplates[target].difficulty;
                }

                //  Spawning the template

                WaveTemplate wave = availableTemplates[target];

                difficulty -= wave.difficulty;
                SpawnBoat(wave);

                yield return new WaitForSeconds(.1f);
            }
        }


        /// <summary>
        /// Spawns a boat for a wave template
        /// </summary>
        private void SpawnBoat(WaveTemplate wave)
        {
            //  Randomize the spawn position

            int angle = _randomizer.Next(360);

            Quaternion quat = Quaternion.Euler(0, angle, 0);
            Vector3 pos = quat * Vector3.forward * _spawnRadius;

            //  Spawn the boat

            GameObject boat = Instantiate(_boatPrefab.gameObject, pos, quat);

            boat.transform.parent = transform;
            boat.GetComponent<Boat>().template = wave;
        }


        /// <summary>
        /// Add an enemy to the wave spawner.
        /// </summary>
        public static void AddEnemy(Enemy enemy)
        {
            instance._enemies.Add(enemy);
        }


        /// <summary>
        /// Remove an enemy from the wave spawner.
        /// </summary>
        public static void RemoveEnemy(Enemy enemy)
        {
            if (!instance) return;

            instance._enemies.Remove(enemy);
            if (instance._enemies.Count == 0) instance._onWaveFinish.Invoke();
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up,  _spawnRadius, 10);
        }
#endif
    }
}
