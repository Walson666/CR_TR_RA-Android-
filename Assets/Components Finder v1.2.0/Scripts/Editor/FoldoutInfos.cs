/*Script created by Pierre Stempin*/

using UnityEngine;
using System;
using System.Collections.Generic;

namespace ComponentsFinder
{
	 [Serializable]
	public class FoldoutInfos 
	{
		public string Name;
		public bool IsFoldout = true;
		public bool SpacesMediumAtEnd = true;
		public ComponentInfos [] _ComponentInfos;
		public FoldoutInfos [] _FoldoutInfos;

		//GUI Layout
		public float CurrentCategoryHeight;
		public float CategoryHeightFold;
		public float CategoryHeightFoldout;

		public FoldoutInfos () 
		{
			
		}

		public FoldoutInfos (string name)
		{
			Name = name;
		}
	}
}
