/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
using UnityEngine.EventSystems;
#endif

namespace ComponentsFinder
{
	public class SubCategoryCreator_UserScripts
	{
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

		static ComponentInfos [] currentComponentInfosArray;

		static bool [] userSubcategory_isFoldout;

		public static void CreateUserScriptsCategory ()
		{
			if (_Favorites != null && _Favorites.UserScriptsComponentInfosList == null)
			{
				_Favorites.UserScriptsComponentInfosList = new List <ComponentInfos> ();
			}

			UserScriptsGetter.GetScripts ();
			CreateAllUserScriptsSubFoldouts ();
			StarButtonCreator.onDisableStar += CallDisableStar;
		}

		static void CreateAllUserScriptsSubFoldouts ()
		{
			userSubcategory_isFoldout = new bool [UserScriptsGetter.Namespaces.Count];

			for (int namespaceID = 0; namespaceID < UserScriptsGetter.Namespaces.Count; namespaceID ++) 
			{
				CreateUserScriptsSubFoldout (namespaceID);
			}
		}

		static void CreateUserScriptsSubFoldout (int namespaceID)
		{
			string userSubcategoryName = UserScriptsGetter.Namespaces [namespaceID];

			if (userSubcategoryName == null)
			{
				userSubcategoryName = "No Namespace";
			}

			userSubcategory_isFoldout [namespaceID] = true;
			FoldoutCreator.CreateFoldout (ref userSubcategory_isFoldout [namespaceID], userSubcategoryName);

			if (userSubcategory_isFoldout [namespaceID]) 
			{
				CreateUserScriptsSubFoldoutContent (namespaceID);
#if UNITY_5_4_OR_NEWER
				GUILayout.Space (WindowValues.ShortSpacingHeight);
#endif
			}
		}

		static void CreateUserScriptsSubFoldoutContent (int namespaceID)
		{
			for (int scriptID = 0; scriptID < UserScriptsGetter.Scripts.Count; scriptID ++) 
			{
				if (UserScriptsGetter.Namespaces [namespaceID] == UserScriptsGetter.Scripts [scriptID].Namespace)
				{
					string name = UserScriptsGetter.Scripts [scriptID].Name;
					ComponentInfos componentInfos = new ComponentInfos (name);

					if (_Favorites != null)
					{
						for (int i = 0; i < _Favorites.UserScriptsComponentInfosList.Count; i++) 
						{
							bool b = _Favorites.UserScriptsComponentInfosList [i].DisplayedName == componentInfos.DisplayedName;
							
							if (b)
							{
								componentInfos.IsFavorite = _Favorites.UserScriptsComponentInfosList [i].IsFavorite;
							}
						}
						
						GUILayout.BeginHorizontal ();
						StarButtonCreator.CreateStarButton (ref componentInfos);
						ButtonCreator.CreateComponentButton (componentInfos);
						GUILayout.EndHorizontal ();
						
						if (_Favorites.UserScriptsComponentInfosList != null && _Favorites.UserScriptsComponentInfosList.Count != 0)
						{
							bool isAlreadyThere = false;
							
							for (int i = 0; i < _Favorites.UserScriptsComponentInfosList.Count; i++) 
							{
								bool b = _Favorites.UserScriptsComponentInfosList [i].DisplayedName == componentInfos.DisplayedName;
								
								if (b)
								{
									isAlreadyThere = true;
								}
							}
							
							if (!isAlreadyThere)
							{
								_Favorites.UserScriptsComponentInfosList.Add (componentInfos);
							}
						}
						else
						{
							_Favorites.UserScriptsComponentInfosList.Add (componentInfos);
						}
						
						for (int i = 0; i < _Favorites.UserScriptsComponentInfosList.Count; i++) 
						{
							bool b = _Favorites.UserScriptsComponentInfosList [i].DisplayedName == componentInfos.DisplayedName;
							
							if (b)
							{
								_Favorites.UserScriptsComponentInfosList [i].IsFavorite = componentInfos.IsFavorite;
							}
						}
					}
				}
			}
		}

		public static void CallDisableStar (ComponentInfos componentInfos)
		{
            if (_Favorites != null) 
            {
			    currentComponentInfosArray = _Favorites.UserScriptsComponentInfosList.ToArray ();
			    StarButtonCreator.DisableStar (ref currentComponentInfosArray, componentInfos);
			    _Favorites.UserScriptsComponentInfosList = new List <ComponentInfos> ();

			    for (int i = 0; i < currentComponentInfosArray.Length; i ++) 
			    {
				    _Favorites.UserScriptsComponentInfosList.Add (currentComponentInfosArray [i]);
			    }

			    StarButtonCreator.onDisableStar -= CallDisableStar;
            }
		}
	}
}
