/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;

namespace ComponentsFinder
{
	public class Favorites : ScriptableObject
	{
		public List <ComponentInfos> FavoritesComponentInfosList = new List<ComponentInfos> ();
		public List <ComponentInfos> UserScriptsComponentInfosList;
	}
}
