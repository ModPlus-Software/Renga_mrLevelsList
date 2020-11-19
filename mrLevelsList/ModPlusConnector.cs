namespace mrLevelsList
{
    using System.Collections.Generic;
    using ModPlusAPI.Abstractions;
    using ModPlusAPI.Enums;

    /// <inheritdoc/>
    public class ModPlusConnector : IModPlusPluginForRenga
    {
        private static ModPlusConnector _instance;

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static ModPlusConnector Instance => _instance ?? (_instance = new ModPlusConnector());

        /// <inheritdoc/>
        public SupportedProduct SupportedProduct => SupportedProduct.Renga;

        /// <inheritdoc/>
        public string Name => "mrLevelsList";

        /// <inheritdoc/>
        public string LName => "Уровни";

        /// <inheritdoc/>
        public string Description => "Отображение списка уровней с возможностью управления их видимостью";

        /// <inheritdoc/>
        public string Author => "Пекшев Александр aka Modis";

        /// <inheritdoc/>
        public string Price => "0";

        /// <inheritdoc/>
        public string FullDescription => string.Empty;

        /// <inheritdoc/>
        public RengaFunctionUILocation UiLocation => RengaFunctionUILocation.PrimaryPanel;

        /// <inheritdoc/>
        public RengaContextMenuShowCase ContextMenuShowCase => RengaContextMenuShowCase.Scene;

        /// <inheritdoc/>
        public List<RengaViewType> ViewTypes => new List<RengaViewType>();

        /// <inheritdoc/>
        public bool IsAddingToMenuBySelf => false;
    }
}
