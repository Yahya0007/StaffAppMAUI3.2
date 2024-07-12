using Microsoft.Maui.Controls;

namespace StaffApp.Controls {
    public partial class TitleView : TitleViewFix {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TitleView));
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        public TitleView() {
            InitializeComponent();
        }
    }
}
