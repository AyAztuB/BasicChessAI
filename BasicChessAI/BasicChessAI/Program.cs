using System;

namespace BasicChessAI
{
    class Program
    {
        static void Main(string[] args)
        {
            FullGame game = new FullGame(args);
            game.PlayGame();
        }
    }
}
