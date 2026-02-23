using static SDL2.SDL;
using static SDL2.SDL_image;
using System.Runtime.InteropServices;

namespace BH_PhotoFrame
{
    public class PhotoSlideshow
    {
        private string[] imageFiles;
        private int delayMs;

        public PhotoSlideshow(string folderPath, int delayMs)
        {
            this.delayMs = delayMs;
            imageFiles = Directory.GetFiles(folderPath, "*.*")
                                  .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png"))
                                  .OrderBy(_ => Guid.NewGuid()) // 랜덤 정렬
                                  .ToArray();
        }

        public void Run()
        {
            SDL_Init(SDL_INIT_VIDEO);
            IMG_Init(IMG_InitFlags.IMG_INIT_JPG | IMG_InitFlags.IMG_INIT_PNG);

            IntPtr window = SDL_CreateWindow("Frame", 0, 0, 800, 480, SDL_WindowFlags.SDL_WINDOW_FULLSCREEN);
            IntPtr renderer = SDL_CreateRenderer(window, -1, 0);

            int index = 0;
            bool running = true;

            while (running)
            {
                var texture = LoadTexture(renderer, imageFiles[index]);
                SDL_RenderClear(renderer);
                SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero);
                SDL_RenderPresent(renderer);
                SDL_DestroyTexture(texture);

                uint start = SDL_GetTicks();
                while (SDL_GetTicks() - start < delayMs)
                {
                    SDL_PollEvent(out SDL_Event e);
                    if (e.type == SDL_EventType.SDL_QUIT)
                        running = false;
                    else if (e.type == SDL_EventType.SDL_KEYDOWN)
                    {
                        switch (e.key.keysym.sym)
                        {
                            case SDL_Keycode.SDLK_q: running = false; break;
                            case SDL_Keycode.SDLK_RIGHT: index = (index + 1) % imageFiles.Length; return;
                            case SDL_Keycode.SDLK_LEFT: index = (index - 1 + imageFiles.Length) % imageFiles.Length; return;
                        }
                    }
                    SDL_Delay(10);
                }

                index = (index + 1) % imageFiles.Length;
            }

            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);
            IMG_Quit();
            SDL_Quit();
        }

        private IntPtr LoadTexture(IntPtr renderer, string path)
        {
            IntPtr surface = IMG_Load(path);
            if (surface == IntPtr.Zero)
                throw new Exception($"Could not load image: {path}");

            IntPtr texture = SDL_CreateTextureFromSurface(renderer, surface);
            SDL_FreeSurface(surface);
            return texture;
        }
    }
}