using System;
using System.Windows;

namespace VisualDPMB
{
	public class Zooming
	{
		public double Zoom { get; private set; } = 1.0;
		private Point center;
		private Size screen;
		private void CheckLimits()
		{
			center.X=Math.Min(Math.Max(center.X, screen.Width/2/Zoom), screen.Width-screen.Width/2/Zoom);
			center.Y=Math.Min(Math.Max(center.Y, screen.Height/2/Zoom), screen.Height-screen.Height/2/Zoom);
		}
		public Zooming(Size screen)
		{
			this.screen=screen;
			center=new Point(screen.Width/2, screen.Height/2);
		}
		public double ToScreenX(double x)
		{
			return screen.Width/2+Zoom*(x-center.X);
		}
		public double ToScreenY(double y)
		{
			return screen.Height/2+Zoom*(y-center.Y);
		}
		public double ToImageX(double x)
		{
			return (x-screen.Width/2)/Zoom+center.X;
		}
		public double ToImageY(double y)
		{
			return (y-screen.Height/2)/Zoom+center.Y;
		}
		public void Shift(Point shift)
		{
			center.X+=shift.X/Zoom;
			center.Y+=shift.Y/Zoom;
			CheckLimits();
		}
		public void SetZoom(double new_zoom, Point zoom_point)
		{
			center.X=ToImageX(zoom_point.X);
			center.Y=ToImageY(zoom_point.Y);
			Zoom=Math.Max(1.0, new_zoom);
			CheckLimits();
		}
	}
}
