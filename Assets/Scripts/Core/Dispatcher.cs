using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pong.Core
{
    class Dispatcher: MonoBehaviour
    {
        static Dispatcher _instance;
        public static Dispatcher Instance => GetInstance();

        readonly Queue<Action> _jobs = new Queue<Action>();

        public static Dispatcher GetInstance()
        {
            if (!_instance) {
                throw new NullReferenceException("Could not find Dispatcher object.");
            }

            return _instance;
        }

        public void Enqueue(IEnumerator job)
        {
            lock (_jobs) {
                _jobs.Enqueue(() => StartCoroutine(job));
            }
        }

        public void Enqueue(Action job)
            => Enqueue(GetEnumerator(job));

        IEnumerator GetEnumerator(Action job)
        {
            job();
            yield return null;
        }

        void Awake()
        {
            if (!_instance) {
                _instance = this;
            }
        }

        void OnDestroy()
        {
            _instance = null;
        }

        void Update()
        {
            lock (_jobs) {
                while (_jobs.Count > 0) {
                    _jobs.Dequeue().Invoke();
                }
            }
        }
    }
}