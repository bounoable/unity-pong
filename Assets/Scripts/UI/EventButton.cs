using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong.UI
{
	[RequireComponent(typeof(UnityEngine.UI.Button))]
	class EventButton: MonoBehaviour
	{
		public event Action Click = delegate {};

		public bool Interactable
		{
			get { return _button.interactable; }
			set
			{
				if (_button) {
					_button.interactable = value;
				}
			}
		}

		UnityEngine.UI.Button _button;

		void Awake()
		{
			_button = GetComponent<UnityEngine.UI.Button>();
			_button.onClick.AddListener(() => Click());
		}
	}
}
