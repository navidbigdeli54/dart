using System.Reflection;

namespace Network
{
    public interface IRemoteProcedures
    {
        void Invoke(string method);

        void Invoke(string method, object[] parameters);
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
            ((IRemoteProcedures)this).Invoke(method, null);
        }

        void IRemoteProcedures.Invoke(string method, object[] parameters)
        {
            if (_procedures.TryGetValue(method, out MethodInfo methodInfo))
            {
                methodInfo?.Invoke(this, parameters);
            }
        }
        #endregion
    }
}
