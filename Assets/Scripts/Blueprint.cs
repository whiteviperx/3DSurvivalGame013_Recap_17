public class Blueprint

// public class Blueprint : MonoBehaviour
	{
	public string itemName;

	public string Req1;

	public string Req2;

	// public string Req3;

	public int Req1amount;

	public int Req2amount;

	// public int Req3amount;

	public int numOfRequirements;

	public int numberOfItemsToProduce;

	public Blueprint(string name, int producedItems, int reqNUM, string R1, int R1num, string R2, int R2num)

		{
		itemName = name;

		numOfRequirements = reqNUM;

		numberOfItemsToProduce = producedItems;

		Req1 = R1;
		Req2 = R2;

		// Req3 = R3;

		Req1amount = R1num;
		Req2amount = R2num;

		// Req3amount = R3num;
		}
	}