using AdornerBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void thumbDragAround_DragDelta(object sender, DragDeltaEventArgs args)
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

        private void thumbTopLeft_DragDelta(object sender, DragDeltaEventArgs args)
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

        private void thumbTopRight_DragDelta(object sender, DragDeltaEventArgs args)
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

        private void thumbBottomLeft_DragDelta(object sender, DragDeltaEventArgs args)
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

        private void thumbBottomRight_DragDelta(object sender, DragDeltaEventArgs args)
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
