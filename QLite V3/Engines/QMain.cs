using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.ServiceProcess;
using System.Reflection;
using QLite.Servers;
using QLite.Plugins;
using QLite.QueryPlugin;
using QLite.Structures;

namespace QLite.Engines
{
    class QMain
    {
        #region Globals
        private static QDialog Dialogs = new QDialog();

        #endregion

        public static void SetDefaults()
        {
            Global.Defaults.XmlDefaults = new XmlDocument();
            Global.Defaults.XmlFileName = "QLite2.xml";
            Global.Defaults.XmlDefaults.Load(Global.Defaults.XmlFileName);
            Global.Defaults.Connection = new string[4];
            Global.Defaults.InvalidNameChars = new char[] { '~','`', '!', '@', '$', '%', '^', '*', '|', '{', '}', '[', ']', ':', ';', '?', '/', '.', ',', '\"', '\\', '\'', '<', '>' };

            XmlNode nodeOptions = QXML.PullOneDeep("Options");
            
            foreach(XmlNode option in nodeOptions.ChildNodes)
                if (option.Name.ToUpper() == "DEFAULTS")
                    foreach (XmlNode setting in option.ChildNodes)
                    {
                        XmlAttributeCollection attributes;
                        switch (setting.Name.ToUpper())
                        {
                            /*case "FONTS":
                                foreach (XmlNode font in setting.ChildNodes)
                                {
                                    attributes = font.Attributes;
                                    QliteFont tmpFont = new QliteFont();

                                    String fName = attributes[0].Value.ToString();
                                    Single fSize = Convert.ToSingle(attributes[1].Value);
                                    bool[] style = new bool[attributes[2].Value.Length];
                                    string[] fcolors = attributes[3].Value.Split(','), bcolors = attributes[4].Value.Split(',');
                                    Color fColor = Color.FromArgb(Convert.ToInt32(fcolors[0]), Convert.ToInt32(fcolors[1]), Convert.ToInt32(fcolors[2]), Convert.ToInt32(fcolors[3])),
                                          bColor = Color.FromArgb(Convert.ToInt32(bcolors[0]), Convert.ToInt32(bcolors[1]), Convert.ToInt32(bcolors[2]), Convert.ToInt32(bcolors[3]));

                                    for (int i = 0; i < attributes[2].Value.Length; i++)
                                        if (Convert.ToBoolean(Convert.ToInt32(attributes[2].Value.Substring(i, 1)))) style[i] = true;

                                    foreach(FontType ft in Enum.GetValues(typeof(FontType)))
                                        if (ft.ToString() == font.Name.ToString())
                                        { tmpFont.Type = ft; break; }

                                    tmpFont.QFont = new Font(fName, fSize);
                                    tmpFont.Colors = new HighlightColors(fColor, bColor);
                                    tmpFont.Style = style;
                                    Global.Defaults.Fonts.Add(tmpFont);
                                }

                                break;*/
                            case "CONNECTION":
                                foreach(XmlAttribute att in setting.Attributes)
                                    switch (att.Name.ToUpper())
                                    {
                                        case "INSTANCE":
                                            Global.Defaults.Connection[0] = att.Value;
                                            break;
                                        case "MACHINE":
                                            Global.Defaults.Connection[1] = att.Value;
                                            break;
                                        case "USER":
                                            Global.Defaults.Connection[2] = att.Value;
                                            break;
                                        case "PASSWORD":
                                            Global.Defaults.Connection[3] = att.Value;
                                            break;
                                    }
                                break;
                            case "PLUGPATH":
                                attributes = setting.Attributes;
                                if (attributes[0].Value != "") Global.Defaults.PluginPath = attributes[0].Value;
                                else Global.Defaults.PluginPath = Application.StartupPath;
                                break;
                        }
                }
        }

        public static void SetCommandLineArgs()
        {
            if (Environment.GetCommandLineArgs().Length > 1)
                foreach (string arg in Environment.GetCommandLineArgs())
                    if (arg.Substring(0, 1) == "/")
                        switch (arg.Substring(1, 1).ToUpper())
                        {
                            case "P":
                                if (arg.Substring(2, 1) != null)
                                    if (arg.Substring(2, 1).ToUpper() == "F")
                                        Global.Defaults.PasswordLock = false;
                                    else if (arg.Substring(2, 1).ToUpper() == "T")
                                        Global.Defaults.PasswordLock = true;
                                    else
                                        Console.WriteLine("Invalid passoword option. Please select either T for true or F for false");
                                break;
                            case "D":
                                if (isNumeric(arg.Substring(2,1), System.Globalization.NumberStyles.Integer))
                                    if (Convert.ToInt32(arg.Substring(2, 1)) >= 0 && Convert.ToInt32(arg.Substring(2, 1)) < 6)

                                        Global.Defaults.DebugLvl = Convert.ToInt32(arg.Substring(2, 1));
                                    else
                                        Console.WriteLine("Invalid Debug level. Please choose a value from 0 to 5. 1 Being lowest, 5 being highest.");
                                else
                                    Console.WriteLine("Invalid Debug value. Please choose an integer between 0 and 5.");

                                break;
                            case "?":
                                ReportValidCmdArgs();
                                break;
                            default:
                                ReportValidCmdArgs();
                                break;
                        }
                    else
                    { Console.WriteLine("No valid command line arguments detected."); ReportValidCmdArgs(); }
            else
            {
                Global.Defaults.DebugLvl = 0;
                Global.Defaults.PasswordLock = true;
            }
        }

