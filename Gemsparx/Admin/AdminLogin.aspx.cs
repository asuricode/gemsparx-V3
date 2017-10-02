using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Web.Security;

public partial class AdminLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userid"] != null)
        //Response.Redirect("~/Admin/UploadStock.aspx");

    }

    // For Encrypt and Decrypt The Password for Security
    //private string Encrypt(string clearText)
    //{
    //    string EncryptionKey = "MAKV2SPBNI99200";
    //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
    //    using (Aes encryptor = Aes.Create())
    //    {
    //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
    //        encryptor.Key = pdb.GetBytes(32);
    //        encryptor.IV = pdb.GetBytes(16);
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
    //            {
    //                cs.Write(clearBytes, 0, clearBytes.Length);
    //                cs.Close();
    //            }
    //            clearText = Convert.ToBase64String(ms.ToArray());
    //        }
    //    }
    //    return clearText;
    //}
    //private string Decrypt(string cipherText)
    //{
    //    string EncryptionKey = "MAKV2SPBNI99200";
    //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
    //    using (Aes encryptor = Aes.Create())
    //    {
    //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
    //        encryptor.Key = pdb.GetBytes(32);
    //        encryptor.IV = pdb.GetBytes(16);
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
    //            {
    //                cs.Write(cipherBytes, 0, cipherBytes.Length);
    //                cs.Close();
    //            }
    //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
    //        }
    //    }
    //    return cipherText;
    //}
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["GemSparxConnStr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select UserId from GemUserData where UserName =@username and Password=@password", con))
                {
                    cmd.Parameters.AddWithValue("@username", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    con.Open();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        FormsAuthentication.SetAuthCookie(txtUserName.Text.ToUpper(), false);
                        Session["userid"] = txtUserName.Text;
                        // Session["userid"] = dt.Rows[0][0].ToString();
                        Response.Redirect("~/Admin/UploadStock.aspx");
                    }
                    else
                    {
                        Session.Abandon();
                        Response.Write("<script>alert('Please enter valid Username and Password')</script>");
                        // ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Invalid Username and Password')</script>");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert( ex.Message)</script>");

        }
    }

    
}