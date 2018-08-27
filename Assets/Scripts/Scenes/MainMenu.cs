using Pong.UI;
using Pong.Core;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pong.Scenes
{
	class MainMenu: MonoBehaviour
	{
		[SerializeField] EventButton _startServerBtn;
		[SerializeField] EventButton _startClientBtn;
		[SerializeField] EventButton _quitBtn;

		[SerializeField] InitGameCanvas _startServerCanvas;
		[SerializeField] InitGameCanvas _startClientCanvas;

		void Awake()
		{
			if (!(_startServerBtn && _startClientBtn && _quitBtn && _startServerCanvas && _startClientCanvas)) {
				Destroy(gameObject);
				return;
			}

			_startServerBtn.Click += () => _startServerCanvas.IsEnabled = true;
			_startClientBtn.Click += () => _startClientCanvas.IsEnabled = true;
			_quitBtn.Click += Application.Quit;

			Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
		}
	}
}
