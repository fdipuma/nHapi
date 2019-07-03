using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NHapi.Base
{
	internal class EventMapper
	{
		private Hashtable _map = new Hashtable();
		private static readonly EventMapper _instance = new EventMapper();
		internal bool UseCache { private get; set; }
		private readonly object _lockObj = new object();

		#region Constructors

		static EventMapper()
		{
		}

		private EventMapper()
		{

		}

		private static string RemoveTrailingDot(Hl7Package package)
		{
			string assemblyString = package.PackageName;
			char lastChar = assemblyString.LastOrDefault();
			bool trailingDot = lastChar != default(char) && lastChar.ToString() == ".";
			if (trailingDot)
			{
				assemblyString = assemblyString.Substring(0, assemblyString.Length - 1);
			}
			return assemblyString;
		}

		#endregion

		#region Properties

		public static EventMapper Instance
		{
			get { return _instance; }
		}

		public Hashtable Maps
		{
			get
			{
				CreateCache();
				return _map;
			}
		}

		private void CreateCache()
		{
			if (UseCache) return;

			lock (_lockObj)
			{
				if (UseCache) return;

				_map.Clear();

				var packages = PackageManager.Instance.GetAllPackages();

				foreach (var package in packages)
				{
					Assembly assembly = null;
					try
					{
						var assemblyToLoad = RemoveTrailingDot(package);
						assembly = Assembly.Load(assemblyToLoad);
					}
					catch (FileNotFoundException)
					{
						//Just skip, this assembly is not used
					}

					var structures = new NameValueCollection();
					if (assembly != null)
					{
						structures = GetAssemblyEventMapping(assembly, package);
					}

					_map[package.Version] = structures;
				}

				UseCache = true;
			}
		}

		#endregion

		#region Methods

		private NameValueCollection GetAssemblyEventMapping(Assembly assembly, Hl7Package package)
		{
			NameValueCollection structures = new NameValueCollection();
			using (Stream inResource = assembly.GetManifestResourceStream(package.EventMappingResourceName))
			{
				if (inResource != null)
				{
					using (StreamReader sr = new StreamReader(inResource))
					{
						string line = sr.ReadLine();
						while (line != null)
						{
							if ((line.Length > 0) && ('#' != line[0]))
							{
								string[] lineElements = line.Split(' ', '\t');
								structures.Add(lineElements[0], lineElements[1]);
							}
							line = sr.ReadLine();
						}
					}
				}
			}
			return structures;
		}

		#endregion
	}
}