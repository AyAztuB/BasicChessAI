using BasicChessAI.Game.Pieces;

namespace BasicChessAI.Game
{
    public class Tile
    {
        private int x;
        private int y;
        private Piece piece;
        private Board board;

        public int X => this.x;
        public int Y => this.y;
        public Piece Piece => this.piece;
        public Board Board => this.board;

        public Tile(int x, int y, Board board, char piece)
        {
            this.x = x;
            this.y = y;
            this.board = board;
            this.piece = PieceByChar(this, board, piece);
        }

        public Tile(Tile tile, Board newBoard)
        {
            this.x = tile.X;
            this.y = tile.Y;
            this.board = newBoard;
            this.piece = PieceByChar(this, newBoard, tile.Piece.ToString()[0]);
        }

        public void ChangePiece(Piece newPiece)
        {
            this.piece = newPiece;
        }

        private Piece PieceByChar(Tile tile, Board board, char c)
        {
            switch (c)
            {
                case 'p':
                case 'P':
                    return new Pawn(tile, c == 'p' ? Color.White : Color.Black, board);
                case 'n':
                case 'N':
                    return new Knight(tile, c == 'n' ? Color.White : Color.Black, board);
                case 'b':
                case 'B':
                    return new Bishop(tile, c == 'b' ? Color.White : Color.Black, board);
                case 'r':
                case 'R':
                    return new Rook(tile, c == 'r' ? Color.White : Color.Black, board);
                case 'q':
                case 'Q':
                    return new Queen(tile, c == 'q' ? Color.White : Color.Black, board);
                case 'k':
                case 'K':
                    return new King(tile, c == 'k' ? Color.White : Color.Black, board);
            }

            return new Empty(tile, board);
        }
    }
}