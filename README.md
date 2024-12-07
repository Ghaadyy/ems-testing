# Event Management System

_Authors: Ghady Youssef, Antoine Karam, Georges Murr_

## Phase 3 - Testing

This project contains tests related to User, Event and Ticket Management.

## Getting Started

In order to run these tests, please execute the following commands.

```bash
dotnet restore
dotnet test --verbosity normal
```

## Project Structure

This project contains a `Services` and `Models` modules. The `Services` contains interfaces and mock implementations. The `Models` contains the Data Layer.

- `EventTest.cs`: handling all event-related actions.
- `UserTest.cs`: handling all user-related actions.
- `TicketTest.cs`: handling all ticket-related actions.
