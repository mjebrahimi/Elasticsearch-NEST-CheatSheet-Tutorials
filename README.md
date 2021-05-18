# Elasticsearch and NEST (.NET Client) Cheatsheet

> A collection of most used Queries, Methods Operations, and Concepts of Elasticsearch and NEST (.NET Client) with related refrences and articles.

## Get Started with Elasticsearch NEST in .NET
- https://code-maze.com/elasticsearch-aspnet-core/
- https://dzone.com/articles/elasticsearch-with-net-core
- https://schneide.blog/2020/08/31/using-elasticsearch-with-net-core/
- https://blexin.com/en/blog-en/how-to-integrate-elasticsearch-in-asp-net-core/
- https://www.red-gate.com/simple-talk/dotnet/net-development/how-to-build-a-search-page-with-elasticsearch-and-net/
- https://miroslavpopovic.com/posts/2018/07/elasticsearch-with-aspnet-core-and-docker
- https://www.methylium.com/articles/elastic-search-filters/
- https://www.emtec.digital/think-hub/blogs/accelerate-search-with-elasticsearch/
- https://mentormate.medium.com/elasticsearch-how-to-add-full-text-search-to-your-database-ee2f3ea4d3f3
- https://levelup.gitconnected.com/how-to-implement-full-text-search-in-net-application-whit-elasticsearch-4a2b0e6c0b59
- https://khalidabuhakmeh.com/search-experiences-for-your-aspnet-core-apps-with-elasticsearch
- https://www.javaer101.com/en/article/15506423.html
- https://www.c-sharpcorner.com/article/working-on-elasticsearch-using-net-nest/
- https://www.toptal.com/dot-net/elasticsearch-dot-net-developers
- https://www.codeproject.com/Articles/5250059/How-to-Implement-Full-Text-Search-in-NET-Applicati
- https://www.codeproject.com/Articles/1029482/A-Beginners-Tutorial-for-Understanding-and-Imple-2
- https://www.codeproject.com/Articles/1033116/A-Beginners-Tutorial-for-Understanding-and-Imple-3
- https://www.arcanys.com/blog/elasticsearch-net-using-nest-part1
- https://www.arcanys.com/blog/elasticsearch-net-using-nest-part2
- https://www.arcanys.com/blog/elasticsearch-net-using-nest-part3
- https://www.devbridge.com/articles/a-tutorial-getting-started-with-elastic-using-net-nest-library-part-one/
- https://www.devbridge.com/articles/getting-started-with-elastic-using-net-nest-library-part-two/
- https://www.devbridge.com/articles/getting-started-with-elastic-using-net-nest-library-part-three/
- https://www.devbridge.com/articles/getting-started-with-elastic-using-net-nest-library-part-four/
- https://www.elastic.co/blog/indexing-documents-with-the-nest-elasticsearch-net-client
- https://www.elastic.co/blog/how-jetbrains-uses-net-elasticsearch-csv-and-kibana-for-awesome-dashboards
### Repositories
- https://github.com/glautrou/ElasticSearchDemo
- https://github.com/khalidabuhakmeh/aspnet-core-elasticsearch-demo
- https://github.com/zeppaman/csharp-elastisearch-tutorial
- https://github.com/damienbod/AspNetCoreElasticsearchNestAuditTrail
- https://github.com/elastic/elasticsearch-net-example/tree/7.x
- https://github.com/elastic/elasticsearch-net


## Query
- https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/query-dsl.html

- **MatchAll**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-all-query.html

