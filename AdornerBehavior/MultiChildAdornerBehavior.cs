using System;
using System.Collections.Generic;
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
        /// Dependency properties. 
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

        /// <summary>
        /// 用于Adorned FrameworkElement,表示保存的当前Adorner
        /// </summary>
        public static readonly DependencyProperty AdornerProperty =
            DependencyProperty.RegisterAttached("Adorner", typeof(FrameworkElementMultiChildAdorner), typeof(MultiChildAdornerBehavior));

        /// <summary>
        /// 用于Adorned FrameworkElement,表示Adorner Child的列表
        /// </summary>
        public static readonly DependencyProperty AdornerChildrenProperty =
            DependencyProperty.RegisterAttached("AdornerChildrenInternal", typeof(AdornerCollection), typeof(MultiChildAdornerBehavior)
            , new PropertyMetadata(OnAdornerChildrenPropertyChanged));
        /// <summary>
        /// Used in XAML to define the UI content of the adorner.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static AdornerCollection GetAdornerChildren(DependencyObject d)
        {
            var collection = (AdornerCollection)d.GetValue(AdornerChildrenProperty);
            if (collection == null)
            {
                collection = new AdornerCollection();
                d.SetValue(AdornerChildrenProperty, collection);
            }
            return collection;
        }
        public static void SetAdornerChildren(DependencyObject d, AdornerCollection value)
        {
            d.SetValue(AdornerChildrenProperty, value);
        }

        /// <summary>
        /// 用在每一个Adorner Child上
        /// </summary>
        public static readonly DependencyProperty HorizontalPlacementProperty =
            DependencyProperty.RegisterAttached("HorizontalPlacement", typeof(AdornerPlacement), typeof(MultiChildAdornerBehavior),
                new FrameworkPropertyMetadata(AdornerPlacement.Across));
        /// <summary>
        /// Specifies the horizontal placement of the adorner relative to the adorned control.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static AdornerPlacement GetHorizontalPlacement(DependencyObject d)
        {
            return (AdornerPlacement)d.GetValue(HorizontalPlacementProperty);
        }
        public static void SetHorizontalPlacement(DependencyObject d, AdornerPlacement value)
        {
            d.SetValue(HorizontalPlacementProperty, value);
        }

        /// <summary>
        /// 用在每一个Adorner Child上
        /// </summary>
        public static readonly DependencyProperty VerticalPlacementProperty =
            DependencyProperty.RegisterAttached("VerticalPlacement", typeof(AdornerPlacement), typeof(MultiChildAdornerBehavior),
                new FrameworkPropertyMetadata(AdornerPlacement.Across));
        /// <summary>
        /// Specifies the vertical placement of the adorner relative to the adorned control.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static AdornerPlacement GetVerticalPlacement(DependencyObject d)
        {
            return (AdornerPlacement)d.GetValue(VerticalPlacementProperty);
        }
        public static void SetVerticalPlacement(DependencyObject d, AdornerPlacement value)
        {
            d.SetValue(VerticalPlacementProperty, value);
        }

        /// <summary>
        /// 用在每一个Adorner Child上
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.RegisterAttached("OffsetX", typeof(double), typeof(MultiChildAdornerBehavior),
            new PropertyMetadata(.0));
        /// <summary>
        /// X offset of the adorner.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double GetOffsetX(DependencyObject d)
        {
            return (double)d.GetValue(OffsetXProperty);
        }
        public static void SetOffsetX(DependencyObject d, double value)
        {
            d.SetValue(OffsetXProperty, value);
        }

        /// <summary>
        /// 用在每一个Adorner Child上
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.RegisterAttached("OffsetY", typeof(double), typeof(MultiChildAdornerBehavior),
            new PropertyMetadata(.0));
        /// <summary>
        /// Y offset of the adorner.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
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
            var fe = d as FrameworkElement;
            UpdateAdorner(fe);
        }
        private static void OnAdornerChildrenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            UpdateAdorner(fe);
        }
        #endregion

        #region EventHandlers
        private static void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var fe = sender as FrameworkElement;
            if (fe != null)
            {
                var adornerChildren = GetAdornerChildren(fe);
                foreach (var adornerChild in adornerChildren)
                {
                    adornerChild.DataContext = fe.DataContext;
                }
            }
        }
        private static void OnAdornedFrameworkElementLoaded(object sender, RoutedEventArgs args)
        {
            UpdateAdorner((FrameworkElement)sender);
        }
        #endregion

        #region Methods
        private static void UpdateAdorner(FrameworkElement fe)
        {
            if (fe != null)
            {
                if (GetIsAdornerVisible(fe))
                {
                    ShowAdorner(fe);
                    fe.DataContextChanged += OnDataContextChanged;
                    fe.Loaded += OnAdornedFrameworkElementLoaded;
                }
                else
                {
                    HideAdorner(fe);
                    fe.DataContextChanged -= OnDataContextChanged;
                }
            }
        }
        private static void ShowAdorner(FrameworkElement fe)
        {
            var adornerChildren = GetAdornerChildren(fe);

            if (fe != null && fe.GetValue(AdornerProperty) == null && adornerChildren != null)
            {
                var al = AdornerLayer.GetAdornerLayer(fe);
                if (al != null)
                {
                    var adorner = new FrameworkElementMultiChildAdorner(adornerChildren, fe);
                    al.Add(adorner);
                    BindAdorner(fe, adorner);
                    fe.SetValue(AdornerProperty, adorner);
                }
            }
        }
        private static void HideAdorner(FrameworkElement fe)
        {
            //var adornerChildren = GetAdornerChildren(fe);

            if (fe != null && fe.GetValue(AdornerProperty) != null)
            {
                var al = AdornerLayer.GetAdornerLayer(fe);
                if (al != null)
                {
                    var adorner = fe.GetValue(AdornerProperty) as FrameworkElementMultiChildAdorner;
                    if (adorner != null)
                    {
                        al.Remove(adorner);
                        adorner.DisconnectChildren();
                        fe.SetValue(AdornerProperty, null);
                    }
                }
            }
        }
        private static void BindAdorner(FrameworkElement fe, FrameworkElementMultiChildAdorner adorner)
        {
            if (adorner == null)
            {
                throw new ArgumentNullException(nameof(adorner));
            }

            var binding = new Binding() { Path = new PropertyPath(AdornerChildrenProperty) };
            binding.Mode = BindingMode.OneWay;
            binding.Source = fe ?? throw new ArgumentNullException(nameof(fe));
            adorner.SetBinding(FrameworkElementMultiChildAdorner.AdornerChildrenProperty, binding);
        }
        #endregion
    }
}
