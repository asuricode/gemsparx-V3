<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ColumnMapping.aspx.cs" Inherits="ColumnMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gemsparx - Column Mapping</title>
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
    <form id="form1" runat="server">

        <div class="container">
            <div class="main">
               
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
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
                                <asp:Button runat="server" ID="btnContinue" CssClass="btn btn-primary" Text="Continue" />
                            <asp:Button runat="server" ID="btnColTypeContinue" Text="Continue" />
                        </div>

                    </div>
                    <br />
                </asp:Panel>

                <div class="col-sm-4 col-sm-offset-4">
                    <div class="form-group" style="padding-top: 20px">
                        <asp:Label ID="lblvendor" Text="Select a Vendor: " runat="server"></asp:Label>
                        <asp:DropDownList ID="ddlVendor" CssClass="form-control" runat="server" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                            <asp:ListItem Text="---Select---" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        
                        <table style="vertical-align: middle">
                            <tr>
                                <td>
                                    <asp:Button ID="btnNewColumnMapping" Text="Add New Mapping" CssClass="btn btn-primary" runat="server" Visible="false" OnClick="btnNewColumnMapping_Click" /></td>
                            </tr>
                        </table>
                    </div>

                </div>
                <div id="vendorDiv" runat="server">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Button ID="btnStockUpload" Text="Stock Upload" CssClass="btn btn-primary" runat="server" OnClick="btnStockUpload_Click" />
                                </div>
                            </div>
                        </div>
                <%--<div id="divColMapGrid" runat="server">--%>
                <asp:GridView ID="gvColumnMapping" CssClass="grid" AllowSorting="true" runat="server" AutoGenerateColumns="False">
                    <RowStyle HorizontalAlign="center" Font-Size="Small" />
                    <HeaderStyle Font-Bold="True" />
                    <Columns>
                        <asp:BoundField DataField="DBColumn" HeaderText="DBColumn" />
                        <asp:TemplateField HeaderText="Vendor Mapping column">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtVendorMappingColumn" Width="400px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Is Required">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsDefault" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="btnUpdate" Text="Update" CssClass="btn btn-primary" runat="server" Visible="false" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnSave" Text="Save" CssClass="btn btn-primary" runat="server" Visible="false" OnClick="btnSave_Click" />
                <%--</div>--%>
            </div>
        </div>
       
    </form>
</body>
</html>
