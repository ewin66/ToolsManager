using Atlante.Common;
using Atlante.Mef.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace ToolsManager.Client.ViewModels
{
    public class AddInsViewModel
    {
        private const string ADDINS_FOLDER = "Add-ins";

        [ImportMany]
        public IEnumerable<IAddIn> AddIns { get; private set; }

        public AddInsViewModel()
        {
            this.LoadData();
        }

        private void LoadData()
        {
            Logger.AddTrace("Loading Data");

            try
            {
                var catalog = new AggregateCatalog();

                var addInDirectories = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), ADDINS_FOLDER), "*.*", SearchOption.AllDirectories);

                foreach (string directoryPath in addInDirectories)
                {
                    catalog.Catalogs.Add(new DirectoryCatalog(directoryPath, "*.dll"));
                    catalog.Catalogs.Add(new DirectoryCatalog(directoryPath, "*.exe"));
                }

                var container = new CompositionContainer(catalog);

                container.ComposeParts(this);
            }
            catch (CompositionException e)
            {
                Logger.AddException(e);
            }
            catch (Exception e)
            {
                Logger.AddException(e);

                foreach (var loadException in (e as ReflectionTypeLoadException).LoaderExceptions)
                    Logger.AddException(loadException);
            }
        }
    }
}
