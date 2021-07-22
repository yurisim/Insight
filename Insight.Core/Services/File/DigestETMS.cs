using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Insight.Core.Helpers;
using Insight.Core.Models;
using Insight.Core.Properties;
using Insight.Core.Services.Database;

namespace Insight.Core.Services.File
{
	public class DigestETMS : IDigest
	{
		int IDigest.Priority { get => 3; }

		private readonly IList<string> FileContents = new List<string>();

		public DigestETMS(IList<string> input)
		{
			this.FileContents = input;
		}

		public void DigestLines()
		{

			var person = InsightController.GetPersonByName("", "");

			if (person == null)
			{
			}

			InsightController.Add(person);
		}
	}
}
