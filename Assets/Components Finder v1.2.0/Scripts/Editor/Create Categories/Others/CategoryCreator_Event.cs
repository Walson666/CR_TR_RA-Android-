/*Script created by Pierre Stempin*/

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ComponentsFinder
{
	public class CategoryCreator_Event 
	{
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
			_FoldoutInfos.Name = ComponentsFinderStrings.Event;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> ()
			{
				new ComponentInfos (typeof (EventSystem)),
				new ComponentInfos (typeof (EventTrigger)),

#if UNITY_5_5_OR_NEWER
				//new ComponentInfos (typeof (HoloLensInputModule)),
#endif

				new ComponentInfos (typeof (Physics2DRaycaster)),
				new ComponentInfos (typeof (PhysicsRaycaster), ComponentsFinderStrings._Physics3DRaycaster),
				new ComponentInfos (typeof (StandaloneInputModule)),
				new ComponentInfos (typeof (TouchInputModule)), //obsolete component
				new ComponentInfos (typeof (GraphicRaycaster))
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
	}
}
#endif
