Scaffold-DbContext Command

Use Scaffold-DbContext to create a model based on your existing database. The following parameters can be specified with Scaffold-DbContext in Package Manager Console:

Scaffold-DbContext [-Connection] [-Provider] [-OutputDir] [-Context] [-Schemas>] [-Tables>] 
                    [-DataAnnotations] [-Force] [-Project] [-StartupProject] [<CommonParameters>]

https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx

for example : 
#-----------------------
Scaffold-DbContext "Data Source=.;Initial Catalog=OSSECLINKDB;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir EntityModel -Force
#-----------------------

Reverse Engineering
https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli

PM> Scaffold-DbContext "Data SourceDESKTOP-VFCVBRF\SQLEXPRESS;Initial Catalog=ECOM_Web;User ID=sa;Password=pamoda" Microsoft.EntityFrameworkCore.SqlServer -OutputDir EntityModel -Force