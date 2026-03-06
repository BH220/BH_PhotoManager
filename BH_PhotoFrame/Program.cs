using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SDL2;
using SkiaSharp;

namespace BH_PhotoFrame
{
    enum Command
    {
        Next,
        Prev,
        Play,
        Pause,
        Rescan
    }

    class Program
    {
        static IntPtr window;
        static IntPtr renderer;
        static IntPtr texture;

        static int width = 1920;
        static int height = 1080;

        static List<string> photoList = new();
        static int photoIndex = 0;

        static SKBitmap prevImage;
        static SKBitmap currentImage;
        static SKBitmap nextImage;

        static string currentPath;

        static bool isFading = false;
        static float fadeProgress = 0f;

        static bool isPlaying = true;

        static DateTime lastSlideTime = DateTime.Now;

        const float fadeDuration = 0.5f;
        const float slideDuration = 5f;

        static void Main(string[] args)
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            window = SDL.SDL_CreateWindow(
                "BH_PhotoFrame",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                width,
                height,
                SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
            );

            renderer = SDL.SDL_CreateRenderer(window, -1, 0);

            texture = SDL.SDL_CreateTexture(
                renderer,
                SDL.SDL_PIXELFORMAT_ABGR8888,
                (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING,
                width,
                height
            );

            photoList = ScanPhotos("photo");

            if (photoList.Count > 0)
                PrepareImages();

            RunLoop();

            SDL.SDL_DestroyTexture(texture);
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }

        static List<string> ScanPhotos(string folder)
        {
            if (!Directory.Exists(folder))
                return new List<string>();

            var files = Directory.GetFiles(folder)
                .Where(f =>
                    f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                .ToList();

            Random rng = new Random();
            return files.OrderBy(x => rng.Next()).ToList();
        }

        static SKBitmap LoadImage(string path)
        {
            using var stream = File.OpenRead(path);
            return SKBitmap.Decode(stream);
        }

        static void PrepareImages()
        {
            currentPath = photoList[photoIndex];
            currentImage = LoadImage(currentPath);

            int nextIndex = (photoIndex + 1) % photoList.Count;
            nextImage = LoadImage(photoList[nextIndex]);
        }

        static void StartFade()
        {
            isFading = true;
            fadeProgress = 0f;
        }

        static void FinishFade()
        {
            prevImage?.Dispose();

            prevImage = currentImage;
            currentImage = nextImage;

            photoIndex = (photoIndex + 1) % photoList.Count;

            int nextIndex = (photoIndex + 1) % photoList.Count;

            nextImage = LoadImage(photoList[nextIndex]);

            currentPath = photoList[photoIndex];

            isFading = false;
            lastSlideTime = DateTime.Now;
        }

        static void ExecuteCommand(Command cmd)
        {
            switch (cmd)
            {
                case Command.Next:
                    ForceNext();
                    break;

                case Command.Prev:
                    ForcePrev();
                    break;

                case Command.Play:
                    isPlaying = true;
                    lastSlideTime = DateTime.Now;
                    break;

                case Command.Pause:
                    isPlaying = false;
                    break;

                case Command.Rescan:
                    RescanPhotos();
                    break;
            }
        }

        static void ForceNext()
        {
            if (photoList.Count == 0) return;

            prevImage?.Dispose();

            prevImage = currentImage;
            currentImage = nextImage;

            photoIndex = (photoIndex + 1) % photoList.Count;

            int nextIndex = (photoIndex + 1) % photoList.Count;

            nextImage = LoadImage(photoList[nextIndex]);

            currentPath = photoList[photoIndex];

            lastSlideTime = DateTime.Now;
        }

        static void ForcePrev()
        {
            if (photoList.Count == 0) return;

            prevImage?.Dispose();
            nextImage?.Dispose();

            photoIndex--;

            if (photoIndex < 0)
                photoIndex = photoList.Count - 1;

            currentImage = LoadImage(photoList[photoIndex]);
            currentPath = photoList[photoIndex];

            int nextIndex = (photoIndex + 1) % photoList.Count;
            nextImage = LoadImage(photoList[nextIndex]);

            lastSlideTime = DateTime.Now;
        }

        static void RescanPhotos()
        {
            photoList = ScanPhotos("photo");

            if (photoList.Count == 0)
                return;

            photoIndex = 0;

            prevImage?.Dispose();
            currentImage?.Dispose();
            nextImage?.Dispose();

            PrepareImages();

            lastSlideTime = DateTime.Now;
        }

        static void RunLoop()
        {
            bool running = true;

            SKImageInfo info = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);

            byte[] pixelBuffer = new byte[width * height * 4];

            var handle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
            IntPtr bufferPtr = handle.AddrOfPinnedObject();

            while (running)
            {
                SDL.SDL_Event e;

                while (SDL.SDL_PollEvent(out e) == 1)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                        running = false;

                    if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        switch (e.key.keysym.sym)
                        {
                            case SDL.SDL_Keycode.SDLK_F7:
                                ExecuteCommand(Command.Prev);
                                break;

                            case SDL.SDL_Keycode.SDLK_F8:
                                ExecuteCommand(Command.Next);
                                break;

                            case SDL.SDL_Keycode.SDLK_F9:
                                ExecuteCommand(Command.Play);
                                break;

                            case SDL.SDL_Keycode.SDLK_F10:
                                ExecuteCommand(Command.Pause);
                                break;

                            case SDL.SDL_Keycode.SDLK_F5:
                                ExecuteCommand(Command.Rescan);
                                break;
                        }
                    }
                }

