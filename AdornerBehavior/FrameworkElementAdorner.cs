using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

//
// This code based on code available here:
//
//  http://www.codeproject.com/KB/WPF/WPFJoshSmith.aspx
//
namespace AdornerBehavior
{
    /// <summary>
    /// This class is an adorner that allows a FrameworkElement derived class to adorn another FrameworkElement.
    /// </summary>
    public class FrameworkElementAdorner : Adorner
    {
        /// <summary>
        /// The framework element that is the adorner.
        /// </summary>
        public static readonly DependencyProperty AdornersProperty =
            DependencyProperty.Register("Adorners", typeof(AdornerCollection), typeof(FrameworkElementAdorner));

        public AdornerCollection Adorners
        {
            get
            {
                return (AdornerCollection)this.GetValue(AdornersProperty);
            }
            set
            {
                this.SetValue(AdornersProperty, value);
            }
        }

        public static readonly DependencyProperty AdornedElementProperty =
            DependencyProperty.RegisterAttached("AdornedElement", typeof(FrameworkElement), typeof(FrameworkElementAdorner));

        /// <summary>
        /// Position X of each AdornerChild.
        /// </summary>
        public static FrameworkElement GetAdornedElement(DependencyObject d)
        {
            return (FrameworkElement)d.GetValue(AdornedElementProperty);
        }

        public static void SetAdornedElement(DependencyObject d, FrameworkElement value)
        {
            d.SetValue(AdornedElementProperty, value);
        }

        public static readonly DependencyProperty PositionXProperty =
            DependencyProperty.RegisterAttached("PositionX", typeof(double), typeof(FrameworkElementAdorner), new PropertyMetadata(double.NaN));

        /// <summary>
        /// Position X of each AdornerChild.
        /// </summary>
        public static double GetPositionX(DependencyObject d)
        {
            return (double)d.GetValue(PositionXProperty);
        }
        public static void SetPositionX(DependencyObject d, double value)
        {
            d.SetValue(PositionXProperty, value);
        }

        public static readonly DependencyProperty PositionYProperty =
            DependencyProperty.RegisterAttached("PositionY", typeof(double), typeof(FrameworkElementAdorner), new PropertyMetadata(double.NaN));

        /// <summary>
        /// Position Y of each AdornerChild.
        /// </summary>
        public static double GetPositionY(DependencyObject d)
        {
            return (double)d.GetValue(PositionYProperty);
        }
        public static void SetPositionY(DependencyObject d, double value)
        {
            d.SetValue(PositionYProperty, value);
        }

        public FrameworkElementAdorner(AdornerCollection adorners, FrameworkElement adornedElement)
            : base(adornedElement)
        {
            this.Adorners = adorners;
            //this.ConnectChildren(adornedElement);

            adornedElement.SizeChanged += this.AdornedElement_SizeChanged;
        }

