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

# ElasticSearch Important Tips Summary

## Some Tips

1. Using logical operators (&&, !, ||, +) instead of (Must, MustNot, Should, Filter) is simpler but more prone to bug and a little trickly. Indeed It is easier to use logical operators, but if you are not careful enough you can easily make a mistake.
2. Using Filter (instead of Must) can be useful in improving performance where the relevancy score for the query is not required to affect the order of results.
3. Do not use Term for checking with null (is null or is not null). instead use Exists for checking with null.
4. If you whant to null check a term fields. use null_value option to indexing null values.
5. Use doc_values=false when you don't need to sort or aggregate on a field. 
6. Use index=false when you don't need to search on a field.
7. Use norms=false when you don't need to scoring on a field.

## object

JSON documents are hierarchical in nature: the document may contain inner objects which, in turn, may contain inner objects themselves.

But elasticsearch has no concept of inner objects. Therefore, it flattens object hierarchies into a simple list of field names and values.

Object is default type for any undefined types, so you are not required to set the field type to object explicitly.

If you need to index arrays of objects instead of single objects, read Nested first.

The following parameters are accepted by object fields:

- **dynamic**

    Whether or not new properties should be added dynamically to an existing object. Accepts true (default), runtime, false and strict.
- **enabled**

    Whether the JSON value given for the object field should be parsed and indexed (true, default) or completely ignored (false).

- **properties**

    The fields within the object, which can be of any data type, including object. New properties may be added to an existing object.

## nested

Elasticsearch has no concept of inner objects. Therefore, it flattens object hierarchies into a simple list of field names and values.

The nested type is a specialised version of the object data type that allows arrays of objects to be indexed in a way that they can be queried independently of each other.

When you need to index arrays of objects and query them independently consider to use Nested.

If you use Must (And &&), Filter (+) query on inner types that is not nested, queries would match incorrectly documents.

Nested types can not be queried in non-nested queries. Do not forget to use AutoMap when mapping Nested() with attribute mapping in for inner properties.

When ingesting key-value pairs with a large, arbitrary set of keys, you might consider modeling each key-value pair as its own nested document with key and value fields.

Instead, consider using the flattened data type, which maps an entire object as a single field and allows for simple searches over its contents.

Nested documents and queries are typically expensive, so using the flattened data type for this use case is a better option.

Nested documents can be:

- queried with the nested query.
- analyzed with the nested and reverse_nested aggregations.
- sorted with nested sorting.
- retrieved and highlighted with nested inner hits.

Because nested documents are indexed as separate documents, they can only be accessed within the scope of the nested query, the nested/reverse_nested aggregations, or nested inner hits.

For instance, if a string field within a nested document has index_options set to offsets to allow use of the postings during the highlighting, these offsets will not be available during the main highlighting phase.

Instead, highlighting needs to be performed via nested inner hits. The same consideration applies when loading fields during a search through docvalue_fields or stored_fields.

The following parameters are accepted by nested fields:

- **dynamic**

    (Optional, string) Whether or not new properties should be added dynamically to an existing nested object. Accepts true (default), false and strict.

- **properties**

    (Optional, object) The fields within the nested object, which can be of any data type, including nested. New properties may be added to an existing nested object.

- **include_in_parent**

    (Optional, Boolean) If true, all fields in the nested object are also added to the parent document as standard (flat) fields. Defaults to false.

- **include_in_root**

    (Optional, Boolean) If true, all fields in the nested object are also added to the root document as standard (flat) fields. Defaults to false.

## flattened

By default, each subfield in an object is mapped and indexed separately.

If the names or types of the subfields are not known in advance, then they are mapped dynamically.

The flattened type provides an alternative approach, where the entire object is mapped as a single field.

Given an object, the flattened mapping will parse out its leaf values and index them into one field as keywords.

The object’s contents can then be searched through simple queries and aggregations.

This data type can be useful for indexing objects with a large or unknown number of unique keys.

Only one field mapping is created for the whole JSON object, which can help prevent a mappings explosion from having too many distinct field mappings.

On the other hand, flattened object fields present a trade-off in terms of search functionality. Only basic queries are allowed, with no support for numeric range queries or highlighting.

Further information on the limitations can be found in the Supported operations section.

The flattened mapping type should not be used for indexing all document content, as it treats all values as keywords and does not provide full search functionality.

The default approach, where each subfield has its own entry in the mappings, works well in the majority of cases.

## keyword

keyword, which is used for structured content such as IDs, email addresses, hostnames, status codes, zip codes, or tags.

Keyword fields are often used in sorting, aggregations, and term-level queries, such as term.

