using Distance.UnityExplorer.Decorators;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Logging.Base;
using Reactor.API.Logging.Decorators;
using Reactor.API.Storage;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityExplorer;

namespace Distance.UnityExplorer
{
	[ModEntryPoint("com.github.sinai-dev/UnityExplorer")]
	public sealed class Mod : MonoBehaviour
	{
		public static Mod Instance { get; private set; }

		public IManager Manager { get; private set; }

		public Log Logger { get; private set; }

		public ExplorerStandalone UnityExplorer { get; private set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;
			Logger = CreateLog();

			SetUEWorkingDirectory();

			UnityExplorer = ExplorerStandalone.CreateInstance(OnUnityExplorerLog);
		}

		private void SetUEWorkingDirectory()
		{
			DirectoryInfo directory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;

			// Set UnityExplorer root path
			// private static string ExplorerStandalone.s_explorerFolder
			FieldInfo s_explorerFolder = HarmonyLib.AccessTools.Field(typeof(ExplorerStandalone), "s_explorerFolder");
			s_explorerFolder.SetValue(null, directory.FullName);
		}

		private Log CreateLog()
		{
			Log log = LogManager.GetForCurrentAssembly();

			log.WithOutputTemplate("[{Time} {LogLevel}] [{LogName}] {Message}");

			// Add a prefix to the UnityExplorer log lines in the console so they can be found more easily
			Decorator dateTime = new DateTimeDecorator();
			Decorator coloredDateTime = new DecoratorColorator(dateTime, 111, 193, 209);

			Decorator logName = new LabelDecorator("Unity Explorer", 111, 193, 209);
			log.DecorateWith(logName, "LogName");
			log.DecorateWith(coloredDateTime, "Time");

			return log;
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
