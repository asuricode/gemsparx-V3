<%@ Page enableEventValidation="false" Language="C#" AutoEventWireup="true" CodeFile="SearchMain.aspx.cs" Inherits="SearchDia.SearchMain" %>


<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>

<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8" >
  <title>Search Diamonds</title>
    <%--the below three references are for slider--%>
    <link rel="stylesheet" href="Styles/jquery-ui.css">
  <script src="Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
  <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
  <%--<script src="//code.jquery.com/jquery-1.11.3.min.js"></script>--%>
  <link rel="stylesheet" href="/resources/demos/style.css">
    <link rel="Stylesheet" href="Styles/jquery.dataTables.min.css" />
    <script src="Scripts/jquery.dataTables.min.js" type="text/javascript"></script>   
    <script src="Scripts/dataTables.scroller.min.js" type="text/javascript"></script> 
    <link rel="stylesheet" href="Styles/jquery-ui-flick.css" />
<script src="Scripts/jquery-ui-slider-pips.js" type="text/javascript"></script>
<link rel="stylesheet" href="Styles/jquery-ui-slider-pips.css" /> 
  <style>      
.ui-slider-horizontal {
    height: 8px;
    width: 200px;
}

 #slider label {
  position: absolute;
  width: 20px;
  margin-top: 20px;
  margin-left: -10px;
  text-align: center;
  font-size:small;
}
 #slider-color label {
  position: absolute;
  width: 20px;
  margin-top: 20px;
  margin-left: -10px;
  text-align: center;
  font-size:small;
}

 #slider-clarity label {
  position: absolute;
  width: 20px;
  margin-top: 20px;
  margin-left: -10px;
  text-align: center;
  font-size:small;
}

.hiddencol {
    display: none;
      }
      #btn_submit {
