using Lucene.Net.Analysis.Fa;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Lucene.Net.Documents.Extensions;

#pragma warning disable S125 // Sections of code should not be commented out
#pragma warning disable S1481 // Unused local variables should be removed
namespace Sample
{
    public static class LuceneSearch
    {
        public static void Test()
        {
            LuceneExtensions.DeleteAll();

            var posts = new Post[]
            {
                new Post(101, "آموزش Entity Framework",       "در این مقاله به آموزش Entity Framework می پردازیم",        new DateTime(2021, 01, 01), 11),
                new Post(102, "آموزش ASP.NET MVC",            "در این مقاله به آموزش ASP.NET MVC می پردازیم",             new DateTime(2021, 01, 02), 12),
                new Post(103, "آموزش EF Core",                "در این مقاله به آموزش EF Core می پردازیم",                 new DateTime(2021, 01, 03), 13),
                new Post(104, "آموزش ASP.NET Core",           "در این مقاله به آموزش ASP.NET Core می پردازیم",            new DateTime(2021, 01, 04), 14),
                new Post(105, "آموزش Domain Driven Design",   "در این مقاله به آموزش Domain Driven Design می پردازیم",    new DateTime(2021, 01, 05), 15),
                new Post(106, "آموزش Microservices",          "در این مقاله به آموزش Microservices می پردازیم",           new DateTime(2021, 01, 06), 16),
                new Post(107, "از به با برای C#",             "در این مقاله به آموزش C# می پردازیم",                      new DateTime(2021, 01, 07), 17),
                new Post(108, "یکسان Title",                  "در این مقاله به بررسی Term می پردازیم",                    new DateTime(2021, 01, 08), 18),
                new Post(109, "یکسان Title",                  "در این مقاله به بررسی Match می پردازیم",                   new DateTime(2021, 01, 09), 19),
            };

            IndexPosts(posts);

            #region TitleString
            {
                PostSearchField = "TitleString";
                var resultx1 = SearchPost("آموزش ASP.NET MVC", 3).ToList();
                var resultx2 = SearchPost("آموزش EF Core", 3).ToList();
                var resultx3 = SearchPost("اموزش ASP.NET MVC", 3).ToList();
                var resultx4 = SearchPost("اموزش EF Core", 3).ToList();


                var result01 = SearchPost("Entity", 3).ToList();
                var result02 = SearchPost("Framework", 3).ToList();
                var result03 = SearchPost("Entity Framework", 3).ToList();

                var result04 = SearchPost("ASP", 3).ToList();
                var result05 = SearchPost("ASP.NET", 3).ToList();
                var result06 = SearchPost("ASP.NET Core", 3).ToList();
                var result07 = SearchPost("ASP.NET MVC", 3).ToList();
                var result08 = SearchPost("MVC", 3).ToList();

                var result09 = SearchPost("EF", 3).ToList();
                var result10 = SearchPost("EF Core", 3).ToList();

                var result11 = SearchPost("Domain", 3).ToList();
                var result12 = SearchPost("Domain Driven Design", 3).ToList();

                var result13 = SearchPost("Microservices", 3).ToList();
                var result14 = SearchPost("آموزش", 3).ToList();
                var result15 = SearchPost("Core", 3).ToList();
                var result16 = SearchPost("Design", 3).ToList();

                var result17 = SearchPost("Design Driven Domain", 3).ToList();
                var result18 = SearchPost("چرت", 3).ToList();
                var result19 = SearchPost("اموزش", 3).ToList();
                var result20 = SearchPost("برای", 3).ToList();
                var result21 = SearchPost("C#", 3).ToList();
                var result22 = SearchPost("C", 3).ToList();
            }
            #endregion

            #region TitleText
            {
                PostSearchField = "TitleText";
                var resultx1 = SearchPost("آموزش ASP.NET MVC", 3).ToList();
                var resultx2 = SearchPost("آموزش EF Core", 3).ToList();
                var resultx3 = SearchPost("اموزش ASP.NET MVC", 3).ToList();
                var resultx4 = SearchPost("اموزش EF Core", 3).ToList();

                var result01 = SearchPost("Entity", 3).ToList();
                var result02 = SearchPost("Framework", 3).ToList();
                var result03 = SearchPost("Entity Framework", 3).ToList();

                var result04 = SearchPost("ASP", 3).ToList();
                var result05 = SearchPost("ASP.NET", 3).ToList();
                var result06 = SearchPost("ASP.NET Core", 3).ToList();
                var result07 = SearchPost("ASP.NET MVC", 3).ToList();
                var result08 = SearchPost("MVC", 3).ToList();

                var result09 = SearchPost("EF", 3).ToList();
                var result10 = SearchPost("EF Core", 3).ToList();

                var result11 = SearchPost("Domain", 3).ToList();
                var result12 = SearchPost("Domain Driven Design", 3).ToList();

                var result13 = SearchPost("Microservices", 3).ToList();
                var result14 = SearchPost("آموزش", 3).ToList();
                var result15 = SearchPost("Core", 3).ToList();
                var result16 = SearchPost("Design", 3).ToList();

                var result17 = SearchPost("Design Driven Domain", 3).ToList();
                var result18 = SearchPost("چرت", 3).ToList();
                var result19 = SearchPost("اموزش", 3).ToList();
                var result20 = SearchPost("برای", 3).ToList();
                var result21 = SearchPost("C#", 3).ToList();
                var result22 = SearchPost("C", 3).ToList();
            }
            #endregion

            #region TitleFullTextSearchHighlight
            {
                PostSearchField = "TitleFullTextSearchHighlight";

                var result01 = SearchPost("Entity", 3).ToList();
                var result02 = SearchPost("Framework", 3).ToList();
                var result03 = SearchPost("Entity Framework", 3).ToList();

                var result04 = SearchPost("ASP", 3).ToList();
                var result05 = SearchPost("ASP.NET", 3).ToList();
                var result06 = SearchPost("ASP.NET Core", 3).ToList();
                var result07 = SearchPost("ASP.NET MVC", 3).ToList();
                var result08 = SearchPost("MVC", 3).ToList();

                var result09 = SearchPost("EF", 3).ToList();
                var result10 = SearchPost("EF Core", 3).ToList();

                var result11 = SearchPost("Domain", 3).ToList();
                var result12 = SearchPost("Domain Driven Design", 3).ToList();

                var result13 = SearchPost("Microservices", 3).ToList();
                var result14 = SearchPost("آموزش", 3).ToList();
                var result15 = SearchPost("Core", 3).ToList();
                var result16 = SearchPost("Design", 3).ToList();

                var result17 = SearchPost("Design Driven Domain", 3).ToList();
                var result18 = SearchPost("چرت", 3).ToList();
                var result19 = SearchPost("اموزش", 3).ToList();
                var result20 = SearchPost("برای", 3).ToList();
                var result21 = SearchPost("C#", 3).ToList();
                var result22 = SearchPost("C", 3).ToList();
            }
            #endregion

        }

