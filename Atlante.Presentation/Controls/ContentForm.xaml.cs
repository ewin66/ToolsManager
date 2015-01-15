using System.Windows;

namespace Atlante.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for ContentForm.xaml
    /// </summary>
    public partial class ContentForm : Window
    {
        private const int HORIZONTAL_TOLERANCE = 28;
        private const int VERTICAL_TOLERANCE = 78;

        public double ContentHeight { get; set; }
        public double ContentWidth { get; set; }
        public string CurrentTitle { get; set; }

        public FrameworkElement ContentElement { get; set; }

        public ContentForm( string title, FrameworkElement content )
        {
            this.DataContext = this;

            this.CurrentTitle = title;
            this.ContentWidth = content.MinWidth + HORIZONTAL_TOLERANCE;
            this.ContentHeight = content.MinHeight + VERTICAL_TOLERANCE;

            this.ContentElement = content;

            if ( this.ContentWidth == 0.0 )
                this.ContentWidth = 500;

            if ( this.ContentHeight == 0.0 )
                this.ContentWidth = 350;

            InitializeComponent( );
        }

        private void Close_Click( object sender, RoutedEventArgs e )
        {
            this.DialogResult = true;
        }

        private void Window_MouseLeftButtonDown( object sender, System.Windows.Input.MouseButtonEventArgs e )
        {
            this.DragMove( );
        }
    }
}
