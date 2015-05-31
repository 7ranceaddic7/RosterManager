using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using KSP.IO;

namespace RosterManager
{
    internal  static class RMSettings
    {
        #region Properties

        internal static bool Loaded = false;

        internal static ConfigNode settings = null;
        private static readonly string SETTINGS_PATH = KSPUtil.ApplicationRootPath + "GameData/RosterManager/Plugins/PluginData";
        private static readonly string SETTINGS_FILE = SETTINGS_PATH + "/RMSettings.dat";

        // This value is assigned from AssemblyInfo.cs
        internal static string CurVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        // Persisted properties
        // Realism Options
        internal static bool RealismMode = true;
        internal static bool LockSettings = false;

        //Highlighting Options
        internal static bool EnableHighlighting = true;
        internal static string SourcePartColor = "red";
        internal static string TargetPartColor = "green";

        // Tooltip Options
        internal static bool ShowToolTips = true;


        //Configuration options
        internal static bool EnableBlizzyToolbar = false; // off by default
        internal static bool VerboseLogging = false;
        internal static string ErrorLogLength = "1000";
        internal static bool EnableKerbalRename = false;
        internal static bool RenameWithProfession = false;
        internal static bool AutoDebug = false;
        internal static bool SaveLogOnExit = false;

        // Non user managed Internal options
        internal static Color defaultColor = new Color(0.478f, 0.698f, 0.478f, 0.698f);
        internal static string DebugLogPath = "\\Plugins\\PluginData\\";
        // End Persisted Properties

        // Settings Window Option undo properties (Cancel button support)

        // Realism settings
        internal static bool prevRealismMode = false;
        internal static bool prevLockSettings = false;

        // Highlighting Settings
        internal static bool prevEnableHighlighting = true;

        // Tooltip Settings
        internal static bool prevShowToolTips = true;
        internal static bool prevSettingsToolTips = true;
        internal static bool prevRosterToolTips = true;
        internal static bool prevDebuggerToolTips = true;

        // Configuration Settings
        internal static bool prevEnableBlizzyToolbar = false;
        internal static bool prevShowDebugger = false;
        internal static bool prevVerboseLogging = false;
        internal static string prevErrorLogLength = "1000";
        internal static bool prevSaveLogOnExit = true;
        internal static bool prevEnableKerbalRename = false;
        internal static bool prevRenameWithProfession = false;


        // Internal properties for plugin management.  Not persisted, not user managed.
        internal static Dictionary<string, Color> Colors;

        #endregion

        #region Methods

        internal static void LoadColors()
        {
            Colors = new Dictionary<string, Color>();

            Colors.Add("black", Color.black);
            Colors.Add("blue", Color.blue);
            Colors.Add("clea", Color.clear);
            Colors.Add("cyan", Color.cyan);
            Colors.Add("gray", Color.gray);
            Colors.Add("green", Color.green);
            Colors.Add("magenta", Color.magenta);
            Colors.Add("red", Color.red);
            Colors.Add("white", Color.white);
            Colors.Add("yellow", Color.yellow);
        }

        internal static void StoreTempSettings()
        {
            prevRealismMode = RealismMode;
            prevShowDebugger = WindowDebugger.ShowWindow;
            prevVerboseLogging = VerboseLogging;
            prevEnableHighlighting = EnableHighlighting;
            prevEnableKerbalRename = EnableKerbalRename;
            prevRenameWithProfession = RenameWithProfession;
            prevLockSettings = LockSettings;
            prevEnableBlizzyToolbar = EnableBlizzyToolbar;
            prevSaveLogOnExit = SaveLogOnExit;
            prevShowToolTips = ShowToolTips;
            prevSettingsToolTips = WindowSettings.ShowToolTips;
            prevRosterToolTips = WindowRoster.ShowToolTips;
            prevDebuggerToolTips = WindowDebugger.ShowToolTips;

            // sounds

            //debugger Settings
            prevErrorLogLength = ErrorLogLength;
        }

        internal static void RestoreTempSettings()
        {
            RealismMode = prevRealismMode;
            WindowDebugger.ShowWindow = prevShowDebugger;
            VerboseLogging = prevVerboseLogging;
            EnableHighlighting = prevEnableHighlighting;
            EnableKerbalRename = prevEnableKerbalRename;
            RenameWithProfession = prevRenameWithProfession;
            LockSettings = prevLockSettings;
            EnableBlizzyToolbar = prevEnableBlizzyToolbar;
            SaveLogOnExit = prevSaveLogOnExit;
            ShowToolTips = prevShowToolTips;
            WindowSettings.ShowToolTips = prevSettingsToolTips;
            WindowRoster.ShowToolTips = prevRosterToolTips;
            WindowDebugger.ShowToolTips = prevDebuggerToolTips;

            //debugger Settings
            prevErrorLogLength = ErrorLogLength;
        }

        internal static ConfigNode loadSettingsFile()
        {
            if (settings == null)
                settings = ConfigNode.Load(SETTINGS_FILE) ?? new ConfigNode();
            return settings;
        }

