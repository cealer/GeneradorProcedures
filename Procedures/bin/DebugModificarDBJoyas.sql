USE DBJoyas 
GO  
CREATE PROCEDURE SPU_MODIFICAR_Sucursal
@Id char(8), 
@IdEmpresa char(8), 
@Descripcion varchar(150), 
@Direccion varchar(250) 
AS
UPDATE Sucursal SET IdEmpresa=@IdEmpresa,Descripcion=@Descripcion,Direccion=@Direccion WHERE Id =@Id
GO
