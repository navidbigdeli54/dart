using System.ComponentModel;
using System.Text.Json.Nodes;

namespace Network
{
    public class Parameter
    {
        public string Name { get; }

        public object Value { get; }

        public Parameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public Parameter(JsonObject jsonObject)
        {
            Name = jsonObject["Name"].ToString();
            string typeName = jsonObject["Type"].ToString();
            Type type = Type.GetType(typeName);
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            string stringifiedValue = jsonObject["Value"].ToString();
            Value = converter.ConvertFrom(stringifiedValue);
        }

        public override string ToString()
        {
            return ToJson().ToJsonString();
        }

        public JsonObject ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["Name"] = Name;
            jsonObject["Type"] = Value.GetType().FullName;
            jsonObject["Value"] = Value.ToString();
            return jsonObject;
        }
    }
}
