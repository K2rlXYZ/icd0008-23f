
~~~ terminal
dotnet ef migrations --project DAL --startup-project WebApp add initial

dotnet ef database --project DAL --startup-project WebApp update
~~~

~~~ Terminal
dotnet aspnet-codegenerator razorpage -m DbGame -dc AppDbContext -udl -outDir Pages/DbGames --referenceScriptLibraries
~~~