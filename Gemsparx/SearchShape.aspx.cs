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
            //do not remove this line to trigger post back on slider value change
            ClientScript.GetPostBackEventReference(this, string.Empty);
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

        protected void btn_slider_event_click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void Server_Changed(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void LoadData()
        {
            //get the values from sliders
            string strshape = Request.QueryString["shape"];
            var min_price = minprice.Value;
            var max_price = maxprice.Value;
            var min_carat = mincarat.Value;
            var max_carat = maxcarat.Value;

            lblShape.Text = strshape;
            var calldb = GetShapeCount(strshape, min_price, max_price, min_carat, max_carat);
            count.InnerText = calldb.Rows.Count.ToString() + " diamonds";
            records = calldb;
            //bind data only on button click event
            gvResults.DataSource = null;
            
            gvResults.DataBind();

            
        }
        public static DataTable GetShapeCount(string inshape, string minprice, string maxprice, string mincarat, string maxcarat)
        {
            var results = new DataTable();
            var sqlconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["GemSparxConnStr"].ConnectionString);
            var sqlAdapter = new SqlDataAdapter();
            var query = "SELECT TOP 500 * FROM [ldstock]" +
                        "WHERE Carats BETWEEN @mincarat AND @maxcarat AND [Amount] BETWEEN @minprice AND @maxprice AND SHAPE =@shape";

            var cmd = new SqlCommand(string.Format(query), sqlconn);

            cmd.Parameters.AddWithValue("@minprice", minprice);
            cmd.Parameters.AddWithValue("@maxprice", maxprice);
            cmd.Parameters.AddWithValue("@mincarat", mincarat);
            cmd.Parameters.AddWithValue("@maxcarat", maxcarat);
            cmd.Parameters.AddWithValue("@shape", inshape);
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

       
}
}