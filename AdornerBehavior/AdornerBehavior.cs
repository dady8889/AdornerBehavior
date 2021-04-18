using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace AdornerBehavior
{
    /// <summary>
    /// A behavior that allows a collection of adorners be defined in XAML.
    /// </summary>
    public static class AdornerBehavior
    {
        #region Dependency Properties

        /// <summary>
        /// Apply this on the target element you want to adorn.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(AdornerBehavior),
                new FrameworkPropertyMetadata(OnIsEnabledPropertyChanged));

        public static bool GetIsEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject d, bool value)
        {
            d.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// The internal encapsulating adorner.
        /// </summary>
        public static readonly DependencyProperty AdornerProperty =
            DependencyProperty.RegisterAttached("Adorner", typeof(FrameworkElementAdorner), typeof(AdornerBehavior));

        /// <summary>
        /// Used in XAML to define the collection of adorners.
        /// </summary>
        /// <remarks>
        /// The Get property of this DependencyProperty is basically a constructor for this behavior.
        /// https://stackoverflow.com/questions/1448899/attached-property-of-type-list
        /// </remarks>
        public static readonly DependencyProperty AdornersProperty =
            DependencyProperty.RegisterAttached("AdornersInternal", typeof(AdornerCollection), typeof(AdornerBehavior),
                new PropertyMetadata(OnAdornersPropertyChanged));

        public static AdornerCollection GetAdorners(DependencyObject d)
        {
            var collection = (AdornerCollection)d.GetValue(AdornersProperty);
            if (collection == null)
            {
                var fe = (FrameworkElement)d;

                collection = new AdornerCollection(fe);
                collection.CollectionChanged += Adorners_CollectionChanged;

                d.SetValue(AdornersProperty, collection);

                fe.DataContextChanged += AdornedFrameworkElement_DataContextChanged;
                fe.Loaded += AdornedFrameworkElement_Loaded;
                fe.Unloaded += AdornedFrameworkElement_Unloaded;
            }

            return collection;
        }

        public static void SetAdorners(DependencyObject d, AdornerCollection value)
        {
            d.SetValue(AdornersProperty, value);
        }

        /// <summary>
        /// The horizontal placement of the adorner. Used only on Left or Right horizontally aligned adorners.
        /// </summary>
        public static readonly DependencyProperty HorizontalPlacementProperty =
            DependencyProperty.RegisterAttached("HorizontalPlacement", typeof(AdornerPlacement), typeof(AdornerBehavior),
                new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        public static AdornerPlacement GetHorizontalPlacement(DependencyObject d)
        {
            return (AdornerPlacement)d.GetValue(HorizontalPlacementProperty);
        }

        public static void SetHorizontalPlacement(DependencyObject d, AdornerPlacement value)
        {
            d.SetValue(HorizontalPlacementProperty, value);
        }

        /// <summary>
        /// The vertical placement of the adorner. Used only on Top or Bottom vertically aligned adorners.
        /// </summary>
        public static readonly DependencyProperty VerticalPlacementProperty =
            DependencyProperty.RegisterAttached("VerticalPlacement", typeof(AdornerPlacement), typeof(AdornerBehavior),
                new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        public static AdornerPlacement GetVerticalPlacement(DependencyObject d)
        {
            return (AdornerPlacement)d.GetValue(VerticalPlacementProperty);
        }

        public static void SetVerticalPlacement(DependencyObject d, AdornerPlacement value)
        {
            d.SetValue(VerticalPlacementProperty, value);
        }

        /// <summary>
        /// X axis offset relative to the specified placement and alignment.
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.RegisterAttached("OffsetX", typeof(double), typeof(AdornerBehavior), new PropertyMetadata(0.0));

        public static double GetOffsetX(DependencyObject d)
        {
            return (double)d.GetValue(OffsetXProperty);
        }

        public static void SetOffsetX(DependencyObject d, double value)
        {
            d.SetValue(OffsetXProperty, value);
        }

        /// <summary>
        /// Y axis offset relative to the specified placement and alignment.
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.RegisterAttached("OffsetY", typeof(double), typeof(AdornerBehavior), new PropertyMetadata(0.0));

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

        /// <summary>
        /// Event raised when the value of Adorners has changed.
        /// </summary>
        private static void OnAdornersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateAdorner((FrameworkElement)d);
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// Let the adorners in the collection inherit parent DataContext.
        /// </summary>
        private static void AdornedFrameworkElement_DataContextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var fe = sender as FrameworkElement;
            if (fe == null)
                return;

            var adorners = GetAdorners(fe);

            foreach (var adorner in adorners)
                adorner.DataContext = fe.DataContext;
        }

        private static void AdornedFrameworkElement_Loaded(object sender, RoutedEventArgs args)
        {
            UpdateAdorner((FrameworkElement)sender);
        }

        private static void AdornedFrameworkElement_Unloaded(object sender, RoutedEventArgs args)
        {
            HideAdorner((FrameworkElement)sender);
        }

        private static void Adorners_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var adornedElement = (FrameworkElement)sender;
            var encapsulatingAdorner = adornedElement.GetValue(AdornerProperty) as FrameworkElementAdorner;
            if (encapsulatingAdorner == null)
            {
                // The statically typed Adorners in XAML will not pass through this function, because the view is not rendered yet.
                return;
            }

            //System.Diagnostics.Debug.WriteLine($"Action: {e.Action} NewItems: {e.NewItems?.Count ?? 0} OldItems: {e.OldItems?.Count ?? 0}");

            var newItems = e.NewItems?.Cast<FrameworkElement>()?.ToList();
            var oldItems = e.OldItems?.Cast<FrameworkElement>()?.ToList();

            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (newItems != null)
                    {
                        foreach (var adorner in newItems)
                            encapsulatingAdorner.ConnectAdorner(adornedElement, adorner);
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if (oldItems != null)
                    {
                        foreach (var adorner in oldItems)
                            encapsulatingAdorner.DisconnectAdorner(adorner);
                    }

                    break;
                default:
                    break;
            }

            encapsulatingAdorner.InvalidateMeasure();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Decide whether to show or hide the adorner collection.
        /// </summary>
        private static void UpdateAdorner(FrameworkElement fe)
        {
            if (fe == null)
                return;

            if (GetIsEnabled(fe))
                ShowAdorner(fe);
            else
                HideAdorner(fe);
        }

        /// <summary>
        /// Create a new encapsulating adorner or reuse existing and connect the children.
        /// </summary>
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

        /// <summary>
        /// Detach the encapsulating adorner from the visual tree, including children.
        /// </summary>
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

        /// <summary>
        /// Bind the adorner collection to the encapsulating adorner.
        /// </summary>
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
