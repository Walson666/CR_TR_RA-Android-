/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;

#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif

namespace ComponentsFinder
{
	public class CategoryCreator_Network 
	{
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4_OR_NEWER
		static FoldoutInfos foldoutInfos;
		public static FoldoutInfos _FoldoutInfos 
		{
			get 
			{
				if (foldoutInfos == null)
				{
					ConfigureCategory ();
				}

				return foldoutInfos;
			}
			set {foldoutInfos = value;}
		}

		public static void ConfigureCategory ()
		{
			_FoldoutInfos = new FoldoutInfos ();
			_FoldoutInfos.Name = ComponentsFinderStrings.Network;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				/*new ComponentInfos (typeof (NetworkAnimator)),

	#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (NetworkDiscovery)),
	#endif

				new ComponentInfos (typeof (NetworkIdentity)),
				new ComponentInfos (typeof (NetworkLobbyManager)),
				new ComponentInfos (typeof (NetworkLobbyPlayer)),
				new ComponentInfos (typeof (NetworkManagerHUD)),

	#if UNITY_5_3 || UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (NetworkMigrationManager)),
	#endif

				new ComponentInfos (typeof (NetworkProximityChecker)),
				new ComponentInfos (typeof (NetworkStartPosition)),
				new ComponentInfos (typeof (NetworkTransform)),

	#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (NetworkTransformChild)),
	#endif

				new ComponentInfos (typeof (NetworkTransformVisualizer))*/
			}.ToArray ();
		}

		public static void CreateCategory ()
		{
			_FoldoutInfos = CategoryCreator.CreateFoldout (_FoldoutInfos);
			StarButtonCreator.onDisableStar += CallDisableStar;
		}

		public static void CallDisableStar (ComponentInfos componentInfos)
		{
			StarButtonCreator.DisableStar (ref _FoldoutInfos._ComponentInfos, componentInfos);
			StarButtonCreator.onDisableStar -= CallDisableStar;
		}
#endif
	}
}
