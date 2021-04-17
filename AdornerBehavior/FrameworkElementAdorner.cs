using System;
using System.Collections;
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
    //
    // This class is an adorner that allows a FrameworkElement derived class to adorn another FrameworkElement.
    //
    public class FrameworkElementAdorner : Adorner
    {
        /// <summary>
        /// The framework element that is the adorner.
        /// </summary>
        public static readonly DependencyProperty AdornerContentProperty
            = DependencyProperty.Register("AdornerContent", typeof(FrameworkElement), typeof(FrameworkElementAdorner));

        public FrameworkElement AdornerContent
        {
            get
            {
                return (FrameworkElement)this.GetValue(AdornerContentProperty);
            }
            set
            {
                this.SetValue(AdornerContentProperty, value);
            }
        }

        //
        // Placement of the AdornerContent.
        //
        public static readonly DependencyProperty HorizontalAdornerPlacementProperty
            = DependencyProperty.Register("HorizontalAdornerPlacement", typeof(AdornerPlacement), typeof(FrameworkElementAdorner),
            new PropertyMetadata(AdornerPlacement.Inside));
        public AdornerPlacement HorizontalAdornerPlacement
        {
            get
            {
                return (AdornerPlacement)this.GetValue(HorizontalAdornerPlacementProperty);
            }
            set
            {
                this.SetValue(HorizontalAdornerPlacementProperty, value);
            }
        }
        public static readonly DependencyProperty VerticalAdornerPlacementProperty
            = DependencyProperty.Register("VerticalAdornerPlacement", typeof(AdornerPlacement), typeof(FrameworkElementAdorner),
            new PropertyMetadata(AdornerPlacement.Inside));
        public AdornerPlacement VerticalAdornerPlacement
        {
            get
            {
                return (AdornerPlacement)this.GetValue(VerticalAdornerPlacementProperty);
            }
            set
            {
                this.SetValue(VerticalAdornerPlacementProperty, value);
            }
        }
        //
        // Offset of the AdornerContent.
        //
        public static readonly DependencyProperty AdornerOffsetXProperty
            = DependencyProperty.Register("AdornerOffsetX", typeof(double), typeof(FrameworkElementAdorner),
            new PropertyMetadata(0.0));
        public double AdornerOffsetX
        {
            get
            {
                return (double)this.GetValue(AdornerOffsetXProperty);
            }
            set
            {
                this.SetValue(AdornerOffsetXProperty, value);
            }
        }
        public static readonly DependencyProperty AdornerOffsetYProperty
            = DependencyProperty.Register("AdornerOffsetY", typeof(double), typeof(FrameworkElementAdorner),
            new PropertyMetadata(0.0));
        public double AdornerOffsetY
        {
            get
            {
                return (double)this.GetValue(AdornerOffsetYProperty);
            }
            set
            {
                this.SetValue(AdornerOffsetYProperty, value);
            }
        }

        //
        // Position of the AdornerContent (when not set to NaN).
        //
        private double positionX = Double.NaN;
        private double positionY = Double.NaN;

        public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement)
            : base(adornedElement)
        {
            this.AdornerContent = adornerChildElement;

            base.AddLogicalChild(adornerChildElement);
            base.AddVisualChild(adornerChildElement);
        }

        /// <summary>
        /// Event raised when the adorned control's size has changed.
        /// </summary>
        private void AdornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        //
        // Position of the AdornerContent (when not set to NaN).
        //
        public double PositionX
        {
            get
            {
                return this.positionX;
            }
            set
            {
                this.positionX = value;
            }
        }

        public double PositionY
        {
            get
            {
                return this.positionY;
            }
            set
            {
                this.positionY = value;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this.AdornerContent.Measure(constraint);
            return this.AdornerContent.DesiredSize;
        }

        /// <summary>
        /// Determine the X coordinate of the AdornerContent.
        /// </summary>
        private double DetermineX()
        {
            switch (this.AdornerContent.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    {
                        if (this.HorizontalAdornerPlacement == AdornerPlacement.Across)
                        {
                            return -this.AdornerContent.DesiredSize.Width / 2 + this.AdornerOffsetX;
                        }
                        if (this.HorizontalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            return -this.AdornerContent.DesiredSize.Width + this.AdornerOffsetX;
                        }
                        else
                        {
                            return this.AdornerOffsetX;
                        }
                    }
                case HorizontalAlignment.Right:
                    {
                        if (this.HorizontalAdornerPlacement == AdornerPlacement.Across)
                        {
                            var adornerWidth = this.AdornerContent.DesiredSize.Width;
                            var adornedWidth = this.AdornedElement.ActualWidth;
                            var x = adornedWidth - adornerWidth;
                            return x / 2 + this.AdornerOffsetX;
                        }
                        if (this.HorizontalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            var adornedWidth = this.AdornedElement.ActualWidth;
                            return adornedWidth + this.AdornerOffsetX;
                        }
                        else
                        {
                            var adornerWidth = this.AdornerContent.DesiredSize.Width;
                            var adornedWidth = this.AdornedElement.ActualWidth;
                            var x = adornedWidth - adornerWidth;
                            return x + this.AdornerOffsetX;
                        }
                    }
                case HorizontalAlignment.Center:
                    {
                        var adornerWidth = this.AdornerContent.DesiredSize.Width;
                        var adornedWidth = this.AdornedElement.ActualWidth;
                        var x = (adornedWidth / 2) - (adornerWidth / 2);
                        return x + this.AdornerOffsetX;
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
        private double DetermineY()
        {
            switch (this.AdornerContent.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    {
                        if (this.VerticalAdornerPlacement == AdornerPlacement.Across)
                        {
                            return -this.AdornerContent.DesiredSize.Height / 2 + this.AdornerOffsetY;
                        }
                        if (this.VerticalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            return -this.AdornerContent.DesiredSize.Height + this.AdornerOffsetY;
                        }
                        else
                        {
                            return this.AdornerOffsetY;
                        }
                    }
                case VerticalAlignment.Bottom:
                    {
                        if (this.VerticalAdornerPlacement == AdornerPlacement.Across)
                        {
                            var adornerHeight = this.AdornerContent.DesiredSize.Height;
                            var adornedHeight = this.AdornedElement.ActualHeight;
                            var x = adornedHeight - adornerHeight;
                            return x / 2 + this.AdornerOffsetY;
                        }
                        if (this.VerticalAdornerPlacement == AdornerPlacement.Outside)
                        {
                            var adornedHeight = this.AdornedElement.ActualHeight;
                            return adornedHeight + this.AdornerOffsetY;
                        }
                        else
                        {
                            var adornerHeight = this.AdornerContent.DesiredSize.Height;
                            var adornedHeight = this.AdornedElement.ActualHeight;
                            var x = adornedHeight - adornerHeight;
                            return x + this.AdornerOffsetY;
                        }
                    }
                case VerticalAlignment.Center:
                    {
                        var adornerHeight = this.AdornerContent.DesiredSize.Height;
                        var adornedHeight = this.AdornedElement.ActualHeight;
                        var x = (adornedHeight / 2) - (adornerHeight / 2);
                        return x + this.AdornerOffsetY;
                    }
                case VerticalAlignment.Stretch:
                    {
                        return 0.0;
                    }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the width of the AdornerContent.
        /// </summary>
        private double DetermineWidth()
        {
            if (!Double.IsNaN(this.PositionX))
            {
                return this.AdornerContent.DesiredSize.Width;
            }

            switch (this.AdornerContent.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    {
                        return this.AdornerContent.DesiredSize.Width;
                    }
                case HorizontalAlignment.Right:
                    {
                        return this.AdornerContent.DesiredSize.Width;
                    }
                case HorizontalAlignment.Center:
                    {
                        return this.AdornerContent.DesiredSize.Width;
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
        private double DetermineHeight()
        {
            if (!Double.IsNaN(this.PositionY))
            {
                return this.AdornerContent.DesiredSize.Height;
            }

            switch (this.AdornerContent.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    {
                        return this.AdornerContent.DesiredSize.Height;
                    }
                case VerticalAlignment.Bottom:
                    {
                        return this.AdornerContent.DesiredSize.Height;
                    }
                case VerticalAlignment.Center:
                    {
                        return this.AdornerContent.DesiredSize.Height;
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
            var x = this.PositionX;
            if (Double.IsNaN(x))
            {
                x = this.DetermineX();
            }
            var y = this.PositionY;
            if (Double.IsNaN(y))
            {
                y = this.DetermineY();
            }
            var adornerWidth = this.DetermineWidth();
            var adornerHeight = this.DetermineHeight();
            this.AdornerContent.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
            return finalSize;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return this.AdornerContent;
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                var list = new ArrayList {
                    this.AdornerContent
                };
                return list.GetEnumerator();
            }
        }

        /// <summary>
        /// Disconnect the AdornerContent element from the visual tree so that it may be reused later.
        /// </summary>
        public void DisconnectChild()
        {
            base.RemoveLogicalChild(this.AdornerContent);
            base.RemoveVisualChild(this.AdornerContent);
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
