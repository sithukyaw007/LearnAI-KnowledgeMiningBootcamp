# Finished Solution - Custom Skills Lab

Hello!

Here are the body requests for the custom skills lab. Don't forget to adjust the URLs to use your Azure Search service name.

## Skillset

```json
{
  "description":
  "Extract entities, detect language and extract key-phrases. Also, Moderates the content with a custom skill",
  "skills":
  [
    {
      "@odata.type": "#Microsoft.Skills.Text.NamedEntityRecognitionSkill",
      "categories": [ "Organization" ],
      "defaultLanguageCode": "en",
      "inputs": [
        {
          "name": "text", "source": "/document/content"
        }
      ],
      "outputs": [
        {
          "name": "organizations", "targetName": "organizations"
        }
      ]
    },
    {
      "@odata.type": "#Microsoft.Skills.Text.LanguageDetectionSkill",
      "inputs": [
        {
          "name": "text", "source": "/document/content"
        }
      ],
      "outputs": [
        {
          "name": "languageCode",
          "targetName": "languageCode"
        }
      ]
    },
    {
      "@odata.type": "#Microsoft.Skills.Text.SplitSkill",
      "textSplitMode" : "pages",
      "maximumPageLength": 4000,
      "inputs": [
      {
        "name": "text",
        "source": "/document/content"
      },
      {
        "name": "languageCode",
        "source": "/document/languageCode"
      }
    ],
    "outputs": [
      {
            "name": "textItems",
            "targetName": "pages"
      }
    ]
  },
  {
      "@odata.type": "#Microsoft.Skills.Text.KeyPhraseExtractionSkill",
      "context": "/document/pages/*",
      "inputs": [
        {
          "name": "text", "source": "/document/pages/*"
        },
        {
          "name":"languageCode", "source": "/document/languageCode"
        }
      ],
      "outputs": [
        {
          "name": "keyPhrases",
          "targetName": "keyPhrases"
        }
      ]
    },
    {
        "@odata.type": "#Microsoft.Skills.Custom.WebApiSkill",
        "description": "Our new moderator custom skill",
        "uri": "https://[your-azure-function-name].azurewebsites.net/api/ContentModerator?code=[your-azure-function-key]",
        "batchSize":1,
        "context": "/document",
        "inputs": [
          {
            "name": "text",
            "source": "/document/content"
          }
        ],
        "outputs": [
          {
            "name": "text",
            "targetName": "ModeratedText"
          }
        ]
      }
  ]
}
```

## Index

```json
{
  "fields": [
    {
      "name": "id",
      "type": "Edm.String",
      "key": true,
      "searchable": true,
      "filterable": false,
      "facetable": false,
      "sortable": true
    },
     {
      "name": "blob_uri",
      "type": "Edm.String",
      "searchable": true,
      "filterable": false,
      "facetable": false,
      "sortable": true
    },
    {
      "name": "content",
      "type": "Edm.String",
      "sortable": false,
      "searchable": true,
      "filterable": false,
      "facetable": false
    },
    {
      "name": "languageCode",
      "type": "Edm.String",
      "searchable": true,
      "filterable": false,
      "facetable": false
    },
    {
      "name": "keyPhrases",
      "type": "Collection(Edm.String)",
      "searchable": true,
      "filterable": false,
      "facetable": false
    },
    {
      "name": "organizations",
      "type": "Collection(Edm.String)",
      "searchable": true,
      "sortable": false,
      "filterable": false,
      "facetable": false
    },
        {
      "name": "ModeratedText",
      "type": "Edm.Boolean",
      "searchable": false,
      "sortable": false,
      "filterable": false,
      "facetable": false
    }
  ]
}
```

## Indexer

```json
{
  "name":"demoindexer",
  "dataSourceName" : "demodata",
  "targetIndexName" : "demoindex",
  "skillsetName" : "demoskillset",
  "fieldMappings" : [
        {
          "sourceFieldName" : "metadata_storage_path",
          "targetFieldName" : "id",
          "mappingFunction" :
            { "name" : "base64Encode" }
        },
        {
          "sourceFieldName" : "content",
          "targetFieldName" : "content"
        },
         {
          "sourceFieldName" : "metadata_storage_path",
          "targetFieldName" : "blob_uri"
        }
   ],
  "outputFieldMappings" :
  [
        {
          "sourceFieldName" : "/document/organizations",
          "targetFieldName" : "organizations"
        },
        {
          "sourceFieldName" : "/document/pages/*/keyPhrases/*",
          "targetFieldName" : "keyPhrases"
        },
        {
            "sourceFieldName": "/document/languageCode",
            "targetFieldName": "languageCode"
        },
         {
            "sourceFieldName": "/document/moderatedText",
            "targetFieldName": "moderatedText"
        }
  ],
  "parameters":
  {
      "maxFailedItems":-1,
      "maxFailedItemsPerBatch":-1,
      "configuration":
    {
        "dataToExtract": "contentAndMetadata",
         "imageAction": "generateNormalizedImages"
        }
  }
}
```

## Check Status

```http
GET https://[servicename].search.windows.net/indexers/demoindexer/status?api-version=2017-11-11-Preview
Content-Type: application/json
api-key: [api-key]
```

## Check the Moderated Content extracted

```http
GET https://[servicename].search.windows.net/indexes/demoindex/docs?search=*&$select=ModeratedText,blob_uri&api-version=2017-11-11-Preview
Content-Type: application/json
api-key: [api-key]
```

## Next Step

[Bots Lab](../../labs/lab-bot-business-documents.md) or
[Back to Read Me](../../README.md)