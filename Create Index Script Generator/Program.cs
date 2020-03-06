using Create_Index_Script_Generator.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Create_Index_Script_Generator
{
	class Program
	{
		static void Main(string[] args)
		{
			List<DataBase> dataBases = new List<DataBase>();
			List<Index> missedIndexes = new List<Index>();
			List<Index> notExpectedIndexes = new List<Index>();
			var dbName = "DB01";
			//var dbName = "DB02";
			//var dbName = "DB03";
			//var dbName = "DB04";
			//var dbName = "DBRui";

			using (StreamReader r = new StreamReader($"../../../JSON/indexes{dbName}.json"))
			{
				string json = r.ReadToEnd();
				dataBases = JsonConvert.DeserializeObject<List<DataBase>>(json);
			}

			foreach (DataBase db in dataBases)
			{
				foreach (Collection collection in db.Collections)
				{
					if (collection.MissedIndexes.Any())
					{
						foreach (string indexName in collection.MissedIndexes)
							missedIndexes.Add(new Index(collection.Name, indexName, db.DataBaseName, db.CompanyName));
					}

					if (collection.NotExpectedIndexes.Any())
					{
						foreach (string indexName in collection.NotExpectedIndexes)
							notExpectedIndexes.Add(new Index(collection.Name, indexName, db.DataBaseName, db.CompanyName));
					}
				}
			}

			var missedIndexesGrouped = missedIndexes.GroupBy(index => index.DataBaseName);
			var notExpectedGrouped = notExpectedIndexes.GroupBy(index => new { index.CollectionName, index.Name });
			var indexesToAddOnInstalattion = notExpectedGrouped.Select(index => index.Key.CollectionName);

			var scriptStringBuilder = new StringBuilder();

			if (missedIndexes.Any())
			{
				scriptStringBuilder.AppendLine("var database = null;");
				scriptStringBuilder.AppendLine("");
				foreach (var group in missedIndexesGrouped)
				{
					scriptStringBuilder.AppendLine($"database = db.getMongo().getDB('{group.Key}');");
					foreach (var index in group)
					{
						scriptStringBuilder.AppendLine($"database.getCollection('{index.CollectionName}').createIndex({{ {index.Name}: 1 }},{{ background: true}})");
					}
					scriptStringBuilder.AppendLine("");
				}
			}

			if (notExpectedIndexes.Any())
			{
				scriptStringBuilder.AppendLine("");
				scriptStringBuilder.AppendLine("//Indexes para adicionar no arquivo de instalação de bases do Zen");
				foreach (var group in notExpectedGrouped)
				{
					scriptStringBuilder.AppendLine($"//CreateIndex('{group.Key.CollectionName}', '{group.Key.Name}');");
				}
			}

			var scriptString = scriptStringBuilder.ToString();

			using (FileStream fs = File.Create($"../../../Result Script/insertIndexes{dbName}.js"))
			{
				byte[] info = new UTF8Encoding(true).GetBytes(scriptString);
				fs.Write(info, 0, info.Length);
			}
		}
	}


}
