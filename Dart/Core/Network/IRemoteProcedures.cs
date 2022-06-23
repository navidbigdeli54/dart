using System.Reflection;
using System.ComponentModel;
using System.Text.Json.Nodes;

namespace Network
{
    public interface IRemoteProcedures
    {
        void Invoke(string method);

        void Invoke(string method, object[] parameters);

        void Invoke(Procedure procedure);
    }

    public abstract class RemoteProcedures : IRemoteProcedures
    {
        #region Fields
        private readonly Dictionary<string, MethodInfo> _procedures = new Dictionary<string, MethodInfo>();
        #endregion

        #region Constructors
        public RemoteProcedures()
        {
            Type type = GetType();

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < methods.Length; ++i)
            {
                MethodInfo methodInfo = methods[i];
                _procedures.Add(methodInfo.Name, methodInfo);
            }
        }
        #endregion

        #region Public Methods
        public virtual void OnConnected(string remoteEndPoint)
        {
            Console.WriteLine($"Client {remoteEndPoint} connected!");
        }
        #endregion

        #region IRemoteProcedures Implementation
        void IRemoteProcedures.Invoke(string method)
        {
            if (_procedures.TryGetValue(method, out MethodInfo methodInfo))
            {
                methodInfo?.Invoke(this, null);
            }
        }

        void IRemoteProcedures.Invoke(string method, object[] parameters)
        {
            if (_procedures.TryGetValue(method, out MethodInfo methodInfo))
            {
                methodInfo?.Invoke(this, parameters);
            }
        }

        void IRemoteProcedures.Invoke(Procedure procedure)
        {
            if (_procedures.TryGetValue(procedure.Name, out MethodInfo methodInfo))
            {
                methodInfo?.Invoke(this, procedure.Parameters.Select(x => x.Value).ToArray());
            }
        }
        #endregion
    }

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
