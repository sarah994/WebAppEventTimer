using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GuildWars2WorldBossTimer.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string[,] List = new string[114, 5];

        public MainPageViewModel()
        {
            this.Items = new ObservableCollection<MainPageViewModel>();
        }
        public ObservableCollection<MainPageViewModel> Items { get; private set; }

        public void LoadData()
        {
            int i = 0;

            XDocument doc = XDocument.Load("Data/test1.xml", LoadOptions.None);

            var bosses = from boss in doc.Descendants("boss")
                         //where (double)boss.Element("utc") > 150
                         orderby (string)boss.Element("nr") ascending
                         select new
                         {
                             name = (string)boss.Element("name"),
                             utc = (string)boss.Element("utc"),
                             image_pre = (string)boss.Element("image_pre"),
                             image_location = (string)boss.Element("image_location"),
                             image_boss = (string)boss.Element("image_boss")
                         };

            foreach (var boss in bosses)
            {
                List[i, 0] = boss.name;
                List[i, 1] = boss.utc;
                List[i, 2] = boss.image_pre;
                List[i, 3] = boss.image_location;
                List[i, 4] = boss.image_boss;
                i++;
            }
        }

        public int BossUp()
        {
            string currentTime = DateTime.Now.ToString("HH:mm");
            string[] currentTimeDivide = currentTime.Split(':');
            int currentHour = Convert.ToInt32(currentTimeDivide[0]);
            int currentMinute = Convert.ToInt32(currentTimeDivide[1]);

            bool foundTimeUp = false;
            string eventTime;
            int upTimeIndex = 0;
            int eventHour, eventMinute;

            while (foundTimeUp == false)
            {
                eventTime = List[upTimeIndex, 1];
                string[] eventTimeDivide = eventTime.Split(':');
                eventHour = Convert.ToInt32(eventTimeDivide[0]);
                eventMinute = Convert.ToInt32(eventTimeDivide[1]);
                if (eventHour == 00)
                {
                    if ((eventMinute <= 00) && (eventMinute > (00 - 14)))
                    {
                        foundTimeUp = true;
                    }
                }
            }

            return upTimeIndex;
        }

        private string _bossName;
        public string BossName
        {
            get
            { return _bossName; }
            set
            {
                if (value != _bossName)
                {
                    _bossName = List[BossUp(), 0];
                    NotifyPropertyChanged("BossName");
                }
            }
        }

        private string _utc;
        public string Utc
        {
            get
            { return _utc; }
            set
            {
                if (value != _utc)
                {
                    _utc = List[BossUp(), 1];
                    NotifyPropertyChanged("Utc");
                }
            }
        }

        private string _nearestWaypoint;
        public string NearestWaypoint
        {
            get
            { return _nearestWaypoint; }
            set
            {
                if (value != _nearestWaypoint)
                {
                    _nearestWaypoint = "";
                    NotifyPropertyChanged("NearestWaypoint");
                }
            }
        }

        private string _bossPre;
        public string BossPre
        {
            get
            { return _bossPre; }
            set
            {
                if (value != _bossPre)
                {
                    _bossPre = List[BossUp(), 2];
                    NotifyPropertyChanged("BossPre");
                }
            }
        }

        private string _bossPlace;
        public string BossPlace
        {
            get
            { return _bossPlace; }
            set
            {
                if (value != _bossPlace)
                {
                    _bossPlace = List[BossUp(), 3];
                    NotifyPropertyChanged("BossPlace");
                }
            }
        }

        private string _bossLook;
        public string BossLook
        {
            get
            { return _bossLook; }
            set
            {
                if (value != _bossLook)
                {
                    _bossLook = List[BossUp(), 4];
                    NotifyPropertyChanged("BossLook");
                }
            }
        }

        private string _timeTillEvent;
        public string TimeTillEvent
        {
            get
            { return _timeTillEvent; }
            set
            {
                if (value != _timeTillEvent)
                {
                    _timeTillEvent = "15:00:00";
                    NotifyPropertyChanged("TimeTillEvent");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
