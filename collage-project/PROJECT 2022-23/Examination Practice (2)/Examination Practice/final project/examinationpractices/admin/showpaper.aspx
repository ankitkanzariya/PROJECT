    <%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="showpaper.aspx.cs" Inherits="admin_showpaper" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="container w3-animate-zoom">
    <div class="row">
        <div class="col-md-12">
        
         <%  System.Data.SqlClient.SqlConnection con;
                        System.Data.SqlClient.SqlDataAdapter da;
                        System.Data.DataTable dt;
                        con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                        da = new System.Data.SqlClient.SqlDataAdapter("select que,chno,disque,attque,totmark from paperstyle where cid=2 ", con);
                        dt = new System.Data.DataTable(); 
                         da.Fill(dt);
                         int tot = dt.Rows.Count;
                         %>

    
	       <%for(int j=0;j<tot;j++){ %>
            <%Response.Write(dt.Rows[j][0].ToString()); %> atte <% Response.Write(dt.Rows[j][3].ToString()); %> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <%Response.Write(dt.Rows[1][4].ToString()); %><br />
	        <% int dos = Convert.ToInt32(dt.Rows[j][2].ToString()) ; %>
            <% for(int i=0;i<=dos;i++)
            { %>
                    <% Response.Write(i); %> sdfajsdfkjs?<br />            
	        <%} %>
            <%} %>
    
     </div> 
</div> 
</div> 
    </asp:Content>

