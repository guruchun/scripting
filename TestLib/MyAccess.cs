using Microsoft.VisualBasic.Logging;
using System.Data;
using System.Diagnostics;

namespace TestLib
{
    public class MyAccess
    {
        // static
        private static readonly MyAccess _myAccess = new();
        // public
        // private
        private readonly DataTable _dtTags = new("TabTable");

        // static
        public static MyAccess GetInstance()
        {
            return _myAccess;
        }

        // constructor
        private MyAccess()
        {
            // initialize data table
            string[] colName = { "Name", "Present", "SetNew", "Description"};
            string[] colType = { "System.String", "System.Object", "System.Object", "System.String"};
            //bool[] colReadOnly = { true, true, false, false, true, true, true, true };
            for (int i = 0; i < colName.Length; i++)
            {
                DataColumn col = new(colName[i], Type.GetType(colType[i]) ?? typeof(string));
                _dtTags.Columns.Add(col);
            }
            // for searching, filtering
            _dtTags.PrimaryKey = new DataColumn[] { _dtTags.Columns[0] };   // "Name"
        }

        // public


        // private


        public void SetTag<T>(string tag, T value)
        {
            // data table에서 tag에 해당되는 row 찾기
            try
            {
                DataRow? row = _dtTags.Rows.Find(tag);
                if (row != null)
                {
                    row["Present"] = value;
                } 
                else
                {
                    _dtTags.Rows.Add(new object[] { tag, value, null, "" });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"not defined the primary key on data table, tag={tag}, ex={ex.Message}");
            }
        }

        public T? GetTag<T>(string tag)
        {
            try
            {
                DataRow? row = _dtTags.Rows.Find(tag);
                if (row != null)
                {
                    object value = row["Present"];
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                else
                {
                    Debug.WriteLine($"not found tag on data table, tag={tag}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"not defined the primary key on data table, tag={tag}, ex={ex.Message}");
            }
            return default; // null?
        }
    }
}