        #region Post Specific Methods
        public static IEnumerable<FoundDocument<Post>> SearchPost(string term, int top)
        {
            var query = CreatePostQuery(term);
            var searchResult = LuceneExtensions.SearchIndex(query, top);

            foreach (var item in searchResult.Documents)
            {
                var IdInt32 = item.Document.GetField("IdInt32");
                var IdString = item.Document.GetField("IdString");

                var TitleString = item.Document.GetField("TitleString");
                var TitleText = item.Document.GetField("TitleText");
                var TitleFullTextSearchHighlight = item.Document.GetField("TitleFullTextSearchHighlight");

                //var bodyField = item.Document.GetField("Post.Body");

                var PublishDateTicksStored = item.Document.GetField("PublishDateTicksStored");
                var PublishDateStringStored = item.Document.GetField("PublishDateStringStored");

                var LikeCountStored = item.Document.GetField("LikeCountStored");
                var LikeCountInt32 = item.Document.GetField("LikeCountInt32");
                //var LikeCountSortedDocValues = item.Document.GetField("LikeCountSortedDocValues");

                yield return new FoundDocument<Post>
                {
                    Score = item.Score,
                    Document = new Post
                    {
                        IdInt32 = IdInt32.GetInt32Value() ?? throw null,
                        IdString = Convert.ToInt32(IdString.GetStringValue()),

                        TitleString = TitleString.GetStringValue(),
                        TitleText = TitleText.GetStringValue(),
                        TitleFullTextSearchHighlight = TitleFullTextSearchHighlight.GetStringValue(),

                        PublishDateTicksStored = new DateTime(PublishDateTicksStored.GetInt64Value() ?? throw null),
                        PublishDateStringStored = DateTime.Parse(PublishDateStringStored.GetStringValue()),

                        LikeCountStored = LikeCountStored.GetInt32Value() ?? throw null,
                        LikeCountInt32 = LikeCountInt32.GetInt32Value() ?? throw null,
                        //LikeCountSortedDocValues = LikeCountSortedDocValues.GetInt32Value() ?? throw null,
                    }
                };
            }
        }

        public static string PostSearchField = "";
        public static Query CreatePostQuery(string term)
        {
            //return LuceneExtensions.CreateMultiPhraseQuery(term, PostSearchField);
            return LuceneExtensions.CreateMultiFieldQuery(term, PostSearchField);

            //return LuceneExtensions.CreateMultiPhraseQuery(term, "Title" /*, "Body"*/);

            //return LuceneExtensions.CreateMultiFieldQuery(term, "Title" /*, "Body"*/);

            //var fieldsBoos = new Dictionary<string, float>
            //{
            //    ["Title"] = 1.5f,
            //    //["Body"] = 1.0f
            //};
            //return LuceneExtensions.CreateMultiFieldQuery(term, fieldsBoos);
        }

