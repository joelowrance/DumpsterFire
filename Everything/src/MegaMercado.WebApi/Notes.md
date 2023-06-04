Add Migration
`dotnet ef migrations add "SampleMigration" --project src\MegaMercado.Infrastructure --startup-project src\MegaMercado.WebApi --output-dir Persistence\Migrations`

Update database
`dotnet ef database update   --project src\MegaMercado.Infrastructure --startup-project src\MegaMercado.WebApi`

Undo last migration
`dotnet ef migrations remove  --project src\MegaMercado.Infrastructure --startup-project src\MegaMercado.WebApi`


To Learn
- Mediatr pipelines
- fluent validation
- redis
- faceted search
- EventStore db
- SQL w/ EF
  - Geography
  - Hierarchy
  - History Tables
- Lucene
  - Background Service
