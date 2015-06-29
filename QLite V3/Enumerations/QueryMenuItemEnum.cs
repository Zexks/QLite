using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using QLite.Objects.Query;
using QLite.Collections;

namespace QLite.Enumerations
{
    public class QueryMenuItemEnum : IEnumerable, IDisposable
    {
        QueryMenuItems list = new QueryMenuItems();
        int position = -1;

        public QueryMenuItemEnum(QueryMenuItems questionList)
        {
            list = questionList;
        }

        public IEnumerator GetEnumerator()
        {
            for (int x = 0; x < list.Count; x++)
                yield return list[x];
        }

        public bool MoveNext()
        {
            position++;
            if (position < list.Count)
                return true;
            return false;
        }

        public QueryMenuItem Current()
        {
            return list[position];
        }

        public void Reset()
        {
            position = -1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (GetEnumerator());
        }

        public void Dispose() { }
    }

}
