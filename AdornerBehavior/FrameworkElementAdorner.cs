using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

// The code is based on:
// http://www.codeproject.com/KB/WPF/WPFJoshSmith.aspx

namespace AdornerBehavior
{
    /// <summary>
    /// This class is an adorner that allows a FrameworkElement derived class to adorn multiple FrameworkElements.
    /// </summary>
    public class FrameworkElementAdorner : Adorner
    {
        public FrameworkElementAdorner(AdornerCollection adorners, FrameworkElement adornedElement) : base(adornedElement)
        {
            this.Adorners = adorners;

            // AdornerBehavior is responsible for the connection and disconnection of adorners.
            // this.ConnectChildren(adornedElement);

            adornedElement.SizeChanged += this.AdornedElement_SizeChanged;
        }

        #region Dependency Properties

        /// <summary>
        /// Collection of adorners.
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

        public static FrameworkElement GetAdornedElement(DependencyObject d)
        {
            return (FrameworkElement)d.GetValue(AdornedElementProperty);
        }

        public static void SetAdornedElement(DependencyObject d, FrameworkElement value)
        {
            d.SetValue(AdornedElementProperty, value);
        }

        /// <summary>
        /// Position X of adorners.
        /// </summary>
        public static readonly DependencyProperty PositionXProperty =
            DependencyProperty.RegisterAttached("PositionX", typeof(double), typeof(FrameworkElementAdorner), new PropertyMetadata(double.NaN));


        public static double GetPositionX(DependencyObject d)
        {
            return (double)d.GetValue(PositionXProperty);
        }

        public static void SetPositionX(DependencyObject d, double value)
        {
            d.SetValue(PositionXProperty, value);
        }

        /// <summary>
        /// Position Y of adorners.
        /// </summary>
        public static readonly DependencyProperty PositionYProperty =
            DependencyProperty.RegisterAttached("PositionY", typeof(double), typeof(FrameworkElementAdorner), new PropertyMetadata(double.NaN));

        public static double GetPositionY(DependencyObject d)
        {
            return (double)d.GetValue(PositionYProperty);
        }

        public static void SetPositionY(DependencyObject d, double value)
        {
            d.SetValue(PositionYProperty, value);
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// Event raised when the adorned control's size has changed.
        /// </summary>
        private void AdornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determine the X coordinate of the adorner.
        /// </summary>
        private double DetermineX(FrameworkElement adorner)
        {
            var horizontalPlacement = AdornerBehavior.GetHorizontalPlacement(adorner);
            var offsetX = AdornerBehavior.GetOffsetX(adorner);
            switch (adorner.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    {
                        if (horizontalPlacement == AdornerPlacement.Across)
                            return -adorner.DesiredSize.Width / 2 + offsetX;
                        if (horizontalPlacement == AdornerPlacement.Outside)
                            return -adorner.DesiredSize.Width + offsetX;

                        return offsetX;
                    }
                case HorizontalAlignment.Right:
                    {
                        if (horizontalPlacement == AdornerPlacement.Across)
                        {
                            var adornerWidth = adorner.DesiredSize.Width;
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
                            var adornerWidth = adorner.DesiredSize.Width;
                            var adornedWidth = this.AdornedElement.ActualWidth;
                            var x = adornedWidth - adornerWidth;
                            return x + offsetX;
                        }
                    }
                case HorizontalAlignment.Center:
                    {
                        var adornerWidth = adorner.DesiredSize.Width;
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
        /// Determine the Y coordinate of the adorner.
        /// </summary>
        private double DetermineY(FrameworkElement adorner)
        {
            var verticalPlacement = AdornerBehavior.GetVerticalPlacement(adorner);
            var offsetY = AdornerBehavior.GetOffsetY(adorner);

            switch (adorner.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    {
                        if (verticalPlacement == AdornerPlacement.Across)
                            return -adorner.DesiredSize.Height / 2 + offsetY;

                        if (verticalPlacement == AdornerPlacement.Outside)
                            return -adorner.DesiredSize.Height + offsetY;

                        return offsetY;
                    }
                case VerticalAlignment.Bottom:
                    {
                        if (verticalPlacement == AdornerPlacement.Across)
                        {
                            var adornerHeight = adorner.DesiredSize.Height;
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
                            var adornerHeight = adorner.DesiredSize.Height;
                            var adornedHeight = this.AdornedElement.ActualHeight;
                            var x = adornedHeight - adornerHeight;
                            return x + offsetY;
                        }
                    }
                case VerticalAlignment.Center:
                    {
                        var adornerHeight = adorner.DesiredSize.Height;
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
        /// Determine the width of the adorner.
        /// </summary>
        private double DetermineWidth(FrameworkElement adorner)
        {
            var positionX = GetPositionX(adorner);
            if (!double.IsNaN(positionX))
                return adorner.DesiredSize.Width;

            switch (adorner.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    {
                        return adorner.DesiredSize.Width;
                    }
                case HorizontalAlignment.Right:
                    {
                        return adorner.DesiredSize.Width;
                    }
                case HorizontalAlignment.Center:
                    {
                        return adorner.DesiredSize.Width;
                    }
                case HorizontalAlignment.Stretch:
                    {
                        return this.AdornedElement.ActualWidth;
                    }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the height of the adorner.
        /// </summary>
        private double DetermineHeight(FrameworkElement adorner)
        {
            var positionY = GetPositionY(adorner);
            if (!double.IsNaN(positionY))
                return adorner.DesiredSize.Height;

            switch (adorner.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    {
                        return adorner.DesiredSize.Height;
                    }
                case VerticalAlignment.Bottom:
                    {
                        return adorner.DesiredSize.Height;
                    }
                case VerticalAlignment.Center:
                    {
                        return adorner.DesiredSize.Height;
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
            foreach (var adorner in this.Adorners)
            {
                var x = GetPositionX(adorner);

                if (double.IsNaN(x))
                    x = this.DetermineX(adorner);

                var y = GetPositionY(adorner);
                if (double.IsNaN(y))
                    y = this.DetermineY(adorner);

                var adornerWidth = this.DetermineWidth(adorner);
                var adornerHeight = this.DetermineHeight(adorner);
                adorner.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
            }
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (this.Adorners == null)
                return null;

            return new List<FrameworkElement>(this.Adorners)[index];
        }

        /// <summary>
        /// Remove all adorners from the visual tree.
        /// </summary>
        public void DisconnectChildren()
        {
            foreach (var adorner in this.Adorners)
            {
                this.DisconnectAdorner(adorner);
            }
        }

        /// <summary>
        /// Add all adorners to the visual tree.
        /// </summary>
        public void ConnectChildren(FrameworkElement adornedElement)
        {
            foreach (var adorner in this.Adorners)
            {
                this.ConnectAdorner(adornedElement, adorner);
            }
        }

        /// <summary>
        /// Add a single adorner to the visual tree.
        /// </summary>
        public void ConnectAdorner(FrameworkElement adornedElement, FrameworkElement adorner)
        {
            this.AddLogicalChild(adorner);
            this.AddVisualChild(adorner);
            SetAdornedElement(adorner, adornedElement);
        }

        /// <summary>
        /// Remove a single adorner from the visual tree.
        /// </summary>
        public void DisconnectAdorner(FrameworkElement adorner)
        {
            this.RemoveLogicalChild(adorner);
            this.RemoveVisualChild(adorner);
            SetAdornedElement(adorner, null);
        }

        #endregion

        #region Properties

        protected override IEnumerator LogicalChildren
        {
            get
            {
                return this.Adorners.GetEnumerator();
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return this.Adorners.Count;
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

        #endregion
    }
}
