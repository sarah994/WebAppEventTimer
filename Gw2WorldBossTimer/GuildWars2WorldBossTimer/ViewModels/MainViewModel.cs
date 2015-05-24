using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Net;
using Microsoft.Phone.Net.NetworkInformation;
using WindowsPhoneLibrary;

namespace GuildWars2WorldBossTimer.ViewModels
{
    public class MainViewModel
    {
        public delegate void StartEventHandler();
        public event StartEventHandler Start;
        private const string urlString = "http://raw.githubusercontent.com/sarah994/TimerDataXml/master/boss_timers.xml";
        private const string filename = "boss_timers.xml";

        private FileIO io = new FileIO();

        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public void LoadData()
        {
            string active;
            if (io.LoadTextFromSettings("activation", out active)==false)
            {
                DownloadFeed();
            }
            else
            {
                ReadFeed();
            }
        }

        private void DownloadFeed()
        {
            if (DeviceNetworkInformation.IsNetworkAvailable)
            {
                WebClient client = new WebClient();
                client.DownloadStringCompleted += client_DownloadStringCompleted;
                client.DownloadStringAsync(new Uri(urlString));
            }
            io.SaveTextToSettings("activation", "true");
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string xmlText;

            if (e.Error != null)
                return;
            xmlText = e.Result;

            io.SaveTextToFile(filename, xmlText);

            ParseFeed(xmlText);
            if (Start != null)
            {
                Start();
            }
        }

        private void ReadFeed()
        {
            string xmlText;

            if (io.LoadTextFromFile(filename, out xmlText))
            {
                ParseFeed(xmlText);
                if (Start != null)
                {
                    Start();
                }
            }
        }

        private void ParseFeed(string xmlText)
        {
            XDocument doc = XDocument.Parse(xmlText);

            var bosses = from boss in doc.Descendants("boss")
                         orderby (int)boss.Element("nr") ascending
                         select new
                         {
                             name = (string)boss.Element("name"),
                             utc = (string)boss.Element("utc"),
                             waypoint = (string)boss.Element("waypoint"),
                             location = (string)boss.Element("location"),
                             pre = (string)boss.Element("pre"),
                             preWaypoint = (string)boss.Element("pre_waypoint"),
                             image_boss = (string)boss.Element("image_boss")
                         };

            foreach (var boss in bosses)
            {
                Uri uri = new Uri(boss.image_boss, UriKind.Relative);
                BitmapImage source = new BitmapImage(uri);
                this.Items.Add(new ItemViewModel()
                {
                    BossName = boss.name,
                    Utc = boss.utc,
                    Waypoint = boss.waypoint,
                    Location = boss.location,
                    Pre = boss.pre,
                    PreWaypoint = boss.preWaypoint,
                    BossLook = source
                });
            }
        }

        public int indexEventUp()
        {
            string currentTime = DateTime.UtcNow.ToString("HH:mm");
            string[] currentTimeDivide = currentTime.Split(':');
            int currentHour = Convert.ToInt32(currentTimeDivide[0]);
            int currentMinute = Convert.ToInt32(currentTimeDivide[1]);

            bool foundTimeUp = false;
            int upTimeIndex = -1;
            string eventTime;
            int eventHour, eventMinute;

            while (foundTimeUp == false)
            {
                upTimeIndex++;
                eventTime = this.Items[upTimeIndex].Utc;
                string[] eventTimeDivide = eventTime.Split(':');
                eventHour = Convert.ToInt32(eventTimeDivide[0]);
                eventMinute = Convert.ToInt32(eventTimeDivide[1]);
                if ((eventHour == currentHour) && (currentMinute < 50))
                {
                    if ((eventMinute + 5 >= currentMinute) && (eventMinute - 15 < currentMinute) && (currentMinute >= 5))
                    {
                        foundTimeUp = true;
                    }
                }
                if (((eventHour - 1 == currentHour) && (currentMinute >= 50)) || ((eventHour == currentHour) && (currentMinute < 5)))
                {
                    foundTimeUp = true;
                }
            }

            return upTimeIndex;
        }

        public ItemViewModel BossUp()
        {
            int index = indexEventUp();
            this.Items[index].TimeTillEvent = CheckTimer(index);
            return this.Items[indexEventUp()];
        }

        public ItemViewModel Boss2nd()
        {
            int index = indexEventUp() + 1;
            this.Items[index].TimeTillEvent = convertUtcToLocal(index);
            return this.Items[index];
        }

        public ItemViewModel Boss3rd()
        {
            int index = indexEventUp() + 2;
            this.Items[index].TimeTillEvent = convertUtcToLocal(index);
            return this.Items[index];
        }

        private string CheckTimer(int index)
        {
            string utcCurrentTime = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");
            string[] dateTimeUtcSplit = utcCurrentTime.Split(null);
            string[] utcTimeSplit = dateTimeUtcSplit[1].Split(':');
            int currentHour = Convert.ToInt32(utcTimeSplit[0]);
            int currentMinute = Convert.ToInt32(utcTimeSplit[1]);
            int currentSecond = Convert.ToInt32(utcTimeSplit[2]);

            string[] eventTimeSplit = this.Items[index].Utc.Split(':');
            int eventHour = Convert.ToInt32(eventTimeSplit[0]);
            int eventMinute = Convert.ToInt32(eventTimeSplit[1]);
            int eventSecond = Convert.ToInt32(eventTimeSplit[2]);
            if (eventMinute == 00)
            {
                eventHour -= 1;
                eventMinute = 60;
            }

            int hour = eventHour - currentHour;
            string h = Convert.ToString(hour);
            int min = eventMinute - currentMinute - 1;
            string m = Convert.ToString(min);
            int sec = 60 - currentSecond;
            string s = Convert.ToString(sec);

            if (sec == 60)
            {
                int minOpSec = min + 1;
                m = Convert.ToString(minOpSec);
                s = "0";
            }
            if ((hour < 0) || (min < 0) || (sec < 0))
            {
                string[] splitTime = convertUtcToLocal(index).Split(':');
                h = splitTime[0];
                m = splitTime[1];
                s = splitTime[2];
            }

            return h + ":" + m + ":" + s;
        }

        private string convertUtcToLocal(int index)
        {
            DateTime utcEvent = DateTime.Parse(this.Items[index].Utc);
            DateTime utcToLocal = utcEvent.ToLocalTime();
            string localEvent = Convert.ToString(utcToLocal);
            string[] localSplit = localEvent.Split(null);
            return Convert.ToString(localSplit[1]);
        }
    }
}
