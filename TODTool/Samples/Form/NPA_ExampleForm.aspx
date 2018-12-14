<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="NPA_ExampleForm.aspx.cs" Inherits="TODTool.NPA_ExampleForm" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <h2>Add Employee</h2>
        <asp:Label ID="lblMessage" runat="server" /><br />
        <br />
        <table>
            <tr>
                <td>Employee ID:</td>
                <td>
                    <asp:TextBox ID="txtEmployeeID" runat="server" MaxLength="5" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtEmployeeID" runat="server"
                        ErrorMessage="Employee ID must be provided" />
                </td>
            </tr>
            <tr>
                <td>First Name:</td>
                <td>
                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="40" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFirstName" runat="server"
                        ErrorMessage="First name must be provided" />
                </td>
            </tr>
            <tr>
                <td>Last Name:</td>
                <td>
                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="30" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtLastName" runat="server"
                        ErrorMessage="Last name must be provided" />
                </td>
            </tr>
            <tr>
                <td align="center"><asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_OnClick" Text="Add Employee" /></td>
                <td><asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_OnClick" Text="Cancel" CausesValidation="false" /></td>
            </tr>
        </table>
    </div>
    <div>
        <h2>Employees</h2>
        <br />
        <asp:GridView ID="grdEmployees" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="Employee ID" />
                <asp:BoundField DataField="FirstName" HeaderText="First name" />
                <asp:BoundField DataField="LastName" HeaderText="Last name" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
