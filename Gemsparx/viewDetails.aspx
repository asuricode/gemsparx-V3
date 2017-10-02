<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewDetails.aspx.cs" Inherits="SearchDia.ViewDetails" %>

<!DOCTYPE html>

<style type="text/css">
    .auto-style1 {
        width: 10px;
    }

    .auto-style2 {
        width: 12px;
    }

    .auto-style3 {
        width: 13px;
    }

    .auto-style4 {
        width: 35px;
    }
</style>

<!--[if IE 8]>			<html class="ie ie8"> <![endif]-->
<!--[if IE 9]>			<html class="ie ie9"> <![endif]-->
<!--[if gt IE 9]><!-->
<html>
<!--<![endif]-->
<head runat="server">
    <meta charset="utf-8">
    <title>GemSparX - Diamond Details</title>
    <meta name="description" content="The Largest Diamond Selection, Quality and Cost at its best.">
    <meta name="author" content="GEMSPARX.COM">
    <!-- Mobile Metas -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <!-- Google Fonts  -->
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700,800' rel='stylesheet' type='text/css'>
    <link href='http://fonts.googleapis.com/css?family=Oswald:400,300,700' rel='stylesheet' type='text/css'>

    <!-- Library CSS -->
    <!--#include virtual="/include/styles.inc"-->
    <!-- The HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
      <script src="//html5shim.googlecode.com/svn/trunk/html5.js"></script>
      <script src="js/respond.min.js"></script>
      <![endif]-->
    <!--[if IE]>
      <link rel="stylesheet" href="css/ie.css">
      <![endif]-->
    <script type="text/javascript" src="Scripts/jquery.js"></script>



    <script type="text/javascript" src="Scripts/jquery.jcarousel.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.pikachoose.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.touchwipe.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.gdocsviewer.min.js"></script>
    <link type="text/css" href="Styles/css3.css" rel="stylesheet" />
    <script type="text/javascript">
        function GetCertLink() {
            var certlink = document.getElementById('certlink').value;

            window.open(certlink);
        }
        $(document).ready(
            function () {

                $("#imgdiamonds").PikaChoose();
            });
        $(document).ready(function () {

            $('a.embed').gdocsViewer({ width: 500, height: 300 });
        });
    </script>


