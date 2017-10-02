<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestFileUpload.aspx.cs" Inherits="TestFileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <hr />
            <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="UploadFile" />
            <br />
            <asp:Label ID="lblMessage" ForeColor="Green" runat="server" />
        </div>
    </form>
</body>
</html>
