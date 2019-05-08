# .NET Core 2.1 をベースイメージとする
FROM microsoft/dotnet:2.1-sdk
# 作業ディレクリの指定
WORKDIR /app

# プロジェクトをコピーして別のレイヤーで復元します。
COPY *.csproj ./
RUN dotnet restore

# 全てコピーしてビルドします。
COPY . ./
RUN dotnet publish -c Release -o out

ENTRYPOINT ["dotnet", "out/HelloVueTS.dll"]