Avoid using keyword fields for full-text search. Use the text field type instead.
Not all numeric data should be mapped as a numeric field data type.

Elasticsearch optimizes numeric fields, such as integer or long, for range queries.

However, keyword fields are better for term and other term-level queries.
Identifiers, such as an ISBN or a product ID, are rarely used in range queries.

 However, they are often retrieved using term-level queries.
Consider mapping a numeric identifier as a keyword if:

You don’t plan to search for the identifier data using range queries.
Fast retrieval is important. term query searches on keyword fields are often faster than term searches on numeric fields.

If you’re unsure which to use, you can use a multi-field to map the data as both a keyword and a numeric data type.

**Conclusion**:

1. keyword data type is optimized for term-level (or sorting/aggregation) queries.
2. Use keyword datatype for fields that you would like to use term-level (or sorting/aggregation) queries on that.
3. If use range queries on numeric fields use numeric types (long, ...)
4. Else if use term-level (or sorting/aggregation) queries on numeric fields, use keyword data type instead.
5. If you’re unsure which to use, you can use a multi-field to map the data as both a keyword and a numeric data type.

## text

The popular type for full-text content such as the body of an email or the description of a product.

Text fields are not used for sorting and seldom used for aggregations. (not optimized for this, use keyword instead)

Field data is the only way to access the analyzed tokens from a full text field in aggregations, sorting, or scripting. For example, a full text field like New York would get analyzed as new and york. To aggregate on these tokens requires field data.

https://www.elastic.co/guide/en/elasticsearch/reference/current/text.html#fielddata-mapping-param

## analyzer

The analyzer which should be used for the text field, both at index-time and at search-time (unless overridden by the search_analyzer).

Defaults to the default index analyzer, or the standard analyzer.

## normalizer

The normalizer property of keyword fields is similar to analyzer for text fields and applied prior to indexing the keyword, as well as at search-time when the keyword field is searched via a query parser such as the match query or via a term-level query such as the term query.

A simple normalizer called lowercase ships with elasticsearch and can be used. Custom normalizers can be defined as part of analysis settings as follows.

## boost

Mapping field-level query time boosting. Accepts a floating point number, defaults to 1.0. **Deprecated in 5.0.0.**

Index time boost is deprecated. Instead, the field mapping boost is applied at query time. For indices created before 5.0.0, the boost will still be applied at index time.

**Why index time boosting is a bad idea**

We advise against using index time boosting for the following reasons:
You cannot change index-time boost values without reindexing all of your documents.

Every query supports query-time boosting which achieves the same effect. The difference is that you can tweak the boost value without having to reindex.

Index-time boosts are stored as part of the norm, which is only one byte. This reduces the resolution of the field length normalization factor which can lead to lower quality relevance calculations.

## doc_values

Should the field be stored on disk in a column-stride fashion, so that it can later be used for sorting, aggregations, or scripting? Accepts true (default) or false.

All fields which support doc values have them enabled by default. If you are sure that you don’t need to sort or aggregate on a field, or access the field value from a script, you can disable doc values in order to save disk space

## eager_global_ordinals

Should global ordinals be loaded eagerly on refresh? Accepts true or false (default).
Enabling this is a good idea on fields that are frequently used for terms aggregations.

## fields

Multi-fields allow the same string value to be indexed in multiple ways for different purposes, This is the purpose of multi-fields.

For instance, a string field could be mapped as a text field for full-text search, and as a keyword field for sorting or aggregations.

Another use case of multi-fields is to analyze the same field in different ways for better relevance.

For instance we could index a field with the standard analyzer which breaks text up into words, and again with the english analyzer which stems words into their root form

## ignore_above

Do not index any string longer than this value. Defaults to 2147483647 so that all values would be accepted.

Please however note that default dynamic mapping rules create a sub keyword field that overrides this default by setting ignore_above: 256.

Strings longer than the ignore_above setting will not be indexed or stored.

For arrays of strings, ignore_above will be applied for each array element separately and string elements longer than ignore_above will not be indexed or stored.

## index

Should the field be searchable? Accepts true (default) or false. Fields that are not indexed are not queryable.

## index_phrases

If enabled, two-term word combinations (shingles) are indexed into a separate field.

This allows exact phrase queries (no slop) to run more efficiently, at the expense of a larger index.

Note that this works best when stopwords are not removed, as phrases containing stopwords will not use the subsidiary field and will fall back to a standard phrase query. Accepts true or false (default).

## position_increment_gape

Analyzed text fields take term positions into account, in order to be able to support proximity or phrase queries.

