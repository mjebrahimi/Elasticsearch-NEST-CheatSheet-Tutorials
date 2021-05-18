using System;
using System.Text;

namespace Sample
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            //LuceneSearch.Test();

            ElasticSearch.Test();

            Console.WriteLine("Hello World!");
        }
    }
}
