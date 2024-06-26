---ROLL OUT
ALTER TABLE TBL_DIAS_TRABAJADOS
ADD ID_PERFIL NUMERIC(18,0)
---ROLL BACK
ALTER TABLE TBL_DIAS_TRABAJADOS
DROP COLUMN ID_PERFIL 

	
---ROLL OUT
CREATE TABLE [dbo].[TBL_SISTEMA](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL Primary key,
	[PARAMETRO] [varchar](255) NULL,
	[VALOR] [varchar](255) NULL
)
---ROLL BACK
DROP TABLE [dbo].[TBL_SISTEMA]


---ROLL OUT
CREATE TABLE [dbo].[TBL_TOKENS_DIAN](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL Primary key,
	[FECHA_TOKEN] [datetime] NULL,
	[FECHA_VENCIMIENTO] [datetime] NULL,
	[ACCESS_TOKEN] [varchar](4000) NULL,
	[EXPIRES_IN] [int] NULL,
	[TOKEN_TYPE] [varchar](255) NULL,
	[SCOPE] [varchar](255) NULL
)
INSERT INTO [dbo].[TBL_TOKENS_DIAN]
           ([FECHA_TOKEN]
           ,[FECHA_VENCIMIENTO]
           ,[ACCESS_TOKEN]
           ,[EXPIRES_IN]
           ,[TOKEN_TYPE]
           ,[SCOPE])
VALUES
	('2024-06-22 00:00:57.650','2024-06-22 00:44:09.650','eyJhbGciOiJSUzI1NiIsImtpZCI6IjExNDQzRDg2OUYxMzgwODlEREUwOTdENTNBN0YxNzVCNkQwNzIxNzdSUzI1NiIsInR5cCI6ImF0K2p3dCIsIng1dCI6IkVVUTlocDhUZ0luZDRKZlZPbjhYVzIwSElYYyJ9.eyJuYmYiOjE3MTkwMzI0NTYsImV4cCI6MTcyMTYyNDQ1NiwiaXNzIjoiaHR0cDovL21zLXNlY3VyaXR5OjUwMDAiLCJhdWQiOiJodHRwOi8vbXMtc2VjdXJpdHk6NTAwMC9yZXNvdXJjZXMiLCJjbGllbnRfaWQiOiJTaWlnb0FQSSIsInN1YiI6IjIwMzY0ODgiLCJhdXRoX3RpbWUiOjE3MTkwMzI0NTYsImlkcCI6ImxvY2FsIiwibmFtZSI6ImRhZ29tZXouMTBAaG90bWFpbC5jb20iLCJtYWlsX3NpaWdvIjoiZGFnb21lei4xMEBob3RtYWlsLmNvbSIsImNsb3VkX3RlbmFudF9jb21wYW55X2tleSI6IkpPU0VEQVZJREdPTUVaQ1JVWiIsInVzZXJzX2lkIjoiNDY2IiwidGVuYW50X2lkIjoiMHgwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDk3MTk3OCIsInVzZXJfbGljZW5zZV90eXBlIjoiMCIsInBsYW5fdHlwZSI6IjEyIiwidGVuYW50X3N0YXRlIjoiMSIsIm11bHRpdGVuYW50X2lkIjoiMTI5NyIsImNvbXBhbmllcyI6IjAiLCJhcGlfc3Vic2NyaXB0aW9uX2tleSI6IjBlOGNkODM1MmExNTRhODU4OTJkYmVjNGMzNDU2NmZmIiwiYXBpX3VzZXJfY3JlYXRlZF9hdCI6IjE3MTkwMTc1MDUiLCJhY2NvdW50YW50IjoiZmFsc2UiLCJqdGkiOiIzNkU2MzkyOUYzMEMzRUNGNEU0ODNBRjZENkEzNTU3QSIsImlhdCI6MTcxOTAzMjQ1Niwic2NvcGUiOlsiU2lpZ29BUEkiXSwiYW1yIjpbImN1c3RvbSJdfQ.nn5LTaxJZ3OdzTbEwhmmP8_33SzCbdrF0eNj26mQcVQO3e4BgvUsoekXX24ZKgIcm68_KimQzJ8BrE93N8Gfg_fnZkpKWrRaHjZMylGU5sQ5MtF8B9gJ3e3FXf3MYOn7Uu6piac8V1fMFz6oLKwNJaz5h8dXRD7M0F_16Da9ttqHtI88gpb-O8O4Jz6nAvVR7OPLubp2vPUDe9TlIvp1NluNfGNJvTcMtqzhAUPVSKq3p_COVgxZyJ-nP5RUVWb28KfEIXg9_6w_3HxAdguBcv0Gu1IfBK29RddDQ9wChPJNijUKonbK79pWlgE9sHpc2DxnQnvUPLaFWc3hSm7UBA',
2592000, 'Bearer', 'SiigoAPI')
ALTER TABLE TBL_PRODUCTOS
ADD UP_DIAN NUMERIC(2,0)
UPDATE TBL_PRODUCTOS
SET UP_DIAN = 0
ALTER TABLE TBL_PRODUCTOS
ADD ID_DIAN VARCHAR(255), ACCOUNT_GROUP_DIAN NUMERIC(10,0)
CREATE TABLE [dbo].[TBL_CLIENTES_DIAN](
	[ID] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,	
	[TIPO_PERSONA] [varchar](255) NULL,
	[CODIGO_DOCUMENTO] [varchar](255) NULL,
	[NOMBRE_DOCUMENTO] [varchar](255) NULL,
	[NUMERO_IDENTIFICACION] [varchar](255) NULL,
	NOMBRES [varchar](255) NULL,
	[APELLIDOS] [varchar](255) NULL,
	RAZON_SOCIAL [varchar](255) NULL,
	NOMBRE_COMERCIAL [varchar](255) NULL,
	DIRECCION [varchar](255) NULL,
	COD_CIUDAD [varchar](255) NULL,
	NOM_CIUDAD [varchar](255) NULL,
	EMAIL [varchar](255) NULL,
	RESPONSABLE_IVA bit,
	CODIGO_R_FISCAL VARCHAR(255),
	NOMBRE_R_FISCAL VARCHAR(255),
	ID_CODIGO_DIAN VARCHAR(100) NULL,
)
ALTER TABLE TBL_SOLICITUD
ADD ID_CLIENTE VARCHAR(255), FACTURACION_ELECTRONICA VARCHAR(2), ENVIO_DIAN VARCHAR(2)
---ROLL BACK
DROP TABLE [dbo].[TBL_PRODUCTOS]
ALTER TABLE TBL_PRODUCTOS
DROP COLUMN UP_DIAN NUMERIC, ID_DIAN, ACCOUNT_GROUP_DIAN
DROP TABLE [dbo].[TBL_CLIENTES_DIAN]





