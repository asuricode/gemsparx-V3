using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SearchDiamonds.DataAccess
{
    public class GemDBAccess
    {
         public static string ConnectionString = @"Data Source=198.71.226.2;Initial Catalog=gemsparkle;UID= gemuser; Password=Heera@9876";

  
        public static DataTable GetPrices(string minprice, string maxprice, string color, string cut, string clarity, string mincarat, string maxcarat, string shape)
        {
            var results = new DataTable();
            var sqlconn = new SqlConnection(ConnectionString);
            var sqlAdapter = new SqlDataAdapter();
            var query = "SELECT TOP 500 * FROM [ldstock]" +
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

        public static DataTable GetStoneInfo(string stoneid)
        {
            var results = new DataTable();
            var sqlconn = new SqlConnection(ConnectionString);
            var sqlAdapter = new SqlDataAdapter();
            var query = "SELECT * FROM [ldstock]" +
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