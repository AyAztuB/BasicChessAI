using System;
using BasicChessAI.Game;
using BasicChessAI.Game.Pieces;

namespace BasicChessAI.Players
{
    public enum PlayerType
    {
        Human,
        AI,
    }
    
    public abstract class Player
    {
        private PlayerType type;
        private Color playerColor;
        private Board board;

        public PlayerType Type => this.type;
        public Color PlayerColor => this.playerColor;
        public Board Board => this.board;

        public Player(PlayerType type, Color color, Board board)
        {
            this.type = type;
            this.playerColor = color;
            this.board = board;
        }

        public abstract (Tile,Tile) PlayOneRound(Tile lastMove, Tile nextPos);
    }
}