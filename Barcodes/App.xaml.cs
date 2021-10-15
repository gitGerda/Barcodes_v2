//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Euro Plus">
//     Copyright © Euro Plus 2014.
// </copyright>
// <summary>This is the App class.</summary>
//-----------------------------------------------------------------------

namespace SDK.DemoApp
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using NiceLabel.SDK;
    /// <summary>
    /// Interaction logic for App.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            this.InitializePrintEngine();

            base.OnStartup(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            PrintEngineFactory.PrintEngine.Shutdown();

            base.OnExit(e);
        }

        /// <summary>
        /// Initializes the Print engine.
        /// </summary>
        private void InitializePrintEngine()
        {
            try
            {
                string sdkFilesPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\SDKFiles");
                if (Directory.Exists(sdkFilesPath))
                {
                    PrintEngineFactory.SDKFilesPath = sdkFilesPath;
                }

                PrintEngineFactory.PrintEngine.Initialize();
            }
            catch (NiceLabel.SDK.SDKException exception)
            {
                MessageBox.Show("Initialization of the SDK failed." + Environment.NewLine + Environment.NewLine + exception.ToString());
                this.Shutdown();
            }
        }
    }
}