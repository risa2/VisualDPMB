using System.Windows.Controls;
using System.Windows.Media;

namespace VisualDPMB
{
    public partial class Vehicle : UserControl
    {
        public Vehicle(Brush color, int? angle)
        {
            InitializeComponent();
            Width = 4;
            Height = 4;
            circle.Fill = color;
            if(angle.HasValue)
            {
                triangle.RenderTransform = new RotateTransform(angle.Value, Width / 2, Height / 2);
            }
            else
            {
                triangle.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
