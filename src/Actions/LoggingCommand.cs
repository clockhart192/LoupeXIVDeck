namespace Loupedeck.LoupeXIVDeck
{
    using System;

    using static Loupedeck.LoupeXIVDeck.FFXIVGameTypes;


    public class Logging : PluginDynamicCommand
    {
        private IFFXIVPluginLink _pluginLink;
        private IFFXIVApi _api;
        private IDisposable isApplicationReadySubscription;
        private Boolean isApplicationReady;

        // Initializes the command class.
        private Logging()
            : base("Logging", "Logging", "FFXIV Commands") => this.ActionImageChanged();



        // This method is called when Loupedeck needs to show the command on the console or the UI.
        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) =>
            $"Logging";

        protected override Boolean OnLoad()
        {
            this.GetInstances();

            this.isApplicationReadySubscription = this._pluginLink.GetIsApplicationReadySubject()
                .Subscribe(ready => {
                    this.isApplicationReady = ready;
                    this.ActionImageChanged();
                });

            return true;
        }

        protected override async void RunCommand(String actionParameter)
        {
            if (this.isApplicationReady)
            {
                var classes =  await this._api.GetClasses();
                var ClassString = JsonHelpers.SerializeAnyObject(classes);

                PluginLog.Info(ClassString);
            }
        }

        protected override Boolean OnUnload()
        {
            this.isApplicationReadySubscription.Dispose();

            return base.OnUnload();
        }

        private void GetInstances()
        {
            var plugin = (LoupeXIVDeckPlugin)base.Plugin;
            var container = plugin.container;

            this._pluginLink = container.GetInstance<IFFXIVPluginLink>();
            this._api = container.GetInstance<IFFXIVApi>();
        }
    }
}
