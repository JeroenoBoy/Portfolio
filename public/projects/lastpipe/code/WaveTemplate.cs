using System;
using UnityEngine;



namespace Enemies
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Wave", order = 1)]
    public class WaveTemplate : ScriptableObject
    {
        [SerializeField] private bool _updateFromName = true;

        public int difficulty;
        public int minWaveNumber;
        public int maxWaveNumber = -1;

        public Enemy[] enemies;


        /// <summary>
        /// Makes it easier for me to edit the wave template in the editor.
        /// </summary>
        private void OnValidate()
        {
            if (!_updateFromName) return;

            //  Splits wave number

            string[] splitted1 = name.Split(' ');
            if (splitted1.Length < 2) return;

            //  Splits difficulty

            string[] splitted2 = splitted1[0].Split('-');
            if (splitted2.Length != 2) return;

            //  Parses all values

            /* difficulty */ if (!int.TryParse(splitted1[1], out int diff)) return;
            /* min wave   */ if (!int.TryParse(splitted2[0], out int min)) return;
            /* max wave   */ if (!int.TryParse(splitted2[1] == "" ? "-1" : splitted2[1], out int max)) return;

            //  Apply all the values

            difficulty = diff;
            minWaveNumber = min;
            maxWaveNumber = max;
        }
    }
}
