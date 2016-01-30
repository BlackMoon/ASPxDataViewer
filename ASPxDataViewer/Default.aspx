
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"/>

        <asp:DropDownList ID="DropDownList1" runat="server"/>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server"/>        
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <asp:dropdownlist runat="server"/>
    </div>
    </form>
</body>

</html>
