/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace ComponentsFinder
{
	public class CategoryCreator_Effects 
	{
		public static Action onCreateSubCategory;

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
			SetSubcategory ();

			_FoldoutInfos = new FoldoutInfos ();
			_FoldoutInfos.Name = ComponentsFinderStrings.Effects;

			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (ParticleSystem)),
				new ComponentInfos (typeof (TrailRenderer)),
				new ComponentInfos (typeof (LineRenderer)),
				new ComponentInfos (typeof (LensFlare)),
				new ComponentInfos (ComponentsFinderStrings._Halo),
				new ComponentInfos (typeof (Projector), BoolOption.AddsASpace),
				new ComponentInfos (onCreateSubCategory)
			}.ToArray ();
		}

		private static void SetSubcategory ()
		{
			if (onCreateSubCategory == null)
			{
				onCreateSubCategory += SubCategoryCreator_LegacyParticles.CreateCategory;
			}
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
