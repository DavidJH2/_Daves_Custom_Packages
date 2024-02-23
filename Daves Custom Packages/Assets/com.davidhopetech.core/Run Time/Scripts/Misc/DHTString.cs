public class DHTString
{
	public string Value { get; private set; }

	public DHTString()
	{
		Value = string.Empty;
	}

	public DHTString(string initialValue)
	{
		Value = initialValue;
	}

	// Overload the + operator to concatenate DHTString instances
	public static DHTString operator +(DHTString a, DHTString b)
	{
		return new DHTString(a.Value + b.Value);
	}

	// Overload the == operator to compare DHTString instances
	public static bool operator ==(DHTString a, DHTString b)
	{
		return a.Value == b.Value;
	}

	// Overload the != operator to compare DHTString instances
	public static bool operator !=(DHTString a, DHTString b)
	{
		return a.Value != b.Value;
	}

	// Override the Equals method
	public override bool Equals(object obj)
	{
		if (obj is DHTString)
		{
			return this.Value == ((DHTString)obj).Value;
		}
		return false;
	}

	// Override the GetHashCode method
	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}

	// Override the ToString method to return the string value
	public override string ToString()
	{
		return Value;
	}
}