        public static void IndexPosts(this IEnumerable<Post> posts)
        {
            var documents = posts.Select(p => p.ConvertPostToDocument()).ToList();
            LuceneExtensions.AddIndex(documents);
        }

        public static Document ConvertPostToDocument(this Post post)
        {
            #region Tips
            //Describes the properties of a field such as:
            //IsIndexed, IsStored, IsTokenized (via Analyzer), StoreTermVectors/Offsets/Positions/Payloads, OmitNorms, IndexOptions, DocValueType, NumericType, NumericPrecisionStep
            //Lucene.Net.Documents.FieldType : Lucene.Net.Index.IIndexableFieldType

            //Base type for lucene document Fields.Each field has three parts: name, type and value.
            //Values may be text(System.String, System.IO.TextReader or pre - analyzed Lucene.Net.Analysis.TokenStream), binary(byte[]), or numeric(System.Int32, System.Int64, System.Single, or System.Double)
            //Constructors which has no FieldType parameter is DEPRECATED
            //Lucene.Net.Documents.Field

            //Most users should use one of the sugar subclasses:
            //Creates a stored or un-stored (IsStored:true/false) numderic field.
            //IsIndexed:true, IsTokenized:true, OmitNorms:true IndexOptions:DOCS_ONLY, NumericPrecisionStep:4 and StoreTermVectors/Offsets/Positions/Payloads:false
            //Lucene.Net.Documents.Int32Field
            //Lucene.Net.Documents.Int64Field
            //Lucene.Net.Documents.SingleField
            //Lucene.Net.Documents.DoubleField

            //Creates a stored or un-stored (IsStored:true/false) string field that is indexed but not tokenized.
            //IsIndexed:true, IsTokenized:false, OmitNorms:true IndexOptions:DOCS_ONLY, and StoreTermVectors/Offsets/Positions/Payloads:false
            //Lucene.Net.Documents.StringField
            //Creates a stored or un-stored (IsStored:true/false) string field that is indexed and tokenized, without term vectors.
            //IsIndexed:true, IsTokenized:false, OmitNorms:false IndexOptions:DOCS_AND_FREQS_AND_POSITIONS, and StoreTermVectors/Offsets/Positions/Payloads:false
            //Lucene.Net.Documents.TextField

            //Create a stored-only field
            //Lucene.Net.Documents.StoredField
            //Don't Know
            //Lucene.Net.Documents.BinaryDocValuesField
            //Lucene.Net.Documents.SingleDocValuesField
            //Lucene.Net.Documents.DoubleDocValuesField
            //Lucene.Net.Documents.NumericDocValuesField
            //Lucene.Net.Documents.SortedDocValuesField
            //Lucene.Net.Documents.SortedSetDocValuesField

            ////Store the original field value in the index. this is useful for short texts like a document's title which should be displayed with the results (YES).
            //Lucene.Net.Documents.Field.Store

            ////Deprecated: Specifies whether and how a field should be indexed. NO (not index, not searchable) - ANALYZED (index using Analyzer, searchable, useful for common text)
            ////NOT_ANALYZED (index and searchable, but not Analyzer, useful for unique Ids like product numbers) - NOT_ANALYZED_NO_NORMS - ANALYZED_NO_NORMS
            //Lucene.Net.Documents.Field.Index

            ////Deprecated: Used for highlighter. Store the term vector (list of the document's terms and their number of occurrences), Token position, and offset information
            //Lucene.Net.Documents.Field.TermVector

            //Replacement for two above:
            //Lucene.Net.Index.IndexOptions.NONE: 
            //Lucene.Net.Index.IndexOptions.DOCS_ONLY: Only documents are indexed, term frequencies and positions are omitted.
            //Lucene.Net.Index.IndexOptions.DOCS_AND_FREQS: Only documents and term frequencies are indexed, positions are omitted.
            //Lucene.Net.Index.IndexOptions.DOCS_AND_FREQS_AND_POSITIONS: Indexes documents, frequencies and positions. this is a typical default for full-text search.
            //Lucene.Net.Index.IndexOptions.DOCS_AND_FREQS_AND_POSITIONS_AND_OFFSETS: Indexes documents, frequencies, positions and offsets. Useful for highligher

            //Others:
            //Lucene.Net.Search.SortFieldType
            //Lucene.Net.Search.NumericRangeQuery
            //Lucene.Net.Search.NumericRangeFilter

            //Create a string field that is indexed but not tokenized
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddStringField
            //Create a string field that is indexed and tokenized, without term vectors
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddTextField
            //Create a numeric field that is indexed and tokenized
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddInt32Field
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddInt64Field
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddSingleField
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddDoubleField
            //Don't Know
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddStoredField
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddBinaryDocValuesField
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddDoubleDocValuesField
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddSingleDocValuesField
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddNumericDocValuesField
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddSortedDocValuesField
            //Lucene.Net.Documents.Extensions.DocumentExtensions.AddSortedSetDocValuesField


            //  Field   |   Description             |   Stored  |   Indexed |   Tokenized   |   Normalized  |   StoreTermVectors/* - IndexOptions.DOCS_AND_FREQS_AND_POSITIONS_AND_OFFSETS
            //----------------------------------------------------------------------------------------------------------------------------------------------------------
            //  Id      | retrive, simple-search    |   true    |   true    |   false       |   false       |   false
            //  Title   | no-retrive, full-search   |   false   |   true    |   true        |   true        |   false
            //  Body    | full-search, highlight    |   true    |   true    |   true        |   true        |   true
            //  Comment | retrive, no-search        |   true    |   false   |   false       |   false       |   false
            //----------------------------------------------------------------------------------------------------------------------------------------------------------
            //  StringField (Simple Search)         |   ?       |   true    |   false       |   false       |   false
            //  TextField (Full-Text Search)        |   ?       |   true    |   true        |   true        |   false
            //  Full-Text Highlight Search          |   true    |   true    |   true        |   true        |   true
            //  StoredField (Retrive-Only)          |   true    |   ?       |   ?           |   ?           |   ? 
            #endregion

            var document = new Document();

            document.AddInt32Field("IdInt32", post.IdInt32, Field.Store.YES);
            document.AddStringField("IdString", post.IdString.ToString(), Field.Store.YES);

            document.AddStringField("TitleString", post.TitleString, Field.Store.YES); //indexed but doesn't tokenized (SimpleSearchField)
            document.AddTextField("TitleText", post.TitleText, Field.Store.YES); //indexed and tokenized (FullTextSearchField)
            document.AddFullTextSearchHighlightField("TitleFullTextSearchHighlight", post.TitleFullTextSearchHighlight);

            //Text.Body.RemoveHtmlTags()

            document.AddStoredField("PublishDateTicksStored", post.PublishDateTicksStored.Ticks);
            document.AddStoredField("PublishDateStringStored", post.PublishDateStringStored.ToString());

            document.AddStoredField("LikeCountStored", post.LikeCountStored);
            document.AddInt32Field("LikeCountInt32", post.LikeCountInt32, Field.Store.YES);
            document.AddSortedDocValuesField("LikeCountSortedDocValues", new BytesRef(post.LikeCountSortedDocValues.ToString()));

            return document;
        }
        #endregion

