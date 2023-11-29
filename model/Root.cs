using System.Collections.Generic;

namespace Municipios.model
{
    public class C
    {
        public object v { get; set; }
        public string f { get; set; }
    }
    public class Col
    {
        public string id { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string pattern { get; set; }
    }

    public class Root
    {
        public string version { get; set; }
        public string reqId { get; set; }
        public string status { get; set; }
        public string sig { get; set; }
        public string url { get; set; }
        public Table table { get; set; }
    }

    public class Row
    {
        public List<C> c { get; set; }
    }

    public class Table
    {
        public List<Col> cols { get; set; }
        public List<Row> rows { get; set; }
        public int parsedNumHeaders { get; set; }
    }
}
