<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadStock.aspx.cs" Inherits="UploadStock" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>Stock Upload</title>
    <!-- Library CSS -->
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

    <form runat="server" id="searchform">


        <asp:UpdateProgress ID="updateProgress" runat="server">
            <ProgressTemplate>
                <div class="loading-panel">
                    <div class="loading-container">
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



        <%--  <asp:Button ID="Button2" Text="Add Entity" CssClass="btn btn-primary" runat="server" OnClick="btnAddVendor_Click" />--%>
        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">

            <ContentTemplate>

                <div class="container">
                    <div class="main">
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <!-- ModalPopupExtender -->
                        <cc1:ModalPopupExtender ID="mpErrorMessage" runat="server" PopupControlID="Panel1" TargetControlID="btnTest"
                            BackgroundCssClass="modalBackground" CancelControlID="btnErrorPopCancel">
                        </cc1:ModalPopupExtender>

                        <asp:Button ID="btnTest" runat="server" Style="visibility: hidden" />
                        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopupMesage" align="center" Style="display: none">
                            <div class="alerts-wrap">
                                <div runat="server" id="divErrorNew" visible="false">
                                    <div class="icon-wrap">
                                        <div class="error-wrap">
                                            <i class="fa fa-times-circle" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="divErrorSuccessNew" visible="false">
                                    <div class="icon-wrap">
                                        <div class="success-wrap">
                                            <i class="fa fa-check-circle" aria-hidden="true"></i>
                                        </div>
                                    </div>
                                </div>
                                <div class="alert-type">
                                    <asp:Label runat="server" ID="lblErrorMessageType" CssClass="error-text"></asp:Label>
                                </div>
                                <div class="alert-msg">
                                    <asp:Label runat="server" ID="lblErrorMessage"></asp:Label>
                                </div>

                                <div class="alerts-buttons">
                                    <asp:Button runat="server" ID="btnErrorPopCancel" CssClass="btn btn-primary" Text="Ok" />&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnErrorContinue" CssClass="btn btn-primary" Text="Continue" OnClick="btnErrorContinue_Click" />&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnSuccessContinue" CssClass="btn btn-primary" Text="Continue" OnClick="btnSuccessContinue_Click" />

                                </div>

                            </div>

                            <br />

                        </asp:Panel>

                        <!-- ModalPopupExtender -->

                        <cc1:ModalPopupExtender ID="mpMessage" runat="server" PopupControlID="Panel2" TargetControlID="btn"
                            BackgroundCssClass="modalBackground" CancelControlID="btnMessageCancel">
                        </cc1:ModalPopupExtender>

                        <asp:Button ID="btn" runat="server" Style="visibility: hidden" />
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
                                <asp:Button runat="server" ID="btnContinue" CssClass="btn btn-primary" Text="Continue" OnClick="btnContinue_Click" />
                                    <asp:Button runat="server" ID="btnExcelContinue" CssClass="btn btn-primary" Text="Continue" OnClick="btnExcelContinue_Click"/>
                                    <asp:Button runat="server" ID="btnColTypeContinue" Text="Continue" OnClick="btnColTypeContinue_Click" />
                                </div>

                            </div>

                            <br />

                        </asp:Panel>



                        <div id="vendorDiv" runat="server">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Button ID="btnAddVendor" Text="Manage Entities" CssClass="btn btn-primary" runat="server" OnClick="btnAddVendor_Click" />
                                    <asp:Button ID="btnNewColumnMapping" Text="Manage Vendor Column Mapping" CssClass="btn btn-primary" runat="server" OnClick="btnNewColumnMapping_Click" Style="display: none" />
                                    <%--<asp:Button ID="btnNewColorMapping" Text="New Color Mapping" CssClass="btn btn-primary" runat="server" />--%>
                                    <asp:Button ID="btnMarginSetUp" Text="Manage Stock" CssClass="btn btn-primary" runat="server" OnClick="btnMarginSetUp_Click" />
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="object-hint">
                                    <span>Select vendor to upload the stock</span>
                                </div>

                                <div class="form-group" style="padding-top: 20px">

                                    <asp:Label ID="lblvendor" Text="Vendor: " runat="server"></asp:Label>

                                    <asp:DropDownList ID="vendorDropDown" CssClass="form-control" runat="server" OnSelectedIndexChanged="vendorDropDown_SelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="true">
                                        <asp:ListItem Text="---Select---" Value="0"></asp:ListItem>
                                    </asp:DropDownList>

                                </div>

                                <%-- <asp:Button ID="btnAddVendor" Text="Add Entity" CssClass="btn btn-primary" runat="server"  />--%>
                                <asp:Button ID="Button1" Text=" Next " runat="server" CssClass="btn btn-default" OnClick="btnNext_Click" />

                            </div>
                        </div>

                        <div id="mappingDiv" style="display: none;" runat="server">

                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="vendor-wrap">
                                        <asp:Label ID="lblEname" runat="server" CssClass="font-bold" Font-Size="Medium">  &nbsp;&nbsp;&nbsp; </asp:Label>
                                        <asp:Label ID="lblVname" runat="server" Font-Size="Medium"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:Button Text="Stock Upload Home" ID="btnHome" CssClass="btn btn-primary" runat="server" OnClick="btnHome_Click" />
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <asp:Label ID="upload" Text="Mode Of Upload: " runat="server"></asp:Label>
                                                <asp:DropDownList ID="DropDownList1" CssClass="form-control" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="true">
                                                    <%--<asp:ListItem Text="---Select---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Excel" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Text" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="API" Value="3"></asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <div class="replace-check">
                                                <asp:CheckBox ID="chkModifiedFile" runat="server" Text="Replace existing stock for the vendor." Checked="true" />
                                                    </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div id="trfile" runat="server" visible="false" style="margin-bottom:10px;">
                                                <asp:FileUpload ID="FileUpload1" name="FileUpload" runat="server" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-3">
                                            <%--<div class="col-sm-12 upload-button-wrap">--%>
                                                <div id="trsubmit" runat="server" visible="false">
                                                    <asp:Button Text="Upload" ID="btnsubmit" CssClass="btn btn-primary" runat="server" OnClick="btnsubmit_Click" />
                                                </div>

                                                <div id="tdPull" runat="server" visible="false">

                                                    <div class="form-group">
                                                        <asp:Button Text="Pull Stock" ID="btnPullStock" CssClass="btn btn-primary" runat="server" OnClick="btnPullStock_Click" />
                                                        <asp:Button ID="btnDownloadVendorAPIFile" Text="Download Vendor API Stock" CssClass="btn btn-primary" runat="server" OnClick="btnDownloadVendorAPIFile_Click" />
                                                    </div>

                                                    <%-- <asp:Button Text="Pull Stock" ID="btnPullStock" CssClass="btn btn-primary" runat="server" OnClick="btnPullStock_Click" />
                                                <asp:Button ID="btnDownloadVendorAPIFile" Text="Download Vendor API Stock" CssClass="btn btn-primary" runat="server" OnClick="btnDownloadVendorAPIFile_Click" />--%>
                                                    <asp:HiddenField ID="hdnAPIurl" runat="server" />
                                                    <asp:HiddenField ID="hdnAPIPullMethod" runat="server" />
                                                    <asp:HiddenField ID="hdnAPIUID" runat="server" />
                                                </div>
                                            <%--</div>--%>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-9">
                                    <%-- <asp:UpdatePanel runat="server" id="UpdatePanel" updatemode="Conditional">
              <ContentTemplate>--%>
                                    <div id="submapDiv" style="display: none; margin-top: 10px;" runat="server">
                                        <table>
                                            <tr class="grid-star">
                                                <td style="padding: 0px; float: right; margin-top: 10px;">
                                                    <asp:GridView ID="gvRequired" runat="server" CssClass="reqgrid no-pad" AutoGenerateColumns="true">
                                                        <RowStyle HorizontalAlign="Right" Font-Size="Small" />

                                                        <HeaderStyle Font-Bold="True" />
                                                    </asp:GridView>
                                                </td>
                                                <td style="vertical-align: top; padding-left: 0px;">

                                                    <asp:GridView ID="ListBox1" OnSorting="dbColumns_Sorting" Width="305px" AllowSorting="true" runat="server" CssClass="grid" AutoGenerateColumns="true" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged">
                                                        <RowStyle HorizontalAlign="center" Font-Size="Small" />
                                                        <HeaderStyle Font-Bold="True" />
                                                    </asp:GridView>
                                                    <asp:HiddenField runat="server" ID="hdnSelectedDBColumn" />
                                                </td>

                                                <td style="padding-left: 10px; vertical-align: top; padding-top: 260px; text-align: center">
                                                    <asp:Button ID="btnRight" Text=">>" CssClass="btn btn-primary" Style="width: 70px" runat="server" OnClick="btnRight_Click" /><br />
                                                    <br />
                                                    <br />
                                                    <asp:Button ID="btnLeft" Text="<<" CssClass="btn btn-primary" Style="width: 70px" runat="server" OnClick="btnLeft_Click" />
                                                </td>
                                                <td style="vertical-align: top;">

                                                    <div id="divGrid"
                                                        runat="server">
                                                        <%--   <asp:ListBox ID="ListBox3" runat="server" Width="150px" Font-Size="Small"></asp:ListBox>--%>
                                                        <asp:GridView ID="ListBox3" CssClass="grid fixed_headers table" OnSorting="unMappedColumns_Sorting" AllowSorting="true" runat="server" OnRowDataBound="OnRowDataBound2" OnSelectedIndexChanged="OnSelectedIndexChanged2" AutoGenerateColumns="true">
                                                            <RowStyle HorizontalAlign="center" Font-Size="Small" />
                                                            <HeaderStyle Font-Bold="True" />
                                                        </asp:GridView>
                                                        <asp:HiddenField runat="server" ID="hdnUnMappedColName" />
                                                    </div>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="padding-left: 40px; padding-top: 50px;"></td>
                                                <td style="padding-left: 60px; padding-top: 50px"></td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 40px; padding-top: 20px;"></td>
                                                <td style="padding-left: 60px; padding-top: 20px">
                                                    <asp:Button Text="Submit" ID="btnfinalSub" CssClass="btn btn-primary" runat="server" Style="width: 100px" OnClick="btnfinalSub_Click" />
                                                </td>
                                                <td style="padding-top: 20px">

                                                    <%--<asp:Button Text=" Next " ID="btnnext2" Style="width: 100px" CssClass="btn btn-default" runat="server" OnClick="btnnext2_Click" />--%>

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <%-- </ContentTemplate>
             </asp:UpdatePanel>--%>
                                </div>
                            </div>


                        </div>


                        <div id="TypeDiv" style="margin-left: 100px; display: none; margin-top: 30px; align-content: center; vertical-align: middle" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblvendortype" Text="Vendor" runat="server"></asp:Label>
                                    </td>
                                    <td style="padding-left: 20px;">Type :
                                 <asp:DropDownList ID="TypeDropDown" runat="server" OnSelectedIndexChanged="TypeDropDown_SelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="true">
                                     <asp:ListItem Text="---Select---" Value="0"></asp:ListItem>
                                     <asp:ListItem Text="Stone" Value="1"></asp:ListItem>
                                 </asp:DropDownList>
                                    </td>
                                    <td style="padding-left: 20px;">
                                        <asp:DropDownList ID="TypeDropDown2" runat="server" Visible="false">
                                            <asp:ListItem Text="---Select---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Red" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="White" Value="2"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
                                    <td style="padding-left: 20px;"></td>
                                    <td style="padding-left: 60px; padding-top: 30px">
                                        <asp:Button ID="btnClose" Text="Close" runat="server" OnClick="btnClose_Click" />
                                    </td>
                                </tr>
                            </table>

                        </div>

                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnsubmit" />
                <asp:PostBackTrigger ControlID="btnfinalSub" />
                <asp:PostBackTrigger ControlID="btnErrorContinue" />
                <asp:PostBackTrigger ControlID="btnSuccessContinue" />
                <asp:PostBackTrigger ControlID="btnDownloadVendorAPIFile" />

            </Triggers>
        </asp:UpdatePanel>

    </form>
    <!--#include virtual="/include/footermin.inc"-->
</body>
</html>

