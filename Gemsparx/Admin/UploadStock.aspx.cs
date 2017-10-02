using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Drawing;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Reflection;
using System.CodeDom.Compiler;

using System.Web.Services.Description;
using System.CodeDom;
using OfficeOpenXml;

public partial class UploadStock : System.Web.UI.Page
{
    string conString = ConfigurationManager.ConnectionStrings["GemSparxConnStr"].ConnectionString;
    string conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties = 'Excel 8.0;HDR=True;IMEX=1';";
    string strColumnMapQuery = "select DBColumn,MappingColumn,IsDefault,IsAdditionalColoredField from GemColumnMapping";
    List<string> lstUnMapColumnData, lstMapColumnData, lstdbColumns, lstInitialMap;
    public DataTable dtDBColumns, dtVendorColumns, dtXml, dtXmlColumns, dtAPIStock, dtDictColumns;
    public HelperClass objHelper;
    public ClientScriptManager csm;
    protected void Page_Load(object sender, EventArgs e)
    {
        objHelper = new HelperClass();
        csm = Page.ClientScript;
        if (!IsPostBack)
        {
            if (Session["userid"] == null)
                Response.Redirect("AdminLogin.aspx");
            txtAdminLogin.Visible = false;
            txtLogout.Visible = true;
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            DataTable dtEntities = objHelper.GetEntityTypes();
            vendorDropDown.DataSource = dtEntities;
            vendorDropDown.DataValueField = "EntityID";
            vendorDropDown.DataTextField = "EntityName";
            vendorDropDown.DataBind();
        }
    }

    private Dictionary<string, string> CheckValidFile()
    {
        Dictionary<string, string> dictMessage = new Dictionary<string, string>();
        string filename = FileUpload1.FileName;
        string fileExtension = filename.Split('.').Last().ToUpper();
        if (filename == "")
        {
            dictMessage.Add("MessageType", "Error");
            dictMessage.Add("Message", "Please select a file to upload!");
        }
        else if (fileExtension.ToUpper() != "XLSX" && fileExtension.ToUpper() != "XLS")
        {
            dictMessage.Add("MessageType", "Error");
            dictMessage.Add("Message", "Please select an EXCEL file only!");
        }
        return dictMessage;
    }
    private string CheckGemColorType(string color, DataTable dtStock)
    {
        string stoneType = string.Empty;
        if (color != "")
        {
            var dictColumnValues = new Dictionary<string, string>();
            var dictGemColorMapping = objHelper.GemTypeColourMapping();
            var dictGemtypes = objHelper.GemTypes();
            DataTable excelData = new DataTable(); //= objHelper.GetExelData(conStr, "select [" + color + "] from ");
            excelData.Columns.Add(color);

            var colorColResults = (from DataRow myRow in dtStock.Rows
                                   select new { Color = myRow[color] }).ToList();
            DataRow dr;
            for (int i = 0; i < colorColResults.ToList().Count; i++)
            {
                dr = excelData.NewRow();
                dr[color] = colorColResults[i].Color.ToString();
                excelData.Rows.Add(dr);

            }
            int colorLessCount = 0, colorFullCount = 0, emptyColorCellCount = 0;
            for (int i = 0; i <= excelData.Rows.Count - 1; i++)
            {
                if (excelData.Rows[i][color].ToString() != "")
                {
                    var colValue = excelData.Rows[i][color];
                    var GemTypeId = dictGemColorMapping.Where(x => x.Key.ToUpper() == (colValue.ToString().Trim().ToUpper())).Select(x => x.Value).FirstOrDefault();
                    if (GemTypeId > 0)
                    {
                        var colorType = dictGemtypes[GemTypeId];
                        if (colorType == 1)
                        {
                            colorLessCount += 1;
                        }
                        else
                            colorFullCount += 1;
                    }
                }
                else
                {
                    emptyColorCellCount += 1;
                }
            }
            if (colorLessCount == excelData.Rows.Count - emptyColorCellCount)
            {
                stoneType = "White";
            }
            else if (colorFullCount == excelData.Rows.Count - emptyColorCellCount)
            {
                stoneType = "Color";
            }
            else
            {
                stoneType = "Both";
            }
        }
        else
        {
            stoneType = "";
        }
        return stoneType;
    }
    private Dictionary<string, string> GetColumnMapping(DataTable dtExcelData)
    {
        Dictionary<string, string> dictMappingColumns = new Dictionary<string, string>();
        List<string> lstExcel = new List<string>();
        foreach (var c in dtExcelData.Columns)
        {
            string excolumn = c.ToString().TrimEnd(' ');
            lstExcel.Add(excolumn);
        }
        // Get GemColumnMapping table data
        var GSMData = objHelper.GetData(strColumnMapQuery);
        Dictionary<string, string> dict = new Dictionary<string, string>();
        var lstSort = new List<string>();
        foreach (DataRow row in GSMData.Rows)
        {
            lstSort.Add(row.Field<string>("DBColumn"));
            dict.Add(row.Field<string>("DBColumn"), row.Field<string>("MappingColumn").ToString().ToUpper());
        }
        lstSort.Sort();
        foreach (var c in lstSort)
        {
            dictMappingColumns.Add(c, "");
        }
        lstdbColumns = new List<string>();
        lstMapColumnData = new List<string>();
        lstUnMapColumnData = new List<string>();
        foreach (var c in dictMappingColumns)
        {
            lstdbColumns.Add(c.Key);
        }
        foreach (string data in lstExcel)
        {
            var dictItem = dict.Where(x => x.Value.ToUpper().Split(',').ToList().IndexOf(data.ToUpper()) > -1).ToList();
            if (dictItem != null && dictItem.Count > 0)
            {
                var checkdic = dictMappingColumns.Keys.Any(key => key.Contains(dictItem[0].Key));
                if (checkdic == true)
                {
                    dictMappingColumns[dictItem[0].Key] = data;
                }
            }
            else
            {
                lstUnMapColumnData.Add(data);
            }
        }

        foreach (var c in dictMappingColumns.Values)
        {
            lstMapColumnData.Add(c);
        }
        //lstUnMapColumnData.Add("IsDefaultDiamondImageURL");
        //lstUnMapColumnData.Add("IsDefaultDiamondVideoURL");
        Session["lstmap"] = lstMapColumnData;
        Session["lstDBColumns"] = lstdbColumns;
        Session["lstunmap"] = lstUnMapColumnData;
        return dictMappingColumns;
    }
    private string CheckStoneTypeforUploadedData(DataTable dtExcelData, string colorColName)
    {
        string colorName = string.Empty;
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict = GetColumnMapping(dtExcelData);
        Session["Dummydict"] = dict;
        colorName = dict["Color"];
        if (string.IsNullOrEmpty(colorName))
        {
            colorName = colorColName;
        }
        string stoneType = CheckGemColorType(colorName, dtExcelData);
        return stoneType;
    }
    private void ShowColumnMapping(Dictionary<string, string> dictMappingData, string stoneType)
    {
        DataTable GSMData = new DataTable();
        if (stoneType == "White")
        {
            GSMData = objHelper.GetData("SELECT DBColumn FROM GemColumnMapping where IsDefault = 1 and IsAdditionalColoredField = 0 ");
        }
        else if (stoneType == "Both" || stoneType == "Color" || stoneType == "")
        {
            GSMData = objHelper.GetData("SELECT DBColumn FROM GemColumnMapping where IsDefault = 1 ");
        }
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict = dictMappingData;
        ViewState["InitialDummydict"] = dict;
        dtDictColumns = new DataTable();
        dtDictColumns.Columns.AddRange(new DataColumn[2] { new DataColumn("DB Columns"), new DataColumn("Vendor Columns") });
        //  RequiredFields(GSMData);
        foreach (var item in dict)
        {
            dtDictColumns.Rows.Add(item.Key, item.Value);
        }
        ListBox1.DataSource = dtDictColumns;
        ListBox1.DataBind();
        ViewState["dirState"] = dtDictColumns;
        if (ViewState["sortdr"] == null)
            ViewState["sortdr"] = "Asc";
        lstUnMapColumnData = new List<string>();
        lstUnMapColumnData = (List<string>)Session["lstunmap"];
        lstUnMapColumnData.Sort();
        dtXmlColumns = new DataTable();
        dtXmlColumns.Columns.Add("UnMapped Columns", typeof(String));
        foreach (var item in lstUnMapColumnData)
        {
            dtXmlColumns.Rows.Add(item);
        }
        ListBox3.DataSource = dtXmlColumns;
        ListBox3.DataBind();
        RequiredFields(GSMData);
    }
    private void RequiredFields(DataTable GSMData)
    {
        DataTable dtRequired = new DataTable();
        dtRequired.Columns.Add("*");
        DataRow dr;
        foreach (GridViewRow row in ListBox1.Rows)
        {
            dr = dtRequired.NewRow();
            dr["*"] = "";
            dtRequired.Rows.Add(dr);
            for (int i = 0; i < GSMData.Rows.Count; i++)
            {
                if (row.Cells[0].Text == GSMData.Rows[i]["DBColumn"].ToString())
                    dtRequired.Rows[row.RowIndex]["*"] = "*";
            }
        }
        ViewState["GSMData"] = GSMData;
        gvRequired.DataSource = dtRequired;
        gvRequired.DataBind();
        ViewState["RequiredData"] = dtRequired;


    }
    private Boolean CheckRequiredFieldsHavingData()
    {
        var lstRequired = new List<string>();
        Boolean checkRequired = false;
        DataTable dt = (DataTable)ViewState["RequiredData"];

        var indexs = from row in dt.AsEnumerable()
                     let r = row.Field<string>("*")
                     where r == "*"
                     select dt.Rows.IndexOf(row);

        int requiredCount = 0;
        for (int i = 0; i < indexs.ToList().Count; i++)
        {
            int key = int.Parse(indexs.ToList()[i].ToString());
            if (ListBox1.Rows[key].Cells[1].Text != "&nbsp;")
            {

                requiredCount += 1;

            }
            else
            {
                lstRequired.Add(ListBox1.Rows[key].Cells[0].Text);
            }
        }
        if (requiredCount == indexs.ToList().Count)
            checkRequired = true;

        Session["lstRequired"] = lstRequired;
        return checkRequired;
    }
    private void setMessageModalPopUp(string MessageType, string Purpose)
    {
        divError.Visible = false;
        divInfo.Visible = false;
        divSuccess.Visible = false;
        divWarning.Visible = false;
        btnMessageCancel.Visible = false;
        btnPopUpCancel.Visible = false;
        btnContinue.Visible = false;
        btnExcelContinue.Visible = false;
        btnColTypeContinue.Visible = false;
        if (MessageType == "Error")
        {
            divError.Visible = true;
            btnMessageCancel.Visible = true;
            mpMessage.CancelControlID = btnMessageCancel.ID;
        }
        else if (MessageType == "Warning")
        {
            divWarning.Visible = true;
            btnPopUpCancel.Visible = true;
            if (Purpose == "TypeCheck")
                btnColTypeContinue.Visible = true;
            else
                btnContinue.Visible = true;
            mpMessage.CancelControlID = btnPopUpCancel.ID;
        }
        else if (MessageType == "Info")
        {
            divInfo.Visible = true;
            btnPopUpCancel.Visible = true;
            btnContinue.Visible = true;
            mpMessage.CancelControlID = btnPopUpCancel.ID;
        }
        else if (MessageType == "ExcelInfo")
        {
            divInfo.Visible = true;
            btnPopUpCancel.Visible = true;
            btnContinue.Visible = false;
            btnExcelContinue.Visible = true;
            mpMessage.CancelControlID = btnPopUpCancel.ID;
        }
        else if (MessageType == "Success")
        {
            divSuccess.Visible = true;
            btnMessageCancel.Visible = true;
            mpMessage.CancelControlID = btnMessageCancel.ID;
        }
    }
    private void GetMapping()
    {
        mpMessage.Hide();
        var GSMData = objHelper.GetData(strColumnMapQuery);
        string stoneType = (string)ViewState["stoneType"];
        Dictionary<string, string> dict = new Dictionary<string, string>();
        Dictionary<string, string> dictCheckMandatoryColumns = new Dictionary<string, string>();
        dict = (Dictionary<string, string>)Session["Dummydict"];

        //remove the colored required columns if stone type is white.
        if (stoneType == "White")
        {
            var Additionlcoloredfields = from DataRow myRow in GSMData.Rows
                                         where (Boolean)myRow["IsAdditionalColoredField"] == true
                                         select new { DBColumn = myRow["DBColumn"] };
            for (int i = 0; i < Additionlcoloredfields.ToList().Count; i++)
            {
                string key = Additionlcoloredfields.ToList()[i].DBColumn.ToString();
                dict.Remove(key);
            }
        }
        ShowColumnMapping(dict, stoneType);
        submapDiv.Style.Add("display", "block");
    }
    protected void btnContinue_Click(object sender, EventArgs e)
    {
        GetMapping();
    }