        /// <summary>
        /// Event raised when the adorned control's size has changed.
        /// </summary>
        private void AdornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        /// <summary>
        /// Determine the X coordinate of the AdornerContent.
        /// </summary>
        private double DetermineX(FrameworkElement adornerChild)
        {
            var horizontalPlacement = AdornerBehavior.GetHorizontalPlacement(adornerChild);
            var offsetX = AdornerBehavior.GetOffsetX(adornerChild);
            switch (adornerChild.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    {
                        if (horizontalPlacement == AdornerPlacement.Across)
                            return -adornerChild.DesiredSize.Width / 2 + offsetX;
                        if (horizontalPlacement == AdornerPlacement.Outside)
                            return -adornerChild.DesiredSize.Width + offsetX;

                        return offsetX;
                    }
                case HorizontalAlignment.Right:
                    {
                        if (horizontalPlacement == AdornerPlacement.Across)
                        {
                            var adornerWidth = adornerChild.DesiredSize.Width;
                            var adornedWidth = this.AdornedElement.ActualWidth;
                            var x = adornedWidth - adornerWidth / 2;
                            return x + offsetX;
                        }

                        if (horizontalPlacement == AdornerPlacement.Outside)
                        {
                            var adornedWidth = this.AdornedElement.ActualWidth;
                            return adornedWidth + offsetX;
                        }

                        {
                            var adornerWidth = adornerChild.DesiredSize.Width;
                            var adornedWidth = this.AdornedElement.ActualWidth;
                            var x = adornedWidth - adornerWidth;
                            return x + offsetX;
                        }
                    }
                case HorizontalAlignment.Center:
                    {
                        var adornerWidth = adornerChild.DesiredSize.Width;
                        var adornedWidth = this.AdornedElement.ActualWidth;
                        var x = (adornedWidth / 2) - (adornerWidth / 2);
                        return x + offsetX;
                    }
                case HorizontalAlignment.Stretch:
                    {
                        return 0.0;
                    }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the Y coordinate of the AdornerContent.
        /// </summary>
        private double DetermineY(FrameworkElement adornerChild)
        {
            var verticalPlacement = AdornerBehavior.GetVerticalPlacement(adornerChild);
            var offsetY = AdornerBehavior.GetOffsetY(adornerChild);

            switch (adornerChild.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    {
                        if (verticalPlacement == AdornerPlacement.Across)
                            return -adornerChild.DesiredSize.Height / 2 + offsetY;

                        if (verticalPlacement == AdornerPlacement.Outside)
                            return -adornerChild.DesiredSize.Height + offsetY;

                        return offsetY;
                    }
                case VerticalAlignment.Bottom:
                    {
                        if (verticalPlacement == AdornerPlacement.Across)
                        {
                            var adornerHeight = adornerChild.DesiredSize.Height;
                            var adornedHeight = this.AdornedElement.ActualHeight;
                            var x = adornedHeight - adornerHeight / 2;
                            return x + offsetY;
                        }

                        if (verticalPlacement == AdornerPlacement.Outside)
                        {
                            var adornedHeight = this.AdornedElement.ActualHeight;
                            return adornedHeight + offsetY;
                        }

                        {
                            var adornerHeight = adornerChild.DesiredSize.Height;
                            var adornedHeight = this.AdornedElement.ActualHeight;
                            var x = adornedHeight - adornerHeight;
                            return x + offsetY;
                        }
                    }
                case VerticalAlignment.Center:
                    {
                        var adornerHeight = adornerChild.DesiredSize.Height;
                        var adornedHeight = this.AdornedElement.ActualHeight;
                        var x = (adornedHeight / 2) - (adornerHeight / 2);
                        return x + offsetY;
                    }
                case VerticalAlignment.Stretch:
                    {
                        return 0.0;
                    }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the width of each AdornerChild.
        /// </summary>
        private double DetermineWidth(FrameworkElement adornerChild)
        {
            var positionX = GetPositionX(adornerChild);
            if (!double.IsNaN(positionX))
                return adornerChild.DesiredSize.Width;

            switch (adornerChild.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    {
                        return adornerChild.DesiredSize.Width;
                    }
                case HorizontalAlignment.Right:
                    {
                        return adornerChild.DesiredSize.Width;
                    }
                case HorizontalAlignment.Center:
                    {
                        return adornerChild.DesiredSize.Width;
                    }
                case HorizontalAlignment.Stretch:
                    {
                        return this.AdornedElement.ActualWidth;
                    }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the height of the AdornerContent.
        /// </summary>
        private double DetermineHeight(FrameworkElement adornerChild)
        {
            var positionY = GetPositionY(adornerChild);
            if (!double.IsNaN(positionY))
                return adornerChild.DesiredSize.Height;

            switch (adornerChild.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    {
                        return adornerChild.DesiredSize.Height;
                    }
                case VerticalAlignment.Bottom:
                    {
                        return adornerChild.DesiredSize.Height;
                    }
                case VerticalAlignment.Center:
                    {
                        return adornerChild.DesiredSize.Height;
                    }
                case VerticalAlignment.Stretch:
                    {
                        return this.AdornedElement.ActualHeight;
                    }
            }

            return 0.0;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var adornerChild in this.Adorners)
            {
                var x = GetPositionX(adornerChild);

                if (double.IsNaN(x))
                    x = this.DetermineX(adornerChild);

                var y = GetPositionY(adornerChild);
                if (double.IsNaN(y))
                    y = this.DetermineY(adornerChild);

                var adornerWidth = this.DetermineWidth(adornerChild);
                var adornerHeight = this.DetermineHeight(adornerChild);
                adornerChild.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
            }
            return finalSize;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return this.Adorners.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (this.Adorners == null)
                return null;

            return new List<FrameworkElement>(this.Adorners)[index];
        }

        /// <summary>
        /// Remove all adorner children from the visual tree.
        /// </summary>
        public void DisconnectChildren()
        {
            foreach (var adornerChild in this.Adorners)
            {
                this.RemoveLogicalChild(adornerChild);
                this.RemoveVisualChild(adornerChild);
                SetAdornedElement(adornerChild, null);
            }
        }

        /// <summary>
        /// Add all adorner children to the visual tree.
        /// </summary>
        public void ConnectChildren(FrameworkElement adornedElement)
        {
            foreach (var adornerChild in this.Adorners)
            {
                this.AddLogicalChild(adornerChild);
                this.AddVisualChild(adornerChild);
                SetAdornedElement(adornerChild, adornedElement);
            }
        }

        /// <summary>
        /// Override AdornedElement from base class for less type-checking.
        /// </summary>
        public new FrameworkElement AdornedElement
        {
            get
            {
                return (FrameworkElement)base.AdornedElement;
            }
        }
    }
}
