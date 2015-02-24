namespace Oxide.Ext.BriansMod
{
	using System.IO;
	using System.Linq;
	using System.Text;
	using Newtonsoft.Json;
	using UnityEngine;

	public class MapData
	{
		public static string GetMonuments()
		{
			var sb = new StringBuilder();
			using (var writer = new JsonTextWriter(new StringWriter(sb)))
			{
				writer.Formatting = (Formatting) 1;
				writer.WriteStartArray();
				foreach (var gameObject in GameObjectsByName("autospawn/monument/", true))
				{
					writer.WriteStartObject();
					writer.WritePropertyName("name");
					writer.WriteValue(gameObject.name.Substring(19));
					writer.WritePropertyName("position");
					writer.WriteStartObject();
					writer.WritePropertyName("x");
					writer.WriteValue(gameObject.transform.position.x);
					writer.WritePropertyName("y");
					writer.WriteValue(gameObject.transform.position.y);
					writer.WritePropertyName("z");
					writer.WriteValue(gameObject.transform.position.z);
					writer.WriteEndObject();
					writer.WritePropertyName("rotation");
					var rotation = gameObject.transform.rotation;
					var num = (int) @rotation.eulerAngles.y;
					writer.WriteValue(num);
					writer.WriteEndObject();
				}
				writer.WriteEndArray();
				return sb.ToString();
			}
		}

		private static GameObject[] GameObjectsByName(string name, bool startsWith = true)
		{
			var gameObjectArray = Object.FindObjectsOfType<GameObject>();
			var list =
				gameObjectArray.Where(
					gameObject => startsWith && gameObject.name.StartsWith(name) || !startsWith && gameObject.name == name).ToArray();
			return list;
		}
	}
}