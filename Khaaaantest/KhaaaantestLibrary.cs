using System.Net.Mail;
using System.Web;
using umbraco;
using Umbraco.Web.BaseRest;

namespace Khaaaantest
{
    [RestExtension("KhaaaantestLibrary")]
    public class KhaaaantestLibrary
    {
        
        [RestExtensionMethod(ReturnXml = false)]
        public static string SubmitContest()
        {
            var umbracoContestId = HttpContext.Current.Request["contestId"];
            var firstName = HttpContext.Current.Request["fname"];
            var lastName = HttpContext.Current.Request["lname"];
            var email = HttpContext.Current.Request["email"];
            
            // var newsletter = HttpContext.Current.Request["newsletter"];

            var hasError = false;
            var errorMessage = "";

            // Built in message in the net namespace will validate the email, no need for crazy regex here
            try
            {
                var message = new MailMessage { From = new MailAddress(email) };
            }
            catch
            {
                hasError = true;
                errorMessage += library.GetDictionaryItem("Khaaaantest_Error_FormatEmail") + "<br/>";
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                hasError = true;
                errorMessage += library.GetDictionaryItem("Khaaaantest_Error_RequiredFirstName") + "<br/>";
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                hasError = true;
                errorMessage += library.GetDictionaryItem("Khaaaantest_Error_RequiredEmail") + "<br/>";
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                hasError = true;
                errorMessage += library.GetDictionaryItem("Khaaaantest_Error_RequiredEmail") + "<br/>";
            }

            if (!hasError)
            {
                var sqlHelper = global::umbraco.BusinessLogic.Application.SqlHelper;

                // Get the contest id from the umbraco node.
                var reader = sqlHelper.ExecuteReader(
                    "SELECT * " +
                    "FROM khaaaantest_Contests " +
                    "WHERE ContestId = @ContestId",
                    sqlHelper.CreateParameter("@ContestId", umbracoContestId)
                );

                if (reader.Read())
                {

                    var contestId = reader.GetInt("Id");
                    if (contestId > 0)
                    {
                        // Validation, 1 participation per email
                        var validationReader = sqlHelper.ExecuteReader(
                            "SELECT COUNT(1) AS Count " +
                            "FROM khaaaantest_Contestant " +
                            "WHERE Email = @Email " +
                            "AND ContestId = @ContestId",
                            sqlHelper.CreateParameter("@Email", email),
                            sqlHelper.CreateParameter("@ContestId", contestId)
                        );

                        if (validationReader.Read())
                        {
                            var count = validationReader.GetInt("Count");
                            if (count == 0)
                            {

                                var ipAdd = HttpContext.Current.Request.UserHostAddress;

                                // Insert into contest DB
                                var result = sqlHelper.ExecuteNonQuery(
                                    "INSERT INTO [khaaaantest_Contestant] ([FirstName], [LastName], [Email], [ContestId], [IPAdd]) " +
                                    "VALUES (@FirstName, @LastName, @Email, @ContestId, @IPAdd)",
                                    sqlHelper.CreateParameter("@FirstName", firstName),
                                    sqlHelper.CreateParameter("@LastName", lastName),
                                    sqlHelper.CreateParameter("@Email", email),
                                    sqlHelper.CreateParameter("@ContestId", contestId),
                                    sqlHelper.CreateParameter("@IPAdd", ipAdd)
                                );

                                if (result != 1)
                                {
                                    // Something went wrong, don't know what though...
                                }

                            }
                            else
                            {
                                errorMessage = library.GetDictionaryItem("Khaaaantest_Error_AlreadyContestant") + "<br/>";
                            }
                        }
                        else
                        {
                            // todo: log this instead
                            errorMessage = "Erreur avec le système de concours...";
                        }
                    }
                    else
                    {
                        // todo: log this instead
                        errorMessage = "Concours introuvable...";
                    }

                }
                else
                {
                    // todo: log this instead
                    errorMessage = "Erreur avec le système de concours...";
                }

                // todo: Insert hook for newsletter module
                // Call cake mail for newsletter
                /*if (newsletter != "")
                {
                    try
                    {
                        const string listId = "f9caf6e4f21005cfd886dcec1187a9b3";
                        const string baseUrl = "http://link.defactointeractif.com/oi/1/";
                        var registerUrl = baseUrl + listId + "?email=" + email + "&source=Concours" + "&no_triggers=false";

                        var request = (HttpWebRequest)WebRequest.Create(registerUrl);
                        var response = (HttpWebResponse)request.GetResponse();
                    }
                    catch (Exception e)
                    {
                        return "Erreur... " + e.Message;
                    }

                }*/

                if (errorMessage != "")
                {
                    return errorMessage;
                }
                else
                {
                    return "ok";
                }

            }
            else
            {
                return errorMessage;

            }
        }

    }
}