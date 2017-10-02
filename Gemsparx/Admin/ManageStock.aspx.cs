using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ManageStock : System.Web.UI.Page
{
    HelperClass objHelper;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["GridSearch"] = false;
            if (Session["userid"] == null)
            Response.Redirect("AdminLogin.aspx");
            txtAdminLogin.Visible = false;
            txtLogout.Visible = true;
            BindAlLGemstock();
            txtFilterValue.Visible = false;
            txtGridFilterStoneID.Visible = false;
            mpStockDetails.Hide();
            divFilterError.Visible = false;
            divSeUpMarginError.Visible = false;
        }
    }

    private void BindAlLGemstock()
    {
        objHelper = new HelperClass();
        gvGemStock.DataSource = objHelper.GetAllGemStock();
      //  System.Threading.Thread.Sleep(5000);
        gvGemStock.DataBind();
        var totalCount = objHelper.GetAllGemStock().Rows.Count.ToString();
        lblRowCount.InnerText = "Total Recrods: " + totalCount;
    }

    //protected void btnSetDiscountMargin_Click(object sender, EventArgs e)
    //{

    //}

    protected void ddlColumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlColumn.SelectedIndex > 0)
        {
            discCarats.Visible = false;
            if (ddlColumn.SelectedValue == "StoneID")
            {
                ddlFilterResult.Items.Clear();
                txtFilterValue.Text = "";
                txtFilterValue.Visible = true;
                ddlFilterResult.Visible = false;
            }
            else if (ddlColumn.SelectedValue == "Carats")
            {
                ddlFilterResult.Items.Clear();
                txtFilterValue.Visible = false;
                ddlFilterResult.Visible = false;
                discCarats.Visible = true;
                txtCarFrom.Text = "";
                txtCarTo.Text = "";
            }
           else if (ddlColumn.SelectedValue == "Shape")
            {
                objHelper = new HelperClass();
                ddlFilterResult.DataSource = objHelper.GetDiscMarginFilterData(ddlColumn.SelectedItem.Text, ddlColumn.SelectedValue);
                string[] ResultFields = ddlColumn.SelectedValue.Split(',');
                ddlFilterResult.DataValueField = ResultFields[0];
                ddlFilterResult.DataTextField = ResultFields[0];
                ddlFilterResult.DataBind();
                ddlFilterResult.Visible = true;
                txtFilterValue.Visible = false;

                ddlFilterResult.Items.Insert(0, new ListItem("--Select--", ""));
            }
            else
            {
                objHelper = new HelperClass();
                ddlFilterResult.DataSource = objHelper.GetDiscMarginFilterData(ddlColumn.SelectedItem.Text, ddlColumn.SelectedValue);
                string[] ResultFields = ddlColumn.SelectedValue.Split(',');
                ddlFilterResult.DataValueField = ResultFields[0];
                ddlFilterResult.DataTextField = ResultFields[1];
                ddlFilterResult.DataBind();
                ddlFilterResult.Visible = true;
                txtFilterValue.Visible = false;

                ddlFilterResult.Items.Insert(0, new ListItem("--Select--", ""));
            }
            mp1.Show();
        }
        else
        {
            ddlFilterResult.Items.Clear();
            ddlFilterResult.Items.Insert(0, new ListItem("--Select--", ""));
            mp1.Show();
        }
    }

    protected void gvGemStock_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "view" || e.CommandName == "deletestock" || e.CommandName == "enable")
        {
            lblPopUpHeader.Text = "View Stock Details";
            int index = Convert.ToInt32(e.CommandArgument.ToString());

            HiddenField hdnStockSrNo = (HiddenField)gvGemStock.Rows[index].Cells[9].FindControl("hdnID");
            hdnStSrNo.Value = hdnStockSrNo.Value;
            int StockSrNo = int.Parse(hdnStockSrNo.Value);
            objHelper = new HelperClass();
            DataTable dtStock = objHelper.GetStockDetailsById(StockSrNo);
            hdnStSrNo.Value = StockSrNo.ToString();
            lblStoneIDView.Text = dtStock.Rows[0]["stoneID"].ToString();
            lblCertificateNoView.Text = dtStock.Rows[0]["CertificateNo"].ToString();
            lblShapeView.Text = dtStock.Rows[0]["Shape"].ToString();
            lblColorView.Text = dtStock.Rows[0]["Color"].ToString();
            lblCutView.Text = dtStock.Rows[0]["Cut"].ToString();
            lblEntityView.Text = dtStock.Rows[0]["EntityName"].ToString();
            lblMMView.Text = dtStock.Rows[0]["MM"].ToString();
            lblPolishView.Text = dtStock.Rows[0]["Polish"].ToString();
            lblLabView.Text = dtStock.Rows[0]["Lab"].ToString();
            lblClarityView.Text = dtStock.Rows[0]["Clarity"].ToString();
            lblCaratsView.Text = dtStock.Rows[0]["Carats"].ToString();
            lblSymmView.Text = dtStock.Rows[0]["Symm"].ToString();
            lblDepthView.Text = dtStock.Rows[0]["Depth"].ToString();
            lblTableView.Text = dtStock.Rows[0]["Table"].ToString();
            lblKeyToSymbolsView.Text = dtStock.Rows[0]["KeyToSymbols"].ToString();
            lblRapView.Text = dtStock.Rows[0]["Rap"].ToString();
            lblFullRapView.Text = dtStock.Rows[0]["FullRap"].ToString();
            lblFluoView.Text = dtStock.Rows[0]["Fluo"].ToString();
            lblDiscView.Text = dtStock.Rows[0]["Disc"].ToString();
            lblPavAngView.Text = dtStock.Rows[0]["PavAng"].ToString();
            lblGridlePerView.Text = dtStock.Rows[0]["Girdle%"].ToString();
            lblGridleConditionView.Text = dtStock.Rows[0]["GirdleCondition"].ToString();
            lblCrAngView.Text = dtStock.Rows[0]["CrAng"].ToString();
            lblCrHgtView.Text = dtStock.Rows[0]["CrHgt"].ToString();
            lblPriceView.Text = dtStock.Rows[0]["Price"].ToString();
            lblAmount.Text = dtStock.Rows[0]["CostPrice"].ToString();
            lblFancyColorView.Text = dtStock.Rows[0]["FancyColor"].ToString();
            lblFancyColorIntensityView.Text = dtStock.Rows[0]["FancyColorIntensity"].ToString();
            lblFancyColorOvertoneView.Text = dtStock.Rows[0]["FancyColorOvertone"].ToString();
            lblGemTypeView.Text = dtStock.Rows[0]["TypeName"].ToString();
            if (e.CommandName == "view")
                btnDeleteStock.Visible = false;
            else if (e.CommandName == "view")
            {
                btnDeleteStock.Visible = true;
                btnDeleteStock.Text = "Enable";
                lblPopUpHeader.Text = "Enable Stock";
            }
            else
            {
                btnDeleteStock.Visible = true;
                btnDeleteStock.Text = "Delete";
                lblPopUpHeader.Text = "Delete Stock";
            }
            mpStockDetails.Show();
        }
    }

    protected void bnSaveMargin_Click(object sender, EventArgs e)
    {
        if (ddlColumn.SelectedIndex > 0)
        {
            if (ddlFilterResult.SelectedIndex > 0 || (txtFilterValue.Visible == true && !string.IsNullOrEmpty(txtFilterValue.Text) || (txtCarFrom.Visible == true && !string.IsNullOrEmpty(txtCarFrom.Text))))
            {
                objHelper = new HelperClass();

                if (!string.IsNullOrEmpty(txtDiscMargin.Text))
                {
                    float num;
                    if (float.TryParse(txtDiscMargin.Text, out num))
                    {
                        divSeUpMarginError.Visible = false;
                        lblerror.Text = "";
                        try
                        {
                            string StoneID = string.Empty;
                            string FilterValue = string.Empty;
                            decimal FromVal = 0;
                            decimal ToVal = 0;
                            if (txtCarFrom.Text != "")
                                FromVal = Convert.ToDecimal(txtCarFrom.Text);
                            if (txtCarTo.Text != "")
                                ToVal = Convert.ToDecimal(txtCarTo.Text);

                            if (ddlColumn.SelectedValue == "StoneID")
                            {
                                StoneID = txtFilterValue.Text;
                            }
                            else if (ddlColumn.SelectedValue == "Shape")
                            { FilterValue = ddlFilterResult.SelectedValue; }
                            else
                            {
                                FilterValue = ddlFilterResult.SelectedValue;
                            }

                            if (IfStoneIDExists(StoneID) == true)
                            {
                                var DiscMargin = float.Parse(txtDiscMargin.Text);
                                objHelper.SetDiscountMargin(ddlColumn.SelectedItem.Text, FilterValue, StoneID, DiscMargin, Session["userid"].ToString());
                                mp1.Hide();
                                lblMessageType.Text = "Success";
                                lblMessage.Text = "Margin updated successfully!";
                                setMessageModalPopUp(lblMessageType.Text, "");
                                BindAlLGemstock();
                                mpMessage.Show();
                            }
                            else if (IfStoneIDExists(StoneID) != true)
                                {
                                    var DiscMargin = float.Parse(txtDiscMargin.Text);
                                    objHelper.SetDiscountMargin(ddlColumn.SelectedItem.Text, FilterValue, StoneID, DiscMargin, Session["userid"].ToString(), FromVal,ToVal);
                                    mp1.Hide();
                                    lblMessageType.Text = "Success";
                                    lblMessage.Text = "Margin updated successfully!";
                                    setMessageModalPopUp(lblMessageType.Text, "");
                                    BindAlLGemstock();
                                    mpMessage.Show();
                                }
                                else
                            {
                                divSeUpMarginError.Visible = true;
                                lblerror.Text = "StoneID doesn't exists. Please enter a valid StoneID.";
                                mp1.Show();
                            }
                        }
                        catch (Exception ex)
                        {
                            mp1.Hide();
                            lblMessageType.Text = "Error";
                            lblMessage.Text = ex.Message;
                            setMessageModalPopUp(lblMessageType.Text, "");
                            mpMessage.Show();
                        }
                    }
                    else
                    {
                        divSeUpMarginError.Visible = true;
                        lblerror.Text = "Please enter a valid value for Margin.";
                        mp1.Show();
                    }
                }
                else
                {
                    divSeUpMarginError.Visible = true;
                    lblerror.Text = "Please enter a value for Margin.";
                    mp1.Show();
                }
            }
            else
            {
                if (txtFilterValue.Visible == true)
                {
                    divSeUpMarginError.Visible = true;
                    lblerror.Text = "Please enter a filter value.";
                    mp1.Show();
                }
                else
                {
                    divSeUpMarginError.Visible = true;
                    lblerror.Text = "Please select a filter value.";
                    mp1.Show();
                }
            }
        }
        else
        {
            divSeUpMarginError.Visible = true;
            lblerror.Text = "Please select a filter.";
            mp1.Show();
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

    protected void btnDeleteStock_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean IsEnable = false;
            if (btnDeleteStock.Text == "Enable")
                IsEnable = true;
            objHelper = new HelperClass();
            objHelper.InActiveStockById(int.Parse(hdnStSrNo.Value), IsEnable);
            mp1.Hide();
            lblMessageType.Text = "Success";
            if (btnDeleteStock.Text == "Enable")
                lblMessage.Text = "Stock enabled successfully!";
            else
                lblMessage.Text = "Stock deleted successfully!";
            setMessageModalPopUp(lblMessageType.Text, "");
            BindAlLGemstock();
            mpMessage.Show();
        }
        catch (Exception ex)
        {
            mp1.Hide();
            lblMessageType.Text = "Error";
            lblMessage.Text = ex.Message;
            setMessageModalPopUp(lblMessageType.Text, "");
            mpMessage.Show();
        }
    }

    protected void btnGridSearch_Click(object sender, EventArgs e)
    {
       // System.Threading.Thread.Sleep(5000);
        if (ddlGridFilter.SelectedIndex > 0)
        {
            decimal number3 = 0;
            //if (ddlGridFilterValue.SelectedIndex > 0)
            if ((ddlGridFilterValue.Visible == true && ddlGridFilterValue.SelectedIndex > 0) || ((ddlGridFilterValue.Visible == false && txtGridFilterStoneID.Visible == false) && ((txtCaratsFrom.Visible == true && decimal.TryParse(txtCaratsFrom.Text, out number3) && (txtCaratsTo.Text == "" || (txtCaratsTo.Visible == true && decimal.TryParse(txtCaratsTo.Text, out number3)))))) || (txtGridFilterStoneID.Visible == true && !string.IsNullOrEmpty(txtGridFilterStoneID.Text)))
            {
                ViewState["GridSearch"] = true;
                BindSearchStock();
                divFilterError.Visible = false;
                divSeUpMarginError.Visible = false;
                lblFilterError.Text = "";
              //  lblCaratsMessage.Visible = false;
            }
            else
            {
                if (txtCaratsFrom.Visible == true)
                {
                    carats.Visible = true;
                    divFilterError.Visible = true;
                    lblFilterError.Text = "Please enter the Numeric values.";
                }
                else if (txtGridFilterStoneID.Visible == true)
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

    private void BindSearchStock()
    {
        string StoneID = string.Empty;
        string FilterValue = string.Empty;
        decimal Tovalue = 0;
        decimal Fromvalue = 0;
     
        if (ddlGridFilter.SelectedValue == "StoneID")
        {
            StoneID = txtGridFilterStoneID.Text;
        }
        else if (ddlGridFilter.SelectedValue == "InActive")
        {
            FilterValue = ddlGridFilter.SelectedValue;
        }
        else if (ddlGridFilter.SelectedValue == "Shape")
        {
            FilterValue = ddlGridFilterValue.SelectedValue;
        }
        else if (ddlGridFilter.SelectedValue == "Carats")
        {
            if(txtCaratsFrom.Text != "")
             Fromvalue = Convert.ToDecimal(txtCaratsFrom.Text);
            if (txtCaratsTo.Text != "")
                Tovalue =  Convert.ToDecimal( txtCaratsTo.Text);
        }
        else
        {
            FilterValue = ddlGridFilterValue.SelectedValue;
        }
        objHelper = new HelperClass();
        gvGemStock.DataSource = objHelper.GetStockSearchData(ddlGridFilter.SelectedItem.Text, FilterValue, txtGridFilterStoneID.Text, Fromvalue,Tovalue);
        gvGemStock.DataBind();
        var searchCount = objHelper.GetStockSearchData(ddlGridFilter.SelectedItem.Text, FilterValue, txtGridFilterStoneID.Text, Fromvalue, Tovalue).Rows.Count.ToString();
        lblRowCount.InnerText = "Total Recrods: " + searchCount;
    }

    protected void ddlGridFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlGridFilter.SelectedIndex > 0)
        {
            txtGridFilterStoneID.Text = "";
            carats.Visible = false;
            if (ddlGridFilter.SelectedValue == "StoneID")
            {
                txtGridFilterStoneID.Visible = true;
                ddlGridFilterValue.Visible = false;
                
                
            }
           else if (ddlGridFilter.SelectedValue == "Carats")
            {
                //  carats.Visible = true;
                carats.Attributes.CssStyle[HtmlTextWriterStyle.Visibility] = "visible";
                txtGridFilterStoneID.Visible = false;
                ddlGridFilterValue.Visible = false;
                carats.Visible = true;
                txtCaratsFrom.Text = "";
                txtCaratsTo.Text = "";
            }
            else if (ddlGridFilter.SelectedValue == "InActive")
            {
                txtGridFilterStoneID.Visible = false;
                ddlGridFilterValue.Visible = false;
            }
            else if (ddlGridFilter.SelectedValue == "Shape")
            {
                objHelper = new HelperClass();
                ddlGridFilterValue.DataSource = objHelper.GetDiscMarginFilterData(ddlGridFilter.SelectedItem.Text, ddlGridFilter.SelectedValue);
                string[] ResultFields = ddlGridFilter.SelectedValue.Split(',');
                ddlGridFilterValue.DataValueField = ResultFields[0];
                ddlGridFilterValue.DataTextField = ResultFields[0];
                ddlGridFilterValue.DataBind();
                ddlGridFilterValue.Visible = true;
                txtGridFilterStoneID.Visible = false;
                ddlGridFilterValue.Items.Insert(0, new ListItem("--Select--", ""));
            }

            else
            {
                objHelper = new HelperClass();
                ddlGridFilterValue.DataSource = objHelper.GetDiscMarginFilterData(ddlGridFilter.SelectedItem.Text, ddlGridFilter.SelectedValue);
                string[] ResultFields = ddlGridFilter.SelectedValue.Split(',');
                ddlGridFilterValue.DataValueField = ResultFields[0];
                ddlGridFilterValue.DataTextField = ResultFields[1];
                ddlGridFilterValue.DataBind();
                ddlGridFilterValue.Visible = true;
                txtGridFilterStoneID.Visible = false;
                ddlGridFilterValue.Items.Insert(0, new ListItem("--Select--", ""));
            }
        }
        else
        {
            ddlGridFilterValue.Items.Clear();
            ddlGridFilterValue.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }

    protected void gvGemStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvGemStock.PageIndex = e.NewPageIndex;
        Boolean isSearchGrid;
        isSearchGrid = (Boolean)ViewState["GridSearch"];
        if (isSearchGrid == false)
            BindAlLGemstock();
        else
            BindSearchStock();
    }

    protected void btnRemovegridFilter_Click(object sender, EventArgs e)
    {
        carats.Visible = false;
        ddlGridFilter.SelectedIndex = 0;
        ddlGridFilterValue.Visible = true;
        txtGridFilterStoneID.Visible = false;
        txtGridFilterStoneID.Text = "";
        ddlGridFilterValue.Items.Clear();
        ViewState["GridSearch"] = false;
        BindAlLGemstock();
    }

    protected void btnSetDiscountMargin_Click(object sender, EventArgs e)
    {
        divSeUpMarginError.Visible = false;
        lblerror.Text = "";
        ddlColumn.SelectedIndex = 0;
        ddlFilterResult.Visible = true;
        ddlFilterResult.Items.Clear();
        txtDiscMargin.Text = "";
        txtFilterValue.Text = "";
        discCarats.Visible = false;
        txtFilterValue.Visible = false;
        mp1.Show();
    }

    protected void gvGemStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnIsActive = (HiddenField)(e.Row.FindControl("hdnIsActive"));
            LinkButton lnkDelete = (LinkButton)(e.Row.FindControl("lblDeleteStock"));
            LinkButton lnkEnable = (LinkButton)(e.Row.FindControl("lbEnableStock"));
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

    protected void btnStockUpload_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/UploadStock.aspx");
    }
    private Boolean IfStoneIDExists(string StoneID)
    {
        Boolean isExists = false;
        objHelper = new HelperClass();
        var dtStoneID = objHelper.GetData("Select StoneID, IsActive from GemStock where StoneID = '" + txtFilterValue.Text + "' and IsActive = 1");
        if (dtStoneID.Rows.Count > 0)
        {
            isExists = true;
        }
        return isExists;
    }
}