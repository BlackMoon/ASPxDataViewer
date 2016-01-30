using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Data;
using Extentions;

public partial class Default : System.Web.UI.Page
{
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
        
          
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        ProviderType providerType;

        string value = ListSrcProviderTypes.SelectedValue;
        Enum.TryParse(value, out providerType);

        IDataProvider<Order> dataProvider = ProviderFactory.Instance.GetProvider(providerType);

        GridOrders.DataSource = new List<Order>() { new Order() { Code = 1 }, new Order() { Code = 2 }, new Order() { Code = 3 } };
        GridOrders.DataBind();
    }

    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        ProviderType providerType;

        string value = ListDstProviderTypes.SelectedValue;
        Enum.TryParse(value, out providerType);

        IDataProvider<Order> dataProvider = ProviderFactory.Instance.GetProvider(providerType);
    }
}