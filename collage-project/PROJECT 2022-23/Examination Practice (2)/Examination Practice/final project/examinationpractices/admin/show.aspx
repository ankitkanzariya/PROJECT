<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="show.aspx.cs" Inherits="admin_show" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%  System.Data.SqlClient.SqlConnection con;
    System.Data.SqlClient.SqlDataAdapter da;
    System.Data.DataSet ds;%>
     
<div class="container">
        <div class="row">
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <div class="col-md-12">
            <table border="2">
            
                
               <tr align="center">
               <th>course</th>
               <th>sem</th>
               <th>sub</th>
               
               <th>disque</th>
               <th>attque</th>
               <th>eachque</th>
               <th>totmark</th>
               </tr>
                <tr align="center">
               
                <% int f = Convert.ToInt32(Session["mainno"]); %> 

               <%   Response.Write("<td>");
                    string data1 = Convert.ToString(Session["disque"]);
                    string[] splitData1 = data1.Split(',');
                    for (int i = 0; i < f; i++)
                    {
                        Response.Write(splitData1[i] + "<br />");
                        Session["disque"] = splitData1[i]; 
                    }
                    Response.Write("</td>");
                %> 
               <% string data2 = Convert.ToString(Session["attque"]);
                          string[] splitData2 = data2.Split(',');
                          Response.Write("<td>");
                          for (int i = 0; i < f; i++)
                          {
                              Response.Write(splitData2[i] + "<br />"); Session["attque"] = splitData2[i];
                          }
                          Response.Write("</td>");
               %> 
                <%//Response.Write(Session["eachque"]);
                          string data3 = Convert.ToString(Session["eachque"]);
                          string[] splitData3 = data3.Split(',');
                          Response.Write("<td>");
                          for (int i = 0; i < f; i++)
                          {
                              Response.Write(splitData3[i] + "<br />"); Session["eachque"] = splitData3[i];
                          }
                          Response.Write("</td>");
                           %>
               <%//Response.Write(Session["totmark"]);
                          string data4 = Convert.ToString(Session["totmark"]);
                          string[] splitData4 = data4.Split(',');
                          Response.Write("<td>");
                          for (int i = 0; i < f; i++)
                          {
                              Response.Write(splitData4[i] + "<br />");
                              Session["totmarki"] = splitData4[i];
                          }
                          Response.Write("</td></tr>");
                           %> <br />

                           
               
                
                <%  for (int j = 0; j < f; j++)
                    {
                        Session["jsd"]=j;
                    }
                     
                    con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                    string sd= Convert.ToString(Session["course1"]);
                    da = new System.Data.SqlClient.SqlDataAdapter("insert into master_p_style (cid,datetime,mainque,examtime) values (" + Session["course"] + ",'" + Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "'," + Session["mainno"] + ",'" + Session["time"] + "')", con);
                    ds = new System.Data.DataSet();
                    da.Fill(ds);
                    
                        for (int i = 0; i < f; i++)
                        {
                            int s = i + 1;
                            con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                            da = new System.Data.SqlClient.SqlDataAdapter("insert into child_p_style (cid,disque,attque,eachque,totmark,que,gt,datetime) values(" + Session["course"] + "," + splitData1[i] + "," + splitData2[i] + "," + splitData3[i] + "," + splitData4[i] + "," + s + "," + Session["gt"] + ",'" + Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "')", con);
                            ds = new System.Data.DataSet(); 
                            da.Fill(ds);
                       }
                       Response.Redirect("paperstyle.aspx");         
                         %>

                    
                    </tr>
                </table>
            </div>
       </div>
</div>
   <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
  <script type="text/javascript">
  
  
  </script>
</asp:Content>

