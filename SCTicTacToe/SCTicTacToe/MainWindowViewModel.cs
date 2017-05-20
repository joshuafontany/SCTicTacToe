using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SCTicTacToe
{
    class MainWindowViewModel : NotifyPropertyChanges
    {
        public MainWindowViewModel()
        {
            ///DelegateCommand setup.
            this.ChangeBackgroundCommand = new DelegateCommand(ChangeBackgroundAction);
            this.NewGameCommand = new DelegateCommand(NewGameAction, CanNewGameCommand);
            this.ConfirmNewGameCommand = new DelegateCommand(ConfirmNewGameAction, CanConfirmNewGameCommand);
            this.ChangeHeroCommand = new DelegateCommand(ChangeHeroAction);
            this.GameboardButtonCommand = new DelegateCommand(GameboardButtonAction, CanGameboardButtonCommand);
            this.CreditsButtonCommand = new DelegateCommand(CreditsButtonAction, CanCreditsButtonCommand);

            //Init
            this.Foo = "This is a test.";
            _bgIndex = 0;
            this.CurrentBackground = Images.Instance.GetBackgrounds()[_bgIndex];
            this.NewGameGomoku = false;

            //Load Player1, Player 2, GameInstance, and PlayerRecordsInstance from JSON files, or Initialize.
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"playerRecords.json")))
            {
                this.PlayerRecordsInstance = JSONSerializer.ReadFromJsonFile<PlayerRecords>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"playerRecords.json"));
            }
            else
            {
                this.PlayerRecordsInstance = new PlayerRecords();
            }

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"player1.json")))
            {
                this.Player1 = JSONSerializer.ReadFromJsonFile<Player>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"player1.json"));
                this.NewPlayer1 = this.Player1;
            }
            else
            {
                this.Player1 = new Player(1);
                this.NewPlayer1 = new Player(1);
                this.PlayerRecordsInstance.AddPlayer(this.Player1);
            }

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"player2.json")))
            {
                this.Player2 = JSONSerializer.ReadFromJsonFile<Player>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"player2.json"));
                this.NewPlayer2 = this.Player2;
            }
            else
            {
                this.Player2 = new Player(2);
                this.NewPlayer2 = new Player(2);
                this.PlayerRecordsInstance.AddPlayer(this.Player2);
            }

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"gameInstance.json")))
            {
                this.GameInstance = JSONSerializer.ReadFromJsonFile<TicTacToeGame>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"gameInstance.json"));
                if (GameInstance.GameComplete)
                {
                    this.GameInstance.NewGame(this.Player1, this.Player2, false);
                }
            }
            else
            {
                this.GameInstance = new TicTacToeGame();
                this.GameInstance.NewGame(this.Player1, this.Player2, false);
            }

            
        }

        //Close Command
        private ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                if (_closeWindowCommand == null)
                {
                    _closeWindowCommand = new DelegateCommand(param => this.CloseWindow(), null);
                }
                return _closeWindowCommand;
            }
        }
        private void CloseWindow()
        {
            //Save Player1, Player 2, GameInstance, and PlayerRecordsInstance to JSON files.
            JSONSerializer.WriteToJsonFile<Player>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"player1.json"), Player1);
            JSONSerializer.WriteToJsonFile<Player>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"player2.json"), Player2);
            JSONSerializer.WriteToJsonFile<TicTacToeGame>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"gameInstance.json"), GameInstance);
            JSONSerializer.WriteToJsonFile<PlayerRecords>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"playerRecords.json"), PlayerRecordsInstance);
        }


        //Test String
        private string _foo;
        public string Foo
        {
            get { return _foo; }
            set
            {
                _foo = value;
                OnPropertyChanged();
            }
        }

        #region Gamestate Properties
        private TicTacToeGame _gameInstance;
        public TicTacToeGame GameInstance
        {
            get { return _gameInstance; }
            set
            {
                _gameInstance = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Player Properties/Commands
        private Player _p1;
        public Player Player1
        {
            get { return _p1; }
            set
            {
                _p1 = value;
                OnPropertyChanged();
            }
        }
        private Player _p2;
        public Player Player2
        {
            get { return _p2; }
            set
            {
                _p2 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Background Properties/Commands
        private int _bgIndex;
        private string _currentBackground;
        public string CurrentBackground
        {
            get { return _currentBackground; }
            set
            {
                _currentBackground = value;
                OnPropertyChanged();
            }
        }
        public DelegateCommand ChangeBackgroundCommand { get; private set; }
        private void ChangeBackgroundAction(object parameter)
        {
            int x = Int32.Parse((string)parameter);
            if (x==-1)
            {
                if (_bgIndex == 0)
                {
                    _bgIndex = Images.Instance.GetBackgrounds().Count;
                }
                _bgIndex--;
            }
            else if (x==1)
            {
                _bgIndex++;
                if (_bgIndex == Images.Instance.GetBackgrounds().Count)
                {
                    _bgIndex = 0;
                }
            }
            this.CurrentBackground = Images.Instance.GetBackgrounds()[_bgIndex];
        }
        #endregion

        #region NewGame Properties/Commands
        private Player _np1;
        public Player NewPlayer1
        {
            get { return _np1; }
            set
            {
                _np1 = value;
                OnPropertyChanged();
            }
        }
        private Player _np2;
        public Player NewPlayer2
        {
            get { return _np2; }
            set
            {
                _np2 = value;
                OnPropertyChanged();
            }
        }
        private bool _np1Selected;
        public bool IsUserName1Focused
        {
            get { return _np1Selected; }
            set
            {
                _np1Selected = value;
                OnPropertyChanged();
            }
        }
        private bool _np2Selected;
        public bool IsUserName2Focused
        {
            get { return _np2Selected; }
            set
            {
                _np2Selected = value;
                OnPropertyChanged();
            }
        }
        private bool _ngg;
        public bool NewGameGomoku
        {
            get { return _ngg; }
            set
            {
                _ngg = value;
                OnPropertyChanged();
            }
        }
        private string _ngDialogVisible = "Collapsed";
        public string NGDialogVisible
        {
            get { return _ngDialogVisible; }
            set
            {
                _ngDialogVisible = value;
                OnPropertyChanged();
            }
        }
        //Commands
        public DelegateCommand NewGameCommand { get; private set; }
        private bool CanNewGameCommand(object parameter)
        {
            //this method validate whether or not the New Game Button is active
            return true;
        }
        private void NewGameAction(object parameter)
        {
            //this method will be triggered when the New Game Button is clicked
            if (GODialogVisible == "Visible")
            {
                GODialogVisible = "Collapsed";
            }

            this.NewPlayer1 = Player.NewPlayer(this.Player2);
            this.NewPlayer2 = Player.NewPlayer(this.Player1);
            //Toggle textbox selection, bugfix for visual artifacts
            IsUserName1Focused = true;
            IsUserName1Focused = false;
            IsUserName2Focused = true;
            IsUserName2Focused = false;
            this.NGDialogVisible = "Visible";            
        }

        public DelegateCommand ConfirmNewGameCommand { get; private set; }
        private bool CanConfirmNewGameCommand(object parameter)
        {
            return true;
        }
        private void ConfirmNewGameAction(object parameter)
        {
            if ((string)parameter == "Confirm")
            {
                //Abort on invalid usernames
                Regex regex = new Regex(@"^(?=.{1,15}$)[A-Za-z0-9]+(?:[ _-][A-Za-z0-9]+)*$");
                Match match1 = regex.Match(NewPlayer1.Name);
                if (!match1.Success)
                {
                    NewPlayer1.Name = "player 1";
                    IsUserName1Focused = true;
                    return;
                }
                Match match2 = regex.Match(NewPlayer2.Name);
                if (!match2.Success)
                {
                    NewPlayer2.Name = "player 2";
                    IsUserName2Focused = true;
                    return;
                }

                //Enfore lowercase
                NewPlayer1.Name = NewPlayer1.Name.ToLower();
                NewPlayer2.Name = NewPlayer2.Name.ToLower();

                // If the new player exists in PlayerRecords
                // load their profile here and update faction & hero
                IEnumerable<Player> toUpdate1 = this.PlayerRecordsInstance.Leaderboard.Where(x => x.Name == this.NewPlayer1.Name);
                if (toUpdate1.Count() > 0)
                {
                    Player oldPlayerStats = toUpdate1.First<Player>();
                    oldPlayerStats.FactionIcon = NewPlayer1.FactionIcon;
                    oldPlayerStats.HeroIcon = NewPlayer1.HeroIcon;
                    NewPlayer1 = oldPlayerStats;

                    this.PlayerRecordsInstance.Leaderboard.Remove(oldPlayerStats);
                    this.PlayerRecordsInstance.Leaderboard.Add(NewPlayer1);
                    this.Player1 = NewPlayer1;
                }
                else
                {
                    this.Player1 = Player.NewPlayer(NewPlayer1);
                }
                IEnumerable<Player> toUpdate2 = this.PlayerRecordsInstance.Leaderboard.Where(x => x.Name == this.NewPlayer2.Name);
                if (toUpdate2.Count() > 0)
                {
                    Player oldPlayerStats = toUpdate2.First<Player>();
                    oldPlayerStats.FactionIcon = NewPlayer2.FactionIcon;
                    oldPlayerStats.HeroIcon = NewPlayer2.HeroIcon;
                    NewPlayer2 = oldPlayerStats;

                    this.PlayerRecordsInstance.Leaderboard.Remove(oldPlayerStats);
                    this.PlayerRecordsInstance.Leaderboard.Add(NewPlayer2);
                    this.Player2 = NewPlayer2;
                }
                else
                {
                    this.Player2 = Player.NewPlayer(NewPlayer2);
                }

                this.GameInstance.NewGame(this.NewPlayer1, this.NewPlayer2, this.NewGameGomoku);

            }
            this.NGDialogVisible = "Collapsed";
        }

        public DelegateCommand ChangeHeroCommand { get; private set; }
        private void ChangeHeroAction(object parameter)
        {
            List<string> heroIcons = Images.Instance.GetHeroIcons();
            int player = Int32.Parse(parameter.ToString().Split(',')[0]);
            int x = Int32.Parse(parameter.ToString().Split(',')[1]);

            int heroIndex = (player == 1) ? heroIcons.IndexOf(NewPlayer1.HeroIcon): heroIcons.IndexOf(NewPlayer2.HeroIcon);

            if (x  == -1)
            {                
                if (heroIndex == 0)
                {
                    heroIndex = heroIcons.Count;
                }
                heroIndex--;
            }
            else if (x == 1)
            {
                heroIndex++;
                if (heroIndex == heroIcons.Count)
                {
                    heroIndex = 0;
                }
            }
            if (player == 1)
            {
                NewPlayer1.HeroIcon = heroIcons[heroIndex];
            }
            else if (player == 2)
            {
                NewPlayer2.HeroIcon = heroIcons[heroIndex];
            }
        }
        #endregion

        #region PlayerArea Properties/Commands

        #endregion

        #region Gameboard Properties/Commands
        private string _goDialogVisible = "Collapsed";
        public string GODialogVisible
        {
            get { return _goDialogVisible; }
            set
            {
                _goDialogVisible = value;
                OnPropertyChanged();
            }
        }


        public DelegateCommand GameboardButtonCommand { get; private set; }

        private bool CanGameboardButtonCommand(object parameter)
        {
            return true;
        }
        private void GameboardButtonAction(object parameter)
        {
            GamePiece selection = (GamePiece)parameter;
            //Can only select an empty space, test position chosen against gamestate.
            if (selection.Owner == 0)
            {
                //test
                this.Foo = String.Format("{0},{1} pressed by {2}!", selection.X, selection.Y, (this.GameInstance.Player1Turn) ? this.Player1.Name : this.Player2.Name);

                //Pass gameboard position chosen to NextTurn()
                this.GameInstance.NextTurn(selection);
                if (this.GameInstance.GameComplete)
                {
                    GODialogVisible = "Visible";
                }
            }
            else
            {
                return;
            }
            if (this.GameInstance.GameComplete)
            {
                //Update Leaderboard
                IEnumerable<Player> toUpdate1 = this.PlayerRecordsInstance.Leaderboard.Where(x => x.Name == this.GameInstance.Player1.Name);
                if (toUpdate1.Count() > 0)
                {
                    Player oldPlayerStats = toUpdate1.First<Player>();

                    this.PlayerRecordsInstance.Leaderboard.Remove(oldPlayerStats);
                    this.PlayerRecordsInstance.Leaderboard.Add(this.GameInstance.Player1);
                }
                else
                {
                    this.PlayerRecordsInstance.Leaderboard.Add(this.GameInstance.Player1);
                }
                IEnumerable<Player> toUpdate2 = this.PlayerRecordsInstance.Leaderboard.Where(x => x.Name == this.GameInstance.Player2.Name);
                if (toUpdate2.Count() > 0)
                {
                    Player oldPlayerStats = toUpdate2.First<Player>();

                    this.PlayerRecordsInstance.Leaderboard.Remove(oldPlayerStats);
                    this.PlayerRecordsInstance.Leaderboard.Add(this.GameInstance.Player2);
                }
                else
                {
                    this.PlayerRecordsInstance.Leaderboard.Add(this.GameInstance.Player2);
                }
                //Sort and replace Leaderboard
                List<Player> newList = this.PlayerRecordsInstance.Leaderboard.OrderByDescending(x => x.Wins).ThenByDescending(x => x.Draws).ThenBy(x => x.Losses).ThenBy(x => x.Name).ToList();
                this.PlayerRecordsInstance.Leaderboard = newList;
            }
        }
        #endregion

        #region Leaderboard Properties/Commands
        private PlayerRecords _playerRecordsInstance;
        public PlayerRecords PlayerRecordsInstance
        {
            get { return _playerRecordsInstance; }
            set
            {
                _playerRecordsInstance = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Credits Properties/Commands
        private string _creditsDialogVisible = "Collapsed";
        public string CreditsDialogVisible
        {
            get { return _creditsDialogVisible; }
            set
            {
                _creditsDialogVisible = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand CreditsButtonCommand { get; private set; }

        private bool CanCreditsButtonCommand(object parameter)
        {
            return true;
        }
        private void CreditsButtonAction(object parameter)
        {
            bool open = bool.Parse(parameter.ToString());
            if (open)
            {
                CreditsDialogVisible = "Visible";
            }
            else
            {
                CreditsDialogVisible = "Collapsed";
            }
        }

        #endregion
    }
}
