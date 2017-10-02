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
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Collections;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using System.Drawing;
using OfficeOpenXml;


/// <summary>
/// Summary description for HelperClass
/// </summary>
public class HelperClass
{
    string conString = ConfigurationManager.ConnectionStrings["GemSparxConnStr"].ConnectionString;
    SqlConnection con;
    public int entityId = 0;
    public HelperClass()
    {
        con = new SqlConnection(conString);
    }

    public DataTable ConvertExelToDataTable(Stream filedata, string query)
    {
        //Currently working
        //string MapingData = "";
        string MapingData = ",";
        SqlCommand cmd = new SqlCommand("select MappingColumn from GemColumnMapping");
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    foreach (DataRow data in dt.Rows)
                        MapingData += "," + data["MappingColumn"].ToString();

                    //for (int m = 0; m <= dt.Rows.Count; m++)
                    //{
                    //    MapingData += dt.Rows[m]["MappingColumn"].ToString();
                    //}



                }
            }
        }

        using ( var excel = new ExcelPackage(filedata))
        {
            int v = 0;
            try
            {
                v = excel.Workbook.Worksheets.Count();
            }
            catch
            {
                v = excel.Workbook.Worksheets.Count();
            }
          
            // string headerText = "";
            string[] headerText = new string[20];
            var tbl = new DataTable();
            var ws = (OfficeOpenXml.ExcelWorksheet)null;
             
            ws = excel.Workbook.Worksheets.First();

            //for (int z = 1; z <= excel.Workbook.Worksheets.Count(); z++)
            //{
            //    ws = excel.Workbook.Worksheets[z];
            //}

            List<string> MappingColumns = MapingData.ToUpper().Split(',').ToList<string>();
            int i = 1;
            int j = 0;
            int k = 0;
            int m = 0;
            bool t = false;
            for (; i <= ws.Dimension.End.Row; i++)
            {
                j = 1;
                var wsRow = ws.Cells[i, 1, i, ws.Dimension.End.Column];
                foreach (var cell in wsRow)
                {
                    if (cell.Text != "")
                    {
                        j = j == 1 ? j = cell.Start.Column : j;
                        if (MappingColumns.Contains(cell.Text.ToUpper()))
                        {
                            t = true;
                            break;

                        }
                    }
                    else
                    {
                        j++;
                    }
                }
                if (t)
                    break;
            }
            var hasHeader = true;  // adjust accordingly
                                   // add DataColumns to DataTable
            foreach (var firstRowCell in ws.Cells[i, j, i, ws.Dimension.End.Column])
            {
                tbl.Columns.Add(hasHeader ? firstRowCell.Text
                    : String.Format("Column {0}", j));
                headerText[0] += firstRowCell.Text.Trim();
            }



            // add DataRows to DataTable
            int startRow = hasHeader ? i + 1 : 1;
            for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                k = rowNum;
                var wsRow = ws.Cells[rowNum, j, rowNum, ws.Dimension.End.Column];
                DataRow row = tbl.NewRow();
                foreach (var cell in wsRow)
                    if (cell.Text != "")
                    {
                        string data = cell.Text.Replace("$", string.Empty);
                        if (cell.Formula == "")
                        {
                            row[cell.Start.Column - j] = data;
                        }
                        else
                        {
                            // row[cell.Start.Column - j] = cell.Text;

                            List<string> list = new List<string>();
                            Regex urlRx = new Regex(@"((https?|ftp|file)\://|www.)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*", RegexOptions.IgnoreCase);

                            MatchCollection matches = urlRx.Matches(cell.Formula);
                            if (matches.Count == 0)
                            { row[cell.Start.Column - j] = data; }
                            else
                            {
                                foreach (Match match in matches)
                                {
                                    row[cell.Start.Column - j] = match.Value;
                                }
                            }
                        }
                    }

                if (k != 0)
                    tbl.Rows.Add(row);
            }
            foreach (DataColumn col in tbl.Columns)
            {
                //col.ColumnName = Regex.Replace(col.ColumnName, @"[^0-9a-zA-Z]+", "");
                col.ColumnName = col.ColumnName.ToString().TrimEnd(' ');
            }

            return tbl;
        }


        //var file = new FileInfo(con);
        //ExcelPackage package = new ExcelPackage(filedata);

        //// ExcelPackage package = new ExcelPackage(con);
        //ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
        //DataTable table = new DataTable();
        //foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        //{
        //    table.Columns.Add(firstRowCell.Text);
        //}

        //for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        //{
        //    var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
        //    var newRow = table.NewRow();
        //    foreach (var cell in row)
        //    {
        //        newRow[cell.Start.Column - 1] = cell.Text;
        //    }
        //    table.Rows.Add(newRow);
        //}
        ////System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);
        //workSheet = null;
        //return table;
    }

    public DataTable GetExelData(string con, string query)
    {
        OleDbConnection connExcel = new OleDbConnection(con);
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();
        cmdExcel.Connection = connExcel;
        //Get the name of First Sheet
        connExcel.Open();
        DataTable dtExcelSchema;
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        //connExcel.Close();
        //Read Data from First Sheet
        //connExcel.Open();
        cmdExcel.CommandText = query + "[" + SheetName + "]";
        //if (query == "")
        //{
        //    cmdExcel.CommandText = "SELECT  * From [" + SheetName + "]";
        //}
        //else
        //{
        //    cmdExcel.CommandText = "SELECT " + query + "  From [" + SheetName + "]";
        //}
        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);
        foreach (DataColumn col in dt.Columns)
        {
            //col.ColumnName = Regex.Replace(col.ColumnName, @"[^0-9a-zA-Z]+", "");
            col.ColumnName = col.ColumnName.ToString().TrimEnd(' ');
        }
        connExcel.Close();
        return dt;
    }
    public DataTable GetData(string query)
    {
        SqlCommand cmd = new SqlCommand(query);
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }

    //spIsStockExistByStoneIDAndEntityID
    //public Boolean CheckIfStockExistsByStoneIDAndEntityID(string StoneID, int EntityID)
    //{
    //    Boolean isExists = false;
    //    try
    //    {
    //        using (SqlConnection con = new SqlConnection(conString))
    //        {
    //            con.Open();
    //            using (SqlDataAdapter sda = new SqlDataAdapter())
    //            {
    //                SqlCommand cmd = new SqlCommand("spIsStockExistByStoneIDAndEntityID", con);
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.Parameters.AddWithValue("@StoneID", StoneID);
    //                cmd.Parameters.AddWithValue("@EntityID", EntityID);
    //                cmd.Connection = con;
    //                sda.SelectCommand = cmd;
    //                using (DataTable dt = new DataTable())
    //                {
    //                    sda.Fill(dt);
    //                    if (dt.Rows.Count > 0)
    //                        isExists = true;
    //                }
    //            }
    //        }
    //        return isExists;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public DataTable GetDiscMarginFilterData(string FilterType, string FilterColumns)
    {
        string query = string.Empty;

        if (FilterType == "Vendor")
        {
            query = "Select " + FilterColumns + " from GemEntities where EntityType = 'Vendor' and IsActive = 1";
        }
        else if (FilterType == "Gem Type")
        {
            query = "Select " + FilterColumns + " from GemStoneTypes";
        }
        else if (FilterType == "Gem Color Type")
        {
            query = "Select " + FilterColumns + " from GemTypes";
        }
        else if (FilterType == "Shape")
        {
            query = "Select distinct " + FilterColumns + " from GemStock ";
        }

       

        SqlCommand cmd = new SqlCommand(query);
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }

    public DataTable GetEntitiesFilterData(string FilterType, string FilterColumns)
    {
        string query = string.Empty;

        if (FilterType == "EntityType")
        {
            query = "Select distinct EntityType from GemEntities";
        }
        else if (FilterType == "Country")
        {
            query = "Select " + FilterColumns + " from tblCountries";
        }      

        SqlCommand cmd = new SqlCommand(query);
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }

    //spGetStockSearchData
    public DataTable GetStockSearchData(string FilterType, string FilterColumnValue, string StoneID, decimal Fromvalue = 0,decimal ToValue=0)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    SqlCommand cmd = new SqlCommand("spGetStockSearchData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FilterType", FilterType);
                    cmd.Parameters.AddWithValue("@FilterColumnValue", FilterColumnValue);
                    cmd.Parameters.AddWithValue("@StoneID", StoneID);
                    cmd.Parameters.AddWithValue("@FromValue", Fromvalue);
                    cmd.Parameters.AddWithValue("@ToValue", ToValue);
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //spGetEntitySearchData

    public DataTable GetEntitySearchData(string FilterType, string FilterColumnValue, string EntityID)
    {
        //int Id = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //if (!string.IsNullOrEmpty(EntityID))
                    //    Id = int.Parse(EntityID);
                    SqlCommand cmd = new SqlCommand("spGetEntitySearchData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FilterType", FilterType);
                    cmd.Parameters.AddWithValue("@FilterColumnValue", FilterColumnValue);
                    cmd.Parameters.AddWithValue("@EntityName", EntityID);
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //spSetDiscountMargin
    public void SetDiscountMargin(string FilterType, string FilterColumnValue, string StoneID, float DiscountMargin, string ModifiedBy, decimal Fromvalue = 0, decimal ToValue = 0)
    {
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spSetDiscountMargin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FilterType", FilterType);
            cmd.Parameters.AddWithValue("@FilterColumnValue", FilterColumnValue);
            cmd.Parameters.AddWithValue("@StoneID", StoneID);
            cmd.Parameters.AddWithValue("@Margin", DiscountMargin.ToString());
            cmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy);
            cmd.Parameters.AddWithValue("@Fromvalue", Fromvalue);
            cmd.Parameters.AddWithValue("@ToValue", ToValue);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //spIsStockExistByStoneIDAndEntityID
    public void CheckIfStockExistsByStoneIDAndEntityID(string StoneID, int EntityID)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    SqlCommand cmd = new SqlCommand("spIsStockExistByStoneIDAndEntityID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StoneID", StoneID);
                    cmd.Parameters.AddWithValue("@EntityID", EntityID);
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GetData("DELETE FROM GemStock WHERE StoneID = '" + StoneID + "' and EntityID =" + EntityID + "");

                        }

                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //spUpdateChangedMapping

    public void UpdateChangedMapping(int EntityID, string DBColumn, string MappingString)
    {
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdateChangedMapping", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@EntityID", EntityID);
            cmd.Parameters.AddWithValue("@DBColumn", DBColumn);
            cmd.Parameters.AddWithValue("@MappingColumn", MappingString);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void Entity_Insert(string entityType, string entityName)
    {
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("GemEntities_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("EntityType", entityType);
            cmd.Parameters.AddWithValue("EntityName", entityName);
            cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            entityId = Convert.ToInt32(cmd.Parameters["@id"].Value.ToString());
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public void Vendor_Insert(string Addr1, string Addr2, string city, string state, string country, int zip, string phone, string mail, string ModeOfUpload, string url, string method, string uid, string createdBy, DateTime createdDate, string modifiedBy, DateTime modifiedDate)
    {
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("GemEntityDemographics_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("EntityID", entityId);
            cmd.Parameters.AddWithValue("EntityAddress1", Addr1);
            cmd.Parameters.AddWithValue("EntityAddress2", Addr2);
            cmd.Parameters.AddWithValue("EntityCity", city);
            cmd.Parameters.AddWithValue("EntityState", state);
            cmd.Parameters.AddWithValue("EntityCountry", country);
            cmd.Parameters.AddWithValue("EntityZipCode", zip);
            cmd.Parameters.AddWithValue("EntityPhone", phone);
            cmd.Parameters.AddWithValue("EntityEmail", mail);

            cmd.Parameters.AddWithValue("UploadTypes", ModeOfUpload);
            cmd.Parameters.AddWithValue("APIurl", url);
            cmd.Parameters.AddWithValue("APIGetMethod", method);
            cmd.Parameters.AddWithValue("APIUID", uid);

            cmd.Parameters.AddWithValue("CreatedBy", createdBy);
            cmd.Parameters.AddWithValue("CreatedDate", createdDate);
            cmd.Parameters.AddWithValue("ModifiedBy", modifiedBy);
            cmd.Parameters.AddWithValue("ModifiedDate", modifiedDate);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //spUpdateEntityDetails

    public void UpdateEntityByID(int EntityID, string entityType, string entityName, string Addr1, string Addr2, string city, string state, string country, int zip, string phone, string mail, string ModeOfUpload, string url, string method, string uid, string modifiedBy)
    {
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdateEntityDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EntityID", EntityID);
            cmd.Parameters.AddWithValue("@EntityType", entityType);
            cmd.Parameters.AddWithValue("@EntityName", entityName);
            cmd.Parameters.AddWithValue("@EntityAddress1", Addr1);
            cmd.Parameters.AddWithValue("@EntityAddress2", Addr2);
            cmd.Parameters.AddWithValue("@EntityCity", city);
            cmd.Parameters.AddWithValue("@EntityState", state);
            cmd.Parameters.AddWithValue("@EntityCountry", country);
            cmd.Parameters.AddWithValue("@EntityZipCode", zip);
            cmd.Parameters.AddWithValue("@EntityPhone", phone);
            cmd.Parameters.AddWithValue("@EntityEmail", mail);
            cmd.Parameters.AddWithValue("@UploadTypes", ModeOfUpload);
            cmd.Parameters.AddWithValue("@APIurl", url);
            cmd.Parameters.AddWithValue("@APIGetMethod", method);
            cmd.Parameters.AddWithValue("@APIUID", uid);
            cmd.Parameters.AddWithValue("@ModifiedBy", modifiedBy);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //spInsertUpdateColumnMapping

    //spInsertUpdateEntityDetails

    public void InsertUpdateEntityDetails(int EntityID, string entityType, string entityName, string Addr1, string Addr2, string city, string state, string country, int zip, string phone, string mail, string ModeOfUpload, string url, string method, string uid, string modifiedBy)
    {
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spInsertUpdateEntityDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EntityID", EntityID);
            cmd.Parameters.AddWithValue("@EntityType", entityType);
            cmd.Parameters.AddWithValue("@EntityName", entityName);
            cmd.Parameters.AddWithValue("@EntityAddress1", Addr1);
            cmd.Parameters.AddWithValue("@EntityAddress2", Addr2);
            cmd.Parameters.AddWithValue("@EntityCity", city);
            cmd.Parameters.AddWithValue("@EntityState", state);
            cmd.Parameters.AddWithValue("@EntityCountry", country);
            cmd.Parameters.AddWithValue("@EntityZipCode", zip);
            cmd.Parameters.AddWithValue("@EntityPhone", phone);
            cmd.Parameters.AddWithValue("@EntityEmail", mail);
            cmd.Parameters.AddWithValue("@UploadTypes", ModeOfUpload);
            cmd.Parameters.AddWithValue("@APIurl", url);
            cmd.Parameters.AddWithValue("@APIGetMethod", method);
            cmd.Parameters.AddWithValue("@APIUID", uid);
            cmd.Parameters.AddWithValue("@ModifiedBy", modifiedBy);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void InsertUpdateColumnMapping(string Action, string DBColumn, string VendorColumn, Boolean IsDefault, int EntityID)
    {
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spInsertUpdateColumnMapping", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", Action);
            cmd.Parameters.AddWithValue("@DBColumn", DBColumn);
            cmd.Parameters.AddWithValue("@MappingColumn", VendorColumn);
            cmd.Parameters.AddWithValue("@IsDefault", IsDefault);
            cmd.Parameters.AddWithValue("@EntityID", EntityID);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //spGetAllGemStock
    public DataTable GetAllGemStock()
    {
        try
        {

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    SqlCommand cmd = new SqlCommand("spGetAllGemStock", con);
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetAllEntities()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    SqlCommand cmd = new SqlCommand("spGetAllEntities", con);
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //spGetEntityDetailsByID

    public DataTable GetEntityDetailsByID(int EntityID)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    SqlCommand cmd = new SqlCommand("spGetEntityDetailsByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EntityID", EntityID);
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //spInActiveStockById

    public void InActiveStockById(int SrNo, Boolean EnableStock)
    {
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spInActiveStockById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SrNo", SrNo);
            cmd.Parameters.AddWithValue("@EnableStock", EnableStock);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //spInActiveEntityById

    public void InActiveEntityById(int EntityID, Boolean EnableEntity)
    {
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spInActiveEntityById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EntityID", EntityID);
            cmd.Parameters.AddWithValue("@EnableEntity", @EnableEntity);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //spGetStockDetailsById
    public DataTable GetStockDetailsById(int StockSrNo)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    SqlCommand cmd = new SqlCommand("spGetStockDetailsById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SrNo", StockSrNo);
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable GetEntityTypes()
    {
        DataTable entityTypes = GetData("select * from GemEntities where EntityType='Vendor' and IsActive = 1");
        return entityTypes;
    }

    public List<string> GetEntity(string strEntity)
    {
        List<string> lstEntityTypes = new List<string>();
        var entityTypes = GetData("select * from GemEntities where IsActive = 1");
        var results = from myRow in entityTypes.AsEnumerable()
                      where myRow.Field<string>("EntityType") == "Vendor"
                      select myRow.Field<string>("EntityName");
        return results.ToList();
    }

    public DataTable GemStockTable(List<string> lst)
    {
        var dt = new DataTable();
        foreach (var item in lst)
        {
            if (item == "&nbsp;")
            {
                dt.Columns.Add("", typeof(String));
            }
            else
            {
                dt.Columns.Add(item, typeof(String));
            }

        }
        dt.Columns.Add("ErrorMessage");
        return dt;
    }

    public Dictionary<string, int> GemTypeColourMapping()
    {
        var dict = new Dictionary<string, int>();
        SqlCommand cmd = new SqlCommand("select * from GemTypeToColorMapping");
        SqlConnection con = new SqlConnection(conString);
        SqlDataAdapter sda = new SqlDataAdapter();
        DataTable dt = new DataTable();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        sda.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            dict.Add(row.Field<string>("ColorName"), row.Field<int>("GEMTypeID"));
        }
        return dict;
    }

    public Dictionary<int, int> GemTypes()
    {
        var dict = new Dictionary<int, int>();
        SqlCommand cmd = new SqlCommand("select * from GemTypes");
        SqlConnection con = new SqlConnection(conString);
        SqlDataAdapter sda = new SqlDataAdapter();
        DataTable dt = new DataTable();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        sda.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
            dict.Add(row.Field<int>("TypeID"), row.Field<int>("StoneTypeID"));
        }
        return dict;
    }

    public string GetColumnDataType(string colName)
    {
        string dType = string.Empty;
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spGetColumnDatatype", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter Datatype = new SqlParameter("@Dtype", SqlDbType.VarChar, 150);
            cmd.Parameters.AddWithValue("@ColumnName", colName);
            Datatype.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(Datatype);
            cmd.ExecuteNonQuery();
            dType = cmd.Parameters["@Dtype"].Value.ToString();
            con.Close();
        }
        catch (Exception ex)
        {
            //throw ex;
            con.Close();
        }
        return dType;
    }

    public DataTable APIDetails(int id)
    {
        DataTable dt = new DataTable();
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select APIurl,APIGetMethod,APIUID from GemEntityDemographics where EntityID=" + id + "", con);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            sda.Fill(dt);

        }
        catch (Exception ex)
        {
            con.Close();
        }

        return dt;
    }
}