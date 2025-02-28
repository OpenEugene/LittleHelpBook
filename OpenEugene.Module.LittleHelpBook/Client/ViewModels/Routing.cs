using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEugene.Module.LittleHelpBook.Client.Viewmodels
{
    internal class Routing
    {
        // provider constants for URLParameters
        public const string ProviderTemplate = "/{providerId}";
        public const string ProviderId = "providerId";
        public const string ProviderRoute = "/provider";
        public const string ProviderList = "/providers";

        // phone constants for URLParameters
        public const string PhoneTemplate = "/{providerId}/{phoneId}";
        public const string PhoneId = "phoneId";

        // phone constants for URLParameters
        public const string AddressTemplate = "/{providerId}/{addressId}";
        public const string AddressId = "addressId";

    }
}
