using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHapi.Base.Parser;

namespace NHapi.Base
{
	public class PackageManager
	{
		private static readonly PackageManager _instance = new PackageManager();
		private List<Hl7Package> _packages = new List<Hl7Package>();

		#region Constructors

		static PackageManager()
		{
		}

		private PackageManager()
		{
			LoadBaseVersions();
		}

		#endregion

		#region Properties

		public static PackageManager Instance
		{
			get { return _instance; }
		}

		internal IList<Hl7Package> GetAllPackages()
		{
			return _packages;
		}

		#endregion

		#region Methods

		private void LoadBaseVersions()
		{
			string[] versions = new string[] { "2.1", "2.2", "2.3", "2.3.1", "2.4", "2.5", "2.5.1", "2.6", "2.7", "2.7.1", "2.8", "2.8.1" };
			foreach (string version in versions)
			{
				string packageName = GetVersionPackageName(version);
				_packages.Add(new Hl7Package(packageName, version));
			}
		}

		/// <summary>
		/// Adds a custom package and version
		/// </summary>
		/// <param name="packageName"></param>
		/// <param name="version"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public void AddCustomVersion(string packageName, string version)
		{
			if (packageName == null) throw new ArgumentNullException(nameof(packageName));
			if (version == null) throw new ArgumentNullException(nameof(version));

			if (_packages.Any(p => p.PackageName == packageName && p.Version == version))
				return;

			_packages.Add(new Hl7Package(packageName, version));

			// invalidate static cache of other components relying on PackageManager
			EventMapper.Instance.UseCache = false;
			DefaultModelClassFactory.UseCache = false;
		}

		internal bool IsValidVersion(string version)
		{
			version = version.ToUpper().Trim();
			foreach (Hl7Package package in _packages)
			{
				if (package.Version.ToUpper().Trim().Equals(version))
					return true;
			}
			return false;
		}

		#endregion

		#region Static GetVersion and GetVersionPackage

		/// <summary> Returns the path to the base package for model elements of the given version
		/// - e.g. "NHapi.Model.VXXX".
		/// This package should have the packages datatype, segment, group, and message
		/// under it. The path ends in with a slash.
		/// </summary>
		internal static String GetVersionPackagePath(String ver)
		{
			StringBuilder path = new StringBuilder("NHapi.Model.V");
			char[] versionChars = new char[ver.Length];
			SupportClass.GetCharsFromString(ver, 0, ver.Length, versionChars, 0);
			for (int i = 0; i < versionChars.Length; i++)
			{
				if (versionChars[i] != '.')
					path.Append(versionChars[i]);
			}
			path.Append("/");
			return path.ToString();
		}


		/// <summary> Returns the package name for model elements of the given version - e.g.
		/// "NHapi.Base.Model.v24.".  This method
		/// is identical to <code>getVersionPackagePath(...)</code> except that path
		/// separators are replaced with dots.
		/// </summary>
		internal static String GetVersionPackageName(String ver)
		{
			String path = GetVersionPackagePath(ver);
			String packg = path.Replace('/', '.');
			packg = packg.Replace('\\', '.');
			return packg;
		}

		#endregion
	}
}