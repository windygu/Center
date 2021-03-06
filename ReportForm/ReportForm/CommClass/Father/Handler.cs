using System;
using System.Collections;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace ReportForm.CommClass.Father
{
    abstract public class Handler : I_Report
    {
        #region ==== 變數 ====
        private int width = 0;
        private int height = 0;
        private int titleCnt = 0;
        private int nonShow = -1;
        private int reportColCnt = 0;
        protected System.Collections.Generic.List<int> perList = null;
        protected System.Collections.Generic.List<int> cellWidth = new System.Collections.Generic.List<int>();
        #endregion ==== 變數 ====

        #region ==== 屬性 ====
        public int Width
        {
            set { width = value; }
            get { return width; }
        }

        public int Height
        {
            set { height = value; }
            get { return height; }
        }

        public int TitleCnt
        {
            set { titleCnt = value; }
            get { return titleCnt; }
        }

        public int ReportColCnt
        {
            set { reportColCnt = value; }
            get { return reportColCnt; }
        }

        /// <summary>
        /// 不顯示的欄位
        /// </summary>
        public int NonShow
        {
            set { nonShow = value; }
            get { return nonShow; }
        }
        #endregion ==== 屬性 ====

        public System.Collections.Generic.List<int> GetCellWidth(System.Collections.Generic.List<int> percentList, int ColCnt)
        {
            if (percentList == null || percentList.Count != ColCnt)
            {
                for (int i = 0; i < ColCnt; i++)
                {
                    cellWidth.Add(Width / ColCnt);
                }
            }
            else
            {
                int percent = 0;
                for (int p = 0; p < percentList.Count; p++) percent += percentList[p];

                if (percent == 100)
                {
                    for (int p = 0; p < percentList.Count; p++)
                    {
                        cellWidth.Add((int)(((double)percentList[p] / 100) * Width));
                    }
                }
                else
                    throw new Exception("百分比設定錯誤!總加未逹100%");
            }
            return cellWidth;
        }

        abstract public XRTable GetHandler(ArrayList arrList);
    }
}
