
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace SearchDia
{
    public partial class SearchMain : System.Web.UI.Page
    {
        public static string[] color = { "J", "I", "H", "G", "F", "E", "D", "K","N","M","L" };
        public static string[] cut = { "FR", "GD", "VG", "PR", "EX" };
        public static string[] clarity = { "SI2", "SI1", "VS2", "VS1", "VVS2", "VVS1", "IF", "I1", "I2", "I3" };

        protected void Page_Load(object sender, EventArgs e)
        {
            //do not remove this line to trigger post back on slider value change
            ClientScript.GetPostBackEventReference(this, string.Empty);           
        }       

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btn_slider_event_click(object sender, EventArgs e)
        {
            LoadData(true);
        }

        protected void Server_Changed(object sender, EventArgs e)
        {
            LoadData(true);
        }     

        private void LoadData(bool isslidercall = false)
        {
            //get the values from sliders
            string strshape = string.Empty;
            var min_price = minprice.Value;
            var max_price = maxprice.Value;
            var min_carat = mincarat.Value;
            var max_carat = maxcarat.Value;
            var colorfilter = color[Convert.ToInt32(mincolor.Value)];
            for (int i = Convert.ToInt32(mincolor.Value); i <= Convert.ToInt32(maxcolor.Value); ++i)
            {
                colorfilter += "," + color[i];
            }
            var cutfilter = cut[Convert.ToInt32(mincut.Value)];
            for (int i = Convert.ToInt32(mincut.Value); i <= Convert.ToInt32(maxcut.Value); ++i)
            {
                cutfilter += "," + cut[i];
            }
            var clarityfilter = clarity[Convert.ToInt32(minclarity.Value)];
            for (int i = Convert.ToInt32(minclarity.Value); i <= Convert.ToInt32(maxclarity.Value); ++i)
            {
                clarityfilter += "," + clarity[i];
            }
            //We just need HtmlInputCheckBox
            IEnumerable<Control> _ctrls = from Control n in this.searchform.Controls where n as System.Web.UI.WebControls.CheckBox != null select n;

            if (_ctrls.Count() > 0)
            {
                foreach (System.Web.UI.WebControls.CheckBox item in _ctrls)
                {
                    if (item.Checked)
                    {
                        strshape += item.ID + ",";
                    }
                }
            }
            var calldb = GetPrices(min_price, max_price, colorfilter, cutfilter, clarityfilter, min_carat, max_carat, strshape);
            count.InnerText = calldb.Rows.Count.ToString() + " diamonds";
            if (isslidercall == false)
            {
                gvResults.DataSource = calldb;
                gvResults.DataBind();
            }
            else
            {
                gvResults.DataSource = null;
                gvResults.DataBind();
            }
        }
        public static string ConnectionString = @"Data Source=198.71.226.2;Initial Catalog=gemsparkle;UID= gemuser; Password=Heera@9876";


        public static DataTable GetPrices(string minprice, string maxprice, string color, string cut, string clarity, string mincarat, string maxcarat, string shape)
        {
            var results = new DataTable();
            var sqlconn = new SqlConnection(ConnectionString);
            var sqlAdapter = new SqlDataAdapter();
            var query = "SELECT * FROM [ldstock]" +
                        "WHERE Carats BETWEEN @mincarat AND @maxcarat AND [Amount] BETWEEN @minprice AND @maxprice AND Color IN ({0}) AND CUT IN ({1}) AND CLARITY IN ({2}) AND SHAPE IN ({3})";
            //"ORDER BY Shape, Color, Clarity, Country";
            //in clause for color
            var colors = color.Split(',');
            var colorparams = colors.Select((s, i) => "@color" + i.ToString()).ToArray();
            var colorinclause = string.Join(",", colorparams);
            //inclause for cut
            var cuts = cut.Split(',');
            var cutparams = cuts.Select((s, i) => "@cut" + i.ToString()).ToArray();
            var cutinclause = string.Join(",", cutparams);
            //inclause for clarity
            var clarityarry = clarity.Split(',');
            var clarityparams = clarityarry.Select((s, i) => "@clarity" + i.ToString()).ToArray();
            var clartyinclause = string.Join(",", clarityparams);
            //inclause for shape
            var shapearry = shape.Split(',');
            var shapeparams = shapearry.Select((s, i) => "@shape" + i.ToString()).ToArray();
            var shapeinclause = string.Join(",", shapeparams);
            //create command
            var cmd = new SqlCommand(string.Format(query, colorinclause, cutinclause, clartyinclause, shapeinclause), sqlconn);
            for (var i = 0; i < colors.Length; i++)
            {
                cmd.Parameters.AddWithValue(colorparams[i], colors[i]);
            }
            for (var i = 0; i < cuts.Length; i++)
            {
                cmd.Parameters.AddWithValue(cutparams[i], cuts[i]);
            }
            for (var i = 0; i < clarityarry.Length; i++)
            {
                cmd.Parameters.AddWithValue(clarityparams[i], clarityarry[i]);
            }
            for (var i = 0; i < shapearry.Length; i++)
            {
                cmd.Parameters.AddWithValue(shapeparams[i], shapearry[i]);
            }
            cmd.Parameters.AddWithValue("@minprice", minprice);
            cmd.Parameters.AddWithValue("@maxprice", maxprice);
            cmd.Parameters.AddWithValue("@mincarat", mincarat);
            cmd.Parameters.AddWithValue("@maxcarat", maxcarat);
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
            var url = "ViewDiamond.aspx?StoneId=" + stoneid;
            Response.Redirect(url);
        }
    }
}