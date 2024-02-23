/*Script created by Pierre Stempin*/

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace ComponentsFinder
{
	public class StarButtonCreator 
	{
		public static Action <ComponentInfos> onDisableStar;

		static string star = ComponentsFinderStrings.Star;
		static string enabledStarFileName = ComponentsFinderStrings.Enabled + ComponentsFinderStrings._Space + star;
		static string disabledStarFileName = ComponentsFinderStrings.Disabled + ComponentsFinderStrings._Space + star;
		static string iconsFolderName = ComponentsFinderStrings.Icons;
		static string currentStarFileName;
		static Texture currentStarIcon;

		public static void CreateStarButton (ref ComponentInfos componentInfos)
		{
			RefreshCurrentStarIcon (componentInfos.IsFavorite);

			if (currentStarIcon != null)
			{
				if (GUILayout.Button (currentStarIcon))
				{
					//Press the button
					componentInfos.IsFavorite = !componentInfos.IsFavorite;
					SaveData (componentInfos.ScriptName, componentInfos.IsFavorite);

					if (componentInfos.IsFavorite)
					{
						CategoryCreator_Favorites.AddComponent (componentInfos);
					}
					else
					{
						CategoryCreator_Favorites.RemoveComponent (componentInfos);
					}
				}
			}
		}
			
		public static void CreateStarButton_FavoriteCategory (ComponentInfos componentInfos)
		{
			RefreshCurrentStarIcon (true);

			if (currentStarIcon != null)
			{
				if (GUILayout.Button (currentStarIcon))
				{
					if (componentInfos.IsFavorite)
					{
						CategoryCreator_Favorites.RemoveComponent (componentInfos);

						if (onDisableStar != null)
						{
							onDisableStar (componentInfos);
						}
					}
				}
			}
		}

		public static void DisableStar (ref ComponentInfos [] componentInfosArray, ComponentInfos componentInfos)
		{
			for (int i = 0; i < componentInfosArray.Length; i ++) 
			{
				if (componentInfos.ScriptName == componentInfosArray [i].ScriptName)
				{
					componentInfosArray [i].IsFavorite = false;
					SaveData (componentInfosArray [i].ScriptName, componentInfosArray [i].IsFavorite);
				}
			}
		}

		private static void SaveData (string key, bool value)
		{
			EditorPrefs.SetBool (key, value);
		}

		public static void RefreshCurrentStarIcon (bool isEnabled)
		{
			currentStarFileName = isEnabled ? enabledStarFileName : disabledStarFileName;
			currentStarIcon = Resources.Load (iconsFolderName + ComponentsFinderStrings.Slash + currentStarFileName) as Texture;
		}
	}
}
