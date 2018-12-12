using Microsoft.SharePoint.Client;
using System;
using System.Configuration;
using System.Linq;
using System.Net;

namespace CSOM.SP.Helpers
{
    public class AHAuthenticate
    {
        public bool isUserAuthFor(string userName, string groupName, string sharePointURL = "Default")
        {
            string targetDomain = ConfigurationManager.AppSettings["Domain"];
            string adminUserName = ConfigurationManager.AppSettings["SharepointUserName"];
            string adminpassword = ConfigurationManager.AppSettings["SharepointPassword"];
            sharePointURL = sharePointURL != "Default" ? sharePointURL : ConfigurationManager.AppSettings["SharepointURL"];

            bool isAuth = false;
            bool groupExists = false;
            UserCollection collUser = null;
            User user = null;
            GroupCollection gcollection = null;

            using (ClientContext context = new ClientContext(sharePointURL))
            {
                context.Credentials = new NetworkCredential(adminUserName, adminpassword, targetDomain);
                Web site = context.Web;

                try
                {
                    user = site.EnsureUser(userName);
                    context.Load(user);
                    context.ExecuteQuery();
                }
                catch (Exception)
                {
                    return false;
                }

                try
                {
                    gcollection = site.SiteGroups;
                    context.Load(gcollection);
                    context.ExecuteQuery();
                }
                catch (Exception)
                {

                    return false;
                }

                Group groupsa = gcollection.Where(gp => gp.Title == groupName).FirstOrDefault();


                if (groupsa != null)
                {
                    // Group is matched with the required one
                    // Fill collUser with users of the required group
                    collUser = groupsa.Users;
                    context.Load(collUser);
                    context.ExecuteQuery();
                    groupExists = true;

                }
                if (groupExists)
                {
                    User userin = collUser.Where(us => us.LoginName == user.LoginName).FirstOrDefault();

                    if (userin != null)
                    {
                        // Target user exist
                        // Giv user permission to edit
                        isAuth = true;
                    }
                }
                else
                {
                    isAuth = false;
                }
            }
            return isAuth;
        }
    }
}
