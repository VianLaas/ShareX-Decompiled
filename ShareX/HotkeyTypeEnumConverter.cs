using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShareX;

internal class HotkeyTypeEnumConverter : StringEnumConverter
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.String)
		{
			string text = reader.Value!.ToString();
			if (!string.IsNullOrEmpty(text) && text.Equals("WindowRectangle"))
			{
				return HotkeyType.RectangleRegion;
			}
		}
		return base.ReadJson(reader, objectType, existingValue, serializer);
	}
}
