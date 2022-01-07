using Elasticsearch.Net;
using Nest;
using Nest.Specification.IndicesApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

#pragma warning disable S125 // Sections of code should not be commented out
#pragma warning disable S1481 // Unused local variables should be removed
namespace Sample
{
    public static class ElasticSearch
    {
        public static void Test()
        {

            var client = ElastisSearchExtensions.GetElasticClient();

            ElastisSearchExtensions.TestAnalyzer(client);

            //ElastisSearchExtensions.MappingPost(client);
            ElastisSearchExtensions.CreateIndexPost(client);
            ElastisSearchExtensions.IndexPosts(client);
            ElastisSearchExtensions.GetPost(client);

            //FTS Queries
            ElastisSearchExtensions.MatchAll(client);
            ElastisSearchExtensions.MatchNone(client);
            ElastisSearchExtensions.Match(client);
            ElastisSearchExtensions.MultiMatch(client); //match on multi fields
            ElastisSearchExtensions.MatchPhrase(client); //match in same order. all tokens in indexed term must be appear in input (thus not found for : "آموزش Core")
            ElastisSearchExtensions.MatchPhrasePrefix(client); //same as MatchPhrase but prefix for last word.
            ElastisSearchExtensions.MatchBoolPrefix(client);

            //Term-Level Queries
            ElastisSearchExtensions.Term(client); //not found for "Core" or even "ef core" (only exactly tokenized term like "ef" or "core" not both)
            ElastisSearchExtensions.Terms(client); //bool query with should (affects scoring) clauses of terms (returns documents that contains any exaclty terms)
            ElastisSearchExtensions.Prefix(client); //only one exactly term with prefix
            ElastisSearchExtensions.Exists(client);
            ElastisSearchExtensions.LongRange(client);
            ElastisSearchExtensions.Ids(client);
            ElastisSearchExtensions.Fuzzy(client);
            ElastisSearchExtensions.Bool(client);
            //ElastisSearchExtensions.MoreLikeThis(client);
            ElastisSearchExtensions.Highlight(client);
            ElastisSearchExtensions.Suggest(client);
            ElastisSearchExtensions.TermVectors(client);
            ElastisSearchExtensions.Others(client);

            //Other Methods
            ElastisSearchExtensions.MultiSearch(client);
            ElastisSearchExtensions.DocumentExists(client);
            ElastisSearchExtensions.Count(client);
            ElastisSearchExtensions.CreateDocument(client);
            ElastisSearchExtensions.UpdatePosts(client);
            ElastisSearchExtensions.Bulk(client);
            ElastisSearchExtensions.DeletePosts(client);


            //ElastisSearchExtensions.MappingArticle(client);
            ElastisSearchExtensions.CreateIndexArticle(client);
            ElastisSearchExtensions.IndexArticles(client);
            ElastisSearchExtensions.SearchArticles(client);

            ElastisSearchExtensions.DeleteIndices(client);

            ElastisSearchExtensions.DeleteIndices(client);
        }

