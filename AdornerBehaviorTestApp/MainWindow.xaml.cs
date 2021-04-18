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
        private int _currentDynamicAdorner;

        public int CurrentDynamicAdorner
        {
            get
            {
                return this._currentDynamicAdorner;
            }
            set
            {
                if (value == 5)
                    value = 0;

                this._currentDynamicAdorner = value;
            }
        }

        public MainWindow()
        {
            this.InitializeComponent();
            this.CurrentDynamicAdorner = 1;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var adornedElement = sender as FrameworkElement;
            if (adornedElement == null)
                return;

            var adorners = AdornerBehavior.AdornerBehavior.GetAdorners(adornedElement);

            if (this.CurrentDynamicAdorner == 0)
            {
                adorners.Clear();
                this.CurrentDynamicAdorner++;
                return;
            }

            var elementToFind = "DynamicAdorner" + this.CurrentDynamicAdorner;
            var newAdorner = this.FindResource(elementToFind) as FrameworkElement;
            adorners.Add(newAdorner);
            this.CurrentDynamicAdorner++;
        }
    }
}
