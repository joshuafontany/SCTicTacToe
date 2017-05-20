using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCTicTacToe
{
    public enum PlayerFaction
    {
        _0_Terran, _1_Zerg, _2_Protoss
    }

    public class Player : NotifyPropertyChanges
    {

        #region Player Properties
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private PlayerFaction _playerFaction;
        public PlayerFaction PlayerFaction
        {
            get { return _playerFaction; }
            set
            {
                _playerFaction = value;
                this.FactionIcon = Images.Instance.GetFactionIcon((int)_playerFaction);
                this.GamePieceImage = Images.Instance.GetGamePiece((int)_playerFaction);
                OnPropertyChanged();
            }
        }

        private string _factionIcon;
        public string FactionIcon
        {
            get { return _factionIcon; }
            set
            {
                _factionIcon = value;
                OnPropertyChanged();
            }
        }
        private string _heroIcon;
        public string HeroIcon
        {
            get { return _heroIcon; }
            set
            {
                _heroIcon = value;
                OnPropertyChanged();
            }
        }
        private string _gamePieceImage;
        public string GamePieceImage
        {
            get { return _gamePieceImage; }
            set
            {
                _gamePieceImage = value;
                OnPropertyChanged();
            }
        }

        private int _total;
        public int Total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged();
            }
        }
        private int _wins;
        public int Wins
        {
            get { return _wins; }
            set
            {
                _wins = value;
                OnPropertyChanged();
            }
        }
        private int _losses;
        public int Losses
        {
            get { return _losses; }
            set
            {
                _losses = value;
                OnPropertyChanged();
            }
        }
        private int _draws;
        public int Draws
        {
            get { return _draws; }
            set
            {
                _draws = value;
                OnPropertyChanged();
            }
        }

        public int PlayerPosition { get; set; }
        #endregion

        public Player()
        {
            this.Name = String.Format("New player");
            this.PlayerPosition = 0;
            this.PlayerFaction = 0;
            this.FactionIcon = Images.Instance.GetFactionIcon((int)this.PlayerFaction);

            List<string> heroIcons = Images.Instance.GetHeroIcons();
            int defaultHero = heroIcons.FindIndex(stringToCheck => stringToCheck.Contains("marine_64.gif"));
            this.HeroIcon = heroIcons[defaultHero];
            this.GamePieceImage = Images.Instance.GetGamePiece((int)this.PlayerFaction * 2);
        }

        public Player(int position) {
            this.Name = String.Format("player {0}", position.ToString());
            this.PlayerPosition = position;
            this.PlayerFaction = (PlayerFaction)position-1;
            this.FactionIcon = Images.Instance.GetFactionIcon((int)this.PlayerFaction);

            List<string> heroIcons = Images.Instance.GetHeroIcons();
            int defaultHero = (position == 1) ? heroIcons.FindIndex(stringToCheck => stringToCheck.Contains("marine_64.gif")) : heroIcons.FindIndex(stringToCheck => stringToCheck.Contains("zergling_64.gif"));
            this.HeroIcon = heroIcons[defaultHero];
            this.GamePieceImage = Images.Instance.GetGamePiece((int)this.PlayerFaction *2);
        }

        public static Player NewPlayer(Player previousPlayer) {
            Player NewPlayer = new Player(previousPlayer.PlayerPosition);
            NewPlayer.Name = (String.IsNullOrWhiteSpace(previousPlayer.Name))?String.Format("player {0}", NewPlayer.PlayerPosition.ToString()) : previousPlayer.Name;
            NewPlayer.PlayerFaction = previousPlayer.PlayerFaction;
            NewPlayer.FactionIcon = Images.Instance.GetFactionIcon((int)previousPlayer.PlayerFaction);
            NewPlayer.HeroIcon = previousPlayer.HeroIcon;
            NewPlayer.GamePieceImage = Images.Instance.GetGamePiece((int)previousPlayer.PlayerFaction * 2);

            return NewPlayer;
        }
    }

    public class PlayerRecords : NotifyPropertyChanges
    {

        private List<Player> _leaderboard;
        public List<Player> Leaderboard
        {
            get
            {
                return _leaderboard;
            }

            set
            {
                _leaderboard = value;
                OnPropertyChanged();
            }

        }

        public PlayerRecords()
        {
            this.Leaderboard = new List<Player>();
        }

        public PlayerRecords(List<Player> SavedLeaderboard)
        {
            this.Leaderboard = SavedLeaderboard;
        }

        public void UpdatePlayer(Player playerUpdate)
        {

            if (this.Exists(playerUpdate))
            {
                this.UpdateStatistics(playerUpdate);
            }
            else
            {
                this.AddPlayer(playerUpdate);
            }

        }

        public bool Exists(Player player)
        {

            return true;
        }

        public void UpdateStatistics(Player player)
        {

        }

        public void AddPlayer(Player newPlayer)
        {
            this.Leaderboard.Add(newPlayer); 
        }

    }
}
