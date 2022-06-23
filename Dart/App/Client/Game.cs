using Network;

namespace Client
{
    public class Game
    {
        private const float HIT_CHANCE = 0.6f;

        private const int MISS_SCORE = -5;

        private readonly int[] HIT_SCORE = new int[] { 1, 2, 3, 5, 10, 15, 25 };

        private readonly int MAX_THROW = 10;

        private readonly int THROW_INTERVAL = 5000;

        private readonly Guid _userId;

        public Game(Guid userId)
        {
            _userId = userId;
        }

        public void Start()
        {
            for (int i = 0; i < MAX_THROW; ++i)
            {
                Throw();

                Thread.Sleep(THROW_INTERVAL);
            }
        }

        private void Throw()
        {
            int currentScore = CalculateScore();

            SendToServer(currentScore);
        }

        private int CalculateScore()
        {
            float dice = Random.Shared.NextSingle();

            if (dice < HIT_CHANCE) return HIT_SCORE[Random.Shared.Next(0, HIT_SCORE.Length)];

            else return MISS_SCORE;
        }

        private void SendToServer(int score)
        {
            Procedure procedure = new Procedure(
                    "DartThrowed",
                    new Parameter[] {
                        new Parameter("userId", _userId),
                        new Parameter("Score", score)
                    });

            Program.ClientInstance.Send(procedure);
        }
    }
}