    protected void btnExcelContinue_Click(object sender, EventArgs e)
    {
        GetMapping();

    }
    private Dictionary<string, string> CheckForMandatoryColumns(Dictionary<string, string> mappedDict, DataTable dtMappingData, string stoneType)
    {
        Dictionary<string, string> dictMessage = new Dictionary<string, string>();
        //if (stoneType == "White")
        //{
        var WhiteResults = from DataRow myRow in dtMappingData.Rows
                           where (Boolean)myRow["IsDefault"] == true && (Boolean)myRow["IsAdditionalColoredField"] == false
                           select new { DBColumn = myRow["DBColumn"] };

        int count = 0;
        for (int i = 0; i < WhiteResults.ToList().Count; i++)
        {
            foreach (var item in mappedDict.Keys)
            {
                if (item == WhiteResults.ToList()[i].DBColumn.ToString())
                {
                    var a = mappedDict[item];
                    if (!string.IsNullOrEmpty(mappedDict[item].ToString()))
                        count += 1;
                }
            }
        }
        //}
        if (stoneType == "Color" || stoneType == "Both")
        {
            var results = from DataRow myRow in dtMappingData.Rows
                          where (Boolean)myRow["IsDefault"] == true
                          select new { DBColumn = myRow["DBColumn"] };
            int Colorcount = 0;
            for (int i = 0; i < results.ToList().Count; i++)
            {
                foreach (var item in mappedDict.Keys)
                {
                    if (item == results.ToList()[i].DBColumn.ToString())
                    {
                        var a = mappedDict[item];
                        if (!string.IsNullOrEmpty(mappedDict[item].ToString()))
                            Colorcount += 1;
                    }
                }
            }
            if (count == WhiteResults.ToList().Count && Colorcount != results.ToList().Count)
            {
                dictMessage.Add("MessageType", "Error");
                dictMessage.Add("Message", "The white stone types have the required columns. But the colored stone types does not have the required columns to upload the data. Please set the column mapping for the required fields for the selected vendor.");
            }
            if (count != WhiteResults.ToList().Count && Colorcount == results.ToList().Count)
            {
                dictMessage.Add("MessageType", "Error");
                dictMessage.Add("Message", "The colored stone types have the required columns. But the white stone types does not have the required columns to upload the data. Please set the column mapping for the required fields  for the selected vendor.");
            }
            else if (count != WhiteResults.ToList().Count && Colorcount != results.ToList().Count)
            {
                dictMessage.Add("MessageType", "Error");
                dictMessage.Add("Message", "The file does not have the required columns to upload the data!  Please set the column mapping for the required fields for the selected vendor.");
            }
        }
        else
        {
            if (count != WhiteResults.ToList().Count)
            {
                dictMessage.Add("MessageType", "Error");
                dictMessage.Add("Message", "The file does not have the required columns to upload the data!  Please set the column mapping for the required fields for the selected vendor.");
            }
        }
        return dictMessage;
    }
    private string CheckForNewColor(DataTable dtExcelData)
    {
        string colorName = string.Empty;
        string newColors = string.Empty;
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict = GetColumnMapping(dtExcelData);
        colorName = dict["Color"];
        var distinctIds = dtExcelData.AsEnumerable()
                    .Select(s => new
                    {
                        color = s.Field<string>(colorName),
                    })
                    .Distinct().ToList();
        if(distinctIds.Count>0)
        {
            distinctIds.RemoveAll(item => item.color == null);
        }
       

        string query = "SELECT ColorName FROM GemTypeToColorMapping";
        var colorData = objHelper.GetData(query);

        foreach (var item in distinctIds)
        {
            if (!string.IsNullOrEmpty(item.color.Trim()))
            {
                bool contains = colorData.AsEnumerable().Any(row => item.color.Trim().ToUpper() == row.Field<String>("ColorName").ToUpper());
                if (contains == false)
                {
                    newColors = newColors + "," + item.color.Trim();
                }
            }
        }
      if(newColors.Length>1)
            newColors = newColors.Substring(1);
        return newColors;
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckValidFile().Count == 0)
        {
            string strFilePath = string.Empty;
            string folderPath = string.Empty;
            if (!string.IsNullOrEmpty(FileUpload1.PostedFile.ToString()))
            {
                folderPath = Server.MapPath("~/Files/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                strFilePath = folderPath + Path.GetFileName(FileUpload1.FileName);
                ViewState["UploadedFile"] = strFilePath;
                FileUpload1.SaveAs(strFilePath);
            }
            conStr = String.Format(conStr, strFilePath);
            string query = "SELECT * From ";
            //  var excelData = objHelper.GetExelData(conStr, query);
            //Update for sorting(17/jul/2017)
            //  var excelData = objHelper.ConvertExelToDataTable(FileUpload1.PostedFile.InputStream, query);
            DataTable excelData = null;
           
                excelData = objHelper.ConvertExelToDataTable(FileUpload1.PostedFile.InputStream, query);

                //if (excelData == null)
                //{
                //    lblMessageType.Text = "ExcelInfo";
                //    lblMessage.Text = "This Spreadsheet have multiple worksheets,So please consolidated into signle sheet.";
                //    setMessageModalPopUp(lblMessageType.Text, "");
                //    mpMessage.Show();
                //    return;
                //}


                foreach (string file in Directory.GetFiles(folderPath))
                    File.Delete(file);


                var strColors = CheckForNewColor(excelData);
                Session["StockData"] = excelData;
                if (string.IsNullOrEmpty(strColors))
                {
                    submapDiv.Style.Add("display", "none");
                    //if (!excelData.Columns.Contains("IsDefaultDiamondImageURL"))
                    //{
                    //    excelData.Columns.Add("IsDefaultDiamondImageURL", typeof(string));
                    //}
                    //if (!excelData.Columns.Contains("IsDefaultDiamondVideoURL"))
                    //{
                    //    excelData.Columns.Add("IsDefaultDiamondVideoURL", typeof(string));
                    //}
                    //if (!excelData.Columns.Contains("IsDefaultCertificateURL"))
                    //{
                    //    excelData.Columns.Add("IsDefaultCertificateURL", typeof(string));
                    //}

                    //&& excelData.Columns.Contains("IsDefaultDiamondVideoURL") && excelData.Columns.Contains("IsDefaultCertificateURL")
                    //else
                    //{
                    //    excelData.Columns.Add("IsDefaultDiamondImageURL", typeof(string));
                    //    excelData.Columns.Add("IsDefaultDiamondVideoURL", typeof(string));
                    //    excelData.Columns.Add("IsDefaultCertificateURL", typeof(string));
                    //}
                    if (chkModifiedFile.Checked == false)
                    {
                        if (excelData.Columns.Contains("ErrorMessage"))
                        {
                            excelData.Columns.Remove("ErrorMessage");
                        }
                    }


                    if (excelData.Rows.Count > 0)
                    {
                        var dictMapping = GetColumnMapping(excelData);
                        string stoneType = CheckStoneTypeforUploadedData(excelData, "");
                        ViewState["stoneType"] = stoneType;
                        if (stoneType == "White")
                        {
                            lblMessageType.Text = "Info";
                            lblMessage.Text = "Uploaded file has all White gem type stock. Press continue to upload the stock";
                            setMessageModalPopUp(lblMessageType.Text, "");
                            mpMessage.Show();
                        }
                        else if (stoneType == "Color")
                        {
                            lblMessageType.Text = "Info";
                            lblMessage.Text = "Uploaded file has all Colored gem type stock. Press continue to upload the stock";
                            setMessageModalPopUp(lblMessageType.Text, "");
                            mpMessage.Show();
                        }
                        else if (stoneType == "Both")
                        {
                            lblMessageType.Text = "Info";
                            lblMessage.Text = "Uploaded file has both White and Colored gem types stock. Press continue to upload the stock";
                            setMessageModalPopUp(lblMessageType.Text, "");
                            mpMessage.Show();
                        }
                        else if (stoneType == "")
                        {
                            GetMapping();
                        }
                    }
                    else
                    {
                        lblMessageType.Text = "Error";
                        lblMessage.Text = "Uploaded file has no data to process! Please verify the file and upload again.";
                        setMessageModalPopUp(lblMessageType.Text, "");
                        mpMessage.Show();
                    }
                }
                else
                {
                    submapDiv.Style.Add("display", "none");
                    lblMessageType.Text = "Error";
                    lblMessage.Text = strColors + " new colors have been detected and will have to be added manually. Add this color type to the table for this file.";
                    setMessageModalPopUp(lblMessageType.Text, "");
                    mpMessage.Show();
                }
          


         
        }
        else
        {
            lblMessageType.Text = CheckValidFile()["MessageType"];
            lblMessage.Text = CheckValidFile()["Message"];
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();
             return;
        }

        int v = 0;
        using (var excel = new ExcelPackage(FileUpload1.PostedFile.InputStream))
        {

            try
            {
                v = excel.Workbook.Worksheets.Count();
            }
            catch
            {
                v = excel.Workbook.Worksheets.Count();
            }

        }

        if (v > 1)
        {
          
            lblMessageType.Text = "ExcelInfo";
            lblMessage.Text = "This Spreadsheet have multiple worksheets,So please consolidated into signle sheet.";
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();

        }
        }
        catch (Exception ex)
        {
            LogUtility.SaveLogEntry(ex.Message + " | Error Source:- " + ex.Source + " | TargetSite:- " + ex.TargetSite.ToString());
            lblMessageType.Text = "Error";
            lblMessage.Text = "The uploaded file has formatting issue. Please copy the data to a new spreadsheet and try uploading again.";//ex.Message;
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();

        }
    }
    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(ListBox1, "Select$" + e.Row.RowIndex);
        }
    }
    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in ListBox1.Rows)
        {
            if (row.RowIndex == ListBox1.SelectedIndex)
            {
                hdnSelectedDBColumn.Value = row.Cells[0].Text;
                row.Cells[1].BackColor = ColorTranslator.FromHtml("#A1DCF2");
            }
            else if (row.Cells[1].BackColor == Color.Yellow)
            {
                row.Cells[1].BackColor = Color.Yellow;
            }
            else
            {
                row.Cells[1].BackColor = ColorTranslator.FromHtml("#FFFFFF");
            }
        }
    }
    protected void OnRowDataBound2(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(ListBox3, "Select$" + e.Row.RowIndex);
        }
    }
    protected void OnSelectedIndexChanged2(object sender, EventArgs e)
    {
        foreach (GridViewRow row in ListBox3.Rows)
        {
            if (row.RowIndex == ListBox3.SelectedIndex)
            {
                hdnUnMappedColName.Value = row.Cells[0].Text;
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            }
        }
    }
    private Boolean ColumnIsFloatType(string colName, string dataType)
    {
        Boolean boolIsType = false;
        DataTable dtStock = (DataTable)Session["StockData"];//objHelper.GetExelData(conStr, "select [" + colName + "] from ");
        DataTable excelData = new DataTable();
        excelData.Columns.Add(colName);
        //HIMA : ADDED FOLLOWING CODE "HttpUtility.HtmlDecode(colName) FOR HANDLING ENCODED CHARACTERS."
        var colResults = (from DataRow myRow in dtStock.Rows
                          select new { colName = myRow[HttpUtility.HtmlDecode(colName)] }).ToList();
        DataRow dr;
        for (int i = 0; i < colResults.ToList().Count; i++)
        {
            dr = excelData.NewRow();
            dr[colName] = colResults[i].colName.ToString();
            excelData.Rows.Add(dr);

        }
        int rCount = excelData.Rows.Count;
        int successCount = 0, failedCount = 0, fieldEmpty = 0;
        for (int i = 0; i < rCount; i++)
        {
            float f = 0;
            int t = 0;
            bool success = false;
            if (excelData.Rows[i][0].ToString() != "")
            {
                success = float.TryParse(excelData.Rows[i][0].ToString(), out f);//Int32.TryParse(excelData.Rows[i][0].ToString(), out t) || 
                if (success == false)
                {
                    failedCount += 1;
                }
                else if (success == true)
                {
                    successCount += 1;
                }
            }
            else
            {
                fieldEmpty += 1;
            }
        }
        if (dataType == "float")
        {
            if (successCount == (rCount - fieldEmpty))
            {
                boolIsType = true;
            }
        }
        else if (dataType == "varchar")
        {
            if (failedCount == (rCount - fieldEmpty))
            {
                boolIsType = true;
            }
        }
        return boolIsType;
    }
    private Boolean CheckWebAPIforSelectedVendor()
    {
        Boolean isAPIAvailable = false;
        int EntityID = int.Parse(vendorDropDown.SelectedValue);//(int)Session["EntityID"];
        DataTable dt = objHelper.APIDetails(EntityID);
        if (dt.Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dt.Rows[0]["APIurl"].ToString()) || !string.IsNullOrEmpty(dt.Rows[0]["APIGetMethod"].ToString()))
            {
                hdnAPIurl.Value = dt.Rows[0]["APIurl"].ToString();
                hdnAPIPullMethod.Value = dt.Rows[0]["APIGetMethod"].ToString();
                hdnAPIUID.Value = dt.Rows[0]["APIUID"].ToString();
                isAPIAvailable = true;
            }
        }
        return isAPIAvailable;
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        var val = DropDownList1.SelectedValue;
        if (val == "0")
        {
            trfile.Visible = false;
            trsubmit.Visible = false;
            tdPull.Visible = false;
            submapDiv.Style.Add("display", "none");
        }
        else if (val == "3")
        {
            if (CheckWebAPIforSelectedVendor() == false)
            {
                lblMessageType.Text = "Error";
                lblMessage.Text = "Web API url or the associated webmethod are not provided for the selected Vendor. Please check..";
                setMessageModalPopUp(lblMessageType.Text, "");
                mpMessage.Show();
            }
            trfile.Visible = false;
            trsubmit.Visible = false;
            tdPull.Visible = true;
            submapDiv.Style.Add("display", "none");
        }
        else
        {
            trfile.Visible = true;
            trsubmit.Visible = true;
            tdPull.Visible = false;
            submapDiv.Style.Add("display", "none");
        }
    }
    protected void vendorDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        var dtVendorUploadMethods = objHelper.GetData("SELECT UploadTypes FROM GemEntityDemographics where EntityID =" + vendorDropDown.SelectedValue);

        string[] uploadMethods = dtVendorUploadMethods.Rows[0]["UploadTypes"].ToString().Split(',');
        DropDownList1.Items.Add(new ListItem("---Select---", "0"));
        for (int i = 0; i < uploadMethods.Length; i++)
        {
            if (uploadMethods[i] == "Excel")
            {
                DropDownList1.Items.Add(new ListItem("Excel", "1"));
            }
            else if (uploadMethods[i] == "Text")
            {
                DropDownList1.Items.Add(new ListItem("Text", "2"));
            }
            else
            {
                DropDownList1.Items.Add(new ListItem("API", "3"));
            }
        }
        DropDownList1.SelectedValue = "0";
        lblEname.Text = lblvendor.Text + "  ";
        lblVname.Text = vendorDropDown.SelectedItem.ToString();
    }
    protected void EntityDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void EtyDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        //var dictEntityTypes = (Dictionary<int, string>)(Session["entityTypes"]);
        //var colorKey = dictEntityTypes.Where(x => x.Value.ToUpper() == (EtyDropDown.SelectedValue.ToUpper())).Select(x => x.Key).FirstOrDefault();
        //Session["EntityID"] = colorKey;
        //if (EtyDropDown.SelectedValue == "Vendor")
        //{

        //lblvendor.Text = "Vendor:";
        //}
        //else
        //{
        //    lblvendor.Text = "Customer:";
        //}

    }
    private Dictionary<string, string> GetChangedValues()
    {
        var dictChangedValues = new Dictionary<string, string>();
        foreach (GridViewRow row in ListBox1.Rows)
        {

            if (row.Cells[1].BackColor == Color.Yellow)
            {
                dictChangedValues.Add(row.Cells[0].Text, row.Cells[1].Text);
            }

        }
        return dictChangedValues;
    }
    private Boolean CheckRequiredFieldsForUpload()
    {

        Boolean checkMandatory = false;
        var GSMData = objHelper.GetData(strColumnMapQuery);
        string stoneType = string.Empty;
        stoneType = (string)ViewState["stoneType"];

        if (string.IsNullOrEmpty(stoneType))
        {
            string strColor = string.Empty;
            for (int i = 0; i <= ListBox1.Rows.Count - 1; i++)
            {
                if (ListBox1.Rows[i].Cells[0].Text == "Color")
                {
                    strColor = ListBox1.Rows[i].Cells[1].Text;
                    break;
                }

            }

            var dtExcelData = (DataTable)Session["StockData"];
            stoneType = CheckStoneTypeforUploadedData(dtExcelData, strColor);
        }
        if (stoneType == "White")
        {
            var WhiteResults = from DataRow myRow in GSMData.Rows
                               where (Boolean)myRow["IsDefault"] == true && (Boolean)myRow["IsAdditionalColoredField"] == false
                               select new { DBColumn = myRow["DBColumn"] };
            int requiredCount = 0;
            for (int i = 0; i < WhiteResults.ToList().Count; i++)
            {
                string key = WhiteResults.ToList()[i].DBColumn.ToString();

                foreach (GridViewRow row in ListBox1.Rows)
                {
                    string v = row.Cells[0].Text;
                    if (key == row.Cells[0].Text)
                    {
                        string val = row.Cells[1].Text;
                        if (!string.IsNullOrEmpty(row.Cells[1].Text))
                            requiredCount += 1;
                    }
                }
            }
            if (requiredCount == WhiteResults.ToList().Count)
                checkMandatory = true;
        }
        else if (stoneType == "Both" || stoneType == "Color")
        {
            var colorResults = from DataRow myRow in GSMData.Rows
                               where (Boolean)myRow["IsDefault"] == true
                               select new { DBColumn = myRow["DBColumn"] };
            int requiredCount = 0;
            for (int i = 0; i < colorResults.ToList().Count; i++)
            {
                string key = colorResults.ToList()[i].DBColumn.ToString();

                foreach (GridViewRow row in ListBox1.Rows)
                {
                    string v = row.Cells[0].Text;
                    if (key == row.Cells[0].Text)
                    {
                        string val = row.Cells[1].Text;
                        if (!string.IsNullOrEmpty(row.Cells[1].Text))
                            requiredCount += 1;
                    }
                }
            }
            if (requiredCount == colorResults.ToList().Count)
                checkMandatory = true;

        }
        return checkMandatory;
    }
    private Boolean CheckMandatoryFieldData(Dictionary<string, string> dtRowData)
    {
        var lstFailFields = new List<string>();
        Boolean checkMandatory = false;
        var GSMData = objHelper.GetData(strColumnMapQuery);
        string stoneType = string.Empty;//(string)ViewState["stoneType"];


        var dictGemColorMapping = objHelper.GemTypeColourMapping();
        var GemTypeId = dictGemColorMapping.Where(x => x.Key.ToUpper() == (dtRowData["Color"].ToUpper())).Select(x => x.Value).FirstOrDefault();

        var stoneTypeName = objHelper.GetData("select StoneTypeName from GemStoneTypes where StoneTypeID in (select StoneTypeID from [GemTypes] where TypeID =" + GemTypeId.ToString() + ")");
        if (stoneTypeName.Rows.Count > 0)
        {
            stoneType = stoneTypeName.Rows[0]["StoneTypeName"].ToString();

            if (stoneType == "White")
            {
                var WhiteResults = from DataRow myRow in GSMData.Rows
                                   where (Boolean)myRow["IsDefault"] == true && (Boolean)myRow["IsAdditionalColoredField"] == false
                                   select new { DBColumn = myRow["DBColumn"] };
                int requiredCount = 0;
                for (int i = 0; i < WhiteResults.ToList().Count; i++)
                {
                    string key = WhiteResults.ToList()[i].DBColumn.ToString();
                    if (!string.IsNullOrEmpty(dtRowData[key]))
                    {
                        requiredCount += 1;
                    }
                    else
                    {
                        lstFailFields.Add(key);
                    }
                }
                if (requiredCount == WhiteResults.ToList().Count)
                    checkMandatory = true;
            }
            else if (stoneType == "Colored")
            {

                var colorResults = from DataRow myRow in GSMData.Rows
                                   where (Boolean)myRow["IsDefault"] == true
                                   select new { DBColumn = myRow["DBColumn"] };
                int requiredCount = 0;
                for (int i = 0; i < colorResults.ToList().Count; i++)
                {
                    string key = colorResults.ToList()[i].DBColumn.ToString();
                    if (!string.IsNullOrEmpty(dtRowData[key]))
                    {
                        requiredCount += 1;
                    }
                    else
                    {
                        lstFailFields.Add(key);
                    }
                }
                if (requiredCount == colorResults.ToList().Count)
                    checkMandatory = true;

            }
        }
        if (string.IsNullOrEmpty(dtRowData["Color"]))
        {
            lstFailFields.Add("Color");
        }
        if (string.IsNullOrEmpty(stoneType) && !string.IsNullOrEmpty(dtRowData["Color"]))
        {
            lstFailFields.Add(dtRowData["Color"] + " color doesn't exist in the database. StoneType");
        }
        Session["lstFail"] = lstFailFields;
        return checkMandatory;
    }
    private Dictionary<string, string> GetMappedValues()
    {
        var dtmap = new DataTable();
        var dictChanged = new Dictionary<string, string>();
        var dictMapData = new Dictionary<string, string>();
        dtmap = objHelper.GetData(strColumnMapQuery);
        foreach (DataRow row in dtmap.Rows)
        {
            dictMapData.Add(row.Field<string>("DBColumn"), row.Field<string>("MappingColumn").ToString().ToUpper());
        }
        var dictInitial = (Dictionary<string, string>)ViewState["InitialDummydict"];
        var dictFinal = GetChangedValues();
        foreach (var item in dictFinal)
        {
            var mapValue = dictInitial[item.Key];
            var mapStringValue = dictMapData[item.Key];
            var ChangedValue = dictFinal[item.Key];

            List<string> lst = mapStringValue.Split(',').Distinct().ToList();
            lst.Add(ChangedValue);

            string finalString = (string.Join(",", lst.Select(x => x.ToString()).ToArray()));
            dictChanged.Add(item.Key, finalString);
        }
        return dictChanged;
    }
    protected void btnfinalSub_Click(object sender, EventArgs e)
    {
        if (CheckRequiredFieldsHavingData() == true)
        {
            if (CheckRequiredFieldsForUpload() == true)
            {
                lstUnMapColumnData = (List<string>)Session["lstunmap"];
                lstdbColumns = (List<string>)Session["lstDBColumns"];
                string stoneType = (string)ViewState["stoneType"];
                if (stoneType == "White")
                {
                    var GSMData = objHelper.GetData(strColumnMapQuery);
                    var Additionlcoloredfields = from DataRow myRow in GSMData.Rows
                                                 where (Boolean)myRow["IsAdditionalColoredField"] == true
                                                 select new { DBColumn = myRow["DBColumn"] };
                    for (int i = 0; i < Additionlcoloredfields.ToList().Count; i++)
                    {
                        string key = Additionlcoloredfields.ToList()[i].DBColumn.ToString();
                        lstdbColumns.Remove(key);
                    }
                }
                lstMapColumnData = new List<string>();
                foreach (GridViewRow row in ListBox1.Rows)
                {
                    lstMapColumnData.Add(row.Cells[1].Text);
                }
                DataTable excelData = new DataTable();
                excelData = (DataTable)Session["StockData"];
                var ExcelColumnsList = (from DataColumn x in excelData.Columns
                                        select x.ColumnName).ToList();

                DataTable tblCatch;
                tblCatch = objHelper.GemStockTable(ExcelColumnsList);
                DataTable deletedRecords = null;
                if (chkModifiedFile.Checked == true)
                {
                    deletedRecords = objHelper.GetData("Delete from GemStock where EntityID =" + vendorDropDown.SelectedValue + "; Select * from GemStock where EntityID = " + vendorDropDown.SelectedValue);
                }
                if ((deletedRecords == null) || (deletedRecords != null && deletedRecords.Rows.Count == 0))
                {
                    for (int i = 0; i <= excelData.Rows.Count - 1; i++)
                    {

                        var dictColumnValues = new Dictionary<string, string>();
                        var dictAdditionalVendorValues = new Dictionary<string, string>();
                        for (int j = 0; j <= lstdbColumns.Count - 1; j++)
                        {
                            string strDBColumn = lstdbColumns[j];
                            string strVendorColumn = HttpUtility.HtmlDecode(lstMapColumnData[j]);
                            if (lstMapColumnData[j] != "&nbsp;")
                            {
                                dictColumnValues.Add(strDBColumn, excelData.Rows[i][strVendorColumn].ToString().Trim());
                            }
                            else
                            {
                                dictColumnValues.Add(strDBColumn, "");
                            }

                            //if (lstMapColumnData[j] == "IsDefaultDiamondImageURL")
                            //{
                            //    dictColumnValues[strDBColumn] = Server.MapPath("~/images/404_background.jpg");
                            //}
                            //if (lstMapColumnData[j] == "IsDefaultDiamondVideoURL")
                            //{
                            //    dictColumnValues[strDBColumn] = Server.MapPath("~/images/404_background.jpg");
                            //}
                            //if (lstMapColumnData[j] == "IsDefaultCertificateURL")
                            //{
                            //    dictColumnValues[strDBColumn] = "https://mail.google.com";
                            //}

                        }


                        if (CheckMandatoryFieldData(dictColumnValues) == true)
                        {

                            var dtAdditioanlDataColumns = new DataTable();
                            for (int k = 0; k < ListBox3.Rows.Count; k++)
                            {
                                string gridValue = HttpUtility.HtmlDecode(ListBox3.Rows[k].Cells[0].Text);
                                dtAdditioanlDataColumns.Columns.Add(gridValue, typeof(string));
                                string excelDataColumn = excelData.Rows[i][gridValue].ToString();
                                dictAdditionalVendorValues.Add(gridValue, excelDataColumn);
                            }

                            DataRow drAdditioanlDataColumn = dtAdditioanlDataColumns.NewRow();
                            foreach (var c in dictAdditionalVendorValues)
                            {
                                drAdditioanlDataColumn[c.Key] = c.Value;
                            }
                            dtAdditioanlDataColumns.Rows.Add(drAdditioanlDataColumn);

                            foreach (DataColumn col in dtAdditioanlDataColumns.Columns)
                            {
                                col.ColumnName = col.ColumnName.Replace(" ", "");
                            }

                            if (chkModifiedFile.Checked == false)
                            {
                                objHelper.CheckIfStockExistsByStoneIDAndEntityID(dictColumnValues["StoneID"].ToString(), int.Parse(vendorDropDown.SelectedValue));
                            }

                            dtAdditioanlDataColumns.TableName = "AdditionalDataColumns";
                            StringWriter sw = new StringWriter();
                            dtAdditioanlDataColumns.WriteXml(sw);
                            string xmlAdditionalcolumnsData = sw.ToString();
                            string val = dictColumnValues["StoneID"].ToString();
                            var con = new SqlConnection(conString);
                            con.Open();
                            SqlCommand cmd = new SqlCommand("GemStock_insert", con);
                            var dictGemColorMapping = objHelper.GemTypeColourMapping();
                            var dictEntityTypes = (Dictionary<int, string>)(Session["entityTypes"]);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("stoneID", dictColumnValues["StoneID"]);
                            var GemTypeId = dictGemColorMapping.Where(x => x.Key.ToUpper() == (dictColumnValues["Color"].ToUpper())).Select(x => x.Value).FirstOrDefault();
                            cmd.Parameters.AddWithValue("GemTypeID", GemTypeId);
                            cmd.Parameters.AddWithValue("EntityID", int.Parse(vendorDropDown.SelectedValue));
                            cmd.Parameters.AddWithValue("Lab", dictColumnValues["Lab"]);
                            cmd.Parameters.AddWithValue("CertificateNo", dictColumnValues["CertificateNo"]);
                            cmd.Parameters.AddWithValue("Shape", dictColumnValues["Shape"]);
                            cmd.Parameters.AddWithValue("Color", dictColumnValues["Color"]);
                            cmd.Parameters.AddWithValue("Clarity", dictColumnValues["Clarity"]);
                            cmd.Parameters.AddWithValue("Cut", dictColumnValues["Cut"]);
                            cmd.Parameters.AddWithValue("Polish", dictColumnValues["Polish"]);
                            cmd.Parameters.AddWithValue("Symm", dictColumnValues["Symm"]);
                            cmd.Parameters.AddWithValue("Fluo", dictColumnValues["Fluo"]);
                            if (dictColumnValues["Rap"] != "")
                            {
                                float f;
                                if (float.TryParse(dictColumnValues["Rap"], out f))
                                {
                                    var Rap = f.ToString();
                                    cmd.Parameters.AddWithValue("Rap", Rap);
                                }
                                 
                                else
                                    cmd.Parameters.AddWithValue("Rap", 0);
                            }
                            else
                                cmd.Parameters.AddWithValue("Rap", 0);

                            if (dictColumnValues["Disc"] != "")
                            {
                                float f;
                                dictColumnValues["Disc"] = dictColumnValues["Disc"].Replace("%", string.Empty);
                                if (float.TryParse(dictColumnValues["Disc"], out f))
                                {
                                    var Disc = f.ToString();
                                    cmd.Parameters.AddWithValue("Disc", Disc);
                                }

                                else
                                    cmd.Parameters.AddWithValue("Disc", 0);
                            }
                            else { cmd.Parameters.AddWithValue("Disc", 0); }
                            cmd.Parameters.AddWithValue("KeyToSymbols", dictColumnValues["KeyToSymbols"]);
                            cmd.Parameters.AddWithValue("CrAng", dictColumnValues["CrAng"]);
                            cmd.Parameters.AddWithValue("CrHgt", dictColumnValues["CrHgt"]);
                            cmd.Parameters.AddWithValue("PavAng", dictColumnValues["PavAng"]);
                            cmd.Parameters.AddWithValue("GirdleCondition", dictColumnValues["GirdleCondition"]);
                            cmd.Parameters.AddWithValue("Girdle", dictColumnValues["Girdle%"]);
                            if (dictColumnValues["Carats"] != "")
                            {
                                float f;
                                if (float.TryParse(dictColumnValues["Carats"], out f))
                                {
                                    var Carats = f.ToString();
                                    cmd.Parameters.AddWithValue("Carats", Carats);
                                }

                                else
                                    cmd.Parameters.AddWithValue("Carats", 0);
                            }
                            else { cmd.Parameters.AddWithValue("Carats", 0); }

                            if (dictColumnValues["Depth"] != "")
                            {
                                float f;
                                if (float.TryParse(dictColumnValues["Depth"], out f))
                                {
                                    var Depth = f.ToString();
                                    cmd.Parameters.AddWithValue("Depth", Depth);
                                }

                                else
                                    cmd.Parameters.AddWithValue("Depth", 0);
                            }
                            else { cmd.Parameters.AddWithValue("Depth", 0); }

                            if (dictColumnValues["Price"] != "")
                            {
                                float f;
                                if (float.TryParse(dictColumnValues["Price"], out f))
                                {
                                    var Price = f.ToString();
                                    cmd.Parameters.AddWithValue("Price", Price);
                                }

                                else
                                    cmd.Parameters.AddWithValue("Price", 0);
                            }
                            else { cmd.Parameters.AddWithValue("Price", 0); }

                            if (dictColumnValues["CostPrice"] != "")
                            {
                                float f;
                                //dictColumnValues["CostPrice"] = dictColumnValues["CostPrice"].Replace("$", string.Empty);

                                if (float.TryParse(dictColumnValues["CostPrice"], out f))
                                {
                                    var Amount = f.ToString();
                                    cmd.Parameters.AddWithValue("CostPrice", Amount);
                                }

                                else
                                    cmd.Parameters.AddWithValue("CostPrice", 0);
                            }
                            else { cmd.Parameters.AddWithValue("CostPrice", 0); }

                            if (dictColumnValues["FullRap"] != "")
                            {
                                float f;
                                if (float.TryParse(dictColumnValues["FullRap"], out f))
                                {
                                    var FullRap = f.ToString();
                                    cmd.Parameters.AddWithValue("FullRap", FullRap);
                                }

                                else
                                    cmd.Parameters.AddWithValue("FullRap", 0);
                            }
                            else { cmd.Parameters.AddWithValue("FullRap", 0); }
                            cmd.Parameters.AddWithValue("MM", dictColumnValues["MM"]);

                            if (dictColumnValues["Table"] != "")
                            {
                                float f;
                                if (float.TryParse(dictColumnValues["Table"], out f))
                                {
                                    var Table = f.ToString();
                                    cmd.Parameters.AddWithValue("Table", Table);
                                }

                                else
                                    cmd.Parameters.AddWithValue("Table", 0);
                            }
                            else { cmd.Parameters.AddWithValue("Table", 0); }
                            cmd.Parameters.AddWithValue("CertificateURL", dictColumnValues["CertificateURL"]);
                            //if (!string.IsNullOrEmpty(dictColumnValues["DiamondImageURL"]))
                            //{
                            //    cmd.Parameters.AddWithValue("DiamondImageURL", dictColumnValues["DiamondImageURL"]);
                            //}
                            //else
                            //{
                            //    cmd.Parameters.AddWithValue("DiamondImageURL", Server.MapPath("~/images/404_background.jpg"));
                            //}
                            //if (!string.IsNullOrEmpty(dictColumnValues["DiamondVideoURL"]))
                            //{
                            //    cmd.Parameters.AddWithValue("DiamondVideoURL", dictColumnValues["DiamondVideoURL"]);
                            //}
                            //else
                            //{
                            //    cmd.Parameters.AddWithValue("DiamondVideoURL", Server.MapPath("~/images/404_background.jpg"));
                            //}
                            cmd.Parameters.AddWithValue("XMLAdditionalData", xmlAdditionalcolumnsData);
                            //cmd.Parameters.AddWithValue("CreatedBy", Session["userid"]);
                            cmd.Parameters.AddWithValue("CreatedBy", "Admin");
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                DataRow dr = tblCatch.NewRow();
                                var count = tblCatch.Rows.Count;
                                var ColNames = (from DataColumn x in tblCatch.Columns
                                                select x.ColumnName).ToArray();

                                for (int j = 0; j < tblCatch.Columns.Count - 1; j++)
                                {
                                    dr[ColNames[j]] = excelData.Rows[i][ColNames[j]].ToString();
                                }
                                dr["ErrorMessage"] = ex.Message;
                                tblCatch.Rows.Add(dr);
                            }
                            con.Close();
                        }
                        else
                        {
                            DataRow dr = tblCatch.NewRow();
                            var count = tblCatch.Rows.Count;
                            var ColNames = (from DataColumn x in tblCatch.Columns
                                            select x.ColumnName).ToArray();
                            for (int j = 0; j < tblCatch.Columns.Count - 1; j++)
                            {
                                dr[ColNames[j]] = excelData.Rows[i][ColNames[j]].ToString();
                            }
                            var fields = (List<string>)Session["lstFail"];
                            var result = String.Join(",", fields);
                            dr["ErrorMessage"] = result + " fields does't have values for these required fileds.";
                            tblCatch.Rows.Add(dr);
                        }
                    }


                    if (tblCatch.Rows.Count > 0)
                    {
                        Dictionary<string, string> dctChangedMapping = new Dictionary<string, string>();
                        dctChangedMapping = GetMappedValues();
                        foreach (KeyValuePair<string, string> entry in dctChangedMapping)
                        {
                            objHelper.UpdateChangedMapping(int.Parse(vendorDropDown.SelectedValue), entry.Key, entry.Value);
                        }
                        tblCatch.TableName = "UnSuccessful Records";
                        Session["ErrorRecordsTable"] = tblCatch;
                        lblErrorMessageType.Text = "Error";
                        lblErrorMessage.Text = "Out of the " + excelData.Rows.Count.ToString() + " records, " + tblCatch.Rows.Count.ToString() + " records got failed in uploading. Please click on continue to get the failed records spreadsheet.";
                        SetModalErrorPopUpFields(lblErrorMessageType.Text);
                        mpErrorMessage.Show();
                    }
                    else
                    {
                        Dictionary<string, string> dctChangedMapping = new Dictionary<string, string>();
                        dctChangedMapping = GetMappedValues();
                        try
                        {
                            foreach (KeyValuePair<string, string> entry in dctChangedMapping)
                            {
                                objHelper.UpdateChangedMapping(int.Parse(vendorDropDown.SelectedValue), entry.Key, entry.Value);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        if (DropDownList1.SelectedValue == "3")
                        {
                            lblErrorMessageType.Text = "Success";
                            lblErrorMessage.Text = "All the API stock uploaded successfully! Click on Continue to get the API stock spreadsheet.";
                            SetModalErrorPopUpFields(lblErrorMessageType.Text);
                            mpErrorMessage.Show();
                        }
                        else
                        {
                            lblMessageType.Text = "Success";
                            lblMessage.Text = "File uploaded successfully!";
                            setMessageModalPopUp(lblMessageType.Text, "");
                            mpMessage.Show();
                          //  DeleteRelatedInformation();
                        }
                    }
                }
                else
                {
                    lblMessageType.Text = "Error";
                    lblMessage.Text = "Failed in deleting the previous stock for the vendor.";
                    setMessageModalPopUp(lblMessageType.Text, "");
                    mpMessage.Show();
                }
            }
            else
            {
                lblMessageType.Text = "Error";
                lblMessage.Text = "Required Vendor column is empty! Please verify.";
                setMessageModalPopUp(lblMessageType.Text, "");
                mpMessage.Show();
            }
        }
        else
        {
            var fields = (List<string>)Session["lstRequired"];
            var result = String.Join(", ", fields);
            lblMessageType.Text = "Error";
            lblMessage.Text = result + " field(s) does't have a mapping. Please have a mapping for the required fields.";
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();
        }
    }
    private void ExporttoExcel(DataTable table)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=GemStock.xls");
        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");
        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        for (int j = 0; j < columnscount; j++)
        {      //write in new column
            HttpContext.Current.Response.Write("<Td>");
            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ColumnName.ToString());
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }
        HttpContext.Current.Response.Write("</TR>");
        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (vendorDropDown.SelectedIndex == 0)
        {

            csm.RegisterStartupScript(this.GetType(), "winPop", "alert('Please select the " + lblvendor.Text.Remove(lblvendor.Text.Length - 1) + ".');", true);
        }
        else
        {
            vendorDiv.Style.Add("display", "none");
            mappingDiv.Style.Add("display", "block");
            submapDiv.Style.Add("display", "none");
            DropDownList1.SelectedIndex = 0;
            trfile.Visible = false;
            trsubmit.Visible = false;
            tdPull.Visible = false;
        }

    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        vendorDropDown.SelectedIndex = 0;
        vendorDiv.Style.Add("display", "block");
        TypeDiv.Style.Add("display", "none");
    }
    protected void TypeDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TypeDropDown.SelectedIndex == 0)
        {

        }
        else
        {
            TypeDropDown2.Visible = true;
        }
    }
    protected void btnRight_Click(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex == -1)
        {
            csm.RegisterStartupScript(this.GetType(), "winPop", "alert('Please select any item from vendor columns.');", true);
        }
        else
        {

            var selectedindex = ListBox1.SelectedIndex;
            var dict = (Dictionary<string, string>)Session["Dummydict"];
            var selectedValue = HttpUtility.HtmlDecode(ListBox1.SelectedRow.Cells[1].Text);
            
            if (selectedValue != null)
            {
                lstUnMapColumnData = (List<string>)Session["lstunmap"];
                lstUnMapColumnData.Add(selectedValue);
                lstUnMapColumnData.Sort();
                //Update for sorting(17/jul/2017)
                if (Session["sortlist3"] != null)
                {
                    if (Session["sortlist3"].ToString() == "Desc")
                        lstUnMapColumnData.Reverse();
                }
                var distinctUnMapData = lstUnMapColumnData.Distinct().ToList();
                dtXmlColumns = new DataTable();
                dtXmlColumns.Columns.Add("UnMapped Columns", typeof(String));
                foreach (var item in distinctUnMapData)
                {
                    dtXmlColumns.Rows.Add(item);
                }
                ListBox3.DataSource = dtXmlColumns;
                ListBox3.DataBind();

                var key = dict.FirstOrDefault(m => m.Value == selectedValue).Key;
                if (key != null)
                    dict[key] = "";

                dtDictColumns = new DataTable();
                dtDictColumns.Columns.AddRange(new DataColumn[2] { new DataColumn("DB Columns"), new DataColumn("Vendor Columns") });
                foreach (var item in dict)
                {
                    dtDictColumns.Rows.Add(HttpUtility.HtmlDecode(item.Key), HttpUtility.HtmlDecode(item.Value));
                }
                ListBox1.DataSource = dtDictColumns;
                ListBox1.DataBind();
                ListBox1.SelectedIndex = -1;
                ViewState["dirState"] = dtDictColumns;
                if (ViewState["sortdr"] == null)
                    ViewState["sortdr"] = "Asc";
                Session["Dummydict"] = dict;
                ColorChangedMethod(dict);


                //Update for sorting(17/jul/2017)		
                if (Session["field"] == null)
                    Session["field"] = "DB Columns";
                dtDictColumns.DefaultView.Sort = Session["field"].ToString() + " " + ViewState["sortdr"];
                ListBox1.DataSource = dtDictColumns.DefaultView;
                ListBox1.DataBind();
                var lstcol = new List<string>();
                foreach (var c in dict.Values)
                {
                    lstcol.Add(c);
                }
                //Session["lstText"]		
                //var lstText = new List<string>();		
                List<string> lst = (List<string>)Session["lstText"];
                for (int i = 0; i <= ListBox1.Rows.Count - 1; i++)
                {
                    foreach (var item in lst)
                    {
                        if (ListBox1.Rows[i].Cells[1].Text == item)
                        {
                            if (ListBox1.Rows[i].Cells[1].Text != "&nbsp;")
                            {
                                ListBox1.Rows[i].Cells[1].BackColor = Color.Yellow;
                            }
                        }
                    }
                }
                //For list box3 sorting		
                if (Session["sortlist3"] == null)
                    Session["sortlist3"] = "Asc";
                dtXmlColumns.DefaultView.Sort = "UnMapped Columns " + Session["sortlist3"];
                ListBox3.DataSource = dtXmlColumns.DefaultView;
                ListBox3.DataBind();
            }
        }
    }
    private void ColorChangedMethod(Dictionary<string, string> dict)
    {
        var dictInitial = (Dictionary<String, String>)ViewState["InitialDummydict"];
        var dictColor = new Dictionary<string, string>();
        foreach (var item in dictInitial)
        {
            bool isContain = false;
            foreach (var c in dict)
            {
                if (c.Key == item.Key && c.Value == item.Value)
                {
                    dictColor.Add(c.Key, c.Value);
                    isContain = true;
                }
            }
            if (isContain == false)
            {
                dictColor.Add(item.Key, "");
            }
        }
        var lstcol = new List<string>();
        foreach (var c in dictColor.Values)
        {
            lstcol.Add(c);
        }
        var result = Enumerable.Range(0, dictColor.Count).Where(i => lstcol[i] == "").ToList();
        var lstText = new List<string>();
        foreach (var c in result)
        {
            if (ListBox1.Rows[c].Cells[1].Text != "&nbsp;")
            {
                ListBox1.Rows[c].Cells[1].BackColor = Color.Yellow;
            }
            lstText.Add(ListBox1.Rows[c].Cells[1].Text);
        }
        Session["lstText"] = lstText;
    }
    protected void btnLeft_Click(object sender, EventArgs e)
    {
        if (ListBox3.SelectedIndex == -1)
        {
            lblMessageType.Text = "Error";
            lblMessage.Text = "Please select any item.";
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();
        }
        else
        {
            var selectindex = ListBox1.SelectedIndex;
            if (selectindex == -1)
            {
                lblMessageType.Text = "Error";
                lblMessage.Text = "Please select any item from vendor columns.";
                setMessageModalPopUp(lblMessageType.Text, "");
                mpMessage.Show();
            }
            else
            {
                var dict = (Dictionary<string, string>)Session["Dummydict"];

                lstUnMapColumnData = (List<string>)Session["lstunmap"];
                var selectedindex = ListBox3.SelectedIndex;
                var selectedValue = lstUnMapColumnData.ElementAt(selectedindex);
                var someItem = ListBox1.SelectedRow.Cells[1].Text;
                if (someItem != "&nbsp;")
                {
                    lblMessageType.Text = "Error";
                    lblMessage.Text = "Please select the empty vendor column.";
                    setMessageModalPopUp(lblMessageType.Text, "");
                    mpMessage.Show();
                }
                else
                {
                    lstMapColumnData = (List<string>)Session["lstunmap"];
                    var GSMData = objHelper.GetData(strColumnMapQuery);
                    Boolean IsContain =
                        (GSMData.AsEnumerable().SelectMany(r => r.Field<string>("MappingColumn").Split(',')).Select(str => (object)str.ToUpper()).Contains(selectedValue.ToUpper()) &&
                        ! (lstMapColumnData.Contains(selectedValue)));

                  
                    if (IsContain == false)
                    {
                        if (selectedValue != null)
                        {
                            objHelper = new HelperClass();
                            string strType = objHelper.GetColumnDataType(hdnSelectedDBColumn.Value);
                            Boolean isFloatType = ColumnIsFloatType(hdnUnMappedColName.Value, strType);
                            if (isFloatType == true)
                            {
                                MoveSelectedColumn(dict, HttpUtility.HtmlDecode(selectedValue));
                            }
                            else
                            {
                                if (strType == "float")
                                {
                                    lblMessageType.Text = "Error";
                                    lblMessage.Text = "The DBColumn is of type float! You cannot map a string type column to a float type column.";
                                    setMessageModalPopUp(lblMessageType.Text, "");
                                    mpMessage.Show();
                                }
                                else if (strType == "varchar")
                                {
                                    lblMessageType.Text = "Warning";
                                    lblMessage.Text = "The DBColumn is of type string! You are trying to map a column with type float. If you still wish to map the column with type float, you can click on Continue.";
                                    setMessageModalPopUp(lblMessageType.Text, "TypeCheck");
                                    mpMessage.Show();
                                }
                            }
                        }
                    }
                    else
                    {
                        lblMessageType.Text = "Error";
                        lblMessage.Text = "The Mapping column already exists. Please assign a new name for the in the spread sheet.";
                        setMessageModalPopUp(lblMessageType.Text, "");
                        mpMessage.Show();
                    }
                }
            }
        }
    }
    private void MoveSelectedColumn(Dictionary<string, string> dict, string selectedValue)
    {

        lstUnMapColumnData = (List<string>)Session["lstunmap"];
        lstUnMapColumnData.RemoveAt(ListBox3.SelectedIndex);
        Session["lstunmap"] = lstUnMapColumnData;
        lstUnMapColumnData.Sort();
        dtXmlColumns = new DataTable();
        dtXmlColumns.Columns.Add("UnMapped Columns", typeof(String));
        foreach (var item in lstUnMapColumnData)
        {
            dtXmlColumns.Rows.Add(item);
        }
        ListBox3.DataSource = dtXmlColumns;
        ListBox3.DataBind();

        string myKey = ListBox1.Rows[ListBox1.SelectedIndex].Cells[0].Text.ToString();
        // var key = dict.Keys.ElementAt(ListBox1.SelectedIndex);
        dict[myKey] = selectedValue;
        dtDictColumns = new DataTable();
        dtDictColumns.Columns.AddRange(new DataColumn[2] { new DataColumn("DB Columns"), new DataColumn("Vendor Columns") });
        foreach (var item in dict)
        {
            dtDictColumns.Rows.Add(item.Key, item.Value);
        }
        ListBox1.DataSource = dtDictColumns;
        ListBox1.DataBind();
        ViewState["dirState"] = dtDictColumns;
        if (ViewState["sortdr"] == null)
            ViewState["sortdr"] = "Asc";
        Session["Dummydict"] = dict;
        ColorChangedMethod(dict);

        //Update for sorting(17/jul/2017)

        if (ViewState["sortdr"] == null)
            ViewState["sortdr"] = "Asc";
        if (Session["field"] == null)
            Session["field"] = "DB Columns";
        dtDictColumns.DefaultView.Sort = Session["field"].ToString() + " " + ViewState["sortdr"];
        ListBox1.DataSource = dtDictColumns.DefaultView;
        ListBox1.DataBind();
        var lstcol = new List<string>();
        foreach (var c in dict.Values)
        {
            lstcol.Add(c);
        }
        //Session["lstText"]		
        //var lstText = new List<string>();		
        List<string> lst = (List<string>)Session["lstText"];
        for (int i = 0; i <= ListBox1.Rows.Count - 1; i++)
        {
            foreach (var item in lst)
            {
                if (ListBox1.Rows[i].Cells[1].Text == item)
                {
                    if (ListBox1.Rows[i].Cells[1].Text != "&nbsp;")
                    {
                        ListBox1.Rows[i].Cells[1].BackColor = Color.Yellow;
                    }
                }
            }
        }
        if (Session["sortlist3"] == null)
            Session["sortlist3"] = "Asc";
        dtXmlColumns.DefaultView.Sort = "UnMapped Columns " + Session["sortlist3"];
        ListBox3.DataSource = dtXmlColumns.DefaultView;
        ListBox3.DataBind();

    }
    protected void btnPullStock_Click(object sender, EventArgs e)
    {

        if (CheckWebAPIforSelectedVendor() == true)
        {
            DataTable dtVendorAPIStock = PullStockUsingAPI(hdnAPIurl.Value, hdnAPIPullMethod.Value, hdnAPIUID.Value);
            if (!dtVendorAPIStock.Columns.Contains("IsDefaultDiamondImageURL"))
            {
                dtVendorAPIStock.Columns.Add("IsDefaultDiamondImageURL", typeof(string));
            }
            if (!dtVendorAPIStock.Columns.Contains("IsDefaultDiamondVideoURL"))
            {
                dtVendorAPIStock.Columns.Add("IsDefaultDiamondVideoURL", typeof(string));
            }
            if (!dtVendorAPIStock.Columns.Contains("IsDefaultCertificateURL"))
            {
                dtVendorAPIStock.Columns.Add("IsDefaultCertificateURL", typeof(string));
            }
            Session["StockData"] = dtVendorAPIStock;
            if (dtVendorAPIStock != null && dtVendorAPIStock.Rows.Count > 0)
            {
                var dictMapping = GetColumnMapping(dtVendorAPIStock);
                string stoneType = CheckStoneTypeforUploadedData(dtVendorAPIStock, "");
                ViewState["stoneType"] = stoneType;

                if (stoneType == "White")
                {
                    lblMessageType.Text = "Info";
                    lblMessage.Text = "The API has pulled all White gem type stock only. Press continue to upload the stock";
                    setMessageModalPopUp(lblMessageType.Text, "");
                    mpMessage.Show();
                }
                else if (stoneType == "Color")
                {
                    lblMessageType.Text = "Info";
                    lblMessage.Text = "The API has pulled all Colored gem type stock only. Press continue to upload the stock";
                    setMessageModalPopUp(lblMessageType.Text, "");
                    mpMessage.Show();
                }
                else if (stoneType == "Both")
                {
                    lblMessageType.Text = "Info";
                    lblMessage.Text = "The API has pulled both White and Colored gem types stock. Press continue to upload the stock";
                    setMessageModalPopUp(lblMessageType.Text, "");
                    mpMessage.Show();
                }
            }
            else
            {
                lblMessageType.Text = "Error";
                lblMessage.Text = "The API haven't pulled any data to process! Please verify the API settings or check with the vendor.";
                setMessageModalPopUp(lblMessageType.Text, "");
                mpMessage.Show();
            }
        }
        else
        {
            lblMessageType.Text = "Error";
            lblMessage.Text = "Web API url or the associated webmethod are not provided for the selected Vendor. Please check.";
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();
        }

    }
    private DataTable PullStockUsingAPI(string APIurl, string pullMethod, string UID)
    {
        DataTable dtVendorAPIStock = null;
        try
        {

            System.Net.WebClient client = new System.Net.WebClient();
            System.IO.Stream stream =
                   client.OpenRead(APIurl);//("http://service.cdinesh.in/fullstockapi.asmx?wsdl");
            ServiceDescription description = ServiceDescription.Read(stream);
            var dtType = description.GetType().GetMembers();
            string serviceName = description.Services[0].Name;
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
            importer.ProtocolName = "Soap12";
            importer.AddServiceDescription(description, null, null);
            importer.Style = ServiceDescriptionImportStyle.Client;
            importer.CodeGenerationOptions =
                     System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;
            CodeNamespace nmspace = new CodeNamespace();
            CodeCompileUnit unit1 = new CodeCompileUnit();
            unit1.Namespaces.Add(nmspace);
            ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit1);
            if (warning == 0)
            {
                CodeDomProvider provider1 = CodeDomProvider.CreateProvider("CSharp");
                string[] assemblyReferences =
                  new string[2] { "System.Web.Services.dll", "System.Xml.dll" };
                CompilerParameters parms = new CompilerParameters(assemblyReferences);
                CompilerResults results = provider1.CompileAssemblyFromDom(parms, unit1);
                object[] args = new object[1];
                if (!string.IsNullOrEmpty(UID))
                    args[0] = UID;//"741cfe8f-39e3-41c2-905c-03e44d01996a";
                object wsvcClass = results.CompiledAssembly.CreateInstance(serviceName);
                //if (!string.IsNullOrEmpty(pullMethod))
                MethodInfo mi = wsvcClass.GetType().GetMethod(pullMethod);//("GetStockJsonSP");
                string result = mi.Invoke(wsvcClass, args).ToString();
                dtVendorAPIStock = (DataTable)JsonConvert.DeserializeObject(result, (typeof(DataTable)));
                dtVendorAPIStock.TableName = "Stocks";
                //ExporttoExcel(dt);
            }
        }
        catch (Exception ex)
        {
            return dtVendorAPIStock;
        }

        return dtVendorAPIStock;
    }
    protected void dbColumns_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dtrslt = (DataTable)ViewState["dirState"];
        if (dtrslt.Rows.Count > 0)
        {
            if (Convert.ToString(ViewState["sortdr"]) == "Asc")
            {
                dtrslt.DefaultView.Sort = e.SortExpression + " Desc";
                ViewState["sortdr"] = "Desc";
            }
            else
            {
                dtrslt.DefaultView.Sort = e.SortExpression + " Asc";
                ViewState["sortdr"] = "Asc";
            }
            Session["field"] = e.SortExpression;
            ListBox1.DataSource = dtrslt;
            ListBox1.DataBind();

            DataTable GSMData = (DataTable)ViewState["GSMData"];
            RequiredFields(GSMData);
            List<string> lst = (List<string>)Session["lstText"];
            if (lst != null)
            {
                for (int i = 0; i <= ListBox1.Rows.Count - 1; i++)
                {
                    foreach (var item in lst)
                    {
                        if (ListBox1.Rows[i].Cells[1].Text == item)
                        {
                            if (ListBox1.Rows[i].Cells[1].Text != "&nbsp;")
                            {
                                ListBox1.Rows[i].Cells[1].BackColor = Color.Yellow;
                            }

                        }
                    }
                }
            }
        }
    }
    protected void unMappedColumns_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Update for sorting(17/jul/2017)
        if (Session["sortlist3"] == null)
            Session["sortlist3"] = "Desc";
        else if (Session["sortlist3"].ToString() == "Asc")
            Session["sortlist3"] = "Desc";
        else if (Session["sortlist3"].ToString() == "Desc")
            Session["sortlist3"] = "Asc";

        lstUnMapColumnData = (List<string>)Session["lstunmap"];
        lstUnMapColumnData.Reverse();
        dtXmlColumns = new DataTable();
        dtXmlColumns.Columns.Add("UnMapped Columns", typeof(String));
        foreach (var item in lstUnMapColumnData)
        {
            dtXmlColumns.Rows.Add(item);
        }
        Session["lstunmap"] = lstUnMapColumnData;
        ListBox3.DataSource = dtXmlColumns;
        ListBox3.DataBind();
    }
    protected void btnColTypeContinue_Click(object sender, EventArgs e)
    {
        try
        {
            lstUnMapColumnData = (List<string>)Session["lstunmap"];
            var dict = (Dictionary<string, string>)Session["Dummydict"];
            var selectedindex = ListBox3.SelectedIndex;
            var selectedValue = lstUnMapColumnData.ElementAt(selectedindex);
            //var selectedValue = ListBox3.SelectedValue.ToString(); 
            MoveSelectedColumn(dict, selectedValue);
        }
        catch (Exception ex)
        {
        }
       
    }
    protected void DDColumnMappingVendor_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnNewColumnMapping_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/ColumnMapping.aspx");
    }
    protected void btnAddVendor_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/ManageEntities.aspx");
    }
    protected void btnMarginSetUp_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/ManageStock.aspx");
    }
    protected void btnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/UploadStock.aspx");
    }
    protected void btnErrorContinue_Click(object sender, EventArgs e)
    {
        DataTable dtCatch = (DataTable)Session["ErrorRecordsTable"];
        ExporttoExcel(dtCatch);
        mpErrorMessage.Hide();
    }
    protected void btnSuccessContinue_Click(object sender, EventArgs e)
    {
        DataTable dtVendorAPIData = (DataTable)Session["APIData"];
        dtVendorAPIData.TableName = "VendorAPI Stock";
        ExporttoExcel(dtVendorAPIData);
        mpErrorMessage.Hide();
    }
    private void SetModalErrorPopUpFields(string MessageType)
    {
        if (MessageType == "Error")
        {
            divErrorNew.Visible = true;
            divErrorSuccessNew.Visible = false;
            btnErrorContinue.Visible = true;
            btnSuccessContinue.Visible = false;
        }
        else if (MessageType == "Success")
        {
            divErrorNew.Visible = false;
            divErrorSuccessNew.Visible = true;
            btnErrorContinue.Visible = false;
            btnSuccessContinue.Visible = true;
        }
    }
    protected void btnDownloadVendorAPIFile_Click(object sender, EventArgs e)
    {
        //hdnAPIurl.Value, hdnAPIPullMethod.Value, hdnAPIUID.Value
        DataTable dtVendorAPIStock = null;
        System.Net.WebClient client = new System.Net.WebClient();
        System.IO.Stream stream =
               client.OpenRead(hdnAPIurl.Value);//("http://service.cdinesh.in/fullstockapi.asmx?wsdl");
        ServiceDescription description = ServiceDescription.Read(stream);
        var dtType = description.GetType().GetMembers();
        string serviceName = description.Services[0].Name;
        ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
        importer.ProtocolName = "Soap12";
        importer.AddServiceDescription(description, null, null);
        importer.Style = ServiceDescriptionImportStyle.Client;
        importer.CodeGenerationOptions =
                 System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;
        CodeNamespace nmspace = new CodeNamespace();
        CodeCompileUnit unit1 = new CodeCompileUnit();
        unit1.Namespaces.Add(nmspace);
        ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit1);
        if (warning == 0)
        {
            CodeDomProvider provider1 = CodeDomProvider.CreateProvider("CSharp");
            string[] assemblyReferences =
              new string[2] { "System.Web.Services.dll", "System.Xml.dll" };
            CompilerParameters parms = new CompilerParameters(assemblyReferences);
            CompilerResults results = provider1.CompileAssemblyFromDom(parms, unit1);
            object[] args = new object[1];
            if (!string.IsNullOrEmpty(hdnAPIUID.Value))
                args[0] = hdnAPIUID.Value;//"741cfe8f-39e3-41c2-905c-03e44d01996a";
            object wsvcClass = results.CompiledAssembly.CreateInstance(serviceName);
            //if (!string.IsNullOrEmpty(pullMethod))
            MethodInfo mi = wsvcClass.GetType().GetMethod(hdnAPIPullMethod.Value);//("GetStockJsonSP");
            string result = mi.Invoke(wsvcClass, args).ToString();
            dtVendorAPIStock = (DataTable)JsonConvert.DeserializeObject(result, (typeof(DataTable)));
            dtVendorAPIStock.TableName = "Stocks";
            ExporttoExcel(dtVendorAPIStock);
        }
    }
    private Boolean CheckHaveRequiredFields(Dictionary<string, string> dctMapping)
    {
        Boolean checkMandatory = false;
        var GSMData = objHelper.GetData(strColumnMapQuery);
        string stoneType = (string)ViewState["stoneType"];

        var WhiteResults = from DataRow myRow in GSMData.Rows
                           where (Boolean)myRow["IsDefault"] == true && (Boolean)myRow["IsAdditionalColoredField"] == false
                           select new { DBColumn = myRow["DBColumn"] };
        int requiredCount = 0;
        for (int i = 0; i < WhiteResults.ToList().Count; i++)
        {
            string key = WhiteResults.ToList()[i].DBColumn.ToString();


            for (int index = 0; index < dctMapping.Count; index++)
            {
                var item = dctMapping.ElementAt(index);
                var ItemKey = item.Key;
                var itemValue = item.Value;
                if (key == ItemKey)
                {
                    string val = itemValue;
                    if (!string.IsNullOrEmpty(itemValue))
                        requiredCount += 1;
                }
            }
        }
        if (requiredCount == WhiteResults.ToList().Count)
            checkMandatory = true;

        return checkMandatory;
    }

    private void DeleteRelatedInformation()
    {
        if (File.Exists(ViewState["UploadedFile"].ToString()))
        {
            File.Delete(ViewState["UploadedFile"].ToString());
        }
        ViewState["UploadedFile"] = null;
        ViewState["dirState"] = null;
        ViewState["sortdr"] = null;
        ViewState["InitialDummydict"] = null;
        ViewState["GSMData"] = null;
        ViewState["RequiredData"] = null;
        ViewState["stoneType"] = null;
        Session["lstunmap"] = null;
        Session["lstDBColumns"] = null;
        Session["StockData"] = null;
        Session["entityTypes"] = null;
        Session["lstText"] = null;
        Session["APIData"] = null;
        Session["ErrorRecordsTable"] = null;
        Session["lstmap"] = null;
        Session["Dummydict"] = null;
        Session["lstFail"] = null;
        Session["lstRequired"] = null;
    }
}