using System.Windows;
using SuperGra.Model;

namespace SuperGra.Helpers
{
    class Attached
    {
        #region IsDragging

        public static bool GetIsDragging(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDraggingProperty);
        }

        public static void SetIsDragging(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDraggingProperty, value);
        }

        public static readonly DependencyProperty IsDraggingProperty =
            DependencyProperty.RegisterAttached("IsDragging", typeof(bool), typeof(Attached), new UIPropertyMetadata(false));
        #endregion

        #region CanDrag

        public static bool GetCanDrag(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanDragProperty);
        }

        public static void SetCanDrag(DependencyObject obj, bool value)
        {
            obj.SetValue(CanDragProperty, value);
        }

        public static readonly DependencyProperty CanDragProperty =
            DependencyProperty.RegisterAttached("CanDrag", typeof(bool), typeof(Attached), new UIPropertyMetadata(false, CanDragChanged));

        static void CanDragChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var ele = sender as FrameworkElement;
            if ((bool)e.NewValue)
            {
                ele.MouseDown += new System.Windows.Input.MouseButtonEventHandler(ele_MouseDown);
                ele.MouseUp += new System.Windows.Input.MouseButtonEventHandler(ele_MouseUp);
                ele.MouseMove += new System.Windows.Input.MouseEventHandler(ele_MouseMove);
                ele.Unloaded += new RoutedEventHandler(ele_Unloaded);
            }
            else
                RemoveHandlers(ele);
        }

        static void ele_Unloaded(object sender, RoutedEventArgs e)
        {
            var ele = sender as FrameworkElement;
            RemoveHandlers(ele);
        }

        private static void RemoveHandlers(FrameworkElement ele)
        {
            ele.MouseDown -= new System.Windows.Input.MouseButtonEventHandler(ele_MouseDown);
            ele.MouseUp -= new System.Windows.Input.MouseButtonEventHandler(ele_MouseUp);
            ele.MouseMove -= new System.Windows.Input.MouseEventHandler(ele_MouseMove);
            ele.Unloaded -= new RoutedEventHandler(ele_Unloaded);
        }

        static void ele_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var ele = sender as FrameworkElement;
            var isDragging = (bool)ele.GetValue(Attached.IsDraggingProperty);
            if (isDragging)
            {
                var obj = ele.DataContext as IMovable;

                var parent = ele.Tag as FrameworkElement;
                obj.X = e.GetPosition(parent).X - (ele.ActualWidth / 2);
                obj.Y = e.GetPosition(parent).Y - (ele.ActualHeight / 2);
            }
        }

        static void ele_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var ele = sender as FrameworkElement;
            ele.SetValue(Attached.IsDraggingProperty, false);
        }

        static void ele_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var ele = sender as FrameworkElement;
            ele.SetValue(Attached.IsDraggingProperty, true);
        } 
        #endregion

        #region AttachedEvent

        public class ParentDragEventRoutedEventArgs : RoutedEventArgs
        {
            //No args used in this example, but this could be the new X & Y, or just the moved difference...
        }

        public delegate void ParentDragEventRoutedEventHandler(object sender, ParentDragEventRoutedEventArgs e);

        public static readonly RoutedEvent ParentDragEventChangedEvent = EventManager.RegisterRoutedEvent("ParentDragEvent", RoutingStrategy.Bubble, typeof(ParentDragEventRoutedEventHandler), typeof(Attached));

        public static void AddParentDragEventChangedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(Attached.ParentDragEventChangedEvent, handler);
            }
        }

        public static void RemoveParentDragEventChangedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(Attached.ParentDragEventChangedEvent, handler);
            }
        } 

        #endregion

    }
}