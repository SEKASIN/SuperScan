using System;
using Exiled.API.Features;

namespace SuperScan.com.github.sekasin.superscan {
    public class SuperScan : Plugin<Config> {
        public override string Name => "SuperScan";
        public override string Author => "TenDRILLL";
        public override Version Version => new Version(1, 2, 0);
        public EventHandler EventHandler;

        public override void OnEnabled() {
            Log.Info("SuperScan loading...");
            if (!Config.IsEnabled) {
                Log.Warn("SuperScan disabled from config, unloading...");
                OnDisabled();
                return;
            }
            EventHandler = new EventHandler(this);
            Log.Info("SuperScan loaded.");
        }

        public override void OnDisabled()
        {
            EventHandler.UnregisterEvents();
            Log.Info("SuperScan unloaded.");
        }
    }
}