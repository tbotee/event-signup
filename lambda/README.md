# LEGO Event Signup Lambda Function

A simple Lambda function that provides a health check endpoint for the LEGO Event Signup system.

## Features

- **Health Check**: Simple health endpoint to verify the service is running
- **CORS Support**: Cross-origin request support for web applications
- **Error Handling**: Proper error responses and logging

## Endpoints

### GET /health
Returns a health status response:
```json
{
  "status": "OK",
  "timestamp": "2024-12-20T12:30:00Z",
  "service": "LEGO Event Signup API"
}
```

## Building

```bash
cd lambda
dotnet build
dotnet publish -c Release -o publish
```

## Deployment

The function is deployed via CDK from the main project. The CDK stack packages this lambda directory and deploys it to AWS Lambda. 