using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using QLite.Objects.Query;
using QLite.Enumerations;

namespace QLite.Collections
{
    public class QueryMenuItems : CollectionBase
    {
        public QueryMenuItems() { }

        public void UpdateIndex()
        {
            foreach (QueryMenuItem item in this)
                if (item.Action != QAction.Delete && item.Index != this.IndexOf(item))
                { item.Index = this.IndexOf(item); item.Update_QuerySet(); item.Action = QAction.Update; }
        }

        public void LoadItemQueries()
        {
            foreach (QueryMenuItem item in this)
                item.Load_QuerySet();
        }

        public QueryMenuItem this[int index]
        {
            get { return (QueryMenuItem)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int IndexOf(QueryMenuItem item)
        {
            return base.List.IndexOf(item);
        }

        public int Add(QueryMenuItem item)
        {
            return this.List.Add(item);
        }

        public void Remove(QueryMenuItem item)
        {
            this.InnerList.Remove(item);
        }

        public void CopyTo(Array array, int index)
        {
            this.List.CopyTo(array, index);
        }

        public void AddRange(QueryMenuItems collection)
        {
            for (int i = 0; i < collection.Count; i++)
                this.List.Add(collection[i]);
        }

        public void AddRange(QueryMenuItem[] collection)
        {
            this.AddRange(collection);
        }

        public bool Contains(QueryMenuItem item)
        {
            return this.List.Contains(item);
        }

        public void Insert(int index, QueryMenuItem item)
        {
            this.List.Insert(index, item);
        }

        public QueryMenuItemEnum GetServerEnumerator()
        {
            return new QueryMenuItemEnum(this);
        }

    }
   
}
