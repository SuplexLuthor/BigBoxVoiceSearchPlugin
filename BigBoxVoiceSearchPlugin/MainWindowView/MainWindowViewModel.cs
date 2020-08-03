using BigBoxVoiceSearchPlugin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Speech.Recognition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Unbroken.LaunchBox.Plugins.Data;

namespace BigBoxVoiceSearchPlugin.MainWindowView
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public MainWindowViewModel()
        {
            IsActive = false;
            IsInitializing = false;
            IsRecognizing = false;
            IsDisplayingResults = false;
            VoiceRecognitionResults = new List<VoiceRecognitionResult>();
        }

        // shift everything up
        public void MoveToPreviousGame()
        {
            SelectedGameIndex = Prev1GameIndex;
            GameIndexChanged();
        }
        // shift everything down 
        public void MoveToNextGame()
        {
            SelectedGameIndex = Next1GameIndex;
            GameIndexChanged();
        }

        public void GameIndexChanged()
        {
            if (SelectedVoiceRecognitionResult?.MatchingGames?.Count != null)
            {
                SelectedGame = SelectedVoiceRecognitionResult.MatchingGames[SelectedGameIndex];

                Prev1GameIndex = GetPrevIndex(SelectedGameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Prev1Game = SelectedVoiceRecognitionResult.MatchingGames[Prev1GameIndex];

                Next1GameIndex = GetNextIndex(SelectedGameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Next1Game = SelectedVoiceRecognitionResult.MatchingGames[Next1GameIndex];

                Next2GameIndex = GetNextIndex(Next1GameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Next2Game = SelectedVoiceRecognitionResult.MatchingGames[Next2GameIndex];

                Next3GameIndex = GetNextIndex(Next2GameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Next3Game = SelectedVoiceRecognitionResult.MatchingGames[Next3GameIndex];

                Next4GameIndex = GetNextIndex(Next3GameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Next4Game = SelectedVoiceRecognitionResult.MatchingGames[Next4GameIndex];

                Next5GameIndex = GetNextIndex(Next4GameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Next5Game = SelectedVoiceRecognitionResult.MatchingGames[Next5GameIndex];

                Next6GameIndex = GetNextIndex(Next5GameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Next6Game = SelectedVoiceRecognitionResult.MatchingGames[Next6GameIndex];

                Next7GameIndex = GetNextIndex(Next6GameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Next7Game = SelectedVoiceRecognitionResult.MatchingGames[Next7GameIndex];

                Next8GameIndex = GetNextIndex(Next7GameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Next8Game = SelectedVoiceRecognitionResult.MatchingGames[Next8GameIndex];
            }
        }

        public void NextSearchResultChanged()
        {
            CurrentGameIndex_NextList = 0;
            if (NextVoiceRecognitionResult?.MatchingGames?.Count != null)
            {
                CurrentGame_NextList = NextVoiceRecognitionResult.MatchingGames[CurrentGameIndex_NextList];

                Next1GameIndex_NextList = GetNextIndex(CurrentGameIndex_NextList, NextVoiceRecognitionResult.MatchCount);
                Next1Game_NextList = NextVoiceRecognitionResult.MatchingGames[Next1GameIndex_NextList];

                Next2GameIndex_NextList = GetNextIndex(Next1GameIndex_NextList, NextVoiceRecognitionResult.MatchCount);
                Next2Game_NextList = NextVoiceRecognitionResult.MatchingGames[Next2GameIndex_NextList];

                Next3GameIndex_NextList = GetNextIndex(Next2GameIndex_NextList, NextVoiceRecognitionResult.MatchCount);
                Next3Game_NextList = NextVoiceRecognitionResult.MatchingGames[Next3GameIndex_NextList];

                Next4GameIndex_NextList = GetNextIndex(Next3GameIndex_NextList, NextVoiceRecognitionResult.MatchCount);
                Next4Game_NextList = NextVoiceRecognitionResult.MatchingGames[Next4GameIndex_NextList];

                Next5GameIndex_NextList = GetNextIndex(Next4GameIndex_NextList, NextVoiceRecognitionResult.MatchCount);
                Next5Game_NextList = NextVoiceRecognitionResult.MatchingGames[Next5GameIndex_NextList];

                Next6GameIndex_NextList = GetNextIndex(Next5GameIndex_NextList, NextVoiceRecognitionResult.MatchCount);
                Next6Game_NextList = NextVoiceRecognitionResult.MatchingGames[Next6GameIndex_NextList];

                Next7GameIndex_NextList = GetNextIndex(Next6GameIndex_NextList, NextVoiceRecognitionResult.MatchCount);
                Next7Game_NextList = NextVoiceRecognitionResult.MatchingGames[Next7GameIndex_NextList];

                Next8GameIndex_NextList = GetNextIndex(Next7GameIndex_NextList, NextVoiceRecognitionResult.MatchCount);
                Next8Game_NextList = NextVoiceRecognitionResult.MatchingGames[Next8GameIndex_NextList];
            }
        }


        // shift everything to the left
        public void MoveToPreviousSearchResult()
        {
            SelectedVoiceRecognitionResultIndex = PrevVoiceRecognitionResultIndex;
            SearchResultIndexChanged();
        }

        // shift eveything to the right
        public void MoveToNextSearchResult()
        {
            SelectedVoiceRecognitionResultIndex = NextVoiceRecognitionResultIndex;
            SearchResultIndexChanged();
        }

        // update objects 
        public void SearchResultIndexChanged()
        {
            SelectedVoiceRecognitionResult = VoiceRecognitionResults[SelectedVoiceRecognitionResultIndex];
            PrevVoiceRecognitionResultIndex = GetPrevIndex(SelectedVoiceRecognitionResultIndex, VoiceRecognitionResults.Count);
            PrevVoiceRecognitionResult = VoiceRecognitionResults[PrevVoiceRecognitionResultIndex];
            NextVoiceRecognitionResultIndex = GetNextIndex(SelectedVoiceRecognitionResultIndex, VoiceRecognitionResults.Count);
            NextVoiceRecognitionResult = VoiceRecognitionResults[NextVoiceRecognitionResultIndex];

            SelectedGameIndex = 0;
            GameIndexChanged();
            NextSearchResultChanged();
        }

        public static int GetPrevIndex(int currentIndex, int listCount)
        {
            if(currentIndex == 0)
            {
                return (listCount - 1);
            }
            else
            {
                return (currentIndex - 1);
            }
        }

        public static int GetNextIndex(int currentIndex, int listCount)
        {
            if(currentIndex == (listCount - 1))
            {
                return (0);
            }
            else
            {
                return (currentIndex + 1);
            }
        }

        private int selectedVoiceRecognitionResultIndex;
        public int SelectedVoiceRecognitionResultIndex
        {
            get { return selectedVoiceRecognitionResultIndex; }
            set
            {
                if (selectedVoiceRecognitionResultIndex != value)
                {
                    selectedVoiceRecognitionResultIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedVoiceRecognitionResultIndex"));
                }
            }
        }

        private int nextVoiceRecognitionResultIndex;
        public int NextVoiceRecognitionResultIndex
        {
            get { return nextVoiceRecognitionResultIndex; }
            set
            {
                if (nextVoiceRecognitionResultIndex != value)
                {
                    nextVoiceRecognitionResultIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("NextVoiceRecognitionResultIndex"));
                }
            }
        }

        private int prevVoiceRecognitionResultIndex;
        public int PrevVoiceRecognitionResultIndex
        {
            get { return prevVoiceRecognitionResultIndex; }
            set
            {
                if (prevVoiceRecognitionResultIndex != value)
                {
                    prevVoiceRecognitionResultIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("PrevVoiceRecognitionResultIndex"));
                }
            }
        }

        private VoiceRecognitionResult selectedVoiceRecognitionResult;
        public VoiceRecognitionResult SelectedVoiceRecognitionResult
        {
            get { return selectedVoiceRecognitionResult; }
            set
            {
                if(selectedVoiceRecognitionResult != value)
                {
                    selectedVoiceRecognitionResult = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedVoiceRecognitionResult"));
                }
            }
        }

        private VoiceRecognitionResult nextVoiceRecognitionResult;
        public VoiceRecognitionResult NextVoiceRecognitionResult
        {
            get { return nextVoiceRecognitionResult; }
            set
            {
                if (nextVoiceRecognitionResult != value)
                {
                    nextVoiceRecognitionResult = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("NextVoiceRecognitionResult"));
                }
            }
        }

        private VoiceRecognitionResult prevVoiceRecognitionResult;
        public VoiceRecognitionResult PrevVoiceRecognitionResult
        {
            get { return prevVoiceRecognitionResult; }
            set
            {
                if (prevVoiceRecognitionResult != value)
                {
                    prevVoiceRecognitionResult = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("PrevVoiceRecognitionResult"));
                }
            }
        }

        private MatchingGame selectedGame;
        public MatchingGame SelectedGame
        {
            get { return selectedGame; }
            set
            {
                if(selectedGame != value)
                {
                    selectedGame = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedGame"));
                }
            }
        }

        private int selectedGameIndex;
        public int SelectedGameIndex
        {
            get
            {
                return selectedGameIndex;
            }
            set
            {
                if (selectedGameIndex != value)
                {
                    selectedGameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedGameIndex"));
                }
            }
        }

        private int prev1GameIndex;
        public int Prev1GameIndex
        {
            get { return prev1GameIndex; }
            set
            {
                if (prev1GameIndex != value)
                {
                    prev1GameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Prev1GameIndex"));
                }
            }
        }

        private MatchingGame prev1Game;
        public MatchingGame Prev1Game
        {
            get { return prev1Game; }
            set
            {
                if (prev1Game != value)
                {
                    prev1Game = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Prev1Game"));
                }
            }
        }


        private int next1GameIndex;
        public int Next1GameIndex
        {
            get { return next1GameIndex; }
            set
            {
                if (next1GameIndex != value)
                {
                    next1GameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next1GameIndex"));
                }
            }
        }

        private MatchingGame next1Game;
        public MatchingGame Next1Game
        {
            get { return next1Game; }
            set
            {
                if (next1Game != value)
                {
                    next1Game = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next1Game"));
                }
            }
        }

        private int next2GameIndex;
        public int Next2GameIndex
        {
            get { return next2GameIndex; }
            set
            {
                if (next2GameIndex != value)
                {
                    next2GameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next2GameIndex"));
                }
            }
        }

        private MatchingGame next2Game;
        public MatchingGame Next2Game
        {
            get { return next2Game; }
            set
            {
                if (next2Game != value)
                {
                    next2Game = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next2Game"));
                }
            }
        }

        private int next3GameIndex;
        public int Next3GameIndex
        {
            get { return next3GameIndex; }
            set
            {
                if (next3GameIndex != value)
                {
                    next3GameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next3GameIndex"));
                }
            }
        }

        private MatchingGame next3Game;
        public MatchingGame Next3Game
        {
            get { return next3Game; }
            set
            {
                if (next3Game != value)
                {
                    next3Game = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next3Game"));
                }
            }
        }

        private int next4GameIndex;
        public int Next4GameIndex
        {
            get { return next4GameIndex; }
            set
            {
                if (next4GameIndex != value)
                {
                    next4GameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next4GameIndex"));
                }
            }
        }

        private MatchingGame next4Game;
        public MatchingGame Next4Game
        {
            get { return next4Game; }
            set
            {
                if (next4Game != value)
                {
                    next4Game = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next4Game"));
                }
            }
        }


        private int next5GameIndex;
        public int Next5GameIndex
        {
            get { return next5GameIndex; }
            set
            {
                if (next5GameIndex != value)
                {
                    next5GameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next5GameIndex"));
                }
            }
        }

        private MatchingGame next5Game;
        public MatchingGame Next5Game
        {
            get { return next5Game; }
            set
            {
                if (next5Game != value)
                {
                    next5Game = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next5Game"));
                }
            }
        }

        private int next6GameIndex;
        public int Next6GameIndex
        {
            get { return next6GameIndex; }
            set
            {
                if (next6GameIndex != value)
                {
                    next6GameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next6GameIndex"));
                }
            }
        }

        private MatchingGame next6Game;
        public MatchingGame Next6Game
        {
            get { return next6Game; }
            set
            {
                if (next6Game != value)
                {
                    next6Game = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next6Game"));
                }
            }
        }

        private int next7GameIndex;
        public int Next7GameIndex
        {
            get { return next7GameIndex; }
            set
            {
                if (next7GameIndex != value)
                {
                    next7GameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next7GameIndex"));
                }
            }
        }

        private MatchingGame next7Game;
        public MatchingGame Next7Game
        {
            get { return next7Game; }
            set
            {
                if (next7Game != value)
                {
                    next7Game = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next7Game"));
                }
            }
        }

        private int next8GameIndex;
        public int Next8GameIndex
        {
            get { return next8GameIndex; }
            set
            {
                if (next8GameIndex != value)
                {
                    next8GameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next8GameIndex"));
                }
            }
        }
        private MatchingGame next8Game;
        public MatchingGame Next8Game
        {
            get { return next8Game; }
            set
            {
                if (next8Game != value)
                {
                    next8Game = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next8Game"));
                }
            }
        }


        private int currentGameIndex_NextList;
        public int CurrentGameIndex_NextList
        {
            get { return currentGameIndex_NextList; }
            set
            {
                if (currentGameIndex_NextList != value)
                {
                    currentGameIndex_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentGameIndex_NextList"));
                }
            }
        }
        private MatchingGame currentGame_NextList;
        public MatchingGame CurrentGame_NextList
        {
            get { return currentGame_NextList; }
            set
            {
                if (currentGame_NextList != value)
                {
                    currentGame_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentGame_NextList"));
                }
            }
        }

        private int next1GameIndex_NextList;
        public int Next1GameIndex_NextList
        {
            get { return next1GameIndex_NextList; }
            set
            {
                if (next1GameIndex_NextList != value)
                {
                    next1GameIndex_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next1GameIndex_NextList"));
                }
            }
        }
        private MatchingGame next1Game_NextList;
        public MatchingGame Next1Game_NextList
        {
            get { return next1Game_NextList; }
            set
            {
                if (next1Game_NextList != value)
                {
                    next1Game_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next1Game_NextList"));
                }
            }
        }

        private int next2GameIndex_NextList;
        public int Next2GameIndex_NextList
        {
            get { return next2GameIndex_NextList; }
            set
            {
                if (next2GameIndex_NextList != value)
                {
                    next2GameIndex_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next2GameIndex_NextList"));
                }
            }
        }
        private MatchingGame next2Game_NextList;
        public MatchingGame Next2Game_NextList
        {
            get { return next2Game_NextList; }
            set
            {
                if (next2Game_NextList != value)
                {
                    next2Game_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next2Game_NextList"));
                }
            }
        }


        private int next3GameIndex_NextList;
        public int Next3GameIndex_NextList
        {
            get { return next3GameIndex_NextList; }
            set
            {
                if (next3GameIndex_NextList != value)
                {
                    next3GameIndex_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next3GameIndex_NextList"));
                }
            }
        }
        private MatchingGame next3Game_NextList;
        public MatchingGame Next3Game_NextList
        {
            get { return next3Game_NextList; }
            set
            {
                if (next3Game_NextList != value)
                {
                    next3Game_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next3Game_NextList"));
                }
            }
        }

        private int next4GameIndex_NextList;
        public int Next4GameIndex_NextList
        {
            get { return next4GameIndex_NextList; }
            set
            {
                if (next4GameIndex_NextList != value)
                {
                    next4GameIndex_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next4GameIndex_NextList"));
                }
            }
        }
        private MatchingGame next4Game_NextList;
        public MatchingGame Next4Game_NextList
        {
            get { return next4Game_NextList; }
            set
            {
                if (next4Game_NextList != value)
                {
                    next4Game_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next4Game_NextList"));
                }
            }
        }


        private int next5GameIndex_NextList;
        public int Next5GameIndex_NextList
        {
            get { return next5GameIndex_NextList; }
            set
            {
                if (next5GameIndex_NextList != value)
                {
                    next5GameIndex_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next5GameIndex_NextList"));
                }
            }
        }
        private MatchingGame next5Game_NextList;
        public MatchingGame Next5Game_NextList
        {
            get { return next5Game_NextList; }
            set
            {
                if (next5Game_NextList != value)
                {
                    next5Game_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next5Game_NextList"));
                }
            }
        }

        private int next6GameIndex_NextList;
        public int Next6GameIndex_NextList
        {
            get { return next6GameIndex_NextList; }
            set
            {
                if (next6GameIndex_NextList != value)
                {
                    next6GameIndex_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next6GameIndex_NextList"));
                }
            }
        }
        private MatchingGame next6Game_NextList;
        public MatchingGame Next6Game_NextList
        {
            get { return next6Game_NextList; }
            set
            {
                if (next6Game_NextList != value)
                {
                    next6Game_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next6Game_NextList"));
                }
            }
        }

        private int next7GameIndex_NextList;
        public int Next7GameIndex_NextList
        {
            get { return next7GameIndex_NextList; }
            set
            {
                if (next7GameIndex_NextList != value)
                {
                    next7GameIndex_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next7GameIndex_NextList"));
                }
            }
        }
        private MatchingGame next7Game_NextList;
        public MatchingGame Next7Game_NextList
        {
            get { return next7Game_NextList; }
            set
            {
                if (next7Game_NextList != value)
                {
                    next7Game_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next7Game_NextList"));
                }
            }
        }

        private int next8GameIndex_NextList;
        public int Next8GameIndex_NextList
        {
            get { return next8GameIndex_NextList; }
            set
            {
                if (next8GameIndex_NextList != value)
                {
                    next8GameIndex_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next8GameIndex_NextList"));
                }
            }
        }
        private MatchingGame next8Game_NextList;
        public MatchingGame Next8Game_NextList
        {
            get { return next8Game_NextList; }
            set
            {
                if (next8Game_NextList != value)
                {
                    next8Game_NextList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Next8Game_NextList"));
                }
            }
        }







        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsActive"));
                }
            }
        }

        private bool isInitializing;
        public bool IsInitializing
        {
            get { return isInitializing; }
            set
            {
                if (isInitializing != value)
                {
                    isInitializing = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsInitializing"));
                }
            }
        }

        private bool isRecognizing;
        public bool IsRecognizing
        {
            get { return isRecognizing; }
            set
            {
                if (isRecognizing != value)
                {
                    isRecognizing = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsRecognizing"));
                }
            }
        }

        private bool isDisplayingResults;
        public bool IsDisplayingResults
        {
            get { return isDisplayingResults; }
            set
            {
                if (isDisplayingResults != value)
                {
                    isDisplayingResults = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsDisplayingResults"));
                }
            }
        }

        private bool isDisplayingSearchError;
        public bool IsDisplayingSearchError 
        {
            get
            {
                return isDisplayingSearchError;
            }
            set
            {
                if (isDisplayingSearchError != value)
                {
                    isDisplayingSearchError = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsDisplayingSearchError"));
                }
            }
        }

        private List<VoiceRecognitionResult> voiceRecognitionResults;
        public List<VoiceRecognitionResult> VoiceRecognitionResults
        {
            get { return voiceRecognitionResults; }
            set
            {
                if (voiceRecognitionResults != value)
                {
                    voiceRecognitionResults = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("VoiceRecognitionResults")); 
                }
            }
        }

        private string searchErrorText;
        public string SearchErrorText
        {
            get { return searchErrorText; }
            set
            {
                if (searchErrorText != value)
                {
                    searchErrorText = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SearchErrorText"));
                }
            }
        }

        public Uri VoiceRecognitionGif
        {
            get
            {
                return new Uri("pack://application:,,,/BigBoxVoiceSearchPlugin;component/resources/VoiceRecognitionGif.gif");
            }
        }
    }
}
 