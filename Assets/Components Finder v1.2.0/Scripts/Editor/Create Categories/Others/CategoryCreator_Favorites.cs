/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;

namespace ComponentsFinder
{
	public class CategoryCreator_Favorites
	{
		static bool isFoldout = true;
		public static List <ComponentInfos> _ComponentInfos = new List <ComponentInfos> ();

		static Favorites favorites;
		public static Favorites _Favorites 
		{
			get 
			{
				if (favorites == null)
				{
					ConfigureCategory ();
				}

				return favorites;
			}
			set {favorites = value;}
		}

		public static void ConfigureCategory ()
		{
			string fileName = "Favorites";
			favorites = Resources.Load <Favorites> (fileName) as Favorites;
		}

		public static void CreateCategory ()
		{
			if (_Favorites != null)
			{
				_ComponentInfos = _Favorites.FavoritesComponentInfosList;
			}

			string categoryName = ComponentsFinderStrings.Favorites;
			FoldoutCreator.CreateFoldout (ref isFoldout, categoryName);

			if (isFoldout) 
			{
				for (int i = 0; i < _ComponentInfos.Count; i ++) 
				{
					GUILayout.BeginHorizontal ();
					StarButtonCreator.CreateStarButton_FavoriteCategory (_ComponentInfos [i]);

					//ComponentInfosList.Count can change during the CreateStarFavoriteButton method
					if (i < _ComponentInfos.Count)
					{
						ButtonCreator.CreateComponentButton (_ComponentInfos [i]);
					}

					GUILayout.EndHorizontal ();
				}

				GUILayout.Space (WindowValues.ShortSpacingHeight);
			}
		}

		public static void AddComponent (ComponentInfos componentInfos)
		{
			_ComponentInfos.Add (componentInfos);
			Save ();
		}

		public static void RemoveComponent (ComponentInfos componentInfos)
		{
			for (int i = 0; i < _ComponentInfos.Count; i ++) 
			{
				if (componentInfos.ScriptName == _ComponentInfos [i].ScriptName)
				{
					_ComponentInfos.RemoveAt (i);
					Save ();
				}
			}
		}

		public static void Save ()
		{
			if (_Favorites != null)
			{
				_Favorites.FavoritesComponentInfosList = _ComponentInfos;
			}
		}
	}
}