-moz-border-radius: 15px;
-webkit-border-radius: 15px;
border-radius: 15px; }

      body {
          /*background:url(Images/bg.jpg);*/
          background-color: #006699;
      } 
      #main{
         background: white;
	    min-width: 780px;
	    max-width: 1180px;
	    margin: 10px auto;
      }
      .center{
         margin: 0 50px;
          width: 1000px;          
          padding: 10px;         
      }
      .fontfamily{
          font-family: 'Book Antiqua';
          font-size:medium;
          color: white;
      }
      .rowstylefontfamily{
          font-family: 'Book Antiqua';
          font-size: small;          
          color:darkslategray;
      }

  .ui-slider {
  background: #d5cebc;
  border: none;
  border-radius: 0; }
                    
  .ui-slider .ui-slider-handle {
    width: 12px;
    height: 12px;
    border-radius: 50% 50% 0;
    border-color: #006699;    
    transition: border 0.4s ease; }
                    
    .ui-slider .ui-slider-handle.ui-state-hover, 
    .ui-slider .ui-slider-handle.ui-state-focus, 
    .ui-slider .ui-slider-handle.ui-state-active {
      border-color: #172f38; }
                    
  .ui-slider .ui-slider-pip .ui-slider-line {
    background: #d5cebc;
    transition: all 0.4s ease; }
                    
  .ui-slider.ui-slider-horizontal {
    height: 6px; }
                    
   .ui-slider.ui-slider-horizontal .ui-slider-handle {
      -webkit-transform: rotateZ(45deg);
              transform: rotateZ(45deg);
      top: -16px;
      margin-left: -7px; }
                    
    .ui-slider.ui-slider-horizontal .ui-slider-pip {
      top: 10px; }
                    
     .ui-slider.ui-slider-horizontal .ui-slider-pip .ui-slider-line {
        width: 2px;
        height: 7px;
        margin-left: -1px; }                                                                                     
                    
 .ui-slider-range { background: #006699; }  

  </style>
  <script type="text/javascript">    
      var cutval = {
          0: "FAIR",
          1: "GOOD",
          2: "VERY GOOD",
          3: "IDEAL",
          4: "SUPER IDEAL"
      };

      var colorval = {
          0: "J",
          1: "I",
          2: "H",
          3: "G",
          4: "F",
          5: "E",
          6: "D",
          7: "K",
          8: "N",
          9: "M",
          10: "L"
      };

      var clarityrval = {
          0: "SI2",
          1: "SI1",
          2: "VS2",
          3: "VS1",
          4: "VVS2",
          5: "VVS1",
          6: "IF",
          7: "I1",
          8: "I2",
          9: "I3"
      };

      $(function () {
          var maxprice = $("#maxprice").val();
          var minprice = $("#minprice").val();
          $("#slider-range").slider({
              range: true,
              min: 199,
              max: 99500,
              values: [minprice, maxprice],
              slide: function (event, ui) {
                  $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
                  $("#minprice").val(ui.values[0]);
                  $("#maxprice").val(ui.values[1]);                
              },
              change: function (event, ui) {
                  $("#minprice").val(ui.values[0]);
                  $("#maxprice").val(ui.values[1]);  
                  __doPostBack('<%=btn_slider_event.ClientID %>', 'OnClick');
              }
          });
          $("#amount").val("$" + $("#slider-range").slider("values", 0) +
      " - $" + $("#slider-range").slider("values", 1));
          $("#minprice").val($("#slider-range").slider("values", 0));
          $("#maxprice").val($("#slider-range").slider("values", 1));
      });

      $(function () {
          var maxcarat = $("#maxcarat").val();
          var mincarat = $("#mincarat").val();
          $("#slider-carat").slider({
              range: true,
              min: 0.09,
              max: 5.62,
              step: 0.01,
              values: [mincarat, maxcarat],
              slide: function (event, ui) {
                  $("#carat").val(ui.values[0] + " - " + ui.values[1]);
                  $("#mincarat").val(ui.values[0]);
                  $("#maxcarat").val(ui.values[1]);
              },
              change: function (event, ui) {
                  $("#mincarat").val(ui.values[0]);
                  $("#maxcarat").val(ui.values[1]);
                  __doPostBack('<%=btn_slider_event.ClientID %>', 'OnClick');
              }
          }).slider("pips", {
              first: "pip",
              last: "pip"
          });
          $("#carat").val($("#slider-carat").slider("values", 0) +
      " - " + $("#slider-carat").slider("values", 1));
          $("#mincarat").val($("#slider-carat").slider("values", 0));
          $("#maxcarat").val($("#slider-carat").slider("values", 1));
      });

      $(function () {
          var mincut = $("#mincut").val();
          var maxcut = $("#maxcut").val();     
          $("#slider").slider({
              range: true,
              min: 0,
              max: 4,
              values: [mincut, maxcut],
              slide: function (event, ui) {
                  $("#cut").val(cutval[ui.values[0]] + " - " + cutval[ui.values[1]]);
                  $("#mincut").val(ui.values[0]);
                  $("#maxcut").val(ui.values[1]);
              },
              change: function (event, ui) {
                  $("#mincut").val(ui.values[0]);
                  $("#maxcut").val(ui.values[1]);
                  __doPostBack('<%=btn_slider_event.ClientID %>', 'OnClick');
              }
          })
.each(function () {
    // Add labels to slider whose values 
    // are specified by min, max
    // Get the options for this slider (specified above)
    var opt = $(this).data().uiSlider.options;
    // Get the number of possible values
    var vals = opt.max - opt.min;
    // Position the labels
    for (var i = 0; i <= vals; i++) {
        // Create a new element and position it with percentages
        var el = $('<label>' + cutval[i + opt.min] + '</label>').css('left', (i / vals * 100) + '%');
        // Add the element inside #slider
        $("#slider").append(el);
    }

}).slider("pips", {
    first: "pip",
    last: "pip"
});
          $("#cut").val(cutval[$("#slider").slider("values", 0)] +
               " - " + cutval[$("#slider").slider("values", 1)]);
          $("#mincut").val($("#slider").slider("values", 0));
          $("#maxcut").val($("#slider").slider("values", 1));
      });
      //for slider color
      $(function () {
          var mincolor = $("#mincolor").val();
          var maxcolor = $("#maxcolor").val(); 
          $("#slider-color").slider({              
              range: true,
              min: 0,
              max: 10,
              values: [mincolor, maxcolor],
              slide: function (event, ui) {
                  $("#color").val(colorval[ui.values[0]] + " - " + colorval[ui.values[1]]);
                  $("#mincolor").val(ui.values[0]);
                  $("#maxcolor").val(ui.values[1]);
              },
              change: function (event, ui) {
                  $("#mincolor").val(ui.values[0]);
                  $("#maxcolor").val(ui.values[1]);
                  __doPostBack('<%=btn_slider_event.ClientID %>', 'OnClick');
              }
          }).each(function () {

    // Add labels to slider whose values 
    // are specified by min, max
    // Get the options for this slider (specified above)
    var opt = $(this).data().uiSlider.options;
    // Get the number of possible values
    var vals = opt.max - opt.min;
    // Position the labels
    for (var i = 0; i <= vals; i++) {
        // Create a new element and position it with percentages
        var el = $('<label>' + colorval[i + opt.min] + '</label>').css('left', (i / vals * 100) + '%');
        // Add the element inside #slider
        $("#slider-color").append(el);

    }

          }).slider("pips", {
              first: "pip",
              last: "pip"
          });
          $("#color").val(colorval[$("#slider-color").slider("values", 0)] +
               " - " + colorval[$("#slider-color").slider("values", 1)]);
          $("#mincolor").val($("#slider-color").slider("values", 0));
          $("#maxcolor").val($("#slider-color").slider("values", 1));
      });
      //for slider clarity
      $(function () {
          var minclarity = $("#minclarity").val();
          var maxclarity = $("#maxclarity").val(); 
          $("#slider-clarity").slider({
              range: true,
              min: 0,
              max: 9,
              values: [minclarity, maxclarity],
              slide: function (event, ui) {
                  $("#clarity").val(clarityrval[ui.values[0]] + " - " + clarityrval[ui.values[1]]);
                  $("#minclarity").val(ui.values[0]);
                  $("#maxclarity").val(ui.values[1]);
              },
              change: function (event, ui) {
                  $("#minclarity").val(ui.values[0]);
                  $("#maxclarity").val(ui.values[1]);
                  __doPostBack('<%=btn_slider_event.ClientID %>', 'OnClick');
              }
          })
.each(function () {

    // Add labels to slider whose values 
    // are specified by min, max

    // Get the options for this slider (specified above)
    var opt = $(this).data().uiSlider.options;

    // Get the number of possible values
    var vals = opt.max - opt.min;

    // Position the labels
    for (var i = 0; i <= vals; i++) {

        // Create a new element and position it with percentages
        var el = $('<label>' + clarityrval[i + opt.min] + '</label>').css('left', (i / vals * 100) + '%');

        // Add the element inside #slider
        $("#slider-clarity").append(el);

    }

}).slider("pips", {
    first: "pip",
    last: "pip"
});
          $("#clarity").val(clarityrval[$("#slider-clarity").slider("values", 0)] +
               " - " + clarityrval[$("#slider-clarity").slider("values", 1)]);
          $("#minclarity").val($("#slider-clarity").slider("values", 0));
          $("#maxclarity").val($("#slider-clarity").slider("values", 1));
      });
  </script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $("#gvResults").prepend($("<thead></thead>").append($("#gvResults").find("tr:first"))).DataTable({ bFilter: false, bLengthChange: false, "iDisplayLength": 25});
        });
        function GetCertificateUrl(certificate, lab)
        {
            var url = "";
            switch (lab) {
                case "GIA":
                    url = "http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + certificate;
                    break;
                case "IGI":
                    url = "http://www.igiworldwide.com/verify.php?r=" + certificate;
                    break;
                case "HRD":
                    url = "https://my.hrdantwerp.com/"
                    break;                                                   
            }
            return url;
        }
        function ViewDetails(stoneid)
        {
            var url = "ViewDiamond.aspx?StoneId=" + stoneid 
            return url
        }       
        </script>
