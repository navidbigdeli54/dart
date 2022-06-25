namespace Core.Network
{
    public interface IRemoteProcedures
    {
        void Invoke(string method);

        void Invoke(string method, object[] parameters);

        void Invoke(Procedure procedure);
    }
}
