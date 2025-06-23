# Build the lambda function

cd lambda
dotnet build
dotnet publish -c Release -o publish

 
### List All Events
```graphql
query {
  listEvents {
    id
    name
    date
    maxAttendees
  }
}
```

### Sign Up for Event
```graphql
mutation {
  signupForEvent(input: {
    eventId: 1
    name: "John Doe"
    email: "john@example.com"
  }) {
    success
    message
    participant {
      id
      name
      email
    }
  }
}
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