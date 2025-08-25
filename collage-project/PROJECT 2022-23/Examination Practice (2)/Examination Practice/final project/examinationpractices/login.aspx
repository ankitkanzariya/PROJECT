<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row"><br/ >
              <div class="col-md-3" align="center">
                		  <div id="id01" class="w3-modal">
							<div class="w3-modal-content w3-animate-zoom w3-card-4" style="width:500px">
							  
							  <div class="w3-container" align="center"><br />
								  <div class="w3-center"><br>
                                        <a href="Default.aspx"><span onclick="document.getElementById('id01').style.display='none'" class="w3-closebtn w3-hover-red w3-container w3-padding-8 w3-display-topright" title="Close Modal">&times;</span></a>
                                        <img src="admin/images/img_avatar4.png" alt="Avatar" style="width:30%" class="w3-circle w3-margin-top">
                                      </div>
								  
                                     <form class="w3-container">
                                        <div class="w3-section">
                                          <label><b>Email</b></label>
                                            <asp:TextBox ID="email" runat="server" class="w3-input w3-border"  type="text" placeholder="Enter E-mail"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server" ErrorMessage="Please Enter email" ControlToValidate="email" ForeColor="Red"></asp:RequiredFieldValidator>
                                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Enter Valid EMail address" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="email"></asp:RegularExpressionValidator><br />
                                          <label><b>Password</b></label>
                                            <asp:TextBox ID="pass" runat="server" class="w3-input w3-border" type="password" placeholder="Enter Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  runat="server" ErrorMessage="Please Enter Password" ControlToValidate="pass" ForeColor="Red"></asp:RequiredFieldValidator><br />
                                          <asp:Button ID="logins" runat="server" Text="Login" class="w3-btn-block w3-green w3-section w3-padding" onclick="logins_Click" />
                                          <p><span class="w3-tag w3-blue"><a style="text-decoration:none;color:white;" href="registration.aspx">Registration</a></span></p>
                                          <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        <asp:HyperLink ID="HyperLink1" runat="server" BackColor="White" 
                                            ForeColor="#3399FF" NavigateUrl="http://gmail.com" Visible="False">Examinationpractices@gmail.com</asp:HyperLink>
                                        </div>
                                    </form>
                                    
                              </div>
							</div>
						  </div>
			</div>
	<!-- w3 container-->
    </div>

<script type="text/javascript">
    $(document).ready(function () {
        document.getElementById('id01').style.display = 'block'
    });

</script>

</asp:Content>

