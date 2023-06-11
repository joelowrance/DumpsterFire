Add Migration
`dotnet ef migrations add "SampleMigration" --project src\MegaMercado.Infrastructure --startup-project src\MegaMercado.WebApi --output-dir Persistence\Migrations`

Update database
`dotnet ef database update   --project src\MegaMercado.Infrastructure --startup-project src\MegaMercado.WebApi`

remove last migration
`dotnet ef migrations remove  --project src\MegaMercado.Infrastructure --startup-project src\MegaMercado.WebApi`

undo last migration
`dotnet ef database update "20230606233547_SoftDelete" --project src\MegaMercado.Infrastructure --startup-project src\MegaMercado.WebApi`



To Learn
- [x] Mediatr pipelines
  - https://garywoodfine.com/how-to-use-mediatr-pipeline-behaviours/
- [x] fluent validation
- [ ] redis
- [ ] faceted search
- [ ] EventStore db
- [ ] SQL w/ EF
  - [ ] Geography
  - [ ]Hierarchy
  - [ ] History Tables
- [ ] Lucene
- [ ] Background Service
- https://code-maze.com/cqrs-mediatr-in-aspnet-core/