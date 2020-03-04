using Newtonsoft.Json;
using System.Collections.Generic;

namespace Create_Index_Script_Generator.Models
{
	public class Collection
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("expected_indexes")]
		public List<string> ExpectedIndexes { get; set; }

		[JsonProperty("missed_indexes")]
		public List<string> MissedIndexes { get; set; }

		[JsonProperty("not_expected_indexes")]
		public List<string> NotExpectedIndexes { get; set; }
	}
}
