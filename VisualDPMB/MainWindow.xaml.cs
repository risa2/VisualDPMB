using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VisualDPMB
{
    public class VehicleInfo
	{
		public int vehicleId, route;
		public int? bearing;
		public float latitude, longitude;
		public string course, headsign;
		public string consist;
	}
	public class Vehicle
    {
        public VehicleInfo info;
        public double RelativeY(double min, double max)
        {
            return (info.latitude - min) / (max - min);
        }
        public double RelativeX(double min, double max)
        {
            return (info.longitude - min) / (max - min);
        }
        public bool IsTram()
        {
            return info.route <= 13;
        }
        public bool IsBus()
        {
            return info.route >= 40;
        }
    }
	public partial class MainWindow: Window
	{
		private SimpleTimer timer;
		private List<Vehicle> vehicles;
		private JsonDownload<List<VehicleInfo>> vehicle_source = new JsonDownload<List<VehicleInfo>>("http://sotoris.cz/DataSource/CityHack2015/vehiclesBrno.aspx");
		private void UpdateVehicles()
		{
			platno.Children.Clear();
			foreach (var vehicle in vehicles)
			{
				var e = new Ellipse
				{
					Width = 4,
					Height = 4,
					Fill = vehicle.IsTram()?Brushes.Red:vehicle.IsBus()?Brushes.Blue:Brushes.GreenYellow
				};
                e.MouseDown += (s, arg) => (from v in vehicles where v==vehicle select v).First();
				Canvas.SetLeft(e, vehicle.RelativeX(16.4777558, 16.7327803)*platno.Width);
				Canvas.SetTop(e, vehicle.RelativeY(49.1321760, 49.2905053)*platno.Height);
				platno.Children.Add(e);
			}
        }
		private void LoadVehicles()
		{
			try
			{
                vehicles = (from vehicle in vehicle_source.Download() select new Vehicle() { info = vehicle }).ToList<Vehicle>();
			}
			catch(HttpRequestException e)
			{
				Console.WriteLine(e);
				Close();
			}
			UpdateVehicles();
		}
		public MainWindow()
		{
			InitializeComponent();

			image.Source=new BitmapImage(new Uri("/mapa.png", UriKind.Relative));
			image.Stretch=Stretch.Fill;
 
			LoadVehicles();
			UpdateVehicles();
			timer=new SimpleTimer(TimeSpan.FromMilliseconds(1000), (s, e) => LoadVehicles());
			SizeChanged+=(s, e) => UpdateVehicles();
		}
	}
}