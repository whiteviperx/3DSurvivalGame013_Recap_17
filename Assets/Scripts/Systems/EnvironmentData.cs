using System.Collections.Generic;

using NUnit.Framework;

[System.Serializable]

public class EnvironmentData
	{

	public List<string> pickedupItems;
	public EnvironmentData(List<string> _pickedupItems)
		{
		pickedupItems = _pickedupItems;

		}
	}