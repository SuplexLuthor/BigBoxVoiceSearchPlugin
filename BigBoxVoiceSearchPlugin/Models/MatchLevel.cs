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
        FullTitleStartsWith = 3,
        SubtitleStartsWith = 4,
        FullTitleContains = 5, 
        None = 100
    }
}
