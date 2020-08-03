using System;
using System.Collections.Generic;
using System.Linq;
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
        public MatchLevel MatchLevel { get; set; }

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

                return (frontImage);
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

        public MatchingGame(IGame game, MatchLevel matchLevel)
        {
            Id = game.Id;
            LaunchBoxDbId = game.LaunchBoxDbId;            
            Title = game.Title;
            Platform = game.Platform;
            PlatformClearLogoPath = game.PlatformClearLogoImagePath;
            ClearLogoPath = game.ClearLogoImagePath;
            FrontImagePath = game.FrontImagePath;
            VideoPath = game.VideoPath;
            BackgroundImagePath = game.BackgroundImagePath;
            MatchLevel = matchLevel;
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