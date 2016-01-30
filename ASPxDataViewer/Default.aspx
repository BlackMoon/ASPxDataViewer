
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"/>

        <asp:DropDownList ID="ListSrcProviderTypes" runat="server"/>
        <asp:Button ID="BtnShow" runat="server" Text="Показать" Width="100px" OnClick="BtnShow_Click" />
        
        <div id="divGrid">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GridOrders" runat="server" Width="100%" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <asp:dropdownlist ID="ListDstProviderTypes" runat="server"/>
        <asp:Button ID="BtnAdd" runat="server" Text="Добавить" Width="100px" OnClick="BtnAdd_Click"/>
        <asp:Button ID="BtnSave" runat="server" Text="Сохранить" Width="100px"/>
    </div>
    </form>
</body>

</html>
