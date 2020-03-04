using System;
using System.Collections.Generic;
using System.Text;

namespace Create_Index_Script_Generator.Models
{
	public class Index
	{
		public string CollectionName { get; set; }

		public string Name { get; set; }

		public string DataBaseName { get; set; }

		public string CompanyName { get; set; }

		public Index(string collectionName, string name)
		{
			CollectionName = collectionName;
			Name = name;
		}

		public Index(string collectionName, string name, string databaseName, string companyName)
		{
			CollectionName = collectionName;
			Name = name;
			DataBaseName = databaseName;
			CompanyName = companyName;
		}
	}
}
