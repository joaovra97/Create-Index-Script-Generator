using Newtonsoft.Json;
using System.Collections.Generic;

namespace Create_Index_Script_Generator.Models
{
	public class DataBase
	{
		[JsonProperty("db")]
		public string DataBaseName { get; set; }
		
		[JsonProperty("company_name")]
		public string CompanyName { get; set; }

		[JsonProperty("federal_registration")]
		public string FederalRegistration { get; set; }

		[JsonProperty("collections")]
		public List<Collection> Collections { get; set; }
	}	
}
