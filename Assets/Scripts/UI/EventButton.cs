﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong.UI
{
	[RequireComponent(typeof(UnityEngine.UI.Button))]
	class EventButton: MonoBehaviour
	{
		public event Action Click = delegate {};

		UnityEngine.UI.Button _button;

		void Awake()
		{
			_button = GetComponent<UnityEngine.UI.Button>();
			_button.onClick.AddListener(() => Click());
		}
	}
}
