﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class TestFileUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
            Response.Redirect("AdminLogin.aspx");
    }

    protected void UploadFile(object sender, EventArgs e)
    {
        string folderPath = Server.MapPath("~/Files/");

        //Check whether Directory (Folder) exists.
        if (!Directory.Exists(folderPath))
        {
            //If Directory (Folder) does not exists. Create it.
            Directory.CreateDirectory(folderPath);
        }

        //Save the File to the Directory (Folder).
        string str = FileUpload1.PostedFile.FileName;
        FileUpload1.SaveAs(folderPath + Path.GetFileName(FileUpload1.FileName));

        //Display the success message.
        lblMessage.Text = Path.GetFileName(FileUpload1.FileName) + " has been uploaded.";
    }
}