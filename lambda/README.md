# Build the lambda function

cd lambda
dotnet build
dotnet publish -c Release -o ./publish --self-contained false

cdk publish