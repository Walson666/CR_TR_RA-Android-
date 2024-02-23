/*Script created by Pierre Stempin*/

using UnityEngine;
using System;
using System.Collections.Generic;

namespace ComponentsFinder
{
	public class RefreshComponentsInfosList 
	{
		public static void AddComponentInfos (ref List <ComponentInfos> componentInfosList, Type T)
		{
			ComponentInfos componentInfos = new ComponentInfos (T);
			componentInfosList.Add (componentInfos);
		}

		public static void AddComponentInfos (ref List <ComponentInfos> componentInfosList, Type T, bool adds3D)
		{
			ComponentInfos componentInfos = new ComponentInfos (T, BoolOption.Adds3D);
			componentInfosList.Add (componentInfos);
		}

		public static void AddComponentInfos (ref List <ComponentInfos> componentInfosList, Type T, string displayedName)
		{
			ComponentInfos componentInfos = new ComponentInfos (T, displayedName);
			componentInfosList.Add (componentInfos);
		}

		public static void AddComponentInfos (ref List <ComponentInfos> componentInfosList, string displayedName)
		{
			ComponentInfos componentInfos = new ComponentInfos (displayedName);
			componentInfosList.Add (componentInfos);
		}
	}
}
