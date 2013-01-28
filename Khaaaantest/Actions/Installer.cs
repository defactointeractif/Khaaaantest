using System;
using System.IO;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace Khaaaantest.Actions
{
    public class Installer : IPackageAction
    {

        #region IPackageAction Members

        public string Alias()
        {
            return "KhaaaantestInstaller";
        }

        /// <summary>
        /// Executes the sql action
        /// </summary>
        public bool Execute(string packageName, System.Xml.XmlNode xmlData)
        {
            try
            {
                var result = Install();
                return result;
            }
            catch (Exception e)
            {
                Log.Add(LogTypes.Error, -1, string.Format("Error in action for package {0} error:{1} ", packageName, e.ToString()));
                return false;
            }
            
            
        }

        public System.Xml.XmlNode SampleXml()
        {
            // Is this used?
            const string sample = "<Action runat=\"install\" undo=\"false\" alias=\"ExecuteSql\"><![CDATA[CREATE TABLE tmp (	[ClientCategoryId] [int] IDENTITY(1,1) NOT NULL)]]></Action>";
            return helper.parseStringToXmlNode(sample);
        }

        /// <summary>
        /// When you want to Undo an Sql Install create a new action that only runs at UnInstall 
        /// </summary>
        public bool Undo(string packageName, System.Xml.XmlNode xmlData)
        {
            var result = Uninstall();
            return result;
        }
        
        #endregion

        /// <summary>
        /// Package installation process
        /// </summary>
        private bool Install()
        {
            var sqlHelper = Application.SqlHelper;

            // Run install script
            var installScriptPath = System.Web.Hosting.HostingEnvironment.MapPath("/Khaaaantest/SQLScripts/Install.sql");
            if (!string.IsNullOrEmpty(installScriptPath))
            {
                var script = File.ReadAllText(installScriptPath);
                var result = sqlHelper.ExecuteNonQuery(script);
                if (result != 1)
                {
                    // Something went wrong, don't know what though...
                }
                else
                {
                    return true;
                }
            }

            return false;

        }

        /// <summary>
        /// Package uninstallation process
        /// </summary>
        private bool Uninstall()
        {

            var sqlHelper = Application.SqlHelper;

            // Run uninstall script
            var uninstallScriptPath = System.Web.Hosting.HostingEnvironment.MapPath("/Khaaaantest/SQLScripts/Uninstall.sql");
            if (!string.IsNullOrEmpty(uninstallScriptPath))
            {
                var script = File.ReadAllText(uninstallScriptPath);
                var result = sqlHelper.ExecuteNonQuery(script);
                if (result != 1)
                {
                    // Something went wrong, don't know what though...
                }
                else
                {
                    return true;
                }
            }
            
            return false;

        }

    }
}