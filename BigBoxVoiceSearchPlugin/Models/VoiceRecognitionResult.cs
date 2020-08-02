using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace BigBoxVoiceSearchPlugin.Models
{
    public class VoiceRecognitionResult : INotifyPropertyChanged
    {
        private float confidence;
        private string recognizedPhrase;
        private List<MatchingGame> matchingGames;

        public List<MatchingGame> MatchingGames 
        { 
            get => matchingGames;
            set
            {
                if (matchingGames != value)
                {
                    matchingGames = value;
                    NotifyPropertyChanged("MatchingGames");
                }
            }
        }

        public string RecognizedPhrase
        {
            get => recognizedPhrase;
            set
            {
                if (recognizedPhrase != value)
                {
                    recognizedPhrase = value;
                    NotifyPropertyChanged("RecognizedPhrase");
                }
            }
        }

        public float Confidence
        {
            get => confidence;
            set
            {
                if (confidence != value)
                {
                    confidence = value;
                    NotifyPropertyChanged("Confidence");
                }
            }
        }

        public MatchLevel MaxMatchLevel
        {
            get
            {
                if (MatchingGames == null)
                    return MatchLevel.None;

                return MatchingGames.OrderBy(s => s.MatchLevel).FirstOrDefault().MatchLevel;
            }
        }

        public VoiceRecognitionResult()
        {
            MatchingGames = new List<MatchingGame>();
        }

        public Brush Brush
        {
            get
            {
                if (Confidence <= 0.50)
                    return new SolidColorBrush(Colors.Red);

                if (Confidence <= 0.60)
                    return new SolidColorBrush(Colors.Orange);

                if (Confidence <= 0.70)
                    return new SolidColorBrush(Colors.Yellow);

                if (Confidence <= 0.80)
                    return new SolidColorBrush(Colors.YellowGreen);

                if (Confidence <= 0.90)
                    return new SolidColorBrush(Colors.GreenYellow);

                return new SolidColorBrush(Colors.Green);
            }
        }

        public int MatchCount
        {
            get
            {
                if (MatchingGames == null)
                {
                    return (0);
                }

                return MatchingGames.Count();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}