using System;
using System.Windows;
using System.Windows.Input;
using Raytracer.Types;


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

            this.Loaded += GenerateImage_Event;
            this.SizeChanged += GenerateImage_Event;


            this.KeyDown += MainWindow_KeyDown;

            raytracer = new RenderEngine(Scene.BuildExampleScene());
        }

        private void GenerateImage_Event(object sender, RoutedEventArgs e)
        {
            GenerateImage();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            double stepSize = 0.3;
            var cam = raytracer.Scene.Camera;

            var pos = cam.Position;
            switch (e.Key)
            {
                case Key.W:
                    pos.Z += stepSize;
                    break;
                case Key.S:
                    pos.Z -= stepSize;
                    break;
                case Key.D:
                    pos.X += stepSize;
                    break;
                case Key.A:
                    pos.X -= stepSize;
                    break;
            }

            if (cam.Position != pos)
            {
                cam.RecalculateMatrix(pos, pos + Vector3D.Forward);
                GenerateImage();
            }
            
        }

        private void GenerateImage()
        {
            var size = new Size2D(Viewbox.ActualWidth, Viewbox.ActualHeight);
            if (size == Size2D.Zero)
                    size = new Size2D(1, 1);

            var renderOutput = raytracer.Render(size);

            RenderOutput.Source = renderOutput;
        }

    }
}
