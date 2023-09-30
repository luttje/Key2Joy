using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Plugins
{
    public class AppDomainHelper
    {
        public static bool GetIsOwnDomain()
        {
            try
            {
                // Trying to access the BaseDirectory whilst in the wrong appdomain will throw an exception
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                return baseDirectory != null;
            }
            catch (SecurityException)
            {
                Debug.WriteLine("SecurityException was thrown through AppDomainHelper.GetIsOwnDomain() check.");
                return false;
            }
        }
    }
}
