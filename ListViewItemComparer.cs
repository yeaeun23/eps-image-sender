using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageMgr
{
    class ListViewItemComparer : IComparer
    {
        private int col;
        public string sort = "asc";

        public ListViewItemComparer()
        {
            col = 0;
        }

        // <summary>컬럼과 정렬 기준(asc, desc)을 사용하여 정렬 함.</summary>
        // <param name="column">몇 번째 컬럼인지를 나타냄.</param>
        // <param name="sort">정렬 방법을 나타냄. Ex) asc, desc</param>
        public ListViewItemComparer(int column, string sort)
        {
            col = column;
            this.sort = sort;
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int StrCmpLogicalW(string x, string y);
        public int Compare(object x, object y)
        {
            if (((ListViewItem)x).SubItems[0].Text == "...")
                return -1;
            
            if (sort == "asc")
                return StrCmpLogicalW(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            else           
                return StrCmpLogicalW(((ListViewItem)y).SubItems[col].Text, ((ListViewItem)x).SubItems[col].Text);
        }        
    }
}
