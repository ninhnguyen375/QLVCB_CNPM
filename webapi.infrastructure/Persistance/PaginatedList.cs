using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace webapi.infrastructure.Persistance {
    public class PaginatedList<T> : List<T> {
        public PaginatedList (IEnumerable<T> source, int page, int offset, int count, int totalCount) {
            Page = page;
            Offset = offset;
            CountInPage = count;
            TotalCount = totalCount;
            this.AddRange (source);
        }
        public int Page { get; private set; }
        public int Offset { get; private set; }
        public int CountInPage { get; private set; }
        public int TotalCount { get; private set; }

        public static Object Create (IEnumerable<T> source, int page, int offset) {
            int totalCount = source.Count<T> ();
            var items = source.Skip ((page - 1) * offset).Take<T> (offset);

            int totalPage = (int) Math.Ceiling (totalCount / (double) offset);

            var count = items.Count ();
            return new {
                data = items,
                    page,
                    offset,
                    count,
                    totalCount
            };
        }
    }
}