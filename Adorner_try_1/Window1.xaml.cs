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
using System.Windows.Shapes;

namespace adorn_2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        public AdornerLayer myAdornerLayer { get; set; }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            myAdornerLayer = AdornerLayer.GetAdornerLayer(win);
            myAdornerLayer.Add(new SimpleCircleAdorner(win));

            //myAdornerLayer = AdornerLayer.GetAdornerLayer(pnl);
            //foreach (UIElement toAdorn in pnl.Children)
            //    myAdornerLayer.Add(new SimpleCircleAdorner(toAdorn));
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }// Adorners must subclass the abstract base class Adorner. 
    public class SimpleCircleAdorner : Adorner
    {
        // Be sure to call the base class constructor. 
        public SimpleCircleAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
        }

        // A common way to implement an adorner's rendering behavior is to override the OnRender 
        // method, which is called by the layout system as part of a rendering pass. 
        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            // Some arbitrary drawing implements.
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            double renderRadius = 5.0;

            // Draw a circle at each corner.
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
        }
    }
}
