using BigBoxVoiceSearchPlugin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Windows.Controls;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;
using Unbroken.LaunchBox.Plugins.RetroAchievements;

namespace BigBoxVoiceSearchPlugin.MainWindowView
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : UserControl, IBigBoxThemeElementPlugin
    {
        private Dictionary<string, List<GameMatch>> GameTitleGames;
        private MainWindowViewModel mainWindowViewModel;
        private IGame[] AllGames;
        public MainWindowView()
        {
            InitializeComponent();
            mainWindowViewModel = this.DataContext as MainWindowViewModel;
            mainWindowViewModel.IsInitializing = true;
            AllGames = PluginHelper.DataManager.GetAllGames();
            BuildGameTitleDictionary();
            CreateRecognizer();
            mainWindowViewModel.IsInitializing = false;
        }
        

        public void BuildGameTitleDictionary()
        {
            try
            {
                GameTitleGames = Helpers.ReadGamesDatabaseAsJson();
                if(GameTitleGames != null)
                {
                    return;
                }
            }
            catch(Exception ex)
            {
                Helpers.LogException(ex, "reading games database");
            }

            GameTitleGames = new Dictionary<string, List<GameMatch>>();
            foreach (IGame game in AllGames)
            {
                GameTitleGrammarBuilder gameTitleGrammarBuilder = new GameTitleGrammarBuilder(game);
                if (!string.IsNullOrWhiteSpace(gameTitleGrammarBuilder.Title))
                {
                    AddGame(gameTitleGrammarBuilder.Title, new GameMatch(game.Id, MatchLevel.FullTitleMatch));
                }

                if (!string.IsNullOrWhiteSpace(gameTitleGrammarBuilder.MainTitle))
                {
                    AddGame(gameTitleGrammarBuilder.MainTitle, new GameMatch(game.Id, MatchLevel.MainTitleMatch));
                }

                if (!string.IsNullOrWhiteSpace(gameTitleGrammarBuilder.Subtitle))
                {
                    AddGame(gameTitleGrammarBuilder.Subtitle, new GameMatch(game.Id, MatchLevel.SubtitleMatch));
                }

                for (int i = 0; i < gameTitleGrammarBuilder.TitleWords.Count; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int j = i; j < gameTitleGrammarBuilder.TitleWords.Count; j++)
                    {
                        sb.Append($"{gameTitleGrammarBuilder.TitleWords[j]} ");
                        if (!GameTitleGrammarBuilder.IsNoiseWord(sb.ToString().Trim()))
                        {
                            AddGame(sb.ToString().Trim(), new GameMatch(game.Id, MatchLevel.FullTitleContains));
                        }
                    }
                }
            }

            try
            {
                Helpers.WriteGamesDatabaseAsJson(GameTitleGames);
            }
            catch (Exception ex)
            {
                Helpers.LogException(ex, "reading games database");
            }
        }

        private void AddGame(string phrase, GameMatch gameMatch)
        {
            if(GameTitleGrammarBuilder.IsNoiseWord(phrase))
            {
                return;
            }

            // add the phrase if it's not in the dictionary
            if (!GameTitleGames.ContainsKey(phrase))
            {
                GameTitleGames.Add(phrase, new List<GameMatch>());
            }

            // add the game if it's not already in the collection of matching games
            if (!GameTitleGames[phrase].Contains(gameMatch))
            {
                GameTitleGames[phrase].Add(gameMatch);
            }
        }


        public bool OnUp(bool held)
        {
            if (!mainWindowViewModel.IsActive) return false;

            mainWindowViewModel.MoveToPreviousSearchResult();

            return true;
        }

        public bool OnDown(bool held)
        {
            if(!mainWindowViewModel.IsActive) return false;

            mainWindowViewModel.MoveToNextSearchResult();

            return true;
        }

        public bool OnLeft(bool held)
        {
            if (!mainWindowViewModel.IsActive) return false;

            mainWindowViewModel.MoveToPreviousGame();

            return true;
        }

        public bool OnRight(bool held)
        {
            if (!mainWindowViewModel.IsActive) return false;

            mainWindowViewModel.MoveToNextGame();

            return true;
        }

        public bool OnPageDown()
        {
            mainWindowViewModel.IsActive = true;
            DoRecognize();
            return true;
        }

        public bool OnPageUp()
        {
            mainWindowViewModel.IsActive = true;
            DoRecognize();
            return true;
        }

        public bool OnEnter()
        {
            if (!mainWindowViewModel.IsActive) return false;
            return true;
        }

        public bool OnEscape()
        {
            if (!mainWindowViewModel.IsActive) return false;

            mainWindowViewModel.IsActive = false;
            return true;
        }

        public void OnSelectionChanged(FilterType filterType, string filterValue, IPlatform platform, IPlatformCategory category, IPlaylist playlist, IGame game)
        {
            
        }

        public SpeechRecognitionEngine Recognizer { get; set; }
        public List<VoiceRecognitionResult> VoiceRecognitionResultsTemp { get; set; }

        private bool CreateRecognizer()
        {
            // todo: testing with dictionary
            List<string> titleElements = new List<string>(GameTitleGames.Keys);

            // add the distinct phrases to the list of choices
            Choices choices = new Choices();
            choices.Add(titleElements.ToArray());

            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(choices);

            Grammar grammar = new Grammar(grammarBuilder)
            {
                Name = "Game title elements"
            };

            // setup the recognizer
            Recognizer = new SpeechRecognitionEngine();
            Recognizer.InitialSilenceTimeout = TimeSpan.FromSeconds(5.0);
            Recognizer.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(RecognizeCompleted);
            Recognizer.LoadGrammarAsync(grammar);
            Recognizer.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(SpeechHypothesized);
            Recognizer.SetInputToDefaultAudioDevice();
            Recognizer.RecognizeAsyncCancel();
            return (true);
        }

        public void DoRecognize()
        {
            if (mainWindowViewModel.IsRecognizing)
                return;

            if (mainWindowViewModel.IsInitializing)
                return;

            // reset status to display recognizing
            mainWindowViewModel.IsRecognizing = true;
            mainWindowViewModel.IsDisplayingResults = false;
            mainWindowViewModel.IsDisplayingSearchError = false;

            // reset the results and the temporary results
            VoiceRecognitionResultsTemp = new List<VoiceRecognitionResult>();

            // kick off voice recognition 
            Recognizer.RecognizeAsync(RecognizeMode.Single);
        }

        // add anything that isn't a noise word - group results once recognition is complete and take highest confidence for any duplicates
        void SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            // ignore noise words 
            if (!GameTitleGrammarBuilder.IsNoiseWord(e.Result.Text))
            {
                VoiceRecognitionResultsTemp.Add(new VoiceRecognitionResult
                {
                    RecognizedPhrase = e.Result.Text,
                    Confidence = e.Result.Confidence
                });
            }
        }

        // once recognition is completed, match the voice recognition result against the games list
        void RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            mainWindowViewModel.IsRecognizing = false;
            mainWindowViewModel.VoiceRecognitionResults = new List<VoiceRecognitionResult>();

            if (e?.Error != null)
            {
                if (Recognizer != null)
                {
                    Recognizer.RecognizeAsyncCancel();
                }
                mainWindowViewModel.IsDisplayingSearchError = true;
                mainWindowViewModel.SearchErrorText = e.Error.Message;
                return;
            }

            if (e?.InitialSilenceTimeout == true || e?.BabbleTimeout == true)
            {
                if (Recognizer != null)
                {
                    Recognizer.RecognizeAsyncCancel();
                }
                mainWindowViewModel.IsDisplayingSearchError = true;
                mainWindowViewModel.SearchErrorText = "Voice recognition could not hear anything, please try again";
                return;
            }


            if (VoiceRecognitionResultsTemp?.Count() > 0)
            {
                // in case the same phrase was recognized multiple times, group by phrase and keep only the max confidence
                var distinctRecognizedPhrases = VoiceRecognitionResultsTemp
                    .GroupBy(s => s.RecognizedPhrase)
                    .Select(s => new VoiceRecognitionResult { RecognizedPhrase = s.Key, Confidence = s.Max(m => m.Confidence) }).ToList();

                foreach (var voiceRecognitionResult in distinctRecognizedPhrases)
                {
                    Helpers.Log($"Recognize completed.  Processing phrase: {voiceRecognitionResult.RecognizedPhrase}");

                    List<GameMatch> gameMatches;
                    if(GameTitleGames.TryGetValue(voiceRecognitionResult.RecognizedPhrase, out gameMatches))
                    {
                        Helpers.Log($"Found {voiceRecognitionResult.RecognizedPhrase} in game titles dictionary with {gameMatches.Count} matches");

                        var matchingGames = from gameMatch in gameMatches
                                    join game in AllGames
                                    on gameMatch.Id equals game.Id
                                    select new MatchingGame(game, gameMatch.MatchLevel);

                        Helpers.Log($"Got matching games for {voiceRecognitionResult.RecognizedPhrase}");

                        matchingGames = matchingGames.OrderBy(s => s.MatchLevel).ThenBy(s => s.Title);

                        Helpers.Log($"Performed ordering of {voiceRecognitionResult.RecognizedPhrase}");

                        voiceRecognitionResult.MatchingGames = new List<MatchingGame>(matchingGames);

                        Helpers.Log($"Assigned results for {voiceRecognitionResult.RecognizedPhrase}");
                    }
                }

                Helpers.Log($"Ordering final results");

                mainWindowViewModel.VoiceRecognitionResults = 
                    new List<VoiceRecognitionResult>(distinctRecognizedPhrases
                                                    .OrderBy(s => s.MaxMatchLevel)
                                                    .ThenBy(s => s.RecognizedPhrase.Length)
                                                    .ThenBy(s => s.Confidence));
            }

            mainWindowViewModel.IsDisplayingResults = true;
            mainWindowViewModel.SelectedVoiceRecognitionResultIndex = 0;
            mainWindowViewModel.SearchResultIndexChanged();
        }
    }
}
