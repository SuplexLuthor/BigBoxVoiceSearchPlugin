using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBoxVoiceSearchPlugin.Models
{
    [Serializable]
    public class GameMatch
    {
        public string Id { get; set; }
        public MatchLevel MatchLevel { get; set; }

        public GameMatch()
        {
        }

        public GameMatch(string id, MatchLevel matchLevel)
        {
            Id = id;
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
                GameMatch other = obj as GameMatch;
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