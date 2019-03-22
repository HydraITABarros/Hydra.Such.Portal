@echo off
set user="such_portal_user"
set password="SuchPW.2K17"
cd .. 
dotnet ef dbcontext scaffold "Server=10.101.1.10\sqlnav;initial catalog=EvolutionWEB;user id=%user%;password=%password%;" Microsoft.EntityFrameworkCore.SqlServer -o Database -f