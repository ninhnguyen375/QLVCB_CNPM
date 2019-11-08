using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace webapi.Services {
    public class PaginatedList<T> : List<T> {
        public PaginatedList (IEnumerable<T> source, int current, int pageSize, int count, int totalPage) {
            _current = current;
            _pageSize = pageSize;
            _count = count;
            _totalPage = totalPage;
            this.AddRange (source);
        }
        public int _current { get; private set; }
        public int _pageSize { get; private set; }
        public int _count { get; private set; }
        public int _totalPage { get; private set; }

        public static Object Create (IEnumerable<T> source, int current, int pageSize) {
            int total = source.Count<T> ();
            var items = source.Skip ((current - 1) * pageSize).Take<T> (pageSize);
            int totalPage = (int) Math.Ceiling (total / (double) pageSize);
            var count = items.Count ();

            return new {
                data = items,
                    current,
                    pageSize,
                    count,
                    total,
                    totalPage
            };
        }
    }
}