When indexing text fields with multiple values a "fake" gap is added between the values to prevent most phrase queries from matching across the values.

The size of this gap is configured using position_increment_gap and defaults to 100.

## index_prefixese

The index_prefixes parameter enables the indexing of term prefixes to speed up prefix searches.

It accepts the following optional settings:

- **min_chars:**

    The minimum prefix length to index. Must be greater than 0, and defaults to 2. The value is inclusive.

- **max_chars:**

    The maximum prefix length to index. Must be less than 20, and defaults to 5. The value is inclusive.

## index_options

The index_options parameter controls what information is added to the inverted index for search and highlighting purposes.

The index_options parameter is intended for use with text fields only. Avoid using index_options with other field data types. (it can be used for keyword data type too!)

Use index_options [positions] for normal searching that supports [frequencies] to score highest repeated terms, also supports [positions] can be used for proximity or phrase queries.

Use [offsets] that is the most complete and also supports [ofsets] which are used by the unified highlighter to speed up highlighting.

## norms

Norms store various normalization factors that are later used at query time in order to compute the score of a document relatively to a query.

if you don’t need scoring on a specific field, you should disable norms on that field. In particular, this is the case for fields that are used solely for filtering or aggregations.

## null_value

A null value cannot be indexed or searched. When a field is set to null, (or an empty array or an array of null values) it is treated as though that field has no values.

The null_value parameter allows you to replace explicit null values with the specified value so that it can be indexed and searched.

The null_value needs to be the same data type as the field. For instance, a long field cannot have a string null_value.

The null_value only influences how data is indexed, it doesn’t modify the _source document.

## store

Whether the field value should be stored and retrievable separately from the _source field. Accepts true or false (default).

By default, field values are indexed to make them searchable, but they are not stored. This means that the field can be queried, but the original field value cannot be retrieved.

Usually this doesn’t matter. The field value is already part of the _source field, which is stored by default.

If you only want to retrieve the value of a single field or of a few fields, instead of the whole _source, then this can be achieved with source filtering.

https://www.elastic.co/guide/en/elasticsearch/reference/current/search-fields.html#source-filtering

In certain situations it can make sense to store a field. For instance, if you have a document with a title, a date, and a very large content field, you may want to retrieve just the title and the date without having to extract those fields from a large _source field

Stored fields returned as arrays:

For consistency, stored fields are always returned as an array because there is no way of knowing if the original field value was a single value, multiple values, or an empty array.

If you need the original value, you should retrieve it from the _source field instead.

## similarity

Which scoring algorithm or similarity should be used. Defaults to BM25.

## analyzer

Only text fields support the analyzer mapping parameter.

The analyzer parameter specifies the analyzer used for text analysis when indexing or searching a text field.

Unless overridden with the search_analyzer mapping parameter, this analyzer is used for both index and search analysis.

## search_analyzere

Usually, the same analyzer should be applied at index time and at search time, to ensure that the terms in the query are in the same format as the terms in the inverted index.

Sometimes, though, it can make sense to use a different analyzer at search time, such as when using the edge_ngram tokenizer for autocomplete or when using search-time synonyms.

By default, queries will use the analyzer defined in the field mapping, but this can be overridden with the search_analyzer setting

## search_quote_analyzer

The search_quote_analyzer setting allows you to specify an analyzer for phrases, this is particularly useful when dealing with disabling stop words for phrase queries.

To disable stop words for phrases a field utilising three analyzer settings will be required:

1. An analyzer setting for indexing all terms including stop words
2. A search_analyzer setting for non-phrase queries that will remove stop words
3. A search_quote_analyzer setting for phrase queries that will not remove stop words

## term_vectore

Term vectors contain information about the terms produced by the analysis process, including:

- a list of terms.
- the position (or order) of each term.
- the start and end character offsets mapping the term to its origin in the original string.
- payloads (if they are available) — user-defined binary data associated with each term position.
These term vectors can be stored so that they can be retrieved for a particular document.
The term_vector setting accepts:
- no: No term vectors are stored. (default)
- yes: Just the terms in the field are stored.
- with_positions: Terms and positions are stored.
- **with_offsets**: Terms and character offsets are stored.
- **with_positions_offsets**: Terms, positions, and character offsets are stored.
- **with_positions_payloads**: Terms, positions, and payloads are stored.
- **with_positions_offsets_payloads**:  Terms, positions, offsets and payloads are stored.
The fast vector highlighter requires with_positions_offsets. The term vectors API can retrieve whatever is stored.
Setting with_positions_offsets will double the size of a field’s index.