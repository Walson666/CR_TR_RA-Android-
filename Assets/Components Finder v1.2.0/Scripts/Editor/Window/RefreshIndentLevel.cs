/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace ComponentsFinder
{
	public class RefreshIndentLevel 
	{
		public static void Refresh (int i)
		{
			EditorGUI.indentLevel += i;
		}

		public static void Add ()
		{
			Refresh (1);
		}

		public static void Remove ()
		{
			Refresh (-1);
		}
	}
}