                var now = DateTime.Now;

                if (isPlaying)
                {
                    if (!isFading)
                    {
                        if ((now - lastSlideTime).TotalSeconds >= slideDuration)
                            StartFade();
                    }
                    else
                    {
                        fadeProgress += 1f / (fadeDuration * 60f);

                        if (fadeProgress >= 1f)
                        {
                            fadeProgress = 1f;
                            FinishFade();
                        }
                    }
                }

                using (var surface = SKSurface.Create(info, bufferPtr, width * 4))
                {
                    var canvas = surface.Canvas;

                    canvas.Clear(SKColors.Black);

                    if (currentImage != null)
                    {
                        float x = (width - currentImage.Width) / 2f;
                        float y = (height - currentImage.Height) / 2f;

                        if (!isFading)
                        {
                            canvas.DrawBitmap(currentImage, x, y);
                        }
                        else
                        {
                            using var paintA = new SKPaint
                            {
                                Color = SKColors.White.WithAlpha((byte)(255 * (1f - fadeProgress)))
                            };

                            using var paintB = new SKPaint
                            {
                                Color = SKColors.White.WithAlpha((byte)(255 * fadeProgress))
                            };

                            canvas.DrawBitmap(currentImage, x, y, paintA);

                            if (nextImage != null)
                                canvas.DrawBitmap(nextImage, x, y, paintB);
                        }

                        DrawDate(canvas);
                    }
                }

                IntPtr pixels;
                int pitch;

                SDL.SDL_LockTexture(texture, IntPtr.Zero, out pixels, out pitch);
                Marshal.Copy(pixelBuffer, 0, pixels, pixelBuffer.Length);
                SDL.SDL_UnlockTexture(texture);

                SDL.SDL_RenderClear(renderer);
                SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero);
                SDL.SDL_RenderPresent(renderer);

                SDL.SDL_Delay(16);
            }

            handle.Free();
        }

        static void DrawDate(SKCanvas canvas)
        {
            if (string.IsNullOrEmpty(currentPath))
                return;

            var name = Path.GetFileNameWithoutExtension(currentPath);
            var date = name.Split('_')[0];

            float margin = 40;

            using var stroke = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 60,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 6
            };

            using var fill = new SKPaint
            {
                Color = SKColors.White,
                TextSize = 60,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            var bounds = new SKRect();
            fill.MeasureText(date, ref bounds);

            float x = width - bounds.Width - margin;
            float y = height - margin;

            canvas.DrawText(date, x, y, stroke);
            canvas.DrawText(date, x, y, fill);
        }
    }
}