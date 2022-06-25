using System.Text.Json.Nodes;

namespace Network
{
    public class Procedure
    {
        public string Name { get; }

        public Parameter[] Parameters { get; }

        public Procedure(string name, Parameter[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public Procedure(JsonObject jsonObject)
        {
            Name = jsonObject["Name"].ToString();

            JsonArray parameterArray = jsonObject["Parameters"].AsArray();
            Parameters = new Parameter[parameterArray.Count];
            for (int i = 0; i < parameterArray.Count; ++i)
            {
                JsonObject parameterJsonObject = parameterArray[i].AsObject();
                Parameters[i] = new Parameter(parameterJsonObject);
            }
        }

        public override string ToString()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["Name"] = Name;

            JsonArray parameterArray = new JsonArray();
            for (int i = 0; i < Parameters.Length; ++i)
            {
                parameterArray.Add(Parameters[i].ToJson());
            }
            jsonObject["Parameters"] = parameterArray;

            return jsonObject.ToJsonString();
        }
    }
}
