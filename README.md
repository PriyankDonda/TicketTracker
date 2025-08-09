# TicketTracker

A full-stack ticket management system built with ASP.NET Core Web API and React.

## Features

- **Multi-role System**: Client, Agent, Admin roles with different permissions
- **Ticket Management**: Create, track, and manage support tickets
- **Comments & Attachments**: Communication and file sharing on tickets
- **Priority & Status Tracking**: Organize by priority (Low/Medium/High/Critical) and status
- **JWT Authentication**: Secure API access
- **RESTful API**: Clean API with Swagger documentation

## Tech Stack

**Backend (ASP.NET Core 8.0)**
- PostgreSQL with Entity Framework Core
- JWT Authentication & Role-based Authorization
- AutoMapper, Serilog, BCrypt
- Swagger/OpenAPI documentation

**Frontend (React 19 + Vite)**
- Tailwind CSS for styling
- Axios for API calls
- Context API for state management

## Quick Start

### Prerequisites
- .NET 8.0 SDK
- Node.js 18+
- PostgreSQL

### Backend Setup

1. **Configure Database**
   ```bash
   cd TicketTracker
   cp appsettings.example.json appsettings.json
   # Update connection string and JWT settings
   ```

2. **Run Migrations & Start API**
   ```bash
   dotnet restore
   dotnet ef database update
   dotnet run
   ```
   API available at: `https://localhost:7021` (Swagger: `/swagger`)

### Frontend Setup

1. **Install & Configure**
   ```bash
   cd client-app
   npm install
   # Create .env file with: VITE_BASE_URL=https://localhost:7021
   ```

2. **Start Development Server**
   ```bash
   npm run dev
   ```
   Frontend available at: `http://localhost:5173`

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register user
- `POST /api/auth/login` - Login user

### Tickets
- `GET /api/ticket/{id}` - Get ticket
- `POST /api/ticket/fetch` - Get filtered tickets (paginated)
- `POST /api/ticket` - Create ticket (Client role)
- `PUT /api/ticket/{id}` - Update ticket
- `DELETE /api/ticket/{id}` - Delete ticket

### Comments & Attachments
- `POST /api/comments` - Add comment
- `GET /api/comments/{ticketId}` - Get comments
- `POST /api/attachment` - Upload file
- `GET /api/attachment/{ticketId}` - Get attachments

## Configuration

**appsettings.json example:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tickettracker;Username=user;Password=pass"
  },
  "Jwt": {
    "Key": "YourSecretKeyMustBe32CharactersLong!",
    "Issuer": "TicketTracker",
    "Audience": "TicketTracker",
    "ExpiresInMinutes": 60
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:5173"]
  }
}
```

## Project Structure

```
TicketTracker/
├── Controllers/     # API endpoints
├── Models/         # Domain models (User, Ticket, Comment, Attachment)
├── Services/       # Business logic
├── DTOs/          # Data transfer objects
├── Data/          # EF DbContext
└── Helpers/       # Utilities & enums

client-app/
├── src/
│   ├── api/       # API integration
│   ├── config/    # Configuration
│   └── context/   # React Context
└── package.json
```

## User Roles

- **Client**: Create tickets, view own tickets, add comments
- **Agent**: View all tickets, update status, assign tickets
- **Admin**: Full system access, user management

## Development Status

⚠️ **Note**: Frontend is currently in early development with basic UI. Backend API is fully functional.

## Deployment

**Backend:**
```bash
dotnet publish -c Release
```

**Frontend:**
```bash
npm run build
```

## Future Features

- Email notifications
- Real-time updates (SignalR)
- Advanced reporting
- Mobile app

## License

MIT License 