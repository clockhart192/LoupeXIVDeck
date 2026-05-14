namespace Loupedeck.LoupeXIVDeck
{
    using System;

    using StructureMap;

    // This class contains the plugin-level logic of the Loupedeck plugin.

    public class LoupeXIVDeckPlugin : Plugin
    {
        // Gets a value indicating whether this is an API-only plugin.
        public override Boolean UsesApplicationApiOnly => true;

        // Gets a value indicating whether this is a Universal plugin or an Application plugin.
        public override Boolean HasNoApplication => true;
        private IDisposable isApplicationReadySubscription;
        public Container container;

        // Initializes a new instance of the plugin class.
        public LoupeXIVDeckPlugin()
        {
            // Initialize the plugin log.
            PluginLog.Init(this.Log);

            // Initialize the plugin resources.
            PluginResources.Init(this.Assembly);

            this.SetupDiContainer();
        }

        // This method is called when the plugin is loaded.
        public override void Load() => this.SetupFFXIVConnection();

        private void SetupDiContainer() => this.container = new Container(new FFXIVRegistry());

        private void SetupFFXIVConnection()
        {
            var pluginLink = this.container.GetInstance<IFFXIVPluginLink>();

            this.isApplicationReadySubscription = pluginLink.GetIsApplicationReadySubject().Subscribe(isReady =>
            {
                if (isReady)
                {
                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Normal, null, null, null);
                }
                else
                {
                    this.OnPluginStatusChanged(Loupedeck.PluginStatus.Error,
                        Constants.NO_CONNECTION_ERROR_MESSAGE,
                        Constants.SUPPORT_URL,
                        Constants.SUPPORT_URL_TITLE);
                }
            });

            // Only connect after isApplicationReadySubject subscription is available
            pluginLink.Connect();
        }

        public override void Unload() => this.isApplicationReadySubscription.Dispose();
    }
}
