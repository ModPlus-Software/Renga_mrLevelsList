namespace mrLevelsList
{
    using System;
    using ModPlus;
    using ModPlusAPI.Windows;
    using View;
    using ViewModels;

    /// <inheritdoc />
    public class PluginStarter : IRengaFunction
    {
        /// <inheritdoc />
        public void Start()
        {
            if (MainWindow.IsOpen)
                return;
#if !DEBUG
            ModPlusAPI.Statistic.SendCommandStarting(ModPlusConnector.Instance);
#endif
            try
            {
                var mainViewModel = new MainViewModel();
                var mainWindow = new MainWindow { DataContext = mainViewModel };
                mainWindow.ContentRendered += (sender, args) => mainViewModel.GetLevels();
                mainWindow.Show();
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        }
    }
}
