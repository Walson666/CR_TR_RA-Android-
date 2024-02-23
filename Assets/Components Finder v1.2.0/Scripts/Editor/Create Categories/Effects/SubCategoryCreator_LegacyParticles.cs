/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;

namespace ComponentsFinder
{
	public class SubCategoryCreator_LegacyParticles
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
			_FoldoutInfos.Name = "Legacy Particles";
			_FoldoutInfos.SpacesMediumAtEnd = false;

			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
#if UNITY_5 || UNITY_2017_1_OR_NEWER
                //new ComponentInfos (typeof (EllipsoidParticleEmitter)), //obsolete component
				//new ComponentInfos (typeof (MeshParticleEmitter), BoolOption.AddsASpace), //obsolete component
				//new ComponentInfos (typeof (ParticleAnimator)), //obsolete component
				//new ComponentInfos (ComponentsFinderStrings.WorldParticleCollider, BoolOption.AddsASpace),
				//new ComponentInfos (typeof (ParticleRenderer)) //obsolete component
#else
                new ComponentInfos ("EllipsoidParticleEmitter"), //obsolete component
				new ComponentInfos ("MeshParticleEmitter", BoolOption.AddsASpace), //obsolete component
				new ComponentInfos (typeof (ParticleAnimator)), //obsolete component
				new ComponentInfos (ComponentsFinderStrings.WorldParticleCollider, BoolOption.AddsASpace),
                new ComponentInfos (typeof (ParticleRenderer)) //obsolete component
#endif
            }.ToArray ();
		}

		public static void CreateCategory ()
		{
			_FoldoutInfos = SubCategoryCreator.CreateFoldout (_FoldoutInfos);
			StarButtonCreator.onDisableStar += CallDisableStar;
		}

		public static void CallDisableStar (ComponentInfos componentInfos)
		{
			StarButtonCreator.DisableStar (ref _FoldoutInfos._ComponentInfos, componentInfos);
			StarButtonCreator.onDisableStar -= CallDisableStar;
		}
	}
}
