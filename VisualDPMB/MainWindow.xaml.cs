using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using System.Windows.Media;

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

                image.Width = platno.Width;
                image.Height = platno.Height;
                image.Source = new BitmapImage(new Uri("/mapa.gif", UriKind.Relative));
                image.Stretch = Stretch.Fill;
                foreach (var vehicle in vehicles)
                {
                    var e = new Ellipse
                    {
                        Width = 5,
                        Height = 5,
                        Fill = Brushes.GreenYellow
                    };
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