        internal static void ApplySettings()
        {
            LoadColors();

            if (settings == null)
                loadSettingsFile();
            ConfigNode WindowsNode = settings.HasNode("RM_Windows") ? settings.GetNode("RM_Windows") : settings.AddNode("RM_Windows");
            ConfigNode SettingsNode = settings.HasNode("RM_Settings") ? settings.GetNode("RM_Settings") : settings.AddNode("RM_Settings");
            ConfigNode HiddenNode = settings.HasNode("RM_Hidden") ? settings.GetNode("RM_Hidden") : settings.AddNode("RM_Hidden");

            // Lets get our window rectangles...
            WindowDebugger.Position = getRectangle(WindowsNode, "DebuggerPosition", WindowDebugger.Position);
            WindowSettings.Position = getRectangle(WindowsNode, "SettingsPosition", WindowSettings.Position);
            WindowRoster.Position = getRectangle(WindowsNode, "RosterPosition", WindowRoster.Position);

            // now the settings

            // Realism Settings
            RealismMode = SettingsNode.HasValue("RealismMode") ? bool.Parse(SettingsNode.GetValue("RealismMode")) : RealismMode;
            LockSettings = SettingsNode.HasValue("LockSettings") ? bool.Parse(SettingsNode.GetValue("LockSettings")) : LockSettings;

            // Highlighting settings
            EnableHighlighting = SettingsNode.HasValue("EnableHighlighting") ? bool.Parse(SettingsNode.GetValue("EnableHighlighting")) : EnableHighlighting;

            // ToolTip Settings
            ShowToolTips = SettingsNode.HasValue("ShowToolTips") ? bool.Parse(SettingsNode.GetValue("ShowToolTips")) : ShowToolTips;
            WindowSettings.ShowToolTips = SettingsNode.HasValue("SettingsToolTips") ? bool.Parse(SettingsNode.GetValue("SettingsToolTips")) : WindowSettings.ShowToolTips;
            WindowRoster.ShowToolTips = SettingsNode.HasValue("RosterToolTips") ? bool.Parse(SettingsNode.GetValue("RosterToolTips")) : WindowRoster.ShowToolTips;
            WindowDebugger.ShowToolTips = SettingsNode.HasValue("DebuggerToolTips") ? bool.Parse(SettingsNode.GetValue("DebuggerToolTips")) : WindowDebugger.ShowToolTips;

            // Config Settings
            EnableBlizzyToolbar = SettingsNode.HasValue("EnableBlizzyToolbar") ? bool.Parse(SettingsNode.GetValue("EnableBlizzyToolbar")) : EnableBlizzyToolbar;
            WindowDebugger.ShowWindow = SettingsNode.HasValue("ShowDebugger") ? bool.Parse(SettingsNode.GetValue("ShowDebugger")) : WindowDebugger.ShowWindow;
            VerboseLogging = SettingsNode.HasValue("VerboseLogging") ? bool.Parse(SettingsNode.GetValue("VerboseLogging")) : VerboseLogging;
            AutoDebug = SettingsNode.HasValue("AutoDebug") ? bool.Parse(SettingsNode.GetValue("AutoDebug")) : AutoDebug;
            DebugLogPath = SettingsNode.HasValue("DebugLogPath") ? SettingsNode.GetValue("DebugLogPath") : DebugLogPath;
            ErrorLogLength = SettingsNode.HasValue("ErrorLogLength") ? SettingsNode.GetValue("ErrorLogLength") : ErrorLogLength;
            SaveLogOnExit = SettingsNode.HasValue("SaveLogOnExit") ? bool.Parse(SettingsNode.GetValue("SaveLogOnExit")) : SaveLogOnExit;
            EnableKerbalRename = SettingsNode.HasValue("EnableKerbalRename") ? bool.Parse(SettingsNode.GetValue("EnableKerbalRename")) : EnableKerbalRename;
            RenameWithProfession = SettingsNode.HasValue("RenameWithProfession") ? bool.Parse(SettingsNode.GetValue("RenameWithProfession")) : RenameWithProfession;


            // Hidden Settings
            // Hidden Highlighting
            SourcePartColor = HiddenNode.HasValue("SourcePartColor") ? HiddenNode.GetValue("SourcePartColor") : SourcePartColor;
            TargetPartColor = HiddenNode.HasValue("TargetPartColor") ? HiddenNode.GetValue("TargetPartColor") : TargetPartColor;
            //Hidden sound

            // Okay, set the Settings loaded flag
            Loaded = true;

            // Lets make sure that the windows can be seen on the screen. (supports different resolutions)
            RepositionWindows();

        }

