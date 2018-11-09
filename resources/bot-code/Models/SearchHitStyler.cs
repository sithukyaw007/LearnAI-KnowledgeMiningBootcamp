using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class SearchHitStyler
    {
        public void Apply<T>(ref IMessageActivity activity, string prompt, IReadOnlyList<T> options, IReadOnlyList<string> descriptions = null)
        {
            var hits = options as IList<SearchHit>;
            if (hits != null)
            {
                var urlButton = hits.Select(c => new CardAction()
                {
                    Value = c.documentUrl,
                    Type = "openUrl",
                    Title = "Open URL"
                });
                var searchButton = new CardAction()
                {
                    Value = "search",
                    Type = "imBack",
                    Title = "Search for something else"
                };
                var helpButton = new CardAction()
                {
                    Value = "Help",
                    Type = "imBack",
                    Title = "Help"
                };

                List<CardAction> cardButtons = new List<CardAction>();
                cardButtons.Add(searchButton);
                cardButtons.Add(helpButton);

                var cards = hits.Select(h => new HeroCard
                {
                    Title = h.documentUrl,
                    Subtitle = "(click URL to view document)",
                    Text = h.Description,
                    Buttons = cardButtons
                });

                activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                activity.Attachments = cards.Select(c => c.ToAttachment()).ToList();
                activity.Text = prompt;
            }
        }
    }
}
