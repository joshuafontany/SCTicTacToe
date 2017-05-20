using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SCTicTacToe
{

    class GamePiece : NotifyPropertyChanges
    {
        public int _owner;
        public int Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                OnPropertyChanged();
            }
        }

        private int _x;
        public int X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged();
            }
        }

        private int _y;
        public int Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }

    }

    class TicTacToeGame : NotifyPropertyChanges
    {

        //Defines the games as a Tic-Tac-Toe (3 in a row) or Gomoku (5 in a row) game.

        #region GameInstance Properties
        /* GameInstance Propertiess */

        private Player _player1;
        public Player Player1
        {
            get { return _player1; }
            set
            {
                _player1 = value;
                OnPropertyChanged();
            }
        }

        public Player _player2;
        public Player Player2
        {
            get { return _player2; }
            set
            {
                _player2 = value;
                OnPropertyChanged();
            }
        }

        private string _gameStatus;
        public string GameStatus
        {
            get { return _gameStatus; }
            set
            {
                _gameStatus = value;
                OnPropertyChanged();
            }
        }

        private bool _player1Turn;
        public bool Player1Turn
        {
            get { return _player1Turn; }
            set
            {
                _player1Turn = value;
                OnPropertyChanged();
            }
        }

        private bool _gameComplete;
        public bool GameComplete
        {
            get { return _gameComplete; }
            set
            {
                _gameComplete = value;
                OnPropertyChanged();
            }
        }

        private bool _gomokuGame;
        public bool GomokuGame
        {
            get { return _gomokuGame; }
            set
            {
                _gomokuGame = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<GamePiece> _gamestate;
        public ObservableCollection<GamePiece> GameState
        {
            get { return _gamestate; }
            set
            {
                _gamestate = value;
                OnPropertyChanged();
            }
        }

        public int _gameWinnerInt;
        public int GameWinnerInt
        {
            get { return _gameWinnerInt; }
            set
            {
                _gameWinnerInt = value;
                OnPropertyChanged();
            }
        }

        private Player _winner;
        public Player Winner
        {
            get { return _winner; }
            set
            {
                _winner = value;
                OnPropertyChanged();
            }
        }

        private int _gameBoardSize;
        public int GameBoardSize
        {
            get { return _gameBoardSize; }
            set
            {
                _gameBoardSize = value;
                OnPropertyChanged();
            }
        }

        private int _winCondition;
        public int WinCondition
        {
            get { return _winCondition; }
            set
            {
                _winCondition = value;
                OnPropertyChanged();
            }
        }

        private int _gameTurn;
        public int GameTurn
        {
            get { return _gameTurn; }
            set
            {
                _gameTurn = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public TicTacToeGame()
        {
            this.GameState = new ObservableCollection<GamePiece> { };
            for (int i = 0; i < 3; i++)
            {
                for (int v = 0; v < 3; v++)
                {
                    this.GameState.Add(new GamePiece() {Owner = 0, X=i, Y=v });
                }
            }

            this.GomokuGame = false;
            this.GameBoardSize = 3;
            this.WinCondition = 3;
            this.GameTurn = 1;
            this.GameWinnerInt = 0;
            this.Winner = null;
            this.GameComplete = false;
            this.Player1Turn = true;
            this.Player1 = new Player(1);
            this.Player2 = new Player(2);

            this.GameStatus = string.Format("{0}'s Turn ( {1} ).", (this.Player1Turn) ? this.Player1.Name : this.Player2.Name, this.GameTurn.ToString());

        }

        public void NewGame(Player player1, Player player2, bool gomokuGame) {
            //set new gametype
            this.GomokuGame = gomokuGame;
            if (gomokuGame)
            {
                this.GameBoardSize = 9;
                this.WinCondition = 5;
            }
            else
            {
                this.GameBoardSize = 3;
                this.WinCondition = 3;
            }
            //Rebuild
            RebuildBoard();            

            this.GameTurn = 1;
            this.GameWinnerInt = 0;
            this.Winner = null;
            this.GameComplete = false;
            this.Player1Turn = true;

            if (player1 == null)
            {
                this.Player1 = new Player(1);
            }
            else {
                this.Player1 = player1;
            }

            if (player2 == null)
            {
                this.Player2 = new Player(2);
            }
            else {
                this.Player2 = player2;
            }

            //handle case of players belong to the same faction
            if (this.Player2.PlayerFaction == this.Player1.PlayerFaction)
            {
                    this.Player2.GamePieceImage = Images.Instance.GetGamePiece((int)this.Player2.PlayerFaction * 2 + 1);
            }
            else
            {
                    this.Player2.GamePieceImage = Images.Instance.GetGamePiece((int)this.Player2.PlayerFaction * 2);
            }

            this.GameStatus = string.Format("{0}'s Turn ({1}).", (this.Player1Turn) ? this.Player1.Name : this.Player2.Name, this.GameTurn.ToString());

        }

        public void RebuildBoard()
        {
            this.GameState.Clear();
            for (int i = 0; i < this.GameBoardSize; i++)
            {
                for (int v = 0; v < this.GameBoardSize; v++)
                {
                    this.GameState.Add(new GamePiece() { Owner = 0, X = i, Y = v });
                }
            }
            this.GameStatus = "Gameboards Cleared";
        }

        public void NextTurn(GamePiece selection)
        {
            //Set current player's piece
            if (selection.Owner == 0)
            {
                selection.Owner = (this.Player1Turn) ? 1 : 2;
            }

            //Validate Gameboard
            this.ValidateGamestate(selection);

            //Finally, set Status string.
            if (!this.GameComplete)
            {
                //Advance variables
                this.GameTurn++;
                this.Player1Turn = !this.Player1Turn;
                this.GameStatus = string.Format("{0}'s Turn ({1}).", (this.Player1Turn) ? this.Player1.Name : this.Player2.Name, this.GameTurn.ToString());
            }
        }

        public void ValidateGamestate(GamePiece lastMove)
        {
            //Reduce complexity by restricting the search space.
            //Wins can only happen along the horizontal, vertical, diagonal, and negative diagonal from the last move played,
            //and this must occur within WinContition-1 spaces from the last move, but not past the edges of the board.

            int MinBoundX = lastMove.X - (this.WinCondition -1);
            int MaxBoundX = lastMove.X + (this.WinCondition - 1);
            if (MinBoundX < 0)
            {
                MinBoundX = 0;
            }
            if (MaxBoundX >= this.GameBoardSize)
            {
                MaxBoundX = this.GameBoardSize - 1;
            }

            int MinBoundY = lastMove.Y - (this.WinCondition - 1);
            int MaxBoundY = lastMove.Y + (this.WinCondition - 1);
            if (MinBoundY < 0)
            {
                MinBoundY = 0;
            }
            if (MaxBoundY >= this.GameBoardSize)
            {
                MaxBoundY = this.GameBoardSize - 1;
            }

            List<GamePiece> Vertical = new List<GamePiece>();
            List<GamePiece> Horizontal = new List<GamePiece>();
            List<GamePiece> Diagonal = new List<GamePiece>();
            List<GamePiece> NegDiagonal = new List<GamePiece>();

            Vertical = GameState.Where(n => lastMove.Y == n.Y).ToList();
            Horizontal = GameState.Where(n => lastMove.X == n.X).ToList();
            Diagonal = GameState.Where(n => (n.X - lastMove.X) == (n.Y - lastMove.Y)).ToList();
            NegDiagonal = GameState.Where(n => (n.X - lastMove.X) == -1 * (n.Y - lastMove.Y)).ToList();

            TestLine(Vertical, lastMove);
            TestLine(Horizontal, lastMove);
            TestLine(Diagonal, lastMove);
            TestLine(NegDiagonal, lastMove);

            if (this.GameTurn == this.GameBoardSize * this.GameBoardSize && this.GameWinnerInt == 0)
            {
                this.GameComplete = true;
                this.GameStatus = "Game over.";
            }

            if (this.GameComplete)
            {
                switch (GameWinnerInt)
                {
                    case 0:
                        this.Player1.Draws++;
                        this.Player1.Total++;
                        this.Player2.Draws++;
                        this.Player2.Total++;
                        break;
                    case 1:
                        this.Player1.Wins++;
                        this.Player1.Total++;
                        this.Player2.Losses++;
                        this.Player2.Total++;
                        this.Winner = this.Player1;
                        break;
                    case 2:
                        this.Player1.Losses++;
                        this.Player1.Total++;
                        this.Player2.Wins++;
                        this.Player2.Total++;
                        this.Winner = this.Player2;
                        break;
                }
            }
        }

        private void TestLine(List<GamePiece> testLine, GamePiece lastMove)
        {
            int PlayerScore = 0;
            //Test each line for >= WinCondition
            foreach (GamePiece test in testLine)
            {
                if (test.Owner == lastMove.Owner)
                {
                    PlayerScore++;
                    if (PlayerScore >= this.WinCondition)
                    {
                        //Player Wins
                        this.GameWinnerInt = lastMove.Owner;
                        this.GameComplete = true;
                        this.GameStatus = "Game over.";
                        break;
                    }
                }
                else
                {
                    PlayerScore = 0;
                }
            }
        }
    }
}
