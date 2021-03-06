using System;
using System.Collections.Generic;
using System.Text;
using ReportForm;
using ReportForm.CommClass.Handler;
using ReportForm.CommClass.Main;
using ReportForm.CommClass.Minor;
using System.Data;
using System.Management;
using DevExpress.XtraPrinting.Localization;
using System.IO;

namespace ReportForm
{   
    /// <summary>
    /// 本類別為主要實做ReportSever的類別
    /// </summary>
    public class ReportBuilder
    {
        System.Threading.Timer timer = null;
        string section;
        string SavePath;
        string FileName;
        

        #region ==== 建構元 ====
        public ReportBuilder()
        {
            //timer = new System.Threading.Timer(new System.Threading.TimerCallback(Detector));            
        }
        #endregion ==== 建構元 ====

        #region ==== Public 方法 =====
        public void PrintRoport(int aReport)
        {
            //            ReportInterface.RptHander aReportName
            ReportForm.RptHander aReportName = (ReportForm.RptHander)Enum.Parse(typeof(ReportForm.RptHander), aReport.ToString());
            System.Data.DataTable dtDev = new DataTable();
            System.Data.DataTable dt = new DataTable();
            System.Collections.ArrayList arrListRpt = new System.Collections.ArrayList();
            List<string> list=new List<string>();
            list.Add("CMS");
            string user = "('排程')";
            DateTime starTime=DateTime.Now.AddHours(-2);
            DateTime endTime=DateTime.Now;

            GetData(aReportName, user, list,starTime, endTime, ref dtDev, ref dt, ref arrListRpt);
            PrintRoport(aReportName, arrListRpt, dt, dtDev, user, starTime, endTime);
        }

        private void PrintRoport(ReportForm.RptHander aReportName, System.Collections.ArrayList arrList, System.Data.DataTable dt, System.Data.DataTable dtDev, string sUser, DateTime sDate, DateTime eDate)
        {
            string sReportName = aReportName.ToString();
            string startDate = sDate.ToString("yyyy-MM-dd HH:mm");
            string endDate = eDate.ToString("yyyy-MM-dd HH:mm");

            DevExpress.XtraReports.UI.XtraReport myXtraReport = null;

            DevExpress.XtraReports.UI.XtraReport NewsPaper = null;

            //是否為主從報表
            foreach (string s in Enum.GetNames(typeof(ReportForm.CommClass.Father.HasMain)))
            {
                if (aReportName.ToString() == s)
                {
                    myXtraReport = new ReportForm.clsReport(sReportName.ToString(), new VDReportHandler(), new VDReportMaim(), new VDReportMinor(), 4, arrList, dt, dtDev, sUser, startDate, endDate);
                    myXtraReport.Print(@"\\xml\HPLaserJ");
                    //myXtraReport.ShowPreview();
                    break;
                }
            }
            //不是主從報表,但有設備重類
            foreach (string s in Enum.GetNames(typeof(ReportForm.CommClass.Father.HasKind)))
            {
                if (aReportName.ToString() == s)
                {
                    List<int> perList = new List<int>();
                    perList.AddRange(new int[] { 12, 12, 12, 40, 12, 12 });
                    myXtraReport = new ReportForm.clsReport(sReportName.ToString(), new HadKindHandler(perList), null, new HadKindMinor(perList), 3, arrList, dt, dtDev, sUser, startDate, endDate);
                    //myXtraReport.ShowPreview();
                    myXtraReport.Print(@"\\xml\HPLaserJ");
                    break;
                }
            }
            //不是主從報表,也沒有有設備重類
            foreach (string s in Enum.GetNames(typeof(ReportForm.CommClass.Father.NonKind)))
            {
                if (aReportName.ToString() == s)
                {
                    ReportForm.clsReport report = new ReportForm.clsReport(sReportName.ToString(), new HadKindHandler(), null, new HadKindMinor(), 3, arrList, dt, null, sUser, startDate, endDate);
                    report.IsShowTime = false;
                    myXtraReport = report;
                    //myXtraReport.ShowPreview();
                    myXtraReport.Print(@"\\xml\HPLaserJ");
                    //NewsPaper.Print();
                    break;
                }
            }


            //SHIN ADD  2010-11-05 
            if (aReportName.ToString() == "每日定時路況新聞稿")
            {
              

                NewsPaper = new ReportForm.clsNewsPaper(sReportName.ToString(), section, dtDev, DateTime.Now);
                NewsPaper.PrintingSystem.ShowPrintStatusDialog = false;      //取消列印的訊息視窗
                NewsPaper.PrintingSystem.ShowMarginsWarning = false;

                ExportDataTableToExcel(aReportName.ToString(), dt);
                //NewsPaper.ShowPreview();
                //NewsPaper.Print();
                NewsPaper.Print(@"\\xml\HPLaserJ");

            }
            else if (aReportName.ToString() == "路段壅塞狀況一分鐘報表")
            {

                ExportDataTableToExcel(aReportName.ToString(), dt);
            }



         
          


            //75, 20, 20, 142

            //timer.Change(3000, -1);
            //myXtraReport.Landscape = true;
            //myXtraReport.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            //myXtraReport.PrintingSystem.ShowPrintStatusDialog = false;      //取消列印的訊息視窗
            //myXtraReport.PrintingSystem.ShowMarginsWarning = false;
            //myXtraReport.ShowPreviewDialog();
            //string query = string.Format("SELECT * from Win32_Printer");
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            //ManagementObjectCollection coll = searcher.Get();

            //foreach (ManagementObject printer in coll)
            //{
            //    foreach (PropertyData property in printer.Properties)
            //    {
            //        Console.WriteLine(string.Format("{0}: {1}", property.Name, property.Value));
            //    }
            //}
            //myXtraReport.ShowPreview();
            //myXtraReport.Print(@"\\testdb2\MainPrinter");
            //myXtraReport.Print(@"\\xml\HPLaserJ");
          
          

        }