        [DebuggerDisplay("{TitleText,nq}")]
        public class Post
        {
            public Post()
            {
            }

            public Post(int id, string title, string body, DateTime publishDate, int likeCount)
            {
                IdInt32 = id;
                IdString = id;

                TitleString = title;
                TitleText = title;
                TitleFullTextSearchHighlight = title;

                BodyText = body;
                BodyFullTextSearchHighlight = body;

                PublishDateTicksStored = publishDate;
                PublishDateStringStored = publishDate;

                LikeCountStored = likeCount;
                LikeCountInt32 = likeCount;
                LikeCountSortedDocValues = likeCount;
            }

            public int IdInt32 { get; set; }
            public int IdString { get; set; }

            public string TitleString { get; set; }
            public string TitleText { get; set; }
            public string TitleFullTextSearchHighlight { get; set; }

            public string BodyText { get; set; }
            public string BodyFullTextSearchHighlight { get; set; }

            public DateTime PublishDateTicksStored { get; set; }
            public DateTime PublishDateStringStored { get; set; }

            public int LikeCountStored { get; set; }
            public int LikeCountInt32 { get; set; }
            public int LikeCountSortedDocValues { get; set; }
        }
    }


    public static class LuceneExtensions
    {
        #region CRUD Index
        public static void AddIndex(this IEnumerable<Document> documents)
        {
            var writer = GetIndexWriter();
            writer.AddDocuments(documents);

            writer.Commit();
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }

        public static void UpdateIndex(this IEnumerable<Document> documents, string fieldId = "Id")
        {
            var writer = GetIndexWriter();

            foreach (var document in documents)
            {
                var term = new Term(fieldId, document.Get(fieldId));
                writer.UpdateDocument(term, document);
            }

            writer.Commit();
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }

        public static void DeleteIndex(this IEnumerable<Document> documents, string fieldId = "Id")
        {
            var writer = GetIndexWriter();

            var terms = documents
                .Select(p => new Term(fieldId, p.Get(fieldId)))
                .ToArray();

            writer.DeleteDocuments(terms);

            writer.Commit();
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }

