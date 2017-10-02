using SearchDiamonds.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            var calldb = GemDBAccess.GetPrices(min_price, max_price, colorfilter, cutfilter, clarityfilter, min_carat, max_carat, strshape);
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