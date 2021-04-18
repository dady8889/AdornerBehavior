using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace AdornerBehavior
{
    /// <summary>
    /// A behavior that allows an adorner for multiple child to
    /// be defined in XAML.
    /// </summary>
    public static class AdornerBehavior
    {
        #region Dependency Properties

        /// <summary>
        /// Apply this on Adorned FrameworkElement
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(AdornerBehavior),
                new FrameworkPropertyMetadata(OnIsEnabledPropertyChanged));

        /// <summary>
        /// Shows or hides the adorner.
        /// Set to 'true' to show the adorner or 'false' to hide the adorner.
        /// </summary>
        public static bool GetIsEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject d, bool value)
        {
            d.SetValue(IsEnabledProperty, value);
        }

        public static readonly DependencyProperty AdornerProperty =
            DependencyProperty.RegisterAttached("Adorner", typeof(FrameworkElementAdorner), typeof(AdornerBehavior));


        /// <summary>
        /// Used in XAML to define the collection of adorners.
        /// </summary>
        public static readonly DependencyProperty AdornersProperty =
            DependencyProperty.RegisterAttached("AdornersInternal", typeof(AdornerCollection), typeof(AdornerBehavior),
                new PropertyMetadata(OnAdornersPropertyChanged));

        public static AdornerCollection GetAdorners(DependencyObject d)
        {
            var collection = (AdornerCollection)d.GetValue(AdornersProperty);
            if (collection == null)
            {
                collection = new AdornerCollection();
                d.SetValue(AdornersProperty, collection);

                var fe = (FrameworkElement)d;
                fe.DataContextChanged += OnDataContextChanged;
                fe.Loaded += OnAdornedFrameworkElementLoaded;
                fe.Unloaded += OnAdornedFrameworkElementUnloaded;
            }

            return collection;
        }

        public static void SetAdorners(DependencyObject d, AdornerCollection value)
        {
            d.SetValue(AdornersProperty, value);
        }

        public static readonly DependencyProperty HorizontalPlacementProperty =
            DependencyProperty.RegisterAttached("HorizontalPlacement", typeof(AdornerPlacement), typeof(AdornerBehavior),
                new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        /// <summary>
        /// Specifies the horizontal placement of the adorner relative to the adorned control.
        /// </summary>
        public static AdornerPlacement GetHorizontalPlacement(DependencyObject d)
        {
            return (AdornerPlacement)d.GetValue(HorizontalPlacementProperty);
        }
        public static void SetHorizontalPlacement(DependencyObject d, AdornerPlacement value)
        {
            d.SetValue(HorizontalPlacementProperty, value);
        }

        public static readonly DependencyProperty VerticalPlacementProperty =
            DependencyProperty.RegisterAttached("VerticalPlacement", typeof(AdornerPlacement), typeof(AdornerBehavior),
                new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        /// <summary>
        /// Specifies the vertical placement of the adorner relative to the adorned control.
        /// </summary>
        public static AdornerPlacement GetVerticalPlacement(DependencyObject d)
        {
            return (AdornerPlacement)d.GetValue(VerticalPlacementProperty);
        }
        public static void SetVerticalPlacement(DependencyObject d, AdornerPlacement value)
        {
            d.SetValue(VerticalPlacementProperty, value);
        }

        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.RegisterAttached("OffsetX", typeof(double), typeof(AdornerBehavior), new PropertyMetadata(0.0));

        /// <summary>
        /// X offset of the adorner.
        /// </summary>
        public static double GetOffsetX(DependencyObject d)
        {
            return (double)d.GetValue(OffsetXProperty);
        }
        public static void SetOffsetX(DependencyObject d, double value)
        {
            d.SetValue(OffsetXProperty, value);
        }

        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.RegisterAttached("OffsetY", typeof(double), typeof(AdornerBehavior), new PropertyMetadata(0.0));

        /// <summary>
        /// Y offset of the adorner.
        /// </summary>
        public static double GetOffsetY(DependencyObject d)
        {
            return (double)d.GetValue(OffsetYProperty);
        }

        public static void SetOffsetY(DependencyObject d, double value)
        {
            d.SetValue(OffsetYProperty, value);
        }

        #endregion Dependency Properties

        #region Callbacks

        /// <summary>
        /// Event raised when the value of IsEnabled has changed.
        /// </summary>
        private static void OnIsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateAdorner((FrameworkElement)d);
        }

        private static void OnAdornersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateAdorner((FrameworkElement)d);
        }

        #endregion

        #region EventHandlers

        private static void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var fe = sender as FrameworkElement;
            if (fe == null)
                return;

            var adorners = GetAdorners(fe);

            foreach (var adornerChild in adorners)
                adornerChild.DataContext = fe.DataContext;
        }

        private static void OnAdornedFrameworkElementLoaded(object sender, RoutedEventArgs args)
        {
            UpdateAdorner((FrameworkElement)sender);
        }

        private static void OnAdornedFrameworkElementUnloaded(object sender, RoutedEventArgs args)
        {
            HideAdorner((FrameworkElement)sender);
        }

        #endregion

        #region Methods

        private static void UpdateAdorner(FrameworkElement fe)
        {
            if (fe == null)
                return;

            if (GetIsEnabled(fe))
                ShowAdorner(fe);
            else
                HideAdorner(fe);
        }

        private static void ShowAdorner(FrameworkElement fe)
        {
            if (fe == null)
                return;

            var adorners = GetAdorners(fe);
            if (adorners == null)
                return;

            var al = AdornerLayer.GetAdornerLayer(fe);
            if (al == null)
                return;

            // create new adorner if it doesnt exist
            var adorner = fe.GetValue(AdornerProperty) as FrameworkElementAdorner;
            if (adorner == null)
            {
                adorner = new FrameworkElementAdorner(adorners, fe);
                fe.SetValue(AdornerProperty, adorner);
                BindAdorner(fe, adorner);
            }

            if (adorner.Parent == null)
            {
                adorner.ConnectChildren(fe);
                al.Add(adorner);
            }
        }

        private static void HideAdorner(FrameworkElement fe)
        {
            if (fe == null)
                return;

            var adorner = fe.GetValue(AdornerProperty) as FrameworkElementAdorner;
            if (adorner == null)
                return;

            var al = AdornerLayer.GetAdornerLayer(fe);
            if (al != null)
                al.Remove(adorner);

            adorner.DisconnectChildren();
            fe.SetValue(AdornerProperty, null);
        }

        private static void BindAdorner(FrameworkElement fe, FrameworkElementAdorner adorner)
        {
            if (adorner == null)
                throw new ArgumentNullException(nameof(adorner));

            if (fe == null)
                throw new ArgumentNullException(nameof(fe));

            var binding = new Binding() { Path = new PropertyPath(AdornersProperty) };
            binding.Mode = BindingMode.OneWay;
            binding.Source = fe;
            adorner.SetBinding(FrameworkElementAdorner.AdornersProperty, binding);
        }

        #endregion
    }
}
