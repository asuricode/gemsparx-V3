<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageEntities.aspx.cs" Inherits="ManageEntities" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gemsparx - Manage Entities</title>
    <link rel="stylesheet" href="/css/bootstrap.css" />
    <link rel="stylesheet" href="/css/bootstrap-theme.css" />

    <link rel="stylesheet" href="/css/stylemin.css" />
    <link rel="stylesheet" href="/css/fonts/font-awesome//css/font-awesome.css" />
    <link rel="stylesheet" href="/css/animations.css" media="screen" />
    <link rel="stylesheet" href="/css/superfish.css" media="screen" />
    <link rel="stylesheet" href="/css/revolution-slider//css/settings.css" media="screen" />
    <link rel="stylesheet" href="prettyPhoto.css" media="screen" />
    <link rel="stylesheet" href="/css/new-layout.css" />


    <!-- Skin -->
    <link rel="stylesheet" href="/css/colors/blue.css" class="colors" />
    <!-- Responsive CSS -->
    <link rel="stylesheet" href="/css/theme-responsive.css" />
    <!-- Switcher CSS -->
    <link href="/css/switcher.css" rel="stylesheet" />
    <link href="/css/spectrum.css" rel="stylesheet" />

    <!-- The HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
      <script src="//html5shim.googlecode.com/svn/trunk/html5.js"></script>
      <script src="js/respond.min.js"></script>
      <![endif]-->
    <!--[if IE]>
      <link rel="stylesheet" href="css/ie.css">
      <![endif]-->


    <%--the below three references are for slider--%>
    <link rel="stylesheet" href="Styles/jquery-ui.css" />
    <script src="Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <%--<script src="//code.jquery.com/jquery-1.11.3.min.js"></script>--%>
    <link rel="Stylesheet" href="Styles/jquery.dataTables.min.css" />
    <script src="Scripts/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="Scripts/dataTables.scroller.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="Styles/jquery-ui-flick.css" />
    <script src="Scripts/jquery-ui-slider-pips.js" type="text/javascript"></script>
    <link rel="stylesheet" href="Styles/jquery-ui-slider-pips.css" />

    <script type="text/javascript" language="javascript">
        function setUrlControls() {
            alert();
            document.getElementById('<%= txtUrl.ClientID %>').style.display = 'none';
            document.getElementById('<%= txtPullMethod.ClientID %>').style.display = 'none';
            document.getElementById('<%= txtUID.ClientID %>').style.display = 'none';
        }
        function checkcheckbox() {
            //say for example you have checkbox with id="chk"
            var value = document.getElementById("<%= chkAPI.ClientID %>").checked; //alert(value);
            if (value == true) {
                alert();
                document.getElementById('<%= txtUrl.ClientID %>').style.visibility = '';
            }
            else {
                document.getElementById('<%= txtUrl.ClientID %>').style.visibility = 'none';
            }

            // now value will have true or false. If true that means checkbox is checked and if value is false, that means checkbox is unchecked.
        }
    </script>





