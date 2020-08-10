using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBoxVoiceSearchPlugin.Models
{
    [Serializable]
    public enum MatchLevel
    {
        FullTitleMatch = 0,
        MainTitleMatch = 1,
        SubtitleMatch = 2,
        FullTitleContains = 3, 
        None = 100
    }
}
