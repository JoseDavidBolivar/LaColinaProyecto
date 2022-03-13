

/****** Object:  Database [COLINADB]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE DATABASE [COLINADB]
USE [COLINADB]

/****** Object:  Table [dbo].[TBL_CATEGORIAS]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_CATEGORIAS](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CATEGORIA] [varchar](255) NULL,
	[ESTADO] [varchar](255) NULL,
)

/****** Object:  Table [dbo].[TBL_CIERRES]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_CIERRES](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FECHA_HORA_APERTURA] [datetime] NULL,
	[FECHA_HORA_CIERRE] [datetime] NULL,
	[ID_USUARIO] [numeric](18, 0) NULL,
	[CANT_MESAS_ATENDIDAS] [numeric](18, 0) NULL,
	[CANT_FINALIZADAS] [numeric](18, 0) NULL,
	[TOTAL_FINALIZADAS] [numeric](18, 0) NULL,
	[CANT_LLEVAR] [numeric](18, 0) NULL,
	[TOTAL_LLEVAR] [numeric](18, 0) NULL,
	[CANT_CANCELADAS] [numeric](18, 0) NULL,
	[TOTAL_CANCELADAS] [numeric](18, 0) NULL,
	[CANT_CONSUMO_INTERNO] [numeric](18, 0) NULL,
	[TOTAL_CONSUMO_INTERNO] [numeric](18, 0) NULL,
	[OTROS_COBROS_TOTAL] [numeric](18, 0) NULL,
	[DESCUENTOS_TOTAL] [numeric](18, 0) NULL,
	[IVA_TOTAL] [numeric](18, 0) NULL,
	[I_CONSUMO_TOTAL] [numeric](18, 0) NULL,
	[SERVICIO_TOTAL] [numeric](18, 0) NULL,
	[TOTAL_EFECTIVO] [numeric](18, 0) NULL,
	[TOTAL_TARJETA] [numeric](18, 0) NULL,
	[VENTA_TOTAL] [numeric](18, 0) NULL
)

/****** Object:  Table [dbo].[TBL_DIAS_TRABAJADOS]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_DIAS_TRABAJADOS](
	[ID] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[ID_USUARIO_NOMINA] [numeric](18, 0) NULL,
	[FECHA_TRABAJADO] [datetime] NULL,
	[SUELDO_DIARIO] [numeric](18, 0) NULL,
	[PROPINAS] [decimal](18, 2) NULL,
	[FECHA_PAGO] [datetime] NULL,
	[ID_PERFIL] [numeric](18, 0) NULL,	
)

/****** Object:  Table [dbo].[TBL_IMPRESORAS]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_IMPRESORAS](
	[ID] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[NOMBRE_IMPRESORA] [varchar](255) NULL,
	[DESCRIPCION] [varchar](255) NULL,
)

/****** Object:  Table [dbo].[TBL_IMPUESTOS]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_IMPUESTOS](
	[ID] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[NOMBRE_IMPUESTO] [varchar](255) NULL,
	[PORCENTAJE] [numeric](18, 0) NULL,
	[ESTADO] [varchar](255) NULL,
)

/****** Object:  Table [dbo].[TBL_MASTER_MESAS]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_MASTER_MESAS](
	[ID] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[NOMBRE_MESA] [varchar](255) NULL,
	[DESCRIPCION] [varchar](255) NULL,
	[ESTADO] [varchar](255) NULL,
	[ID_USUARIO] [numeric](18, 0) NULL,
	[NUMERO_MESA] [numeric](18, 0) NULL,
)

/****** Object:  Table [dbo].[TBL_NOMINA]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_NOMINA](
	[ID] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[ID_USUARIO_SISTEMA] [numeric](18, 0) NULL,
	[ID_PERFIL] [numeric](18, 0) NULL,
	[CEDULA] [numeric](18, 0) NULL,
	[NOMBRE] [varchar](255) NULL,
	[CARGO] [varchar](255) NULL,
	[DIAS_TRABAJADOS] [numeric](18, 0) NULL,
	[PROPINAS] [decimal](18, 2) NULL,
	[FECHA_PAGO] [datetime] NULL,
	[FECHA_NACIMIENTO] [date] NULL,
	[DIRECCION_RESIDENCIA] [varchar](255) NULL,
	[TELEFONO] [numeric](18, 0) NULL,
	[ESTADO] [varchar](255) NULL,
	[TOTAL_PAGAR] [decimal](18, 2) NULL,
	[CONSUMO_INTERNO] [numeric](18, 2) NULL,
)

CREATE TABLE [dbo].[TBL_PERFIL](
	[ID] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[NOMBRE_PERFIL] [varchar](255) NULL,
	[PORCENTAJE_PROPINA] [numeric](18, 0) NULL,
)

/****** Object:  Table [dbo].[TBL_PRODUCTOS]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_PRODUCTOS](
	[ID] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[ID_CATEGORIA] [numeric](18, 0) NOT NULL,
	[FECHA_INGRESO] [datetime] NULL,
	[NOMBRE_PRODUCTO] [varchar](255) NULL,
	[PRECIO] [varchar](255) NULL,
	[CANTIDAD] [numeric](18, 0) NULL,
	[DESCRIPCION] [varchar](255) NULL,
	[ID_IMPRESORA] [numeric](18, 0) NULL,
)

/****** Object:  Table [dbo].[TBL_PRODUCTOS_SOLICITUD]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_PRODUCTOS_SOLICITUD](
	[ID] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[FECHA_REGISTRO] [datetime] NULL,
	[ID_SOLICITUD] [numeric](18, 0) NULL,
	[ID_PRODUCTO] [numeric](18, 0) NULL,
	[ID_MESERO] [numeric](18, 0) NULL,
	[PRECIO_PRODUCTO] [numeric](18, 0) NULL,
	[ESTADO_PRODUCTO] [varchar](255) NULL,
	[DESCRIPCION] [varchar](1000) NULL,
)

/****** Object:  Table [dbo].[TBL_SOLICITUD]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_SOLICITUD](
	[ID] [numeric](18, 0) IDENTITY(10100,1) PRIMARY KEY NOT NULL,
	[FECHA_SOLICITUD] [datetime] NULL,
	[ID_MESA] [numeric](18, 0) NULL,
	[ID_MESERO] [numeric](18, 0) NULL,
	[IDENTIFICACION_CLIENTE] [varchar](255) NULL,
	[NOMBRE_CLIENTE] [varchar](255) NULL,
	[ESTADO_SOLICITUD] [varchar](255) NULL,
	[OBSERVACIONES] [ntext] NULL,
	[OTROS_COBROS] [numeric](18, 0) NULL,
	[DESCUENTOS] [numeric](18, 0) NULL,
	[SUBTOTAL] [numeric](18, 0) NULL,
	[PORCENTAJE_IVA] [numeric](18, 0) NULL,
	[IVA_TOTAL] [numeric](18, 0) NULL,
	[PORCENTAJE_I_CONSUMO] [numeric](18, 0) NULL,
	[I_CONSUMO_TOTAL] [numeric](18, 0) NULL,
	[PORCENTAJE_SERVICIO] [numeric](18, 0) NULL,
	[SERVICIO_TOTAL] [numeric](18, 0) NULL,
	[TOTAL] [numeric](18, 0) NULL,
	[METODO_PAGO] [varchar](255) NULL,
	[VOUCHER] [varchar](255) NULL,
	[CANT_EFECTIVO] [numeric](18, 0) NULL,
)

/****** Object:  Table [dbo].[TBL_USUARIOS]    Script Date: 23/10/2021 5:49:42 p. m. ******/
CREATE TABLE [dbo].[TBL_USUARIOS](
	[ID] [numeric](18, 0) IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[CEDULA] [numeric](18, 0) NULL,
	[NOMBRE] [varchar](255) NULL,
	[CONTRASEÑA] [varchar](255) NULL,
	[ID_PERFIL] [numeric](18, 0) NULL,
)

INSERT INTO TBL_IMPUESTOS VALUES ('IVA', 10, 'INACTIVO')
INSERT INTO TBL_IMPUESTOS VALUES ('IMPUESTO CONSUMO', 10, 'INACTIVO')
INSERT INTO TBL_IMPUESTOS VALUES ('SERVICIO', 10, 'ACTIVO')

INSERT INTO TBL_PERFIL VALUES ('ADMINISTRADOR', 0)
INSERT INTO TBL_PERFIL VALUES ('CAJERO', 0)
INSERT INTO TBL_PERFIL VALUES ('MESERO', 50)
INSERT INTO TBL_PERFIL VALUES ('PARRILLA/COCINA Y BAR', 40)

INSERT INTO TBL_USUARIOS VALUES (1076622744, 'JOSE DAVID GOMEZ', '622744', 1)

