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
    public partial class ViewDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            var stoneid = Request.QueryString["StoneId"];
               
            DataTable stoneinfo = GetStoneInfo(stoneid);
            foreach (DataRow row in stoneinfo.Rows)
            {
                //diamond details
                lblstone.Text = row["StoneID"].ToString();
                lblDescription.Text = row["Description"].ToString();
                
                lbllab.Text = row["Lab"].ToString();
                lblinscription.Text = row["Certificate"].ToString();
                lblshape.Text = row["Shape"].ToString();
                lblcolor.Text = row["Color"].ToString();
                lblclarity.Text = row["Clarity"].ToString();
                lblcut.Text = row["Cut"].ToString();
                lblfeature.Text = row["Key To Symbols"].ToString();
                //specs
                lbldepth.Text = row["Diameter Min"].ToString() + "-" + row["Diameter Max"].ToString() + "x" + row["Total Depth"].ToString() + "mm";
                lbllw.Text = row["Ratio"].ToString();
                lbldepthper.Text = row["Depth"].ToString();
                lbltable.Text = row["Table"].ToString();
                lblculet.Text = row["Culet"].ToString();
                lblgirdle.Text = row["Girdle Condition"].ToString();
                lblcaratwgt.Text = row["Carats"].ToString();
                lblpolish.Text = row["Polish"].ToString();
                lblsymmetry.Text = row["Symm"].ToString();
                lblflu.Text = row["Fluo Int"].ToString();
                lblprcpercrt.Text = row["Price"].ToString();
                lblamt.Text = String.Format("{0:c}", row["Amount"]);
                
                //set heading 
                heading.InnerText = row["Carats"].ToString() + " Carats " + row["Shape"].ToString() + " Shaped Diamond";
                //set paragraph 
                spcut.InnerText = row["Cut"].ToString();
                spcarats.InnerText = row["Carats"].ToString();
                spclarity.InnerText = row["Clarity"].ToString();
                spcolor.InnerText = row["Color"].ToString();
                spshape.InnerText = row["Shape"].ToString();
                //set gia pdf link
               string certurl = row["CertificateURL"].ToString();
                certurl = certurl.Replace("\\\\", "//");
                certurl = certurl.Replace("\\",@"/");
                certlink.Value = certurl;

                string diamondurl = row["DiamondImageURL"].ToString();
                diamondurl = diamondurl.Replace("\\\\", "//");
                diamondurl = diamondurl.Replace("\\", @"/");

                string DiamondVideoURL = row["DiamondVideoURL"].ToString();
                DiamondVideoURL = DiamondVideoURL.Replace("\\\\", "//");
                DiamondVideoURL = DiamondVideoURL.Replace("\\", @"/");

              
                if (DiamondVideoURL != string.Empty)
                {
                    lblVTag.Text = "Actual Video : " + row["StoneID"].ToString();
                frameVideo.Attributes["src"] = DiamondVideoURL;
                frameVideo.Visible = true;
                  
                    
                    
                }
                //select images
                if (diamondurl != string.Empty)
                { image0.Src = diamondurl; image0.Visible = true; }
               

                var shape = row["Shape"].ToString();
                switch (shape)
                {
                    case "RBC":
                        image1.Src = "Images/round_front.jpg";
                        image2.Src = "Images/round_top.jpg";
                        break;
                    case "Cushion":
                        image1.Src = "Images/cushion_front.jpg";
                        image2.Src = "Images/cushion_top.jpg";
                        break;
                    case "Pear":
                        image1.Src = "Images/pear_front.jpg";
                        image2.Src = "Images/pear_top.jpg";
                        break;
                    case "Radiant":
                        image1.Src = "Images/radiant_front.jpg";
                        image2.Src = "Images/radiant_top.jpg";
                        break;
                    case "Emerald":
                        image1.Src = "Images/emerald_front.jpg";
                        image2.Src = "Images/emerald_top.jpg";
                        break;
                    case "Oval":
                        image1.Src = "Images/oval_front.jpg";
                        image2.Src = "Images/oval_top.jpg";
                        break;
                    case "Marquise":
                        image1.Src = "Images/marquise_front.jpg";
                        image2.Src = "Images/marquise_top.jpg";
                        break;
                    case "Princess":
                        image1.Src = "Images/princess_front.jpg";
                        image2.Src = "Images/princess_top.jpg";
                        break;
                    case "Heart":
                        image1.Src = "Images/heart_front.jpg";
                        image2.Src = "Images/heart_top.jpg";
                        break;
                }
            }
        }

        public static DataTable GetStoneInfo(string stoneid)
        {
            var results = new DataTable();
            var sqlconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["GemSparxConnStr"].ConnectionString);
            var sqlAdapter = new SqlDataAdapter();
            var query = "SELECT * FROM [dbo].[ldstock]" +
                        "WHERE [StoneID] = @stoneid " +
                        "ORDER BY Shape, Color, Clarity, Country";
            //create command
            var cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@stoneid", stoneid);
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
       
}
}