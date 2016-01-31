using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data;
using Extentions;

public partial class Default : Page
{
    private List<Order> Orders
    {
        get
        {
            List <Order> orders = new List<Order>();
            string key = "Orders";

            if (ViewState[key] != null)
                orders = (List<Order>)ViewState[key];

            return orders;
        }
        set { ViewState["Orders"] = value; }
    } 

    protected void Page_Init(object sender, EventArgs e)
    {
        IEnumerable<ProviderType> providerTypes = Enum.GetValues(typeof(ProviderType))
                                                       .Cast<ProviderType>();

        foreach (ProviderType pt in providerTypes)
        {
            ListSrcProviderTypes.Items.Add(
                new ListItem(pt.ToName<ProviderType>(), pt.ToString())
            );

            ListDstProviderTypes.Items.Add(
                new ListItem(pt.ToName<ProviderType>(), pt.ToString())
            );    
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindGrid();
    }

    private void BindGrid()
    {
        GridOrders.DataSource = Orders.Where(o => o.State != ObjectState.Deleted);
        GridOrders.DataBind();
    }

    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        ProviderType providerType;

        string value = ListDstProviderTypes.SelectedValue;
        Enum.TryParse(value, out providerType);

        IDataProvider<Order> dataProvider = ProviderFactory.Instance.GetProvider(providerType);
        dataProvider.Add(Orders.Where(o => o.State != ObjectState.None));

        string msg = "alert('Заказы дабавлены!')";
        ScriptManager.RegisterClientScriptBlock((sender as Control), GetType(), "alert", msg, true);
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        ProviderType providerType;

        string value = ListDstProviderTypes.SelectedValue;
        Enum.TryParse(value, out providerType);

        IDataProvider<Order> dataProvider = ProviderFactory.Instance.GetProvider(providerType);
        dataProvider.Save(Orders.Where(o => o.State != ObjectState.Deleted));

        string msg = "alert('Заказы сохранены!')";
        ScriptManager.RegisterClientScriptBlock((sender as Control), GetType(), "alert", msg, true);
    }

    protected void BtnShow_Click(object sender, EventArgs e)
    {
        ProviderType providerType;

        string value = ListSrcProviderTypes.SelectedValue;
        Enum.TryParse(value, out providerType);

        IDataProvider<Order> dataProvider = ProviderFactory.Instance.GetProvider(providerType);

        Orders = dataProvider.Read().ToList();
        BindGrid();
    }

    protected void GridOrders_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            e.Row.Attributes["ondblclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridOrders, "Edit$" + e.Row.RowIndex);
    }

    protected void GridOrders_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int ix, code;
        decimal amount = 0, price = 0;
        string descr = null;

        GridViewRow row;
        TextBox textBox;
        
        Order order;
        switch (e.CommandName)
        {
            case "CancelUpdate":

                GridOrders.EditIndex = -1;
                break;

            case "DeleteRow":
                
                code = Convert.ToInt32(e.CommandArgument);
                order = Orders.Find(o => o.Code == code);
                if (order != null)
                    order.State = ObjectState.Deleted;
                
                break;

            case "EditRow":
        
                ix = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                GridOrders.EditIndex = ix;

                break;

            case "InsertRow":

                row = GridOrders.FooterRow;

                textBox = (TextBox)row.FindControl("TbDescription");
                if (textBox != null)
                    descr = textBox.Text;

                textBox = (TextBox)row.FindControl("TbAmount");
                if (textBox != null)
                    decimal.TryParse(textBox.Text, out amount);

                textBox = (TextBox)row.FindControl("TbPrice");
                if (textBox != null)
                    decimal.TryParse(textBox.Text, out price);

                Orders.Add(new Order()
                {
                    Code = Orders.Max(o => o.Code) + 1,
                    Description = descr,
                    Amount = amount,
                    Price = price,
                    State = ObjectState.New
                });

                break;

            case "UpdateRow":

                ix = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;

                code = Convert.ToInt32(e.CommandArgument);
                
                order = Orders.Find(o => o.Code == code);
                if (order != null)
                {
                    row = GridOrders.Rows[ix];

                    textBox = (TextBox)row.FindControl("TbDescription");
                    if (textBox != null)
                        descr = textBox.Text;

                    textBox = (TextBox)row.FindControl("TbAmount");
                    if (textBox != null)
                        decimal.TryParse(textBox.Text, out amount);

                    textBox = (TextBox)row.FindControl("TbPrice");
                    if (textBox != null)
                        decimal.TryParse(textBox.Text, out price);

                    order.Description = descr;
                    order.Amount = amount;
                    order.Price = price;
                    order.State = ObjectState.Updated;
                }

                GridOrders.EditIndex = -1;

                break;
            
        }

        BindGrid();
    }

    protected void GridOrders_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        GridOrders.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    protected void GridOrders_OnSorting(object sender, GridViewSortEventArgs e)
    {
        IEnumerable<Order> items = Orders.Where(o => o.State != ObjectState.Deleted);

        String sortOn = e.SortExpression;
        
        
        if (e.SortDirection == SortDirection.Ascending)
        {
            
            
            

        //    items = items.AsEnumerable().OrderBy(lambda);
        }

        //GridOrders.DataSource = items;
        BindGrid();

    }
    protected void GridOrders_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}