</head>
<body>
<form runat="server" id="searchform">
<input type="hidden" runat="server" id="minprice" value="250"/>
<input type="hidden" runat="server" id="maxprice" value="10000"/>
<input type="hidden" runat="server" id="mincolor" value="0"/>
<input type="hidden" runat="server" id="maxcolor" value="5"/>
<input type="hidden" runat="server" id="mincut" value="2"/>
<input type="hidden" runat="server" id="maxcut" value="4"/>
<input type="hidden" runat="server" id="minclarity" value="0"/>
<input type="hidden" runat="server" id="maxclarity" value="6"/>
<input type="hidden" runat="server" id="mincarat" value="0.45" />
<input type="hidden" runat="server" id="maxcarat" value="3.75"/>
    <div id="main">
        <div class="center">
 <div style="width:100%;">
     <h1 style="font-family: 'Batang'; font-style:normal; color: steelblue">SEARCH DIAMONDS</h1>
     <div style="padding-bottom:10px;padding-top:10px;text-align:center">       
         <asp:CheckBox ID="Princess" runat="server" AutoPostBack="true" OnCheckedChanged="Server_Changed" Text = ""  Checked="true"/><img src="Images/princess.jpg" alt="Princess" style="width:70px"/>
         <asp:CheckBox ID="RBC" runat="server" AutoPostBack="true" OnCheckedChanged="Server_Changed" Text = ""  /><img src="Images/round1.jpg" alt="Round" style="width:75px" />   
         <asp:CheckBox ID="Cushion" runat="server" AutoPostBack="true" OnCheckedChanged="Server_Changed" Text = ""  /><img src="Images/cushion.jpg" alt="Cushion" style="width:70px"/>            
         <asp:CheckBox ID="Marquise" runat="server" AutoPostBack="true" OnCheckedChanged="Server_Changed" Text = ""  /><img src="Images/marquise.jpg" alt="Marquise" style="width:45px"/>   
         <asp:CheckBox ID="Pear" runat="server" AutoPostBack="true" OnCheckedChanged="Server_Changed" Text = ""  /><img src="Images/pear.jpg" alt="Pear" style="width:48px"/>        
         <asp:CheckBox ID="Heart" runat="server" AutoPostBack="true" OnCheckedChanged="Server_Changed" Text = ""  /><img src="Images/heart.jpg" alt="Heart" style="width:70px"/>  
         <asp:CheckBox ID="Oval" runat="server" AutoPostBack="true" OnCheckedChanged="Server_Changed" Text = ""  /><img src="Images/oval.jpg" alt="Oval" style="width:60px"/>    
         <asp:CheckBox ID="Emerald" runat="server" AutoPostBack="true" OnCheckedChanged="Server_Changed" Text = ""  /><img src="Images/emerald.jpg" alt="Emerald" style="width:55px"/>                                               
         <asp:CheckBox ID="Radiant" runat="server" AutoPostBack="true" OnCheckedChanged="Server_Changed" Text = ""  /><img src="Images/radiant.jpg" alt="Radiant" style="width:60px"/>                         
     </div>
 <table width="100%" align="center">
     <tr>
         <td>
             <p>
                  <label for="amount" style="color: darkslategrey">PRICE</label>
                  <input type="text" id="amount" readonly style="border:0; color:#f6931f; font-weight:bold"/>
            </p>
            <div id="slider-range" style=" width:70%; text-align: center"></div>
         </td>
         <td style="width:50%; align-content:center">
         <br />
             <p>
                <label for="carat" style="color: darkslategrey">CARAT</label>
                <input type="text" id="carat" readonly style="border:0; color:#f6931f; font-weight:bold"/>
            </p>
            <div id="slider-carat" style=" width:70%;"></div>
         </td>
     </tr>
     
     <tr>
         <td style="width:50%; align-content:center">
            <br />
              <p>
                <label for="cut" style="color: darkslategrey">CUT</label>
                <input type="text" id="cut" readonly style="border:0; color:#f6931f; font-weight:bold; width:50%"/>
            </p>
            <div id="slider" style=" width:70%;"></div>
         </td>
         <td style="width:50%; align-content:center">
             <br />
             <p>
                <label for="color" style="color: darkslategrey">COLOR</label>
                <input type="text" id="color" readonly style="border:0; color:#f6931f; font-weight:bold; width:50%"/>
            </p>
            <div id="slider-color" style=" width:70%;"></div>
         </td>
     </tr>
    <%-- <tr>
         <td><br /></td>
     </tr>
    --%>      
     <tr>
         <td style="width:50%; align-content:center">
