/*Script created by Pierre Stempin*/

using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace ComponentsFinder
{
	/// <summary>
	/// Get all scripts present in the project that can be added on the scene, 
	/// stock it in a list, 
	/// in order to add them in the Components Finder window
	/// </summary>
	public class UserScriptsGetter
	{
		public static List <Type> Scripts;
		public static List <string> Namespaces;

		public static void GetScripts ()
		{
			Scripts = new List <Type> ();
			Namespaces = new List <string> ();

			foreach (Assembly assemmbly in AppDomain.CurrentDomain.GetAssemblies ())
			{
				if (assemmbly.GetName ().Name != ComponentsFinderStrings.AssemblyCSharp) 
				{
					continue;
				}

				foreach (Type T in assemmbly.GetTypes ())
				{
					if (typeof (MonoBehaviour).IsAssignableFrom (T))
					{
						Scripts.Add (T);
						bool nameSpaceIsUsed = false;

						for (int i = 0; i < Namespaces.Count; i ++) 
						{
							if (T.Namespace == Namespaces [i])
							{
								nameSpaceIsUsed = true;
								break;
							}
						}

						if (!nameSpaceIsUsed)
						{
							Namespaces.Add (T.Namespace);
						}
					}
				}
			}

			//Sort namespaces by alphabetical order
			Namespaces.Sort ();
		}
	}
}
