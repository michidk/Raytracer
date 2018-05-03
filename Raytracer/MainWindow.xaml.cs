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
using Raytracer.CustomMath;

namespace Raytracer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private RenderEngine raytracer;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (object sender, RoutedEventArgs e) =>
            {
                GenerateImage();
            };
            this.SizeChanged += (object sender, SizeChangedEventArgs e) =>
            {
                GenerateImage();
            };


            raytracer = new RenderEngine();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateImage();
        }

        private void GenerateImage()
        {
            var size = new Size2D((int) Viewbox.ActualWidth, (int) Viewbox.ActualHeight);
            if (size == Size2D.Zero)
                size = new Size2D(1, 1);

            var renderOutput = raytracer.Render(size);

            RenderOutput.Source = renderOutput;
        }

    }
}
