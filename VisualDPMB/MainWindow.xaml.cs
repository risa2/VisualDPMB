using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
		private SimpleTimer timer;
		private MouseDrag drag;
		private List<VehicleInfo> vehicles;
		private Zooming zoom;
		private JsonDownload<List<VehicleInfo>> vehicle_source = new JsonDownload<List<VehicleInfo>>("http://sotoris.cz/DataSource/CityHack2015/vehiclesBrno.aspx");
		private void UpdateVehicles()
		{
			platno.Children.Clear();
			foreach (var vehicle in vehicles)
			{
				var e = new Ellipse
				{
					Width = 5,
					Height = 5,
					Fill = Brushes.GreenYellow
				};
				Canvas.SetLeft(e, zoom.ToScreenX(vehicle.RelativeX(16.4777558, 16.7327803)*Width));
				Canvas.SetTop(e, zoom.ToScreenY(vehicle.RelativeY(49.1321760, 49.2905053)*Height));
				platno.Children.Add(e);
			}
			Canvas.SetLeft(image, zoom.ToScreenX(0));
			Canvas.SetTop(image, zoom.ToScreenY(0));
			image.Width=zoom.ToScreenX(Width)-zoom.ToScreenX(0);
			image.Height=zoom.ToScreenY(Height)-zoom.ToScreenY(0);
        }
		private void LoadVehicles()
		{
			try
			{
				vehicles=vehicle_source.Download();
			}
			catch(HttpRequestException e)
			{
				Console.WriteLine(e);
				Close();
			}
			UpdateVehicles();
		}
		private void MouseRolled(int delta)
		{
			zoom.SetZoom(zoom.Zoom*Math.Pow(1.5, delta/Math.Abs(delta)), Mouse.GetPosition(this));
			UpdateVehicles();
		}
		private void MouseDragged(Point from, Point to)
		{
			zoom.Shift(new Point(from.X-to.X, from.Y-to.Y));
			UpdateVehicles();
        }
		public MainWindow()
		{
			InitializeComponent();

			image.Source=new BitmapImage(new Uri("/mapa.png", UriKind.Relative));
			image.Stretch=Stretch.Fill;
			zoom=new Zooming(new Size(Width, Height));
			drag=new MouseDrag(this);
 
			LoadVehicles();
			UpdateVehicles();
			timer=new SimpleTimer(TimeSpan.FromMilliseconds(1000), (s, e) => LoadVehicles());
			SizeChanged+=(s, e) => UpdateVehicles();
			MouseWheel+=(s, e) => MouseRolled(e.Delta);
			MouseDown+=drag.MouseDown();
			MouseUp+=drag.MouseUp();
			MouseMove+=drag.MouseMove((from, to) => MouseDragged(from, to));
		}
	}
}