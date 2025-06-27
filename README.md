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
* mapperly

### Sources
* ChilliCream YouTube https://www.youtube.com/@chillicreamtv
* ChilliCream gitgub https://github.com/ChilliCream/graphql-workshop

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

```graphql
query {
  events() {
    nodes {
      id
      date
      participants(first: 5) {
        nodes{
          id
          name
          email
        }
      }
    }
  }
}
```

```graphql
mutation{
  createEvent(input:  {
     name: "Event name I"
     date: "2025-11-20T00:00:00.000Z"
     maxAttendees: 0
  }) {
    event {
      id
      maxAttendees
    }
  }
}
```

```graphql
mutation {
  updateEvent(input: {
     id: 4
     maxAttendees: 11
  }) {
    event {
      maxAttendees
    }
    errors {
     ... on Error {
      message
     }
    }
  }
}
```

```graphql
mutation{
  deleteEvent(input:  {
     eventId: 4
  }) {
    int
    errors {
     ... on Error {
      message
     }
    }
  }
}
```
```graphql
mutation{
  createParticipant(input:  {
     name: "participant 1"
     email: "br@example.com"
  }) {
    participant {
      id
      name
      email
    }
  }
}