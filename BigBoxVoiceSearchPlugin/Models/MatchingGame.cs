using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Unbroken.LaunchBox.Plugins.Data;
using Unbroken.LaunchBox.Plugins.RetroAchievements;

namespace BigBoxVoiceSearchPlugin.Models
{

    [Serializable]
    public class MatchingGame
    {
        public string Id { get; set; }
        public int? LaunchBoxDbId { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public string FrontImagePath { get; set; }
        public string ClearLogoPath { get; set; }
        public string VideoPath { get; set; }
        public string PlatformClearLogoPath { get; set; }
        public string BackgroundImagePath { get; set; }
        public string ScreenshotImagePath { get; set; }
        public MatchLevel MatchLevel { get; set; }
        public int? ReleaseYear { get; set; }
        public string DetailsWithPlatform { get; set; }
        public string DetailsWithoutPlatform { get; set; }
        public float Confidence { get; set; }
        private DateTime? releaseDate { get; set; }

        public string ReleaseDate 
        {
            get
            {
                if(releaseDate?.Year == null)
                {
                    return ("");
                }
                return (releaseDate?.Year.ToString());
            }
        }

        [NonSerialized()]
        private BitmapImage backgroundImage;
        public BitmapImage BackgroundImage
        {
            get
            {
                if(backgroundImage == null)
                {
                    if(!string.IsNullOrWhiteSpace(BackgroundImagePath))
                    {
                        backgroundImage = new BitmapImage(new Uri(BackgroundImagePath));
                    }
                }
                return backgroundImage;
            }
        }

        private double communityStarRating;
        public Uri CommunityStarRatingImage
        {
            get
            {
                {
                    string path = $"pack://application:,,,/BigBoxVoiceSearchPlugin;component/resources/StarRating/{communityStarRating}.png";
                    return new Uri(path);
                }
            }
        }

        private string playMode;
        public Uri PlayModeImage
        {
            get
            {
                string path = $"pack://application:,,,/BigBoxVoiceSearchPlugin;component/resources/PlayMode/{playMode}.png";
                return new Uri(path);
            }
        }


        [NonSerialized()]
        private BitmapImage frontImage;
        public BitmapImage FrontImage 
        { 
            get
            {
                if(frontImage == null)
                {
                    if (!string.IsNullOrWhiteSpace(FrontImagePath))
                    {
                        frontImage = new BitmapImage(new Uri(FrontImagePath));
                    }
                }

                return frontImage;
            }
        }

        [NonSerialized()]
        private BitmapImage clearLogo;
        public BitmapImage ClearLogo
        {
            get
            {
                if(clearLogo == null)
                {
                    if (!string.IsNullOrWhiteSpace(ClearLogoPath))
                    {
                        clearLogo = new BitmapImage(new Uri(ClearLogoPath));
                    }
                }
                return clearLogo;
            }
        }

        [NonSerialized()]
        private BitmapImage screenshotImage;
        public BitmapImage ScreenshotImage
        {
            get
            {
                if(screenshotImage == null)
                {
                    if(!string.IsNullOrWhiteSpace(ScreenshotImagePath))
                    {
                        screenshotImage = new BitmapImage(new Uri(ScreenshotImagePath));
                    }
                }
                return screenshotImage;
            }
        }

        [NonSerialized()]
        private BitmapImage platformClearLogo;
        public BitmapImage PlatformClearLogo
        {
            get
            {
                if(platformClearLogo == null)
                {
                    if(!string.IsNullOrWhiteSpace(PlatformClearLogoPath))
                    {
                        platformClearLogo = new BitmapImage(new Uri(PlatformClearLogoPath));
                    }
                }
                return (platformClearLogo);
            }
        }

        private double matchPercentage;
        public string MatchPercentage
        {
            get { return $"{matchPercentage}% match"; }
        }

        public string Notes { get; set; }

        

        public MatchingGame(IGame game, MatchLevel matchLevel, float confidence = 0)
        {
            Id = game.Id;
            LaunchBoxDbId = game.LaunchBoxDbId;            
            Title = game.Title;
            Platform = game.Platform;
            PlatformClearLogoPath = game.PlatformClearLogoImagePath;
            ClearLogoPath = game.ClearLogoImagePath;
            FrontImagePath = game.FrontImagePath;
            VideoPath = game.GetVideoPath();
            BackgroundImagePath = game.BackgroundImagePath;
            ScreenshotImagePath = game.ScreenshotImagePath;
            MatchLevel = matchLevel;
            ReleaseYear = game.ReleaseYear;
            DetailsWithoutPlatform = game.DetailsWithoutPlatform;
            DetailsWithPlatform = game.DetailsWithPlatform;
            Confidence = confidence;
            releaseDate = game.ReleaseDate;
            Notes = game.Notes;
            communityStarRating = Math.Round(game.CommunityStarRating,1);
            playMode = game.PlayMode;
            Helpers.Log($"Game {game.Title} Star Rating {communityStarRating}");

            matchPercentage = Math.Round(100 * (Confidence + (1 - ((int)MatchLevel / 4))) / 2, 0, MidpointRounding.AwayFromZero);
        }
        
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                MatchingGame other = obj as MatchingGame;
                if (other == null)
                {
                    return (false);
                }

                return (Id == other.Id);
            }
        }

        public override int GetHashCode()
        {
            return (Id.GetHashCode());
        }
    }
}