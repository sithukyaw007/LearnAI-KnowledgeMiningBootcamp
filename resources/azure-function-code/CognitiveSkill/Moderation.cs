using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.CognitiveSearch.WebApiSkills;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CognitiveSkill
{
    public static class Function1
    {
        [FunctionName("ContentModerator")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log, ExecutionContext executionContext)
        {
            log.Info("C# HTTP trigger function processed a request.");
            string skillName = executionContext.FunctionName;

            IEnumerable<WebApiRequestRecord> requestRecords = WebApiSkillHelpers.GetRequestRecords(req);
            if (requestRecords == null)
            {

                return req.CreateErrorResponse(HttpStatusCode.BadRequest, $"{skillName} - Invalid request record array.");
            }
            dynamic obj = requestRecords.First().Data.First().Value;

            if (obj.Length > 1024)
                obj = obj.Substring(0, 1024);

            string val = await MakeRequest(obj);
            ContentModerator mod = JsonConvert.DeserializeObject<ContentModerator>(val);

            bool requiresModeration = false;
            if (mod.PII.Email.Length > 0)
                requiresModeration = true;
            if (mod.PII.Address.Length > 0)
                requiresModeration = true;
            if (mod.PII.IPA.Length > 0)
                requiresModeration = true;
            if (mod.PII.Phone.Length > 0)
                requiresModeration = true;
            WebApiResponseRecord output = new WebApiResponseRecord();
            output.RecordId = requestRecords.First().RecordId;
            output.Data["text"] = requiresModeration;
            
            WebApiSkillResponse resp = new WebApiSkillResponse();
            resp.Values = new List<WebApiResponseRecord>();
            resp.Values.Add(output);
            return req.CreateResponse(HttpStatusCode.OK, resp);
        }
        static async Task<string> MakeRequest(string input)
        {
            var client = new HttpClient();

            //TO-DO: URL of the Moderator API. Fix the Prefix with your URL, what can be found in the Azure Portal. 
            //If you are using southcentralus, don't need to change anything
            var uriPrefix = "https://southcentralus.api.cognitive.microsoft.com/contentmoderator";
            var uriSuffix = "/moderate/v1.0/ProcessText/Screen?autocorrect=false&PII=true&classify=false&language=eng";

            //TO-DO: Add your content moderator key here
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "your content moderator key here");

            //TO-DO: WITHOUT he https://, check the url of the region of your content moderator API. If nos southcentralus, change it.
            client.DefaultRequestHeaders.Add("Host", "southcentralus.api.cognitive.microsoft.com");


            var uri = uriPrefix + uriSuffix;

            HttpResponseMessage response;
            byte[] byteData = Encoding.UTF8.GetBytes(input);
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                response = await client.PostAsync(uri, content);
            }
            var result = response.Content.ReadAsStringAsync();

            return await result;

        }
    }


    public class ContentModerator
    {
        public string OriginalText { get; set; }
        public string NormalizedText { get; set; }
        public string AutoCorrectedText { get; set; }
        public object Misrepresentation { get; set; }
        public Classification Classification { get; set; }
        public Status Status { get; set; }
        public PII PII { get; set; }
        public string Language { get; set; }
        public Terms[] Terms { get; set; }
        public string TrackingId { get; set; }
    }

    public class Classification
    {
        public Category1 Category1 { get; set; }
        public Category2 Category2 { get; set; }
        public Category3 Category3 { get; set; }
        public bool ReviewRecommended { get; set; }
    }

    public class Category1
    {
        public float Score { get; set; }
    }

    public class Category2
    {
        public float Score { get; set; }
    }

    public class Category3
    {
        public float Score { get; set; }
    }

    public class Status
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public object Exception { get; set; }
    }

    public class PII
    {
        public Email[] Email { get; set; }
        public IPA[] IPA { get; set; }
        public Phone[] Phone { get; set; }
        public Address[] Address { get; set; }
    }

    public class Email
    {
        public string Detected { get; set; }
        public string SubType { get; set; }
        public string Text { get; set; }
        public int Index { get; set; }
    }

    public class IPA
    {
        public string SubType { get; set; }
        public string Text { get; set; }
        public int Index { get; set; }
    }

    public class Phone
    {
        public string CountryCode { get; set; }
        public string Text { get; set; }
        public int Index { get; set; }
    }

    public class Address
    {
        public string Text { get; set; }
        public int Index { get; set; }
    }

    public class Terms
    {
        public int Index { get; set; }
        public int OriginalIndex { get; set; }
        public int ListId { get; set; }
        public string Term { get; set; }
    }

}
