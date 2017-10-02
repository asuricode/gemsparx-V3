<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewDiamond.aspx.cs" Inherits="SearchDia.ViewDiamond" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>View Diamond Details</title>

          <!-- Google Fonts  -->
      <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700,800' rel='stylesheet' type='text/css'>
      <link href='http://fonts.googleapis.com/css?family=Oswald:400,300,700' rel='stylesheet' type='text/css'>
      <!-- Library CSS -->
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
        function GetCertLink()
        {
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
    <style type="text/css">
       

        #main {
           
	        min-width: 780px;
	        max-width: 1180px;
	        margin: 10px auto;
            height: 1120px;
        }

        .center {
            margin: 0 50px;
            width: 1000px;
            padding: 10px;
        }

        strong {
            font-weight: bold;
        }

        em {
            font-style: italic;
        }

        table {
           
            border-collapse: separate;
            box-shadow: inset 0 1px 0 #fff;
            font-size: 12px;
            line-height: 10px;
            margin: 30px auto;
            text-align: left;
            width: 300px;
        }

        th {
            /*background: url(http://jackrugile.com/images/misc/noise-diagonal.png), linear-gradient(#777, #444);*/
            background-color: #006699;
            border-left: 0px solid #555;
            border-right: 0px solid #777;
            border-top: 0px solid #555;
            border-bottom: 0px solid #333;
            box-shadow: inset 0 1px 0 #999;
            color: whitesmoke;
            font-size: medium;
            font-weight: normal;
            font-family: 'Book Antiqua';
            text-align:left;
            padding: 10px 15px;
            position: relative;
            text-shadow: 0 1px 0 #000;
        }

            th:after {
                background: linear-gradient(rgba(255,255,255,0), rgba(255,255,255,.08));
                content: '';
                display: block;
                height: 25%;
                left: 0;
                margin: 1px 0 0 0;
                position: absolute;
                top: 25%;
                width: 100%;
            }

            th:first-child {
                border-left: 1px solid #777;
                box-shadow: inset 1px 1px 0 #999;
            }

            th:last-child {
                box-shadow: inset -1px 1px 0 #999;
            }

        td {
            border-right: 1px solid #fff;
            border-left: 1px solid #e8e8e8;
            border-top: 1px solid #fff;
            border-bottom: 1px solid #e8e8e8;
            padding: 10px 15px;
            position: relative;
            transition: all 300ms;
            font-family:Calibri;
            font-size:medium;
            word-wrap:break-word;
            line-break:loose;
        }

            td:first-child {
                box-shadow: inset 1px 0 0 #fff;
            }

            td:last-child {
                border-right: 1px solid #e8e8e8;
                box-shadow: inset -1px 0 0 #fff;
            }

       
    </style>
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
        <div id="main">
            <div class="center">
                <div style="width: 40%; float: left; padding-top: 2.4%;">
                    <ul id="imgdiamonds">
                        <li>
                            <img src="Images/princess_front.jpg" id="image1" runat="server" /></li>
                        <li>
                            <img src="Images/princess_top.jpg" id="image2" runat="server" /></li>
                    </ul>                                           
                </div>
                <div style="width: 500px; float: left; padding-left: 4%">
                    <h1 id="heading" runat="server" style="color: #006699; font-family: 'Book Antiqua'">0.18 Carat Round Shaped Diamond</h1>
                    <p style="font-family:Book Antiqua; font-size: large">
                        This <span style="color: #006699;font-family:Book Antiqua; text-decoration:underline" id="spcut" runat="server">SUPER IDEAL</span> cut diamond has <span style="color: #006699;font-family:Book Antiqua;text-decoration:underline" id="spclarity" runat="server">I1</span>
                        clarity with <span style="color: #006699;font-family:Book Antiqua;text-decoration:underline" id="spcarats" runat="server">0.18</span> carats which is <span style="color: #006699;font-family:Book Antiqua;text-decoration:underline" id="spcolor" runat="server">J</span> colored
                 <span style="color:#006699;font-family:Book Antiqua;text-decoration:underline" id="spshape" runat="server">ROUND</span> shaped diamond is accompanied with a GIA certificate. Please find the GIA certificate below.
                    </p>   
                    <br />
                     <asp:Label runat="server" ID="lblDescription"></asp:Label>
                      <br />
                    <input type="hidden" runat="server" id="certlink"/>
                    
                    <asp:ImageButton runat="server" ID="btnView" AlternateText="View" ImageUrl="~/Images/button.png" Width="150px" OnClientClick="GetCertLink()" />  
                   
                </div>        
              <div style="width: 500px; float: left; padding-left: 4%">
                 
 <div data-embed_type="product" data-shop="gemsparx.myshopify.com" data-product_name="<%Response.Write(Request.QueryString["StoneId"] ); %>" data-product_handle="<%Response.Write((Request.QueryString["StoneId"]).ToLower()); %>" data-has_image="false" data-display_size="regular" data-redirect_to="cart" data-buy_button_text="Add to cart" data-button_background_color="5f83b3" data-button_text_color="ffffff" data-background_color="e7f087"></div>
<script type="text/javascript">
    document.getElementById('ShopifyEmbedScript') || document.write('<script type="text/javascript" src="https://widgets.shopifyapps.com/assets/widgets/embed/client.js" id="ShopifyEmbedScript"><\/script>');
</script>
<noscript><a href="https://gemsparx.myshopify.com/cart/7349729477:1" target="_blank">Buy  <%Response.Write(Request.QueryString["StoneId"] ); %></a></noscript>

                </div>       
                <div style="width: 350px; height: 300px; float: left; padding-left:40px">
                     <h2 style="font-family:'Book Antiqua'">Diamond Details</h2>
                    <table>
                        <thead>
                            <tr>
                                <th>Specifications</th>
                            </tr>
                        </thead>
                        <tbody>                           
                            <tr>
                                <td>Depth:
                     <asp:Label runat="server" ID="lbldepth"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>L/W Ratio:
                     <asp:Label runat="server" ID="lbllw"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Depth %:
                     <asp:Label runat="server" ID="lbldepthper"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Table %:
                     <asp:Label runat="server" ID="lbltable"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Culet:
                     <asp:Label runat="server" ID="lblculet"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Girdle:
                     <asp:Label runat="server" ID="lblgirdle"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Carat Weight:
                     <asp:Label runat="server" ID="lblcaratwgt"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Polish:
                     <asp:Label runat="server" ID="lblpolish"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Symmetry:
                     <asp:Label runat="server" ID="lblsymmetry"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Fluresence:
                     <asp:Label runat="server" ID="lblflu"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Price per crt:
                     <asp:Label runat="server" ID="lblprcpercrt"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Total Amount:
                     <asp:Label runat="server" ID="lblamt"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>                                               
                <div style="width: 200px; height: 300px; float: left;padding-top:50px">                       
                    <table>
                        <thead>
                            <tr>
                                <th>Details</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Stone No:
                     <asp:Label runat="server" ID="lblstone"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Lab:
                     <asp:Label runat="server" ID="lbllab"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Inscription:
                     <asp:Label runat="server" ID="lblinscription"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Shape:
                     <asp:Label runat="server" ID="lblshape"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Color:
                     <asp:Label runat="server" ID="lblcolor"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Clarity:
                     <asp:Label runat="server" ID="lblclarity"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Cut:
                     <asp:Label runat="server" ID="lblcut"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Features:
                                    <br /><br />
                     <asp:Label runat="server" ID="lblfeature" ></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>                
            </div>
        </div>
    </form>
 <!--#include virtual="/include/footermin.inc"-->
</body>
</html>