        public static void DeleteAll()
        {
            var writer = GetIndexWriter();

            writer.DeleteAll();

            writer.Commit();
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }

        public static void ReIndex(this IEnumerable<Document> documents, string fieldId = "Id")
        {
            var writer = GetIndexWriter();

            var terms = documents
                .Select(p => new Term(fieldId, p.Get(fieldId)))
                .ToArray();

            writer.DeleteDocuments(terms);

            writer.AddDocuments(documents);

            writer.Commit();
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }

        public static SearchResult SearchIndex(this Query query, int top, params string[] fieldsToLoad)
        {
            var writer = GetIndexWriter();
            // Re-use the writer to get real-time updates
            using var reader = writer.GetReader(applyAllDeletes: true);
            var searcher = new IndexSearcher(reader);
            var topDocs = searcher.Search(query, top);

            var fields = fieldsToLoad?.Length > 0 ? new HashSet<string>(fieldsToLoad) : null;
            var documents = topDocs.ScoreDocs
                .Select(p =>
                {
                    var doc = fields == null ? searcher.Doc(p.Doc) : searcher.Doc(p.Doc, fields);
                    return (p.Score, doc);
                }).ToList();

            return new SearchResult
            {
                Documents = documents,
                MaxScore = topDocs.MaxScore,
                TotalHits = topDocs.TotalHits
            };
        }
        #endregion

        #region Core
        // Ensures index backward compatibility
        public const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
        public static IndexWriter LuceneIndexWriter;
        public const string LuceneIndexPath = "lucene-index";

        public static IndexWriter GetIndexWriter()
        {
            if (LuceneIndexWriter != null)
                return LuceneIndexWriter;

            //https://yetanotherchris.dev/csharp/6-ways-to-get-the-current-directory-in-csharp/
            //https://www.thecodebuzz.com/get-root-directory-project-asp-net-core-application-path/
            //https://www.hanselman.com/blog/how-do-i-find-which-directory-my-net-core-console-application-was-started-in-or-is-running-from
            //https://www.delftstack.com/howto/csharp/how-to-get-current-folder-path-in-csharp/
            //C:\Users\Ebrahimi\Desktop\New folder\bin\Debug\net6.0
            //Environment.CurrentDirectory
            //AppContext.BaseDirectory
            //AppDomain.CurrentDomain.BaseDirectory
            //Directory.GetCurrentDirectory()
            //Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)

            // Construct a machine-independent path for the index
            var basePath = System.IO.Directory.GetCurrentDirectory();
            var indexPath = Path.Combine(basePath, LuceneIndexPath);

            var directory = FSDirectory.Open(indexPath); /*using*/

            // Create an analyzer to process the text
            var analyzer = GetAnalyzer();

            // Create an index writer
            var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
            LuceneIndexWriter = new IndexWriter(directory, indexConfig); /*using*/

            return LuceneIndexWriter;

            //directory.Dispose();
            //LuceneIndexWriter.Dispose();
        }

        public static Analyzer Analyzer;
        public static Analyzer GetAnalyzer()
        {
            if (Analyzer != null)
                return Analyzer;

            // Create stopwords CharArraySet
            //var stopwordsFile = new FileInfo(@".\stopwords.txt");
            //var stopwords = StopWordExtensions.LoadStopwordSet(stopwordsFile, AppLuceneVersion);
            //var stopwords = new CharArraySet(AppLuceneVersion, new string[] { "از", "به", "با" }, false);

            //var analyzer = new StandardAnalyzer(AppLuceneVersion);
            Analyzer = new PersianAnalyzer(AppLuceneVersion); //new PersianAnalyzer(AppLuceneVersion, stopwords);
                                                              //Lucene.Net.Analysis.Miscellaneous.PerFieldAnalyzerWrapper
                                                              //Lucene.Net.Analysis.Fa.PersianNormalizer

            return Analyzer;
        }

        public static CharArraySet LoadStopwordSet(this FileInfo fileInfo, LuceneVersion matchVersion, string comment = "#", bool ignoreCase = false)
        {
            TextReader textReader = null;
            try
            {
                textReader = IOUtils.GetDecodingReader(fileInfo, Encoding.UTF8);
                return WordlistLoader.GetWordSet(textReader, comment, new CharArraySet(matchVersion, 16, ignoreCase));
            }
            finally
            {
                IOUtils.Dispose(textReader);
            }
        }
        #endregion

