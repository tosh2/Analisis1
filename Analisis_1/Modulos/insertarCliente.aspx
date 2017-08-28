<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/MasterPage.master" AutoEventWireup="true" CodeBehind="insertarCliente.aspx.cs" Inherits="Analisis_1.Modulos.insertarCliente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Content Header (Page header) -->

    <h1>Nuevo cliente</h1>
    <!-- Main content -->
    <!-- AREA DE VENTA -->
     <div class="box box-danger">
        <!-- /.box-header -->
        <div class="box-body">
          <div class="row">
            <div class="col-md-6">
                <div class="col-md-6">
                <div class="form-group">
                  <label>Nombre: </label>
                  <asp:TextBox ID="txtNombre" runat="server" class="form-control" placeholder="Nombre"></asp:TextBox>
                  <label>Apellido: </label>
                  <asp:TextBox ID="txtApellido" runat="server" class="form-control" placeholder="Apellido"></asp:TextBox>
                  <label>Nit: </label>
                  <asp:TextBox ID="txtNit" runat="server" class="form-control" placeholder="Nit"></asp:TextBox>
                  <label>DPI: </label>
                  <asp:TextBox ID="txtDPI" runat="server" class="form-control" placeholder="DPI"></asp:TextBox>
                  <label>Teléfono: </label>
                  <asp:TextBox ID="txtTelefono" runat="server" class="form-control" placeholder="Telefono"></asp:TextBox>
                </div>
                </div>                 
            </div>
          
          </div>
          <!-- /.row -->
            <div class="box-footer">
                
                <asp:Button ID="btnAgregarCliente" runat="server" Text="Agregar Cliente" class="btn btn-primary" OnClick="btnAgregarCliente_Click" />
              </div>
        </div>
        <!-- /.box-body -->
       
      </div>
      <!-- /.box -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentJS" runat="server">
</asp:Content>
