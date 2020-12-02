using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AMS.CustomControls
{
    public class DragBehavior : Behavior<UIElement>
    {
        private Point elementStartPosition;
        private Point mouseStartPosition;
        private TranslateTransform transform = new TranslateTransform();

        protected override void OnAttached()
        {
            Window parent = Application.Current.MainWindow;
            AssociatedObject.RenderTransform = transform;

            AssociatedObject.MouseLeftButtonDown += (sender, e) =>
            {
                mouseStartPosition = e.GetPosition(parent);
                AssociatedObject.CaptureMouse();
            };

            AssociatedObject.MouseLeftButtonUp += (sender, e) =>
            {
                AssociatedObject.ReleaseMouseCapture();
                elementStartPosition.X = transform.X;
                elementStartPosition.Y = transform.Y;
            };

            AssociatedObject.MouseMove += (sender, e) =>
            {
                var mousePos = e.GetPosition(parent);
                Vector diff = (mousePos - mouseStartPosition);
                if (AssociatedObject.IsMouseCaptured)
                {
                    transform.X = elementStartPosition.X + diff.X;
                    transform.Y = elementStartPosition.Y + diff.Y;
                }
            };
        }
    }
}
