using System.Windows;
using System.Windows.Input;

namespace VisualDPMB
{
	class MouseDrag
	{
		private bool pressed = false;
		private Point presspoint;
		private IInputElement relative_to;
		public delegate void DragCallback(Point from, Point to);
		public MouseDrag(IInputElement relative_to)
		{
			this.relative_to=relative_to;
		}
		public MouseButtonEventHandler MouseDown()
		{
			return (s, e) => { pressed=true; presspoint=Mouse.GetPosition(relative_to); };
        }
		public MouseButtonEventHandler MouseUp()
		{
			return (s, e) => pressed=false;
        }
		public MouseEventHandler MouseMove(DragCallback reaction)
		{
			return (s, e) => {
				if(pressed)
				{
					reaction(presspoint, Mouse.GetPosition(relative_to));
					presspoint=Mouse.GetPosition(relative_to);
				}
			};
		}
	}
}
