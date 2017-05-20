using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SCTicTacToe
{
    public sealed class Images
    {
        private static readonly Images instance = new Images();
        public static Images Instance
        {
            get
            {
                return instance;
            }
        }

        private List<string> _backgrounds = new List<string>();
        private List<string> _heroIcons = new List<string>();

        private List<string> _factionIcons = new List<string>()
        {
            @"pack://application:,,,/SCTicTacToe;component/images/Terran_Icon.png",
            @"pack://application:,,,/SCTicTacToe;component/images/Zerg_Icon.png",
            @"pack://application:,,,/SCTicTacToe;component/images/Protoss_Icon.png"
        };
        private List<string> _gamePieces = new List<string>()
        {
            @"pack://application:,,,/SCTicTacToe;component/images/Marine_SC1.gif",
            @"pack://application:,,,/SCTicTacToe;component/images/Vulture_SC1.gif",
            @"pack://application:,,,/SCTicTacToe;component/images/Zergling_SC1.gif",
            @"pack://application:,,,/SCTicTacToe;component/images/Hydralisk_SC1.gif",
            @"pack://application:,,,/SCTicTacToe;component/images/Zealot_SC1.gif",
            @"pack://application:,,,/SCTicTacToe;component/images/Dragoon_SC1.gif"
        };


        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Images()
        {
        }

        private Images()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var rm = new ResourceManager(assembly.GetName().Name + ".g", assembly);
            try
            {
                var list = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true);
                foreach (DictionaryEntry item in list)
                {
                    string keyName = (string)item.Key;
                    if (keyName.StartsWith(@"images/backgrounds/"))
                    {
                        _backgrounds.Add(String.Format("{0}{1}", @"pack://application:,,,/SCTicTacToe;component/", keyName));
                    }
                    else if (keyName.StartsWith(@"images/avatars/"))
                    {
                        _heroIcons.Add(String.Format("{0}{1}", @"pack://application:,,,/SCTicTacToe;component/", keyName));
                    }
                }
                    
            }
            finally
            {
                rm.ReleaseAllResources();
            }

            _backgrounds.Sort();
            _heroIcons.Sort();
        }

        public List<string> GetBackgrounds() {
            return _backgrounds;
        }

        public List<string> GetHeroIcons()
        {
            return _heroIcons;
        }

        public string GetFactionIcon(int fac)
        {
            return _factionIcons[fac];
        }

        public string GetGamePiece(int fac)
        {
            return _gamePieces[fac];
        }
    }
}
