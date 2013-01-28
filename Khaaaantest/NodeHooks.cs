using Umbraco.Core;
using Umbraco.Web;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;

namespace Khaaaantest
{
    public class NodeHooks : IApplicationEventHandler
    {

        public void OnApplicationInitialized(UmbracoApplication httpApplication, ApplicationContext applicationContext)
        {
            Document.New += DocumentNew;
            Document.AfterPublish += AfterPublish;
        }

        public void OnApplicationStarting(UmbracoApplication httpApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarted(UmbracoApplication httpApplication, ApplicationContext applicationContext)
        {
        }

        static void AfterPublish(Document sender, PublishEventArgs args)
        {
            // Khaaaantest_Contest
            if (sender.ContentType.Alias == "Khaaaantest_Contest")
            {

                var sqlHelper = Application.SqlHelper;

                // Get the contest id from the contest module using this umbraco node id.
                var reader = sqlHelper.ExecuteReader(
                    "SELECT * " +
                    "FROM khaaaantest_Contests " +
                    "WHERE ContestId = @ContestId",
                    sqlHelper.CreateParameter("@ContestId", sender.Id)
                );

                // Update if the contest exists or else create it
                if (reader.Read())
                {
                    // Prepare data to sync
                    var name = sender.Text;
                    var numberOfWinners = sender.getProperty("numberOfWinnersToDraw").Value;
                    var startDate = sender.getProperty("startDate").Value;
                    var endDate = sender.getProperty("endDate").Value;

                    // Update the contest module
                    var result = sqlHelper.ExecuteNonQuery(
                        "UPDATE [khaaaantest_Contests] " +
                        "SET Name=@Name, StartDate=@StartDate, EndDate=@EndDate, NumberOfWinners=@NumberOfWinners " +
                        "WHERE ContestId=@ContestId",
                        sqlHelper.CreateParameter("@ContestId", sender.Id),
                        sqlHelper.CreateParameter("@Name", name),
                        sqlHelper.CreateParameter("@StartDate", startDate),
                        sqlHelper.CreateParameter("@EndDate", endDate),
                        sqlHelper.CreateParameter("@NumberOfWinners", numberOfWinners)
                    );

                    if (result != 1)
                    {
                        // Something went wrong, don't know what though...
                        // todo: Log or something...
                    }
                }
                else
                {
                    // Prepare dates
                    var startDate = sender.getProperty("startDate").Value;
                    var endDate = sender.getProperty("endDate").Value;

                    // Insert into DB
                    var result = sqlHelper.ExecuteNonQuery(
                        "INSERT INTO [khaaaantest_Contests] ([Name], [ContestId], [StartDate], [EndDate], [NumberOfWinners]) " +
                        "VALUES (@Name, @ContestId, @StartDate, @EndDate, @NumberOfWinners)",
                        sqlHelper.CreateParameter("@Name", sender.Text),
                        sqlHelper.CreateParameter("@ContestId", sender.Id),
                        sqlHelper.CreateParameter("@StartDate", startDate),
                        sqlHelper.CreateParameter("@EndDate", endDate),
                        sqlHelper.CreateParameter("@NumberOfWinners", sender.getProperty("numberOfWinnersToDraw").Value)
                    );

                    if (result != 1)
                    {
                        // Something went wrong, don't know what though...
                        // todo: Log or something...
                    }
                }

            }

        }

        static void DocumentNew(Document sender, NewEventArgs e)
        {
            if (sender.ContentType.Alias == "Khaaaantest_Contest")
            {
                // Default numberOfWinnersToDraw value
                if (string.IsNullOrEmpty(sender.getProperty("numberOfWinnersToDraw").Value.ToString()))
                {
                    sender.getProperty("numberOfWinnersToDraw").Value = 1;
                }
            }
        }

    }
}