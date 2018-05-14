using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Raytracer.Extensions;
using Raytracer.Types;
using Buffer = Raytracer.Types.Buffer;

namespace Raytracer
{
    public class RenderEngine
    {
        private const int SAMPLES_PER_PIXEL = 4; // 4
        private const int FRAME_COUNT = 30; // 30
        private const bool USE_RANDOM_RAYS = false;
        private const bool USE_THREADS = true;
        
        private readonly Random globalRandom = new Random();
        private readonly Buffer buffer = new Buffer(Size2D.Zero);
        private Size2D size;

        private int currentProgress;
        private float progressSum;
        private BackgroundWorker bw = null;

        public RenderEngine(Scene scene)
        {
            if (scene.Camera == null) throw new ArgumentException("Camera missing in Scene!");

            Scene = scene;
        }

        public Scene Scene { get; }

        public void RenderAsync(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            this.bw = bw;
            args.Result = Render((Size2D) args.Argument);
        }

        public Buffer Render(Size2D size)
        {
            this.size = size;
            buffer.Resize(size);
            Scene.Camera.ScreenSize = size;
            progressSum = CalculateProgressSum(size);
            currentProgress = 0;

            for (int i = 0; i < FRAME_COUNT; i++)
            {
                TraceFrame(i);
            }

            return buffer;
        }

        private void TraceFrame(int frameCount)
        {
            if (USE_THREADS)
            {
                Parallel.For(0, size.Height, (i, state) =>
                {
                    TraceRow(i, frameCount);
                    Interlocked.Increment(ref currentProgress);
                    lock (bw)
                    {
                        bw?.ReportProgress((int)(currentProgress / progressSum * 100));
                    }
                });
            }
            else
            {
                for (int y = 0; y < size.Height; y++)
                {
                    TraceRow(y, frameCount);

                    currentProgress++;
                    bw?.ReportProgress((int) (currentProgress / progressSum * 100f));
                }
                    
            }
        }

        private void TraceRow(int y, int frameCount)
        {
            // genereate seed for a thread-local random variable
            int seed;
            lock (globalRandom)
            {
                seed = globalRandom.Next();
            }
            Random random = new Random(seed);

            double lerpFactor = frameCount / (double)(frameCount + 1);

            for (int x = 0; x < size.Width; x++)
            {
                var pos = new Point2D(x, y);

                // starting color, has to be 0,0,0
                var color = new Types.Color(0, 0, 0);

                for (var i = 0; i < SAMPLES_PER_PIXEL; i++)
                {
                    // trace ray
                    Ray ray;
                    if (USE_RANDOM_RAYS)
                        ray = Scene.Camera.GetRandomizedRay(x, y, random);
                    else
                        ray = Scene.Camera.GetRay(x, y);

                    Types.Color col = Trace(ray, random);

                    // weight color
                    color += col / SAMPLES_PER_PIXEL;
                }

                // lerp with previous color
                var prevCol = buffer.GetPixel(pos);
                color = prevCol * lerpFactor + color * (1 - lerpFactor);
                buffer.SetPixel(pos, color);
            }
        }

        private Color Trace(Ray ray, Random random)
        {
            var hit = HitWorld(ray);

            if (hit.Hit)
            {
                if (Scatter(hit, out Ray scattered, out Color attenuation, random)) // reflect new ray from the surface
                {
                    
                    return hit.Object.Material.Emissive + attenuation * Trace(scattered, random);
                }
                else // ray is absorbed -> no albedo shown
                {
                    return hit.Object.Material.Emissive;
                }
            }
            else // Display Skybox
            {
                return new Color(0.15f, 0.21f, 0.3f);
                return RenderSkybox(hit);
            }                                
        }

        private Types.Color RenderNDotL(RaycastHit hit)
        {
            var l = -new Vector3D(-1, -1, 1).Normalize();
            double product = l.Dot(hit.Normal);
            product = Math.Min(Math.Max(product, 0), 1);
            return hit.Object.Material.Albedo * product;
        }

        private Types.Color RenderSkybox(RaycastHit hit)
        {
            var r = hit.Ray.Direction;
            var t = 0.5 * (r.Y + 1);
            return (new Types.Color(1, 1, 1) * (1 - t) + new Types.Color(0.5, 0.5, 1) * t) * 0.3;
        }

        private RaycastHit HitWorld(Ray ray)
        {
            var cam = Scene.Camera;

            var closestHit = new RaycastHit
            {
                Hit = false,
                Ray = ray,
                DistanceToHit = cam.MaxPlane,
            };

            foreach (var renderable in Scene.Renderables)
            {
                var hit = renderable.HitObject(ray, cam.MinPlane, closestHit.DistanceToHit);

                if (hit.Hit && hit.DistanceToHit <= closestHit.DistanceToHit)  // second check not really necessary because we already pass it as new far plane to HitObject(..)
                {
                    closestHit = hit;
                }
            }

            return closestHit;
        }

        private bool Scatter(RaycastHit hit, out Ray scattered, out Types.Color attenuation, Random random)
        {
            Material mat = hit.Object.Material;

            switch (mat.Type)
            {
                case Material.MaterialType.Lambert:
                {
                    // add random offset to normal
                    var targetDir = hit.Normal + random.RandomUnitVector();
                    scattered = new Ray(hit.Position, targetDir.Normalize());
                    attenuation = mat.Albedo;

                    return true;
                }
            }

            scattered = default(Ray);
            attenuation = default(Types.Color);
            return false;
        }

        private static int CalculateProgressSum(Size2D size)
        {
            return FRAME_COUNT * size.Height;
        }
        
    }
}