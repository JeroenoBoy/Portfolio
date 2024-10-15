using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using JUtils; // My utility library
using JUtils.Singletons;
using Buildings;
using Buildings.BuildSystem;



namespace Managers
{

    public class PipeManager : Singleton<PipeManager>
    {
        [SerializeField] private UnityEvent _onPipeDestroyed = new ();

        private List<Pipe> _pipes = new ();


        /// <summary>
        /// Called when a pipe gets destroyed.
        /// </summary>
        public static event UnityAction onPipeDestroyed
        {
            add    => instance._onPipeDestroyed.AddListener(value);
            remove => instance._onPipeDestroyed.RemoveListener(value);
        }


        /// <summary>
        /// Used to add a pipe to the pipe manager.
        /// </summary>
        /// <param name="pipe"></param>
        public static void PipeCreated(Pipe pipe)
        {
            if (!instance) return;
            instance._pipes.Add(pipe);
        }


        /// <summary>
        /// Used to destroy a pipe and update the power of others.
        /// </summary>
        public static void PipeDestroyed(Pipe pipe)
        {
            if (!instance || !MainBase.instance) return;

            instance._pipes.Remove(pipe);
            MainBase.instance.OnPipeDestroyed();
            instance._onPipeDestroyed.Invoke();
        }
    }
}
