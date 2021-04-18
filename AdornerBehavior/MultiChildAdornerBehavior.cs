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
    public static class MultiChildAdornerBehavior
    {
        #region Dependency Properties

        /// <summary>
        /// Apply this on Adorned FrameworkElement
        /// </summary>
        public static readonly DependencyProperty IsAdornerVisibleProperty =
            DependencyProperty.RegisterAttached("IsAdornerVisible", typeof(bool), typeof(MultiChildAdornerBehavior),
                new FrameworkPropertyMetadata(OnIsAdornerVisiblePropertyChanged));

        /// <summary>
        /// Shows or hides the adorner.
        /// Set to 'true' to show the adorner or 'false' to hide the adorner.
        /// </summary>
        public static bool GetIsAdornerVisible(DependencyObject d)
        {
            return (bool)d.GetValue(IsAdornerVisibleProperty);
        }

        public static void SetIsAdornerVisible(DependencyObject d, bool value)
        {
            d.SetValue(IsAdornerVisibleProperty, value);
        }

        public static readonly DependencyProperty AdornerProperty =
            DependencyProperty.RegisterAttached("Adorner", typeof(FrameworkElementMultiChildAdorner), typeof(MultiChildAdornerBehavior));


        public static readonly DependencyProperty AdornerChildrenProperty =
            DependencyProperty.RegisterAttached("AdornerChildrenInternal", typeof(AdornerCollection), typeof(MultiChildAdornerBehavior),
                new PropertyMetadata(OnAdornerChildrenPropertyChanged));

        /// <summary>
        /// Used in XAML to define the UI content of the adorner.
        /// </summary>
        public static AdornerCollection GetAdornerChildren(DependencyObject d)
        {
            var collection = (AdornerCollection)d.GetValue(AdornerChildrenProperty);
            if (collection == null)
            {
                collection = new AdornerCollection();
                d.SetValue(AdornerChildrenProperty, collection);

                var fe = (FrameworkElement)d;
                fe.DataContextChanged += OnDataContextChanged;
                fe.Loaded += OnAdornedFrameworkElementLoaded;
                fe.Unloaded += OnAdornedFrameworkElementUnloaded;
            }

            return collection;
        }

        public static void SetAdornerChildren(DependencyObject d, AdornerCollection value)
        {
            d.SetValue(AdornerChildrenProperty, value);
        }

        public static readonly DependencyProperty HorizontalPlacementProperty =
            DependencyProperty.RegisterAttached("HorizontalPlacement", typeof(AdornerPlacement), typeof(MultiChildAdornerBehavior),
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
            DependencyProperty.RegisterAttached("VerticalPlacement", typeof(AdornerPlacement), typeof(MultiChildAdornerBehavior),
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
            DependencyProperty.RegisterAttached("OffsetX", typeof(double), typeof(MultiChildAdornerBehavior), new PropertyMetadata(0.0));

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
            DependencyProperty.RegisterAttached("OffsetY", typeof(double), typeof(MultiChildAdornerBehavior), new PropertyMetadata(0.0));

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
        /// Event raised when the value of IsAdornerVisible has changed.
        /// </summary>
        private static void OnIsAdornerVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateAdorner((FrameworkElement)d);
        }

        private static void OnAdornerChildrenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

            var adornerChildren = GetAdornerChildren(fe);

            foreach (var adornerChild in adornerChildren)
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

            if (GetIsAdornerVisible(fe))
                ShowAdorner(fe);
            else
                HideAdorner(fe);
        }

        private static void ShowAdorner(FrameworkElement fe)
        {
            if (fe == null)
                return;

            var adornerChildren = GetAdornerChildren(fe);
            if (adornerChildren == null)
                return;

            var al = AdornerLayer.GetAdornerLayer(fe);
            if (al == null)
                return;

            // create new adorner if it doesnt exist
            var adorner = fe.GetValue(AdornerProperty) as FrameworkElementMultiChildAdorner;
            if (adorner == null)
            {
                adorner = new FrameworkElementMultiChildAdorner(adornerChildren, fe);
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

            var adorner = fe.GetValue(AdornerProperty) as FrameworkElementMultiChildAdorner;
            if (adorner == null)
                return;

            var al = AdornerLayer.GetAdornerLayer(fe);
            if (al != null)
                al.Remove(adorner);

            adorner.DisconnectChildren();
            fe.SetValue(AdornerProperty, null);
        }

        private static void BindAdorner(FrameworkElement fe, FrameworkElementMultiChildAdorner adorner)
        {
            if (adorner == null)
                throw new ArgumentNullException(nameof(adorner));

            if (fe == null)
                throw new ArgumentNullException(nameof(fe));

            var binding = new Binding() { Path = new PropertyPath(AdornerChildrenProperty) };
            binding.Mode = BindingMode.OneWay;
            binding.Source = fe;
            adorner.SetBinding(FrameworkElementMultiChildAdorner.AdornerChildrenProperty, binding);
        }

        #endregion
    }
}
