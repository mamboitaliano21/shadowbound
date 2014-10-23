// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using SharpDX;

namespace Lab
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public LabGame game;
        public MainMenu mainMenu;

        public MainPage()
        {
            InitializeComponent();
            game = new LabGame(this);
            game.Run(this);
            mainMenu = new MainMenu(this);
            this.Children.Add(mainMenu);
        }

        public void UpdateScore(int score)
        {
            ScoreText.Text = score.ToString();
        }
        public void UpdateHP(float hp)
        {
            HealthBar.Value = hp;
            HealthBar.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 224, 51));
            if (hp <= 0)
            {
                game.started = false;
                this.Children.Add(new EndGame(this));
            }
            else if (hp <= 30)
            {
                HealthBar.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 224, 0, 0));
            }
        }


        private void GoBack(object sender, RoutedEventArgs e)
        {
            float finderSpeed = game.finderSpeed;
            float followerSpeed = game.followerSpeed;
            game.started = false;
            mainMenu = new MainMenu(this);
            mainMenu.cmdStart.Content = "Continue";
            mainMenu.cmdRestart.Visibility = Visibility.Visible;
            mainMenu.enemySpeedSld.Value = finderSpeed;
            mainMenu.followerSpeedSld.Value = followerSpeed;
            this.Children.Add(mainMenu);
        }

        public void StartGame()
        {
            this.Children.Remove(mainMenu);
            game.started = true;
        }

        public void restartGame()
        {
            //game.started = false;
            //game = null;
            //game = new LabGame(this);
            //game.Run(this);
            this.Children.Remove(mainMenu);
            
            game.started = true;
        }

        public void IsHit(bool isHit)
        {
            if (isHit)
            {
                background.Opacity = 0.5;
            }
            else
            {
                background.Opacity = 0.0;
            }
        }
    }
}
