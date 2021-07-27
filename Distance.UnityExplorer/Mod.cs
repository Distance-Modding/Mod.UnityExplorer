using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Logging.Decorators;
using System;
using UnityEngine;
using UnityExplorer;

namespace Distance.ModTemplate
{
	[ModEntryPoint("com.github.sinai-dev/UnityExplorer")]
	public sealed class Mod : MonoBehaviour
	{
		public const string LOG_DECORATOR = "UnityExplorer";

		public static Mod Instance { get; private set; }

		public IManager Manager { get; private set; }

		public Log Logger { get; private set; }

		public ExplorerStandalone UnityExplorer { get; private set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;

			Manager = manager;

			Logger = LogManager.GetForCurrentAssembly();
			Logger.DecorateWith<MessageOutputDecorator>("LOG_DECORATOR");

			UnityExplorer = ExplorerStandalone.CreateInstance();
			ExplorerStandalone.OnLog += OnUnityExplorerLog;
		}

		private void OnUnityExplorerLog(string message, LogType type)
		{
			Action<string> func = null;

			switch (type)
			{
				case LogType.Log:
					func = Logger.Info;
					break;
				case LogType.Warning:
					func = Logger.Warning;
					break;
				case LogType.Error:
				case LogType.Exception:
					func = Logger.Error;
					break;
				case LogType.Assert:
					func = Logger.Debug;
					break;
			}

			func?.Invoke(message);
		}
	}
}
