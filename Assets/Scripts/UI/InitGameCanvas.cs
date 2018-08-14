using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityInput = UnityEngine.UI.InputField;

namespace Pong.UI
{
	class InitGameCanvas: MonoBehaviour
	{
		public bool IsEnabled
		{
			get
			{
				return gameObject.activeSelf;
			}
			set
			{
				gameObject.SetActive(value);
			}
		}

		[SerializeField] EventButton _backBtn;
		[SerializeField] protected EventButton _startBtn;
		[SerializeField] protected Text _messageText;

		[SerializeField] protected UnityInput _ipInput;
        [SerializeField] protected UnityInput _tcpPortInput;
        [SerializeField] protected UnityInput _udpPortInput;

		virtual protected void Awake()
		{
			if (!(_backBtn && _startBtn && _messageText && _ipInput && _tcpPortInput && _udpPortInput)) {
				Destroy(this.gameObject);
			}

			_backBtn.Click += () => IsEnabled = false;
		}
	}
}
