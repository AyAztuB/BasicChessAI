using System;
using System.Linq;
using BasicChessAI.Game;
using BasicChessAI.Game.Pieces;
using BasicChessAI.Players;

namespace BasicChessAI
{
    public class FullGame
    {
        private Player white;
        private Player black;
        private Board game;
        private bool pressEnter;

        public FullGame(Player white, Player black, Board game)
        {
            this.game = game;
            this.white = white;
            this.black = black;
            this.pressEnter = true;
        }

        public FullGame(string[] option)
        {
            if (option.Length == 0 || (option.Length == 1 && (option[0] == "--withoutEnter" || option[0] == "--vs")) ||
                (option.Length == 2 && ((option[0] == "--vs" && option[1] == "--withoutEnter") || (option[0] == "--withoutEnter" &&
                        option[1] == "--vs"))))
            {
                this.game = new Board();
                this.white = new Human(Color.White, game);
                this.black = new Human(Color.Black, game);
                this.pressEnter = !option.Contains("--withoutEnter");
            }
            else if ((option.Length == 1 && option[0] == "--vsBot") || (option.Length == 2 && ((option[0] == "--vsBot" &&
                    option[1] == "--withoutEnter") || (option[0] == "--withoutEnter" && option[1] == "--vsBot"))))
            {
                string input;
                do
                {
                    Console.Clear();
                    Console.Write("Do you want to play white ? (Y/n) : ");
                    input = Console.ReadLine();
                } while (input != "Y" && input != "n");

                this.game = new Board();
                if (input == "Y")
                {
                    this.white = new Human(Color.White, game);
                    this.black = new AI(Color.Black, game, 0);
                }
                else
                {
                    this.white = new AI(Color.White, game, 0);
                    this.black = new Human(Color.Black, game);
                }

                this.pressEnter = !option.Contains("--withoutEnter");
            }
            else if ((option.Length == 1 && option[0] == "--bot") || (option.Length == 2 && ((option[0] == "--bot" &&
                option[1] == "--withoutEnter") || (option[0] == "--withoutEnter" && option[1] == "--bot"))))
            {
                this.game = new Board();
                this.white = new AI(Color.White, game, 0);
                this.black = new AI(Color.Black, game, 0);
                this.pressEnter = !option.Contains("--withoutEnter");
            }
            else
            {
                Console.Error.WriteLine("Invalid Argument !");
                Environment.Exit(66);
            }
        }

        public void PlayGame()
        {
            int Round = 1;
            game.Print();
            Color colorToPlay = Color.White;
            Tile start = null;
            Tile target = null;
            while (!game.IsEnd(colorToPlay))
            {
                if (colorToPlay == Color.White)
                    (start, target) = this.white.PlayOneRound(start, target);
                else
                {
                    (start, target) = this.black.PlayOneRound(start, target);
                    Round += 1;
                }
                colorToPlay = (Color)(((int)colorToPlay) * -1);
                if (this.pressEnter)
                {
                    Console.Clear();
                    game.Print(start, target, colorToPlay);
                    Console.WriteLine("Board value: " + Heuristics.AllIn(game));
                    Console.WriteLine($"Round {Round}");
                    Console.ReadLine();
                }
            }
            Console.Clear();
            game.Print(start, target, colorToPlay);
            if (game.IsCheck(colorToPlay))
                Console.WriteLine((colorToPlay == Color.White ? "Black" : "White") + " won!!!");
            else
                Console.WriteLine("Equality...");
        }
    }
}