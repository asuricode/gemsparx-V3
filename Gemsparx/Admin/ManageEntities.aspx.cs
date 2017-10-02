using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class ManageEntities : System.Web.UI.Page
{
    HelperClass objHelper;
    string strError = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("AdminLogin.aspx");
                divModeOfUpload.Visible = true;
            }
            txtAdminLogin.Visible = false;
            txtLogout.Visible = true;
            txtGridFilterEntityID.Visible = false;
            divFilterError.Visible = false;
            divNewEntityError.Visible = false;
            GetAllGemEntities();
            SetControlsForAdd();
        }
    }

    private void GetAllGemEntities()
    {
        try
        {
            objHelper = new HelperClass();
            gvEntities.DataSource = objHelper.GetAllEntities();
            gvEntities.DataBind();
        }
        catch (Exception ex)
        {
            lblMessageType.Text = "Error";
            lblMessage.Text = ex.Message;
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();
        }
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

    protected void bnSaveEntity_Click(object sender, EventArgs e)
    {



        if (bnSaveEntity.Text == "Save" || bnSaveEntity.Text == "Update")
        {
            var Emailpattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            var PhonePattern = "^[0-9]{1,11}$";


            if (txtName.Text == null || txtName.Text == "")
            {
                strError = " Please enter the name." + "<br />";
                divName.Attributes["class"] = "form-group has-error";
            }
            else
                divName.Attributes["class"] = "form-group";

            if (txtMail.Text == null || txtMail.Text == "")
            {
                strError = strError + " Please enter the mail." + "<br />";
                divEmail.Attributes["class"] = "form-group has-error";
            }
            else
                divEmail.Attributes["class"] = "form-group";

            if (!Regex.IsMatch(txtMail.Text, Emailpattern))
            {
                strError = strError + "Invalid email address." + "<br />";
                divEmail.Attributes["class"] = "form-group has-error";
            }
            else
            {
                divEmail.Attributes["class"] = "form-group";
                //objHelper = new HelperClass();
                //DataTable dt = objHelper.GetData("select * from GemEntityDemographics where EntityEmail = '" + txtMail.Text + "'");
                //if (dt.Rows.Count > 0)
                //{
                //    strError = strError + "Email address already exists." + "<br />";
                //    divEmail.Attributes["class"] = "form-group has-error";
                //    divNewEntityError.Visible = true;
                //    lblerror.Text = strError;
                //}
                //else
                //{
                //    divEmail.Attributes["class"] = "form-group";
                //}
            }





            if (txtPhone.Text == null || txtPhone.Text == "")
            {
                strError = strError + " Please enter the phone number." + " <br />";
                divPhone.Attributes["class"] = "form-group has-error";
            }
            else
                divPhone.Attributes["class"] = "form-group";

            if (!Regex.IsMatch(txtPhone.Text, PhonePattern) || txtPhone.Text.Length < 10)
            {
                strError = strError + "Invalid phone number." + " <br />";
                divPhone.Attributes["class"] = "form-group has-error";
            }
            else
                divPhone.Attributes["class"] = "form-group";

            if (ddlCountry.SelectedValue == "")
            {
                strError = strError + " Please select the country." + " <br />";
                divCountry.Attributes["class"] = "form-group has-error";
            }
            else
                divCountry.Attributes["class"] = "form-group";

            if (ddlState.SelectedValue == "")
            {
                strError = strError + " Please select the state." + " <br />";
                divState.Attributes["class"] = "form-group has-error";
            }
            else
                divState.Attributes["class"] = "form-group";

            if (ddlCity.SelectedValue == "" && txtCity.Text == "")
            {
                strError = strError + " Please select (or) enter the city." + " <br />";
                divCity.Attributes["class"] = "form-group has-error";
            }
            else
                divCity.Attributes["class"] = "form-group";

            if (txtzip.Text == null || txtzip.Text == "")
            {
                strError = strError + " Please enter zip code." + " <br />";
                divZip.Attributes["class"] = "form-group has-error";
            }
            else
                divZip.Attributes["class"] = "form-group";

            if (!Regex.IsMatch(txtzip.Text, PhonePattern) || txtzip.Text.Length < 5)
            {
                strError = strError + "Invalid zip code." + " <br />";
                divZip.Attributes["class"] = "form-group has-error";
            }
            else
                divZip.Attributes["class"] = "form-group";

            if (txtAddr1.Text == null || txtAddr1.Text == "")
            {
                strError = strError + " Please enter the address1." + " <br />";
                divAddr.Attributes["class"] = "form-group has-error";
            }
            else
                divAddr.Attributes["class"] = "form-group";


            if (ddlEntityType.SelectedValue == "0")
            {
                if (chkExcel.Checked == false && chkText.Checked == false && chkAPI.Checked == false)
                {
                    strError = strError + " Please select atleast one upload method." + " <br />";

                }

                if (chkAPI.Checked == true && string.IsNullOrEmpty(txtUrl.Text) == true && string.IsNullOrEmpty(txtPullMethod.Text) == true)
                {
                    strError = strError + " Please enter URL and Method fields." + " <br />";
                    divUrl.Attributes["class"] = "form-group has-error";
                    divPull.Attributes["class"] = "form-group has-error";
                }
                else
                {
                    divUrl.Attributes["class"] = "form-group";
                    divPull.Attributes["class"] = "form-group";
                }
            }

            if (strError != "")
            {
                divNewEntityError.Visible = true;
                lblerror.Text = strError;
                mp1.Show();
            }

            else
            {
                objHelper = new HelperClass();
                //var entityTypes = objHelper.GetEntityTypes();
                //objHelper.Entity_Insert(ddlEntityType.SelectedItem.ToString(), txtName.Text.ToString());
                string strType = "";
                if (chkExcel.Checked == true)
                {
                    strType = "Excel" + ",";
                }
                if (chkText.Checked == true)
                {
                    strType = strType + "Text" + ",";
                }
                if (chkAPI.Checked == true)
                {
                    strType = strType + "API";
                }
                strType = strType.TrimEnd(',');
                string city = "";
                if (ddlCity.Visible == false)
                {
                    city = txtCity.Text;
                }
                else
                {
                    city = ddlCity.SelectedItem.ToString();
                }
                try
                {
                    divNewEntityError.Visible = false;
                    lblerror.Text = "";
                    int Entity_ID = 0;
                    if (!string.IsNullOrEmpty(hdnID.Value))
                        Entity_ID = int.Parse(hdnID.Value);
                    objHelper.InsertUpdateEntityDetails(Entity_ID, ddlEntityType.SelectedItem.ToString(), txtName.Text, txtAddr1.Text, txtAddr2.Text, city, ddlState.SelectedItem.ToString(), ddlCountry.SelectedItem.ToString(), int.Parse(txtzip.Text), txtPhone.Text, txtMail.Text, strType, txtUrl.Text, txtPullMethod.Text, txtUID.Text, Session["userid"].ToString());
                    GetAllGemEntities();
                    ddlGridFilter.SelectedIndex = 0;
                    txtGridFilterEntityID.Visible = false;
                    ddlGridFilterValue.Visible = true;
                    lblMessageType.Text = "Success";
                    lblMessage.Text = "Entity " + bnSaveEntity.Text + "d successfully!";
                    setMessageModalPopUp(lblMessageType.Text, "");
                    mpMessage.Show();
                }
                catch (Exception ex)
                {
                    lblMessageType.Text = "Error";
                    lblMessage.Text = "Failed in saving the Entity details. Below is the error message.<br>" + "Message:" + ex.Message;
                    //lblMessage.Text = ex.Message;
                    setMessageModalPopUp(lblMessageType.Text, "");
                    mpMessage.Show();
                }
            }
        }
        else if (bnSaveEntity.Text == "Delete" || bnSaveEntity.Text == "Enable Entity")
        {
            try
            {
                objHelper = new HelperClass();
                int Entity_ID = 0;
                if (!string.IsNullOrEmpty(hdnID.Value))
                    Entity_ID = int.Parse(hdnID.Value);
                Boolean EnableEntity = false;
                if (bnSaveEntity.Text == "Enable Entity")
                    EnableEntity = true;
                objHelper.InActiveEntityById(Entity_ID, EnableEntity);
                GetAllGemEntities();
                ddlGridFilter.SelectedIndex = 0;
                txtGridFilterEntityID.Visible = false;
                ddlGridFilterValue.Visible = true;
                lblMessageType.Text = "Success";
                if (bnSaveEntity.Text == "Enable Entity")
                    lblMessage.Text = "Entity enabled successfully!";
                else
                    lblMessage.Text = "Entity deleted successfully!";
                setMessageModalPopUp(lblMessageType.Text, "");
                mpMessage.Show();
            }
            catch (Exception ex)
            {
                lblMessageType.Text = "Error";
                lblMessage.Text = "Failed in deleting the Entity. Below is the error message.<br>" + "Message:" + ex.Message;
                setMessageModalPopUp(lblMessageType.Text, "");
                mpMessage.Show();
            }
        }
    }

    protected void chkAPI_CheckChanged(object sender, System.EventArgs e)
    {
        mp1.Show();
        if (chkAPI.Checked == true)
        {
            trUrl.Style.Add("display", "visible");
            txtUrl.Visible = true;
            txtPullMethod.Visible = true;
            txtUID.Visible = true;
            lblUrl.Visible = false;
            lblPullMethod.Visible = false;
            lblUID.Visible = false;
        }
        else
        {
            trUrl.Style.Add("display", "none");
            txtUrl.Visible = false;
            txtPullMethod.Visible = false;
            txtUID.Visible = false;
            lblUrl.Visible = false;
            lblPullMethod.Visible = false;
            lblUID.Visible = false;
            txtUrl.Text = "";
            txtPullMethod.Text = "";
            txtUID.Text = "";
        }
    }

    private void IsViewMode(Boolean View)
    {
        Boolean viewAddMode = false;
        if (View == false)
        {
            viewAddMode = true;
        }
        lblEntityType.Visible = View;
        lblName.Visible = View;
        lblPhone.Visible = View;
        lblMail.Visible = View;
        lblAddr1.Visible = View;
        lblAddr2.Visible = View;
        lblCountry.Visible = View;
        //lblCity.Visible = View;
        lblState.Visible = View;
        lblZip.Visible = View;
        chkExcel.Enabled = View;
        chkText.Enabled = View;
        chkAPI.Enabled = View;
        //if (chkAPI.Checked == View)
        //{
        //    //trUrl.Visible = View;
        //    trUrl.Style.Add("display", "none");
        //    lblUrl.Visible = View;
        //    lblPullMethod.Visible = View;
        //    lblUID.Visible = View;
        //    txtUrl.Visible = viewAddMode;
        //    txtPullMethod.Visible = viewAddMode;
        //    txtUID.Visible = viewAddMode;
        //}
        ddlEntityType.Visible = viewAddMode;
        txtName.Visible = viewAddMode;
        txtAddr1.Visible = viewAddMode;
        txtAddr2.Visible = viewAddMode;
        //txtCity.Visible = viewAddMode;
        //txtState.Visible = viewAddMode;
        //txtCountry.Visible = viewAddMode;
        ddlCountry.Visible = viewAddMode;
        ddlState.Visible = viewAddMode;
        //ddlCity.Visible = viewAddMode;
        radioOther.Visible = viewAddMode;
        txtzip.Visible = viewAddMode;
        txtPhone.Visible = viewAddMode;
        txtMail.Visible = viewAddMode;
        chkExcel.Enabled = viewAddMode;
        chkText.Enabled = viewAddMode;
        chkAPI.Enabled = viewAddMode;
        //if (chkAPI.Checked == viewAddMode)
        //{
        //    //trUrl.Visible = viewAddMode;
        //    trUrl.Style.Add("display", "visible");
        //    txtUrl.Visible = viewAddMode;
        //    txtPullMethod.Visible = viewAddMode;
        //    txtUID.Visible = viewAddMode;
        //    lblUrl.Visible = View;
        //    lblPullMethod.Visible = View;
        //    lblUID.Visible = View;
        //}
        //if(View ==true)
        //{
        //    if (chkAPI.Checked == viewAddMode)
        //    {
        //        //trUrl.Visible = viewAddMode;
        //        trUrl.Style.Add("display", "visible");
        //        txtUrl.Visible = viewAddMode;
        //        txtPullMethod.Visible = viewAddMode;
        //        txtUID.Visible = viewAddMode;
        //        lblUrl.Visible = View;
        //        lblPullMethod.Visible = View;
        //        lblUID.Visible = View;
        //    }

        //}
        bnSaveEntity.Visible = viewAddMode;
    }

    protected void gvEntities_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        chkExcel.Checked = false;
        chkText.Checked = false;
        chkAPI.Checked = false;
        trUrl.Style.Add("display", "none");
        txtUrl.Text = "";
        txtPullMethod.Text = "";
        txtUID.Text = "";
        txtCity.Visible = false;
        txtCity.Text = "";
        lblerror.Text = "";
        divNewEntityError.Visible = false;
        //divError

        if (e.CommandName == "view" || e.CommandName == "deleteRow" || e.CommandName == "enable")
        {
            IsViewMode(true);
            lblPopUpHeader.Text = "View Entity";
            int index = Convert.ToInt32(e.CommandArgument.ToString());
            HiddenField hdnEntityID = (HiddenField)gvEntities.Rows[index].Cells[5].FindControl("hdnEntityID");
            hdnID.Value = hdnEntityID.Value;
            int EntityID = int.Parse(hdnEntityID.Value);
            objHelper = new HelperClass();
            DataTable dtEntity = objHelper.GetEntityDetailsByID(EntityID);
            lblEntityType.Text = dtEntity.Rows[0]["EntityType"].ToString();
            lblName.Text = dtEntity.Rows[0]["EntityName"].ToString();
            lblPhone.Text = dtEntity.Rows[0]["EntityPhone"].ToString();
            lblMail.Text = dtEntity.Rows[0]["EntityEmail"].ToString();
            lblAddr1.Text = dtEntity.Rows[0]["EntityAddress1"].ToString();
            lblAddr2.Text = dtEntity.Rows[0]["EntityAddress2"].ToString();


            lblCountry.Text = dtEntity.Rows[0]["EntityCountry"].ToString();
            lblState.Text = dtEntity.Rows[0]["EntityState"].ToString();
            lblCity.Visible = true;
            txtCity.Visible = false;
            ddlCity.Visible = false;
            lblCity.Text = dtEntity.Rows[0]["EntityCity"].ToString();
            lblZip.Text = dtEntity.Rows[0]["EntityZipCode"].ToString();
            if (dtEntity.Rows[0]["EntityType"].ToString() == "Vendor")
            {
                string[] uploadMethods = dtEntity.Rows[0]["UploadTypes"].ToString().Split(',');
                for (int i = 0; i < uploadMethods.Length; i++)
                {
                    if (uploadMethods[i] == "Excel")
                        chkExcel.Checked = true;
                    else if (uploadMethods[i] == "Text")
                        chkText.Checked = true;
                    else
                        chkAPI.Checked = true;
                }
                if (chkAPI.Checked == true)
                {
                    //trUrl.Visible = true;
                    trUrl.Style.Add("display", "visible");
                    lblUrl.Visible = true;
                    lblPullMethod.Visible = true;
                    lblUID.Visible = true;
                    txtUrl.Visible = false;
                    txtPullMethod.Visible = false;
                    txtUID.Visible = false;
                    lblUrl.Text = dtEntity.Rows[0]["APIUrl"].ToString();
                    lblPullMethod.Text = dtEntity.Rows[0]["APIGetMethod"].ToString();
                    lblUID.Text = dtEntity.Rows[0]["APIUID"].ToString();
                }


                divModeOfUpload.Visible = true;
            }
            else
                divModeOfUpload.Visible = false;

            if (e.CommandName == "deleteRow")
            {
                bnSaveEntity.Visible = true;
                bnSaveEntity.Text = "Delete";
                lblPopUpHeader.Text = "Delete Entity";
            }
            else if (e.CommandName == "enable")
            {
                bnSaveEntity.Visible = true;
                bnSaveEntity.Text = "Enable Entity";
                lblPopUpHeader.Text = "Enable Entity";
            }
            //mp1.Show();
        }
        else if (e.CommandName == "EditRow")
        {
            IsViewMode(false);
            lblerror.Text = "";
            divNewEntityError.Visible = false;
            lblCity.Visible = false;
            lblPopUpHeader.Text = "Update Entity";
            int index = Convert.ToInt32(e.CommandArgument.ToString());

            HiddenField hdnEntityID = (HiddenField)gvEntities.Rows[index].Cells[5].FindControl("hdnEntityID");

            int EntityID = int.Parse(hdnEntityID.Value);
            objHelper = new HelperClass();
            DataTable dtEntity = objHelper.GetEntityDetailsByID(EntityID);
            //lblEntityType.Text = dtEntity.Rows[0]["EntityType"].ToString();
            //ddlEntityType.Items.FindByText(dtEntity.Rows[0]["EntityType"].ToString()).Selected = true;
            //var item = dtEntity.Rows[0]["EntityType"].ToString();
            ddlEntityType.SelectedIndex = ddlEntityType.Items.IndexOf(ddlEntityType.Items.FindByText(dtEntity.Rows[0]["EntityType"].ToString()));
            hdnID.Value = hdnEntityID.Value;
            txtName.Text = dtEntity.Rows[0]["EntityName"].ToString();
            txtPhone.Text = dtEntity.Rows[0]["EntityPhone"].ToString();
            txtMail.Text = dtEntity.Rows[0]["EntityEmail"].ToString();
            txtAddr1.Text = dtEntity.Rows[0]["EntityAddress1"].ToString();
            txtAddr2.Text = dtEntity.Rows[0]["EntityAddress2"].ToString();

            BindCountries(dtEntity.Rows[0]["EntityCountry"].ToString());
            BindStates(dtEntity.Rows[0]["EntityState"].ToString());

            DataTable dtCities = objHelper.GetData(" select CityName from tblCities where StateId = " + ddlState.SelectedValue);
            Boolean checkCityExist = false;
            for (int i = 0; i < dtCities.Rows.Count; i++)
            {
                if (dtCities.Rows[i]["CityName"].ToString() == dtEntity.Rows[0]["EntityCity"].ToString())
                {
                    checkCityExist = true;
                    break;
                }
            }
            if (checkCityExist == true)
            {
                ddlCity.Visible = true;
                txtCity.Visible = false;
                BindCities(dtEntity.Rows[0]["EntityCity"].ToString());
            }
            else
            {
                //radioOther.Checked = true;
                txtCity.Visible = true;
                txtCity.Text = dtEntity.Rows[0]["EntityCity"].ToString();
                ddlCity.Visible = false;
            }
            //ddlState.SelectedValue = ddlState.Items.FindByValue(dtEntity.Rows[0]["EntityState"].ToString()).Value;
            txtzip.Text = dtEntity.Rows[0]["EntityZipCode"].ToString();
            if (dtEntity.Rows[0]["EntityType"].ToString() == "Vendor")
            {
                string[] uploadMethods = dtEntity.Rows[0]["UploadTypes"].ToString().Split(',');
                for (int i = 0; i < uploadMethods.Length; i++)
                {
                    if (uploadMethods[i] == "Excel")
                        chkExcel.Checked = true;
                    else if (uploadMethods[i] == "Text")
                        chkText.Checked = true;
                    else if (uploadMethods[i] == "API")
                        chkAPI.Checked = true;
                }
                if (chkAPI.Checked == true)
                {
                    //trUrl.Visible = true;
                    trUrl.Style.Add("display", "visible");
                    txtUrl.Visible = true;
                    txtPullMethod.Visible = true;
                    txtUID.Visible = true;
                    lblUrl.Visible = false;
                    lblPullMethod.Visible = false;
                    lblUID.Visible = false;
                    txtUrl.Text = dtEntity.Rows[0]["APIUrl"].ToString();
                    txtPullMethod.Text = dtEntity.Rows[0]["APIGetMethod"].ToString();
                    txtUID.Text = dtEntity.Rows[0]["APIUID"].ToString();
                }
                divModeOfUpload.Visible = true;
            }
            else
                divModeOfUpload.Visible = false;


            bnSaveEntity.Text = "Update";

            //mp1.Show();
        }
        mp1.Show();
    }

    private void SetControlsForAdd()
    {
        IsViewMode(false);
        txtName.Text = "";
        txtPhone.Text = "";
        txtMail.Text = "";
        txtAddr1.Text = "";
        txtAddr2.Text = "";
        //ddlCountry.SelectedIndex = 0;
        //txtCountry.Text = "";
        txtCity.Text = "";
        //ddlCity.SelectedIndex = 0;
        //ddlState.SelectedIndex = 0;
        //txtState.Text = "";
        txtzip.Text = "";
        chkExcel.Checked = false;
        chkText.Checked = false;
        chkAPI.Checked = false;
        trUrl.Style.Add("display", "none");
        txtUrl.Visible = false;
        txtPullMethod.Visible = false;
        txtUID.Visible = false;
        lblUrl.Visible = false;
        lblPullMethod.Visible = false;
        lblUID.Visible = false;
        txtUrl.Text = "";
        txtPullMethod.Text = "";
        txtUID.Text = "";
        hdnID.Value = "";
        bnSaveEntity.Text = "Save";

    }

    protected void btnAddEntity_Click(object sender, EventArgs e)
    {

        //System.Threading.Thread.Sleep(3000);
        divNewEntityError.Visible = false;
        lblCity.Visible = false;
        txtCity.Visible = false;
        ddlCity.Visible = true;
        lblerror.Text = "";
        SetControlsForAdd();
        ddlEntityType.SelectedIndex = 0;
        divModeOfUpload.Visible = true;
        objHelper = new HelperClass();
        BindCountries("");
        ddlState.Items.Clear();
        ddlCity.Items.Clear();
        ddlCountry.SelectedIndex = 0;
        lblPopUpHeader.Text = "New Entity";
        mp1.Show();
    }
    private void BindCountries(string selectedItem)
    {
        objHelper = new HelperClass();
        ddlCountry.DataSource = objHelper.GetData("Select CountryID,CountryName from tblCountries");
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataValueField = "CountryID";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("--Select--", ""));

        if (!string.IsNullOrEmpty(selectedItem))
        {
            ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByText(selectedItem));
        }
    }

    private void BindStates(string selectedItem)
    {
        objHelper = new HelperClass();
        DataTable dtStates = objHelper.GetData("select StateId, StateName from tblStates where CountryID = " + ddlCountry.SelectedValue);
        ddlState.DataSource = dtStates;
        ddlState.DataValueField = "StateID";
        ddlState.DataTextField = "StateName";
        ddlState.DataBind();
        ddlState.Items.Insert(0, new ListItem("--Select--", ""));

        if (!string.IsNullOrEmpty(selectedItem))
        {
            ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByText(selectedItem));
        }
    }

    private void BindCities(string selectedItem)
    {
        objHelper = new HelperClass();
        DataTable dtCitess = objHelper.GetData(" select* from tblCities where StateId = " + ddlState.SelectedValue);
        ddlCity.DataSource = dtCitess;
        ddlCity.DataValueField = "CityID";
        ddlCity.DataTextField = "CityName";
        ddlCity.DataBind();
        ddlCity.Items.Insert(0, new ListItem("--Select--", ""));

        if (!string.IsNullOrEmpty(selectedItem))
        {
            ddlCity.SelectedIndex = ddlCity.Items.IndexOf(ddlCity.Items.FindByText(selectedItem));
        }
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlState.Items.Clear();
        ddlCity.Items.Clear();
        objHelper = new HelperClass();
        BindStates("");
        mp1.Show();

    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCity.Items.Clear();
        BindCities("");
        mp1.Show();
    }
    protected void radioOther_CheckedChanged(object sender, EventArgs e)
    {
        if (ddlCity.Visible == true)
        {
            if (string.IsNullOrEmpty(ddlCity.SelectedValue))
            {
                if (ddlState.SelectedIndex >= 0)
                {
                    BindCities(ddlState.SelectedValue);
                }
            }
            //ddlCity.Items.Clear();
            ddlCity.Visible = false;
            txtCity.Visible = true;
        }
        else if (txtCity.Visible == true)
        {
            //txtCity.Text = "";
            txtCity.Visible = false;
            if (string.IsNullOrEmpty(ddlCity.SelectedValue))
            {
                if (ddlState.SelectedIndex >= 0)
                {
                    BindCities(ddlState.SelectedValue);
                }
            }
            ddlCity.Visible = true;
            // ddlCity.SelectedIndex = 0;

            //ddlCity.Style.Add("display", "none");
            //txtCity.Style.Add("display", "visible");           
        }
        radioOther.Checked = false;
        mp1.Show();
    }

    protected void btnStockUpload_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/UploadStock.aspx");
    }



    protected void ddlGridFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlGridFilter.SelectedIndex > 0)
        {
            if (ddlGridFilter.SelectedValue == "EntityName")
            {
                txtGridFilterEntityID.Visible = true;
                ddlGridFilterValue.Visible = false;
            }
            else if (ddlGridFilter.SelectedValue == "InActive")
            {
                ddlGridFilterValue.Items.Clear();
                ddlGridFilterValue.Visible = false;
                txtGridFilterEntityID.Visible = false;
            }
            else
            {
                objHelper = new HelperClass();
                ddlGridFilterValue.DataSource = objHelper.GetEntitiesFilterData(ddlGridFilter.SelectedItem.Text, ddlGridFilter.SelectedValue);
                string[] ResultFields = ddlGridFilter.SelectedValue.Split(',');
                ddlGridFilterValue.DataValueField = ResultFields[0];
                ddlGridFilterValue.DataTextField = ResultFields[1];
                ddlGridFilterValue.DataBind();
                ddlGridFilterValue.Visible = true;
                txtGridFilterEntityID.Visible = false;
                ddlGridFilterValue.Items.Insert(0, new ListItem("--Select--", ""));
            }
        }
        else
        {
            ddlGridFilterValue.Items.Clear();
            ddlGridFilterValue.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }

    protected void btnGridSearch_Click(object sender, EventArgs e)
    {
        if (ddlGridFilter.SelectedIndex > 0)
        {
            if ((ddlGridFilterValue.Visible == true && ddlGridFilterValue.SelectedIndex > 0) || (ddlGridFilterValue.Visible == false && txtGridFilterEntityID.Visible == false) || (txtGridFilterEntityID.Visible == true && !string.IsNullOrEmpty(txtGridFilterEntityID.Text)))
            {
                Session["EntityGridSearch"] = true;
                BindSearchEntities();
                divFilterError.Visible = false;
                lblFilterError.Text = "";
            }
            else
            {
                if (txtGridFilterEntityID.Visible == true && string.IsNullOrEmpty(txtGridFilterEntityID.Text))
                {
                    divFilterError.Visible = true;
                    lblFilterError.Text = "Please enter a value for the filter selected.";
                }
                else
                {
                    divFilterError.Visible = true;
                    lblFilterError.Text = "Please select a filter value.";
                }
            }
        }
        else
        {
            divFilterError.Visible = true;
            lblFilterError.Text = "Please select a filter.";
        }
    }

    private void BindSearchEntities()
    {
        string EntityID = string.Empty;
        string FilterValue = string.Empty;

        if (ddlGridFilter.SelectedValue == "EntityName")
        {
            EntityID = txtGridFilterEntityID.Text;
        }
        else if (ddlGridFilterValue.Visible == false && txtGridFilterEntityID.Visible == false)
        {
            FilterValue = ddlGridFilter.SelectedValue;
        }
        else
        {
            FilterValue = ddlGridFilterValue.SelectedValue;
        }
        objHelper = new HelperClass();
        gvEntities.DataSource = objHelper.GetEntitySearchData(ddlGridFilter.SelectedItem.Text, FilterValue, txtGridFilterEntityID.Text);
        gvEntities.DataBind();
    }

    protected void btnRemovegridFilter_Click(object sender, EventArgs e)
    {
        ddlGridFilter.SelectedIndex = 0;
        txtGridFilterEntityID.Visible = false;
        ddlGridFilterValue.Visible = true;
        Session["EntityGridSearch"] = true;
        GetAllGemEntities();
    }
    protected void gvEntities_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnIsActive = (HiddenField)(e.Row.FindControl("hdnIsActive"));
            LinkButton lnkDelete = (LinkButton)(e.Row.FindControl("lbDeleteEntity"));
            LinkButton lnkEnable = (LinkButton)(e.Row.FindControl("lbActivateEntity"));
            string val = hdnIsActive.Value;
            if (val == "True")
            {
                lnkDelete.Visible = true;
                lnkEnable.Visible = false;
            }
            else
            {
                lnkDelete.Visible = false;
                lnkEnable.Visible = true;
            }
        }
    }

    //protected void txtPhone_TextChanged(object sender, EventArgs e)
    //{

    //    txtPhone.Text = Convert.ToInt32(txtPhone.Text).ToString("0:###,###,##0");
    //}

    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEntityType.SelectedValue == "1")
        {
            divModeOfUpload.Visible = false;
        }
        else
            divModeOfUpload.Visible = true;

        mp1.Show();
    }

    protected void txtMail_TextChanged(object sender, EventArgs e)
    {
        objHelper = new HelperClass();
        DataTable dt = objHelper.GetData("select * from GemEntityDemographics where EntityEmail = '" + txtMail.Text + "'");
        if (dt.Rows.Count > 0)
        {
            lblMessageType.Text = "Error";
            lblMessage.Text = "Email address already exists.";
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();
            txtMail.Text = "";

        }

        mp1.Show();
        txtMail.Focus();

    }


}