        #endregion ==== Public 方法 =====

        public void ExportDataTableToExcel(string ReportName,DataTable dt)
        {
            //需加入參考
            //References右鍵AddReferences => COM => Microsoft Excel 10.0 Object Library
            //在References會多Excel及Microsoft.Office.Core

            //DataTable thisTable = this.dsQuery.Tables["test_table"];
           

            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
           

            try
            {
                oXL = new Microsoft.Office.Interop.Excel.Application();
                //加入新的活頁簿
                oWB = (Microsoft.Office.Interop.Excel._Workbook)oXL.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                //引用工作表
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                ////Sheet名稱
                //oSheet.Name = "Excel測試文件";

                ////加入內容
                //oSheet.Cells[1, 1] = "Excel測試文件";

                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    oSheet.Cells[1, i+1] = dt.Columns[i].ColumnName;

                }
              


                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    for (int j = 1; j <= dt.Columns.Count; j++)
                    {
                       
                        oSheet.Cells[i + 1, j] = dt.Rows[i - 1][j - 1];
                       
                    }
                }

                ////合併儲存格範圍
                //oRng = oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[1, dt.Columns.Count]);
                //oRng.MergeCells = true;//合併儲存格
                //oRng.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;//文字對齊方式
                //oRng.NumberFormat = "yyyy-MM-dd";//設定欄位格式
                //oRng.ColumnWidth = 20;//設定欄位寬度

                //存檔
                if (ReportName == "每日定時路況新聞稿")
                {
                     SavePath = @"D:\Newspaper";
                     FileName = "\\" + DateTime.Now.ToString("yyyyMMddHHmm") + "Newspaper";

                }
                else if (ReportName == "路段壅塞狀況一分鐘報表")
                {
                    SavePath = @"D:\TRAFFICdegree";
                     FileName = "\\" + DateTime.Now.ToString("yyyyMMddHHmm") + "degree";
                }



               if (Directory.Exists(SavePath) == false)
                {
                    // Create the directory
                    Directory.CreateDirectory(SavePath);
                }
                
               
              
              
                //若為EXCEL2000, 將最後一個參數拿掉即可
                oWB.SaveAs(SavePath + FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
                    null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared,
                    false, false, null, null, null);

                //關閉文件
                oWB.Close(null, null, null);
                oXL.Workbooks.Close();
                oXL.Quit();

                //釋放資源
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);
                oSheet = null;
                oWB = null;
                oXL = null;

