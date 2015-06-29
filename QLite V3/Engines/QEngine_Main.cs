using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.ServiceProcess;
using System.Reflection;
using QLite.Servers;
using QLite.QueryPlugin;

namespace QLite.Engines
{
    class QEngine_Main
    {
        #region Globals
        private static QEngine_Dialog Dialogs = new QEngine_Dialog();

        #endregion

        public static void SetDefaults()
        {
            Global.QliteDefaults.XmlDefaults = new XmlDocument();
            Global.QliteDefaults.XmlFileName = "QLite2.xml";
            Global.QliteDefaults.XmlDefaults.Load(Global.QliteDefaults.XmlFileName);
            Global.QliteDefaults.InvalidNameChars = new char[] { '~','`', '!', '@', '$', '%', '^', '*', '|', '{', '}', '[', ']', ':', ';', '?', '/', '.', ',', '\"', '\\', '\'', '<', '>' };

            XmlNode nodeOptions = QEngine_XML.PullOneDeep("Options");
            
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
                                    Global.QliteDefaults.Fonts.Add(tmpFont);
                                }

                                break;*/
                            case "CONNECTIONS":
                                foreach (XmlNode connection in setting.ChildNodes)
                                {
                                    attributes = connection.Attributes;
                                    switch (connection.Name.ToUpper())
                                    {
                                        case "DEFAULT":
                                            Global.QliteDefaults.Connection = new string[4];
                                            Global.QliteDefaults.Connection[0] = attributes[0].Value;
                                            Global.QliteDefaults.Connection[1] = attributes[1].Value;
                                            Global.QliteDefaults.Connection[2] = attributes[2].Value;
                                            Global.QliteDefaults.Connection[3] = attributes[3].Value;
                                            break;
                                    }
                                }
                                break;
                            case "PLUGPATH":
                                attributes = setting.Attributes;
                                if (attributes[0].Value != "") Global.QliteDefaults.PluginPath = attributes[0].Value;
                                else Global.QliteDefaults.PluginPath = Application.StartupPath;
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
                                        Global.QliteDefaults.PasswordLock = false;
                                    else if (arg.Substring(2, 1).ToUpper() == "T")
                                        Global.QliteDefaults.PasswordLock = true;
                                    else
                                        Console.WriteLine("Invalid passoword option. Please select either T for true or F for false");
                                break;
                            case "D":
                                if (isNumeric(arg.Substring(2,1), System.Globalization.NumberStyles.Integer))
                                    if (Convert.ToInt32(arg.Substring(2, 1)) >= 0 && Convert.ToInt32(arg.Substring(2, 1)) < 6)

                                        Global.QliteDefaults.DebugLvl = Convert.ToInt32(arg.Substring(2, 1));
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
                Global.QliteDefaults.DebugLvl = 0;
                Global.QliteDefaults.PasswordLock = true;
            }
        }

        public static int StartProcess(TreeNode inNode, Server svr)
        {
            string query = "SELECT * FROM " + inNode.Text.ToUpper();
            return StartQuery(query, svr);
        }

        public static int StartProcess(ToolStripMenuItem inItem, Server svr)
        {
            string tag = inItem.Tag.ToString();
            string[] tagItems = tag.Split(new char[] {','});
            int process = -1;
            switch (tagItems[0])
            {
                case "Query":
                    process = StartQuery("", svr);
                    break;
                case "Plugin":
                    process = StartPlugin(tagItems);
                    if(process != -1)
                        Global.QliteProcesses[process].Page.Tag = Global.QliteProcesses[process].Page.Tag + "," + process.ToString();
                    break;
            }
            return process;
            
        }

        private static int StartQuery(string query, Server svr)
        {
            QueryPlug tmpQuery = new QueryPlug();
            if (svr.Connector == null)
            {
                if (tmpQuery.Initialize())
                    return SetProcess(tmpQuery);
            }
            else
            {
                if (query == "")
                {
                    if (tmpQuery.Initialize(svr))
                        return SetProcess(tmpQuery);
                }
                else
                    if (tmpQuery.Initialize(svr, query))
                        return SetProcess(tmpQuery);
            }
            return -1;
        }

        private static int StartPlugin(string[] tagItems)
        {
            Process tmpProcess = new Process();

            for (int i = 0; i < Global.Plugins.AvailablePlugins.Count; i++)
            {
                string strType = tagItems[1];
                if (Global.Plugins.AvailablePlugins[i] != null)
                    if (Global.Plugins.AvailablePlugins[i].Instance.Name == strType)
                        if (Global.Plugins.AvailablePlugins[i].Instance.Initialize())
                        {
                            tmpProcess.Page = Global.Plugins.AvailablePlugins[i].Instance.Page;
                            tmpProcess.Menu = Global.Plugins.AvailablePlugins[i].Instance.Menu;
                            tmpProcess.PluginIndex = i;
                            Global.QliteProcesses.Add(tmpProcess);
                            return Global.QliteProcesses.IndexOf(tmpProcess);
                        }
            }
            return -1;
        }

        private static int SetProcess(QueryPlug tmpQuery)
        {
                Process tmpProcess = new Process();
                tmpQuery.Page.Tag = "Query,";
                tmpQuery.Page.Text = tmpQuery.Svr.InstanceID;
                tmpProcess.Page = tmpQuery.Page;
                tmpProcess.Menu = tmpQuery.Menu;
                tmpProcess.PluginIndex = 0;
                Global.QliteProcesses.Add(tmpProcess);
                tmpProcess.Page.Tag = tmpProcess.Page.Tag + Global.QliteProcesses.IndexOf(tmpProcess).ToString();
                tmpProcess.Menu.Tag = "Query," + Global.QliteProcesses.IndexOf(tmpProcess).ToString();
                return Global.QliteProcesses.IndexOf(tmpProcess);

        }

        public static void RemoveProcess(TabPage tmpPage)
        {
            string[] tagItems = tmpPage.Tag.ToString().Split(',');
            Global.QliteProcesses.Remove(Global.QliteProcesses[Convert.ToInt32(tagItems[1])]);
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
                    using (ExcelWriter writer = new ExcelWriter(fileName))
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

            QEngine_Dialog.ScrollableMsgBox("Message: " + ex.Message +
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
        
        #region Resource Cleanup
        private bool IsDisposed = false;

        public void Free()
        { if (IsDisposed) { throw new System.ObjectDisposedException("Object Name"); } }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~QEngine_Main()
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
