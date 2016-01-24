USE ABUELO 
GO  
CREATE PROCEDURE SPU_MODIFICAR_PERSONAS
@IdPersona char(8), 
@IdPersona char(8), 
@ApellidoP nvarchar(20), 
@ApellidoM nvarchar(20), 
@Nombres nvarchar(40), 
@Sexo char(1), 
@FechaNac date= NULL,
@Telefono char(9)= NULL,
@Celu_Movi char(9)= NULL,
@Celu_Claro char(9)= NULL,
@Estado char(1) 
AS
UPDATE PERSONAS SET IdPersona=@IdPersona,ApellidoP=@ApellidoP,ApellidoM=@ApellidoM,Nombres=@Nombres,Sexo=@Sexo,FechaNac=@FechaNac,Telefono=@Telefono,Celu_Movi=@Celu_Movi,Celu_Claro=@Celu_Claro,Estado=@Estado WHERE IdPersona =@IdPersona
GO
