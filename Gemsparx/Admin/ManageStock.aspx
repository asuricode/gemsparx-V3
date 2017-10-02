<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageStock.aspx.cs" Inherits="ManageStock" %>

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
    <link rel="stylesheet" href="Admin/prettyPhoto.css" media="screen" />
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
      <script src="Admin/js/respond.min.js"></script>
      <![endif]-->
    <!--[if IE]>
      <link rel="stylesheet" href="Admin/css/ie.css">
      <![endif]-->


    <%--the below three references are for slider--%>
    <link rel="stylesheet" href="Admin/Styles/jquery-ui.css" />
    <script src="Admin/Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Admin/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <%--<script src="//code.jquery.com/jquery-1.11.3.min.js"></script>--%>
    <link rel="Stylesheet" href="Admin/Styles/jquery.dataTables.min.css" />
    <script src="Admin/Scripts/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="Admin/Scripts/dataTables.scroller.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="Admin/Styles/jquery-ui-flick.css" />
    <script src="Admin/Scripts/jquery-ui-slider-pips.js" type="text/javascript"></script>
    <link rel="stylesheet" href="Admin/Styles/jquery-ui-slider-pips.css" />
    <script type="text/javascript">
        function DisableControls() {
            document.getElementById('divSeUpMarginError').style.visibility = 'none';
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

        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">

            <ContentTemplate>
                <div class="container">
                    <div class="main">
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                            <ProgressTemplate>
                                <div class="modal">
                                    <div class="center">
                                        <img alt="" src="dColor.jpg" />
                                    </div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <!-- ModalPopupExtender StockView Panel-->

                        <cc1:ModalPopupExtender ID="mpStockDetails" runat="server" PopupControlID="Panel3" TargetControlID="btnTest"
                            BackgroundCssClass="modalBackground" CancelControlID="btnCancel">
                        </cc1:ModalPopupExtender>


                        <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" Style="display: none;">


                            <div class="modal-head">
                                <asp:Label ID="lblPopUpHeader" Text="View Stock" runat="server" Font-Size="16"></asp:Label>
                            </div>

                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-sm-4">

                                        <div class="form-group">
                                            <label for="" class="control-label">Stone ID:</label>
                                            <asp:Label ID="lblStoneIDView" CssClass="form-control" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hdnStSrNo" runat="server" />

                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Certificate No.:</label>
                                            <asp:Label CssClass="form-control" ID="lblCertificateNoView" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Shape:</label>
                                            <asp:Label CssClass="form-control" ID="lblShapeView" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Color:</label>
                                            <asp:Label ID="Label2" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Color:</label>
                                            <asp:Label ID="lblColorView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">cut:</label>
                                            <asp:Label ID="lblCutView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>


                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label for="" class="control-label">Vendor:</label>
                                            <asp:Label ID="lblEntityView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                        <div class="form-group">
                                            <label for="" class="control-label">Gem Type:</label>
                                            <asp:Label ID="lblGemTypeView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Measurements:</label>
                                            <asp:Label ID="lblMMView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Polish:</label>
                                            <asp:Label ID="lblPolishView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Lab:</label>
                                            <asp:Label ID="lblLabView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label for="" class="control-label">Clarity:</label>
                                            <asp:Label ID="lblClarityView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Carats:</label>
                                            <asp:Label ID="lblCaratsView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Symmetry:</label>
                                            <asp:Label ID="lblSymmView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Depth:</label>
                                            <asp:Label ID="lblDepthView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Table:</label>
                                            <asp:Label ID="lblTableView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                    </div>



                                </div>
                                <div class="row">
                                    <div class="col-sm-4">

                                        <div class="form-group">
                                            <label for="" class="control-label">Key to Symbols:</label>
                                            <asp:Label ID="lblKeyToSymbolsView" CssClass="form-control" runat="server"></asp:Label>

                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Rap:</label>
                                            <asp:Label ID="lblRapView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Full Rap:</label>
                                            <asp:Label ID="lblFullRapView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                        <div class="form-group">
                                            <label for="" class="control-label">Fluo:</label>
                                            <asp:Label ID="lblFluoView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Discount:</label>
                                            <asp:Label ID="lblDiscView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label for="" class="control-label">Pavilion Angle:</label>
                                            <asp:Label ID="lblPavAngView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Gridle %:</label>
                                            <asp:Label ID="lblGridlePerView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Gridle Condition:</label>
                                            <asp:Label ID="lblGridleConditionView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Crown Angle:</label>
                                            <asp:Label ID="lblCrAngView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Crown Height:</label>
                                            <asp:Label ID="lblCrHgtView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label for="" class="control-label">Price:</label>
                                            <asp:Label ID="lblPriceView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Amount:</label>
                                            <asp:Label ID="lblAmount" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Fancy Color:</label>
                                            <asp:Label ID="lblFancyColorView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Fnacy Color Intensity:</label>
                                            <asp:Label ID="lblFancyColorIntensityView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <label for="" class="control-label">Fancy color overtone:</label>
                                            <asp:Label ID="lblFancyColorOvertoneView" CssClass="form-control" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                </div>


                            </div>

                            <div class="modal-footers">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Button ID="btnDeleteStock" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnDeleteStock_Click" />
                                        <%--   <asp:Button ID="btnUpdateStockDetails" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnUpdateStockDetails_Click" />--%>
                                        <asp:Button ID="btnStockDetailsCancel" runat="server" CssClass="btn btn-default" Text="Cancel" />
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                        <!-- ModalPopupExtender set discount margin Panel-->

                        <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnHidden"
                            BackgroundCssClass="modalBackground" CancelControlID="btnCancel">
                        </cc1:ModalPopupExtender>

                        <%--<asp:Button ID="btnTest" runat="server" Style="visibility: hidden" />--%>
                        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopupStock" Style="display: none;">


                            <div class="modal-head">
                                <asp:Label ID="Label1" Text="Set Margin" runat="server" Font-Size="16"></asp:Label>
                            </div>

                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-sm-12">

                                        <div class="alert alert-danger" id="divSeUpMarginError" runat="server">
                                            <asp:Label ID="lblerror" Text="" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">

                                        <div class="form-group">
                                            <label for="" class="control-label">Filter Type:</label>
                                            <asp:DropDownList ID="ddlColumn" Width="100%" CssClass="form-control" runat="server" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlColumn_SelectedIndexChanged">
                                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Vendor" Value="EntityID,EntityName"></asp:ListItem>
                                                <asp:ListItem Text="Gem Type" Value="StoneTypeID,StoneTypeName"></asp:ListItem>
                                                <asp:ListItem Text="Gem Color Type" Value="TypeID,TypeName"></asp:ListItem>
                                                <asp:ListItem Text="Stone ID" Value="StoneID"></asp:ListItem>
                                                <asp:ListItem Text="Shape" Value="Shape"></asp:ListItem>
                                                <asp:ListItem Text="Carats" Value="Carats"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="" class="control-label">Set Margin(%):</label>
                                            <asp:TextBox ID="txtDiscMargin" CssClass="form-control" runat="server"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label for="" class="control-label"></label>
                                            <asp:TextBox ID="txtFilterValue" CssClass="form-control" runat="server"></asp:TextBox>
                                            <div id="discCarats" style="width: 200px" runat="server" visible="false">
                                                <div style="width: 50%; text-align: center; float: left;">
                                                    <asp:TextBox ID="txtCarFrom" CssClass="form-control" placeholder="From Value" runat="server" ToolTip="From Value"></asp:TextBox>
                                                </div>

                                                <div style="text-align: right; width: 49%; float: right">
                                                    <asp:TextBox ID="txtCarTo" CssClass="form-control" runat="server" placeholder="To Value" ToolTip="To Value"></asp:TextBox>
                                                </div>
                                                <br />

                                                <%--<asp:Label ID="Label3" runat="server" Text="Please enter from To values" ForeColor="Black"></asp:Label>--%>
                                            </div>
                                            <asp:DropDownList ID="ddlFilterResult" Width="100%" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <asp:Button ID="bnSaveMargin" runat="server" CssClass="btn btn-primary" Text="Update Margin" OnClick="bnSaveMargin_Click" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Cancel" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </asp:Panel>
                        <!-- ModalPopupExtender -->

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
                                <asp:Button ID="btnSetDiscountMargin" Text="Manage Margin" CssClass="btn btn-primary pull-right" runat="server" OnClick="btnSetDiscountMargin_Click" />
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
                                        <asp:ListItem Text="Gem Type" Value="StoneTypeID,StoneTypeName"></asp:ListItem>
                                        <asp:ListItem Text="Gem Color Type" Value="TypeID,TypeName"></asp:ListItem>
                                        <asp:ListItem Text="InActive" Value="InActive"></asp:ListItem>
                                        <asp:ListItem Text="Stone ID" Value="StoneID"></asp:ListItem>
                                        <asp:ListItem Text="Vendor" Value="EntityID,EntityName"></asp:ListItem>
                                        <asp:ListItem Text="Shape" Value="Shape"></asp:ListItem>
                                        <asp:ListItem Text="Carats" Value="Carats"></asp:ListItem>
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
                                    <div id="carats" runat="server" visible="false">
                                        <div style="width: 80%; text-align: center; float: left;">
                                            <asp:TextBox ID="txtCaratsFrom" runat="server" placeholder="From Value" ToolTip="From Value"></asp:TextBox>
                                        </div>
                                        <%--<div style="text-align: right; width: 20%; float: right">
                                            <asp:TextBox ID="TextBox1" runat="server" ToolTip="To Value"></asp:TextBox>
                                        </div>--%>
                                        <div style="text-align: right; width: 20%; float: right">
                                            <asp:TextBox ID="txtCaratsTo" runat="server" placeholder="To Value" ToolTip="To Value"></asp:TextBox>
                                        </div>
                                        <br />

                                        <%--<asp:Label ID="lblCaratsMessage" runat="server" Text="Please enter from To values" ForeColor="Black"></asp:Label>--%>
                                    </div>
                                    <asp:TextBox ID="txtGridFilterStoneID" CssClass="form-control" runat="server"></asp:TextBox>
                                    <asp:DropDownList ID="ddlGridFilterValue" Width="100%" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 pull-right">
                                <div class="records-wrap">
                                <label id="lblRowCount" runat="server"></label>
                                    </div>
                            </div>
                        </div>

                        <div class="row">
                        <div class="col-sm-12 manage-stock-table">
                            <%--<div class="table-responsive mrg-top20">--%>
                            <asp:GridView ID="gvGemStock" CssClass="table" AllowSorting="true" runat="server" AutoGenerateColumns="False" OnRowCommand="gvGemStock_RowCommand" OnRowDataBound="gvGemStock_RowDataBound" OnPageIndexChanging="gvGemStock_PageIndexChanging" AllowPaging="true" PageSize="20">
                                <RowStyle HorizontalAlign="center" Font-Size="Small" />
                                <HeaderStyle Font-Bold="True" />
                                <Columns>
                                    <asp:BoundField DataField="stoneID" HeaderText="Stone ID" />
                                    <asp:BoundField DataField="CertificateNo" HeaderText="Certificate No." />
                                    <asp:BoundField DataField="Color" HeaderText="Color" />
                                    <asp:BoundField DataField="Clarity" HeaderText="Clarity" />
                                    <asp:BoundField DataField="Carats" HeaderText="Carats" />
                                    <asp:BoundField DataField="Shape" HeaderText="Shape" />                                    
                                    <asp:BoundField DataField="Disc" HeaderText="Discount%" />
                                    <asp:BoundField DataField="Margin" HeaderText="Margin%" />
                                    <asp:BoundField DataField="CostPrice" HeaderText="Cost Price" />
                                    <asp:BoundField DataField="EntityName" HeaderText="Vendor" />
                                    <asp:BoundField DataField="StoneTypeName" HeaderText="Gem Type" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("SrNo") %>' />
                                            <asp:LinkButton ID="lbViewStock" runat="server" Text="View" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnIsActive" runat="server" Value='<%# Eval("IsActive") %>' />
                                            <asp:LinkButton ID="lblDeleteStock" runat="server" Text="Delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                CommandName="deletestock"></asp:LinkButton>
                                            <asp:LinkButton ID="lbEnableStock" runat="server" Text="Enable" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                CommandName="enable"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--</div>--%>
                        </div>
                    </div>
                        </div>
                    <asp:Button ID="btnHidden" runat="server" Style="visibility: hidden" />
                    <asp:Button ID="btnTest" runat="server" Style="visibility: hidden" />
                    <asp:Button ID="btn" runat="server" Style="visibility: hidden" />
                    <%--</div>--%>
                </div>
            </ContentTemplate>
            <Triggers>
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
