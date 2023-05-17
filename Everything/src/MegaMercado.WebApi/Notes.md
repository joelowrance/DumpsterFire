Add Migration
`dotnet ef migrations add "SampleMigration" --project src\MegaMercado.Infrastructure --startup-project src\MegaMercado.WebApi --output-dir Persistence\Migrations`

Update database
`dotnet ef database update   --project src\MegaMercado.Infrastructure --startup-project src\MegaMercado.WebApi` 