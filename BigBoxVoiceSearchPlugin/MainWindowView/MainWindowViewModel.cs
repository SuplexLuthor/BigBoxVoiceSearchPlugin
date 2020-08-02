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

                Prev2GameIndex = GetPrevIndex(Prev1GameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Prev2Game = SelectedVoiceRecognitionResult.MatchingGames[Prev2GameIndex];

                Next1GameIndex = GetNextIndex(SelectedGameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Next1Game = SelectedVoiceRecognitionResult.MatchingGames[Next1GameIndex];

                Next2GameIndex = GetNextIndex(Next1GameIndex, SelectedVoiceRecognitionResult.MatchCount);
                Next2Game = SelectedVoiceRecognitionResult.MatchingGames[Next2GameIndex];
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


        private int prev2GameIndex;
        public int Prev2GameIndex
        {
            get { return prev2GameIndex; }
            set
            {
                if (prev2GameIndex != value)
                {
                    prev2GameIndex = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Prev2GameIndex"));
                }
            }
        }

        private MatchingGame prev2Game;
        public MatchingGame Prev2Game
        {
            get { return prev2Game; }
            set
            {
                if (prev2Game != value)
                {
                    prev2Game = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Prev2Game"));
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
 