        #region Query
        public static Query CreateMultiPhraseQuery(string term, params string[] fileds)
        {
            term = term.Trim();

            #region Tips
            #region Queries
            //Lucene.Net.Search.Query
            //Lucene.Net.Search.AutomatonQuery
            //Lucene.Net.Search.BooleanQuery
            //Lucene.Net.Search.ConstantScoreQuery
            //Lucene.Net.Search.DisjunctionMaxQuery
            //Lucene.Net.Search.FilteredQuery
            //Lucene.Net.Search.FuzzyQuery
            //Lucene.Net.Search.MatchAllDocsQuery
            //Lucene.Net.Search.MultiPhraseQuery
            //Lucene.Net.Search.MultiTermQuery
            //Lucene.Net.Search.NGramPhraseQuery
            //Lucene.Net.Search.NumericRangeQuery<>
            //Lucene.Net.Search.PhraseQuery
            //Lucene.Net.Search.PrefixQuery
            //Lucene.Net.Search.RegexpQuery
            //Lucene.Net.Search.TermQuery
            //Lucene.Net.Search.TermRangeQuery
            //Lucene.Net.Search.WildcardQuery
            //----- Others -----
            //Lucene.Net.Util.QueryBuilder
            //Lucene.Net.Analysis.Query.QueryAutoStopWordAnalyzer
            #endregion

            #region Surround Queries
            //Lucene.Net.QueryParsers.Surround.Query.AndQuery
            //Lucene.Net.QueryParsers.Surround.Query.BasicQueryFactory
            //Lucene.Net.QueryParsers.Surround.Query.ComposedQuery
            //Lucene.Net.QueryParsers.Surround.Query.DistanceQuery
            //Lucene.Net.QueryParsers.Surround.Query.FieldsQuery
            //Lucene.Net.QueryParsers.Surround.Query.NotQuery
            //Lucene.Net.QueryParsers.Surround.Query.OrQuery
            //Lucene.Net.QueryParsers.Surround.Query.SrndPrefixQuery
            //Lucene.Net.QueryParsers.Surround.Query.SrndQuery
            //Lucene.Net.QueryParsers.Surround.Query.SrndTermQuery
            //Lucene.Net.QueryParsers.Surround.Query.SrndTruncQuery
            #endregion

            #region QueryNodes
            //----- Core -----
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.QueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.AndQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.AnyQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.BooleanQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.BoostQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.DeletedQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.FieldQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.FuzzyQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.GroupQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.MatchAllDocsQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.MatchNoDocsQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.ModifierQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.NoTokenFoundQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.OpaqueQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.OrQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.PathQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.PhraseSlopQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.ProximityQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.QuotedFieldQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.SlopQueryNode
            //Lucene.Net.QueryParsers.Flexible.Core.Nodes.TokenizedPhraseQueryNode
            //----- Standard -----
            //Lucene.Net.QueryParsers.Flexible.Standard.Nodes.AbstractRangeQueryNode<>
            //Lucene.Net.QueryParsers.Flexible.Standard.Nodes.MultiPhraseQueryNode
            //Lucene.Net.QueryParsers.Flexible.Standard.Nodes.NumericQueryNode
            //Lucene.Net.QueryParsers.Flexible.Standard.Nodes.NumericRangeQueryNode
            //Lucene.Net.QueryParsers.Flexible.Standard.Nodes.PrefixWildcardQueryNode
            //Lucene.Net.QueryParsers.Flexible.Standard.Nodes.RegexpQueryNode
            //Lucene.Net.QueryParsers.Flexible.Standard.Nodes.StandardBooleanQueryNode
            //Lucene.Net.QueryParsers.Flexible.Standard.Nodes.TermRangeQueryNode
            //Lucene.Net.QueryParsers.Flexible.Standard.Nodes.WildcardQueryNode
            //Lucene.Net.QueryParsers.Flexible.Standard.Nodes.BooleanModifierNode
            //----- Others -----
            //Lucene.Net.QueryParsers.Flexible.Core.Parser.ISyntaxParser
            //Lucene.Net.QueryParsers.Flexible.Standard.Parser.StandardSyntaxParser
            //Lucene.Net.QueryParsers.Flexible.Core.Builders.QueryTreeBuilder<>
            //Lucene.Net.QueryParsers.Flexible.Core.QueryParserHelper<>
            #endregion

            #region QueryNodeBuilders
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.AnyQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.BooleanQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.BoostQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.DummyQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.FieldQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.FuzzyQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.GroupQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.MatchAllDocsQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.MatchNoDocsQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.ModifierQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.MultiPhraseQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.NumericRangeQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.PhraseQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.PrefixWildcardQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.RegexpQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.SlopQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.StandardBooleanQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.TermRangeQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.WildcardQueryNodeBuilder
            //Lucene.Net.QueryParsers.Flexible.Standard.Builders.StandardQueryTreeBuilder
            #endregion

            #region QueryParsers
            //Lucene.Net.QueryParsers.Classic.QueryParser
            //Lucene.Net.QueryParsers.Classic.QueryParserBase
            //Lucene.Net.QueryParsers.Classic.MultiFieldQueryParser
            //----- Others -----
            //Lucene.Net.QueryParsers.Surround.Parser.QueryParser
            //Lucene.Net.QueryParsers.Simple.SimpleQueryParser
            //Lucene.Net.QueryParsers.Flexible.Standard.StandardQueryParser
            //Lucene.Net.QueryParsers.Flexible.Standard.QueryParserUtil
            //Lucene.Net.QueryParsers.Flexible.Core.QueryParserHelper<>
            //Lucene.Net.QueryParsers.Ext.ExtendableQueryParser
            //Lucene.Net.QueryParsers.ComplexPhrase.ComplexPhraseQueryParser
            //Lucene.Net.QueryParsers.Analyzing.AnalyzingQueryParser
            #endregion

            #region Filters
            //Lucene.Net.Search.Filter
            //Lucene.Net.Search.CachingWrapperFilter
            //Lucene.Net.Search.DocTermOrdsRangeFilter
            //Lucene.Net.Search.FieldCacheRangeFilter
            //Lucene.Net.Search.FieldCacheTermsFilter
            //Lucene.Net.Search.FieldValueFilter
            //Lucene.Net.Search.MultiTermQueryWrapperFilter
            //Lucene.Net.Search.NumericRangeFilter
            //Lucene.Net.Search.PrefixFilter
            //Lucene.Net.Search.QueryWrapperFilter
            //Lucene.Net.Search.TermRangeFilter
            //-----
            //Lucene.Net.Queries.BooleanFilter
            //Lucene.Net.Queries.ChainedFilter
            //Lucene.Net.Queries.TermFilter
            //Lucene.Net.Queries.TermsFilter
            //-----
            //Lucene.Net.Queries.FilterClause
            //Lucene.Net.Search.BooleanClause
            #endregion
            #endregion

            // Search with a phrase
            var query = new MultiPhraseQuery();
            var terms = fileds.Select(field => new Term(field, term)).ToArray();
            query.Add(terms);
            return query;
        }