                //System.Diagnostics.Process process = new System.Diagnostics.Process();
                //process.StartInfo.FileName = @"D:\" + FileName + ".xls";
                //process.Start();
                //process.Close();

                ////刪除檔案
                //if (File.Exists(@"D:\" + FileName + ".xls"))
                //    File.Delete(@"D:\" + FileName + ".xls");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }










        #region ==== Private 方法 =====
        //private void Detector(object state)
        //{
        //    //System.Diagnostics.Process Processes = System.Diagnostics.Process.GetCurrentProcess();
        //    try
        //    {
        //        System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        //        //foreach (System.ComponentModel.Component con in Processes.Container.Components)
        //        //{
        //        //    System.Windows.Forms.MessageBox.Show(con.ToString());
        //        //}
        //    }
        //    catch (System.NullReferenceException ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message);
        //    }        
        //}

        public void GetData(ReportForm.RptHander aReportName, string user, List<string> DevorSys, DateTime starTime, DateTime endTime, ref System.Data.DataTable dtDev, ref System.Data.DataTable dt, ref System.Collections.ArrayList arrListRpt)
        {
            clsDBRpt rpt = new clsDBRpt();
            int MyCol = -1;

            //報表標題陣列初始化
            arrListRpt.Clear();
            string sevorSys = "(";
            foreach (string s in DevorSys)
                sevorSys += "'" + s + "',";
            sevorSys = sevorSys.TrimEnd(',');
            sevorSys += ")";

            switch (aReportName)
            {
                case ReportForm.RptHander.操作記錄報表:
                    {
                        //把資料撈出來放進table
                        dt = rpt.GetctrlRPT_OPR1_01(sevorSys, user, starTime, endTime);

                        dtDev = rpt.GetDeviceList();

                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作時間, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作種類, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作設備, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作/執行狀態內容, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作人員, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,執行結果, ");
                    }
                    break;

                case ReportForm.RptHander.每日定時路況新聞稿:
                    {

                        string N1;
                        string N3;
                        string N4;
                        string N6;
                        //把資料撈出來放進table
                        dt = rpt.GetNewspaper(DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm"));
                        //dt = rpt.GetNewspaper("2010-11-03 12:00:00");


                        if (dt.Rows.Count > 0)
                        {

                            N1 = "2.路段狀況\n國道一號 新竹系統到大林路段 順暢/\n ";
                            N3 = "國道三號 香山到古坑 順暢/\n";
                            N4 = "國道四號 清水端到豐原端 順暢/\n";
                            N6 = "國道六號 霧峰系統到埔里端 順暢/\n";
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["LINEID"].ToString() == "N1")
                                {
                                    N1 += "\t" + dt.Rows[i]["from_location"] + "路段" + dt.Rows[i]["congested"] + "平均時速" + dt.Rows[i]["average_speed"].ToString().Trim() + "KM/H" + "\n";

                                }
                                else if (dt.Rows[i]["LINEID"].ToString() == "N3")
                                {

                                    N3 += "\t" + dt.Rows[i]["from_location"] + "路段" + dt.Rows[i]["congested"] + "平均時速" + dt.Rows[i]["average_speed"].ToString().Trim() + "KM/H" + "\n";

                                }
                                else if (dt.Rows[i]["LINEID"].ToString() == "N4")
                                {

                                    N4 += "\t" + dt.Rows[i]["from_location"] + "路段" + dt.Rows[i]["congested"] + "平均時速" + dt.Rows[i]["average_speed"].ToString().Trim() + "KM/H" + "\n";

                                }
                                else if (dt.Rows[i]["LINEID"].ToString() == "N6")
                                {

                                    N6 += "\t" + dt.Rows[i]["from_location"] + "路段" + dt.Rows[i]["congested"] + "平均時速" + dt.Rows[i]["average_speed"].ToString().Trim() + "KM/H" + "\n";

                                }
                            }

                            section = N1 + N3 + N4 + N6;
                        }



                        //dtDev = rpt.GetDeviceList();

                    }
                    break;


                case ReportForm.RptHander.路段壅塞狀況一分鐘報表:
                    {
                        starTime = DateTime.Now.AddMinutes(-1);
                        endTime = DateTime.Now;


                        //把資料撈出來放進table
                        dt = rpt.GetTRAFFICDATALOG(starTime, endTime);

                        //dtDev = rpt.GetDeviceList();

                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,路線, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,方向, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,時間, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,路段起點, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,路段起點里程, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,路段迄點, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,路段迄點里程, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總速度, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總流量, ");
                        MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,壅塞程度, ");
                    }
                    break;





            }





