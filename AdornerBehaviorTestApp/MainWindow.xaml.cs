using AdornerBehavior;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AdornerBehaviorTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ThumbDragAround_DragDelta(object sender, DragDeltaEventArgs args)
        {
            if (sender is FrameworkElement adornerChild)
            {
                FrameworkElement adornedElement = FrameworkElementMultiChildAdorner.GetAdornedElement(adornerChild);

                double left = Canvas.GetLeft(adornedElement);
                if (left.Equals(double.NaN))
                {
                    left = 0;
                }

                double top = Canvas.GetTop(adornedElement);
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
                FrameworkElement adornedElement = FrameworkElementMultiChildAdorner.GetAdornedElement(adornerChild);

                double left = Canvas.GetLeft(adornedElement);
                if (left.Equals(double.NaN))
                {
                    left = 0;
                }

                double top = Canvas.GetTop(adornedElement);
                if (top.Equals(double.NaN))
                {
                    top = 0;
                }

                double width = adornedElement.ActualWidth;
                double height = adornedElement.ActualHeight;

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
                FrameworkElement adornedElement = FrameworkElementMultiChildAdorner.GetAdornedElement(adornerChild);

                double top = Canvas.GetTop(adornedElement);
                if (top.Equals(double.NaN))
                {
                    top = 0;
                }

                double width = adornedElement.ActualWidth;
                double height = adornedElement.ActualHeight;

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
                FrameworkElement adornedElement = FrameworkElementMultiChildAdorner.GetAdornedElement(adornerChild);

                double left = Canvas.GetLeft(adornedElement);
                if (left.Equals(double.NaN))
                {
                    left = 0;
                }

                double width = adornedElement.ActualWidth;
                double height = adornedElement.ActualHeight;

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
                FrameworkElement adornedElement = FrameworkElementMultiChildAdorner.GetAdornedElement(adornerChild);

                double width = adornedElement.ActualWidth;
                double height = adornedElement.ActualHeight;

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
