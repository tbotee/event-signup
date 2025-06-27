namespace EventSignup.Types
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string elementType, int id)
			: base($"The {elementType} with ID '{id}' does not exist.")
		{
		}
	}
}
