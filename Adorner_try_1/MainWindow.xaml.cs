using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace adorn_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(Btn).Add(new FourBoxes(Btn));
        }
    }
    class FourBoxes : Adorner
    {
        public FourBoxes(UIElement adornedElement) :
            base(adornedElement)
        {
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Brushes.Red, null, new System.Windows.Rect(0, 0, 5, 5));
            drawingContext.DrawRectangle(Brushes.Red, null, new System.Windows.Rect(0, ActualHeight - 5, 5, 5));
            drawingContext.DrawRectangle(Brushes.Red, null, new System.Windows.Rect(ActualWidth - 5, 0, 5, 5));
            drawingContext.DrawRectangle(Brushes.Red, null, new System.Windows.Rect(ActualWidth - 5, ActualHeight - 5, 5, 5));
        }
    }
}