        public static Query CreateMultiFieldQuery(string term, params string[] fileds)
        {
            term = term.Trim();
            var analyzer = GetAnalyzer();

            //var query = MultiFieldQueryParser.Parse(AppLuceneVersion, string[] queries, analyzer);
            //var query = MultiFieldQueryParser.Parse(AppLuceneVersion, string[] queries, Occur[] flags, analyzer);
            var parser = new MultiFieldQueryParser(AppLuceneVersion, fileds, analyzer);

            return parser.ParseQuery(term);
        }

        public static Query CreateMultiFieldQuery(string term, Dictionary<string, float> fieldsBoost)
        {
            term = term.Trim();
            var analyzer = GetAnalyzer();

            var parser = new MultiFieldQueryParser(AppLuceneVersion, fieldsBoost.Keys.ToArray(), analyzer, fieldsBoost);

            return parser.ParseQuery(term);
        }

        public static Query ParseQuery(this QueryParser parser, string term)
        {
            try
            {
                return parser.Parse(term);
            }
            catch (ParseException)
            {
                return parser.Parse(QueryParserBase.Escape(term));
            }
        }

        public static string SearchByWords(string term)
        {
            term = term
                .Replace("*", "")
                .Replace("?", "")
                .Trim();

            var terms = term
                .Replace('-', ' ')
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Trim() + "*");

            return string.Join(' ', terms);
        }
        #endregion

