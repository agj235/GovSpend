FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY GovSpend.Web/GovSpend.Web.csproj GovSpend.Web/

RUN dotnet restore GovSpend.Web/GovSpend.Web.csproj

COPY GovSpend.Web/ GovSpend.Web/

RUN dotnet publish GovSpend.Web/GovSpend.Web.csproj \
-c Release \
-o /app/publish \
--no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DemoMode=true
ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "GovSpend.Web.dll"]