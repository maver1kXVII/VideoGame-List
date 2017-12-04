using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VG_List
{
    public class VGame
    {
        public string Title { get; set; }
        public string AltTitle { get; set; }
        public string Platform { get; set; }
        public string Category { get; set; }

        private int score;
        public int Score 
        { 
            get { return score; }
            set 
            { 
                if (value > 10) score = 10;
                else
                {
                    if (value < -1) score = -1;
                    else score = value;
                }
            } 
        }

        private VGStatus status;
        public VGStatus Status
        {
            get { return status; }
            set
            {
                if ((value == VGStatus.Пройдена) && (DateFinished.Year == 1))
                {
                    status = value;
                    DateFinished = DateTime.Today;
                }
                else status = value;
            }
        }

        [XmlIgnore]
        public string ScoreStr
        {
            get 
            {
                if (Score > -1) return Score + "/10";
                else return " – "; 
            }
            set { }
        }

        [XmlIgnore]
        public string DateFinishedStr {
            get { return Date2SortStr(DateFinished); }
            set {  }
        }

        [XmlIgnore]
        public string DateAddedStr
        {
            get
            {
                return Date2SortStr(DateAdded);
            }
            set { }
        }

        public DateTime DateFinished { get; set; }

        public DateTime DateAdded { get; set; }
        public VGame(string Title, string Platform, VGStatus Status)
        {
            this.Title = Title;
            this.Platform = Platform;
            this.Status = Status;
            Init();
        }

        public VGame()
        {
            this.Title = "";
            this.Platform = "PC";
            this.Status = VGStatus.Играю;
            this.Score = -1;
            Init();
        }

        private void Init()
        {
            DateAdded = DateTime.Today;
        }

        private string Date2SortStr(DateTime dt)
        {
            return dt.Year + "." + dt.ToShortDateString().Substring(3, 2) + "." +
                    dt.ToShortDateString().Substring(0, 2);
        }
    }
}
