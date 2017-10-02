using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace SearchDia
{
    public partial class SearchMain : System.Web.UI.Page
    {
        public static DataTable records = new DataTable();


        protected void Page_Load(object sender, EventArgs e)
        {
            //HyperLink hlRow = new HyperLink();
            //hlRow.NavigateUrl = "searchcolored.aspx?colored=Faint Pink";
            //hlRow.Text = "Faint Pink";
            //hlRow.Visible = true;
            //hlRow.Style.Add("color", "#000000");
            //hlRow.Style.Add("text-decoration", "none");

            //this.myHyperlink.Controls.Add(hlRow);
            //this.myHyperlink.Controls.Add(new LiteralControl("<br />"));

            //HyperLink h2Row = new HyperLink();
            //h2Row.NavigateUrl = "searchcolored.aspx?colored=Fancy Gray";
            //h2Row.Text = "Fancy Gray";
            //h2Row.Visible = true;
            //h2Row.Style.Add("color", "#000000");
            //h2Row.Style.Add("text-decoration", "none");
            //this.myHyperlink.Controls.Add(h2Row);
            GetColored(); 
          
            if (!Page.IsPostBack)
            {

                LoadData();
            }
            
        }       

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            gvResults.DataSource = records;
            gvResults.DataBind();
        }

        
        
        private void LoadData()
        {
            //get the values from sliders
            string strcolored = Request.QueryString["colored"];

            lblSelected.Text = " -> " + strcolored;
          
            var calldb = GetShapeCount(strcolored);
            count.InnerText = calldb.Rows.Count.ToString() + " diamonds";
            records = calldb;
            //bind data only on button click event
            gvResults.DataSource = records;
            
            gvResults.DataBind();

            
        }
        public static DataTable GetShapeCount(string strcolored)
        {
            var results = new DataTable();
            var sqlconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["GemSparxConnStr"].ConnectionString);
            var sqlAdapter = new SqlDataAdapter();
            var query = "SELECT TOP 500 * FROM [ldstock]" +
                        "WHERE Color=@colored";

            var cmd = new SqlCommand(string.Format(query), sqlconn);

            cmd.Parameters.AddWithValue("@colored", strcolored);
            sqlconn.Open();
            try
            {
                sqlAdapter.SelectCommand = cmd;
                sqlAdapter.Fill(results);
            }

            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                sqlconn.Close();
            }

            return results;
        }
        public int GetColored()
        {
            var results = new DataTable();
            var sqlconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["GemSparxConnStr"].ConnectionString);
            var sqlAdapter = new SqlDataAdapter();
            var query = "SELECT Color, Count(*) as DCount from ldstock where color not in ('A', 'B', 'C', 'D','E', 'F', 'G', 'H','I', 'J', 'K', 'L','M', 'N', 'O') group by color";

            var cmd = new SqlCommand(string.Format(query), sqlconn);

            
            sqlconn.Open();
            try
            {
                sqlAdapter.SelectCommand = cmd;
                sqlAdapter.Fill(results);

                foreach (DataRow row in results.Rows)
                {
                    

                    HyperLink hlRow = new HyperLink();
                    hlRow.NavigateUrl = "searchcolored.aspx?colored=" + row["Color"].ToString();
                    hlRow.Text = row["Color"].ToString() + "(" + row["DCount"].ToString() + ") ";
                    hlRow.Visible = true;
                    hlRow.Style.Add("color", "#000000");
                    hlRow.Style.Add("text-decoration", "none");
                    this.myHyperlink.Controls.Add(hlRow);
                    this.myHyperlink.Controls.Add(new LiteralControl("&nbsp;"));

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlconn.Close();
            }

            return 0;
        }
        protected void view_click(object sender, EventArgs e)
        {
            var btn = (ImageButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            var rowindex = gvr.RowIndex;
            var stoneid = gvResults.Rows[rowindex].Cells[0].Text;
            var url = "ViewDetails.aspx?StoneId=" + stoneid;
            Response.Redirect(url);
        }
        protected void gvResults_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void lblLB1_Click(object sender, EventArgs e)
        {

        }
}
}