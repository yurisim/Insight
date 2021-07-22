using Insight.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insight.Core.Services.File
{
	public interface IDigest
	{
		int Priority {  get; }

		void DigestLines();
	}
}
