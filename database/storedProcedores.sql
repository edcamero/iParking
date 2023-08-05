SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_IngTarjetas]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[sp_IngTarjetas]
(
	@numero_tarjeta			 		AS NUMERIC,
	@mes_vcto						AS NUMERIC,
	@ano_vcto						AS NUMERIC,
	@cvv							AS NUMERIC,
	@tipo							AS NUMERIC,
	@default_tarjeta				AS NUMERIC,
	@id_usuario						AS NUMERIC,
	@fecha_hora_creado				AS NVARCHAR(20),
	@estado							AS NUMERIC
)
AS

SET NOCOUNT ON

DECLARE @id			AS NUMERIC

BEGIN
	INSERT INTO TBL_TARJETA (numero_tarjeta,mes_vcto,ano_vcto,cvv,tipo,default_tarjeta,id_usuario,fecha_hora_creado,estado)
	VALUES (@numero_tarjeta,@mes_vcto,@ano_vcto,@cvv,@tipo,@default_tarjeta,@id_usuario,@fecha_hora_creado,@estado)
	SET @id = @@IDENTITY
END
SELECT 	''id'' = @id

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_IngPlacas]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create PROCEDURE [dbo].[sp_IngPlacas]
(
	@placa					 		AS NVARCHAR(6),
	@placa_default					AS NUMERIC,
	@id_usuario						AS NUMERIC,
	@fecha_hora_creado   			AS NVARCHAR(20),
	@estado							AS NUMERIC
)
AS

SET NOCOUNT ON

DECLARE @id			AS NUMERIC

BEGIN
	INSERT INTO TBL_PLACA (placa,placa_default,id_usuario,fecha_hora_creado,estado)
	VALUES (@placa,@placa_default,@id_usuario,@fecha_hora_creado,@estado)
	SET @id = @@IDENTITY
END
SELECT 	''id'' = @id
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_IngUsuario]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[sp_IngUsuario]
(
	@rut	  		AS NVARCHAR(12),
	@dv				AS NVARCHAR(1),
	@nombres		AS NVARCHAR(30),
	@apellidos		AS NVARCHAR(40),
	@telefono		AS NVARCHAR(12),
	@clave_acceso	AS NVARCHAR(8),
	@mail			AS NVARCHAR(20),
	@estado			AS NUMERIC
)
AS

SET NOCOUNT ON

DECLARE @id			AS NUMERIC,
@ultimoidmov			AS NUMERIC

-----set @fechaMovBici_1 = convert(DATETIME,getdate(),103)

BEGIN
	INSERT INTO TBL_USUARIOS (rut,dv,nombres,apellidos,telefono,clave_acceso,mail,estado)
	VALUES (@rut,@dv,@nombres,@apellidos,@telefono,@clave_acceso,@mail,@estado)
	SET @id = @@IDENTITY
END
SELECT 	''id'' = @id




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_IngCelulares]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_IngCelulares]
(
	@fecha_hora_activacion  		AS NVARCHAR(20),
	@imei							AS NVARCHAR(20),
	@serie							AS NVARCHAR(20),
	@estado							AS NUMERIC
)
AS

SET NOCOUNT ON

DECLARE @id			AS NUMERIC,
@ultimoidmov			AS NUMERIC


BEGIN
	INSERT INTO TBL_CELULARES (fecha_hora_activacion,imei,serie,estado)
	VALUES (@fecha_hora_activacion,@imei,@serie,@estado)
	SET @id = @@IDENTITY
END
SELECT 	''id'' = @id' 
END