        internal static void SaveSettings()
        {
            if (settings == null)
                settings = loadSettingsFile();

            ConfigNode WindowsNode = settings.HasNode("RM_Windows") ? settings.GetNode("RM_Windows") : settings.AddNode("RM_Windows");
            ConfigNode SettingsNode = settings.HasNode("RM_Settings") ? settings.GetNode("RM_Settings") : settings.AddNode("RM_Settings");
            ConfigNode HiddenNode = settings.HasNode("RM_Hidden") ? settings.GetNode("RM_Hidden") : settings.AddNode("RM_Hidden");

            // Write window positions
            WriteRectangle(WindowsNode, "DebuggerPosition", WindowDebugger.Position);
            WriteRectangle(WindowsNode, "SettingsPosition", WindowSettings.Position);
            WriteRectangle(WindowsNode, "RosterPosition", WindowRoster.Position);

            //Write settings...
            // Realism Settings
            WriteValue(SettingsNode, "LockSettings", LockSettings);

            // Highlighting Settings
            WriteValue(SettingsNode, "EnableHighlighting", EnableHighlighting);

            // ToolTip Settings
            WriteValue(SettingsNode, "ShowToolTips", ShowToolTips);
            WriteValue(SettingsNode, "SettingsToolTips", WindowSettings.ShowToolTips);
            WriteValue(SettingsNode, "RosterToolTips", WindowRoster.ShowToolTips);
            WriteValue(SettingsNode, "DebuggerToolTips", WindowDebugger.ShowToolTips);

            // Config Settings
            WriteValue(SettingsNode, "ShowDebugger", WindowDebugger.ShowWindow);
            WriteValue(SettingsNode, "EnableBlizzyToolbar", EnableBlizzyToolbar);
            WriteValue(SettingsNode, "VerboseLogging", VerboseLogging);
            WriteValue(SettingsNode, "AutoDebug", AutoDebug);
            WriteValue(SettingsNode, "DebugLogPath", DebugLogPath);
            WriteValue(SettingsNode, "ErrorLogLength", ErrorLogLength);
            WriteValue(SettingsNode, "SaveLogOnExit", SaveLogOnExit);
            WriteValue(SettingsNode, "EnableKerbalRename", EnableKerbalRename);
            WriteValue(SettingsNode, "RenameWithProfession", RenameWithProfession);

            // Hidden Settings
            WriteValue(HiddenNode, "SourcePartColor", SourcePartColor);
            WriteValue(HiddenNode, "TargetPartColor", TargetPartColor);

            if (!Directory.Exists(SETTINGS_PATH))
                Directory.CreateDirectory(SETTINGS_PATH);
            settings.Save(SETTINGS_FILE);
        }

        private static Rect getRectangle(ConfigNode WindowsNode, string RectName, Rect defaultvalue)
        {
            Rect thisRect = new Rect();
            ConfigNode RectNode = WindowsNode.HasNode(RectName) ? WindowsNode.GetNode(RectName) : WindowsNode.AddNode(RectName);
            thisRect.x = RectNode.HasValue("x") ? int.Parse(RectNode.GetValue("x")) : defaultvalue.x;
            thisRect.y = RectNode.HasValue("y") ? int.Parse(RectNode.GetValue("y")) : defaultvalue.y;
            thisRect.width = RectNode.HasValue("width") ? int.Parse(RectNode.GetValue("width")) : defaultvalue.width;
            thisRect.height = RectNode.HasValue("height") ? int.Parse(RectNode.GetValue("height")) : defaultvalue.height;

            return thisRect;
        }

        private static void WriteRectangle(ConfigNode WindowsNode, string RectName, Rect rectValue)
        {
            ConfigNode RectNode = WindowsNode.HasNode(RectName) ? WindowsNode.GetNode(RectName) : WindowsNode.AddNode(RectName);
            WriteValue(RectNode, "x", rectValue.x);
            WriteValue(RectNode, "y", rectValue.y);
            WriteValue(RectNode, "width", rectValue.width);
            WriteValue(RectNode, "height", rectValue.height);
        }

        private static void WriteValue(ConfigNode configNode, string ValueName, object value)
        {
            if (configNode.HasValue(ValueName))
                configNode.RemoveValue(ValueName);
            configNode.AddValue(ValueName, value.ToString());
        }

        private static void RepositionWindows()
        {
            if (WindowDebugger.Position.xMax > Screen.currentResolution.width)
                WindowDebugger.Position.x = Screen.currentResolution.width - WindowDebugger.Position.width;
            if (WindowDebugger.Position.yMax > Screen.currentResolution.height)
                WindowDebugger.Position.y = Screen.currentResolution.height - WindowDebugger.Position.height;

            if (WindowSettings.Position.xMax > Screen.currentResolution.width)
                WindowSettings.Position.x = Screen.currentResolution.width - WindowSettings.Position.width;
            if (WindowSettings.Position.yMax > Screen.currentResolution.height)
                WindowSettings.Position.y = Screen.currentResolution.height - WindowSettings.Position.height;

            if (WindowRoster.Position.xMax > Screen.currentResolution.width)
                WindowRoster.Position.x = Screen.currentResolution.width - WindowRoster.Position.width;
            if (WindowRoster.Position.yMax > Screen.currentResolution.height)
                WindowRoster.Position.y = Screen.currentResolution.height - WindowRoster.Position.height;
        }

        #endregion
    }
}