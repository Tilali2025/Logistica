------------------------------------------------------------------------------
--BASE DATOS------------------------------------------------------------------
------------------------------------------------------------------------------

CREATE DATABASE BDALMACEN
GO
USE BDALMACEN 
GO
------------------------------------------------------------------------------
--TABLA-----------------------------------------------------------------------
------------------------------------------------------------------------------

CREATE TABLE dbo.MOV_INVENTARIO (
    COD_CIA varchar(5) NOT NULL,
    COMPANIA_VENTA_3 varchar(5) NOT NULL,
    ALMACEN_VENTA varchar(10) NOT NULL,
    TIPO_MOVIMIENTO varchar(2) NOT NULL,
    TIPO_DOCUMENTO varchar(2) NOT NULL,
    NRO_DOCUMENTO varchar(50) NOT NULL,
    COD_ITEM_2 varchar(50) NOT NULL,
    PROVEEDOR varchar(100) NULL,
    ALMACEN_DESTINO varchar(50) NULL,
    CANTIDAD int NULL,
    DOC_REF_1 varchar(50) NULL,
    DOC_REF_2 varchar(50) NULL,
    DOC_REF_3 varchar(50) NULL,
    DOC_REF_4 varchar(50) NULL,
    DOC_REF_5 varchar(50) NULL,
    FECHA_TRANSACCION DATETIME NULL,
    CONSTRAINT PK_MOV_INVENTARIO PRIMARY KEY CLUSTERED (
        COD_CIA,
        COMPANIA_VENTA_3,
        ALMACEN_VENTA,
        TIPO_MOVIMIENTO,
        TIPO_DOCUMENTO,
        NRO_DOCUMENTO,
        COD_ITEM_2
    )
)
GO
------------------------------------------------------------------------------
--LISTADO---------------------------------------------------------------------
------------------------------------------------------------------------------
CREATE PROCEDURE USP_Listado_Movimiento
AS
BEGIN
    SET NOCOUNT ON
    BEGIN TRY
        SELECT  
        COMPANIA_VENTA_3 AS COMPAÑIA,
        ALMACEN_VENTA,
        ALMACEN_DESTINO,
        TIPO_MOVIMIENTO,
        TIPO_DOCUMENTO,
        NRO_DOCUMENTO,
        PROVEEDOR,
        CANTIDAD,
        DOC_REF_1
        FROM MOV_INVENTARIO
    END TRY
    BEGIN CATCH
        SELECT
            ERROR_NUMBER() AS Numero_Error,
            ERROR_MESSAGE() AS Mensaje_Error
    END CATCH
END
GO

------------------------------------------------------------------------------
--REGISTRAR-------------------------------------------------------------------
------------------------------------------------------------------------------
CREATE PROCEDURE USP_Registrar_Movimiento
(
    @COD_CIA           varchar(5),
    @COMPANIA_VENTA_3  varchar(5),
    @ALMACEN_VENTA     varchar(10),
    @TIPO_MOVIMIENTO   varchar(2),
    @TIPO_DOCUMENTO    varchar(2),
    @NRO_DOCUMENTO     varchar(50),
    @COD_ITEM_2        varchar(50),
    @PROVEEDOR         varchar(100),
    @ALMACEN_DESTINO   varchar(50),
    @CANTIDAD          int,
    @DOC_REF_1         varchar(50),
    @DOC_REF_2         varchar(50),
    @DOC_REF_3         varchar(50),
    @DOC_REF_4         varchar(50),
    @DOC_REF_5         varchar(50),
    @FECHA_TRANSACCION datetime
)
AS
BEGIN
    SET NOCOUNT ON
    BEGIN TRY
        BEGIN TRAN
        IF EXISTS (SELECT 1 FROM MOV_INVENTARIO WHERE COD_CIA = @COD_CIA)
        BEGIN
            UPDATE MOV_INVENTARIO
            SET
                COMPANIA_VENTA_3  = @COMPANIA_VENTA_3,
                ALMACEN_VENTA     = @ALMACEN_VENTA,
                TIPO_MOVIMIENTO   = @TIPO_MOVIMIENTO,
                TIPO_DOCUMENTO    = @TIPO_DOCUMENTO,
                NRO_DOCUMENTO     = @NRO_DOCUMENTO,
                COD_ITEM_2        = @COD_ITEM_2,
                PROVEEDOR         = @PROVEEDOR,
                ALMACEN_DESTINO   = @ALMACEN_DESTINO,
                CANTIDAD          = @CANTIDAD,
                DOC_REF_1         = @DOC_REF_1,
                DOC_REF_2         = @DOC_REF_2,
                DOC_REF_3         = @DOC_REF_3,
                DOC_REF_4         = @DOC_REF_4,
                DOC_REF_5         = @DOC_REF_5,
                FECHA_TRANSACCION = ISNULL(@FECHA_TRANSACCION, GETDATE())
            WHERE COD_CIA = @COD_CIA
        END
        ELSE
        BEGIN
            INSERT INTO MOV_INVENTARIO
            (
                COD_CIA,
                COMPANIA_VENTA_3,
                ALMACEN_VENTA,
                TIPO_MOVIMIENTO,
                TIPO_DOCUMENTO,
                NRO_DOCUMENTO,
                COD_ITEM_2,
                PROVEEDOR,
                ALMACEN_DESTINO,
                CANTIDAD,
                DOC_REF_1,
                DOC_REF_2,
                DOC_REF_3,
                DOC_REF_4,
                DOC_REF_5,
                FECHA_TRANSACCION
            )
            VALUES
            (
                @COD_CIA,
                @COMPANIA_VENTA_3,
                @ALMACEN_VENTA,
                @TIPO_MOVIMIENTO,
                @TIPO_DOCUMENTO,
                @NRO_DOCUMENTO,
                @COD_ITEM_2,
                @PROVEEDOR,
                @ALMACEN_DESTINO,
                @CANTIDAD,
                @DOC_REF_1,
                @DOC_REF_2,
                @DOC_REF_3,
                @DOC_REF_4,
                @DOC_REF_5,
                ISNULL(@FECHA_TRANSACCION, GETDATE())
            )
        END
        COMMIT TRAN
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN
        DECLARE @Msg nvarchar(4000) = ERROR_MESSAGE()
        RAISERROR(@Msg, 16, 1)
    END CATCH
