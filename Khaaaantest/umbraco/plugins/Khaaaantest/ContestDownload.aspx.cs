using System;
using System.Globalization;
using System.Text;

namespace Khaaaantest.umbraco.plugins.Khaaaantest
{
    public partial class ContestDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            int id;
            int.TryParse(Request["id"], out id);
            if (id < 1) return;

            Response.Clear();

            var fileName= String.Format("Contest-{0}.csv", id); 
            Response.ContentType = "text/csv";
            Response.ContentEncoding = Encoding.Unicode;
            Response.AddHeader("content-disposition", "filename=" + fileName);

            //force french culture
            var culture = CultureInfo.CreateSpecificCulture("fr-CA");

            // write string data to Response.OutputStream here
            var reader = global::umbraco.BusinessLogic.Application.SqlHelper.ExecuteReader("select * from khaaaantest_Contestant where ContestId=@id", global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@id", id));
            Response.Write("Id,Prénom,Nom,Courriel,Gagnant numéro,Id de concours,IP Adresse,Entrée le\n");
            while (reader.Read())
            {
                var uid = reader.GetInt("Id").ToString(CultureInfo.InvariantCulture);
                var fname = reader.GetString("FirstName");
                var lname = reader.GetString("LastName");
                var email = reader.GetString("Email");
                var winnumber = reader.GetInt("WinnerNumber");
                var contestid = reader.GetString("ContestId");
                var ipadd = reader.GetString("IPAdd");
                var created = reader.GetDateTime("CreatedOn").ToString("dd MMMM yyyy", culture);
                Response.Write(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}\n",uid,fname,lname,email,winnumber,contestid,ipadd,created));
            }
            

            Response.End();
        }
    }
}