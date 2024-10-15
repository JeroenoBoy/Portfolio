using System;
using System.Collections.Generic;
using System.Linq;
using JUtils.Attributes;
using JUtils.Singletons;
using UnityEngine;



namespace Enemies
{
    public class EnemyShootManager : SingletonBehaviour<EnemyShootManager>
    {
        #region Statics

        /// <summary>
        /// Request for an enemy tank to shoot
        /// </summary>
        public static bool RequestShot(AttackToken token)
        {
            EnemyShootManager manager = instance;

            if (!manager) {
                Debug.LogError("No instance of EnemyShootManager was found!");
                return true;
            }

            if (manager._shotsPerMinute >= manager._maxShotsPerMinute) return false;
            if (manager._shotsPerSecond >= manager._maxShotsPerSecond) return false;

            if (FirstToken() != token) return false;
            RevokeToken(token);

            manager._shotsPerSecond++;
            manager._shotsPerMinute++;

            return true;
        }


        public static AttackToken RequestToken()
        {
            AttackToken token = new ();
            instance._tokens.Add(token);
            return token;
        }


        public static void RevokeToken(AttackToken token)
        {
            token.isRevoked = true;
            instance._tokens.Remove(token);
        }


        public static AttackToken FirstToken()
        {
            EnemyShootManager manager = instance;
            float time = Time.time - manager._tokenMaxAliveTime;

            for (int i = 0; i < manager._tokens.Count; i++) {
                AttackToken token = manager._tokens[i];
                if (!token.isRevoked && token.created > time) return token;

                RevokeToken(token);
                i--;
            }

            return null;
        }

        #endregion


        [SerializeField] private float _maxShotsPerMinute;
        [SerializeField] private float _maxShotsPerSecond;
        [SerializeField] private float _tokenMaxAliveTime = 1f;

        [Header("Debug")]
        [SerializeField, ReadOnly] private float _shotsPerSecond;
        [SerializeField, ReadOnly] private float _shotsPerMinute;

        [SerializeField, ReadOnly] private float _coroutinesSuckSoThisTimerVariableForSeconds;
        [SerializeField, ReadOnly] private float _coroutinesSuckSoThisTimerVariableForMinutes;


        private List<AttackToken> _tokens = new ();


        private void Awake()
        {
            _coroutinesSuckSoThisTimerVariableForMinutes = Time.time;
            _coroutinesSuckSoThisTimerVariableForSeconds = Time.time;
        }


        private void FixedUpdate()
        {
            UpdateSeconds();
            UpdateMinutes();
        }


        private void UpdateSeconds()
        {
            if (_coroutinesSuckSoThisTimerVariableForSeconds > Time.time) return;
            _coroutinesSuckSoThisTimerVariableForSeconds += 1;

            _shotsPerSecond = Mathf.Max(0, _shotsPerSecond - _maxShotsPerSecond);
        }


        private void UpdateMinutes()
        {
            if (_coroutinesSuckSoThisTimerVariableForMinutes > Time.time) return;
            _coroutinesSuckSoThisTimerVariableForMinutes += 60;

            _shotsPerMinute = Mathf.Max(0, _shotsPerMinute - _maxShotsPerMinute);
        }
    }



    public class AttackToken
    {
        public readonly float created;
        public bool isRevoked;

        public AttackToken()
        {
            isRevoked = false;
            created = Time.time;
        }
    }
}