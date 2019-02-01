dotnet tool uninstall --global packater
dotnet build
dotnet pack
dotnet tool install --global --add-source .\nupkg packater
packater