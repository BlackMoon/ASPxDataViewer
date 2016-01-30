<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>

<script runat="server">
    
    void Application_Error(object sender, EventArgs e) 
    { 
        Exception ex = Server.GetLastError().GetBaseException();

        Server.ClearError();
        
        HttpContext.Current.Response.Write("<script language='javascript'>alert('" + ex.Message + "');</" + "script>");
    }
       
</script>
