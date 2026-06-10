FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Imobiliaria/Imobiliaria.csproj Imobiliaria/
RUN dotnet restore Imobiliaria/Imobiliaria.csproj

COPY . .
RUN dotnet publish Imobiliaria/Imobiliaria.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV PORT=8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Imobiliaria.dll"]
