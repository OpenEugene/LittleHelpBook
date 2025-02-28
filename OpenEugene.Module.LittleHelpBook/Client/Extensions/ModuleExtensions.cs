using Oqtane.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenEugene.Module.LittleHelpBook.Client.Extensions;

public static class ModuleExtensions
{
    /// <summary>
    /// handy oqtane extension to build action urls that include the return url
    /// Great for apps that use URLParameters
    /// </summary>
    /// <param name="moduleBase"></param>
    /// <param name="basePath"></param>
    /// <param name="moduleId"></param>
    /// <param name="action"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static string ComposeUrl(this ModuleBase moduleBase, string basePath, int moduleId, string action, params object[] parameters)
    {
        // deep oqtane voodoo. see https://github.com/oqtane/oqtane.framework/discussions/2366

        var parms = moduleBase.AddUrlParameters(parameters);
        var edit = moduleBase.EditUrl(basePath, moduleId, action, parms);
        var returnUrl = moduleBase.NavigateUrl(basePath, $"{parameters[0]}").Replace("?", "/!/"); // Conver to URL Parameters
        return $"{edit}?returnUrl={returnUrl}";
    }

    public static bool IsSuccessStatusCode(this ModuleBase moduleBase, HttpStatusCode statusCode)
    {
        return (int)statusCode >= 200 && (int)statusCode <= 299;
    }

}