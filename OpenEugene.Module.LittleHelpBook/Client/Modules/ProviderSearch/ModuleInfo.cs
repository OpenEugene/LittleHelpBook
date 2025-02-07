using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Shared;
using System.Collections.Generic;

namespace OpenEugene.Module.ProviderSearch
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Provider Search",
            Description = "Provider Search",
            Version = "1.1.0",
            ServerManagerType = "OE.Module.LHB.Manager.LHBManager, OE.Module.LHB.Server.Oqtane",
            ReleaseVersions = "1.1.0",
            Dependencies = "OpenEugene.Module.LittleHelpBook.Shared.Oqtane,MudBlazor",
            PackageName = "OpenEugene.LittleHelpBook"
        };
    }
}
