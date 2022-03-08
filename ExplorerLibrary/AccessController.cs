using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Security.AccessControl;

namespace ExplorerLibrary
{
    public class AccessController
    {
        public static void TakeOwnership(string path)
        {
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(path);
                DirectorySecurity dSecurity = dInfo.GetAccessControl();
                ReplaceAllDescendantPermissionsFromObject(dInfo, dSecurity);
            } catch (Exception ex){
                System.Windows.Forms.MessageBox.Show(ex.ToString() + ex.Message);
            }
        }

        private static void ReplaceAllDescendantPermissionsFromObject(
            DirectoryInfo dInfo, DirectorySecurity dSecurity)
        {
            // Copy the DirectorySecurity to the current directory
            dInfo.SetAccessControl(dSecurity);

            foreach (FileInfo fi in dInfo.GetFiles())
            {
                // Get the file's FileSecurity
                var ac = fi.GetAccessControl();

                // inherit from the directory
                ac.SetAccessRuleProtection(false, false);

                // apply change
                fi.SetAccessControl(ac);
            }
            // Recurse into Directories
            dInfo.GetDirectories().ToList()
                .ForEach(d => ReplaceAllDescendantPermissionsFromObject(d, dSecurity));
        }
    }
}
