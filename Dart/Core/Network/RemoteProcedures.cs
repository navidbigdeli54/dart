using System.Reflection;
using System.Collections.Concurrent;

namespace Network
{
    public abstract class RemoteProcedures : IRemoteProcedures
    {
        #region Fields
        private readonly Dictionary<string, MethodInfo> _registeredRemoteProcedures = new Dictionary<string, MethodInfo>();

        private readonly ConcurrentQueue<Procedure> _remoteProcequresQueue = new ConcurrentQueue<Procedure>();

        private bool _isProcesingQueue = false;
        #endregion

        #region Constructors
        public RemoteProcedures()
        {
            Type type = GetType();

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < methods.Length; ++i)
            {
                MethodInfo methodInfo = methods[i];
                _registeredRemoteProcedures.Add(methodInfo.Name, methodInfo);
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
            _remoteProcequresQueue.Enqueue(new Procedure(method, null));
            if (_isProcesingQueue == false)
            {
                ProcessQueue();
            }
        }

        void IRemoteProcedures.Invoke(string method, object[] parameters)
        {
            _remoteProcequresQueue.Enqueue(new Procedure(method, parameters.Select(x => new Parameter(string.Empty, x)).ToArray()));
            if (_isProcesingQueue == false)
            {
                ProcessQueue();
            }
        }

        void IRemoteProcedures.Invoke(Procedure procedure)
        {
            _remoteProcequresQueue.Enqueue(procedure);
            if (_isProcesingQueue == false)
            {
                ProcessQueue();
            }
        }
        #endregion

        #region Private Methods
        private void ProcessQueue()
        {
            _isProcesingQueue = true;

            while (_remoteProcequresQueue.Count > 0)
            {
                if (_remoteProcequresQueue.TryDequeue(out Procedure? procedure))
                {
                    if (_registeredRemoteProcedures.TryGetValue(procedure.Name, out MethodInfo? methodInfo))
                    {
                        try
                        {
                            methodInfo?.Invoke(this, procedure.Parameters.Select(x => x.Value).ToArray());
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    }
                }
            }

            _isProcesingQueue = false;
        }
        #endregion
    }
}
