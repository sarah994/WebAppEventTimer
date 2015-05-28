using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GuildWars2WorldBossTimer.Resources;
using System.Windows.Threading;

namespace GuildWars2WorldBossTimer
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.ViewModel.Start += ViewModel_Start;

            App.ViewModel.LoadData();
        }

        private void ViewModel_Start()
        {
            this.DataContext = App.ViewModel.BossUp();
            Pivot2nd.DataContext = App.ViewModel.Boss2nd();
            Pivot3rd.DataContext = App.ViewModel.Boss3rd();

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.0)
            };
            timer.Tick += (o, f) =>
            {
                this.DataContext = App.ViewModel.BossUp();
                Pivot2nd.DataContext = App.ViewModel.Boss2nd();
                Pivot3rd.DataContext = App.ViewModel.Boss3rd();
            };
            timer.Start();
        }
    }
}