        public static class ElastisSearchExtensions
        {
            private static ConnectionSettings connectionSettings;
            public static ConnectionSettings GetConnectionSettings()
            {
                if (connectionSettings != null)
                    return connectionSettings;

                #region IConnectionPool
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/connection-pooling.html
                //Elasticsearch.Net.SingleNodeConnectionPool        //pool for single node
                //Elasticsearch.Net.StaticConnectionPool            //static pool for many nodes 
                //Elasticsearch.Net.CloudConnectionPool             //single node pool for cloud
                //Elasticsearch.Net.SniffingConnectionPool          //load baalnce pool for many nodes
                //Elasticsearch.Net.StickyConnectionPool            //dont know - many nodes
                //Elasticsearch.Net.StickySniffingConnectionPool    //dont know - many nodes

                //var nodes = new Uri[]
                //{
                //	new Uri("http://myserver1:9200"),
                //	new Uri("http://myserver2:9200"),
                //	new Uri("http://myserver3:9200")
                //};
                //var pool = new StaticConnectionPool(nodes);
                #endregion

                #region IConnection
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/modifying-default-connection.html
                //Elasticsearch.Net.HttpConnection        //default implementation using HttpClient
                //Elasticsearch.Net.InMemoryConnection    //memory implemenation that never uses any IO
                #endregion

                var node = new Uri("http://localhost:9200"); //DevSkim: ignore DS137138
                var pool = new SingleNodeConnectionPool(node);
                var settings = new ConnectionSettings(pool);

                //The default index when no index has been explicitly specified and no default indices are specified for the given CLR type
                settings.DefaultIndex("default-index");
                settings.DefaultMappingFor<Post>(p => p
                    .IndexName("post-index")
                //.DisableIdInference()
                //.IdProperty("Id")
                //.IdProperty(p => p.Id)
                //.Ignore(p => p.LikeCount)
                //.PropertyName(p => p.LikeCount, "like-count")
                //.RelationName("relationName")
                //.RoutingProperty(p => p.RoutingProperty));
                );

                settings.DefaultMappingFor<Article>(p => p
                    .IndexName("article-index")
                );

                //throw exception instead of checking result.IsValid (except when result.SuccessOrKnownError is false)
                settings.ThrowExceptions();

                //Solve problem with my proxy
                settings.DisableAutomaticProxyDetection();

                settings.EnableDebugMode(callDetails =>
                {
                    //Console.WriteLine(callDetails.DebugInformation); //Below code display shorter summary that does not include tcp and threadpool statistics

                    Console.WriteLine("Request:");
                    Console.WriteLine($"{callDetails.HttpMethod} {callDetails.Uri}");
                    if (callDetails.RequestBodyInBytes != null)
                    {
                        var json = $"{Encoding.UTF8.GetString(callDetails.RequestBodyInBytes)}";

                        try
                        {
                            using var document = JsonDocument.Parse(json);
                            using var ms = new MemoryStream();
                            using var writer = new Utf8JsonWriter(ms, new JsonWriterOptions { Indented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                            document.WriteTo(writer);
                            writer.Flush();
                            json = Encoding.UTF8.GetString(ms.ToArray());
                        }
                        catch { }

                        Console.WriteLine(json);
                    }

                    Console.WriteLine();

                    Console.WriteLine("Response:");
                    Console.WriteLine($"Status: {callDetails.HttpStatusCode}");
                    if (callDetails.ResponseBodyInBytes != null)
                        Console.WriteLine($"{Encoding.UTF8.GetString(callDetails.ResponseBodyInBytes)}");

                    Console.WriteLine($"{new string('-', 30)}\n");
                    Console.WriteLine();
                });
                settings.OnRequestDataCreated(requestData =>
                {
                    //before Sending request;
                });

                //EnableDebugMode calls below methods (useful for development)
                //settings.PrettyJson();
                //settings.IncludeServerStackTraceOnError();
                //settings.DisableDirectStreaming();
                //settings.EnableTcpStats();
                //settings.EnableThreadPoolStats();

                #region Other Methods
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/configuration-options.html
                //----- ConnectionSettingsBase Methods -----
                //settings.DefaultIndex("index-name");
                //settings.DefaultFieldNameInferrer(func);
                //settings.DefaultDisableIdInference();
                //settings.DefaultMappingFor<TDocument>(selector);
                //----- ConnectionConfiguration Methods -----
                //settings.ConnectionLimit(connectionLimit);
                //settings.DeadTimeout(timeout);
                //settings.DisableDirectStreaming();
                //settings.DisablePing();
                //settings.EnableHttpCompression();
                //settings.GlobalHeaders(headers);
                //settings.GlobalQueryStringParameters(queryStringParameters);
                //settings.MaxDeadTimeout(timeout);
                //settings.MaximumRetries(maxRetries);
                //settings.MaxRetryTimeout(maxRetryTimeout);
                //settings.OnRequestCompleted(handler);
                //settings.OnRequestDataCreated(handler);
                //settings.PingTimeout(timeout);
                //settings.Proxy(proxyAddress, username, password);
                //settings.RequestTimeout(timeout);
                //settings.SniffLifeSpan(sniffLifeSpan);
                //settings.SniffOnConnectionFault();
                //settings.SniffOnStartup();
                //settings.TransferEncodingChunked();
                //settings.UserAgent(userAgent);
                #endregion

                return connectionSettings = settings;
            }

            private static ElasticClient elasticClient;
            public static ElasticClient GetElasticClient()
            {
                if (elasticClient != null)
                    return elasticClient;

                var settings = GetConnectionSettings();
                return elasticClient = new ElasticClient(settings);
            }

            #region Post
            public static void MappingPost(ElasticClient client)
            {
                var mappingResponse = client.Map<Post>(p => p
                    .Index("post-index")
                    .AutoMap()
                //.Properties(p => p
                //    .Number(p => p.Name(p => p.Id))
                //    .Text(p => p.Name(p => p.Title).Analyzer("standard").Boost(1.5))
                //    .Text(p => p
                //        .Name(p => p.Body)
                //        .Analyzer("standard")
                //        .Store(true)
                //        .Index(true)
                //        .Norms(true)
                //        .IndexPhrases(true)
                //        .IndexOptions(IndexOptions.Offsets)
                //        .TermVector(TermVectorOption.WithPositionsOffsetsPayloads))
                //    .Date(p => p.Name(p => p.PublishDate))
                //    .Number(p => p.Name(p => p.LikeCount).Type(NumberType.Integer)))
                );

                #region Indices (many of IndexName)
                //Indices indexName = Indices.Index<Post>(); //return IndexName and implicit cast to Indices
                //Indices indexName = typeof(Post);
                //Indices indexName = "post-index";
                #endregion

                #region RequestDescriptorBase (base class for all RequestDescriptors)
                //RequestDescriptors create (implictly convert to) Request. Therefore all RequestDescriptors has these methods
                //RequestDescriptorBase requestDescriptor = null;
                //requestDescriptor.RequestConfiguration(configurationSelector); //Specify settings for this request alone, handy if you need a custom timeout or want to bypass sniffing, retries
                //requestDescriptor.ErrorTrace();                                //Include the stack trace of returned errors.
                //requestDescriptor.Human();                                     //Return human readable values for statistics.
                //requestDescriptor.Pretty();                                    //Pretty format the returned JSON response.
                //requestDescriptor.FilterPath();                                //A comma-separated list of filters used to reduce the response.
                //requestDescriptor.SourceQueryString(sourcequerystring);        //The URL-encoded request definition. Useful for libraries that do not accept a request body for non-POST requests.
                #endregion

                #region PutMappingDescriptor => Create a PutMappingRequest (implictly converted)
                ////PutMappingDescriptor converted to PutMappingRequest
                //https://www.elastic.co/guide/en/elasticsearch/reference/master/indices-put-mapping.html
                //PutMappingDescriptor<Post> putMappingDescriptor = null;
                //PutMappingRequest<Post> putMappingRequest = null;
                //-------
                //putMappingDescriptor.AllIndices();                                //A shortcut into calling Index(Indices.All)
                //putMappingDescriptor.Index(Indices);                              //A comma-separated list of index names the mapping should be added to (supports wildcards); use `_all` or omit to add the mapping on all indices.
                //putMappingDescriptor.AutoMap();                                   //Convenience method to map as much as it can based on ElasticType attributes set on the type.
                //putMappingDescriptor.Timeout(timeout);                            //Explicit operation timeout
                //putMappingDescriptor.MasterTimeout(mastertimeout);                //Specify timeout for connection to master
                //putMappingDescriptor.Properties(propertiesSelector);              //Specifies the mapping properties
                //putMappingDescriptor.RuntimeFields(runtimeFieldsSelector);        //Specifies runtime fields for the mapping
                //putMappingDescriptor.RoutingField(routingFieldSelector);          //Specifies configuration for _routing parameter
                //putMappingDescriptor.SourceField(sourceFieldSelector);            //Specifies configuration for _source parameter
                //putMappingDescriptor.WriteIndexOnly();                            //When true, applies mappings only to the write index of an alias or data stream
                //-------
                //putMappingDescriptor.AllowNoIndices();
                //putMappingDescriptor.DateDetection();
                //putMappingDescriptor.NumericDetection();
                //putMappingDescriptor.IgnoreUnavailable();
                //putMappingDescriptor.DisableSizeField();
                //putMappingDescriptor.Dynamic();
                //putMappingDescriptor.DynamicDateFormats(dateFormats);
                //putMappingDescriptor.DynamicTemplates(dynamicTemplatesSelector);
                //putMappingDescriptor.ExpandWildcards(expandwildcards);
                //putMappingDescriptor.FieldNamesField(fieldNamesFieldSelector);
                //putMappingDescriptor.SizeField(sizeFieldSelector);
                //putMappingDescriptor.IncludeTypeName(includetypename);
                //putMappingDescriptor.Meta(metaDictionary);
                #endregion

                #region Mapping
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/auto-map.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/attribute-mapping.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/fluent-mapping.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/visitor-pattern-mapping.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/parent-child-relationships.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/ignoring-properties.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/multi-fields.html

                //var mappingResponse = client.Map<Post>(p => p
                //    .Index("post-index")
                //    .AutoMap(maxRecursion: 0)
                //    .Properties(p => p
                //        .Text(p => p.Name(p => p.Title).Analyzer("analyzer"))
                //        .Number(p => p.Name(p => p.LikeCount).Type(NumberType.Integer))
                //        .Binary()
                //        .Boolean()
                //        .Nested()
                //        .Object()
                //        .Date()
                //        .DateNanos()
                //        .Ip()
                //        ---- -
                //        .IntegerRange()
                //        .LongRange()
                //        .FloatRange()
                //        .DoubleRange()
                //        .DateRange()
                //        .IpRange()
                //        ---- -
                //        .Join()
                //        .Keyword()
                //        .ConstantKeyword()
                //        .RankFeature()
                //        .RankFeatures()
                //        .Scalar()
                //        .Completion()
                //        .SearchAsYouType()
                //        .TokenCount()
                //        .Version()
                //        .Wildcard()));
                #endregion
            }

            public static void CreateIndexPost(ElasticClient client)
            {
                #region Create IndexName and Indices
                var indexName1 = IndexName.From<Post>();
                var indexName2 = (IndexName)typeof(Post);
                var indexName3 = (IndexName)"post-index";
                var indexName4 = Infer.Index<Post>();

                var indices1 = (Indices)Indices.Index<Post>();
                var indices2 = (Indices)Indices.Index("post-index");
                var indices3 = (Indices)"post-index";
                var indices4 = (Indices)typeof(Post);
                var indices5 = Infer.Indices<Post>();
                var indices6 = Indices.Index("index1", "index2");
                var indices7 = Indices.Index(new[] { "index1", "index2" });
                #endregion

                var postIndex = IndexName.From<Post>();

                //### Delete
                var deleteResponse = client.Indices.Delete(postIndex);
                var deleted = deleteResponse.Acknowledged && deleteResponse.IsValid;

                //### Create Index
                var createIndexResponse = client.Indices.Create(postIndex, p => p
                    .Map<Post>(p => p.AutoMap()
                    //.AutoMap<Post>()
                    //.AutoMap(typeof(Post))
                    //.Properties(p => p
                    //    .Number(p => p.Name(p => p.Id))
                    //    .Text(p => p.Name(p => p.Title).Analyzer("standard").Boost(1.5))
                    //    .Text(p => p
                    //        .Name(p => p.Body)
                    //        .Analyzer("standard")
                    //        .Store(true)
                    //        .Index(true)
                    //        .Norms(true)
                    //        .IndexPhrases(true)
                    //        .IndexOptions(IndexOptions.Offsets)
                    //        .TermVector(TermVectorOption.WithPositionsOffsetsPayloads))
                    //    .Date(p => p.Name(p => p.PublishDate))
                    //    .Number(p => p.Name(p => p.LikeCount).Type(NumberType.Integer)))
                    //)
                    )
                );

                //### Exits
                var existsResponse = client.Indices.Exists(postIndex, p => p
                //.Index("override-index") // override index name
                //.Index<Post>() //override index name
                );
                var exists = existsResponse.Exists;

                //### Settings
                var settingsResponse = client.Indices.GetSettings(postIndex);  //   /post-index/_settings
                var dictionaryOfIndexAndSettings = settingsResponse.Indices;

                //### Mapping
                var getMappingResponse = client.Indices.GetMapping<Post>(); //   /post-index/_mapping
                var dictionaryOfIndexAndMapping = getMappingResponse.Indices;

                #region Client.Indices Methods (IndicesNamespace)
                //https://www.elastic.co/guide/en/elasticsearch/reference/master/indices.html
                //IndicesNamespace indicesNamespace = null;
                //indicesNamespace.Create("index-name", selector);
                //indicesNamespace.Delete(Indices: "index-name, ...", selector);
                //indicesNamespace.Exists(Indices: "index-name, ...", selector);
                //indicesNamespace.GetAsync(Indices: "index-name, ...", selector);
                //indicesNamespace.Close(Indices : "index-name, ...", selector);
                //indicesNamespace.Open(Indices : "index-name, ...", selector);
                //indicesNamespace.Flush(Indices: "index-name, ...", selector);
                //indicesNamespace.GetMapping(selector);
                //indicesNamespace.PutMapping(selector);
                //indicesNamespace.GetFieldMapping(Fields, selector);
                //indicesNamespace.GetSettings(Indices index = null);
                //And many other methods
                #endregion
            }

            public static void IndexPosts(ElasticClient client)
            {
                var posts = new Post[]
                {
                new Post(101, "آموزش Entity Framework",       "در این مقاله به آموزش Entity Framework می پردازیم",        new DateTime(2021, 01, 01), 11),
                new Post(102, "آموزش ASP.NET MVC",            "در این مقاله به آموزش ASP.NET MVC می پردازیم",             new DateTime(2021, 01, 02), 12),
                new Post(103, "آموزش EF Core",                "در این مقاله به آموزش EF Core می پردازیم",                 new DateTime(2021, 01, 03), 13),
                new Post(104, "آموزش ASP.NET Core",           "در این مقاله به آموزش ASP.NET Core می پردازیم",            new DateTime(2021, 01, 04), 14),
                new Post(105, "آموزش Domain Driven Design",   "در این مقاله به آموزش Domain Driven Design می پردازیم",    new DateTime(2021, 01, 05), 15),
                new Post(106, "آموزش Microservices",          "در این مقاله به آموزش Microservices می پردازیم",           new DateTime(2021, 01, 06), 16),
                new Post(107, "از به با برای C#",             "در این مقاله به آموزش C# می پردازیم",                      new DateTime(2021, 01, 07), 17),
                new Post(108, "عنوان تستی",                   "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد.",                   new DateTime(2021, 01, 09), 19),
                new Post(109, "more like this",                "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد. کتابهای زیادی در شصت و سه درصد گذشته، حال و آینده شناخت فراوان جامعه و متخصصان را می طلبد تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی و فرهنگ پیشرو در زبان فارسی ایجاد کرد. در این صورت می توان امید داشت که تمام و دشواری موجود در ارائه راهکارها و شرایط سخت تایپ به پایان رسد وزمان مورد نیاز شامل حروفچینی دستاوردهای اصلی و جوابگوی سوالات پیوسته اهل دنیای موجود طراحی اساسا مورد استفاده قرار گیرد.",                   new DateTime(2021, 01, 09), 19),
                };

                var post1 = posts[0];
                var post2 = posts[1];

                #region Id
                Id id1 = post1.Id;              //implicit cast from long
                Id id2 = "string";              //implicit cast from string
                Id id3 = Guid.NewGuid();        // implicit cast from Guid
                Id id4 = Id.From(post1);        //implicit cast from object (equals to new Id(post1))
                #endregion

                var indexName = IndexName.From<Post>();

                #region IndexDescriptor<TDocument> => Create a IndexRequest<TDocument> (implictly converted)
                var id = Id.From(post1);
                IndexDescriptor<Post> indexDescriptor1 = new IndexDescriptor<Post>();                       // /{index}/_doc (retrive indexName from (IndexName)typeof(TDocument))
                IndexDescriptor<Post> indexDescriptor2 = new IndexDescriptor<Post>(indexName);              // /{index}/_doc
                IndexDescriptor<Post> indexDescriptor3 = new IndexDescriptor<Post>(id);                     // /{index}/_doc/{id} (retrive indexName from (IndexName)typeof(TDocument))
                IndexDescriptor<Post> indexDescriptor4 = new IndexDescriptor<Post>(indexName, id);          // /{index}/_doc/{id}
                IndexDescriptor<Post> indexDescriptor5 = new IndexDescriptor<Post>(post1);                  // /{index}/_doc/{id}
                IndexDescriptor<Post> indexDescriptor6 = new IndexDescriptor<Post>(post1, indexName, id);   // /{index}/_doc/{id}
                #endregion

                //### Index Post
                var indexResponse1 = client.Index(post1, p => p.Index<Post>()
                //.Index(IndexName)
                //.Index("post-index")
                //.Id(Id)
                //.Id(post1)
                //.Refresh(Refresh.True) //Refresh.WaitFor 
                ); //or specify default index via settings.DefaultIndex("default-index");
                //به دلیل ایوانچوال کانسیستنسی رکورد هایی که ایندکس میشن در همون لحظه قابل واکشی نیستند مگر این رفرش رو مقدار دهی کنیم

                //index duplicate post cause Update with increased version
                //var indexResponse1 = client.Index(post2, p => p);

                //IndexResponse
                var isValid = indexResponse1.IsValid;

                //WriteResponseBase
                var documentId = indexResponse1.Id;
                var index = indexResponse1.Index;
                var whatIsSequence = indexResponse1.SequenceNumber;
                var result = indexResponse1.Result;

                //ResponseBase
                var apiCallDetails = indexResponse1.ApiCall;
                var debugInformation = indexResponse1.DebugInformation;
                var originalException = indexResponse1.OriginalException;
                var serverError = indexResponse1.ServerError;

                var indexResponse2 = client.IndexDocument(post2); //Equals to client.Index(post2, p => p) (Useful when defaul index name is set)

                //### Index Many
                var remainedPosts = posts[2..];
                var bulkResponse = client.IndexMany(remainedPosts/*, IndexName: "post-index" */);

                //BulkResponse
                var items = bulkResponse.Items;
                var itemsWithError = bulkResponse.ItemsWithErrors;
                var errors = bulkResponse.Errors;
                var took = bulkResponse.Took;
            }

            public static void GetPost(ElasticClient client)
            {
                #region DocumentPath<Post> : IDocumentPath (combination of IndexName and Id)
                //var documentPath1 = DocumentPath<Post>.Id(Id);      //new DocumentPath<Post>(Id)   or imcplic cast DocumentPath<Post> = Id;
                //var documentPath2 = DocumentPath<Post>.Id(Post);    //new DocumentPath<Post>(Post) or imcplic cast DocumentPath<Post> = post;
                var documentPath3 = (DocumentPath<Post>)101;
                var documentPath4 = (DocumentPath<Post>)"101";
                var documentPath5 = (DocumentPath<Post>)Guid.NewGuid();
                #endregion

                //### Get
                //Returns an IGetResponse mapped 1-to-1 with the Elasticsearch JSON response
                var foundResponse1 = client.Get<Post>(101, p => p
                //.Index("post-index")
                //.Index<Post>()
                //.StoredFields(p => p.Title) //retrive fields if store=true
                //.SourceIncludes(p => p.Title)
                //.SourceExcludes(p => p.Title)
                .SourceEnabled(false)
                );

                var id = foundResponse1.Id;
                var index = foundResponse1.Index;
                var found = foundResponse1.Found;
                var foundPost = foundResponse1.Source; // the original document
                var fields = foundResponse1.Fields;
                var whatIsSequence = foundResponse1.SequenceNumber;

                #region Field
                //Constructors are same
                Field field1 = "Title";
                Field field2 = typeof(Post).GetProperty("Title");
                Field field3 = (Expression<Func<Post, string>>)(p => p.Title);
                Field field4 = Infer.Field<Post>(p => p.Title);
                Field field5 = Infer.Field("Title");
                //When new instantiate can set (boost: 1.5) and (format: "{0:n0}");
                #endregion

                #region Fields : IEnumerable<Field>
                Fields fields1 = "Title";                   //single field name
                Fields fields2 = "Title, Body";             //comma-seprated field names
                Fields fields3 = new[] { "Title", "Body" }; //string array of field names
                //Fields fields4 = Expression and Expression[] of fileds
                //Fields fields5 = PropertyInfo and PropertyInfo[] of fields
                //Fields fields6 = Field[]
                #endregion

                var foundResponse2 = client.Get<Post>(101, p => p
                //Whether the _source should be included in the response
                //.SourceEnabled(false)

                //Include fields
                .SourceIncludes(p => p.Title, p => p.Body)
                //.SourceIncludes(new[] { "Title", "Body" })

                //Exclude fields
                //.SourceExcludes(p => p.PublishDate, p => p.LikeCount)
                //.SourceExcludes(new[] { "PublishDate", "LikeCount" })

                //.StoredFields(p => p.Title, p => p.Body)
                //.StoredFields(new[] { "Title", "Body" })
                );

                //https://www.elastic.co/guide/en/elasticsearch/reference/master/docs-multi-get.html
                //### GetMany
                var multiGetHits1 = client.GetMany<Post>(new long[] { 101, 102 }/*, IndexName: "post-index" or IndexName.From<Post>()*/).ToList();

                //#### MultiGet
                var multiGetResponse1 = client.MultiGet(p => p
                    .Get<Post>(p => p.Id(101))
                    .Get<Post>(p => p.Id(102))
                    .GetMany<Post>(new long[] { 103, 104 })
                );

                var multiGetHit = multiGetResponse1.Get<Post>(101);
                var post = multiGetResponse1.Source<Post>(101); //Equals to Get<T>(id).Source
                var fieldValues = multiGetResponse1.GetFieldSelection<Post>(101); //Equals to .GetFieldValues<Post>("101"); that performs Get<T>(id)?.Fields ?? FieldValues.Empty;
                var multiGetHits = multiGetResponse1.GetMany<Post>(new long[] { 103, 104 });
                var posts = multiGetResponse1.SourceMany<Post>(new long[] { 103, 104 }); //from hit in GetMany<T>(ids) where hit.Found select hit.Source;

                //var multiGetResponse2 = client.MultiGet(p => p
                //    .SourceEnabled(false)
                //
                //    .SourceIncludes<Post>(p => p.Title, p => p.Body)
                //    .SourceIncludes(new[] { "Title", "Body" })
                //
                //    .SourceExcludes<Post>(p => p.PublishDate, p => p.LikeCount)
                //    .SourceExcludes(new[] { "PublishDate", "LikeCount" })
                //
                //    .StoredFields<Post>(p => p.Fields(p => p.Body, p => p.Body))
                //    .StoredFields<Post>(p => p.Fields("Title", "Body"))
                //    .StoredFields(new[] { "Title", "Body" })
                //
                //    .Index("post-index")
                //    .Index<Post>()
                //
                //    .GetMany<Post>(longArr1)
                //    .GetMany<Post>(longArr1, (p, id) => p
                //        .Index("post-index")
                //        .Source(true)
                //        .StoredFields(new[] { "Title", "Body" })
                //        .StoredFields(p => p
                //            .Field(p => p.Title)
                //            .Field(p => p.Body)
                //        )
                //     )
                //);
            }

            public static void MatchAll(ElasticClient client)
            {
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/full-text-queries.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/full-text-queries.html
                //https://opendistro.github.io/for-elasticsearch-docs/docs/elasticsearch/full-text/

                #region Common Query Methods
                //var searchResponse = client.Search<Post>(s => s
                //    .Query(q => q
                //        //.MatchAll()
                //        //---------------- Full-text Queries
                //        //.Match(selector)
                //        //.MatchPhrase(selector)
                //        //.MultiMatch(selector)
                //        //.MatchPhrasePrefix(selector)
                //        //.MatchBoolPrefix(selector)
                //        //.MatchNone(selector)
                //        //.QueryString(selector)
                //        //.SimpleQueryString(selector)
                //        //.CommonTerms(selector)
                //        //.Intervals(selector)
                //        //---------------- Term-level Queries
                //        //.Term(selector)
                //        //.Terms(selector)
                //        //.TermsSet(selector)
                //        //.Prefix(selector)
                //        //.Exists(selector)
                //        //.Fuzzy(selector)
                //        //.FuzzyDate(selector)
                //        //.FuzzyNumeric(selector)
                //        //.Range(selector)
                //        //.LongRange(selector)
                //        //.DateRange(selector)
                //        //.TermRange(selector)
                //        //.Ids(selector)
                //        //.Wildcard(selector)
                //        //.Regexp(selector)
                //        //---------------- Compound Queries
                //        //.Bool(selector)
                //        //.Boosting(selector)
                //        //----- Specialized Queries
                //        //.MoreLikeThis(selector)
                //        //---------------- NEST Specific
                //        //.Raw(rawJson)
                //        //---------------- Dont Know
                //        //.Nested(selector)
                //        //.HasChild(selector)
                //        //.HasParent(selector)
                //        //.ParentId(selector)
                //        //.HasRelationName(field)
                //    )
                //);
                #endregion

                #region Term vs MatchPhrase vs QueryString
                //https://stackoverflow.com/questions/26001002/elasticsearch-difference-between-term-match-phrase-and-query-string
                //https://www.devinline.com/2018/01/full-text-query-in-elasticsearch-using-match.html
                //https://qbox.io/blog/elasticsearch-queries-match-phrase-match/
                //https://opendistro.github.io/for-elasticsearch-docs/docs/elasticsearch/full-text/
                //https://opendistro.github.io/for-elasticsearch-docs/docs/elasticsearch/bool/
                //https://opendistro.github.io/for-elasticsearch-docs/docs/elasticsearch/term/
                //https://stackoverflow.com/questions/42451527/difference-between-match-and-multimatch-query-type-in-elasticsearch

                //MatchAll:         analyzed        - select all
                //Match:            anylyzed        - match if one term matched - effect or score (more matched terms results in better score)
                //Term:             NOT analyzed    - match exactly
                //MatchPhrase:      analyzed        - matched only if the terms come in the same order - all the terms must appear in the field as the same order as the input value
                //QueryString:      query in elasticsearch syntax
                //Prefix            NOT analyzed    - match exactly (similar to term) but start with specified prefix
                //MatchPhrasePrefix analyzed        - similar to MatchPhrase but start with specified prefix
                //MultiMatch        analyzed        - similar to Match but applies on multiple fields
                //Bool              applies multiple queris - Must (AND) - Should (OR) - MustNot (NOT) - Filter (similar to must but not effect on score)
                //Range, LongRange, DateRange, TermRange

                //====== Most Important Queries ======
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/full-text-queries.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-bool-prefix-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-query-phrase.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-query-phrase-prefix.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-multi-match-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-common-terms-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-simple-query-string-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-all-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-bool-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/term-level-queries.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-term-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-terms-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-exists-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-fuzzy-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-prefix-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-terms-set-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-type-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-wildcard-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-mlt-query.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-filter-context.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/filter-search-results.html
                //Other
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/paginate-search-results.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/sort-search-results.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/search-fields.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/highlighting.html

                //====== Cheat Sheet ======
                //http://moliware.com/es-dsl-cheatsheet/ [BEST]
                //https://elasticsearch-cheatsheet.jolicode.com/
                //https://gist.github.com/ruanbekker/e8a09604b14f37e8d2f743a87b930f93#query
                //https://www.bmc.com/blogs/elasticsearch-commands/ | https://logz.io/blog/elasticsearch-cheat-sheet/
                //https://www.john-cd.com/cheatsheets/Search/ElasticSearch/#query-dsl
                #endregion

                #region Common Aggregations Methods
                //var searchResponse = client.Search<Post>(s => s
                //    .Aggregations(p => p
                //        //---------------- Buket Aggregations (Group By)
                //        //.Range("name", selector)
                //        //.DateRange("name", selector)
                //        //.Filter("name", selector)
                //        //.Filters("name", selector)
                //        //.Terms("name", selector)
                //        //.MultiTerms("name", selector)
                //        //.Nested("name", selector)
                //        //.ReverseNested("name", selector)
                //        //.Parent("name", selector)
                //        //.Children("name", selector)
                //        //---------------- Metrics Aggregations
                //        //.Min("name", selector) //MinBucket
                //        //.Max("name", selector) //MaxBucket
                //        //.Average("name", selector) //AverageBucket, MovingAverage, WeightedAverage
                //        //.Sum("name", selector) //SumBucket, CumulativeSum
                //        //.Stats("name", selector) //StatsBucket
                //        //.StringStats("name", selector) //Bucket
                //        //.ValueCount("name", selector)
                //        //And many other methods
                //    )
                //);
                #endregion

                #region Common Search Methods
                //var searchResponse = client.Search<Post>(s => s
                //    //------------------- Commonly Used Methods
                //    .From(0)
                //    .Skip(0)
                //    .Size(10)
                //    .Take(10)
                //    .Fields(Fields)
                //    .Sort(selector)
                //    .Highlight(highlightSelector)
                //    .Explain(true)
                //    .Human(true)
                //    .Pretty(true)
                //    .Profile(true)
                //    .ErrorTrace(true)
                //    .Query(query)
                //    .QueryOnQueryString(string)
                //    .DocValueFields(fields)
                //    .Source(true)
                //    .Source(selector)
                //    .SourceQueryString(string)
                //    .StoredFields(Fields)
                //    .Suggest(selector)
                //    .SuggestField(Field)
                //    .SuggestMode(SuggestMode.Popular)
                //    .SuggestSize(long ?)
                //    .SuggestText(string)
                //    //------------------- Other Methods
                //    .Slice(selector)
                //    .Scroll(Tiem)
                //    .AllIndices()
                //    .AllowNoIndices()
                //    .Analyzer("standard")
                //    .AnalyzeWildcard(true)
                //    .BatchedReduceSize(long ?)
                //    .Collapse(collapseSelector)
                //    .DefaultOperator(DefaultOperator.And)
                //    .ExecuteOnLocalShard()
                //    .ExecuteOnNode("node")
                //    .ExecuteOnPreferredNode("node")
                //    .ExpandWildcards(ExpandWildcards.All)
                //    .FilterPath(string[])
                //    .IndicesBoost(boost)
                //    .MinScore(double ?)
                //    .PointInTime(string)
                //    .PostFilter(filter)
                //    .Preference(string)
                //    .RequestCache(true)
                //    .RequestConfiguration(configurationSelector)
                //    .Routing(new Routing(long))
                //    .RuntimeFields(runtimeFieldsSelector)
                //    .ScriptFields(selector)
                //    .SearchAfter(string[])
                //    .SearchType(SearchType.QueryThenFetch)
                //    .SequenceNumberPrimaryTerm(true)
                //    .Stats(string[])
                //    .TerminateAfter(long ?)
                //    .Timeout(string)
                //    .TotalHitsAsInteger(true)
                //    .TrackScores(true)
                //    .TrackTotalHits(true)
                //    .TypedKeys(true)
                //    .Version(true)
                //    .Rescore(rescoreSelector)
                //    .PreFilterShardSize(long ?)
                //    .IgnoreThrottled(true)
                //    .IgnoreUnavailable(true)
                //);
                #endregion

                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-all-query.html

                var result1 = client.Search<Post>(); //Search all post same as match all

                var searchResponseAll = client.Search<Post>(s => s
                    .Index("post-index") //or specify index via settings.DefaultIndex("index-name");
                    .From(0) // or .Skip(0)
                    .Size(5) //or .Take(5)
                    .Query(q => q
                        .MatchAll(/*p => p.Name("query-name")*/)
                    )
                );

                var documents = searchResponseAll.Documents;
                var hits = searchResponseAll.Hits;

                if (searchResponseAll.Hits.Count > 0)
                {
                    var hit0 = searchResponseAll.Hits.ElementAt(0);
                    var hit0Id = hit0.Id;
                    var hit0Index = hit0.Index;
                    var hit0MatchedQueries = hit0.MatchedQueries;
                    var hit0Score = hit0.Score;
                    var hit0Sorts = hit0.Sorts;
                    var hit0Highlight = hit0.Highlight;
                    var hit0Fields = hit0.Fields;
                }

                var hitsMetadata = searchResponseAll.HitsMetadata;
                var maxScore = searchResponseAll.MaxScore;
                var total = searchResponseAll.Total;
                var aggregations = searchResponseAll.Aggregations;
                var fields = searchResponseAll.Fields;
                var suggest = searchResponseAll.Suggest;

                #region IResponse, IElasticsearchResponse
                ////A lazily computed, human readable string representation of what happened during
                ////a request for both successful and failed requests. Useful whilst developing or
                ////to log when Nest.IResponse.IsValid is false on responses.
                //searchResponseAll.DebugInformation;
                //searchResponseAll.OriginalException; //If the request resulted in an exception on the client side
                //searchResponseAll.ServerError; //If the response results in an error on Elasticsearch's server
                //searchResponseAll.ApiCall; //Sets and returns the IApiCallDetails diagnostic information
                //searchResponseAll.TryGetServerErrorReason(out string reason);
                #endregion

                #region ISearchResponse
                //ISearchResponse<Post> searchResponse = null;
                //searchResponse.Documents;               //(IReadOnlyCollection<TDocument>) Gets the documents inside the hits, by deserializing Nest.IHitMetadata`1.Source into TDocument
                //searchResponse.Fields;                  //(IReadOnlyCollection<FieldValues>)Gets the field values inside the hits, when the search request uses Nest.SearchRequest.StoredFields.
                //searchResponse.HitsMetadata;            //(IHitsMetadata<TDocument>) Gets the meta data about the hits that match the search query criteria.
                //searchResponse.Hits;                    //(IReadOnlyCollection<IHit<TDocument>>) Gets the collection of hits that matched the query
                //searchResponse.MaxScore;                //(double) Gets the maximum score for documents matching the search query criteria
                //searchResponse.Total;                   //(long) Gets the total number of documents matching the search query criteria
                //searchResponse.Aggregations;            //(AggregateDictionary) Gets the collection of aggregations
                //searchResponse.Suggest;                 //(ISuggestDictionary<TDocument>) Gets the suggester results.
                //searchResponse.Took;                    //(long) Time in milliseconds for Elasticsearch to execute the search
                //-------
                //searchResponse.Clusters;                //(ClusterStatistics) Gets the statistics about the clusters on which the search query was executed.
                //searchResponse.Shards;                  //(ShardStatistics) Gets the statistics about the shards on which the search query was executed.
                //searchResponse.NumberOfReducePhases;    //(long) Number of times the server performed an incremental reduce phase
                //searchResponse.Profile;                 //(Profile) Gets the results of profiling the search query. Has a value only when ISearchRequest.Profile is set to true on the search request
                //searchResponse.ScrollId;                //(string) Gets the scroll id which can be passed to the Scroll API in order to retrieve the next batch of results. Has a value only when Nest.SearchRequest.Scroll is specified on the search request
                //searchResponse.PointInTimeId;           //(string) When a search is made over a point in time, this will be the ID of the point in time.
                //searchResponse.TerminatedEarly;         //(bool) Gets a value indicating whether the search was terminated early
                //searchResponse.TimedOut;                //(bool) Gets a value indicating whether the search timed out or not
                #endregion

                #region FieldValues
                //FieldValues fieldValues = null;
                //fieldValues.Value<Post>(Field);
                //fieldValues.ValuesOf<Post>(field);
                //fieldValues.Values<Post>(expression);
                //fieldValues.ValueOf<Post>(expression);
                #endregion

                #region IHit
                //IHit<Post> hit = null;
                //hit.Source;         //(TDocument) The source document for the hit
                //hit.Score;          //(double?) The score for the hit in relation to the query
                //hit.Fields;         //The individual fields requested for a hit
                //hit.Highlight;      //(FieldValues) The field highlights
                //hit.InnerHits;      //The inner hits
                //hit.Nested;         //(NestedIdentity)
                //-------
                //hit.Id;             //(string) The id of the hit
                //hit.Index;          //The index in which the hit resides
                //hit.MatchedQueries; //string[] Which queries the hit is a match for, when a compound query is involved and named queries used
                //hit.Sorts;          //object[] The sequence number for this hit
                //hit.PrimaryTerm;    //(long?) The primary term of the hit
                //hit.Routing;        //(string) The routing value for the hit
                //hit.SequenceNumber; //(long?) The sequence number for this hit
                //hit.Type;           //(string) The type of hit
                //hit.Version;        //(long) The version of the hit
                //hit.Explanation;    //(Explanation) An explanation for why the hit is a match for a query
                #endregion

                #region AggregateDictionary
                //AggregateDictionary aggregate = null;
                //aggregate.Average()
                //aggregate.AverageBucket()
                //aggregate.Children()
                //aggregate.Composite()
                //aggregate.ContainsKey()
                //aggregate.DateRange()
                //aggregate.Filter()
                //aggregate.Filters()
                //aggregate.Max()
                //aggregate.MaxBucket()
                //aggregate.Min()
                //aggregate.MinBucket()
                //aggregate.MultiTerms()
                //aggregate.Nested()
                //aggregate.Normalize()
                //aggregate.Parent()
                //aggregate.PercentileRanks()
                //aggregate.Percentiles()
                //aggregate.PercentilesBucket()
                //aggregate.Range()
                //aggregate.RareTerms()
                //aggregate.Rate()
                //aggregate.ReverseNested()
                //aggregate.Sampler()
                //aggregate.SignificantTerms()
                //aggregate.SignificantText()
                //aggregate.Sum()
                //aggregate.SumBucket()
                //aggregate.Terms()
                //aggregate.TopHits()
                //aggregate.TryGetValue()
                //aggregate.ValueCount()
                //aggregate.WeightedAverage()
                //aggregate.Global()
                #endregion

                #region QueryDescriptorBase (Common) Methods
                //QueryDescriptorBase queryDescriptorBase = null;
                //Class QueryDescriptorBase is the base class for all query descriptor. therefor below methods can be set on any query
                //QueryDescriptorBase.Name("query-name")    //name of the query to retrive based on this from result
                //QueryDescriptorBase.Boost(1.5)            //boosting query
                //QueryDescriptorBase.Strict(bool strict)
                //QueryDescriptorBase.Verbatim(bool verbatim)
                #endregion

                #region Inferrer (Client.Infer)
                var str1 = client.Infer.IndexName<Post>();
                var str2 = client.Infer.IndexName("post-index");
                var str3 = client.Infer.PropertyName(typeof(Post).GetProperty("Title"));
                var str4 = client.Infer.Field("Title");
                var str5 = client.Infer.Id<Post>(new(102));
                var str6 = client.Infer.Id(typeof(Post), new Post(102));

                //client.Infer.Field(Field);              //Uses FieldResolver to resolve field name of the Field
                //client.Infer.Id<Post>(post);            //Uses IdResolver to resolve field name of the Id property
                //client.Infer.Id(typeof(Post), post);    //Uses IdResolver to resolve field name of the Id property
                //client.Infer.IndexName(IndexName);      //Uses IndexNameResolver to resolve name of the Index (accept IndexName, string, Type)
                //client.Infer.IndexName<Post>();         //Uses IndexNameResolver to resolve name of the Index
                //client.Infer.PropertyName(PropertyName);//Uses FieldResolver
                //client.Infer.RelationName(RelationName);//Uses RelationNameResolver
                //client.Infer.RelationName<Post>();      //Uses RelationNameResolver
                //client.Infer.Routing(typeof(Post), post);//Uses RoutingResolver
                //client.Infer.Routing<Post>(post);       //Uses RoutingResolver
                //client.Infer.Resolve(IUrlParameter);    //Uses urlParameter.GetString(_connectionSettings);
                #endregion
            }

            public static void MatchNone(ElasticClient client)
            {
                var searchResponse = client.Search<Post>(s => s
                    .Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .MatchNone()
                    )
                );
            }

            public static void Match(ElasticClient client)
            {
                //Returns documents that match a provided text, number, date or boolean value. The provided text is analyzed before matching.
                //The match query is the standard query for performing a full - text search, including options for fuzzy matching.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/match-usage.html

                var searchResponse = client.Search<Post>(s => s
                  //.Index("post-index")
                  .From(0)
                  .Size(10)
                  .Query(q => q
                      .Match(p => p
                          .Field(f => f.Title) //.Field("title")
                          .Query("ASP.NET Core")
                      //.Fuzziness(Fuzziness.Auto)
                      //.FuzzyTranspositions()
                      //.Operator(Operator.And) //Operator.Or
                      //.Analyzer("standard")
                      //.Boost(1.5)
                      //.MinimumShouldMatch(MinimumShouldMatch.Fixed(2))  //MinimumShouldMatch.Percentage(60)
                      //.AutoGenerateSynonymsPhraseQuery()
                      )
                  )//.Explain()
              );
            }

            public static void MultiMatch(ElasticClient client)
            {
                //The multi_match query builds on the match query to allow multi-field queries.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-multi-match-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/multi-match-usage.html

                var searchResponse = client.Search<Post>(s => s
                    //.Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .MultiMatch(p => p
                            //.Fields(new[] { "title", "body"})
                            //.Fields("title, body")
                            //.Fields(p => p.Fields(new[] { "title", "body" }))
                            //.Fields(p => p.Fields(p => p.Title, p => p.Body))
                            //.Fields(p => p.Field(p => p.Title).Field(p => p.Body))
                            .Fields(p => p.Fields(new[] { "title", "body" }))
                            .Query("Core")
                        //.Operator(Operator.And) //Operator.Or
                        //.Analyzer("standard")
                        //.Boost(1.5)
                        //.MinimumShouldMatch(MinimumShouldMatch.Percentage(60)) //MinimumShouldMatch.Fixed(2)
                        )
                    )
                );
            }

