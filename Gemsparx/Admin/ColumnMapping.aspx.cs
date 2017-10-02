using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ColumnMapping : System.Web.UI.Page
{
    public HelperClass objHelper;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["userid"] == null)
            {             
                Response.Redirect("AdminLogin.aspx");
            }
            txtAdminLogin.Visible = false;
            txtLogout.Visible = true;

            BindVendors();
        }
    }


    private void BindVendors()
    {
        objHelper = new HelperClass();
        DataTable dtEntities = objHelper.GetEntityTypes();
        ddlVendor.DataSource = dtEntities;
        ddlVendor.DataValueField = "EntityID";
        ddlVendor.DataTextField = "EntityName";
        ddlVendor.DataBind();
    }
    private void BindGemStockDbColumns()
    {
        objHelper = new HelperClass();
        var dtDBColumns = objHelper.GetData("spGetGemStockColumns");
        gvColumnMapping.DataSource = dtDBColumns;
        gvColumnMapping.DataBind();
    }

    private void BindColumnMappings()
    {
        objHelper = new HelperClass();
        var dtColumnMapping = objHelper.GetData("Select DBColumn,MappingColumn,IsDefault from GemColumnMapping where EntityID = " + ddlVendor.SelectedValue);
        if (dtColumnMapping.Rows.Count > 0)
        {
            gvColumnMapping.DataSource = dtColumnMapping;
            gvColumnMapping.DataBind();

            for (int i = 0; i <= gvColumnMapping.Rows.Count - 1; i++)
            {
                GridViewRow row = gvColumnMapping.Rows[i];
                TextBox txtMapping = (TextBox)row.FindControl("txtVendorMappingColumn");
                txtMapping.Text = dtColumnMapping.Rows[i]["MappingColumn"].ToString();
                CheckBox Chbox = (CheckBox)row.FindControl("cbIsDefault");
                Chbox.Checked = (Boolean)dtColumnMapping.Rows[i]["IsDefault"];
            }
            btnUpdate.Visible = true;
            btnSave.Visible = false;
        }
        else
        {
            btnNewColumnMapping.Visible = true;
        }
    }


    protected void btnNewColumnMapping_Click(object sender, EventArgs e)
    {
        BindGemStockDbColumns();
        btnSave.Visible = true;
        btnUpdate.Visible = false;
    }

    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindColumnMappings();
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
        else if (MessageType == "Success")
        {
            divSuccess.Visible = true;
            btnMessageCancel.Visible = true;
            mpMessage.CancelControlID = btnMessageCancel.ID;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            objHelper = new HelperClass();
            for (int i = 0; i <= gvColumnMapping.Rows.Count - 1; i++)
            {
                GridViewRow row = gvColumnMapping.Rows[i];
                string DBColumn = row.Cells[0].Text;

                TextBox txtMapping = (TextBox)row.Cells[1].FindControl("txtVendorMappingColumn");
                string MappingColumn = txtMapping.Text;

                CheckBox Chbox = (CheckBox)row.Cells[2].FindControl("cbIsDefault");
                Boolean IsDefault = Chbox.Checked;
                objHelper.InsertUpdateColumnMapping("Update", DBColumn, MappingColumn, IsDefault, int.Parse(ddlVendor.SelectedValue));
            }
            lblMessageType.Text = "Success";
            lblMessage.Text = "Column mapping updated successfully!";
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();
        }
        catch(Exception ex)
        {
            lblMessageType.Text = "Error";
            lblMessage.Text = ex.Message;
            setMessageModalPopUp(lblMessageType.Text,"");
            mpMessage.Show();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            objHelper = new HelperClass();
            for (int i = 0; i <= gvColumnMapping.Rows.Count - 1; i++)
            {
                GridViewRow row = gvColumnMapping.Rows[i];
                string DBColumn = row.Cells[0].Text;

                TextBox txtMapping = (TextBox)row.Cells[1].FindControl("txtVendorMappingColumn");
                string MappingColumn = txtMapping.Text;

                CheckBox Chbox = (CheckBox)row.Cells[2].FindControl("cbIsDefault");
                Boolean IsDefault = Chbox.Checked;
                objHelper.InsertUpdateColumnMapping("Add", DBColumn, MappingColumn, IsDefault, int.Parse(ddlVendor.SelectedValue));
            }
            btnNewColumnMapping.Visible = false;
            lblMessageType.Text = "Success";
            lblMessage.Text = "Column mapping saved successfully!";
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();
        }
        catch(Exception ex)
        {
            lblMessageType.Text = "Error";
            lblMessage.Text = ex.Message;
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();
        }
    }

    protected void btnStockUpload_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/UploadStock.aspx");
    }
}