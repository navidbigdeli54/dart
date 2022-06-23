using Network;

namespace Client
{
    public class ClientRemoteProcedures : RemoteProcedures
    {
        public void Connected(string userId)
        {
            Console.WriteLine($"UserId is {userId}");
        }
    }
}
