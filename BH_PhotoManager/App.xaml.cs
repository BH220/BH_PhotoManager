
using BH_PhotoManager.Common.Navigation;
using BH_PhotoManager.Data.Database;
using BH_PhotoManager.ViewModels;
using BH_PhotoManager.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32.SafeHandles;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Extensions;

namespace BH_PhotoManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean AllocConsole();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_MINIMIZE = 6;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        uint lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        uint hTemplateFile);

        private const int MY_CODE_PAGE = 949;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_WRITE = 0x2;
        private const uint OPEN_EXISTING = 0x3;

        // 강제로 App 타입으로 Type Casting, 현재 실행중인 App 객체 접근
        public new static App? Current => Application.Current as App;

        public IServiceProvider? Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // 1. 기본 WPF 어플리케이션 초기화
            base.OnStartup(e);

            // 2. 어플리케이션 종료 모드 설정
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            SelfElevatedProcess();

            // 4. 시스템 환경 설정
            ShowConsoleWindow(); 

            // DI 컨테이너 구성
            Services = ConfigureServices(); 

            ConfigurationSettings(); 

            var updateManager = Services.GetRequiredService<UpdateSchema>();
            updateManager.DoSchemaUpdate();

            //var scope = Services.GetRequiredService<WindowServiceScope>();
            //var provider = scope.CreateScopedProvider();

            var viewModel = Services.GetRequiredService<MainViewModel>();
            viewModel.Title = "BH Photo Manager";
            var view = Services.GetRequiredService<MainView>();
            view.DataContext = viewModel;

            view.Show();
        }

        public static void ConfigurationSettings()
        {
            // 관리자 설정 Initialize
            //ConfigManager<AdminSettings>.Initialize(ConfigPaths.GetPathFor<AdminSettings>());
            //  사용자 설정 Initialize
            //ConfigManager<UserSettings>.Initialize(ConfigPaths.GetPathFor<UserSettings>());
            // 프로젝트 설정 Initialize -> 프로젝트 설정은 따로 초기화 할 필요 없이 사용하는곳에서 부르면 알아서 초기화 후 사용됨
        }

        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMessenger>(new WeakReferenceMessenger());
            services.AddSingleton<INavigationViewPageProvider, NavigationViewPageProvider>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ISnackbarService, SnackbarService>();
            services.AddSingleton<IContentDialogService, ContentDialogService>();


            services.AddSingleton<UpdateSchema>();
            services.AddSingleton<MainView>();
            services.AddSingleton<MainViewModel>();
            ////services.AddSingleton<ImageManager>();
            //services.AddSingleton<MainViewFactory>();

            ////Data관련 서비스
            //services.AddSingleton<HistoryRepository>();
            //services.AddSingleton<StatisticsRepository>();
            //services.AddSingleton<SettingRepository>();
            //services.AddSingleton<ITimeService, TimeService>();

            //services.AddSingleton<IItemNameProvider, ItemNameProvider>();

            return services.BuildServiceProvider();
        }

        private void SelfElevatedProcess()
        {
            Console.WriteLine("관리자 권한으로 실행준비...");

            if (!IsRunAsAdmin())
            {
                Console.WriteLine("관리자 권한이 없으므로 관리자 권한으로 실행");

                ProcessStartInfo proc = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = Environment.ProcessPath,
                    Verb = "runas"
                };

                Console.WriteLine("관리자권한으로 실행 -- 관리자권한으로 실행할 프로세스 : " + proc.FileName);

                try
                {
                    Process.Start(proc);
                    Console.WriteLine("관리자 권한으로 실행...");

                    Application.Current.Shutdown();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("관리자 실행 실패: " + ex.Message);
                }
            }
        }
        private static bool IsRunAsAdmin()
        {
            bool isAdmin = false;
            try
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(id);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
            }
            return isAdmin;
        }

        private void ShowConsoleWindow()
        {
            bool OpenConsole = false;
#if DEBUG
            OpenConsole = true;
#else
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "CTest.dat"))
                OpenConsole = true;
#endif
            if (OpenConsole)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                if (!AllocConsole())
                    MessageBox.Show("Console Window Load Failed");
                else
                {
                    IntPtr stdHandle = CreateFile("CONOUT$", GENERIC_WRITE, FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0);
                    SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
                    FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                    Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
                    StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
                    standardOutput.AutoFlush = true;
                    Console.SetOut(standardOutput);
                    Console.WriteLine("This will show up in the Console window.");
                }
            }
        }
    }
}
