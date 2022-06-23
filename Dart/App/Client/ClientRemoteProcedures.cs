using Network;

namespace Client
{
    public class ClientRemoteProcedures : RemoteProcedures
    {
        public void Connected(string userId)
        {
            Console.WriteLine($"UserId is {userId}");

            int[] scores = new int[] { 1, 2, 3, 5, 10, 15, 25 };

            for (int i = 0; i < 10; ++i)
            {
                Procedure procedure = new Procedure(
                    "DartThrowed",
                    new Parameter[] {
                        new Parameter("userId", userId),
                        new Parameter("Score", scores[Random.Shared.Next(0, scores.Length)])
                    });

                Program.ClientInstance.Send(procedure);

                Thread.Sleep(1000);
            }
        }

        public void UpdateLeaderboard(string strigifinedJson)
        {

        }
    }
}
