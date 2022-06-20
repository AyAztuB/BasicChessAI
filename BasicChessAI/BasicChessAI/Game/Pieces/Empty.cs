using System.Buffers.Text;

namespace BasicChessAI.Game.Pieces
{
    public class Empty:Piece
    {
        public Empty(Tile tile, Board board) : base(tile, PieceType.Empty, Color.Null, board)
        {
        }
    }
}