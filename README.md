# ActuarialApplications

### Description


## Entity Framework

dotnet ef migrations add InitialCreate -c LocalDbContext
dotnet ef database update -c LocalDbContext

dotnet ef migrations add InitialCreate -c LocalLifeDbContext
dotnet ef database update -c LocalLifeDbContext


```bash
dotnet ef database update
```

```bash
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator --version 6.0
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version 6.0
```

```bash
dotnet ef migrations add InitialCreate  
```

Run Cashflow API
```bash
docker run -it --rm -p 8005:80 holmen1/estimate-liabilities-api:latest
```

```bash

Pull and run docker image smith-wilson-api
```bash
 ./run.sh
```
Unable to find image 'holmen1/smith-wilson-api:latest' locally  
latest: Pulling from holmen1/smith-wilson-api  
3689b8de819b: Already exists  
af8cd5f36469: Already exists   
80384e04044f: Already exists   
9fe06c6a3fe3: Already exists   
f0712d0bdb15: Already exists   
0b8877ad7468: Already exists   
7065785fc5f9: Already exists   
d6db181673bf: Already exists   
c428a67ff4e2: Already exists   
776b6fa606d8: Already exists   
Digest: sha256:75396d15bf01bdc864e727aaa149824a19c7ab68554678b743413c885a66e691  
Status: Downloaded newer image for holmen1/smith-wilson-api:latest  
64336dd0f4a57b2045b7abc4a2de3f443e8025334ea0e2b5b7e69c8c95b318b6  


Pull and run docker image estimate-liabilities-api
```bash
docker run -it --rm -p 8005:80 holmen1/estimate-liabilities-api:latest
```




## Projects

* ActuarialApplications (ASP.NET Web app)


## TODO
[x] Database  
[x] RFR  
[ ] ESG  
[ ] Projections Life    
[ ] Projections P&C
