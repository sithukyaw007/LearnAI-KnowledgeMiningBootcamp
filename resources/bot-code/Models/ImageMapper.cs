using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class ImageMapper
    {
        public static SearchHit ToSearchHit(SearchResult hit)
        {

            // Retrives fields from Cognitive Search.
            var keyPhrases = string.Join(",", (hit.Document["keyPhrases"] as string[]));
            if (keyPhrases.Length > 250)
                keyPhrases = keyPhrases.Substring(0, 250);
            var organizations = string.Join(",", (hit.Document["organizations"] as string[]));
            if (organizations.Length > 250)
                organizations = organizations.Substring(0, 250);

            var description = "𝐂𝐨𝐠𝐧𝐢𝐭𝐢𝐯𝐞 𝐊𝐞𝐲 𝐏𝐡𝐫𝐚𝐬𝐞𝐬: " +
                System.Environment.NewLine +
                keyPhrases + 
                System.Environment.NewLine +
                "𝐎𝐫𝐠𝐚𝐧𝐢𝐳𝐚𝐭𝐢𝐨𝐧𝐬 𝐈𝐝𝐞𝐧𝐭𝐢𝐟𝐢𝐞𝐝: " +
                System.Environment.NewLine +
                organizations + 
                System.Environment.NewLine 
                ;

            var url = hit.Document["blob_uri"] as string;

            var searchHit = new SearchHit
            {
                documentUrl = url,
                Description = description
            };

            object Tags;
            if (hit.Document.TryGetValue("Tags", out Tags))
            {
                searchHit.PropertyBag.Add("Tags", Tags);
            }
            return searchHit;
        }

    }
}