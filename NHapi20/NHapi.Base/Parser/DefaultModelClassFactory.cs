/*
* Created on 21-Apr-2005
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NHapi.Base.Model;
using NHapi.Base.Log;

namespace NHapi.Base.Parser
{
	/// <summary> Default implementation of ModelClassFactory.  See PackageList() for configuration instructions. 
	/// 
	/// </summary>
	/// <author>  <a href="mailto:bryan.tripp@uhn.on.ca">Bryan Tripp</a>
	/// </author>
	/// <version>  $Revision: 1.2 $ updated on $Date: 2005/05/24 18:15:39 $ by $Author: bryan_tripp $
	/// </version>
	public class DefaultModelClassFactory : IModelClassFactory
	{
		private static readonly object LockObject = new object();
		private static readonly IHapiLog Log;
		private static Dictionary<string, List<string>> _globalPackages = null;
		internal static bool UseCache { get; set; }

		/// <summary> <p>Attempts to return the message class corresponding to the given name, by 
		/// searching through default and user-defined (as per PackageList()) packages. 
		/// Returns GenericMessage if the class is not found.</p>
		/// <p>It is important to note that there can only be one implementation of a particular message 
		/// structure (i.e. one class with the message structure name, regardless of its package) among 
		/// the packages defined as per the <code>PackageList()</code> method.  If there are duplicates 
		/// (e.g. two ADT_A01 classes) the first one in the search order will always be used.  However, 
		/// this restriction only applies to message classes, not (normally) segment classes, etc.  This is because 
		/// classes representing parts of a message are referenced explicitly in the code for the message 
		/// class, rather than being looked up (using findMessageClass() ) based on the String value of MSH-9. 
		/// The exception is that Segments may have to be looked up by name when they appear 
		/// in unexpected locations (e.g. by local extension) -- see findSegmentClass().</p>  
		/// <p>Note: the current implementation will be slow if there are multiple user-
		/// defined packages, because the JVM will try to load a number of non-existent 
		/// classes every parse.  This should be changed so that specific classes, rather 
		/// than packages, are registered by name.</p>
		/// 
		/// </summary>
		/// <param name="theName">name of the desired structure in the form XXX_YYY
		/// </param>
		/// <param name="theVersion">HL7 version (e.g. "2.3")  
		/// </param>
		/// <param name="isExplicit">true if the structure was specified explicitly in MSH-9-3, false if it 
		/// was inferred from MSH-9-1 and MSH-9-2.  If false, a lookup may be performed to find 
		/// an alternate structure corresponding to that message type and event.   
		/// </param>
		/// <returns> corresponding message subclass if found; GenericMessage otherwise
		/// </returns>
		public virtual Type GetMessageClass(string theName, string theVersion, bool isExplicit)
		{
			Type mc = null;
			if (!isExplicit)
			{
				theName = ParserBase.GetMessageStructureForEvent(theName, theVersion);
			}
			mc = FindClass(theName, theVersion, ClassType.Message);
			if (mc == null)
				mc = GenericMessage.getGenericMessageClass(theVersion);
			return mc;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theName"></param>
		/// <param name="theVersion"></param>
		/// <returns></returns>
		public virtual Type GetGroupClass(string theName, string theVersion)
		{
			return FindClass(theName, theVersion, ClassType.Group);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theName"></param>
		/// <param name="theVersion"></param>
		/// <returns></returns>
		public virtual Type GetSegmentClass(string theName, string theVersion)
		{
			return FindClass(theName, theVersion, ClassType.Segment);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theName"></param>
		/// <param name="theVersion"></param>
		/// <returns></returns>
		public virtual Type GetTypeClass(string theName, string theVersion)
		{
			return FindClass(theName, theVersion, ClassType.Datatype);
		}

		/// <summary> <p>Lists all the packages (user-definable) where classes for standard and custom 
		/// messages may be found.  Each package has subpackages called "message", 
		/// "group", "segment", and "datatype" in which classes for these message elements 
		/// can be found. </p> 
		/// <p>At a minimum, this method returns the standard package for the 
		/// given version.  For example, for version 2.4, the package list contains <code>
		/// NHapi.Base.Model.v24</code>.  In addition, user-defined packages may be specified
		/// for custom messages.</p>
		/// <p>If you define custom message classes, and want Parsers to be able to 
		/// find them, you must register them as follows (otherwise you will get an exception when 
		/// the corresponding messages are parsed).  For each HL7 version you want to support, you must 
		/// put a text file on your classpath, under the folder /custom_packages, named after the version.  For example, 
		/// for version 2.4, you might put the file "custom_packages/2.4" in your application JAR.  Each line in the  
		/// file should name a package to search for message classes of that version.  For example, if you 
		/// work at foo.org, you might create a v2.4 message structure called "ZFO" and define it in the class
		/// <code>org.foo.hl7.custom.message.ZFO<code>.  
		/// In order for parsers to find this message
		/// class, you would need to enter the following line in custom_packages/2.4:
		/// <p>org.foo.hl7.custom</p>
		/// <p>Packages are searched in the order specified.  The standard package for a given version
		/// is searched last, allowing you to override the default implementation.  Please note that 
		/// if you create custom classes for messages, segments, etc., their names must correspond exactly 
		/// to their names in the message text.  For example, if you subclass the QBP segment in order to 
		/// add your own fields, your subclass must also be called QBP. although it will obviously be in 
		/// a different package.  To make sure your class is used instead of the default implementation, 
		/// put your package in the package list.  User-defined packages are searched first, so yours 
		/// will be found first and used.  </p>
		/// <p>It is important to note that there can only be one implementation of a particular message 
		/// structure (i.e. one class with the message structure name, regardless of its package) among 
		/// the packages defined as per the <code>PackageList()</code> method.  If there are duplicates 
		/// (e.g. two ADT_A01 classes) the first one in the search order will always be used.  However, 
		/// this restriction only applies to message classes, not segment classes, etc.  This is because 
		/// classes representing parts of a message are referenced explicitly in the code for the message 
		/// class, rather than being looked up (using findMessageClass() ) based on the String value of MSH-9.<p>  
		/// </summary>
		public IEnumerable<string> PackageList(string version)
		{
			//load package lists if necessary ... 

			if (_globalPackages == null || !UseCache)
			{
				lock (LockObject)
				{
					if (_globalPackages == null || !UseCache)
					{
						_globalPackages = PackageManager.Instance
							.GetAllPackages()
							.GroupBy(p => p.Version)
							.ToDictionary(g => g.Key, g => g.Select(p => p.PackageName).ToList());
					}
				}
			}

			if (!_globalPackages.TryGetValue(version, out var global))
				throw new Exception($"Package '{version}' could not be found");
			
			return global;
		}
		
		/// <summary> Finds a message or segment class by name and version.</summary>
		/// <param name="name">the segment or message structure name 
		/// </param>
		/// <param name="version">the HL7 version
		/// </param>
		/// <param name="type">'message', 'group', 'segment', or 'datatype'  
		/// </param>
		private Type FindClass(string name, string version, ClassType type)
		{
			if (ParserBase.ValidVersion(version) == false)
			{
				throw new HL7Exception("The HL7 version " + version + " is not recognized", HL7Exception.UNSUPPORTED_VERSION_ID);
			}

			//get list of packages to search for the corresponding message class 
			var packages = PackageList(version).ToList();

			//get subpackage for component type
			string typeString = type.ToString();
			string subpackage = typeString.Substring(0, 1).ToUpper() + typeString.Substring(1);

			//try to load class from each package
			Type compClass = null;
			int c = 0;
			while (compClass == null && c < packages.Count)
			{
				try
				{
					string p = packages[c];
					if (!p.EndsWith("."))
						p = p + ".";
					string classNameToTry = p + subpackage + "." + name;

					classNameToTry = AddAssemblyName(p, classNameToTry);
					if (Log.DebugEnabled)
					{
						Log.Debug("Trying to load: " + classNameToTry);
					}
					compClass = Type.GetType(classNameToTry);
					if (Log.DebugEnabled)
					{
						Log.Debug("Loaded: " + classNameToTry + " class: " + compClass);
					}
				}
				catch (Exception)
				{
					/* just try next one */
				}
				c++;
			}
			return compClass;
		}

		/// <summary>
		/// Add the assembly name to the class name.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="classNameToTry"></param>
		/// <returns>Assembly name qualified name</returns>
		private static string AddAssemblyName(string p, string classNameToTry)
		{
			// TODO: pull this information out of the config file
			// have to add assembly name since models are broken out into separate assemblies
			//

			string assemblyName =
				classNameToTry += ", " + p.Substring(0, p.Length - 1);
			return classNameToTry;
		}


		static DefaultModelClassFactory()
		{
			Log = HapiLogFactory.GetHapiLog(typeof(DefaultModelClassFactory));
		}
	}
}