            //if (CtrlName == ctrlRPT_OPR1_01.Tag.ToString())//操作記錄報表
            //{

            //    //把資料撈出來放進table
            //    if (ctrlRPT_OPR1_01.GetPeopleList == "")
            //    {
            //        dt = rpt.GetctrlRPT_OPR1_01(ctrlRPT_OPR1_01.sDevSystemList, "", ctrlRPT_OPR1_01.TimeS, ctrlRPT_OPR1_01.TimeE);
            //    }
            //    else
            //    {
            //        dt = rpt.GetctrlRPT_OPR1_01(ctrlRPT_OPR1_01.sDevSystemList, ctrlRPT_OPR1_01.sDevPeopleList, ctrlRPT_OPR1_01.TimeS, ctrlRPT_OPR1_01.TimeE);

            //    }
            //    dtDev = rpt.GetDeviceList();

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作時間, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作種類, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作設備, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作/執行狀態內容, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作人員, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,執行結果, ");
            //}
            //else if (CtrlName == ctrlRPT_HDA_11.Tag.ToString())//匝道平均每日交通量統計報表
            //{
            //    //撈出符合畫面上的資料
            //    dt = rpt.GetCtrlRPT_HDA_11(ctrlRPT_HDA_11.sDevSystemList, ctrlRPT_HDA_11.TimeS, ctrlRPT_HDA_11.TimeE);

            //    dtvd = rpt.GetCtrlRPT_HDA_11vd(ctrlRPT_HDA_11.sDevSystemList, ctrlRPT_HDA_11.TimeS, ctrlRPT_HDA_11.TimeE);
            //    dtDev = rpt.Get_RPT_VD(ctrlRPT_HDA_11.Tag.ToString(), ADDVdstring(dtvd));


            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,日期, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,聯結車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,聯結車比率%, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,大型車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,大型車比率%, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小型車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小型車比率%, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,合計總流量, , ");

            //}
            //else if (CtrlName == ctrlRPT_HDA_10.Tag.ToString())//主線平均每日交通量統計報表
            //{
            //    dt = rpt.Get_RPT_LineDayVolume(ctrlRPT_HDA_10.sDevSystemList, ctrlRPT_HDA_10.TimeS, ctrlRPT_HDA_10.TimeE);
            //    //this.dgvReport.DataSource = dt;

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備編號, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,路線, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,方向, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,里程, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,日期, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,聯結車, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,比率%, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,大型車, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,比率%, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小型車, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,比率%, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,合計總流量, ");

            //}
            //else if (CtrlName == ctrlRPT_HDA_12.Tag.ToString())//全區匝道全日交通量統計報表
            //{
            //    dt = rpt.Get_RPT_RAMPFULLDAY(ctrlRPT_HDA_12.sDevSystemList, ctrlRPT_HDA_12.TimeS, ctrlRPT_HDA_12.TimeE);

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,高速公路編號, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,交流道名稱, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,方向, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,流量, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備編號, ");

            //}
            //else if (CtrlName == ctrlRPT_HDA_14.Tag.ToString())//全區主線小時路段平均速度統計報表
            //{
            //    dt = rpt.Get_RPT_SectionCarSpeed(ctrlRPT_HDA_14.sDevSystemList, ctrlRPT_HDA_14.TimeS, ctrlRPT_HDA_14.TimeE);

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,高速公路編號, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,主線路段, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,方向, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,平均速度, ");

            //}
            //else if (CtrlName == ctrlRPT_HDA_13.Tag.ToString())//全區主線全日交通量統計報表
            //{
            //    dt = rpt.Get_RPT_LINEFULLDAY(ctrlRPT_HDA_13.sDevSystemList, ctrlRPT_HDA_13.TimeS, ctrlRPT_HDA_13.TimeE);

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,高速公路編號, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,主線路段, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,方向, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,流量, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,偵測器編號, ");
            //}
            //else if (CtrlName == ctrlRPT_HDA_05.Tag.ToString())//小時交通平均速度統計報表
            //{
            //    dt = rpt.Get_RPT_HourSpeed(ctrlRPT_HDA_05.sDevSystemList, ctrlRPT_HDA_05.TimeS, ctrlRPT_HDA_05.TimeE);

