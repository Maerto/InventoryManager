using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.IoC;
using System;
using System.IO;
using System.Reflection;
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Buddy;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Fates;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.FlyText;
using Dalamud.Game.Gui.PartyFinder;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Libc;
using Dalamud.Game.Network;
using Dalamud.Game.Text.SeStringHandling;

namespace invManager
{
    public class invManager : IDalamudPlugin
    {
        public string Name => "Inventory Manager";

        private const string CommandName = "/inventory";

        public static DalamudPluginInterface PluginInterface { get; private set; }
        public static CommandManager CommandManager { get; private set; }
        public static SigScanner SigScanner { get; private set; }
        public static DataManager DataManager { get; private set; }
        public static ClientState ClientState { get; private set; }
        public static ChatGui ChatGui { get; private set; }
        public static ChatHandlers ChatHandlers { get; private set; }
        public static Framework Framework { get; private set; }
        public static GameNetwork GameNetwork { get; private set; }
        public static Condition Condition { get; private set; }
        public static KeyState KeyState { get; private set; }
        public static GameGui GameGui { get; private set; }
        public static FlyTextGui FlyTextGui { get; private set; }
        public static ToastGui ToastGui { get; private set; }
        public static JobGauges JobGauges { get; private set; }
        public static PartyFinderGui PartyFinderGui { get; private set; }
        public static BuddyList BuddyList { get; private set; }
        public static PartyList PartyList { get; private set; }
        public static TargetManager TargetManager { get; private set; }
        public static ObjectTable ObjectTable { get; private set; }
        public static FateTable FateTable { get; private set; }
        public static LibcFunction LibcFunction { get; private set; }

        private Configuration Configuration { get; init; }
        private invManagerUI UI { get; init; }

        public invManager(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager,
            [RequiredVersion("1.0")] SigScanner sigScanner,
            [RequiredVersion("1.0")] DataManager dataManager,
            [RequiredVersion("1.0")] ClientState clientState,
            [RequiredVersion("1.0")] ChatGui chatGui,
            [RequiredVersion("1.0")] ChatHandlers chatHandlers,
            [RequiredVersion("1.0")] Framework framework,
            [RequiredVersion("1.0")] GameNetwork gameNetwork,
            [RequiredVersion("1.0")] Condition condition,
            [RequiredVersion("1.0")] KeyState keyState,
            [RequiredVersion("1.0")] GameGui gameGui,
            [RequiredVersion("1.0")] FlyTextGui flyTextGui,
            [RequiredVersion("1.0")] ToastGui toastGui,
            [RequiredVersion("1.0")] JobGauges jobGauges,
            [RequiredVersion("1.0")] PartyFinderGui partyFinderGui,
            [RequiredVersion("1.0")] BuddyList buddyList,
            [RequiredVersion("1.0")] PartyList partyList,
            [RequiredVersion("1.0")] TargetManager targetManager,
            [RequiredVersion("1.0")] ObjectTable objectTable,
            [RequiredVersion("1.0")] FateTable fateTable,
            [RequiredVersion("1.0")] LibcFunction libcFunction)
        {
            PluginInterface = pluginInterface;
            CommandManager = commandManager;
            SigScanner = sigScanner;
            DataManager = dataManager;
            ClientState = clientState;
            ChatGui = chatGui;
            ChatHandlers = chatHandlers;
            Framework = framework;
            GameNetwork = gameNetwork;
            Condition = condition;
            KeyState = keyState;
            GameGui = gameGui;
            FlyTextGui = flyTextGui;
            ToastGui = toastGui;
            JobGauges = jobGauges;
            PartyFinderGui = partyFinderGui;
            BuddyList = buddyList;
            PartyList = partyList;
            TargetManager = targetManager;
            ObjectTable = objectTable;
            FateTable = fateTable;
            LibcFunction = libcFunction;

            this.Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(PluginInterface);
            
            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"goat.png");
            var goatImage = PluginInterface.UiBuilder.LoadImage(imagePath);
            this.UI = new invManagerUI(this.Configuration, goatImage);
            
            CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Displays inventory tools"
            });

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.UI.Dispose();

            CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            this.UI.Visible = true;
        }

        private void DrawUI()
        {
            this.UI.Draw();
        }

        private void DrawConfigUI()
        {
            this.UI.SettingsVisible = true;
        }
    }
}
