using UnityEngine;

namespace Notes
	{
	[AddComponentMenu ("Note")]
	public class Note:MonoBehaviour
		{
		private void OnEnable()
			{
			Destroy (this);
			}
		}
	}