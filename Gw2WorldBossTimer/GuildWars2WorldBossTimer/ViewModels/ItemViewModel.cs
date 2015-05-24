using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace GuildWars2WorldBossTimer.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _bossName;
        private string _utc;
        private string _waypoint;
        private string _location;
        private string _pre;
        private string _preWaypoint;
        private BitmapImage _bossLook;
        private string _timeTillEvent;

        public string BossName
        {
            get
            { return _bossName; }
            set
            {
                if (value != _bossName)
                {
                    _bossName = value;
                    NotifyPropertyChanged("BossName");
                }
            }
        }

        public string Utc
        {
            get
            { return _utc; }
            set
            {
                if (value != _utc)
                {
                    _utc = value;
                    NotifyPropertyChanged("Utc");
                }
            }
        }

        public string Waypoint
        {
            get
            { return _waypoint; }
            set
            {
                if (value != _waypoint)
                {
                    _waypoint = value;
                    NotifyPropertyChanged("Waypoint");
                }
            }
        }

        public string Location
        {
            get
            { return _location; }
            set
            {
                if (value != _location)
                {
                    _location = value;
                    NotifyPropertyChanged("Location");
                }
            }
        }

        public string Pre
        {
            get
            { return _pre; }
            set
            {
                if (value != _pre)
                {
                    _pre = value;
                    NotifyPropertyChanged("Pre");
                }
            }
        }

        public string PreWaypoint
        {
            get
            { return _preWaypoint; }
            set
            {
                if (value != _preWaypoint)
                {
                    _preWaypoint = value;
                    NotifyPropertyChanged("PreWaypoint");
                }
            }
        }

        public BitmapImage BossLook
        {
            get
            { return _bossLook; }
            set
            {
                if (value != _bossLook)
                {
                    _bossLook = value;
                    NotifyPropertyChanged("BossLook");
                }
            }
        }

        public string TimeTillEvent
        {
            get
            { return _timeTillEvent; }
            set
            {
                if (value != _timeTillEvent)
                {
                    _timeTillEvent = value;
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
