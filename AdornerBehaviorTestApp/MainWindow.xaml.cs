using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using AdornerBehavior;

namespace AdornerBehaviorTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void ThumbDragAround_DragDelta(object sender, DragDeltaEventArgs args)
        {
            if (sender is FrameworkElement adornerChild)
            {
                var adornedElement = FrameworkElementAdorner.GetAdornedElement(adornerChild);

                var left = Canvas.GetLeft(adornedElement);
                if (left.Equals(double.NaN))
                {
                    left = 0;
                }

                var top = Canvas.GetTop(adornedElement);
                if (top.Equals(double.NaN))
                {
                    top = 0;
                }

                Canvas.SetLeft(adornedElement, left + args.HorizontalChange);
                Canvas.SetTop(adornedElement, top + args.VerticalChange);
            }
        }

        private void ThumbTopLeft_DragDelta(object sender, DragDeltaEventArgs args)
        {
            if (sender is FrameworkElement adornerChild)
            {
                var adornedElement = FrameworkElementAdorner.GetAdornedElement(adornerChild);

                var left = Canvas.GetLeft(adornedElement);
                if (left.Equals(double.NaN))
                {
                    left = 0;
                }

                var top = Canvas.GetTop(adornedElement);
                if (top.Equals(double.NaN))
                {
                    top = 0;
                }

                var width = adornedElement.ActualWidth;
                var height = adornedElement.ActualHeight;

                if (width - args.HorizontalChange > 0)
                {
                    adornedElement.Width = width - args.HorizontalChange;
                    Canvas.SetLeft(adornedElement, left + args.HorizontalChange);
                }
                if (height - args.VerticalChange > 0)
                {
                    adornedElement.Height = height - args.VerticalChange;
                    Canvas.SetTop(adornedElement, top + args.VerticalChange);
                }
            }
        }

        private void ThumbTopRight_DragDelta(object sender, DragDeltaEventArgs args)
        {
            if (sender is FrameworkElement adornerChild)
            {
                var adornedElement = FrameworkElementAdorner.GetAdornedElement(adornerChild);

                var top = Canvas.GetTop(adornedElement);
                if (top.Equals(double.NaN))
                {
                    top = 0;
                }

                var width = adornedElement.ActualWidth;
                var height = adornedElement.ActualHeight;

                if (width + args.HorizontalChange > 0)
                {
                    adornedElement.Width = width + args.HorizontalChange;
                }

                if (height - args.VerticalChange > 0)
                {
                    adornedElement.Height = height - args.VerticalChange;
                    Canvas.SetTop(adornedElement, top + args.VerticalChange);
                }
            }
        }

        private void ThumbBottomLeft_DragDelta(object sender, DragDeltaEventArgs args)
        {
            if (sender is FrameworkElement adornerChild)
            {
                var adornedElement = FrameworkElementAdorner.GetAdornedElement(adornerChild);

                var left = Canvas.GetLeft(adornedElement);
                if (left.Equals(double.NaN))
                {
                    left = 0;
                }

                var width = adornedElement.ActualWidth;
                var height = adornedElement.ActualHeight;

                if (width - args.HorizontalChange > 0)
                {
                    adornedElement.Width = width - args.HorizontalChange;
                    Canvas.SetLeft(adornedElement, left + args.HorizontalChange);
                }
                if (height + args.VerticalChange > 0)
                {
                    adornedElement.Height = height + args.VerticalChange;
                }
            }
        }

        private void ThumbBottomRight_DragDelta(object sender, DragDeltaEventArgs args)
        {
            if (sender is FrameworkElement adornerChild)
            {
                var adornedElement = FrameworkElementAdorner.GetAdornedElement(adornerChild);

                var width = adornedElement.ActualWidth;
                var height = adornedElement.ActualHeight;

                if (width + args.HorizontalChange > 0)
                {
                    adornedElement.Width = width + args.HorizontalChange;
                }

                if (height + args.VerticalChange > 0)
                {
                    adornedElement.Height = height + args.VerticalChange;
                }
            }
        }
    }
}
