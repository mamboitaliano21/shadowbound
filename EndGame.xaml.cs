using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Lab
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EndGame : Page
    {
        MainPage parent;
        public EndGame(MainPage parent)
        {
            this.parent = parent;
            this.InitializeComponent();
            ScoreText.Text = parent.game.score.ToString();
        }
        private void RestartGame(object sender, RoutedEventArgs e)
        {
            //parent.game.name = playerName.Text;
            parent.restartGame();
            parent.Children.Remove(this);
        }
        private void goToMainMenu(object sender, RoutedEventArgs e)
        {
            //parent.game.name = playerName.Text;
            parent.restartGame();
            float finderSpeed = parent.game.finderSpeed;
            float followerSpeed = parent.game.followerSpeed;
            parent.game.started = false;
            parent.mainMenu = new MainMenu(parent);
            parent.mainMenu.cmdStart.Content = "Start";
            parent.mainMenu.cmdRestart.Visibility = Visibility.Collapsed;
            parent.mainMenu.enemySpeedSld.Value = finderSpeed;
            parent.mainMenu.followerSpeedSld.Value = followerSpeed;
            parent.Children.Add(parent.mainMenu);
            parent.Children.Remove(this);
        }


    }
}
