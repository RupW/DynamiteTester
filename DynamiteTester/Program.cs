using System;
using System.Collections.Generic;
using System.Linq;
using BotInterface.Bot;
using BotInterface.Game;

namespace DynamiteTester
{
    class Program
    {
        static void Main(string[] args)
        {
            // The bots to test
            IBot player1 = new RandomNoDynamiteBot();
            IBot player2 = new AllPaperBot();

            // Game setup
            int target = 1000, maxRounds = 2500, numberDynamites = 100;
            int printEvery = 100;

            // Fight!
            var rounds = new List<GameRound>();
            int score1 = 0, score2 = 0, lastPrinted = 0;
            int dynamites1 = numberDynamites, dynamites2 = numberDynamites;
            string result = null;
            string player1Name = player1.GetType().Name, player2Name = player2.GetType().Name;
            if (player1Name == player2Name)
            {
                player1Name += " A";
                player2Name += " B";
            }

            Console.WriteLine($"{player1Name} vs {player2Name} (first to {target}, {maxRounds} rounds, {numberDynamites} dynamites each)");
            Console.WriteLine();

            while (rounds.Count < maxRounds)
            {
                // Build a new Gamestate for each bot, with their moves as player 1
                var gamestate1 = new Gamestate();
                gamestate1.SetRounds(rounds.Select(r => MakeBotRound(r.Move1, r.Move2)).ToArray());
                var move1 = player1.MakeMove(gamestate1);

                var gamestate2 = new Gamestate();
                gamestate2.SetRounds(rounds.Select(r => MakeBotRound(r.Move2, r.Move1)).ToArray());
                var move2 = player2.MakeMove(gamestate2);

                // Check no-one has exceeded their dynamite limit
                if ((move1 == Move.D) && ((--dynamites1) < 0))
                {
                    result = $"{player1Name} too many dynamites";
                    break;
                }
                if ((move2 == Move.D) && ((--dynamites2) < 0))
                {
                    result = $"{player2Name} too many dynamites";
                    break;
                }

                // Check who won the round, store the round history and increment the scores
                int winner = WhoWon(move1, move2);
                rounds.Add(new GameRound() { Move1 = move1, Move2 = move2, Winner = winner });
                if ((winner == 1) && ((++score1) >= target))
                {
                    result = $"{player1Name} 1 won (first to {target})";
                    break;
                }
                if ((winner == 2) && ((++score2) >= target))
                {
                    result = $"{player1Name} 2 won (first to {target})";
                    break;
                }

                if ((rounds.Count - lastPrinted) >= printEvery)
                {
                    lastPrinted = PrintRounds(rounds, lastPrinted, score1, score2);
                }
            }

            if (rounds.Count > lastPrinted) PrintRounds(rounds, lastPrinted, score1, score2);
            if (String.IsNullOrEmpty(result))
            {
                if (score1 > score2) result = $"{player1Name} won (after {maxRounds} rounds)";
                else if (score2 > score1) result = $"{player2Name} won (after {maxRounds} rounds)";
                else result = $"Tie (after {maxRounds} rounds)";

            }
            Console.WriteLine("{0} {1} - {2} {3}     {4}", player1Name, score1, score2, player2Name, result);
            Console.ReadLine();
        }

        static int PrintRounds(List<GameRound> rounds, int lastPrinted, int score1, int score2)
        {
            // Lines: 1 won flags, 1 move, 2 move, 2 won flags, empty
            var toPrint = rounds.Skip(lastPrinted).ToList();
            Console.WriteLine("      {0}", new string(toPrint.Select(r => r.Winner == 1 ? '*' : ' ').ToArray()));
            Console.WriteLine("{1,4}  {0}{2,6}", String.Join(String.Empty, toPrint.Select(r => r.Move1.ToString())), lastPrinted, score1);
            Console.WriteLine("      {0}{1,6}", String.Join(String.Empty, toPrint.Select(r => r.Move2.ToString())), score2);
            Console.WriteLine("      {0}", new string(toPrint.Select(r => r.Winner == 2 ? '*' : ' ').ToArray()));
            Console.WriteLine();
            return lastPrinted + toPrint.Count;
        }

        class GameRound
        {
            public Move Move1;
            public Move Move2;
            public int Winner;
        }

        static int WhoWon(Move move1, Move move2)
        {
            if (DoesMoveBeat(move1, move2)) return 1;
            if (DoesMoveBeat(move2, move1)) return 2;
            return 0;
        }

        static bool DoesMoveBeat(Move move, Move other)
        {
            switch (move)
            {
                case Move.R:
                    return other == Move.S || other == Move.W;
                case Move.P:
                    return other == Move.R || other == Move.W;
                case Move.S:
                    return other == Move.P || other == Move.W;
                case Move.D:
                    return other != Move.W;
                case Move.W:
                    return other == Move.D;
            }
            // Invalid
            return false;
        }

        static Round MakeBotRound(Move move1, Move move2)
        {
            var botRound = new Round();
            botRound.SetP1(move1);
            botRound.SetP2(move2);
            return botRound;
        }
    }
}
