using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.UI.Xaml;
using Windows.UI.Input;
using Windows.UI.Core;

namespace Lab
{
    class GameInput
    {
        public Accelerometer accelerometer;
        public CoreWindow window;
        public GameInput()
        {
            accelerometer = Accelerometer.GetDefault();
            window = Window.Current.CoreWindow;
        }

    }
}