        #region Utils
        //private static Regex htmlRegex = new Regex(@"<(.|\n)*?>", RegexOptions.Compiled);
        public static string RemoveHtmlTags(this string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : Regex.Replace(text, @"<(.|\n)*?>", string.Empty); //htmlRegex.Replace(text, string.Empty)
        }
        #endregion
    }

    [DebuggerDisplay("{Score}, {Document}")]
    public class FoundDocument<T>
    {
        public T Document { get; set; }
        public float Score { get; set; }
    }

    public class SearchResult
    {
        public List<(float Score, Document Document)> Documents { get; set; }
        public float MaxScore { get; set; }
        public int TotalHits { get; set; }
    }

    public static class FieldExtensions
    {
        //==> StringField
        //public static SimpleSearchField AddSimpleSearchField(this Document document, string name, string value, Field.Store stored)
        //{
        //    var stringField = new SimpleSearchField(name, value, stored);
        //    document.Add(stringField);
        //    return stringField;
        //}

        //==> TextField
        //public static FullTextSearchField AddFullTextSearchField(this Document document, string name, string value, Field.Store stored)
        //{
        //    var stringField = new FullTextSearchField(name, value, stored);
        //    document.Add(stringField);
        //    return stringField;
        //}

        public static FullTextSearchHighlightField AddFullTextSearchHighlightField(this Document document, string name, string value)
        {
            var stringField = new FullTextSearchHighlightField(name, value);
            document.Add(stringField);
            return stringField;
        }
    }

    #region Field
    //==> StringField
    //public sealed class SimpleSearchField : Field
    //{
    //    public static readonly FieldType TYPE_NOT_STORED = LoadTypeNotStored();

    //    public static readonly FieldType TYPE_STORED = LoadTypeStored();

    //    private static FieldType LoadTypeNotStored()
    //    {
    //        var fieldType = new FieldType
    //        {
    //            IsStored = false,
    //            //Simple Search
    //            IsIndexed = true,
    //            IsTokenized = false,
    //            OmitNorms = true,
    //            //No Highlight
    //            StoreTermVectors = false,
    //            StoreTermVectorPositions = false,
    //            StoreTermVectorOffsets = false,
    //            StoreTermVectorPayloads = false,
    //            IndexOptions = IndexOptions.DOCS_ONLY
    //        };
    //        fieldType.Freeze();
    //        return fieldType;
    //    }

    //    private static FieldType LoadTypeStored()
    //    {
    //        var fieldType = new FieldType
    //        {
    //            IsStored = true,
    //            //Simple Search
    //            IsIndexed = true,
    //            IsTokenized = false,
    //            OmitNorms = true,
    //            //No Highlight
    //            StoreTermVectors = false,
    //            StoreTermVectorPositions = false,
    //            StoreTermVectorOffsets = false,
    //            StoreTermVectorPayloads = false,
    //            IndexOptions = IndexOptions.DOCS_ONLY
    //        };
    //        fieldType.Freeze();
    //        return fieldType;
    //    }

    //    public SimpleSearchField(string name, string value, Store stored)
    //        : base(name, value, (stored == Store.YES) ? TYPE_STORED : TYPE_NOT_STORED)
    //    {
    //        //var maxLength = 200;
    //        //if (stored.IsStored() && value.Length > maxLength)
    //        //    throw new("Stored string fields is useful for short text, not long text.");
    //    }
    //}

    //TextField
    //public sealed class FullTextSearchField : Field
    //{
    //    public static readonly FieldType TYPE_NOT_STORED = LoadTypeNotStored();

    //    public static readonly FieldType TYPE_STORED = LoadTypeStored();

    //    private static FieldType LoadTypeNotStored()
    //    {
    //        var fieldType = new FieldType
    //        {
    //            IsStored = false,
    //            //Full-Text Search
    //            IsIndexed = true,
    //            IsTokenized = true,
    //            OmitNorms = false,
    //            //No Highlight
    //            StoreTermVectors = false,
    //            StoreTermVectorPositions = false,
    //            StoreTermVectorOffsets = false,
    //            StoreTermVectorPayloads = false,
    //            IndexOptions = IndexOptions.DOCS_AND_FREQS_AND_POSITIONS
    //        };
    //        fieldType.Freeze();
    //        return fieldType;
    //    }

    //    private static FieldType LoadTypeStored()
    //    {
    //        var fieldType = new FieldType
    //        {
    //            IsStored = true,
    //            //Full-Text Search
    //            IsIndexed = true,
    //            IsTokenized = true,
    //            OmitNorms = false,
    //            //No Highlight
    //            StoreTermVectors = false,
    //            StoreTermVectorPositions = false,
    //            StoreTermVectorOffsets = false,
    //            StoreTermVectorPayloads = false,
    //            IndexOptions = IndexOptions.DOCS_AND_FREQS_AND_POSITIONS
    //        };
    //        fieldType.Freeze();
    //        return fieldType;
    //    }

    //    public FullTextSearchField(string name, string value, Store stored)
    //        : base(name, value, (stored == Store.YES) ? TYPE_STORED : TYPE_NOT_STORED)
    //    {
    //        //var maxLength = 200;
    //        //if (stored.IsStored() && value.Length > maxLength)
    //        //    throw new("Stored string fields is useful for short text, not long text.");
    //    }
    //}

    public sealed class FullTextSearchHighlightField : Field
    {
        public static readonly FieldType TYPE = LoadType();

        private static FieldType LoadType()
        {
            var fieldType = new FieldType
            {
                //Full-Text Search
                IsIndexed = true,
                IsTokenized = true,
                OmitNorms = false,
                //Highlight
                IsStored = true,
                StoreTermVectors = true,
                StoreTermVectorPositions = true,
                StoreTermVectorOffsets = true,
                StoreTermVectorPayloads = true,
                IndexOptions = IndexOptions.DOCS_AND_FREQS_AND_POSITIONS_AND_OFFSETS
            };
            fieldType.Freeze();
            return fieldType;
        }

        public FullTextSearchHighlightField(string name, string value)
            : base(name, value, TYPE)
        {
        }
    }
    #endregion
}
#pragma warning restore S1481 // Unused local variables should be removed
#pragma warning restore S125 // Sections of code should not be commented out
