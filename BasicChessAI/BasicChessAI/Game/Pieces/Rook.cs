using System.Collections.Generic;

namespace BasicChessAI.Game.Pieces
{
    public class Rook:Piece
    {
        public bool hasMoved { get; set; }
        
        public Rook(Tile tile, Color color, Board board, bool hasMoved = false) : base(tile, PieceType.Rook, color, board)
        {
            this.hasMoved = hasMoved;
        }

        public override List<Tile> GetPossibleMoves(bool checkCheck = true) => this.RookMoves(checkCheck);
    }
}