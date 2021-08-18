using System;
using System.Collections.Generic;
using System.Text;

namespace Insight.Core.Models
{
	/// <summary>
	/// This interface is used so that we can flag which models can be searched with generic methods
	/// </summary>
	public interface IGenericable
	{
		int Id { get; }
	}
}
