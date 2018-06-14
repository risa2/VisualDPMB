using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;

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
			return (latitude-min)/(max-min);
		}
		public double RelativeX(double min, double max)
		{
			return (longitude-min)/(max-min);
		}
		public bool IsTram()
		{
			return route<=13;
		}
		public bool IsBus()
		{
			return route>=40;
		}
	}
	public partial class MainWindow: Window
	{
        private double west = 16.4778, east = 16.7328, south = 49.1322, north = 49.2905;
        private SimpleTimer timer;
        private SimpleTimer hider;
		private List<VehicleInfo> vehicles;
		private JsonDownload<List<VehicleInfo>> vehicle_source = new JsonDownload<List<VehicleInfo>>("http://sotoris.cz/DataSource/CityHack2015/vehiclesBrno.aspx");
		private void UpdateVehicles()
		{
			platno.Children.Clear();
			foreach (var vehicle in vehicles)
			{
                var e = new Vehicle(vehicle.IsTram() ? Brushes.Red : vehicle.IsBus() ? Brushes.Blue : Brushes.GreenYellow, vehicle.bearing);
                e.MouseDown += (s, arg) => {
                    vehicle_caption.Visibility = Visibility.Visible;
                    var vi = (from v in vehicles where v.vehicleId == vehicle.vehicleId select v).First();
                    vehicle_caption.Content = "Číslo vozu: " + vi.vehicleId + " Linka: " + vi.route + " Cíl: " + vi.headsign+" Počet vozů: "+((vi.consist??"").Count(c=>c=='+')+1);
                    hider?.Stop();
                    hider = new SimpleTimer(TimeSpan.FromSeconds(4), (es, earg) => { vehicle_caption.Visibility = Visibility.Hidden; hider.Stop(); });
                };
				Canvas.SetLeft(e, vehicle.RelativeX(west, east)*platno.Width);
				Canvas.SetTop(e, vehicle.RelativeY(north, south) *platno.Height);
				platno.Children.Add(e);
			}
        }
		private void LoadVehicles()
		{
			try
			{
                vehicles = vehicle_source.Download();
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
			timer=new SimpleTimer(TimeSpan.FromSeconds(1), (s, e) => LoadVehicles());
			SizeChanged+=(s, e) => UpdateVehicles();
		}
	}
}