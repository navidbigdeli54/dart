using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Nodes;

namespace Network
{
    public interface IRemoteProcedures
    {
        void Invoke(string method);

        void Invoke(string method, object[] parameters);

        void Invoke(JsonObject jsonObject);
    }

    public abstract class RemoteProcedures : IRemoteProcedures
    {
        #region Fields
        private readonly Dictionary<string, MethodInfo> _procedures = new Dictionary<string, MethodInfo>();
        #endregion

        #region Protected Methods
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

        void IRemoteProcedures.Invoke(JsonObject jsonObject)
        {
            string methodName = jsonObject["Procedure"].ToString();

            if (_procedures.TryGetValue(methodName, out MethodInfo methodInfo))
            {
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                object[] parameters = new object[parameterInfos.Length];
                JsonArray parametersJsonArray = jsonObject["Parameters"].AsArray();

                for (int i = 0; i < parameterInfos.Length; ++i)
                {
                    ParameterInfo parameterInfo = parameterInfos[i];

                    for (int j = 0; j < parametersJsonArray.Count; ++j)
                    {
                        JsonObject parameterObject = parametersJsonArray[j].AsObject();
                        string parameterObjectName = parameterObject["Name"].ToString();
                        if (parameterObjectName == parameterInfo.Name)
                        {
                            TypeConverter converter = TypeDescriptor.GetConverter(parameterInfo.ParameterType);
                            string parameterValue = parameterObject["Value"].ToString();
                            parameters[i] = converter.ConvertFrom(parameterValue);
                        }
                    }
                }

                methodInfo?.Invoke(this, parameters);
            }
        }
        #endregion
    }
}
