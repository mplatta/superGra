using System.Windows;

namespace SuperGra.Model
{
    interface IMovable
    {
        FrameworkElement Parent { get; set; }
        double X { get; set; }
        double Y { get; set; }
    }
}
