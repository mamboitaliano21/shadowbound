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
    public sealed partial class HallOfFame : Page
    {

        private MainPage parent;

        public HallOfFame(MainPage parent)
        {
            this.parent = parent;
            this.InitializeComponent();
            showAll();
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            parent.Children.Add(parent.mainMenu);
            parent.Children.Remove(this);
        }

        public void showAll()
        {
            List<Tuple<string, int>> scoreList = parent.game.readFromList("highScores.txt");
            int counter = 1;

            foreach(var s in scoreList)
            {
                switch (counter)
                {
                    case 1:
                        player1.Text = s.Item1;
                        score1.Text = s.Item2.ToString();
                        break;
                    case 2:
                        player2.Text = s.Item1;
                        score2.Text = s.Item2.ToString();
                        break;
                    case 3:
                        player3.Text = s.Item1;
                        score3.Text = s.Item2.ToString();
                        break;
                    case 4:
                        player4.Text = s.Item1;
                        score4.Text = s.Item2.ToString();
                        break;
                    case 5:
                        player5.Text = s.Item1;
                        score5.Text = s.Item2.ToString();
                        break;
                    case 6:
                        player6.Text = s.Item1;
                        score6.Text = s.Item2.ToString();
                        break;
                    case 7:
                        player7.Text = s.Item1;
                        score7.Text = s.Item2.ToString();
                        break;
                    case 8:
                        player8.Text = s.Item1;
                        score8.Text = s.Item2.ToString();
                        break;
                    case 9:
                        player9.Text = s.Item1;
                        score9.Text = s.Item2.ToString();
                        break;
                    case 10:
                        player10.Text = s.Item1;
                        score10.Text = s.Item2.ToString();
                        break;
                }
                counter++;
                if (counter == 11)
                {
                    break;
                }
            }
        }
    }
}