### Full Text Queries (Analyzed)
- https://www.elastic.co/guide/en/elasticsearch/reference/current/full-text-queries.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/full-text-queries.html
- https://opendistro.github.io/for-elasticsearch-docs/docs/elasticsearch/full-text/
- **Match**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/match-usage.html

  Returns documents that match a provided text, number, date or boolean value. The provided text is analyzed before matching.

  The match query is the standard query for performing a full-text search, including options for fuzzy matching.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.Match(c => c
      .Field(p => p.Description)
      .Query("hello world")
      //.Analyzer("standard")
      //.Operator(Operator.Or)
      //.MinimumShouldMatch(2)
      //.Fuzziness(Fuzziness.AutoLength(3, 6))
      //.FuzzyTranspositions()
      //.Name("named_query")
      //.AutoGenerateSynonymsPhraseQuery(false)
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "match": {
          "description": {
              "query": "hello world",
              //"analyzer": "standard",
              //"operator": "or",
              //"minimum_should_match": 2,
              //"fuzziness": "AUTO:3,6",
              //"fuzzy_transpositions": true,
              //"auto_generate_synonyms_phrase_query": false
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **MultiMatch**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-multi-match-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/multi-match-usage.html

  The multi_match query builds on the match query to allow multi-field queries.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.MultiMatch(c => c
      .Fields(f => f
          .Field(p => p.Description)
          .Field("myOtherField")
      )
      .Query("hello world")
      //.Analyzer("standard")
      //.Operator(Operator.Or)
      //.MinimumShouldMatch(2)
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "multi_match": {
          "query": "hello world",
          "fields": [
              "description",
              "myOtherField"
          ],
          //"analyzer": "standard",
          //"operator": "or",
          //"minimum_should_match": 2
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **MatchPhrase**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-query-phrase.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/match-phrase-usage.html

  Returns documents that contain all of the terms as a same order as input value.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.MatchPhrase(c => c
      .Field(p => p.Description)
      .Query("hello world")
      //.Analyzer("standard")
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "match_phrase": {
          "description": {
              "query": "hello world"
              //"analyzer": "standard",
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **MatchPhrasePrefix**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-query-phrase-prefix.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/match-phrase-prefix-usage.html

  Returns documents that contain the words of a provided text, in the same order as provided. The last term of the provided text is treated as a prefix, matching any words that begin with that term.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.MatchPhrasePrefix(c => c
      .Field(p => p.Description)
      .Query("hello worl")
      //.Analyzer("standard")
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "match_phrase_prefix": {
          "description": {
              "query": "hello worl",
              //"analyzer": "standard",
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **MatchBoolPrefix**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-match-bool-prefix-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/match-bool-prefix-usage.html

  A match_bool_prefix query analyzes its input and constructs a bool query from the terms. Each term except the last is used in a term query. The last term is used in a prefix query.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.MatchBoolPrefix(c => c
      .Field(p => p.Description)
      .Query("lorem ips")
      //.Analyzer("standard")
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "match_bool_prefix": {
          "description": {
              "query": "lorem ips",
              //"analyzer": "standard",
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **QueryString**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/query-string-usage.html

  Returns documents based on a provided query string, using a parser with a strict syntax.

  This query uses a syntax to parse and split the provided query string based on operators, such as AND or NOT. The query then analyzes each split text independently before returning matching documents.

  You can use the query_string query to create a complex search that includes wildcard characters, searches across multiple fields, and more. While versatile, the query is strict and returns an error if the query string includes any invalid syntax.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.QueryString(c => c
      .Fields(f => f
          .Field(p => p.Description)
          .Field("myOtherField")
      )
      .Query("hello world")
      //.DefaultOperator(Operator.Or)
      //.Analyzer("standard")
      //.QuoteAnalyzer("keyword")
      //.AnalyzeWildcard()
      //.AllowLeadingWildcard()
      //.Escape()
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "query_string": {
          "query": "hello world",
          "fields": [
              "description",
              "myOtherField"
          ],
          //"default_operator": "or",
          //"analyzer": "standard",
          //"quote_analyzer": "keyword",
          //"analyze_wildcard": true,
          //"allow_leading_wildcard": true,
          //"escape": true
      }
  }

  //Another Sample
  {
      "query": {
          "query_string": {
          "query": "(new york city) OR (big apple)",
          "default_field": "content"
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>

  <details>
  <summary><strong>Query String Syntax (Details)</strong></summary>

  https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html#query-string-syntax

  Where the status field contains active

  `status:active`

  Where the title field contains quick or brown

  `title:(quick OR brown)`

  Where the author field contains the exact phrase "john smith"

  `author:"John Smith"`

  Where the first name field contains Alice (note how we need to escape the space with a backslash)

  `first\ name:Alice`
  Where any of the fields book.title, book.content or book.date contains quick or brown (note how we need to escape the * with a backslash)

  `book.\*:(quick OR brown)`

  Where the field title has any non-null value:

  `_exists_:title`

  Wildcard searches can be run on individual terms, using ? to replace a single character, and * to replace zero or more characters

  `qu?ck bro*`

  Regular expression patterns can be embedded in the query string by wrapping them in forward-slashes ("/")

  `name:/joh?n(ath[oa]n)/`

  We can search for terms that are similar to, but not exactly like our search terms, using the “~” operator (default edit distance: 2)

  `quikc~ brwn~ foks~`

  The default edit distance is 2, but an edit distance of 1 should be sufficient to catch 80% of all human misspellings - avoid mixing fuzziness with wildcards

  `quikc~1`

  While a phrase query expects all of the terms in exactly the same order, a proximity query allows the specified words to be further apart or in a different order. proximity search allows us to specify a maximum edit distance of words in a phrase

  `"fox quick"~5`

  Ranges: Ranges can be specified for date, numeric or string fields. Inclusive ranges are specified with square brackets [min TO max] and exclusive ranges with curly brackets {min TO max}.
  All days in 2012
  
  `date:[2012-01-01 TO 2012-12-31]`

  Numbers 1..5

  `count:[1 TO 5]`

  Tags between alpha and omega, excluding alpha and omega

  `tag:{alpha TO omega}`

  Numbers from 10 upwards

  `count:[10 TO *]`

  Dates before 2012

  `date:{* TO 2012-01-01}`

  Numbers from 1 up to but not including 5 (Curly and square brackets can be combined)

  `count:[1 TO 5}`

  Ranges with one side unbounded can use the following syntax

  ```
  age:>10
  age:>=10
  age:<10
  age:<=10
  ```

  To combine an upper and lower bound with the simplified syntax, you would need to join two clauses with an AND operator:

  ```
  age:(>=10 AND <20)
  age:(+>=10 +<20)
  ```

  Use the boost operator ^ to make one term more relevant than another. The default boost value is 1, but can be any positive floating point number. Boosts between 0 and 1 reduce relevance.

  `quick^2 fox`

  Boosts can also be applied to phrases or to groups:

  `"john smith"^2   (foo bar)^4`

  By default, all terms are optional, as long as one term matches. A search for foo bar baz will find any document that contains one or more of foo or bar or baz. 
  The preferred operators are + (this term must be present) and - (this term must not be present).
  In this case: fox must be present - news must not be present - quick and brown are optional (their presence increases the relevance)

  `quick brown +fox -news`

  Boolean operators equivalent - the relevance scoring bears little resemblance to the original. (for boolean operators AND - OR - NOT parentheses should be specified)

  `((quick AND fox) OR (brown AND fox) OR fox) AND NOT news`

  Grouping: multiple terms or clauses can be grouped together with parentheses, to form sub-queries

  `(quick OR brown) AND fox`

  Groups can be used to target a particular field, or to boost the result of a sub-query

  `status:(active OR pending) title:(full text search)^2`

  </details>
- **SimpleQueryString**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-simple-query-string-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/simple-query-string-usage.html

  Returns documents based on a provided query string, using a parser with a limited but fault-tolerant syntax.
  
  This query uses a simple syntax to parse and split the provided query string into terms based on special operators. The query then analyzes each term independently before returning matching documents.

  While its syntax is more limited than the query_string query, the simple_query_string query does not return errors for invalid syntax. Instead, it ignores any invalid parts of the query string.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.SimpleQueryString(c => c
      .Fields(f => f.Field(p => p.Description).Field("myOtherField"))
      .Query("hello world")
      //.Analyzer("standard")
      //.DefaultOperator(Operator.Or)
      //.Flags(SimpleQueryStringFlags.And | SimpleQueryStringFlags.Near)
      //.AnalyzeWildcard()
      //.MinimumShouldMatch("30%")
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "simple_query_string": {
          "fields": [
              "description",
              "myOtherField"
          ],
          "query": "hello world",
          //"analyzer": "standard",
          //"default_operator": "or",
          //"flags": "AND|NEAR",
          //"analyze_wildcard": true,
          //"minimum_should_match": "30%",
      }
  }

  //Another Sample
  {
      "query": {
          "simple_query_string" : {
              "query": "\"fried eggs\" +(eggplant | potato) -frittata",
              "fields": ["title^5", "body"],
              "default_operator": "and"
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>

  <details>
  <summary><strong>Simple Query String Syntax (Details)</strong></summary>

  https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-simple-query-string-query.html#simple-query-string-syntax

  The simple_query_string query supports the following operators:
    - `+` signifies AND operation
    - `|` signifies OR operation
    - `-` negates a single token
    - `` ` ``  wraps a number of tokens to signify a phrase for searching
    - `*` at the end of a term signifies a prefix query
    - `(` and `)` signify precedence
    - `~N` after a word signifies edit distance (fuzziness)
    - `~N` after a phrase signifies slop amount

  This search is intended to only return documents containing foo or bar that also do not contain baz. However because of a default_operator of OR, this search actually returns documents that contain foo or bar and any documents that don’t contain baz. To return documents as intended, change the query string to foo bar +-baz.
  ```
  {
      "query": {
          "simple_query_string": {
              "fields": [ "content" ],
              "query": "foo bar -baz"
          }
      }
  }
  ```

  You can use the flags parameter to limit the supported operators for the simple query string syntax.

  To explicitly enable only specific operators, use a | separator. For example, a flags value of OR|AND|PREFIX disables all operators except OR, AND, and PREFIX.
  ```
  {
      "query": {
          "simple_query_string": {
              "query": "foo | bar + baz*",
              "flags": "OR|AND|PREFIX"
          }
      }
  }
  ```

  Wildcards and per-field boosts in the fields parameteredit
  Fields can be specified with wildcards. This query the title, first_name and last_name fields.
  ```
  {
      "query": {
          "simple_query_string" : {
              "query":    "Will Smith",
              "fields": [ "title", "*_name" ] 
          }
      }
  }
  ```

  Individual fields can be boosted with the caret (^) notation. The subject field is three times as important as the message field.
  ```
  {
      "query": {
          "simple_query_string" : {
              "query" : "this is a test",
              "fields" : [ "subject^3", "message" ] 
          }
      }
  }
  ```

  </details>
- **CommonTerms (Deprecated)**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-common-terms-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/common-terms-usage.html
- **Intervals**
    - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-intervals-query.html
    - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/intervals-usage.html

### Term Level Queries (NOT Analyzed)
- https://www.elastic.co/guide/en/elasticsearch/reference/current/term-level-queries.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/term-level-queries.html
- https://opendistro.github.io/for-elasticsearch-docs/docs/elasticsearch/term/
- **Term**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-term-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/term-query-usage.html

  Returns documents that contain an exact term in a provided field.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.Term(c => c
      .Field(p => p.Description)
      .Value("project description")
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "term": {
          "description": {
              "value": "project description"
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **Terms**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-terms-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/terms-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/terms-list-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/terms-lookup-query-usage.html

  Returns documents that contain one or more exact terms in a provided field.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.Terms(c => c
      .Field(p => p.Description)
      .Terms("term1", "term2")
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "terms": {
          "description": [
              "term1",
              "term2"
          ]
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **TermsSet**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-terms-set-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/terms-set-query-usage.html
- **Prefix**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-prefix-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/prefix-query-usage.html

  Returns documents that contain a specific prefix in a provided field.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.Prefix(c => c
      .Field(p => p.Description)
      .Value("proj")
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "prefix": {
          "description": {
              "value": "proj"
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **Exists**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-exists-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/exists-query-usage.html

  Returns documents that contain an indexed value for a field.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.Exists(c => c
      .Field(p => p.Description)
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "exists": {
          "field": "description"
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **Range**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-range-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/numeric-range-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/long-range-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/date-range-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/term-range-query-usage.html

  Returns documents that contain terms within a provided range.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.LongRange(c => c
      .Field(p => p.CommentCount)
      .GreaterThan(1)
      .GreaterThanOrEquals(2)
      .LessThan(3)
      .LessThanOrEquals(4)
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "range": {
          "description": {
              "gt": 1,
              "gte": 2,
              "lt": 3,
              "lte": 4
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **Ids**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-ids-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/ids-query-usage.html

  Returns documents based on their IDs. This query uses document IDs stored in the _id field.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.Ids(c => c
      .Values(1, 2, 3, 4)
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "ids": {
          "values": [ 1, 2, 3, 4]
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **Fuzzy**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-fuzzy-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/fuzzy-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/fuzzy-numeric-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/fuzzy-date-query-usage.html

  Returns documents that contain terms similar to the search term, as measured by a Levenshtein edit distance.
  
  To find similar terms, the fuzzy query creates a set of all possible variations, or expansions, of the search term within a specified edit distance. The query then returns exact matches for each expansion.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.Fuzzy(c => c
      .Field(p => p.Description)
      .Fuzziness(Fuzziness.Auto)
      .Value("ki")
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "fuzzy": {
          "description": {
          "fuzziness": "AUTO",
          "value": "ki"
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **Wildcard**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-wildcard-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/wildcard-query-usage.html

  Returns documents that contain terms matching a wildcard pattern.

  A wildcard operator is a placeholder that matches one or more characters. For example, the * wildcard operator matches zero or more characters. You can combine wildcard operators with other characters to create a wildcard pattern.
  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.Wildcard(c => c
      .Field(p => p.Description)
      .Value("p*oj")
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
      "wildcard": {
          "description": {
              "value": "p*oj"
          }
      }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **Regexp**
    - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-regexp-query.html
    - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/regexp-query-usage.html
    - https://www.elastic.co/guide/en/elasticsearch/reference/current/regexp-syntax.html

    Returns documents that contain terms matching a regular expression.

    A regular expression is a way to match patterns in data using placeholder characters, called operators. For a list of operators supported by the regexp query, see [Regular expression syntax](https://www.elastic.co/guide/en/elasticsearch/reference/current/regexp-syntax.html).
    <table>
    <thead>
    <tr>
    <th>NEST Fluent Syntax</th>
    <th style="width:100%">ElasticSearch DSL Syntax</th>
    </tr>
    </thead>
    <tbody>
    <tr>
    <td style="vertical-align: top">

    ```cs
    q.Regexp(c => c
        .Field(p => p.Description)
        .Value("s.*y")
    )
    ```

    </td>
    <td style="vertical-align: top">

    ```json
    {
        "regexp": {
            "description": {
                "value": "s.*y"
            }
        }
    }
    ```

    </td>
    </tr>
    </tbody>
    </table>

### Compound Queries
- https://www.elastic.co/guide/en/elasticsearch/reference/current/compound-queries.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/compound-queries.html
- https://opendistro.github.io/for-elasticsearch-docs/docs/elasticsearch/bool/
- **Bool (Must, Should, MustNot, Filter)**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-bool-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-queries.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-dsl-complex-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-filter-context.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/filter-search-results.html

  A query that matches documents matching boolean combinations of other queries. It is built using one or more boolean clauses, each clause with a typed occurrence. The occurrence types are:

  - **AND** (clause `must` - binary `&&` operator):

    The clause (query) must appear in matching documents and will contribute to the score.

  - **FILTER** (clause `filter` - unary `+` operator):

    The clause (query) must appear in matching documents. However unlike must, the score of the query will be ignored.

  - **OR** (clause `should` - binary `||` operator):

    The clause (query) should appear in the matching document.

  - **NOT** (clause `must_not` - unary `!` operator):

    The clause (query) must not appear in the matching documents.

  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
  q.Query(q => q
      .Bool(b => b
          .Should(
              bs => bs.Term(p => p.Title, "x"),
              bs => bs.Term(p => p.Body, "y")
          )
      )
  )
  
  //Operator overloading
  q.Query(q => q
      .Term(p => p.Title, "x") || q
      .Term(p => p.Body, "y")
  )

  //Another Sample
  q.Query(q => q
      .Bool(b => b
          .MustNot(m => m.MatchAll())
          .Should(m => m.MatchAll())
          .Must(m => m.MatchAll())
          .Filter(f => f.MatchAll())
       )
  )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
    "query": {
      "bool": {
        "should": [
          {
            "term": {
              "title": {
                "value": "x"
              }
            }
          },
          {
            "term": {
              "body": {
                "value": "y"
              }
            }
          }
        ]
      }
    }
  }

  //Another Sample
  {
    "bool": {
      "must": [
        {
          "match_all": {}
        }
      ],
      "must_not": [
        {
          "match_all": {}
        }
      ],
      "should": [
        {
          "match_all": {}
        }
      ],
      "filter": [
        {
          "match_all": {}
        }
      ]
    }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>
- **Boosting**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-boosting-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/boosting-query-usage.html

  Returns documents matching a positive query while reducing the relevance score of documents that also match a negative query.

  You can use the boosting query to demote certain documents without excluding them from the search results.

  <table>
  <thead>
  <tr>
  <th>NEST Fluent Syntax</th>
  <th style="width:100%">ElasticSearch DSL Syntax</th>
  </tr>
  </thead>
  <tbody>
  <tr>
  <td style="vertical-align: top">

  ```cs
   q.Boosting(c => c
        .Positive(qq => qq
            .Term(c => c
                .Field(p => p.Text)
                .Value("Positive Term")
            )
        )
        .Negative(qq => qq
            .Term(c => c
                .Field(p => p.Text)
                .Value("Negative Term")
            )
        )
        .NegativeBoost(1.5)
    )
  ```

  </td>
  <td style="vertical-align: top">

  ```json
  {
    "query": {
      "boosting": {
        "positive": {
          "term": {
            "text": "Positive Term"
          }
        },
        "negative": {
          "term": {
            "text": "Negative Term"
          }
        },
        "negative_boost": 1.5
      }
    }
  }
  ```

  </td>
  </tr>
  </tbody>
  </table>

### Others
- https://www.elastic.co/guide/en/elasticsearch/reference/current/specialized-queries.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/specialized-queries.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/nest-specific-queries.html

- **MoreLikeThis**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-mlt-query.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/more-like-this-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/more-like-this-full-document-query-usage.html

    The More Like This Query finds documents that are "like" a given set of documents. In order to do so, MLT selects a set of representative terms of these input documents, forms a query using these terms, executes the query and returns the results. The user controls the input documents, how the terms should be selected and how the query is formed.

- **Nested**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/nested.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/flattened.html

    The nested type is a specialised version of the object data type that allows arrays of objects to be indexed in a way that they can be queried independently of each other.

    When ingesting key-value pairs with a large, arbitrary set of keys, you might consider modeling each key-value pair as its own nested document with key and value fields. Instead, consider using the flattened data type, which maps an entire object as a single field and allows for simple searches over its contents. Nested documents and queries are typically expensive, so using the flattened data type for this use case is a better option.

  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/nested-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-nested-query.html

    The nested query searches nested field objects as if they were indexed as separate documents. If an object matches the search, the nested query returns the root parent document.

- **Raw Query/Combine**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/raw-query-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/raw-combine-usage.html

## Aggregations
- **Reference**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-metrics.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-pipeline.html
- **NEST (.NET Client)**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/aggregations.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/writing-aggregations.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/reserved-aggregation-names.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/reference-aggregations.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bucket-aggregations.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/metric-aggregations.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/pipeline-aggregations.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/matrix-aggregations.html

### Buket Aggregations (Group By)
- **Range**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-range-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/range-aggregation-usage.html
- **Date Range**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-daterange-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/date-range-aggregation-usage.html
- **Filter**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-filter-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/filter-aggregation-usage.html
- **Filters**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-filters-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/filters-aggregation-usage.html
- **Terms**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-terms-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/terms-aggregation-usage.html
- **Multi Terms**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-multi-terms-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/multi-terms-aggregation-usage.html
- **Nested**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-nested-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/nested-aggregation-usage.html
- **Reverse Nested**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-reverse-nested-aggregation.html
- **Parent**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-parent-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/parent-aggregation-usage.html
- **Children**
    - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-children-aggregation.html
    - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/children-aggregation-usage.html

### Metrics Aggregations
- **Min**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-metrics-min-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/min-aggregation-usage.html
- **Max**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-metrics-max-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/max-aggregation-usage.html
- **Average**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-metrics-avg-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/average-aggregation-usage.html
- **Sum**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-metrics-sum-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/sum-aggregation-usage.html
- **Stats**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-metrics-stats-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/stats-aggregation-usage.html
- **String Stats**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-metrics-string-stats-aggregation.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/string-stats-aggregation-usage.html
- **Value Count**
    - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-metrics-valuecount-aggregation.html
    - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/value-count-aggregation-usage.html

## Search
- **Overview**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/search.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/writing-queries.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-queries.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/returned-fields.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/scrolling-documents.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/reference-search.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-your-data.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-search.html
- **From and Size (Skip and Take) - Pagination**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/from-and-size-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/paginate-search-results.html
- **Fields (StoredFields)**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/fields-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/source-filtering-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-fields.html
- **Inner Hits (Parent/Childs and Nested)**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/inner-hits-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/inner-hits.html
- **Indices Boost**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/indices-boost-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-multiple-indices.html
- **Sliced Scroll**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/sliced-scroll-search-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/scroll-api.html
- **Sort**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/sort-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/sort-search-results.html
- **Min Score**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/min-score-usage.html
- **Explain**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/explain-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-explain.html
- **Profile**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/profile-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-profile.html
- **Highlighting**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/highlighting-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/highlighting.html
- **Suggest**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/suggest-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-suggesters.html
- **Post Filter**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/post-filter-usage.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/filter-search-results.html

## Other Search APIs 
- **Count**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-count.html
- **Multi Search**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/search-multi-search.html
- **Async Search**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/async-search.html

## Indexing Documents
### Indexing (IndexDocument, Index, IndexMany, Bulk, BulkAll)
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/indexing-documents.html
### ReIndexinx (ReindexOnServer, Reindex)
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/reindexing-documents.html

## Mapping
### NEST (.NET Client)
- **Attribute Mapping - Fluent Mapping**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/auto-map.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/attribute-mapping.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/fluent-mapping.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/ignoring-properties.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/multi-fields.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/modelling-documents-with-types.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/ids-inference.html
- **Parent/Child Relation**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/parent-child-relationships.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/types-and-relations-inference.html
- **Visitor Pattern**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/visitor-pattern-mapping.html
### Reference
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/dynamic-mapping.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/runtime.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/explicit-mapping.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-types.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-fields.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-params.html

### Data Types
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-types.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/binary.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/boolean.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/keyword.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/number.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/date.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/date_nanos.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/range.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/ip.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/version.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/text.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/search-suggesters.html#completion-suggester
- https://www.elastic.co/guide/en/elasticsearch/reference/current/search-as-you-type.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/array.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/object.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/flattened.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/nested.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/parent-join.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/multi-fields.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/field-alias.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/token-count.html
- https://www.elastic.co/guide/en/elasticsearch/plugins/current/mapper-annotated-text.html

### Mapping Params
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-params.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-index.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-store.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/enabled.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/index-options.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/term-vector.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/highlighting.html [#unified-highlighter] [#plain-highlighter] [#fast-vector-highlighter]
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-date-format.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/index-phrases.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/index-prefixes.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/search-analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/normalizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-boost.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/properties.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/null-value.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/ignore-above.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/enabled.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/multi-fields.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/copy-to.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/coerce.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/norms.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-field-meta.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/ignore-malformed.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/eager-global-ordinals.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/similarity.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/position-increment-gap.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/dynamic.html

## NEST (.NET Client)
### Connecting
- **ConnectionPool**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/connection-pooling.html
- **Connection**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/modifying-default-connection.html
- **ConnectionConfiguration and ConnectionSettings**
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/configuration-options.html
  - https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/working-with-certificates.html
### Conventions
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/index-name-inference.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/indices-paths.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/field-inference.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/document-paths.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/routing-inference.html
### Custom Serialization
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/custom-serialization.html

## Indices
- https://www.elastic.co/guide/en/elasticsearch/reference/current/indices.html
- **Create**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-create-index.html
- **Delete**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-delete-index.html
- **Get**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-get-index.html
- **Exists**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-exists.html
- **Flush**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-flush.html
- **Get Settings**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-get-settings.html
- **Update Settings**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-update-settings.html
- **Get Mapping and Field Mapping**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-get-mapping.html
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-get-field-mapping.html
- **Update Mappings**
  - https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-put-mapping.html


## Text Analyzers
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/testing-analyzers.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/test-analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/writing-analyzers.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/configuring-analyzers.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-custom-analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/specify-analyzer.html

### Concepts
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-overview.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analyzer-anatomy.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-index-search-time.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/stemming.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/token-graphs.html

### Built-in Analyzers
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-analyzers.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-standard-analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-simple-analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-whitespace-analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-stop-analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-lang-analyzer.html [#persian-analyzer]
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-keyword-analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-pattern-analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-fingerprint-analyzer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-normalizers.html

### Character Filters
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-charfilters.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-htmlstrip-charfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-mapping-charfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-pattern-replace-charfilter.html

### Tokenizer
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-tokenizers.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-standard-tokenizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-whitespace-tokenizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-classic-tokenizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-letter-tokenizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-lowercase-tokenizer.html
- **Others**
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-chargroup-tokenizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-uaxurlemail-tokenizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-keyword-tokenizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-pattern-tokenizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-simplepattern-tokenizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-simplepatternsplit-tokenizer.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-pathhierarchy-tokenizer.html

### Token Filters
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-tokenfilters.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-lowercase-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-uppercase-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-stop-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-trim-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-decimal-digit-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-normalization-tokenfilter.html
- **Others**
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-synonym-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-synonym-graph-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-shingle-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-common-grams-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-truncate-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-length-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-unique-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-remove-duplicates-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-keep-words-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-keep-types-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-multiplexer-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-stemmer-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-porterstem-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-kstem-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-stemmer-override-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-keyword-marker-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-keyword-repeat-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-pattern_replace-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-pattern-capture-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-condition-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-asciifolding-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-apostrophe-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-classic-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-limit-token-count-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-ngram-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-edgengram-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-word-delimiter-tokenfilter.html
- https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-word-delimiter-graph-tokenfilter.html

## Other Articles About Elasticsearch Queries
- https://moliware.com/es-dsl-cheatsheet/
- https://elasticsearch-cheatsheet.jolicode.com/
- https://qbox.io/blog/elasticsearch-queries-match-phrase-match/
- https://www.devinline.com/2018/01/full-text-query-in-elasticsearch-using-match.html
- https://stackoverflow.com/questions/26001002/elasticsearch-difference-between-term-match-phrase-and-query-string
- https://stackoverflow.com/questions/42451527/difference-between-match-and-multimatch-query-type-in-elasticsearch


