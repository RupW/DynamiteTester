using System;
using BotInterface.Bot;
using BotInterface.Game;

namespace DynamiteTester
{
    public class AllPaperBot : IBot
    {
        public Move MakeMove(Gamestate gamestate)
        {
            return Move.P;
        }
    }

    public class RandomNoDynamiteBot : IBot
    {
        private readonly Random _random = new Random();
        private static readonly Move[] Moves = { Move.R, Move.P, Move.S };
        public Move MakeMove(Gamestate gamestate)
        {
            return Moves[_random.Next(Moves.Length)];
            ;
        }
    }
}
