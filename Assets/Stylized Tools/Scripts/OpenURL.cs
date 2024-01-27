using UnityEngine;

namespace StylizedTools
	{
	public class OpenURL:MonoBehaviour
		{
		[SerializeField] private string url;

		public void _OnURLOpen()
			{
			Application.OpenURL (url);
			}
		}
	}