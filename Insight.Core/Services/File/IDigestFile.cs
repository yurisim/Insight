
namespace Insight.Core.Services.File
{
	public interface IDigest
	{
		/// <summary>
		/// Represents order in which files should be digested. Lower is higher priority
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// Cleans FileContents (declared in AbstractDigest)
		/// </summary>
		void CleanInput();

		/// <summary>
		/// Given clean input, digests the input and adds the persons/person's data to database. Clean input is only person data, no extraneous information
		/// </summary>
		void DigestLines();
	}
}