<br /><br />
             <p>
                <label for="clarity" style="color: darkslategrey">CLARITY</label>
                <input type="text" id="clarity" readonly style="border:0; color:#f6931f; font-weight:bold; width:50%"/>
            </p>
            <div id="slider-clarity" style=" width:70%;"></div>
         </td>
         <td style="width:60px; padding-top:6%">             
         <p style="color: #757575; font-size: larger; font-family: 'Book Antiqua'">There are <span style="color:white; background-color:#006699" id="count" runat="server">N diamonds</span> matching your search critirea.</p>           
         </td>         
     </tr>
     <tr>
         <td><br /></td>
     </tr>
  
 </table>
</div>
            <div style="text-align:center">
                     <asp:Button runat="server" ID="btn_submit" OnClick="btn_submit_Click" Text="SEARCH" BackColor="#006699" BorderStyle="None" Width="100px" Height="35px" CssClass="fontfamily"/>
                     <asp:Button runat="server" ID="btn_slider_event" OnClick="btn_slider_event_click" Text="" Visible="false"/>
            </div>
    <br />
    <div style="width:106%; align-content:center; vertical-align: middle">
        <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="false" CssClass="gvr"
            HeaderStyle-BackColor="#006699" GridLines="None" RowStyle-HorizontalAlign="Center" 
            HeaderStyle-HorizontalAlign="Center" AlternatingRowStyle-BackColor="white" RowStyle-BackColor="WhiteSmoke" 
            HeaderStyle-CssClass="fontfamily" RowStyle-CssClass="rowstylefontfamily" Width="100%">
            <Columns>
                <asp:BoundField DataField="StoneID" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" />
                <asp:BoundField DataField="Shape" HeaderText="Shape" ItemStyle-Width="20px"/>
                <asp:BoundField DataField="Carats" HeaderText="Weight" ItemStyle-Width="10px"/>
                <asp:BoundField DataField="Color" HeaderText="Color" ItemStyle-Width="20px"/>
                <asp:BoundField DataField="Clarity" HeaderText="Clarity" ItemStyle-Width="20px"/>
                <asp:BoundField DataField="Cut" HeaderText="Cut" ItemStyle-Width="20px"/>
                <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="20px"/>
                <asp:BoundField DataField="Country" HeaderText="Country" ItemStyle-Width="20px"/>                                                
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Lab">                   
                     <ItemTemplate>
                        <a href="#" onclick ="window.open(GetCertificateUrl(<%# Eval("Certificate") %>, '<%# Eval("Lab") %>'), '_blank')"><%# Eval("Lab") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Polish" HeaderText="Polish" ItemStyle-Width="20px"/>
                <asp:BoundField DataField="Symm" HeaderText="Symmetry" ItemStyle-Width="20px"/>
                <asp:BoundField DataField="Depth" HeaderText="Depth" ItemStyle-Width="20px"/>
                <asp:BoundField DataField="Table" HeaderText="Table" ItemStyle-Width="20px"/>                
                <asp:TemplateField ItemStyle-Width="7%">
                    <ItemTemplate>                                                     
                        <asp:ImageButton runat="server" ID="btnView" AlternateText="View" ImageUrl="~/Images/ast-view-button.png" Width="35px" onclick="view_click"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />      
            </div>
        </div>
</form>
</body>
</html>
