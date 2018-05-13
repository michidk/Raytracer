using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Raytracer.Types;
using Raytracer.Utils;
using Buffer = Raytracer.Types.Buffer;


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

            this.Loaded += OnLoaded;

            raytracer = new RenderEngine(Scene.BuildExampleScene());
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // subscribe to sized event after finishing loading (otherwise we will get lots of 0,0 calls)
            this.SizeChanged += OnSizeChanged;

            // Image has to be initialized with a minimal bitmap to correctly calculate dimensions
            RenderOutput.Source = BitmapUtils.CreateEmptyBitmap();
            UpdateLayout();

            GenerateImage();

            // now listen to key presses
            this.KeyDown += MainWindow_KeyDown;
        }

        private void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            GenerateImage();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            double stepSize = 0.5;
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
            var size = new Size2D(RenderOutput.ActualWidth, RenderOutput.ActualHeight);
            if (size == Size2D.Zero)
                return;

            RenderOutput.Source = raytracer.Render(size);
        }

    }
}
