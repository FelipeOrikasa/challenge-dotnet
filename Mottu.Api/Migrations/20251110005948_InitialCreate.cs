using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remover foreign keys apenas se existirem (compatibilidade com Oracle)
            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (SELECT constraint_name FROM user_constraints WHERE constraint_name = 'FK_Localizacoes_Motos_MotoId' AND table_name = 'Localizacoes')
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Localizacoes"" DROP CONSTRAINT ""FK_Localizacoes_Motos_MotoId""';
                    END LOOP;
                END;
            ");

            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (SELECT constraint_name FROM user_constraints WHERE constraint_name = 'FK_Motos_Patios_PatioId' AND table_name = 'Motos')
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Motos"" DROP CONSTRAINT ""FK_Motos_Patios_PatioId""';
                    END LOOP;
                END;
            ");

            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (SELECT constraint_name FROM user_constraints WHERE constraint_name = 'FK_Patios_Filiais_FilialId' AND table_name = 'Patios')
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Patios"" DROP CONSTRAINT ""FK_Patios_Filiais_FilialId""';
                    END LOOP;
                END;
            ");

            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (SELECT constraint_name FROM user_constraints WHERE constraint_name = 'FK_Sensores_Patios_PatioId' AND table_name = 'Sensores')
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Sensores"" DROP CONSTRAINT ""FK_Sensores_Patios_PatioId""';
                    END LOOP;
                END;
            ");

            // Remover foreign keys que referenciam PK_Motos antes de tentar remover a primary key
            migrationBuilder.Sql(@"
                DECLARE
                    v_table_name VARCHAR2(128);
                    v_constraint_name VARCHAR2(128);
                BEGIN
                    FOR cur_rec IN (
                        SELECT constraint_name, table_name 
                        FROM user_constraints 
                        WHERE constraint_type = 'R' 
                        AND r_constraint_name = 'PK_Motos'
                    )
                    LOOP
                        BEGIN
                            EXECUTE IMMEDIATE 'ALTER TABLE ""' || cur_rec.table_name || '"" DROP CONSTRAINT ""' || cur_rec.constraint_name || '""';
                        EXCEPTION
                            WHEN OTHERS THEN NULL;
                        END;
                    END LOOP;
                END;
            ");

            // Remover primary keys apenas se existirem (e não forem referenciadas)
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_constraints 
                    WHERE constraint_name = 'PK_Motos' AND table_name = 'Motos' AND constraint_type = 'P'
                    AND NOT EXISTS (
                        SELECT 1 FROM user_constraints 
                        WHERE r_constraint_name = 'PK_Motos'
                    );
                    
                    IF v_count > 0 THEN
                        BEGIN
                            EXECUTE IMMEDIATE 'ALTER TABLE ""Motos"" DROP CONSTRAINT ""PK_Motos""';
                        EXCEPTION
                            WHEN OTHERS THEN NULL;
                        END;
                    END IF;
                END;
            ");

            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_constraints 
                    WHERE constraint_name = 'PK_Localizacoes' AND table_name = 'Localizacoes' AND constraint_type = 'P'
                    AND NOT EXISTS (
                        SELECT 1 FROM user_constraints 
                        WHERE r_constraint_name = 'PK_Localizacoes'
                    );
                    
                    IF v_count > 0 THEN
                        BEGIN
                            EXECUTE IMMEDIATE 'ALTER TABLE ""Localizacoes"" DROP CONSTRAINT ""PK_Localizacoes""';
                        EXCEPTION
                            WHEN OTHERS THEN NULL;
                        END;
                    END IF;
                END;
            ");

            // Remover colunas apenas se existirem
            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (SELECT column_name FROM user_tab_columns WHERE table_name = 'Motos' AND column_name = 'MotoId')
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Motos"" DROP COLUMN ""MotoId""';
                    END LOOP;
                END;
            ");

            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (SELECT column_name FROM user_tab_columns WHERE table_name = 'Localizacoes' AND column_name = 'LocalizacaoId')
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Localizacoes"" DROP COLUMN ""LocalizacaoId""';
                    END LOOP;
                END;
            ");

            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (SELECT column_name FROM user_tab_columns WHERE table_name = 'Filiais' AND column_name = 'Cidade')
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Filiais"" DROP COLUMN ""Cidade""';
                    END LOOP;
                END;
            ");

            // Renomear colunas apenas se a coluna antiga existir e a nova não existir
            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (
                        SELECT column_name FROM user_tab_columns 
                        WHERE table_name = 'Sensores' AND column_name = 'SensorId'
                        AND NOT EXISTS (SELECT 1 FROM user_tab_columns WHERE table_name = 'Sensores' AND column_name = 'Id')
                    )
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Sensores"" RENAME COLUMN ""SensorId"" TO ""Id""';
                    END LOOP;
                END;
            ");

            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (
                        SELECT column_name FROM user_tab_columns 
                        WHERE table_name = 'Patios' AND column_name = 'PatioId'
                        AND NOT EXISTS (SELECT 1 FROM user_tab_columns WHERE table_name = 'Patios' AND column_name = 'Id')
                    )
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Patios"" RENAME COLUMN ""PatioId"" TO ""Id""';
                    END LOOP;
                END;
            ");

            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (
                        SELECT column_name FROM user_tab_columns 
                        WHERE table_name = 'Localizacoes' AND column_name = 'DataHora'
                        AND NOT EXISTS (SELECT 1 FROM user_tab_columns WHERE table_name = 'Localizacoes' AND column_name = 'Timestamp')
                    )
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Localizacoes"" RENAME COLUMN ""DataHora"" TO ""Timestamp""';
                    END LOOP;
                END;
            ");

            migrationBuilder.Sql(@"
                BEGIN
                    FOR cur_rec IN (
                        SELECT column_name FROM user_tab_columns 
                        WHERE table_name = 'Filiais' AND column_name = 'FilialId'
                        AND NOT EXISTS (SELECT 1 FROM user_tab_columns WHERE table_name = 'Filiais' AND column_name = 'Id')
                    )
                    LOOP
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Filiais"" RENAME COLUMN ""FilialId"" TO ""Id""';
                    END LOOP;
                END;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Sensores",
                type: "NVARCHAR2(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(100)",
                oldMaxLength: 100,
                oldNullable: true);

            // Adicionar coluna Ativo apenas se não existir
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tab_columns 
                    WHERE table_name = 'Sensores' AND column_name = 'Ativo';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Sensores"" ADD ""Ativo"" NUMBER(10) DEFAULT 0 NOT NULL';
                    END IF;
                END;
            ");

            // Adicionar coluna CapacidadeMaxima apenas se não existir
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tab_columns 
                    WHERE table_name = 'Patios' AND column_name = 'CapacidadeMaxima';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Patios"" ADD ""CapacidadeMaxima"" NUMBER(10) DEFAULT 0 NOT NULL';
                    END IF;
                END;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Motos",
                type: "NVARCHAR2(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "PatioId",
                table: "Motos",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            // Adicionar coluna Modelo em Motos apenas se não existir
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tab_columns 
                    WHERE table_name = 'Motos' AND column_name = 'Modelo';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Motos"" ADD ""Modelo"" NVARCHAR2(100) DEFAULT ''Modelo não informado'' NOT NULL';
                    END IF;
                END;
            ");

            // Adicionar coluna Id em Motos apenas se não existir
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tab_columns 
                    WHERE table_name = 'Motos' AND column_name = 'Id';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Motos"" ADD ""Id"" RAW(16) DEFAULT HEXTORAW(''00000000000000000000000000000000'') NOT NULL';
                    END IF;
                END;
            ");

            // Remover coluna MotoId de Localizacoes se existir (não deveria existir no modelo atual)
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tab_columns 
                    WHERE table_name = 'Localizacoes' AND column_name = 'MotoId';
                    
                    IF v_count > 0 THEN
                        -- Primeiro remover constraint se existir
                        FOR cur_rec IN (SELECT constraint_name FROM user_constraints WHERE table_name = 'Localizacoes' AND constraint_name LIKE '%MotoId%')
                        LOOP
                            EXECUTE IMMEDIATE 'ALTER TABLE ""Localizacoes"" DROP CONSTRAINT ""' || cur_rec.constraint_name || '""';
                        END LOOP;
                        -- Depois remover a coluna
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Localizacoes"" DROP COLUMN ""MotoId""';
                    END IF;
                END;
            ");

            // Adicionar coluna Id em Localizacoes apenas se não existir
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tab_columns 
                    WHERE table_name = 'Localizacoes' AND column_name = 'Id';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Localizacoes"" ADD ""Id"" RAW(16) DEFAULT HEXTORAW(''00000000000000000000000000000000'') NOT NULL';
                    END IF;
                END;
            ");

            // Adicionar colunas Latitude e Longitude apenas se não existirem
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tab_columns 
                    WHERE table_name = 'Localizacoes' AND column_name = 'Latitude';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Localizacoes"" ADD ""Latitude"" decimal(10,8) DEFAULT 0.0 NOT NULL';
                    END IF;
                END;
            ");

            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tab_columns 
                    WHERE table_name = 'Localizacoes' AND column_name = 'Longitude';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Localizacoes"" ADD ""Longitude"" decimal(11,8) DEFAULT 0.0 NOT NULL';
                    END IF;
                END;
            ");

            // Adicionar coluna Endereco com valor default para tabelas que já têm dados
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tab_columns 
                    WHERE table_name = 'Filiais' AND column_name = 'Endereco';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Filiais"" ADD ""Endereco"" NVARCHAR2(200) DEFAULT ''Endereço não informado'' NOT NULL';
                    END IF;
                END;
            ");

            // Atualizar valores de Id para registros existentes antes de criar a primary key
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count FROM ""Motos"";
                    IF v_count > 0 THEN
                        -- Atualizar Ids existentes com GUIDs únicos
                        FOR cur_rec IN (SELECT ROWID as r_id FROM ""Motos"" WHERE ""Id"" = HEXTORAW('00000000000000000000000000000000'))
                        LOOP
                            EXECUTE IMMEDIATE 'UPDATE ""Motos"" SET ""Id"" = SYS_GUID() WHERE ROWID = :r_id' USING cur_rec.r_id;
                        END LOOP;
                    END IF;
                END;
            ");

            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count FROM ""Localizacoes"";
                    IF v_count > 0 THEN
                        -- Atualizar Ids existentes com GUIDs únicos
                        FOR cur_rec IN (SELECT ROWID as r_id FROM ""Localizacoes"" WHERE ""Id"" = HEXTORAW('00000000000000000000000000000000'))
                        LOOP
                            EXECUTE IMMEDIATE 'UPDATE ""Localizacoes"" SET ""Id"" = SYS_GUID() WHERE ROWID = :r_id' USING cur_rec.r_id;
                        END LOOP;
                    END IF;
                END;
            ");

            // Adicionar primary keys apenas se não existirem
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_constraints 
                    WHERE constraint_name = 'PK_Motos' AND table_name = 'Motos' AND constraint_type = 'P';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Motos"" ADD CONSTRAINT ""PK_Motos"" PRIMARY KEY (""Id"")';
                    END IF;
                END;
            ");

            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_constraints 
                    WHERE constraint_name = 'PK_Localizacoes' AND table_name = 'Localizacoes' AND constraint_type = 'P';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'ALTER TABLE ""Localizacoes"" ADD CONSTRAINT ""PK_Localizacoes"" PRIMARY KEY (""Id"")';
                    END IF;
                END;
            ");

            // Criar tabela Entregadores apenas se não existir
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tables 
                    WHERE table_name = 'Entregadores';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE '
                            CREATE TABLE ""Entregadores"" (
                                ""Id"" RAW(16) NOT NULL,
                                ""Nome"" NVARCHAR2(150) NOT NULL,
                                ""CNPJ"" NVARCHAR2(18) NOT NULL,
                                ""DataNascimento"" TIMESTAMP(7) NOT NULL,
                                ""CNH"" NVARCHAR2(20) NOT NULL,
                                ""TipoCNH"" NVARCHAR2(2) NOT NULL,
                                ""ImagemCNH"" NVARCHAR2(2000) NOT NULL,
                                CONSTRAINT ""PK_Entregadores"" PRIMARY KEY (""Id"")
                            )
                        ';
                    END IF;
                END;
            ");

            // Criar tabela Locacoes apenas se não existir
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_tables 
                    WHERE table_name = 'Locacoes';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE '
                            CREATE TABLE ""Locacoes"" (
                                ""Id"" RAW(16) NOT NULL,
                                ""EntregadorId"" RAW(16) NOT NULL,
                                ""MotoId"" RAW(16) NOT NULL,
                                ""DataInicio"" TIMESTAMP(7) NOT NULL,
                                ""DataTerminoPrevista"" TIMESTAMP(7) NOT NULL,
                                ""DataTerminoEfetiva"" TIMESTAMP(7),
                                ""DiasContratados"" NUMBER(10) NOT NULL,
                                ""CustoDiarioContratado"" decimal(18,2) NOT NULL,
                                ""CustoTotalPrevisto"" decimal(18,2) NOT NULL,
                                ""CustoFinal"" decimal(18,2) NOT NULL,
                                ""Status"" NUMBER(10) NOT NULL,
                                CONSTRAINT ""PK_Locacoes"" PRIMARY KEY (""Id"")
                            )
                        ';
                    END IF;
                END;
            ");

            // Criar índices apenas se não existirem
            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_indexes 
                    WHERE index_name = 'IX_Entregadores_CNH';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'CREATE UNIQUE INDEX ""IX_Entregadores_CNH"" ON ""Entregadores"" (""CNH"")';
                    END IF;
                END;
            ");

            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_indexes 
                    WHERE index_name = 'IX_Entregadores_CNPJ';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'CREATE UNIQUE INDEX ""IX_Entregadores_CNPJ"" ON ""Entregadores"" (""CNPJ"")';
                    END IF;
                END;
            ");

            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_indexes 
                    WHERE index_name = 'IX_Locacoes_EntregadorId';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'CREATE INDEX ""IX_Locacoes_EntregadorId"" ON ""Locacoes"" (""EntregadorId"")';
                    END IF;
                END;
            ");

            migrationBuilder.Sql(@"
                DECLARE
                    v_count NUMBER;
                BEGIN
                    SELECT COUNT(*) INTO v_count 
                    FROM user_indexes 
                    WHERE index_name = 'IX_Locacoes_MotoId';
                    
                    IF v_count = 0 THEN
                        EXECUTE IMMEDIATE 'CREATE INDEX ""IX_Locacoes_MotoId"" ON ""Locacoes"" (""MotoId"")';
                    END IF;
                END;
            ");

            // FK_Localizacoes_Motos_MotoId removida - Localizacao não tem relação direta com Moto

            migrationBuilder.AddForeignKey(
                name: "FK_Motos_Patios_PatioId",
                table: "Motos",
                column: "PatioId",
                principalTable: "Patios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Patios_Filiais_FilialId",
                table: "Patios",
                column: "FilialId",
                principalTable: "Filiais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensores_Patios_PatioId",
                table: "Sensores",
                column: "PatioId",
                principalTable: "Patios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Localizacoes_Motos_MotoId",
                table: "Localizacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Motos_Patios_PatioId",
                table: "Motos");

            migrationBuilder.DropForeignKey(
                name: "FK_Patios_Filiais_FilialId",
                table: "Patios");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensores_Patios_PatioId",
                table: "Sensores");

            migrationBuilder.DropTable(
                name: "Locacoes");

            migrationBuilder.DropTable(
                name: "Entregadores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Motos",
                table: "Motos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Localizacoes",
                table: "Localizacoes");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Sensores");

            migrationBuilder.DropColumn(
                name: "CapacidadeMaxima",
                table: "Patios");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Motos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Localizacoes");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Localizacoes");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Localizacoes");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Filiais");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Sensores",
                newName: "SensorId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Patios",
                newName: "PatioId");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Localizacoes",
                newName: "DataHora");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Filiais",
                newName: "FilialId");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Sensores",
                type: "NVARCHAR2(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Placa",
                table: "Motos",
                type: "NVARCHAR2(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "PatioId",
                table: "Motos",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MotoId",
                table: "Motos",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0)
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AlterColumn<int>(
                name: "MotoId",
                table: "Localizacoes",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(Guid),
                oldType: "RAW(16)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocalizacaoId",
                table: "Localizacoes",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0)
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Filiais",
                type: "NVARCHAR2(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Motos",
                table: "Motos",
                column: "MotoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Localizacoes",
                table: "Localizacoes",
                column: "LocalizacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Localizacoes_Motos_MotoId",
                table: "Localizacoes",
                column: "MotoId",
                principalTable: "Motos",
                principalColumn: "MotoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Motos_Patios_PatioId",
                table: "Motos",
                column: "PatioId",
                principalTable: "Patios",
                principalColumn: "PatioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patios_Filiais_FilialId",
                table: "Patios",
                column: "FilialId",
                principalTable: "Filiais",
                principalColumn: "FilialId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensores_Patios_PatioId",
                table: "Sensores",
                column: "PatioId",
                principalTable: "Patios",
                principalColumn: "PatioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
