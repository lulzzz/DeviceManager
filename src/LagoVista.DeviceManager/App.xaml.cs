//#define ENV_LOCALDEV
//#define ENV_LOCALDEV
//#define ENV_LOCALDEV
#define ENV_MASTER


using LagoVista.Client.Core;
using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Interfaces;
using LagoVista.Core.IOC;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.ViewModels;
using LagoVista.DeviceManager.Core.ViewModels;
using LagoVista.XPlat.Core.Services;
using LagoVista.XPlat.Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using LagoVista.Core.Models;

namespace LagoVista.DeviceManager
{
    public partial class App : Application
    {
        static App _instance;

        AppConfig _appConfig;

        public App()
        {
            InitializeComponent();

            _instance = this;

            InitServices();
        }

        private void InitServices()
        {
#if ENV_STAGE
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "api.nuviot.com",
            };
#elif ENV_DEV
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "dev-api.nuviot.com",
            };
#elif ENV_LOCALDEV
            var serverInfo = new ServerInfo()
            {
                SSL = false,
                RootUrl = "localhost:5001",
            };
#elif ENV_MASTER
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "api.nuviot.com",
            };
#endif

            var clientAppInfo = new ClientAppInfo();

            _appConfig = new AppConfig();
            SLWIOC.RegisterSingleton<IClientAppInfo>(clientAppInfo);
            SLWIOC.RegisterSingleton<IAppConfig>(_appConfig);

            var navigation = new ViewModelNavigation(this);
            LagoVista.XPlat.Core.Startup.Init(this, navigation);
            LagoVista.Client.Core.Startup.Init(serverInfo);

            navigation.Add<MainViewModel, Views.MainView>();
            navigation.Add<DeviceExplorerViewModel, Views.DeviceExplorerView>();
            navigation.Add<ProvisionDeviceViewModel, Views.ProvisionDeviceView>();
            navigation.Add<DeviceTypePickerViewModel, Views.DeviceTypePickerView>();
            navigation.Add<MonitorDeviceViewModel, Views.MonitorDeviceView>();
            navigation.Add<ManageDeviceViewModel, Views.ManageDeviceView>();

            navigation.Add<SplashViewModel, Views.SplashView>();

            navigation.Start<SplashViewModel>();

            SLWIOC.RegisterSingleton<IViewModelNavigation>(navigation);
        }

        public static App Instance { get { return _instance; } }

        public void HandleURIActivation(Uri uri)
        {
            var logger = SLWIOC.Get<ILogger>();
            if (this.MainPage == null)
            {
                logger.AddCustomEvent(LogLevel.Error, "App_HandleURIActivation", "Main Page Null");
            }
            else
            {
                var page = this.MainPage as LagoVistaNavigationPage;
                if (page != null)
                {
                    page.HandleURIActivation(uri);
                }
                else
                {

                    logger.AddCustomEvent(LogLevel.Error, "App_HandleURIActivation", "InvalidPageType - Not LagoVistaNavigationPage", new System.Collections.Generic.KeyValuePair<string, string>("type", this.MainPage.GetType().Name));
                }
            }
        }

        public void SetVersionInfo(VersionInfo versionInfo)
        {
            _appConfig.Version = versionInfo;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
