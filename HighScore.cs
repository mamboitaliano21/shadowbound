using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace Lab
{
    public class HighScore
    {
        public int score { get; set; }
        public string name { get; set; }
        public HighScore(int score, string name)
        {
            this.score = score;
            this.name = name;
        }
    }
}
