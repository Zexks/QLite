using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.ComponentModel.Composition.AttributedModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using QLite.Collections;
using QLite.Forms;

namespace QLite.Plugins
{
    public class PluginServices
    {
        [ImportMany(typeof(IPlugin))]
        private AggregateCatalog catalog = new AggregateCatalog();

        public  PluginServices()
        {
            
        }

        public void InitializePlugins()
        {
            //Load internal plugins
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            //Load external plugins
            FindPlugins(Global.Defaults.PluginPath);
            Global.Plugins = new CompositionContainer(catalog);
            Global.Plugins.ComposeParts(catalog);
        }

        public void FindPlugins(string Path)
        {
            foreach (string fileOn in Directory.GetFiles(Path))
                if (new FileInfo(fileOn).Extension == ".dll")
                    catalog.Catalogs.Add(new AssemblyCatalog(fileOn));
        }

        public void About(string Feedback, IPlugin Plugin)
        {
            System.Windows.Forms.Form newForm = null;
            Feedback newFeedbackForm = new Feedback();
            //foreach(var plug in Global.Plugins.Catalog.Parts)


            //newFeedbackForm.PluginDesc = Plugin.Info[0];
            //newFeedbackForm.PluginAuthor = "By: " + Plugin.Info[1];
            //newFeedbackForm.PluginName = Plugin.Name;
            //newFeedbackForm.PluginVersion = Plugin.Info[2];
            //newFeedbackForm.Feedback = Feedback;

            newForm = newFeedbackForm;
            newForm.ShowDialog();
            
            newFeedbackForm = null;
            newForm = null;

        }

    }								

}