</head>
<body>
    <%-- <div class="page-mask">
        <div class="page-loader">

            <div class="spinner"></div>
            Loading...
        </div>

    </div> --%>
    <div class="wrap">
        <!-- Header Start -->
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
        <!-- Header End -->
        <!-- Content Start -->
        <div id="main">
            <form id="form1" runat="server">
                <!-- Title, Breadcrumb Start-->
                <div class="breadcrumb-wrapper">
                    <div class="container">
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-xs-12 col-sm-6">
                                <h2 class="title">Diamond Details</h2>
                            </div>
                            <div class="col-lg-6 col-md-6 col-xs-12 col-sm-6">
                                <div class="breadcrumbs pull-right">
                                    <ul>
                                        <li>You are here:</li>
                                        <li><a href="/">Home</a></li>
                                        <li><a href="/SearchMain.aspx">Search</a></li>
                                        <li>Details</li>
                                    </ul>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <!-- Title, Breadcrumb End-->
                <!-- Main Content start-->
                <div class="content">
                    <div class="container">
                        <div class="row">
                            <!-- Row 1 start-->
                            <div class="sidebar col-lg-4 col-md-6 col-sm-6 col-xs-12">


                                <div class="img-content">
                                    <img src="" id="image0" runat="server" visible="False" />
                                </div>

                                <div class="img-content">


                                    <ul id="imgdiamonds">

                                        <li>
                                            <img src="Images/princess_front.jpg" id="image1" runat="server" /></li>
                                        <li>
                                            <img src="Images/princess_top.jpg" id="image2" runat="server" /></li>
                                    </ul>

                                </div>
                            </div>
                            <!-- Sidebar End -->
                            <div class="posts-block col-lg-7 col-md-6 col-sm-6 col-xs-12">
                                <article class="post hentry">

                                    <h1 id="heading" runat="server" style="color: #006699; font-family: 'Book Antiqua'">0.18 Carat Round Shaped Diamond</h1>
                                    <p style="font-family: Book Antiqua; font-size: large">
                                        This <span style="color: #006699; font-family: Book Antiqua; text-decoration: underline" id="spcut" runat="server">SUPER IDEAL</span> cut diamond has <span style="color: #006699; font-family: Book Antiqua; text-decoration: underline" id="spclarity" runat="server">I1</span>
                                        clarity with <span style="color: #006699; font-family: Book Antiqua; text-decoration: underline" id="spcarats" runat="server">0.18</span> carats which is <span style="color: #006699; font-family: Book Antiqua; text-decoration: underline" id="spcolor" runat="server">J</span> colored
                 <span style="color: #006699; font-family: Book Antiqua; text-decoration: underline" id="spshape" runat="server">ROUND</span> shaped diamond is accompanied with a GIA certificate. Please find the GIA certificate below.
                                    </p>
                                    <br />
                                    <asp:Label runat="server" ID="lblDescription"></asp:Label>
                                    <br />
                                    <input type="hidden" runat="server" id="certlink" />

                                    <asp:ImageButton runat="server" ID="btnView" AlternateText="View" ImageUrl="~/Images/button.png" Width="150px" OnClientClick="GetCertLink()" />
                                    <div data-embed_type="product" data-shop="gemsparx.myshopify.com" data-product_name="<%Response.Write(Request.QueryString["StoneId"]); %>" data-product_handle="<%Response.Write((Request.QueryString["StoneId"]).ToLower()); %>" data-has_image="false" data-display_size="regular" data-redirect_to="cart" data-buy_button_text="Add to cart" data-button_background_color="5f83b3" data-button_text_color="ffffff" data-height="400px" data-background_color="e7f087"></div>
                                    <script type="text/javascript">
                                        document.getElementById('ShopifyEmbedScript') || document.write('<script type="text/javascript" src="https://widgets.shopifyapps.com/assets/widgets/embed/client.js" id="ShopifyEmbedScript"><\/script>');
                                    </script>
                                    <noscript><a href="https://gemsparx.myshopify.com/cart/7349729477:1" target="_blank">Buy  <%Response.Write(Request.QueryString["StoneId"]); %></a></noscript>
                                    <br />
                                    <br />

                                    <table style="width: 435px; border-spacing: 8px; table-layout: auto; border-collapse: separate;">
                                        <thead>
                                            <tr>
                                                <th class="auto-style3" colspan="5">Specifications</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td style="background-color: #FFFFCC">Measurements:
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:Label runat="server" ID="lbldepth"></asp:Label>
                                                </td>
                                                <td class="auto-style5">&nbsp;&nbsp;</td>
                                                <td class="auto-style2" style="background-color: #FFFFCC">Stone No:</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblstone"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFCC">L/W Ratio:
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:Label runat="server" ID="lbllw"></asp:Label>
                                                </td>
                                                <td class="auto-style5">&nbsp;</td>
                                                <td class="auto-style2" style="background-color: #FFFFCC">Lab:</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lbllab"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFCC">Depth %:
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:Label runat="server" ID="lbldepthper"></asp:Label>
                                                </td>
                                                <td class="auto-style5">&nbsp;</td>
                                                <td class="auto-style2" style="background-color: #FFFFCC">Inscription</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblinscription"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFCC">Table %:
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:Label runat="server" ID="lbltable"></asp:Label>
                                                </td>
                                                <td class="auto-style5">&nbsp;</td>
                                                <td class="auto-style2" style="background-color: #FFFFCC">Shape</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblshape"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFCC">Culet:
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:Label runat="server" ID="lblculet"></asp:Label>
                                                </td>
                                                <td class="auto-style5">&nbsp;</td>
                                                <td class="auto-style2" style="background-color: #FFFFCC">Color</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblcolor"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFCC">Girdle:
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:Label runat="server" ID="lblgirdle"></asp:Label>
                                                </td>
                                                <td class="auto-style5">&nbsp;</td>
                                                <td class="auto-style2" style="background-color: #FFFFCC">Clarity</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblclarity"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFCC">&nbsp;Fluresence:
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:Label runat="server" ID="lblflu"></asp:Label>
                                                </td>
                                                <td class="auto-style5">&nbsp;</td>
                                                <td class="auto-style2" style="background-color: #FFFFCC">Cut</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblcut"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFCC">Polish:
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:Label runat="server" ID="lblpolish"></asp:Label>
                                                </td>
                                                <td class="auto-style5">&nbsp;</td>
                                                <td class="auto-style2" style="background-color: #FFFFCC">Carat Weight:</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblcaratwgt"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFCC">Symmetry:
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:Label runat="server" ID="lblsymmetry"></asp:Label>
                                                </td>
                                                <td class="auto-style5">&nbsp;</td>
                                                <td class="auto-style2" style="background-color: #FFFFCC">Price per crt:
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblprcpercrt"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFCC">Features:
                                                </td>
                                                <td colspan="4">
                                                    <asp:Label runat="server" ID="lblfeature"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td class="auto-style4">&nbsp;</td>
                                                <td class="auto-style5">&nbsp;</td>
                                                <td class="auto-style2">&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><strong>Total Amount:
                                                </strong>
                                                </td>
                                                <td class="auto-style5">&nbsp;</td>
                                                <td class="auto-style1" colspan="2">
                                                    <asp:Label runat="server" ID="lblamt" Font-Bold="True" Font-Size="Large"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>


                                </article>
                            </div>
                            <!-- Left Section End -->





                        </div>
                        <!-- Row 1 end-->

                        <div class="row">
                            <!-- Row 2 start-->

                            <div class="col-lg-2 col-md-12 col-sm-12 col-xs-12">
                            </div>

                            <div class="col-lg-10 col-md-12 col-sm-12 col-xs-12">
                                <div class="portfolio-item-title">
                                    <asp:Label ID="lblVTag" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></div>
                                <iframe id="frameVideo" width="800px" visible="false" height="550px" frameborder="0" scrolling="no" runat="server"></iframe>
                            </div>
                        </div>
                        <!-- Row 2 end-->
                    </div>
                    <!-- Main Conatiner  end-->
                </div>
                <!-- Main Content end-->

            </form>
        </div>
        <!-- Content End -->
        <!-- Footer Start -->
        <!--#include virtual="/include/footermin.inc"-->
        <!-- Scroll To Top -->
        <a href="#" class="scrollup"><i class="fa fa-angle-up"></i></a>
    </div>
    <!-- Wrap End -->
    <!--#include virtual="/include/aspxscripts.inc"
      -->
</body>
</html>
