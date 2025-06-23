# Build the lambda function

cd lambda
dotnet build
dotnet publish -c Release -o publish

```

### Get Event Participants
```graphql
query {
  listParticipants(eventId: 1) {
    id
    name
    email
  }
}
```

### Cancel Signup
```graphql
mutation {
  cancelSignup(eventId: 1, email: "john@example.com") {
    success
    message
  }
}
```