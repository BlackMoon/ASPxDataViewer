<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Обработка заказов</title>
    <style type="text/css">
        body {
            font: 12px Arial,Tahoma,Helvetica,FreeSans,sans-serif;
        }

        #divHeader, #divFooter {
            height: 30px;
        }

        #divValidation {
            height: 48px;
        }

        #UpdateProgress1 {
            width: 200px;
            background-color: #FFC080;
            position: absolute;
            bottom: 0;
            left: 0;
        }
    </style>

    <script src="Scripts/jquery-2.2.0.js"></script>
    <script>
        function pageLoad() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequest);
        }

        function onEndRequest(sender, args) {
            var err = args.get_error();
            if (err != undefined) {
                alert(err.message);
                args.set_errorHandled(true);
            }
        }

        var footerHeight, $grid;

        $(function () {
            $grid = $('#divGrid'),
                footerHeight = $('#divFooter').height() + $('#divValidation').height() + 20;    // 20 = absolute bottom from divFooter

            window.onresize.call(this);
        });

        window.onresize = function (e) {
            var h = $(window).height();
            $grid.height(h - $grid.offset().top - footerHeight - 10);       // 8 = (body margin)
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" OnAsyncPostBackError="ScriptManager1_OnAsyncPostBackError" />

        <div id="divHeader">
            <asp:Label ID="Label1" runat="server" Text="Источник:" />
            <asp:DropDownList ID="ListSrcProviderTypes" runat="server" />
            <asp:Button ID="BtnShow" runat="server" Text="Показать" Width="100px" OnClick="BtnShow_Click" ToolTip="Загрузка данных" />
        </div>

        <div id="divFixedHead"></div>

        <div id="divGrid" style="border: 1px solid #A55129; overflow: auto;">
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GridOrders" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" ShowFooter="True"
                        BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="Code"
                        OnRowCommand="GridOrders_OnRowCommand" OnRowDataBound="GridOrders_OnRowDataBound" OnRowEditing="GridOrders_OnRowEditing" OnSorting="GridOrders_OnSorting">

                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle Width="120" />
                                <ItemStyle Width="120" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEdit" CommandArgument='<%# Eval("Code") %>' CommandName="EditRow" ForeColor="#8C4510" runat="server">Изменить</asp:LinkButton>
                                    <asp:LinkButton ID="lbDelete" CommandArgument='<%# Eval("Code") %>' CommandName="DeleteRow" ForeColor="#8C4510" runat="server"
                                        CausesValidation="false" OnClientClick="return confirm('Удалить заказ?')">Удалить</asp:LinkButton>
                                </ItemTemplate>

                                <EditItemTemplate>
                                    <asp:LinkButton ID="lbUpdate" CommandArgument='<%# Eval("Code") %>' CommandName="UpdateRow" ForeColor="#8C4510" runat="server">Принять</asp:LinkButton>
                                    <asp:LinkButton ID="lbCancel" CommandArgument='<%# Eval("Code") %>' CommandName="CancelUpdate" ForeColor="#8C4510" runat="server" CausesValidation="false">Отмена</asp:LinkButton>
                                </EditItemTemplate>

                                <FooterTemplate>
                                    <asp:LinkButton ID="lbInsert" ValidationGroup="InsertRow" runat="server" CommandName="InsertRow">Создать</asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="№">
                                <HeaderStyle Width="100" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Код" InsertVisible="False" SortExpression="Code">
                                <HeaderStyle Width="120" />
                                <ItemStyle Width="120" HorizontalAlign="Center" />

                                <EditItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Code") %>' />
                                </EditItemTemplate>

                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Code") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Описание" SortExpression="Description">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TbDescription" runat="server" Text='<%# Bind("Description") %>' Width="100%" />
                                </EditItemTemplate>

                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("Description") %>' />
                                </ItemTemplate>

                                <FooterTemplate>
                                    <asp:TextBox ID="TbDescription" runat="server" Width="100%" />
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Количество" SortExpression="Amount">
                                <HeaderStyle Width="160" />
                                <ItemStyle Width="160" />

                                <EditItemTemplate>
                                    <asp:TextBox ID="TbAmount" runat="server" Text='<%# Bind("Amount") %>' Width="140" />

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TbAmount" Text="*" ForeColor="Red"
                                        ErrorMessage="Не заполнено {Количество}" />

                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TbAmount" ForeColor="Red" Text="!" Font-Bold="True"
                                        ErrorMessage="Допускаются ввод цифр c 2 знаками после запятой" ValidationExpression="^\d+(\,\d{1,2})?$" />
                                </EditItemTemplate>

                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("Amount") %>' />
                                </ItemTemplate>

                                <FooterTemplate>
                                    <asp:TextBox ID="TbAmount" runat="server" Width="140" />

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TbAmount" Text="*" ForeColor="Red"
                                        ErrorMessage="Не заполнено {Количество}" ValidationGroup="InsertRow" />

                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TbAmount" ForeColor="Red" Text="!" Font-Bold="True"
                                        ErrorMessage="Допускаются ввод цифр c 2 знаками после запятой" ValidationExpression="^\d+(\,\d{1,2})?$" ValidationGroup="InsertRow" />
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Цена" SortExpression="Price">
                                <HeaderStyle Width="160" />
                                <ItemStyle Width="160" />

                                <EditItemTemplate>
                                    <asp:TextBox ID="TbPrice" runat="server" Text='<%# Bind("Price") %>' Width="140" />

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TbPrice" Text="*" ForeColor="Red"
                                        ErrorMessage="Не заполнена {Цена}" />

                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="TbPrice" ForeColor="Red" Text="!" Font-Bold="True"
                                        ErrorMessage="Допускаются ввод цифр c 2 знаками после запятой" ValidationExpression="^\d+(\,\d{1,2})?$" />

                                </EditItemTemplate>

                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("Price") %>' />
                                </ItemTemplate>

                                <FooterTemplate>
                                    <asp:TextBox ID="TbPrice" runat="server" Width="140" />

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                        ControlToValidate="TbPrice" Text="*" ForeColor="Red" ErrorMessage="Не заполнена {Цена}" ValidationGroup="InsertRow" />

                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="TbPrice" ForeColor="Red" Text="!" Font-Bold="True"
                                        ErrorMessage="Допускаются ввод цифр c 2 знаками после запятой" ValidationExpression="^\d+(\,\d{1,2})?$" ValidationGroup="InsertRow" />

                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#FFF1D4" />
                        <SortedAscendingHeaderStyle BackColor="#B95C30" />
                        <SortedDescendingCellStyle BackColor="#F1E5CE" />
                        <SortedDescendingHeaderStyle BackColor="#93451F" />
                    </asp:GridView>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnShow" EventName="Click" />
                </Triggers>

            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    Обработка данных...
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>

        <div id="divValidation">
            <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="InsertRow" ForeColor="Red" runat="server" />
            <asp:ValidationSummary ID="ValidationSummary2" ForeColor="Red" runat="server" />
        </div>

        <div id="divFooter" style="position: absolute; bottom: 20px">
            <asp:Label ID="Label2" runat="server" Text="Получатель:" />
            <asp:DropDownList ID="ListDstProviderTypes" runat="server" />

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Button ID="BtnAdd" runat="server" Text="Добавить" Width="100px" OnClick="BtnAdd_Click" ToolTip="Обновление данных" />
                    <asp:Button ID="BtnSave" runat="server" Text="Сохранить" Width="100px" OnClick="BtnSave_Click" ToolTip="Сохранение с перезаписью данных" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </form>
</body>

</html>
