using Insight.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Insight.Views
{
    public sealed partial class OrganizationsDetailControl : UserControl
    {
        public SampleOrder ListMenuItem
        {
            get { return GetValue(ListMenuItemProperty) as SampleOrder; }
            set { SetValue(ListMenuItemProperty, value); }
        }

        public static readonly DependencyProperty ListMenuItemProperty = DependencyProperty.Register("ListMenuItem", typeof(SampleOrder), typeof(OrganizationsDetailControl), new PropertyMetadata(null, OnListMenuItemPropertyChanged));

        public OrganizationsDetailControl()
        {
            InitializeComponent();
        }

        private static void OnListMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OrganizationsDetailControl control = d as OrganizationsDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