END
GO
------------------------------------------------------------------------------
--ELIMINAR--------------------------------------------------------------------
------------------------------------------------------------------------------
CREATE PROCEDURE USP_Eliminar_Movimiento
(
    @IdMovimiento VARCHAR(5)
)
AS
BEGIN
    SET NOCOUNT ON
    BEGIN TRY
        BEGIN TRANSACTION
            DELETE FROM MOV_INVENTARIO WHERE COD_CIA = @IdMovimiento
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        SELECT
            ERROR_NUMBER() AS Numero_Error,
            ERROR_MESSAGE() AS Mensaje_Error
    END CATCH
END
GO
------------------------------------------------------------------------------
--CONSULTAR-------------------------------------------------------------------
------------------------------------------------------------------------------
CREATE PROCEDURE USP_Listar_Movimiento_Filtro
(
    @Campo        VARCHAR(20) = NULL,
    @Texto        VARCHAR(60) = NULL,
    @FechaInicio  DATE = NULL,
    @FechaFin     DATE = NULL
)
AS
BEGIN
    SET NOCOUNT ON
    SELECT  
        COD_CIA,
        COMPANIA_VENTA_3,
        ALMACEN_VENTA,
        ALMACEN_DESTINO,
        TIPO_MOVIMIENTO,
        TIPO_DOCUMENTO,
        NRO_DOCUMENTO,
        PROVEEDOR,
        CANTIDAD,
        DOC_REF_1,
        FECHA_TRANSACCION
    FROM MOV_INVENTARIO
    WHERE 
        (
            @Texto IS NULL 
            OR LTRIM(RTRIM(@Texto)) = ''
            OR (@Campo = 'ALMACEN_VENTA' AND ALMACEN_VENTA LIKE '%' + @Texto + '%')
            OR (@Campo = 'ALMACEN_DESTINO' AND ALMACEN_DESTINO LIKE '%' + @Texto + '%')
            OR (@Campo = 'PROVEEDOR' AND PROVEEDOR LIKE '%' + @Texto + '%')
        )
        AND (@FechaInicio IS NULL OR FECHA_TRANSACCION >= @FechaInicio)
        AND (@FechaFin IS NULL OR FECHA_TRANSACCION <= @FechaFin)
    ORDER BY FECHA_TRANSACCION DESC
END
------------------------------------------------------------------------------
--BUSCAR-------------------------------------------------------------------
------------------------------------------------------------------------------
CREATE PROCEDURE USP_Buscar_Movimiento  
(  
    @Cod_Cia  VARCHAR(5) = NULL  
)  
AS  
BEGIN  
    SET NOCOUNT ON  
    SELECT
        COD_CIA,
        COMPANIA_VENTA_3,
        ALMACEN_VENTA,
        ALMACEN_DESTINO,
        TIPO_MOVIMIENTO,
        TIPO_DOCUMENTO,
        NRO_DOCUMENTO,
        COD_ITEM_2,
        PROVEEDOR,
        CANTIDAD,
        DOC_REF_1,
        DOC_REF_2,
        DOC_REF_3,
        DOC_REF_4,
        DOC_REF_5,
        FECHA_TRANSACCION
    FROM MOV_INVENTARIO
    WHERE (@Cod_Cia IS NULL OR COD_CIA = @Cod_Cia)
END