            public static void MatchPhrase(ElasticClient client)
            {
                //Returns documents that contain all of the terms as a same order as input value.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-query-phrase.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/match-phrase-usage.html

                var searchResponse = client.Search<Post>(s => s
                    //.Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .MatchPhrase(p => p
                            .Field(p => p.Title) //.Field("title")
                            .Query("آموزش EF")
                        //.Operator(Operator.And)
                        //.Operator(Operator.Or)
                        //.Analyzer("standard")
                        //.Boost(1.5)
                        //.MinimumShouldMatch(MinimumShouldMatch.Percentage(60)) //MinimumShouldMatch.Fixed(2)
                        )
                    )
                );
            }

            public static void MatchPhrasePrefix(ElasticClient client)
            {
                //Returns documents that contain the words of a provided text, in the same order as provided. The last term of the provided text is treated as a prefix, matching any words that begin with that term.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-query-phrase-prefix.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/match-phrase-prefix-usage.html

                var searchResponse = client.Search<Post>(s => s
                    //.Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .MatchPhrasePrefix(p => p
                            .Field(p => p.Title) //.Field("title")
                            .Query("آموزش E")
                        //.Operator(Operator.And)
                        //.Operator(Operator.Or)
                        //.Analyzer("standard")
                        //.Boost(1.5)
                        //.MinimumShouldMatch(MinimumShouldMatch.Percentage(60)) //MinimumShouldMatch.Fixed(2)
                        )
                    )
                );
            }

            public static void MatchBoolPrefix(ElasticClient client)
            {
                //A match_bool_prefix query analyzes its input and constructs a bool query from the terms. Each term except the last is used in a term query. The last term is used in a prefix query.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-bool-prefix-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/match-bool-prefix-usage.html

                var searchResponse = client.Search<Post>(s => s
                    //.Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .MatchBoolPrefix(p => p
                            .Field(p => p.Title) //.Field("title")
                            .Query("ASP.NET Cor")
                        //.Operator(Operator.And)
                        //.Operator(Operator.Or)
                        //.Analyzer("standard")
                        //.Boost(1.5d)
                        //.MinimumShouldMatch(MinimumShouldMatch.Percentage(60)) //MinimumShouldMatch.Fixed(2)
                        )
                    )
                );
            }

            public static void Term(ElasticClient client)
            {
                //Returns documents that contain an exact term in a provided field.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-term-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/term-query-usage.html

                var searchResponse = client.Search<Post>(s => s
                    //.Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .Term(p => p
                            .Field(p => p.Title) //.Field("title")
                            .Value("core")
                        //.CaseInsensitive()
                        )
                    )
                );
            }

            public static void Terms(ElasticClient client)
            {
                //Returns documents that contain one or more exact terms in a provided field.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-terms-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/terms-query-usage.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/terms-list-query-usage.html

                var searchResponse = client.Search<Post>(s => s
                    //.Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .Terms(p => p //bool query with should clauses of terms
                            .Field(p => p.Title) //.Field("title")
                            .Terms(new[] { "ef", "Core" })
                        )
                    )
                );
            }

            public static void Prefix(ElasticClient client)
            {
                //Returns documents that contain a specific prefix in a provided field.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-prefix-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/prefix-query-usage.html

                var searchResponse = client.Search<Post>(s => s
                    //.Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .Prefix(p => p
                            .Field(p => p.Title) //.Field("title")
                            .Value("cor")
                        //.Rewrite(MultiTermQueryRewrite.ConstantScore)
                        )
                    )
                );
            }

            public static void Exists(ElasticClient client)
            {
                //Returns documents that contain an indexed value for a field.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-exists-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/exists-query-usage.html

                var searchResponse = client.Search<Post>(s => s
                    .Query(q => q
                        .Exists(p => p
                            //.Field(Field)
                            //.Field("title")
                            .Field(p => p.Title)
                        )
                    )
                );
            }

            public static void LongRange(ElasticClient client)
            {
                //Returns documents that contain terms within a provided range.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-range-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/numeric-range-query-usage.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/long-range-query-usage.html

                var searchResponse = client.Search<Post>(s => s
                    .Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .LongRange(c => c
                            //.Field(string)
                            .Field(p => p.LikeCount)
                            //.GreaterThan(1)
                            //.LessThanOrEquals(4)
                            .GreaterThanOrEquals(13)
                            .LessThan(15)
                        //.Relation(RangeRelation.Intersects) //Matches documents with a range field value that intersects the query’s range. (Defaults)
                        //.Relation(RangeRelation.Contains) //Matches documents with a range field value that entirely contains the query’s range.
                        //.Relation(RangeRelation.Within) //Matches documents with a range field value entirely within the query’s range.
                        )
                    )
                );
            }

            public static void Ids(ElasticClient client)
            {
                //Returns documents based on their IDs. This query uses document IDs stored in the _id field.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-ids-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/ids-query-usage.html

                var searchResponse = client.Search<Post>(s => s
                    .Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .Ids(c => c.Values(101, 102, 103))
                    )
                );
            }

            public static void Fuzzy(ElasticClient client)
            {
                //Returns documents that contain terms similar to the search term, as measured by a Levenshtein edit distance.
                //To find similar terms, the fuzzy query creates a set of all possible variations, or expansions, of the search term within a specified edit distance.
                //The query then returns exact matches for each expansion.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-fuzzy-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/fuzzy-query-usage.html

                var searchResponse = client.Search<Post>(s => s
                    .Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .Fuzzy(c => c
                            .Field(p => p.Title) //.Field("title")
                            .Fuzziness(Fuzziness.Auto)
                            .Value("farmewook") //not found for "Farmewook"
                            .Transpositions() // (ab → ba)
                        //.MaxExpansions(100)
                        //.PrefixLength(5)
                        //.Rewrite(MultiTermQueryRewrite.ConstantScore)
                        //.Fuzziness(Fuzziness.Auto) //AUTO should generally be the preferred value for fuzziness. (Defaults may be to "AUTO:3,6")
                        //.Fuzziness(Fuzziness.AutoLength(1, 3)) //0..2 => Must match exactly - 3..5 => One edit allowed - >5 => Two edits allowed
                        //.Fuzziness(Fuzziness.EditDistance(3)) //1,2,3 The maximum allowed Levenshtein Edit Distance (or number of edits)
                        //.Fuzziness(Fuzziness.Ratio(30d))
                        //.FuzzyTranspositions()
                        //.FuzzyRewrite(MultiTermQueryRewrite.ConstantScore)
                        )
                    )
                );
            }

            public static void Bool(ElasticClient client)
            {
                //A query that matches documents matching boolean combinations of other queries. It is built using one or more boolean clauses, each clause with a typed occurrence. The occurrence types are:
                //- AND (clause `must` - binary `&&` operator):     The clause (query) must appear in matching documents and will contribute to the score.
                //- FILTER (clause `filter` - unary `+` operator):  The clause (query) must appear in matching documents. However unlike must, the score of the query will be ignored.
                //- OR (clause `should` - binary `||` operator):    The clause (query) should appear in the matching document.
                //- NOT (clause `must_not` - unary `!` operator):   The clause (query) must not appear in the matching documents.
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-queries.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-dsl-complex-query-usage.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-query-usage.html

                var searchResponse = client.Search<Post>(s => s
                    .Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .Bool(b => b
                            .Should(
                                bs => bs.Term(p => p.Title, "asp.net"),
                                bs => bs.Term(p => p.Title, "core")
                                //bs => bs.Term(p => p.Body, "core")
                            )
                        )
                    )
                //.Query(q => q
                //    .Term(p => p.Title, "x") || q
                //    .Term(p => p.Body, "y")
                //)
                //.Query(q => q
                //    .Bool(b => b
                //      .MustNot(m => m.MatchAll()) //must not contains
                //      .Should(m => m.MatchAll())  //prefer to contains (with scoring)
                //      .Must(m => m.MatchAll())    //must contains
                //      .Filter(f => f.MatchAll())  //filter (no scoring)
                //    )
                //)
                );
            }

            public static void MoreLikeThis(ElasticClient client)
            {
                //The More Like This Query finds documents that are "like" a given set of documents.
                //In order to do so, MLT selects a set of representative terms of these input documents, forms a query using these terms, executes the query and returns the results.
                //The user controls the input documents, how the terms should be selected and how the query is formed.
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-mlt-query.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/more-like-this-query-usage.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/more-like-this-full-document-query-usage.html

                var post = new Post(105);
                #region Full Parameters
                //var searchResponse = client.Search<Post>(s => s
                //    .Index("post-index")
                //    .From(0)
                //    .Size(10)
                //    .Query(q => q
                //        .MoreLikeThis(sn => sn
                //            .Like(l => l
                //                //.Document(d => d
                //                //    .Id(Id)
                //                //    .Index(IndexName)
                //                //    .Document(Post)
                //                //    .Fields(Fields)
                //                //    .Fields(p => p.Field(p => p.Title).Field(p => p.Body))
                //                //    .Routing(Routing)
                //                //)
                //                .Text("some long text")
                //            )
                //            .Analyzer("some_analyzer")
                //            .BoostTerms(1.1)
                //            .Include()
                //            .MaxDocumentFrequency(12)
                //            .MaxQueryTerms(12)
                //            .MaxWordLength(300)
                //            .MinDocumentFrequency(1)
                //            .MinTermFrequency(1)
                //            .MinWordLength(10)
                //            .StopWords("and", "the")
                //            .MinimumShouldMatch(1)
                //            .Fields(Fields)
                //            .Fields(p => p.Field(p => p.Title).Field(p => p.Body))
                //            .Unlike(l => l
                //                //.Document(d => d
                //                //    .Id(Id)
                //                //    .Index(IndexName)
                //                //    .Document(Post)
                //                //    .Fields(Fields)
                //                //    .Fields(p => p.Field(p => p.Title).Field(p => p.Body))
                //                //    .Routing(Routing)
                //                //)
                //                .Text("not like this text")
                //            )
                //        )
                //    )
                //);
                #endregion

                var searchResponse = client.Search<Post>(s => s
                    .Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .MoreLikeThis(sn => sn
                            .Like(l => l
                            //.Document(d => d
                            ////.Id(101)
                            ////.Index("post-index")
                            ////.Document(post) //_id or _doc but not both
                            ////.Fields("title, body")
                            ////.Fields(new[] { "title", "body" })
                            ////.Fields(p => p.Field(p => p.Title).Field(p => p.Body)) //which fileds of this specified document must be compared
                            ////.Routing(Routing)
                            //)
                            .Text(post.Body)
                            )
                        //.Analyzer("some_analyzer")
                        //.BoostTerms(1.1)
                        //.Fields("title, body")
                        //.Fields(new[] { "title", "body" })
                        .Fields(p => p.Field(p => p.Title).Field(p => p.Body)) //witch fields of this index must be analyzed
                        .MinTermFrequency(1)
                        )
                    )
                );
            }

            public static void Highlight(ElasticClient client)
            {
                //Highlighters enable you to get highlighted snippets from one or more fields in your search results so you can show users where the query matches are.
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/highlighting-usage.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/highlighting.html

                var result1 = client.Search<Post>(p => p
                    //Search Query
                    .Query(p => p
                        .Match(p => p
                            .Field(p => p.Title)
                            .Query("EF Core")
                        )
                    )
                    //Highlight searched term (EF Core) by default
                    //By default use <em></em> tag
                    .Highlight(h => h
                        .Fields(p => p
                            //Require specify fields for highlight (same field as query field that search performed)
                            .Field(p => p.Title)
                        )
                    )
                );
                //Request:
                //POST /post-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "highlight": {
                //    "fields": {
                //      "title": {}
                //    }
                //  },
                //  "query": {
                //    "match": {
                //      "title": {
                //        "query": "EF Core"
                //      }
                //    }
                //  }
                //}
                var hits1 = result1.Hits;


                var result2 = client.Search<Post>(p => p
                    //Search Query
                    .Query(p => p
                        .Match(p => p
                            .Field(p => p.Title)
                            .Query("EF Core")
                        )
                    )
                    //Highlight searched term (EF Core) by default
                    //Require specify fields for highlight
                    .Highlight(h => h
                        .PreTags("<b>").PostTags("</b>") //tag specified for all fields
                        .HighlightQuery(q => q
                            .Match(p => p
                                .Field(p => p.Title) //specify highlighter query for specified field and term
                                .Query("EF")
                            )
                        )
                        .Fields(p => p
                            .PreTags("<strong>").PostTags("</strong>") //override tag for specified field
                                                                       //Require specify fields for highlight (same field as query field that search performed)
                            .Field(p => p.Title)
                        )
                    )
                );
                //Request:
                //POST /post-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "highlight": {
                //    "fields": {
                //      "title": {
                //        "post_tags": [
                //          "</strong>"
                //        ],
                //        "pre_tags": [
                //          "<strong>"
                //        ]
                //      }
                //    },
                //    "highlight_query": {
                //      "match": {
                //        "title": {
                //          "query": "EF"
                //        }
                //      }
                //    },
                //    "post_tags": [
                //      "</b>"
                //    ],
                //    "pre_tags": [
                //      "<b>"
                //    ]
                //  },
                //  "query": {
                //    "match": {
                //      "title": {
                //        "query": "EF Core"
                //      }
                //    }
                //  }
                //}
                var hits2 = result2.Hits;


                var result3 = client.Search<Post>(p => p
                    //Search Query
                    .Query(p => p
                        .Match(p => p
                            .Field(p => p.Title)
                            .Query("EF Core")
                        )
                    )
                    //Highlight searched term (Core) by default
                    //Require specify fields for highlight
                    .Highlight(h => h
                        .PreTags("<b>").PostTags("</b>") //tag specified for all fields
                        .Encoder(HighlighterEncoder.Html) //HighlighterEncoder.Default
                        .HighlightQuery(q => q
                            .Match(p => p
                                .Field(p => p.Title) //specify highlighter query for specified field and term
                                .Query("آموزش")
                            )
                        )
                        .Fields(p => p
                            .PreTags("<strong>").PostTags("</strong>") //override tag for specified field
                            .Field(p => p.Title)
                            .HighlightQuery(q => q
                                .Match(p => p
                                    .Field(p => p.Title) //override highlighter query for specified field and term
                                    .Query("EF")
                                )
                            ), p => p
                            .PreTags("<ins>").PostTags("</ins>") //override tag for specified field
                            .Field(p => p.Body)
                            .HighlightQuery(q => q
                                .Match(p => p
                                    .Field(p => p.Body) //override highlighter query for specified field and term
                                    .Query("Core")
                                )
                            )
                            //.Type("plain") //HighlighterType.Plain //HighlighterType.Unified
                            //.Type(HighlighterType.Fvh) //Fast Vector Highlighter
                        )
                    )
                );
                //Request:
                //POST /post-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "highlight": {
                //    "encoder": "html",
                //    "fields": {
                //      "title": {
                //        "highlight_query": {
                //          "match": {
                //            "title": {
                //              "query": "EF"
                //            }
                //          }
                //        },
                //        "post_tags": [
                //          "</strong>"
                //        ],
                //        "pre_tags": [
                //          "<strong>"
                //        ]
                //      },
                //      "body": {
                //        "highlight_query": {
                //          "match": {
                //            "body": {
                //              "query": "Core"
                //            }
                //          }
                //        },
                //        "post_tags": [
                //          "</ins>"
                //        ],
                //        "pre_tags": [
                //          "<ins>"
                //        ]
                //      }
                //    },
                //    "highlight_query": {
                //      "match": {
                //        "title": {
                //          "query": "آموزش"
                //        }
                //      }
                //    },
                //    "post_tags": [
                //      "</b>"
                //    ],
                //    "pre_tags": [
                //      "<b>"
                //    ]
                //  },
                //  "query": {
                //    "match": {
                //      "title": {
                //        "query": "EF Core"
                //      }
                //    }
                //  }
                //}
                var hits3 = result3.Hits;


                //var result = client.Search<Post>(p => p
                //    .Query(p => p
                //        .Match(p => p
                //            .Field(p => p.Title/*.Suffix("standard")*/)
                //            .Query("Core")
                //        )
                //    )
                //    .Highlight(h => h
                //        .PreTags("<b>")
                //        .PostTags("</b>")
                //        .Encoder(HighlighterEncoder.Html) //HighlighterEncoder.Default
                //        .HighlightQuery(q => q
                //            .Match(p => p
                //                .Field(p => p.Title/*.Suffix("standard")*/)
                //                .Query("Core")
                //            )
                //        )
                //        .Fields(
                //            fs => fs
                //                .Field(p => p.Name/*.Suffix("standard")*/)
                //                .Type("plain") //HighlighterType.Plain
                //                .Type(HighlighterType.Fvh) //HighlighterType.Unified
                //                .ForceSource()
                //                .FragmentSize(150)
                //                .Fragmenter(HighlighterFragmenter.Span) //HighlighterFragmenter.Simple
                //                .NumberOfFragments(3)
                //                .NoMatchSize(150),
                //            fs => fs
                //                .Field(p => p.LeadDeveloper.FirstName)
                //                .PreTags("<name>")
                //                .PostTags("</name>")
                //                .BoundaryMaxScan(50)
                //                .PhraseLimit(10)
                //                .HighlightQuery(q => q
                //                    .Match(m => m
                //                        .Field(p => p.LeadDeveloper.FirstName)
                //                        .Query("Kurt Edgardo Naomi Dariana Justice Felton")
                //                    )
                //                ),
                //            fs => fs
                //                .Field(p => p.LeadDeveloper.LastName)
                //                .Type(HighlighterType.Unified)
                //                .PreTags("<name>")
                //                .PostTags("</name>")
                //                .HighlightQuery(q => q
                //                    .Match(m => m
                //                        .Field(p => p.LeadDeveloper.LastName)
                //                        .Query("bluh bluh bluh")
                //                    )
                //                )
                //        )
                //    )
                //);
            }

            public static void Suggest(ElasticClient client)
            {
                //The suggest feature suggests similar looking terms based on a provided text by using a suggester.
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/suggest-usage.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/search-suggesters.html

                var result1 = client.Search<Post>(p => p
                    .Suggest(ss => ss
                        .Term("my-term-suggest", t => t
                            .Field(p => p.Title)
                            .Text("Ertity Farmework")
                        )
                    )
                );
                //Request:
                //POST /post-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "suggest": {
                //    "my-term-suggest": {
                //      "term": {
                //        "field": "title"
                //      },
                //      "text": "Ertity Farmework"
                //    }
                //  }
                //}
                var suggest1 = result1.Suggest["my-term-suggest"]; //1:entity 2:framework

                //Request:
                //POST http://localhost:9200/post-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "suggest": {
                //    "my-phrase-suggest": {
                //      "phrase": {
                //        "field": "title"
                //      },
                //      "text": "Enrity Farmework"
                //    }
                //  }
                //}

                //var result = client.Search<Post>(p => p
                //    .Suggest(ss => ss
                //        //The term suggester suggests terms based on edit distance. The provided suggest text is analyzed before terms are suggested.
                //        //.Term("my-term-suggest", t => t
                //        //    .Field(p => p.Title)
                //        //    .Text("Entity Farmework")

                //        //    .MaxEdits(1) //The maximum edit distance. Can only be a value between 1 and 2. Defaults to 2.
                //        //    .Size(10) //Maximum number of suggestion ro return
                //        //    .ShardSize(7) //Sets the maximum number of suggestions to be retrieved from each individual shard. Defaults to the size option
                //        //    .MaxInspections(2) //A factor that is used to multiply with the shards_size in order to inspect more candidate spelling corrections on the shard level. Defaults to 5

                //        //    //Defines how suggestions should be sorted per suggest text term
                //        //    .Sort(SuggestSort.Score) //Sort by score first, then document frequency and then the term itself.
                //        //    .Sort(SuggestSort.Frequency) //Sort by document frequency first, then similarity score and then the term itself.
                //        //    .PrefixLength(6) //The number of minimal prefix characters that must match in order be a candidate for suggestions. Defaults to 1.
                //        //    .MinWordLength(5) //The minimum length a suggest text term must have in order to be included. Defaults to 4.
                //        //    .MinDocFrequency(4) //The minimal threshold in number of documents a suggestion should appear in. Defaults to 0f and is not enabled
                //        //    .MaxTermFrequency(3) //The maximum threshold in number of documents in which a suggest text token can exist in order to be included. Defaults to 0.01f.

                //        //    //Which string distance implementation to use for comparing how similar suggested terms are.
                //        //    .StringDistance(StringDistance.Internal) //The default based on damerau_levenshtein but highly optimized for comparing string distance for terms inside the index.
                //        //    .StringDistance(StringDistance.DamerauLevenshtein) //String distance algorithm based on Damerau-Levenshtein algorithm.
                //        //    .StringDistance(StringDistance.Levenshtein) //String distance algorithm based on Levenshtein edit distance algorithm.
                //        //    .StringDistance(StringDistance.Jarowinkler) //String distance algorithm based on Jaro-Winkler algorithm.
                //        //    .StringDistance(StringDistance.Ngram) //String distance algorithm based on character n-grams.

                //        //    //The suggest mode controls what suggestions are included or controls for what suggest text terms, suggestions should be suggested.
                //        //    .SuggestMode(SuggestMode.Always)  //Suggest any matching suggestions based on terms in the suggest text.
                //        //    .SuggestMode(SuggestMode.Popular) //Only suggest suggestions that occur in more docs than the original suggest text term.
                //        //    .SuggestMode(SuggestMode.Missing) //Only provide suggestions for suggest text terms that are not in the index. This is the default.
                //        //)
                //        //The phrase suggester adds additional logic on top of the term suggester to select entire corrected phrases instead of individual tokens weighted based on ngram-language models.
                //        //.Phrase("my-phrase-suggest", ph => ph
                //        //    .Field(p => p.Title)
                //        //    .Text("Enrity Farmework")
                //        //    .GramSize(1)
                //        //    .Confidence(10.1)
                //        //    .DirectGenerator(d => d
                //        //        .Field(p => p.Body)
                //        //    )
                //        //    .RealWordErrorLikelihood(0.5)
                //        //    .TokenLimit(5)
                //        //    .ForceUnigrams(false)
                //        //    .Collate(c => c
                //        //        .Query(q => q
                //        //            .Source("{ \"match\": { \"{{field_name}}\": \"{{suggestion}}\" }}")
                //        //        )
                //        //        .Params(p => p.Add("field_name", "title"))
                //        //        .Prune()
                //        //    )
                //        //)
                //        //The completion suggester provides auto-complete/search-as-you-type functionality.
                //        //.Completion("my-completion-suggest", c => c
                //        //    .Field(p => p.Title)
                //        //    .Prefix("entit")
                //        //    .Size(8)
                //        //    .SkipDuplicates()
                //        //    .Fuzzy(f => f
                //        //        .Fuzziness(Fuzziness.Auto)
                //        //        .MinLength(1)
                //        //        .PrefixLength(2)
                //        //        .Transpositions()
                //        //        .UnicodeAware(false)
                //        //    )
                //        //    .Contexts(ctxs => ctxs
                //        //        .Context("color",
                //        //            ctx => ctx.Context(Project.First.Suggest.Contexts.Values.SelectMany(v => v).First())
                //        //        )
                //        //    )
                //        //)
                //    )
                //);
            }

            public static void Others(ElasticClient client)
            {
                var searchResponse = client.Search<Post>(s => s
                    .Index("post-index")
                    .From(0)
                    .Size(10)
                    .Query(q => q
                        .Term(p => p.Title, "kimchy") || q //Can set Field, Boost (double?), Name (string - name of the query to retrive base on this latter)
                        .Match(p => p
                            .Field(f => f.Body /*or Field*/)
                            .Query("nest")
                            .Operator(Operator.And | Operator.Or)
                            )
                    )
                );

                var searchResponse2 = client.Search<Post>(s => s
                    .Query(p => p
                        .Match(p => p
                            .Field(f => f.Title)
                            .Query("my query")
                        )
                    )
                    .Sort(p => p
                        .Descending(p => p.Title.Suffix("keyword"))
                    )
                    .Aggregations(p => p
                        .Terms("my terms", t => t
                            .Field(f => f.Title.Suffix("keyword"))
                        )
                    )
                );


                #region Other ElasticClient Methods
                //client.LowLevel;
                //client.RequestResponseSerializer
                //------- Others
                //client.Map();
                //client.Reindex()
                //client.ReindexOnServer()
                //client.ReindexRethrottle()
                //client.DocumentExists();
                //client.ExecutePainlessScript();
                //client.Explain();
                //client.MultiSearch();
                //client.TermVectors();
                //client.MultiTermVectors();
                #endregion
            }

            public static void TermVectors(ElasticClient client)
            {
                var result = client.TermVectors<Post>(p => p
                    //.Index(IndexName)
                    .Id(101)
                    .Fields(p => p.Title, p => p.Body)
                    //.Fields(Fields)
                    .Offsets()
                    .Payloads()
                    .Positions()
                    .TermStatistics()
                );

                var found = result.Found;
                var termVectors = result.TermVectors;
                termVectors.TryGetValue(Infer.Field<Post>(p => p.Title), out var termVectorTitle);
                termVectors.TryGetValue(Infer.Field<Post>(p => p.Body), out var termVectorBody);
            }

            public static void MultiSearch(ElasticClient client)
            {
                var multiSearchResponse = client.MultiSearch(new[] { "post-index" /*"multi-index1", "multi-index2"*/ }, p => p
                    //.Index(new[] { "multi-index11", "multi-index22" }) //override index above indices
                    .Search<Post>("search_name1", p => p
                        //.Index("search-index11")
                        .Query(p => p
                            .Match(p => p
                                //.Name("query_name1") //naming query does not required. it just useful for finding matched queries in hits
                                .Field(p => p.Title)
                                .Query("Core")
                            )
                        )
                    )
                    .Search<Post>("search_name2", p => p
                        //.Index("search-index22")
                        .Query(p => p
                            .Match(p => p
                                //.Name("query_name2") //naming query does not required. it just useful for finding matched queries in hits
                                .Field(p => p.Title)
                                .Query("ASP.NET")
                            )
                        )
                    )
                );

                var toalResponse = multiSearchResponse.TotalResponses;
                var responses = multiSearchResponse.AllResponses;
                var invalidResponses = multiSearchResponse.GetInvalidResponses();
                var postResponses = multiSearchResponse.GetResponses<Post>();
                var response1 = multiSearchResponse.GetResponse<Post>("search_name1");
                var response2 = multiSearchResponse.GetResponse<Post>("search_name2");

                //Request1:
                //POST /post-index/_msearch?pretty=true&error_trace=true&typed_keys=true
                //{}
                //{"query":{"match":{"title":{"query":"Core"}}}}
                //{}
                //{"query":{"match":{"title":{"query":"ASP.NET"}}}}

                //Request2:
                //POST /multi-index11%2Cmulti-index22/_msearch?pretty=true&error_trace=true&typed_keys=true
                //{"index":"search-index11"}
                //{"query":{"match":{"title":{"query":"Core","_name":"query_name"}}}}
                //{"index":"search-index22"}
                //{"query":{"match":{"title":{"query":"ASP.NET","_name":"query_name2"}}}}

                //client.MultiSearch("post-index", p => p
                //    .Search<Post>("search_name1", p => p
                //        .Index(IndexName)
                //        .Query(p => p
                //            .Match(p => p
                //                .Name("query_name")
                //                .Field(p => p.Title)
                //                .Query("Core")
                //            )
                //        )
                //    )
                //    .Search<Post>("search_name2", p => p
                //        .Index(IndexName)
                //        .Query(p => p
                //            .Match(p => p
                //                .Name("query_name2")
                //                .Field(p => p.Title)
                //                .Query("ASP.NET")
                //            )
                //        )
                //    )
                //    .Index(IndexName)
                //    .MaxConcurrentSearches(long?)
                //    .MaxConcurrentShardRequests(long?)
                //    .TypedKeys()
                //    .TotalHitsAsInteger()
                //);
            }

            public static void DocumentExists(ElasticClient client)
            {
                var existsResponse = client.DocumentExists<Post>(101, p => p
                //.Refresh(Refresh.True)
                //.Index(IndexName)
                //.SourceEnabled()
                //.SourceIncludes(new[] { "title", "body" })
                //.SourceExcludes(new[] { "title", "body" })
                );

                //Request:
                //HEAD /post-index/_doc/101?pretty=true&error_trace=true

                var exists = existsResponse.Exists;
            }

            public static void Count(ElasticClient client)
            {
                var countResponse = client.Count<Post>(p => p
                    //.Index(Indices)
                    .Query(p => p
                        .Match(p => p
                            .Field(p => p.Title)
                            .Query("Core")
                        )
                    )
                );

                //Request:
                //POST /post-index/_count?pretty=true&error_trace=true
                //{
                //  "query": {
                //    "match": {
                //      "title": {
                //        "query": "Core"
                //      }
                //    }
                //  }
                //}

                var count = countResponse.Count;
            }

            public static void CreateDocument(ElasticClient client)
            {
                var posts = new Post[]
                {
                    new(120, "test 20", "body 20", default, 120),
                    new(121, "test 21", "body 21", default, 121)
                };
                var createResponse1 = client.Create(posts[0], p => p
                //.Id(Id)
                //.Index(IndexName)
                //.Refresh(Refresh.True)
                );

                var result1 = client.Search<Post>(p => p.MatchAll());

                var createResponse2 = client.CreateDocument(posts[1]);

                //Request (for both):
                //PUT /post-index/_create/120?pretty=true&error_trace=true
                //{
                //  "id": 120,
                //  "title": "test 20",
                //  "body": "body 20",
                //  "publishDate": "0001-01-01T00:00:00",
                //  "likeCount": 120
                //}

                var result2 = client.Search<Post>(p => p.MatchAll());
            }

            public static void UpdatePosts(ElasticClient client)
            {
                var result0 = client.Search<Post>(p => p.MatchAll());

                var posts = new Post[]
                {
                    new(120, "new test 20", "new body 20", default, 120),
                    new(121, "new test 21", "new body 21", default, 121)
                };

                //### Update

                //POST request to the update API, read more about this API online:
                //https://www.elastic.co/guide/en/elasticsearch/reference/master/docs-update.html
                var updateResponse1 = client.Update<Post>(120, p => p
                    .Doc(posts[0])
                );

                //Request:
                //POST /post-index/_update/120?pretty=true&error_trace=true
                //{
                //  "doc": {
                //    "id": 120,
                //    "title": "new test 20",
                //    "body": "new body 20",
                //    "publishDate": "0001-01-01T00:00:00",
                //    "likeCount": 120
                //  }
                //}

                var result1 = client.Search<Post>(p => p.MatchAll());

                var updateResponse11 = client.Update<Post>(121, p => p
                    .Doc(posts[1])
                    .DocAsUpsert()
                //.Upsert(posts[1]) //this does not work but above code works fine
                //.Index(IndexName)
                //.Refresh(Refresh.True)
                //.Script(selector)
                //.ScriptedUpsert()
                );

                //Request:
                //POST /post-index/_update/121?pretty=true&error_trace=true
                //{
                //  "doc": {
                //    "id": 121,
                //    "title": "new test 21",
                //    "body": "new body 21",
                //    "publishDate": "0001-01-01T00:00:00",
                //    "likeCount": 121
                //  },
                //  "doc_as_upsert": true
                //}

                var result11 = client.Search<Post>(p => p.MatchAll());


                //### Update Partial
                var test1 = client.Update<object>(120, p => p
                    .Index("post-index")
                    .Doc(new
                    {
                        Title = "new title"
                    })
                );

                //Request:
                //POST /post-index/_update/120?pretty=true&error_trace=true
                //{
                //  "doc": {
                //    "title": "new title"
                //  }
                //}

                var result2 = client.Search<Post>(p => p.MatchAll());

                // Create partial document with a dynamic
                dynamic updateDoc = new ExpandoObject();
                updateDoc.title = "new title 2"; //Tips: title

                var updateResponse2 = client.Update<Post, dynamic>(120, p => p
                    .Doc(updateDoc)
                );

                var result3 = client.Search<Post>(p => p.MatchAll());

                var updateResponse3 = client.Update<Post, dynamic>(120, p => p
                    .Doc(new
                    {
                        Title = "new title 3"
                    })
                );

                var result4 = client.Search<Post>(p => p.MatchAll());

                var updateResponse4 = client.Update<object, dynamic>(120, p => p
                    .Index("post-index")
                    .Doc(new
                    {
                        Title = "new title 4"
                    })
                );

                //client.Update<Post>(101, p => p
                //    .Index("post-index")
                //    .Index<Post>()
                //    .Doc(post) //The partial update document to be merged on to the existing object.
                //    .Upsert(post)
                //    .DocAsUpsert()
                //    .ScriptedUpsert()
                //    .Source()
                //    .SourceEnabled()
                //    .DetectNoop()
                //    .Refresh(Refresh.True)
                //    .RetryOnConflict(3) //Specify how many times should the operation be retried when a conflict occurs (default: 0)
                //);

                //### UpdateByQuery
                var updateByQueryResponse1 = client.UpdateByQuery<Post>(p => p
                    .Script($"ctx._source.title += ' new title 1'")
                    .Query(p => p
                        .Match(p => p
                            .Field(p => p.Title)
                            .Query("Core")
                        )
                    )
                );

                //Request:
                //POST /post-index/_update_by_query?pretty=true&error_trace=true
                //{
                //  "query": {
                //    "match": {
                //      "title": {
                //        "query": "Core"
                //      }
                //    }
                //  },
                //  "script": {
                //    "source": "ctx._source.title += ' new title 1'
                //  }
                //}

                var result5 = client.Search<Post>(p => p.MatchAll());

                //Workaround for eventual consistency
                Thread.Sleep(2000);
                var updateByQueryResponse2 = client.UpdateByQuery<Post>(p => p
                    .Script(p => p
                        //.Id(string id)          //can be id or source not both
                        //.Params(Dictionary<string, object>)
                        .Source("ctx._source.title += params.Title")
                        .Params(p => p
                            .Add("Title", "new title 2")
                        )
                    )
                    .Query(p => p
                        .Match(p => p
                            .Field(p => p.Title)
                            .Query("Core")
                        )
                    )
                );

                //Request:
                //POST /post-index/_update_by_query?pretty=true&error_trace=true
                //{
                //  "query": {
                //    "match": {
                //      "title": {
                //        "query": "Core"
                //      }
                //    }
                //  },
                //  "script": {
                //    "source": "ctx._source.title += params.Title",
                //    "params": {
                //      "Title": "new title 2"
                //    }
                //  }
                //}

                var result6 = client.Search<Post>(p => p.MatchAll());

                //https://discuss.elastic.co/t/elasticsearch-update-by-query-using-nest/198083/2
                //https://discuss.elastic.co/t/nest-api-updatebyquery/128759/2
                //https://stackoverflow.com/questions/50069565/does-elasticsearch-nest-support-update-by-query
                //client.UpdateByQuery<Post>(p => p
                //    .Script("ctx._source.name=" + "test")     //A script specify the update to make
                //    .Script(p => p
                //        //.Id(string id)          //can be id or source not both
                //        //.Params(Dictionary<string, object>)
                //        .Source("ctx._source.name = params.name")
                //        .Params(p => p
                //            .Add("name", "value")
                //        )
                //    )
                //    .Index("post-index")
                //    .Index<Post>()
                //    .From(long ?)
                //    .Size(long ?)
                //    .Sort(string[])
                //    .MatchAll()
                //    .Query(p => p
                //        .Match(selector)
                //    )
                //    .DefaultOperator(DefaultOperator.And)
                //    .Analyzer("standard")           //The analyzer to use for the query string
                //    .AnalyzeWildcard()              //Specify whether wildcard and prefix queries should be analyzed (default: false)
                //    .MaximumDocuments(long ?)        //Limit the number of processed documents
                //    .TerminateAfter(long ?)          //The maximum number of documents to collect for each shard, upon reaching which the query execution will terminate early.
                //    .Timeout(Time)                  //Time each individual bulk request should wait for shards that are unavailable.
                //    .SearchTimeout(Time)            //Explicit timeout for each search request. Defaults to no timeout.
                //    .Slice(selector)                //Parallelize the deleting process. This parallelization can improve efficency and a convient way to break the request down to smaller parts
                //    .Conflicts(Conflicts.Proceed)   //What to do when the delete by query hits version conflicts?
                //    .Scroll(Time)                   //Specify how long a consistent view of the index should be maintained for scrolled search
                //    .ScrollSize(long ?)             //Size on the scroll request powering the delete by query
                //    .RequestsPerSecond(long?)       //The throttle to set on this request in sub-requests per second. -1 means no throttle.
                //    .SourceEnabled()                //Whether the _source should be included in the response.
                //    .SourceIncludes(new[] { "title", "body" }) //A list of fields to extract and return from the _source field
                //    .SourceExcludes(new[] { "title", "body" }) //A list of fields to exclude from the returned _source field
                //);


                //client.UpdateByQueryRethrottle();
            }

            public static void DeletePosts(ElasticClient client)
            {
                DocumentPath<Post> documentPath1 = DocumentPath<Post>.Id(101);
                DocumentPath<Post> documentPath2 = new DocumentPath<Post>(101);
                DocumentPath<Post> documentPath3 = 101;

                var result0 = client.Search<Post>(p => p.MatchAll());

                //DELETE request to the delete API, read more about this API online:
                //https://www.elastic.co/guide/en/elasticsearch/reference/master/docs-delete.html
                var deleteResponse = client.Delete<Post>(101, p => p
                //.Index("post-index")
                //.Index<Post>()
                //.Refresh(Refresh.True) //apply changes immediately
                );

                //Request:
                //DELETE /post-index/_doc/101?pretty=true&error_trace=true

                var result1 = client.Search<Post>(p => p.MatchAll());

                //Shortcut into the Bulk call that deletes the specified objects
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html
                var bulkResponse = client.DeleteMany(new Post[] { new(120), new(121) }/*, IndexName*/);

                //Request:
                //POST /_bulk?pretty=true&error_trace=true
                //{"delete":{"_id":"120","_index":"post-index"}}
                //{"delete":{"_id":"121","_index":"post-index"}}

                var result2 = client.Search<Post>(p => p.MatchAll());

                //POST request to the delete_by_query API, read more about this API online:
                //https://www.elastic.co/guide/en/elasticsearch/reference/master/docs-delete-by-query.html
                var deleteByQueryResponse = client.DeleteByQuery<Post>(p => p.
                    Query(p => p
                        .Match(p => p
                            .Field(p => p.Title)
                            .Query("Core")
                        )
                    )
                );

                //Request:
                //POST /post-index/_delete_by_query?pretty=true&error_trace=true
                //{
                //  "query": {
                //    "match": {
                //      "title": {
                //        "query": "Core"
                //      }
                //    }
                //  }
                //}

                var result3 = client.Search<Post>(p => p.MatchAll());

                //client.DeleteByQuery<Post>(p => p
                //    .Index("post-index")
                //    .Index<Post>()
                //    .From(long ?)
                //    .Size(long ?)
                //    .Sort(string[])
                //    .MatchAll()
                //    .Query(p => p
                //        .Match(selector)
                //    )
                //    .DefaultOperator(DefaultOperator.And)
                //    .Analyzer("standard")           //The analyzer to use for the query string
                //    .AnalyzeWildcard()              //Specify whether wildcard and prefix queries should be analyzed (default: false)
                //    .MaximumDocuments(long ?)        //Limit the number of processed documents
                //    .TerminateAfter(long ?)          //The maximum number of documents to collect for each shard, upon reaching which the query execution will terminate early.
                //    .Timeout(Time)                  //Time each individual bulk request should wait for shards that are unavailable.
                //    .SearchTimeout(Time)            //Explicit timeout for each search request. Defaults to no timeout.
                //    .Slice(selector)                //Parallelize the deleting process. This parallelization can improve efficency and a convient way to break the request down to smaller parts
                //    .Conflicts(Conflicts.Proceed)   //What to do when the delete by query hits version conflicts?
                //    .Scroll(Time)                   //Specify how long a consistent view of the index should be maintained for scrolled search
                //    .ScrollSize(long ?)             //Size on the scroll request powering the delete by query
                //    .RequestsPerSecond(long ?)      //The throttle to set on this request in sub-requests per second. -1 means no throttle.
                //    .SourceEnabled()                //Whether the _source should be included in the response.
                //    .SourceIncludes(new[] { "title", "body" }) //A list of fields to extract and return from the _source field
                //    .SourceExcludes(new[] { "title", "body" }) //A list of fields to exclude from the returned _source field
                //);

                //POST request to the delete_by_query_rethrottle API, read more about this API 
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html
                //client.DeleteByQueryRethrottle();

                //DELETE request to the delete_script API, read more about this API online:
                //https://www.elastic.co/guide/en/elasticsearch/reference/master/modules-scripting.html
                //client.DeleteScript();
            }

            public static void Bulk(ElasticClient client)
            {
                var posts = new Post[]
                {
                    new(111, "post1", "body1", default, 111),
                    new(112, "post2", "body2", default, 112),
                    new(113, "post3", "body3", default, 113)
                };

                var result1 = client.Search<Post>(p => p.MatchAll());

                //### Bulk
                var bulkResponse = client.Bulk(p => p
                    .Index<Post>(p => p
                        .Document(posts[0])
                    //.Index(IndexName)
                    )
                    .IndexMany(posts[1..], (p, post) => p
                    //.Index(IndexName)
                    )
                    .Delete<Post>(new(109), p => p
                    //.Id(109)
                    //.Index(IndexName)
                    //.Document(new(109))
                    )
                    .DeleteMany<Post>(new long[] { 107, 108 }, (p, post) => p
                    //.Id(109)
                    //.Index(IndexName)
                    //.Document(new(109))
                    )
                    .Refresh(Refresh.True)
                );

                //Request:
                //POST /_bulk?pretty=true&error_trace=true&refresh=true
                //{"index":{"_id":"111","_index":"post-index"}}
                //{"id":111,"title":"post1","body":"body1","publishDate":"0001-01-01T00:00:00","likeCount":111}
                //{"index":{"_id":"112","_index":"post-index"}}
                //{"id":112,"title":"post2","body":"body2","publishDate":"0001-01-01T00:00:00","likeCount":112}
                //{"index":{"_id":"113","_index":"post-index"}}
                //{"id":113,"title":"post3","body":"body3","publishDate":"0001-01-01T00:00:00","likeCount":113}
                //{"delete":{"_id":"109","_index":"post-index"}}
                //{"delete":{"_id":107,"_index":"post-index"}}
                //{"delete":{"_id":108,"_index":"post-index"}}

                var items = bulkResponse.Items;
                var itemsWithErrors = bulkResponse.ItemsWithErrors;

                var result2 = client.Search<Post>(p => p.MatchAll());

                //var bulkResponse = client.Bulk(p => p
                //    .Index("post-index")
                //    .Index<Post>()
                //    .Create<Post>(selector)
                //    .CreateMany<Post>(IEnumerable<Post>, selector)
                //    .Index<Post>(selector)
                //    .IndexMany<Post>(IEnumerable<Post>, selector)
                //    .Delete<Post>(post, selector)
                //    .Delete<Post>(selector)
                //    .DeleteMany<Post>(new long[] { 101, 102 }, selector)
                //    .DeleteMany<Post>(new Post[] { }, selector)
                //    .Update<Post>(selector)
                //    .Update<Post, dynamic>(selector)
                //    .UpdateMany<Post>(IEnumerable<Post>, selector)
                //    .UpdateMany<Post, dynamic>(IEnumerable<Post>, selector)
                //    .SourceEnabled()
                //    .SourceIncludes(Fields)
                //    .SourceExcludes(Fields)
                //    .Refresh(Refresh.True)
                //);

                //### BulkAll
                //var bulkAllObservable = client.BulkAll<Post>(new Post[] { }, p => p
                //    .BackOffRetries(int?)
                //    .BackOffTime(Time)
                //    .BackPressure(int maxConcurrency, int? backPressureFactor)
                //    .BufferToBulk(Action modifier)
                //    .BulkResponseCallback(Action callback)
                //    .ContinueAfterDroppedDocuments()
                //    .DroppedDocumentCallback(Action callback)
                //    .MaxDegreeOfParallelism(int?)
                //    .RefreshIndices(Indices)
                //    .RefreshOnCompleted()
                //    .RetryDocumentPredicate(Func predicate)
                //    .Size(int?)
                //    .Timeout(Time)
                //);
            }
            #endregion

            #region Article
            public static void MappingArticle(ElasticClient client)
            {
                var mappingResponse = client.Map<Article>(p => p
                    //.Index("article-index")
                    //.Properties(p => p)
                    .AutoMap()
                );
            }

            public static void CreateIndexArticle(ElasticClient client)
            {
                var index = IndexName.From<Article>();

                var existsResponse = client.Indices.Exists(index);
                if (existsResponse.Exists)
                {
                    var deleteResponse = client.Indices.Delete(index);
                }

                var createIndexResponse = client.Indices.Create(index, p => p
                    .Map<Article>(p => p.AutoMap()
                    //.Properties(p => p...)
                    )
                );

                //Request:
                //PUT /article-index?pretty=true&error_trace=true
                //{
                //  "mappings": {
                //    "properties": {
                //      "id": {
                //        "type": "integer"
                //      },
                //      "title": {
                //        "fields": {
                //          "keyword": {
                //            "ignore_above": 256,
                //            "type": "keyword"
                //          }
                //        },
                //        "type": "text"
                //      },
                //      "tagIds": {
                //        "type": "integer"
                //      },
                //      "tags": {
                //        "properties": {
                //          "id": {
                //            "type": "integer"
                //          },
                //          "name": {
                //            "fields": {
                //              "keyword": {
                //                "ignore_above": 256,
                //                "type": "keyword"
                //              }
                //            },
                //            "type": "text"
                //          }
                //        },
                //        "type": "object"
                //      },
                //      "author": {
                //        "properties": {
                //          "id": {
                //            "type": "integer"
                //          },
                //          "name": {
                //            "fields": {
                //              "keyword": {
                //                "ignore_above": 256,
                //                "type": "keyword"
                //              }
                //            },
                //            "type": "text"
                //          }
                //        },
                //        "type": "object"
                //      }
                //    }
                //  }
                //}
            }

            public static void IndexArticles(ElasticClient client)
            {
                var articles = new Article[]
                {
                    new Article {
                        Id = 1,
                        Title = "Article1 - ASP.NET Core",
                        Author = new Author { Id = 1,  Name = "Author1" },
                        TagIds = new[] { 1, 2, 3 },
                        Tags = new List<Tag>
                        {
                            new Tag { Id = 1, Name = "Tag1" },
                            new Tag { Id = 2, Name = "Tag2" },
                            new Tag { Id = 3, Name = "Tag3" }
                        }
                    },
                    new Article {
                        Id = 2,
                        Title = "Article2 - EF Core",
                        Author = new Author { Id = 2,  Name = "Author2" },
                        TagIds = new[] { 3, 4, 5 },
                        Tags = new List<Tag>
                        {
                            new Tag { Id = 3, Name = "Tag3" },
                            new Tag { Id = 4, Name = "Tag4" },
                            new Tag { Id = 5, Name = "Tag5" }
                        }
                    }
                };

                //var bulkResponse = client.IndexMany(articles);
                //Request:
                //POST /_bulk?pretty=true&error_trace=true
                //{"index":{"_id":"1","_index":"article-index"}}
                //{"id":1,"title":"Article1 - ASP.NET Core","tagIds":[1,2,3],"tags":[{"id":1,"name":"Tag1"},{"id":2,"name":"Tag2"},{"id":3,"name":"Tag3"}],"author":{"id":1,"name":"Author1"}}
                //{"index":{"_id":"2","_index":"article-index"}}
                //{"id":2,"title":"Article2 - EF Core","tagIds":[3,4,5],"tags":[{"id":3,"name":"Tag3"},{"id":4,"name":"Tag4"},{"id":5,"name":"Tag5"}],"author":{"id":2,"name":"Author2"}}

                var bulkResponse2 = client.Bulk(p => p
                     .IndexMany(articles)
                     .Refresh(Refresh.True)
                );
                //POST /_bulk?pretty=true&error_trace=true&refresh=true
                //{"index":{"_id":"1","_index":"article-index"}}
                //{"id":1,"title":"Article1 - ASP.NET Core","tagIds":[1,2,3],"tags":[{"id":1,"name":"Tag1"},{"id":2,"name":"Tag2"},{"id":3,"name":"Tag3"}],"author":{"id":1,"name":"Author1"}}
                //{"index":{"_id":"2","_index":"article-index"}}
                //{"id":2,"title":"Article2 - EF Core","tagIds":[3,4,5],"tags":[{"id":3,"name":"Tag3"},{"id":4,"name":"Tag4"},{"id":5,"name":"Tag5"}],"author":{"id":2,"name":"Author2"}}
            }

            public static void SearchArticles(ElasticClient client)
            {
                //### Term

                //TagIds Contains 2 (single term)
                var result1 = client.Search<Article>(p => p
                    .Query(p => p
                        .Term(p => p.TagIds, 3)
                    )
                );
                //Request:
                //POST /article-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "query": {
                //    "term": {
                //      "tagIds": {
                //        "value": [
                //          2
                //        ]
                //      }
                //    }
                //  }
                //}
                var document1 = result1.Documents; //2

                //### Terms
                var result2 = client.Search<Article>(p => p
                    .Query(p => p
                        .Terms(p => p
                            .Field(p => p.TagIds)
                            .Terms(new int[] { 1, 2, 3 }) //return document that contains ANY of these terms
                        )
                    )
                );
                //Request:
                //POST /article-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "query": {
                //    "terms": {
                //      "tagIds": [
                //        1,
                //        2,
                //        3
                //      ]
                //    }
                //  }
                //}
                var documents2 = result2.Documents; //2


                //### Bool/Filter/Must
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-queries.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-dsl-complex-query-usage.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-query-usage.html
                var result3 = client.Search<Article>(p => p
                    .Query(p => p
                        .Bool(p => p
                            .Filter(
                                p => p.Term(p => p.TagIds, 1),
                                p => p.Term(p => p.TagIds, 2),
                                p => p.Term(p => p.TagIds, 3)
                            )
                        )
                    )
                );
                //Request:
                //POST /article-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "query": {
                //    "bool": {
                //      "filter": [
                //        {
                //          "term": {
                //            "tagIds": {
                //              "value": 1
                //            }
                //          }
                //        },
                //        {
                //          "term": {
                //            "tagIds": {
                //              "value": 2
                //            }
                //          }
                //        },
                //        {
                //          "term": {
                //            "tagIds": {
                //              "value": 3
                //            }
                //          }
                //        }
                //      ]
                //    }
                //  }
                //}
                var documents3 = result3.Documents; //1

                var result31 = client.Search<Article>(p => p
                    .Query(p =>
                        p.Term(p => p.TagIds, 1) && //Must
                        p.Term(p => p.TagIds, 2) &&
                        p.Term(p => p.TagIds, 3)
                    )
                );
                var documents31 = result31.Documents; //1

                var result32 = client.Search<Article>(p => p
                    .Query(p =>
                        +p.Term(p => p.TagIds, 1) && //Filter
                        +p.Term(p => p.TagIds, 2) &&
                        +p.Term(p => p.TagIds, 3)
                    )
                );
                var documents32 = result32.Documents; //1

                var result33 = client.Search<Article>(p => p
                    .Query(p => p
                        .Terms(p => p
                            .Field(p => p.TagIds)
                            .Terms(new int[] { 2, 3 })
                        )
                    )
                );
                //Request:
                //POST /article-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "query": {
                //    "terms": {
                //      "tagIds": [
                //        2,
                //        3
                //      ]
                //    }
                //  }
                //}
                var document33 = result33.Documents; //2

                var result34 = client.Search<Article>(p => p
                    .Query(p => p
                        .Terms(p => p
                            .Field(p => p.TagIds)
                            .Terms(new int[] { 3 })
                        )
                    )
                );
                //Request:
                //POST /article-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "query": {
                //    "terms": {
                //      "tagIds": [
                //        2,
                //        3
                //      ]
                //    }
                //  }
                //}
                var document34 = result34.Documents; //2


                //### Non-[Nested] Property: Tags
                var tagIdField = Infer.Field<Tag>(p => p.Id);
                var tagIdFieldName = client.Infer.Field(tagIdField); //id
                var tagIdName = client.Infer.PropertyName(typeof(Tag).GetProperty("Id")); //id
                var tagIdFieldInfer = client.Infer.Field<Article>(p => p.Tags).Suffix<Tag>(p => p.Id);

                var result4 = client.Search<Article>(p => p
                    .Query(p => p
                        .Term(p => p.Tags.First().Id, 2)
                    //.Term("tags.id", 2)
                    //.Term(p => p.Tags.Suffix("id"), 2)
                    //.Term(client.Infer.Field<Article>(p => p.Tags).Suffix<Tag>(p => p.Id), 2)
                    //.Term(p => p
                    //    .Field("tags.id")
                    //    .Field(p => p.Tags.First().Id)
                    //    .Field(p => p.Tags.Suffix("id"))
                    //    .Field(client.Infer.Field<Article>(p => p.Tags).Suffix<Tag>(p => p.Id))
                    //    .Value(2)
                    //)
                    )
                );
                var result41 = client.Search<Article>(p => p
                    .Query(p => p
                        .Term(p => p.Tags.First().Id, 2)
                    )
                );
                //Request:
                //POST /article-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "query": {
                //    "term": {
                //      "tags.id": {
                //        "value": 2
                //      }
                //    }
                //  }
                //}
                var document4 = result4.Documents; //1


                //### [Nested] Property: Tags
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/nested.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/flattened.html
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/nested-query-usage.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-nested-query.html

                var result5 = client.Search<Article>(p => p
                    .Query(p => p
                        .Nested(p => p
                            .Path(p => p.Tags)
                            .Query(p => p
                                .Term(p => p
                                    //.Field("tags.id")
                                    //.Field(p => p.Tags.Suffix("id"))
                                    .Field(p => p.Tags.First().Id)
                                    .Value(2)
                                )
                            )
                        )
                    )
                );
                //Request:
                //POST /article-index/_search?pretty=true&error_trace=true&typed_keys=true
                //{
                //  "query": {
                //    "nested": {
                //      "path": "tags",
                //      "query": {
                //        "term": {
                //          "tags.id": {
                //            "value": 2
                //          }
                //        }
                //      }
                //    }
                //  }
                //}
                var document5 = result5.Documents; //1
            }
            #endregion

            public static void DeleteIndices(ElasticClient client)
            {
                var indices1 = Indices.Parse("post-index, article-index");
                var indices2 = Indices.Index("post-index", "article-index");
                var indices3 = Indices.Index(IndexName.From<Post>(), IndexName.From<Article>());
                var indices4 = Indices.Index(new[] { "post-index", "article-index" });
                var indices5 = Indices.Index<Post>().And<Article>();
                var indices6 = IndexName.From<Post>().And<Article>();

                client.Indices.Delete(indices6);
            }

            public static void TestAnalyzer(ElasticClient client)
            {
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/testing-analyzers.html
                //https://www.elastic.co/guide/en/elasticsearch/reference/current/test-analyzer.html

                var result0 = client.Indices.Analyze(a => a
                    .Analyzer("standard") //whitespace
                    .Text("F# is THE SUPERIOR language :)")
                );
                var tokens0 = result0.Tokens; //f - is - the - superior - language
                //Request:
                //POST http://localhost:9200/_analyze?pretty=true&error_trace=true
                //{
                //  "analyzer": "standard",
                //  "text": [
                //    "F# is THE SUPERIOR language :)"
                //  ]
                //}

                var result1 = client.Indices.Analyze(p => p
                    .Tokenizer("standard")
                    .Filter("lowercase", "stop") //lowercase //uppercase //stop //
                                                 //.Filter(p => p
                                                 //    .Lowercase()
                                                 //    .Uppercase()
                                                 //    .Stop(p => p)
                                                 //)
                    .Text("The quick brown FOX.")
                );
                var token1 = result1.Tokens; //quick brown fox
                //Request:
                //POST /_analyze?pretty=true&error_trace=true
                //{
                //  "filter": [
                //    "lowercase",
                //    "stop"
                //  ],
                //  "text": [
                //    "The quick brown FOX."
                //  ],
                //  "tokenizer": "standard"
                //}

                var result2 = client.Indices.Analyze(p => p
                    .Tokenizer("standard")
                    .Filter(p => p
                        .Lowercase()
                        .Stop(p => p
                            .StopWords("آموزش", "تا", "از")
                        //.StopWords(new string[] { "آموزش", "تا", "از" })
                        //.StopWordsPath("path")
                        //.IgnoreCase()
                        //.RemoveTrailing()
                        )
                    )
                    .Text("آموزش از صفر تا صد EF")
                );
                var token2 = result2.Tokens; //صفر - صد - ef
                //Request:
                //POST /_analyze?pretty=true&error_trace=true
                //{
                //  "filter": [
                //    {
                //      "type": "lowercase"
                //    },
                //    {
                //      "stopwords": [
                //        "آموزش",
                //        "از",
                //        "تا",
                //      ],
                //      "type": "stop"
                //    }
                //  ],
                //  "text": [
                //    "آموزش از صفر تا صد EF"
                //  ],
                //  "tokenizer": "standard"
                //}





                //var result = client.Indices.Analyze(p => p
                //    //.Analyzer("standard")
                //    //.CharFilter("")
                //    .Tokenizer("standard")
                //    //.Filter("lowercase", "stop") //lowercase //uppercase //stop //
                //    .Filter(p => p
                //        .Lowercase()
                //        //.Lowercase(p => p.Language("language"))
                //        //.Uppercase()
                //        .Stop(p => p
                //        //.StopWords("آموزش", "از", "به", "با", "برای")
                //        //.StopWords(new string[] { "آموزش", "از", "به", "با", "برای" })
                //        //.StopWordsPath("path")
                //        //.IgnoreCase()
                //        //.RemoveTrailing()
                //        )
                //    //.RemoveDuplicates()
                //    //.Synonym()
                //    //.WordDelimiter()
                //    )
                //    //.Normalizer("")
                //    .Text("The quick brown FOX.")
                //);
            }

            public static void CustomAnalyzer(ElasticClient client)
            {
                //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/writing-analyzers.html

                var createIndexResponse = client.Indices.Create("question-index", c => c
                    .Settings(s => s
                        .Analysis(a => a
                            .CharFilters(cf => cf
                                //Create custom char filters
                                .Mapping("programming_language", mca => mca
                                    .Mappings(new[]
                                    {
                                        "c# => csharp",
                                        "C# => Csharp"
                                    })
                                )
                            )
                            .Analyzers(an => an
                                //Configuring a built-in analyzer
                                //.Standard("standard_english_analyzer", sa => sa
                                //    .StopWords("is", "the")
                                //)
                                .Custom("index_time_custom_analyzer", ca => ca
                                    //Create custom analyzer
                                    .CharFilters("html_strip", "programming_language")
                                    .Tokenizer("standard")
                                    .Filters("lowercase", "stop")
                                )
                                .Custom("search_time_custom_analyzer", ca => ca
                                    .CharFilters("programming_language")
                                    .Tokenizer("standard")
                                    .Filters("lowercase", "stop")
                                )
                            )
                        )
                    )
                    .Map<Question>(mm => mm
                        .AutoMap()
                        .Properties(p => p
                            .Text(t => t
                                .Name(n => n.Body)
                                .Analyzer("index_time_custom_analyzer") //standard_english_analyzer
                                .SearchAnalyzer("search_time_custom_analyzer")
                            )
                        )
                    )
                );

            }
        }

        public class Question
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }

        #region FieldInfer
        public class FieldInfer : Field
        {
            public Inferrer Infer { get; set; }
            public FieldInfer(Inferrer inferrer, string name) : base(name)
            {
                Infer = inferrer;
            }
        }

        public static FieldInfer Field<T>(this Inferrer inferrer, Expression<Func<T, object>> expression) where T : class
        {
            var field = Infer.Field(expression);
            var name = inferrer.Field(field);
            return new(inferrer, name);
        }

        public static FieldInfer Suffix<T>(this FieldInfer fieldInfer, Expression<Func<T, object>> expression) where T : class
        {
            var field = Infer.Field(expression);
            var name = fieldInfer.Infer.Field(field);
            var suffix = fieldInfer.Name + "." + name;
            return new(fieldInfer.Infer, suffix);
        }
        #endregion

        [DebuggerDisplay("{Title,nq}")]
        //[ElasticsearchType(IdProperty = "Id", RelationName = "relation-name")]
        public class Post
        {
            public Post(int id)
            {
                Id = id;
            }

            public Post(int id, string title, string body, DateTime publishDate, int likeCount)
            {
                Id = id;
                Title = title;
                Body = body;
                PublishDate = publishDate;
                LikeCount = likeCount;
            }

            public int Id { get; set; }

            //[Text(Name = "field_title")]
            //[Completion]
            [Text(
                Index = true, //Fields that are not indexed are not queryable/searchable.
                Store = false //stored fileds can be retrive in a single field instead of whole _source
                              //IndexPhrases = false,
                              //IndexOptions = IndexOptions.Offsets, //https://www.elastic.co/guide/en/elasticsearch/reference/current/index-options.html
                              //TermVector = TermVectorOption.Yes
                )]
            //Positions: used for phrase queries or word proximity queries (Defaults)
            //Offsets: used by the (unified highlighter) to speed up highlighting.
            public string Title { get; set; }

            //[Text(Name = "field_body", Analyzer = "analyzer", Norms = true, Similarity = "LMDirichlet")]
            public string Body { get; set; }

            //[Date(Name = "publish_date", Format = "yyyy/MM/dd HH:mm:ss", Store = true)]
            public DateTime PublishDate { get; set; }

            //[PropertyName("like_count", Ignore = false)]
            //[Number(NumberType.Integer, Name = "like_count")]
            public int LikeCount { get; set; }

            //[Nested]
            //[Object(Store = false)]
            //public List<Employee> Employee { get; set; }

            //[Text(Ignore = true)]
            //[PropertyName("ignored_prop", Ignore = true)]
            //[Ignore, JsonIgnore]
            //public string IgnoredProperty { get; set; }

            //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/attribute-mapping.html
            //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/fluent-mapping.html
            //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/ignoring-properties.html
            //[Text(Analyzer = "",
            //  Boost = 1.0,
            //  Index = true,
            //  Store = true,
            //  Norms = true,
            //  IndexPhrases = true,
            //  IndexOptions = IndexOptions.Offsets,
            //  TermVector = TermVectorOption.WithPositionsOffsetsPayloads),
            //  Similarity = "LMDirichlet"]
            //--------
            //[Text]
            //[Boolean]
            //[Binary]
            //[Date]
            //[Number]
            //[Ip]
            //--------
            //[IntegerRange]
            //[FloatRange]
            //[DoubleRange]
            //[DateRange]
            //[Flattened]
            //[IpRange]
            //--------
            //[Object]
            //[Nested]
            //[Ignore]
            //[Keyword]
            //[ConstantKeyword]
            //[PropertyName]
            //[Wildcard]
            //[RankFeature]
            //[RankFeatures]
            //[ElasticsearchType]
            //[Completion]
            //[SearchAsYouType]
        }

        //[DebuggerDisplay("{Score}, {Document}")]
        //public class FoundDocument<T>
        //{
        //    public T Document { get; set; }
        //    public float Score { get; set; }
        //}

        //public class SearchResult
        //{
        //    public List<(float Score, Document Document)> Documents { get; set; }
        //    public float MaxScore { get; set; }
        //    public int TotalHits { get; set; }
        //}

        [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
        public class Article
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int[] TagIds { get; set; }
            [Nested]
            public List<Tag> Tags { get; set; }
            public Author Author { get; set; }

            private string GetDebuggerDisplay() => $"{Id}- {Title}";
        }

        [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
        public class Tag
        {
            public int Id { get; set; }
            public string Name { get; set; }
            private string GetDebuggerDisplay() => $"{Id}- {Name}";
        }

        [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
        public class Author
        {
            public int Id { get; set; }
            public string Name { get; set; }
            private string GetDebuggerDisplay() => $"{Id}- {Name}";
        }
    }
}
#pragma warning restore S1481 // Unused local variables should be removed
#pragma warning restore S125 // Sections of code should not be commented out
