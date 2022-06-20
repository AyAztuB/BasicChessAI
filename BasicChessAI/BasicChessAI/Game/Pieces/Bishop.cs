using System.Collections.Generic;

namespace BasicChessAI.Game.Pieces
{
    public class Bishop:Piece
    {
        public Bishop(Tile tile, Color color, Board board) : base(tile, PieceType.Bishop, color, board)
        {
        }

        public override List<Tile> GetPossibleMoves(bool checkCheck = true) => this.BishopMoves(checkCheck);
    }
}