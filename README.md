# ActuarialApplications

### Description
ASP.NET MVC application for actuarial applications.




## Setup

Databases
```bash 
dotnet ef database update -c LocalRateDbContext  
dotnet ef database update -c LocalLifeDbContext
```

APIs
```bash
docker run -it --rm -p 8004:8000 holmen1/smith-wilson-api:latest  
docker run -it --rm -p 8005:80 holmen1/estimate-liabilities-api:latest
```

## Dependencies

* Smith-Wilson API [github](https://github.com/holmen1/smith-wilson-par)  
Extrapolate Risk Free Rates from par swap rates

* EstimateLiabilities API [github](https://github.com/holmen1/EstimateLiabilitiesLife)  
Estimate liabilities for life insurance policies

### Entity Framework initial setup
```bash
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator --version 6.0
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version 6.0
```

```bash
dotnet ef migrations add InitialCreateRate -c LocalRateDbContext  

dotnet ef migrations add InitialCreateLife -c LocalLifeDbContext  
```

## TODO
[x] Database  
[x] RFR  
[ ] ESG  
[ ] Projections Life    
[ ] Projections P&C