            //    dtvd = rpt.Get_RPT_HourSpeedVd(ctrlRPT_HDA_05.sDevSystemList, ctrlRPT_HDA_05.TimeS, ctrlRPT_HDA_05.TimeE);

            //    dtDev = rpt.Get_RPT_VD(ctrlRPT_HDA_05.Tag.ToString(), ADDVdstring(dtvd));

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,時間, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,設備, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,總平均, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,車道一, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,車道二, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,車道三, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,車道四, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,車道五, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,車道六, , ");

            //}
            //else if (CtrlName == ctrlRPT_DATA_03.Tag.ToString())//一分鐘交通資料記錄報表
            //{
            //    dt = rpt.Get_RPT_VD1MIN(ctrlRPT_DATA_03.sDevSystemList, ctrlRPT_DATA_03.TimeS, ctrlRPT_DATA_03.TimeE);

            //    dtvd = rpt.GetReport(ctrlRPT_DATA_03.sDevSystemList, ctrlRPT_DATA_03.TimeS, ctrlRPT_DATA_03.TimeE, "", "一分鐘交通資料VD");
            //    dtDev = rpt.Get_RPT_VD(ctrlRPT_DATA_03.Tag.ToString(), ADDVdstring(dtvd));


            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,時間, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , , , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總流量,平均速度,平均占量");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  一,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  車 ,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  道 ,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  二 ,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  車 ,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  道 ,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  三 ,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  車 ,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  道 ,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  四 ,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  車 ,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  道 ,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  五 ,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  車 ,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  道 ,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  六 ,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");

            //}
            //else if (CtrlName == ctrlRPT_DATA_01.Tag.ToString())//五分鐘交通資料記錄報表
            //{
            //    dt = rpt.Get_RPT_VD5MIN(ctrlRPT_DATA_01.sDevSystemList, ctrlRPT_DATA_01.TimeS, ctrlRPT_DATA_01.TimeE);

            //    dt.Columns.Remove("priority");

            //    dtvd = rpt.GetReport(ctrlRPT_DATA_01.sDevSystemList, ctrlRPT_DATA_01.TimeS, ctrlRPT_DATA_01.TimeE, "", "五分鐘交通資料VD");
            //    dtDev = rpt.Get_RPT_VD(ctrlRPT_DATA_01.Tag.ToString(), ADDVdstring(dtvd));

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , , , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,時間, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,壅塞程度, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總流量,平均速度,平均占量");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "一,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "二,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "三,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "四,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "五,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "六,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //}
            //else if (CtrlName == ctrlRPT_DATA_06.Tag.ToString())//現點速率調查交通資料記錄報表
            //{
            //    dt = rpt.Get_RPT_VDSPOTSPEED(ctrlRPT_DATA_06.sDevSystemList, ctrlRPT_DATA_06.TimeS, ctrlRPT_DATA_06.TimeE);


            //    //撈出符合Gridview上的資料(這裡撈只是要確定這裡撈出的VD設備名稱有資料)
            //    dtvd = rpt.GetReport(ctrlRPT_DATA_06.sDevSystemList, ctrlRPT_DATA_06.TimeS, ctrlRPT_DATA_06.TimeE, "", "現點速率VD");
            //    dtDev = rpt.Get_RPT_VD(ctrlRPT_DATA_06.Tag.ToString(), ADDVdstring(dtvd));


            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,時間, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , , , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總流量,平均速度,平均占量");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "一,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "二,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "三,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "四,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "五,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "六,小車, , ");
            //}
            //else if (CtrlName == ctrlRPT_STA_01.Tag.ToString())//現場終端設備狀態記錄報表
            //{
            //    dt = rpt.Get_RPT_DeviceStatus(ctrlRPT_STA_01.sDevSystemList, ctrlRPT_STA_01.TimeS, ctrlRPT_STA_01.TimeE);

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備編號, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,路線, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,位置, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,方向, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,里程, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,故障模組/原因, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,發生時間, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,恢復時間, ");

            //}
            //else if (CtrlName == ctrlRPT_OPR2_07.Tag.ToString())//定時比對記錄報表
            //{
            //    //撈出符合畫面上的資料
            //    dt = rpt.Get_RPT_tblDeviceStatusLog(ctrlRPT_OPR2_07.sDevSystemList, ctrlRPT_OPR2_07.TimeS, ctrlRPT_OPR2_07.TimeE);

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備編號, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,路線, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,位置, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,方向, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,里程, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,時間, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,中心顯示內容, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,現場顯示內容, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,比對結果, ");
            //}
            //else if (CtrlName == ctrlRPT_MON_01.Tag.ToString())//資訊可變標誌即時資料報表
            //{
            //    //撈出符合畫面上的資料
            //    dt = rpt.Get_RPT_tblDeviceStatus(ctrlRPT_MON_01.sDevSystemList);


            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備編號, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備位置, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,連線狀態, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作模式, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作狀態, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,顯示資訊, ");

            //}
            //else if (CtrlName == ctrlRPT_DATA_02.Tag.ToString())//五分鐘車道使用率及車間距報表
            //{
            //    dt = rpt.Get_RPT_VD5MIN_INTERVAL(ctrlRPT_DATA_02.sDevSystemList, ctrlRPT_DATA_02.TimeS, ctrlRPT_DATA_02.TimeE);

            //    dtvd = rpt.GetReport(ctrlRPT_DATA_02.sDevSystemList, ctrlRPT_DATA_02.TimeS, ctrlRPT_DATA_02.TimeE, "", "五分鐘車間距資料VD");

            //    dtDev = rpt.Get_RPT_VD(ctrlRPT_DATA_02.Tag.ToString(), ADDVdstring(dtvd));


            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , ,時間, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , ,設備, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,一,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,二,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,三,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,四,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,五,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,六,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總平均,車 長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總平均,車間距, ");

            //}
            //else if (CtrlName == ctrlRPT_HDA_01.Tag.ToString())//小時交通資料紀錄報表
            //{
            //    dt = rpt.Get_RPT_VD1HR(ctrlRPT_HDA_01.sDevSystemList, ctrlRPT_HDA_01.TimeS, ctrlRPT_HDA_01.TimeE);

            //    dtvd = rpt.GetReport(ctrlRPT_HDA_01.sDevSystemList, ctrlRPT_HDA_01.TimeS, ctrlRPT_HDA_01.TimeE, "", "一小時交通資料VD");
            //    dtDev = rpt.Get_RPT_VD(ctrlRPT_HDA_01.Tag.ToString(), ADDVdstring(dtvd));

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , , , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,時間, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , , , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總流量,平均速度,平均占量");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "一,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "二,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "三,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "四,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "五,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "六,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");

            //}
            //else if (CtrlName == ctrlRPT_HDA_03.Tag.ToString())//小時交通流量統計報表（依日期時段彙整）
            //{
            //    dt = rpt.Get_RPT_HourVolume(ctrlRPT_HDA_03.sDevSystemList, ctrlRPT_HDA_03.TimeS, ctrlRPT_HDA_03.TimeE);

            //    dtvd = rpt.Get_RPT_HourVolumeVD(ctrlRPT_HDA_03.sDevSystemList, ctrlRPT_HDA_03.TimeS, ctrlRPT_HDA_03.TimeE);
            //    dtDev = rpt.Get_RPT_VD("小時交通流量統計", ADDVdstring(dtvd));


            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "時間, , , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "設備, , , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "總流量, , , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "小,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "計,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "一,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "二,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "三,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "四,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "五,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "六,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "  ,小車, , ");
            //}
            //else if (CtrlName == ctrlRPT_DATA_00.Tag.ToString())//一天交通資料
            //{
            //    dt = rpt.Get_RPT_VD1DAY(ctrlRPT_DATA_00.sDevSystemList, ctrlRPT_DATA_00.TimeS, ctrlRPT_DATA_00.TimeE);

            //    dtvd = rpt.GetReport(ctrlRPT_DATA_00.sDevSystemList, ctrlRPT_DATA_00.TimeS, ctrlRPT_DATA_00.TimeE, "", "一天交通資料VD");
            //    dtDev = rpt.Get_RPT_VD(ctrlRPT_DATA_00.Tag.ToString(), ADDVdstring(dtvd));

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , , , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,時間, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , , , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總流量,平均速度,平均占量");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "一,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "二,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "三,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "四,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "五,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "車,小計,平均,平均");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "道,聯結, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "六,大車, , ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,小車, , ");
            //}
            //else if (CtrlName == ctrlRPT_DATA_04.Tag.ToString())//一分鐘車道使用率及車間距報表
            //{

            //    dt = rpt.Get_RPT_VD1MIN_INTERVAL(ctrlRPT_DATA_04.sDevSystemList, ctrlRPT_DATA_04.TimeS, ctrlRPT_DATA_04.TimeE);

            //    dtvd = rpt.GetReport(ctrlRPT_DATA_04.sDevSystemList, ctrlRPT_DATA_04.TimeS, ctrlRPT_DATA_04.TimeE, "", "一分鐘車道使用率及車間距報表VD");

            //    dtDev = rpt.Get_RPT_VD(ctrlRPT_DATA_04.Tag.ToString(), ADDVdstring(dtvd));


            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , ,時間, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " , ,設備, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,一,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,二,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,三,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,四,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,五,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,車,使用率, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,道,車長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,六,車間距, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總平均,車 長, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,總平均,車間距, ");
            //}
            //else if (CtrlName == ctrlRPT_MON_07.Tag.ToString())//設備狀態即時監視報表
            //{
            //    dt = rpt.Get_RPT_DeviceMonitor(ctrlRPT_MON_07.sDevSystemList, DateTime.Now, DateTime.Now);


            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備種類, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備編號, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,路線名稱, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,連線狀態, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,操作模式, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,硬體狀態, ");
            //}
            //else if (CtrlName == ctrlRPT_OPR2_06.Tag.ToString())//現場終端設備運作記錄報表
            //{
            //    dt = rpt.Get_RPT_DeviceOpStatus(ctrlRPT_OPR2_06.sDevSystemList, ctrlRPT_OPR2_06.TimeS, ctrlRPT_OPR2_06.TimeE);


            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,設備編號, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,路線, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,位置, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,方向, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,里程, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,時間, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,通訊狀態, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,運作狀態, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,顯示內容, ");

            //}
            //else if (CtrlName == ctrlRPT_OPR2_14.Tag.ToString())//路段旅行時間記錄報表
            //{
            //    dt = rpt.Get_RPT_TrafficDataLogSection(ctrlRPT_OPR2_14.Lineid, ctrlRPT_OPR2_14.direction, ctrlRPT_OPR2_14.start_D, ctrlRPT_OPR2_14.end_D, ctrlRPT_OPR2_14.TimeS, ctrlRPT_OPR2_14.TimeE);

            //    dtDev = rpt.Get_RPT_lineName(ctrlRPT_OPR2_14.Tag.ToString(), ctrlRPT_OPR2_14.Lineid, ctrlRPT_OPR2_14.direction);

            //    dtDev.Rows[0][1] = "里程:" + ctrlRPT_OPR2_14.start_DC.ToString() + "交流道至" + ctrlRPT_OPR2_14.end_DC.ToString() + "交流道(分鐘)";

            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, " ,時間, ");
            //    MyCol = MyCol + 1; arrListRpt.Insert(MyCol, "," + ctrlRPT_OPR2_14.start_DC.ToString() + "交流道至" + ctrlRPT_OPR2_14.end_DC.ToString() + "交流道(分鐘),");


            //}
            //if (dt != null)
            //{
            //    // 呼叫新執行緒
            //    Thread t = new Thread(CommClass.clsMethod.OpenMsg);
            //    t.Start();

            //    // 開始跑報表
            //    LoadReportViwer(dt);

            //    // 結束新執行緒
            //    t.Abort();
            //}
            //else
            //{
            //    MessageBox.Show(this, "您尚未篩選資料！", "錯誤訊息");
            //}
        }


        #endregion ==== Private 方法 =====
    }

    public static class ReportServer
    {
        public static void PrintRoport(int aReport)
        {
            ReportBuilder report = new ReportBuilder();
            report.PrintRoport(aReport);
        }
    }
}
