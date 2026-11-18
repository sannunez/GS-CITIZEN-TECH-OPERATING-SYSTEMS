using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace Aja.HelperApp.Api.Migrations
{
    public partial class ContasDatasEFeedDemonstrativo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "usu_email",
                table: "usu_usuarios",
                type: "varchar(254)",
                maxLength: 254,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "usu_senha_hash",
                table: "usu_usuarios",
                type: "varchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "pos_criada_em",
                table: "pos_postagens",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "pos_postagens",
                keyColumn: "pos_id",
                keyValue: 1L,
                column: "pos_criada_em",
                value: new DateTime(2026, 11, 12, 9, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "pos_postagens",
                keyColumn: "pos_id",
                keyValue: 2L,
                column: "pos_criada_em",
                value: new DateTime(2026, 11, 12, 8, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "pos_postagens",
                keyColumn: "pos_id",
                keyValue: 3L,
                column: "pos_criada_em",
                value: new DateTime(2026, 11, 12, 7, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.InsertData(
                table: "pos_postagens",
                columns: new[] { "pos_id", "pos_criada_em", "pos_imagem_url", "pos_texto", "pos_usuario_id" },
                values: new object[,]
                {
                    { 7L, new DateTime(2026, 11, 12, 11, 45, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 7: discussão sobre comunicação acessível a pessoas surdas no Parque Doroteia, São Paulo.", 1L },
                    { 8L, new DateTime(2026, 11, 12, 12, 0, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 8: discussão sobre acesso seguro às praças no Parque Doroteia, São Paulo.", 2L },
                    { 9L, new DateTime(2026, 11, 12, 12, 15, 0, 0, DateTimeKind.Utc), "https://placehold.co/900x600/png?text=Caso+demonstrativo+9", "Caso demonstrativo 9: discussão sobre participação de pessoas cegas nas melhorias no Parque Doroteia, São Paulo.", 3L },
                    { 14L, new DateTime(2026, 11, 12, 13, 30, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 14: discussão sobre calçadas livres de obstáculos no Parque Doroteia, São Paulo.", 1L },
                    { 15L, new DateTime(2026, 11, 12, 13, 45, 0, 0, DateTimeKind.Utc), "https://placehold.co/900x600/png?text=Caso+demonstrativo+15", "Caso demonstrativo 15: discussão sobre sinalização visual nos serviços públicos no Parque Doroteia, São Paulo.", 2L },
                    { 16L, new DateTime(2026, 11, 12, 14, 0, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 16: discussão sobre orientação tátil nas travessias no Parque Doroteia, São Paulo.", 3L }
                });

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 1L,
                columns: new[] { "usu_email", "usu_senha_hash" },
                values: new object[] { "ana@aja.example", "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow==" });

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 2L,
                columns: new[] { "usu_email", "usu_senha_hash" },
                values: new object[] { "bruno@aja.example", "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow==" });

            migrationBuilder.UpdateData(
                table: "usu_usuarios",
                keyColumn: "usu_id",
                keyValue: 3L,
                columns: new[] { "usu_email", "usu_senha_hash" },
                values: new object[] { "carla@aja.example", "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow==" });

            migrationBuilder.InsertData(
                table: "usu_usuarios",
                columns: new[] { "usu_id", "usu_biografia", "usu_email", "usu_nome", "usu_senha_hash" },
                values: new object[,]
                {
                    { 4L, "Perfil fictício do Parque Doroteia.", "demo4@aja.example", "Lucas Oliveira", "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow==" },
                    { 5L, "Perfil fictício do Parque Doroteia.", "demo5@aja.example", "Mariana Costa", "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow==" },
                    { 6L, "Perfil fictício do Parque Doroteia.", "demo6@aja.example", "Pedro Alves", "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow==" },
                    { 7L, "Perfil fictício do Parque Doroteia.", "demo7@aja.example", "Juliana Souza", "AQAAAAIAAYagAAAAEGnjthc2h5n9E4FJCAnCT9jZO/KdPNIpn2Z6JICQIDM3ur/KdmGvbT+cMI0/xgHEow==" }
                });

            migrationBuilder.InsertData(
                table: "pos_postagens",
                columns: new[] { "pos_id", "pos_criada_em", "pos_imagem_url", "pos_texto", "pos_usuario_id" },
                values: new object[,]
                {
                    { 4L, new DateTime(2026, 11, 12, 11, 0, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 4: discussão sobre calçadas livres de obstáculos no Parque Doroteia, São Paulo.", 5L },
                    { 5L, new DateTime(2026, 11, 12, 11, 15, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 5: discussão sobre sinalização visual nos serviços públicos no Parque Doroteia, São Paulo.", 6L },
                    { 6L, new DateTime(2026, 11, 12, 11, 30, 0, 0, DateTimeKind.Utc), "https://placehold.co/900x600/png?text=Caso+demonstrativo+6", "Caso demonstrativo 6: discussão sobre orientação tátil nas travessias no Parque Doroteia, São Paulo.", 7L },
                    { 10L, new DateTime(2026, 11, 12, 12, 30, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 10: discussão sobre informações de transporte em formato visual no Parque Doroteia, São Paulo.", 4L },
                    { 11L, new DateTime(2026, 11, 12, 12, 45, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 11: discussão sobre continuidade do piso tátil no Parque Doroteia, São Paulo.", 5L },
                    { 12L, new DateTime(2026, 11, 12, 13, 0, 0, 0, DateTimeKind.Utc), "https://placehold.co/900x600/png?text=Caso+demonstrativo+12", "Caso demonstrativo 12: discussão sobre atendimento inclusivo no Parque Doroteia, São Paulo.", 6L },
                    { 13L, new DateTime(2026, 11, 12, 13, 15, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 13: discussão sobre travessias acessíveis no Parque Doroteia, São Paulo.", 7L },
                    { 17L, new DateTime(2026, 11, 12, 14, 15, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 17: discussão sobre comunicação acessível a pessoas surdas no Parque Doroteia, São Paulo.", 4L },
                    { 18L, new DateTime(2026, 11, 12, 14, 30, 0, 0, DateTimeKind.Utc), "https://placehold.co/900x600/png?text=Caso+demonstrativo+18", "Caso demonstrativo 18: discussão sobre acesso seguro às praças no Parque Doroteia, São Paulo.", 5L },
                    { 19L, new DateTime(2026, 11, 12, 14, 45, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 19: discussão sobre participação de pessoas cegas nas melhorias no Parque Doroteia, São Paulo.", 6L },
                    { 20L, new DateTime(2026, 11, 12, 15, 0, 0, 0, DateTimeKind.Utc), null, "Caso demonstrativo 20: discussão sobre informações de transporte em formato visual no Parque Doroteia, São Paulo.", 7L }
                });

            migrationBuilder.Sql("UPDATE usu_usuarios SET usu_email = CONCAT('legacy-', usu_id, '@aja.invalid') WHERE usu_email = '';");

            migrationBuilder.CreateIndex(
                name: "ux_usu_email",
                table: "usu_usuarios",
                column: "usu_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_pos_data_id",
                table: "pos_postagens",
                columns: new[] { "pos_criada_em", "pos_id" });
            PreservarRegistros(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_usu_email",
                table: "usu_usuarios");

            migrationBuilder.DropIndex(
                name: "ix_pos_data_id",
                table: "pos_postagens");

            migrationBuilder.DropColumn(
                name: "usu_email",
                table: "usu_usuarios");

            migrationBuilder.DropColumn(
                name: "usu_senha_hash",
                table: "usu_usuarios");

            migrationBuilder.DropColumn(
                name: "pos_criada_em",
                table: "pos_postagens");
        }

        private static void PreservarRegistros(MigrationBuilder builder)
        {
            foreach (var insert in System.Linq.Enumerable.ToArray(System.Linq.Enumerable.OfType<Microsoft.EntityFrameworkCore.Migrations.Operations.InsertDataOperation>(builder.Operations)))
            {
                var sql = new System.Text.StringBuilder();
                for (var row = 0; row < insert.Values.GetLength(0); row++)
                {
                    var valores = new string[insert.Columns.Length];
                    for (var column = 0; column < insert.Columns.Length; column++)
                        valores[column] = Literal(insert.Values[row, column]);
                    var colunas = string.Join(", ", System.Linq.Enumerable.Select(insert.Columns, column => $"`{column}`"));
                    sql.AppendLine($"INSERT INTO `{insert.Table}` ({colunas}) SELECT {string.Join(", ", valores)} WHERE NOT EXISTS (SELECT 1 FROM `{insert.Table}` WHERE `{insert.Columns[0]}` = {valores[0]});");
                }
                builder.Operations[builder.Operations.IndexOf(insert)] = new Microsoft.EntityFrameworkCore.Migrations.Operations.SqlOperation { Sql = sql.ToString() };
            }
        }

        private static string Literal(object value) => value switch
        {
            null => "NULL",
            string texto => "'" + MySqlConnector.MySqlHelper.EscapeString(texto) + "'",
            DateTime data => "'" + data.ToString("yyyy-MM-dd HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture) + "'",
            _ => Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture)
        };
    }
}
