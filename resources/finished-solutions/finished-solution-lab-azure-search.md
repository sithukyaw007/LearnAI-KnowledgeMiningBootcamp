# Finished Solution - Custom Skills Lab

Hello!

Here are suggestions for the challenge at the end of the [Azure Search Lab](../../labs/lab-azure-search.md)

The commands below should be types in the Search Explorer **Query string** bar.

1. Return only the first document: `$top=1`

1. Search documents where words "Microsoft" and "Cloud" are up to 20 words distant one from the other: `"Microsoft Cloud" ~20`

1. Search for documents about Cloud, ordering the results by the score: `cloud order by score desc`

1. Search for documents about Cloud, but filtering those with mentions to Oracle: `cloud -oracle`

1. Search for documents about Cognitive Services and Bots: `"Cognitive Services", Bots`

## Next Step

[Text Skills Lab](../../labs/lab-text-skills.md) or
[Back to Read Me](../../README.md)