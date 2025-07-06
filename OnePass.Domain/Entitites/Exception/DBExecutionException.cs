using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnePass.Domain
{
    public class DBExecutionException<T> : Exception
    {
        public T QueryValues { get; }
        public string QueryName => typeof(T).Name;

        public DBExecutionException(string message, Exception innerException, T queryValues)
            : base(message, innerException)
        {
            QueryValues = queryValues;
        }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
