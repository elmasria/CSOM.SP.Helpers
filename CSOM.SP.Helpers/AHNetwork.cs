using AHHelperTools;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOM.SP.Helpers
{
    public class AHNetwork
    {
        /// <summary>
        /// To accept Form Based Authentication 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ctx_MixedAuthRequest(object sender, WebRequestEventArgs e)
        {
            try
            {
                //Add the header that tells SharePoint to use Windows authentication.
                e.WebRequestExecutor.RequestHeaders.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
            }
            catch (Exception ex)
            {
                AHlogs.log(ex.Message, "csom-sp-helper");
            }
        }
    }
}
