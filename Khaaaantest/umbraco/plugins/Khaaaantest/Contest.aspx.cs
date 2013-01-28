using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Khaaaantest.Entities;

namespace Khaaaantest.umbraco.plugins.Khaaaantest
{
    public partial class Contest : Page
    {

        public string ContestId = "-1";
        protected void Page_Load(object sender, EventArgs e)
        {

            // Set db connection
            ContestDS.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["umbracoDbDSN"];
            
            if (IsPostBack) return;
            int id;
            int.TryParse(Request["id"], out id);
            if (id < 1) return;
            ContestId = Request["id"];

            //force french culture
            var culture = CultureInfo.CreateSpecificCulture("fr-CA");
            
            var reader = global::umbraco.BusinessLogic.Application.SqlHelper.ExecuteReader("select * from khaaaantest_Contests where Id=@id", global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@id", id));
            var contestantCount = global::umbraco.BusinessLogic.Application.SqlHelper.ExecuteScalar<int>("select count(Id) from khaaaantest_Contestant where ContestId=@id", global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@id", id)); ;
            lblContestantCount.Text = contestantCount.ToString(CultureInfo.InvariantCulture);
            while (reader.Read())
            {
                lblNumberOfWinners.Text = reader.GetInt("NumberOfWinners").ToString(CultureInfo.InvariantCulture);
                lblContestName.Text = reader.GetString("Name");
                lblStartDate.Text = reader.GetDateTime("StartDate").ToString("dd MMMM yyyy",culture);
                lblEndDate.Text = reader.GetDateTime("EndDate").ToString("dd MMMM yyyy",culture);
            }

            var winnerReader = global::umbraco.BusinessLogic.Application.SqlHelper.ExecuteReader("select * from khaaaantest_Contestant where ContestId=@id and WinnerNumber>0 order by WinnerNumber", global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@id", id));
            var winners = new List<Contestant>();
            while (winnerReader.Read())
            {
                winners.Add(new Contestant
                                {
                                    FirstName = winnerReader.GetString("FirstName"),
                                    LastName = winnerReader.GetString("LastName"),
                                    Email = winnerReader.GetString("Email")
                                });
            }
            rptWinners.DataSource = winners;
            rptWinners.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Add") return;
            int id;
            int.TryParse(Request["id"], out id);
            if (id < 1) return;

            //check email first and fail if it is a duplicate
            var strEmail = ((TextBox)GridView1.FooterRow.FindControl("txtEmail")).Text;
            var emailExists = global::umbraco.BusinessLogic.Application.SqlHelper.ExecuteScalar<int>("select Id from khaaaantest_Contestant where Email=@email and ContestId=@id",global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@id", id), global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@email", strEmail));

            if (string.IsNullOrWhiteSpace(strEmail) || emailExists > 0)
            {
                lblError.Text = "Un usager s'est déjà inscrit au concours avec ce courriel, ou encore le champ obligatoire Courriel est vide.";
                ClearFooter();
                return;
            }

            var strFirstName = ((TextBox)GridView1.FooterRow.FindControl("txtFirstName")).Text;
            var strLastName = ((TextBox)GridView1.FooterRow.FindControl("txtLastName")).Text;

            if (string.IsNullOrWhiteSpace(strFirstName) || string.IsNullOrWhiteSpace(strLastName))
            {
                lblError.Text = "Les champs Prénom et Nom sont requis.";
                ClearFooter();
                return;
            }

            try
            {
                ContestDS.InsertParameters["FirstName"].DefaultValue = strFirstName;
                ContestDS.InsertParameters["LastName"].DefaultValue = strLastName;
                ContestDS.InsertParameters["Email"].DefaultValue = strEmail;
                ContestDS.InsertParameters["ContestId"].DefaultValue = id.ToString(CultureInfo.InvariantCulture);

                ContestDS.Insert();
            }
            catch
            {
                lblError.Text = "Un problème est survenu lors de la sauvegarde. Veuillez essayer de nouveau.";
            }
        }

        private void ClearFooter()
        {
            ((TextBox)GridView1.FooterRow.FindControl("txtFirstName")).Text = "";
            ((TextBox)GridView1.FooterRow.FindControl("txtLastName")).Text = "";
            ((TextBox)GridView1.FooterRow.FindControl("txtEmail")).Text = "";
        }

        protected void Winner_OnClick(object sender, EventArgs e)
        {
            int id;
            int.TryParse(Request["id"], out id);
            if (id < 1) return;
            //reset entire list of contestants to -1
            var resetSql = "update khaaaantest_Contestant set WinningNumber=-1 where ContestId=@contestId";
            global::umbraco.BusinessLogic.Application.SqlHelper.ExecuteNonQuery("update khaaaantest_Contestant set WinnerNumber=-1 where ContestId=@contestId", global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@contestId", id));

            //get number of winners for the contest
            var numberWinners=1;
            var reader = global::umbraco.BusinessLogic.Application.SqlHelper.ExecuteReader("select NumberOfWinners from khaaaantest_Contests where Id=@id", global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@id", id));
            while (reader.Read())
            {
                numberWinners = reader.GetInt("NumberOfWinners");
            }
            
            //randomly choose number of records that match the number of contest winners ordering them 1 to number of winners
            // and saving that to the database
            var winnerReader = global::umbraco.BusinessLogic.Application.SqlHelper.ExecuteReader("select top ("+numberWinners+") * from khaaaantest_Contestant where ContestId=@contestId order by newid()", global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@contestId", id));
            var current = 1;
            var winners = new List<Contestant>();
            while (winnerReader.Read())
            {
                winners.Add(new Contestant
                {
                    FirstName = winnerReader.GetString("FirstName"),
                    LastName = winnerReader.GetString("LastName"),
                    Email = winnerReader.GetString("Email")
                });
                var currentWinner = winnerReader.GetInt("Id");
                global::umbraco.BusinessLogic.Application.SqlHelper.ExecuteNonQuery("update khaaaantest_Contestant set WinnerNumber=@winnerNumber where Id=@id", global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@winnerNumber", current), global::umbraco.BusinessLogic.Application.SqlHelper.CreateParameter("@id", currentWinner));
                current++;
            }
            rptWinners.DataSource = winners;
            rptWinners.DataBind();
        }

        protected void GridView1_OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            int id;
            int.TryParse(Request["id"], out id);
            if (id < 1) id=0;
            e.Command.Parameters["@contestId"].Value = id.ToString(CultureInfo.InvariantCulture);
        }
    }
}