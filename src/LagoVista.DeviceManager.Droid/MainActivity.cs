﻿using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using LagoVista.Core.Models;

namespace LagoVista.DeviceManager.Droid
{
    [Activity(Label = "IoT Device Manager", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public const string MOBILE_CENTER_KEY = "5bbcacd9-cd4c-4f01-8715-9e9edf743ea0";

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            /* https://play.google.com/apps/publish/?dev_acc=12258406958683843289 */
            LagoVista.XPlat.Droid.Startup.Init(BaseContext, MOBILE_CENTER_KEY);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            var app = new App();

            var packageInfo = PackageManager.GetPackageInfo(PackageName, 0);

            var versionParts = packageInfo.VersionName.Split('.');
            var versionInfo = new VersionInfo();
            if (versionParts.Length != 4)
            {
                throw new Exception("Expecting android:versionName in AndroidManifest.xml to be a version conisting of four parts 1.0.218.1231 [Major].[Minor].[Build].[Revision]");
            }

            /* if this blows up our versionName in AndroidManaifest.xml is borked...make sure all version numbers are intergers like 1.0.218.1231 */
            versionInfo.Major = Convert.ToInt32(versionParts[0]);
            versionInfo.Minor = Convert.ToInt32(versionParts[1]);
            versionInfo.Build = Convert.ToInt32(versionParts[2]);
            versionInfo.Revision = Convert.ToInt32(versionParts[3]);
            app.SetVersionInfo(versionInfo);

            LoadApplication(app);
        }
    }
}

