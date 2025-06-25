## GraphQl Endpoint
https://svxn3yuy30.execute-api.eu-north-1.amazonaws.com/graphql


## Useful commands

* `dotnet build src` compile this app
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template


# Build the lambda function

* `cd lambda`
* `dotnet build`
* `dotnet publish -c Release -o ./publish --self-contained false`

# Test lambda
* `cd lambda`
* `dotnet test --verbosity normal`


### Enhancements
* Outsource test in new project

### Sources
* ChilliCream YouTube

## Testing

```bash
dotnet test --verbosity normal
```

### Available Queries

#### List Events
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

#### List Participants (Public)
```graphql
query {
  listParticipants(eventId: 1) {
    id
    name
    email
    eventId
  }
}
```

#### List Participants (Authorized)
```graphql
query {
  listParticipantsAuthorized(eventId: 1) {
    id
    name
    email
    eventId
  }
}
```

#### Create Event
```graphql
mutation {
  createEvent(input: {
    name: "Lego Life Event"
    date: "2024-01-15T10:00:00Z"
    maxAttendees: 200
  }) {
    success
    message
    event {
      id
      name
      date
      maxAttendees
    }
  }
}
```

#### Update Event
```graphql
mutation {
  updateEvent(id: 1, input: {
    name: "Updated Lego Event Updated"
    date: "2024-01-20T10:00:00Z"
    maxAttendees: 250
  }) {
    success
    message
    event {
      id
      name
      date
      maxAttendees
    }
  }
}
```

#### Delete Event
```graphql
mutation {
  deleteEvent(id: 1) {
    success
    message
  }
}
```

#### Signup for Event
```graphql
mutation {
  signupForEvent(input: {
    eventId: 1
    name: "Botond T"
    email: "bt@example.com"
  }) {
    success
    message
    participant {
      id
      name
      email
      eventId
    }
  }
}
```

#### Delete All Participants by Email
```graphql
mutation {
  deleteAllParticipantsByEmail(input: {
    email: "bt@example.com"
  }) {
    success
    message
  }
}
```