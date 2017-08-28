<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/MasterPage.master" AutoEventWireup="true" CodeBehind="inventario.aspx.cs" Inherits="Analisis_1.Modulos.Inventario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <!-- Content Header (Page header) -->

    <h1>Inventario</h1>
    <!-- Main content -->
    <!-- AREA DE VENTA -->
     <div class="box box-danger">
        <!-- /.box-header -->
        <div class="box-body">
          <div class="row">
            <div class="col-md-6">
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Seleccione el producto: </label>
                        <center>
                            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="nombre" DataValueField="id">
                            </asp:DropDownList>
                        </center>
                      
                    </div>
                    <div class="form-group">
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SistemaERPConnectionString %>" SelectCommand="SELECT [nombre], [id] FROM [Producto]"></asp:SqlDataSource>
                    </div>
                   
                <div class="form-group">
                                      
                  <label>Cantidad: </label>                                        
                    <asp:TextBox ID="txt_cantidad" runat="server"></asp:TextBox>
                </div>
                </div>                 
            </div>
          
          </div>
          <!-- /.row -->
            <div class="box-footer">
                    <center>
                        <asp:Button ID="Button1" runat="server" Text="Actualizar" OnClick="btn_actualizar_Click"></asp:Button>  
                    </center>
                           
              </div>
        </div>
        <!-- /.box-body -->
       
      </div>
      <!-- /.box -->



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentJS" runat="server">
</asp:Content>
