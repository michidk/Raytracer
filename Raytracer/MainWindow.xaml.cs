using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Raytracer.Types;
using Raytracer.Utils;
using Buffer = Raytracer.Types.Buffer;

namespace Raytracer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const bool AUTO_START_RENDERING = true;
        private const bool USE_BACKGROUND_WORKER = true;


        private const int WM_EXIT_SIZE_MOVE = 0x232;
        private readonly BackgroundWorker bw;

        private readonly RenderEngine raytracer;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;

            raytracer = new RenderEngine(Scene.BuildExampleScene());
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Image has to be initialized with a minimal bitmap to correctly calculate dimensions
            RenderOutput.Source = BitmapUtils.CreateEmptyBitmap();
            UpdateLayout();

            if (AUTO_START_RENDERING)
                GenerateImage();

            // now listen to key presses
            KeyDown += OnKeyDown;

            // add hook to subscribe to hwn message events
            var helper = new WindowInteropHelper(this);
            var source = HwndSource.FromHwnd(helper.Handle);
            source?.AddHook(HwndMessageHook);
        }

        private void OnUpdateButtonPressed(object sender, RoutedEventArgs e)
        {
            GenerateImage();
        }

        private IntPtr HwndMessageHook(IntPtr wnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_EXIT_SIZE_MOVE:
                    GenerateImage();
                    handled = true;
                    break;
            }

            return IntPtr.Zero;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var stepSize = 0.5;
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
                cam.SetPositionAndDirection(pos, -pos);
                GenerateImage();
            }
        }

        private void GenerateImage()
        {
            var size = new Size2D(RenderOutput.ActualWidth, RenderOutput.ActualHeight);
            if (size == Size2D.Zero)
                return;

            if (bw.IsBusy)
                return;

            ProgressBar.Value = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (!USE_BACKGROUND_WORKER)
            {
                RenderOutput.Source = BitmapUtils.GetBitmapSourceFromArray(raytracer.Render(size).RawData, size);
                stopwatch.Stop();
                Console.WriteLine($@"Time: {stopwatch.ElapsedMilliseconds}ms, Ticks: {stopwatch.ElapsedTicks}");
            }
            else
            {
                bw.DoWork -= raytracer.RenderAsync;
                bw.DoWork += raytracer.RenderAsync;

                bw.ProgressChanged += (sender, args) => { ProgressBar.Value = args.ProgressPercentage; };
                bw.RunWorkerCompleted += (sender, args) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        RenderOutput.Source = BitmapUtils.GetBitmapSourceFromArray(((Buffer)args.Result).RawData, size);
                        stopwatch.Stop();
                        Console.WriteLine($@"Time: {stopwatch.ElapsedMilliseconds}ms, Ticks: {stopwatch.ElapsedTicks}");
                    });
                };
                bw.RunWorkerAsync(size);
            }

        }
    }
}