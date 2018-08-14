using System;
using UnityEngine;

namespace Pong.UI
{
    [RequireComponent(typeof(UnityEngine.UI.InputField))]
    class Input: MonoBehaviour
    {
        public event Action<string> Changed = delegate {};

        public bool Interactable
        {
            get { return _input.interactable; }
            set { _input.interactable = value; }
        }

        public string Value
        {
            get { return _input.text; }
            set { _input.text = value; }
        }

        UnityEngine.UI.InputField _input;

        void Awake()
        {
            _input = GetComponent<UnityEngine.UI.InputField>();
            _input.onValueChanged.AddListener(input => Changed(input));
        }
    }
}