</head>
<body>
    <header id="header">
        <!-- Header Top Bar -->
        <!--#include virtual="/include/headertopbar.inc"-->
        <!-- Main Header Start -->
        <div class="main-header">

            <div class="container">

                <!-- TopNav Start -->
                <div class="topnav navbar-header">
                    <a class="navbar-toggle down-button" data-toggle="collapse" data-target=".slidedown">
                        <i class="fa fa-angle-down icon-current"></i>
                    </a>
                </div>
                <!-- TopNav End -->


                <!-- Menu -->
                <!--#include virtual="/include/menu.inc"-->

            </div>
        </div>
        <!-- Main Header End -->
    </header>

    <form id="form1" runat="server">

       <asp:UpdateProgress ID="updateProgress" runat="server">
            <ProgressTemplate>
                <div class="loading-panel" >
                    <div class="loading-container" >
                        <img src="<%= this.ResolveUrl("~/diamond.gif")%>" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <style>
            .loading-panel {
                background: rgba(0, 0, 0, 0.2) none repeat scroll 0 0;
                position: relative;
                width: 100%;
            }

            .loading-container {
                background: rgba(49, 133, 156, 0.4) none repeat scroll 0 0;
                color: #fff;
                font-size: 90px;
                height: 100%;
                left: 0;
                padding-top: 20%;
                position: fixed;
                text-align: center;
                top: 0;
                width: 100%;
                z-index: 999999;
            }
        </style>


        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="container">
                    <div class="main">
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <!-- ModalPopupExtender Entity Panel-->

                        <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnTest"
                            BackgroundCssClass="modalBackground" CancelControlID="btnCancel">
                        </cc1:ModalPopupExtender>

                        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup">


                            <div class="modal-head">
                                <asp:Label ID="lblPopUpHeader" Text="New Entity" runat="server" Font-Size="16"></asp:Label>
                            </div>

                            <div class="modal-body">

                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="alert alert-danger" id="divNewEntityError" runat="server">
                                            <asp:Label ID="lblerror" Text="" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label for="" class="control-label">Entity Type:</label>
                                            <asp:DropDownList ID="ddlEntityType" Width="100%" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="Vendor" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblEntityType" CssClass="form-control" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hdnID" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group" id="divName" runat="server">
                                            <label for="" class="control-label">Name:</label>
                                            <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>

                                            <asp:Label ID="lblName" CssClass="form-control" runat="server"></asp:Label>

                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group" id="divEmail" runat="server">
                                            <label for="" class="control-label">Email:</label>
                                            <asp:TextBox ID="txtMail" CssClass="form-control" runat="server" OnTextChanged="txtMail_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                            <asp:Label ID="lblMail" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="form-group" id="divPhone" runat="server">
                                            <label for="" class="control-label">Phone:</label>
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" runat="server" MaxLength="15"></asp:TextBox>
                                            <asp:Label ID="lblPhone" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group" id="divAddr" runat="server">
                                            <label for="" class="control-label">Address1:</label>
                                            <asp:TextBox ID="txtAddr1" CssClass="form-control" Rows="2" runat="server" TextMode="MultiLine"></asp:TextBox>
                                            <asp:Label ID="lblAddr1" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label for="" class="control-label">Addres2:</label>
                                            <asp:TextBox ID="txtAddr2" CssClass="form-control" Rows="2" runat="server" TextMode="MultiLine"></asp:TextBox>
                                            <asp:Label ID="lblAddr2" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                </div>


                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="form-group" id="divCountry" runat="server">
                                            <label for="" class="control-label">Country:</label>
                                            <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <%--    <asp:TextBox ID="txtCountry" CssClass="form-control" runat="server"></asp:TextBox>--%>
                                            <asp:Label ID="lblCountry" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group" id="divState" runat="server">
                                            <label for="" class="control-label">State:</label>
                                            <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <%--   <asp:TextBox ID="txtState" CssClass="form-control" runat="server"></asp:TextBox>--%>
                                            <asp:Label ID="lblState" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group others-filed" id="divCity" runat="server">
                                            <label for="" class="control-label">City:</label>
                                            <label class="radio-inline">
                                                <asp:RadioButton ID="radioOther" AutoPostBack="true" runat="server" Text="Other" OnCheckedChanged="radioOther_CheckedChanged" />
                                            </label>
                                            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtCity" CssClass="form-control" runat="server"></asp:TextBox>
                                            <asp:Label ID="lblCity" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <%--<div class="col-sm-1">
                                <div class="form-group">
                                    <div class="inline-radio-check-wrap">
                                    <label class="radio-inline">
                                        <asp:RadioButton ID="radio" runat="server" Text="Other" />
                                    </label>
                                </div>
                                </div>
                            </div>--%>
                                </div>


                                <div class="row">

                                    <div class="col-sm-4">
                                        <div class="form-group" id="divZip" runat="server">
                                            <label for="" class="control-label">ZipCode:</label>
                                            <asp:TextBox ID="txtzip" CssClass="form-control" runat="server" MaxLength="6"></asp:TextBox>
                                            <asp:Label ID="lblZip" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div id="divModeOfUpload" runat="server">



                                    <div class="row">
                                        <div class="col-sm-12">
                                            <label for="" class="control-label text-black">Mode Of Upload:</label>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="inline-radio-check-wrap">
                                                <label class="radio-inline">
                                                    <asp:CheckBox ID="chkExcel" runat="server" Text="Excel" />
                                                </label>

                                                <label class="radio-inline">
                                                    <asp:CheckBox ID="chkText" runat="server" Text="Text" />
                                                </label>
                                                <label class="radio-inline">
                                                    <asp:CheckBox ID="chkAPI" runat="server" Text="API" OnCheckedChanged="chkAPI_CheckChanged" AutoPostBack="true" />
                                                </label>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="trUrl" style="display: none" runat="server">
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group" id="divUrl" runat="server">
                                                    <label for="" class="control-label">URL:</label>
                                                    <asp:TextBox ID="txtUrl" CssClass="form-control" runat="server"></asp:TextBox>
                                                    <asp:Label ID="lblUrl" CssClass="form-control" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group" id="divPull" runat="server">
                                                    <label for="" class="control-label">Method:</label>
                                                    <asp:TextBox ID="txtPullMethod" CssClass="form-control" runat="server"></asp:TextBox>
                                                    <asp:Label ID="lblPullMethod" CssClass="form-control" runat="server"></asp:Label>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group" id="divUid" runat="server">
                                                    <label for="" class="control-label">UID:</label>
                                                    <asp:TextBox ID="txtUID" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:Label ID="lblUID" runat="server" CssClass="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="modal-footers">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Button ID="bnSaveEntity" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="bnSaveEntity_Click" />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Cancel" />
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                        <!-- ModalPopupExtender Message Panel-->

                        <cc1:ModalPopupExtender ID="mpMessage" runat="server" PopupControlID="Panel2" TargetControlID="btn"
                            BackgroundCssClass="modalBackground" CancelControlID="btnMessageCancel">
                        </cc1:ModalPopupExtender>

                        <asp:Panel ID="Panel2" runat="server" CssClass="modalPopupMesage" align="center" Style="display: none">
                            <div class="alerts-wrap">
                                <div runat="server" id="divError" visible="false">
                                    <div class="icon-wrap">
                                        <div class="error-wrap">
                                            <i class="fa fa-times-circle" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="divInfo" visible="false">
                                    <div class="icon-wrap">
                                        <div class="info-wrap">
                                            <i class="fa fa-info-circle" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>


                                <div runat="server" id="divWarning" visible="false">
                                    <div class="icon-wrap">
                                        <div class="warning-wrap">
                                            <i class="fa fa-exclamation-circle" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>


                                <div runat="server" id="divSuccess" visible="false">
                                    <div class="icon-wrap">
                                        <div class="success-wrap">
                                            <i class="fa fa-check-circle" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>

                                <div class="alert-type">
                                    <asp:Label runat="server" ID="lblMessageType" CssClass="error-text"></asp:Label>
                                </div>
                                <div class="alert-msg">
                                    <asp:Label runat="server" ID="lblMessage"></asp:Label>
                                </div>

                                <div class="alerts-buttons">
                                    <asp:Button runat="server" ID="btnMessageCancel" CssClass="btn btn-primary" Text="Ok" />&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnPopUpCancel" CssClass="btn btn-primary" Text="Cancel" />&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnContinue" CssClass="btn btn-primary" Text="Continue" />
                                    <asp:Button runat="server" ID="btnColTypeContinue" Text="Continue" />
                                </div>

                            </div>
                            <br />
                        </asp:Panel>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:Button ID="btnStockUpload" Text="Stock Upload" CssClass="btn btn-primary" runat="server" OnClick="btnStockUpload_Click" />
                                <asp:Button ID="btnAddEntity" Text="Add New Entity" CssClass="btn btn-primary pull-right" runat="server" OnClick="btnAddEntity_Click" />
                            </div>
                        </div>
                        <%--<div id="divColMapGrid" runat="server">--%>

                        <div class="row">
                            <div class="col-sm-12 mrg-top20">
                                <div class="alert alert-danger" id="divFilterError" runat="server">
                                    <asp:Label ID="lblFilterError" Text="" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label for="" class="control-label">Filter Type:</label>
                                    <asp:DropDownList ID="ddlGridFilter" Width="100%" CssClass="form-control" runat="server" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlGridFilter_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Country" Value="CountryName,CountryName"></asp:ListItem>
                                        <asp:ListItem Text="Entity Name" Value="EntityName"></asp:ListItem>
                                        <asp:ListItem Text="EntityType" Value="EntityType,EntityType"></asp:ListItem>
                                        <asp:ListItem Text="InActive" Value="InActive"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnGridSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnGridSearch_Click" />
                                    <asp:Button ID="btnRemovegridFilter" runat="server" CssClass="btn btn-primary" Text="Remove Filter" OnClick="btnRemovegridFilter_Click" />
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label for="" class="control-label"></label>
                                    <asp:TextBox ID="txtGridFilterEntityID" CssClass="form-control" runat="server"></asp:TextBox>
                                    <asp:DropDownList ID="ddlGridFilterValue" Width="100%" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-12 manage-entities-table">
                                <div class="table-responsive mrg-top20">
                                    <asp:GridView ID="gvEntities" CssClass="table" AllowSorting="true" runat="server" AutoGenerateColumns="False" OnRowCommand="gvEntities_RowCommand" OnRowDataBound="gvEntities_RowDataBound">
                                        <RowStyle HorizontalAlign="center" Font-Size="Small" />
                                        <HeaderStyle Font-Bold="True" />
                                        <Columns>
                                            <asp:BoundField DataField="EntityName" HeaderText="Entity Name" />
                                            <asp:BoundField DataField="EntityType" HeaderText="Entity Type" />
                                            <asp:BoundField DataField="EntityCountry" HeaderText="Country" />
                                            <asp:BoundField DataField="EntityCity" HeaderText="City" />
                                            <asp:BoundField DataField="EntityPhone" HeaderText="Phone" />
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnEntityID" runat="server" Value='<%# Eval("EntityID") %>' />
                                                    <asp:LinkButton ID="lbViewEntity" runat="server" Text="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                        CommandName="view"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbEditEntity" runat="server" Text="Edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                        CommandName="EditRow"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbDeleteEntity" runat="server" Text="Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                        CommandName="deleteRow"></asp:LinkButton>
                                                    <asp:LinkButton ID="lbActivateEntity" runat="server" Text="Enable" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                        CommandName="enable"></asp:LinkButton>
                                                    <asp:HiddenField ID="hdnIsActive" runat="server" Value='<%# Eval("IsActive") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="btnTest" runat="server" Style="visibility: hidden" />
                                    <asp:Button ID="btn" runat="server" Style="visibility: hidden" />
                                    <%--</div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <%-- <asp:PostBackTrigger ControlID="btnAddEntity" />
                <asp:PostBackTrigger ControlID="bnSaveEntity" />
                 <asp:PostBackTrigger ControlID="ddlCountry" />
                <asp:PostBackTrigger ControlID="ddlState" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
