<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminLogin.aspx.cs" Inherits="AdminLogin" %>

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
    <div class="login-wrapper">
    <form id="form1" class="form-signin" runat="server"> 
        <div class="login-header">
      <h2 class="form-signin-heading">Sign In</h2>
            </div>      
      <asp:TextBox runat="server" class="form-control"  placeholder="User Name" id="txtUserName" required="" autofocus=""></asp:TextBox>
      <asp:TextBox runat="server" class="form-control"  placeholder="Password" TextMode="Password" id="txtPassword" required="" autofocus=""></asp:TextBox>
      

<%--      <label class="checkbox">
        <input type="checkbox" value="remember-me" id="rememberMe" name="rememberMe"> Remember me
      </label>--%>
        <%--<asp:Button ID="Button1" runat="server" Text="Button" />--%>

      <asp:Button class="btn btn-lg btn-primary btn-block" Text="Login" runat="server" type="submit" id="btnSubmit" OnClick="btnSubmit_Click"/>
    </form>
  </div>

      <%--<form id="form1" runat="server">
       <div>         
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblUsername" runat="server" Text="Username:" style="color:red"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvUser"  ErrorMessage="Please enter Username" ControlToValidate="txtUserName" runat="server" style="color:red" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPassword" runat="server" Text="Password:" style="color:red"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPWD" runat="server" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="rfvPWD" runat="server" ControlToValidate="txtPWD" ErrorMessage="Please enter Password" style="color:red"/>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </div>--%>
          
    </form>  
</body>
</html>