        public static int StartProcess(TreeNode inNode, Server svr)
        {
            string query = "SELECT * FROM " + inNode.Text.ToUpper();
            return StartQuery(query, svr);
        }

        public static int StartProcess(ToolStripItem inItem, Server svr)
        {
            string tag = inItem.Tag.ToString();
            string[] tagItems = tag.Split(new char[] {','});
            int process = Global.Processes.Count();
            foreach (var p in Global.Plugins.Catalog.Parts)
                foreach (var ed in p.ExportDefinitions)
                    if (ed.ContractName == "IPlugin")
                        foreach (var meta in ed.Metadata)
                            if (meta.Key == "Name" && meta.Value == tag)
                            {
                                //process = StartPlugin(tag);
                                process = StartPlugin((IPlugin)p);
                                Global.Processes[process].UI.Tag = Global.Processes[process].UI.Tag + ", " + process.ToString();
                            }
            return process;
            
        }

        private static int StartQuery(string query, Server svr)
        {
            QueryPlug tmpQuery = new QueryPlug();
            if (svr.Connector == null && tmpQuery.Initialize()) return SetProcess(tmpQuery);
            else
            {
                if (query == "" && tmpQuery.Initialize(svr)) return SetProcess(tmpQuery);
                else if (tmpQuery.Initialize(svr, query)) return SetProcess(tmpQuery);
            }
            return -1;
        }

        private static int StartPlugin(string tag)
        {
            QProcess tmpProcess = new QProcess();
            return -1;
        }

        private static int StartPlugin(IPlugin plug)
        {
            QProcess tmpProc = new QProcess();
                
            return -1;
        }

        /*private static int StartPlugin(string[] tagItems)
        {
            QProcess tmpProcess = new QProcess();
            QProcess tmpProc = new QProcess();
            foreach (IPlugin plug in Global.Plugins.AvailablePlugins)
            {
                if (plug != null && plug.Name == tagItems[1])
                {
                    if (plug.Initialize())
                    {
                        tmpProc.UI = plug.UI;
                        tmpProc.Menu = plug.Menu;
                        tmpProc.PluginIndex = Convert.ToInt32(Global.Plugins.AvailablePlugins.GetAvailablePluginEnumerator());
                    }
                }
            }
            for (int i = 0; i < Global.Plugins.AvailablePlugins.Count; i++)
            {
                string strType = tagItems[1];
                if (Global.Plugins.AvailablePlugins[i] != null)
                    if (Global.Plugins.AvailablePlugins[i].Instance.Name == strType)
                        if (Global.Plugins.AvailablePlugins[i].Instance.Initialize())
                        {
                            tmpProcess.UI = Global.Plugins.AvailablePlugins[i].Instance.UI;
                            tmpProcess.Menu = Global.Plugins.AvailablePlugins[i].Instance.Menu;
                            tmpProcess.PluginIndex = i;
                            Global.Processes.Add(tmpProcess);
                            return Global.Processes.IndexOf(tmpProcess);
                        }
            }
            return -1;
        }*/

        private static int SetProcess(QueryPlug tmpQuery)
        {
                QProcess tmpProcess = new QProcess();
                tmpQuery.UI.Tag = "Query,";
                tmpQuery.UI.Text = tmpQuery.Svr.InstanceID;
                tmpProcess.UI = tmpQuery.UI;
                tmpProcess.Menu = tmpQuery.Menu;
                tmpProcess.PluginIndex = 0;
                Global.Processes.Add(tmpProcess);
                tmpProcess.UI.Tag = tmpProcess.UI.Tag + Global.Processes.IndexOf(tmpProcess).ToString();
                tmpProcess.Menu.Tag = "Query," + Global.Processes.IndexOf(tmpProcess).ToString();
                return Global.Processes.IndexOf(tmpProcess);

        }

        public static void RemoveProcess(TabPage tmpUI)
        {
            string[] tagItems = tmpUI.Tag.ToString().Split(',');
            Global.Processes.Remove(Global.Processes[Convert.ToInt32(tagItems[1])]);
        }

        public static void ReportValidCmdArgs()
        {

        }

