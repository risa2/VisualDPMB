using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace VisualDPMB
{
    public class VehicleInfo
    {
        public int vehicleId, route;
        public int? bearing;
        public float latitude, longitude;
        public string course, headsign;
        public string consist;
        public double RelativeY(double min, double max)
        {
            return (latitude - min) / (max - min);
        }
        public double RelativeX(double min, double max)
        {
            return (longitude - min) / (max - min);
        }
    }
    public partial class MainWindow: Window
    {
        private DispatcherTimer timer;
        private void UpdateVehicles()
        {
            var client = new HttpClient();
            try
            {
                var str = client.GetStringAsync("http://sotoris.cz/DataSource/CityHack2015/vehiclesBrno.aspx").Result;
                var vehicles = JsonConvert.DeserializeObject<List<VehicleInfo>>(str);
                platno.Children.Clear();
                foreach (var vehicle in vehicles)
                {
                    var e = new Ellipse();
                    e.Width = 5;
                    e.Height = 5;
                    e.Fill = System.Windows.Media.Brushes.GreenYellow;
                    Canvas.SetLeft(e, vehicle.RelativeX(16.4, 17)*Width);
                    Canvas.SetTop(e, vehicle.RelativeY(49.1, 49.4)*Height);
                    platno.Children.Add(e);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.ToString());
                Close();
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            UpdateVehicles();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += (s, e) => UpdateVehicles();
            timer.Start();
        }
    }
}