        public static Stream GetResImage(string image)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            Stream icoTemp = _assembly.GetManifestResourceStream("QLite.Resources.q2icon.ico");
            try
            { icoTemp = _assembly.GetManifestResourceStream("QLite.Resources." + image); }
            catch (Exception ex)
            { ReportEx(ex, "Engine", "GetResImage"); }
            return icoTemp;
        }

        public static Server CreateServer(string connection)
        {
            Server tmpSvr = new Server(connection);
            try
            {
                tmpSvr.Connector.Open();
                tmpSvr.Connector.Close();
                tmpSvr.InstanceID = tmpSvr.Machine + "\\" + tmpSvr.Name;
                tmpSvr.LoadDatabaseSet();
            }
            catch (SqlException sql_ex) { ReportEx(sql_ex, "Engine", "CreateServer"); tmpSvr = null; }
            catch (Exception ex) { ReportEx(ex, "Engine", "CreateServer"); tmpSvr = null; }

            return tmpSvr;
        }

        public static void ExportToExcel(DataSet dataSet, string fileName)
        {
            //export a DataSet to Excel
            DialogResult retry = DialogResult.Retry;

            while (retry == DialogResult.Retry)
            {
                try
                {
                    using (ExportToExcel writer = new ExportToExcel(fileName))
                    {
                        writer.WriteStartDocument();

                        foreach (DataTable data in dataSet.Tables)
                        {
                            string wsName = data.TableName;

                            // Write the worksheet contents
                            writer.WriteStartWorksheet(wsName);

                            //Write header row
                            writer.WriteStartRow();

                            foreach (DataColumn col in data.Columns)
                            {
                                writer.WriteExcelUnstyledCell(col.Caption);
                            }

                            writer.WriteEndRow();

                            //write data
                            foreach (DataRow row in data.Rows)
                            {
                                writer.WriteStartRow();
                                foreach (object o in row.ItemArray)
                                {
                                    writer.WriteExcelAutoStyledCell(o);
                                }
                                writer.WriteEndRow();
                            }

                            writer.WriteEndWorksheet();
                        }

                        // Close up the document
                        writer.WriteEndDocument();
                        writer.Close();
                        retry = DialogResult.Cancel;
                    }
                }
                catch (Exception myException)
                {
                    retry = MessageBox.Show(myException.Message, "Excel Export", MessageBoxButtons.RetryCancel, MessageBoxIcon.Asterisk);
                }
            }
        }

        public static void ReportEx(Exception ex, string className, string methodName)
        {
            Trace.WriteLine(className + "." + methodName + " failed: " +
                        "\nMessage: " + ex.Message +
                        "\nSource: " + ex.Source +
                        "\nData: " + ex.Data +
                        "\nStack Trace: " + ex.StackTrace);

            QDialog.ScrollableMsgBox("Message: " + ex.Message +
                        "\nSource: " + ex.Source +
                        "\nData: " + ex.Data +
                        "\nStack Trace: " + ex.StackTrace,
                        className + "." + methodName + " Failed");
        }

        public static int NumberOServices(string machine)
        {
            int x = 0;
            try
            {
                ServiceController[] services = ServiceController.GetServices(machine);
                foreach (ServiceController service in services)
                    x++;
            }
            catch (Exception ex) { ReportEx(ex, "Engine", "NumberOServices"); }
            return x;
        }

        public static string[] GetSQLInstances(string machine)
        {
            string servicename = "SQL";
            string[] instances = new string[NumberOServices(machine)];
            int x = 0;
            ServiceController[] services = ServiceController.GetServices(machine);
            try
            {
                foreach (ServiceController service in services)
                    if (service == null)
                    {
                        MessageBox.Show(service.DisplayName.ToString());
                        if (service.DisplayName.Contains(servicename))
                        {
                            string tmpName = service.ServiceName;
                            instances[x] = tmpName;
                        }
                    }
            }
            catch (Exception ex)
            { ReportEx(ex, "Engine", "GetSQLInstances"); }
            return instances;
        }

        public static string[] SystemFonts()
        {
            InstalledFontCollection iFontCollection = new InstalledFontCollection();
            string[] holder = new string[iFontCollection.Families.Length];
            int i = 0;
            foreach (FontFamily ff in iFontCollection.Families)
            { holder[i] = ff.Name; i++; }
            return holder;
        }

        public static bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        public static int LoadPlugins()
        {
            try
            {
                Global.PluginServices = new PluginServices();
                Global.PluginServices.InitializePlugins();
            }
            catch (Exception ex)
            {
                ReportEx(ex, "QMain", "LoadPlugins()");
                return -1;
            }
            return 0;
        }

        #region Resource Cleanup
        private bool IsDisposed = false;

        public void Free()
        { if (IsDisposed) { throw new System.ObjectDisposedException("Object Name"); } }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~QMain()
        { Dispose(false); }

        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                if (disposedStatus) { }
            }
        }

        #endregion
    }
}
