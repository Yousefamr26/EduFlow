# EduFlow Waiting List System - Implementation Guide

## Overview
A comprehensive waiting list system has been integrated with the booking module to handle session capacity management and automatic queue processing. When a session reaches full capacity, users are automatically added to a queue in order of request time. When a booked user cancels, the system automatically books the first person from the waiting list.

---

## Components Overview

### 1. Domain Entity - `WaitingListEntry`
**Location:** `EduFlow.Domain/Entities/WaitingListEntry.cs`

Represents a user's position in a session's waiting list.

**Properties:**
- `Id`: Unique identifier
- `StudentId`: Foreign key to the student
- `SessionId`: Foreign key to the session
- `RequestTime`: When the user joined the waiting list
- `QueuePosition`: Current position in the queue (1-based)
- `CreatedAt` / `UpdatedAt`: Audit timestamps

**Key Features:**
- Unique constraint on (StudentId, SessionId) - prevents duplicate queue entries
- Indexed by SessionId for efficient queue queries
- Indexed by RequestTime for maintaining queue order

---

### 2. Database Configuration
**Location:** `EduFlow.Infrastructure/Persistence/Configurations/WaitingListEntryConfiguration.cs`

Defines the database schema and relationships:
- Foreign key constraints with cascade delete
- Unique index on (StudentId, SessionId)
- Indexes on SessionId and RequestTime for query performance

**Migration:**
- File: `20260415000001_AddWaitingListSystem.cs`
- Creates `WaitingListEntries` table with all necessary indexes

---

### 3. Repository Pattern

#### IWaitingListRepository Interface
**Location:** `EduFlow.Application/Interfaces/Repositories/IWaitingListRepository.cs`

**Methods:**
```csharp
Task<WaitingListEntry> GetWaitingListEntryAsync(string studentId, int sessionId)
Task<bool> IsInWaitingListAsync(string studentId, int sessionId)
Task<IEnumerable<WaitingListEntry>> GetWaitingListBySessionIdAsync(int sessionId)
Task<WaitingListEntry> GetFirstInQueueAsync(int sessionId)
Task RemoveFromWaitingListAsync(string studentId, int sessionId)
Task UpdateQueuePositionsAsync(int sessionId)
```

#### WaitingListRepository Implementation
**Location:** `EduFlow.Infrastructure/Repositories/WaitingListRepository.cs`

Provides data access operations for the waiting list, including:
- Checking for duplicate entries
- Retrieving ordered queue lists
- Updating queue positions
- Removing entries

---

### 4. Unit of Work Pattern
**Updated Files:**
- `EduFlow.Application/Interfaces/UnitOfWork/IUnitOfWork.cs`
- `EduFlow.Infrastructure/UnitOfWork/UnitOfWork.cs`

Added `IWaitingListRepository WaitingList { get; }` property to enable centralized repository access.

---

### 5. Services

#### IAutoBookingService / AutoBookingService
**Location:** `EduFlow.Infrastructure/Features/WaitingList/Services/AutoBookingService.cs`

**Purpose:** Automatically books the next person from the waiting list when a slot becomes available.

**Key Logic:**
1. Verifies session availability and capacity
2. Gets the first person in the queue
3. Checks for time conflicts (if conflict, removes from list and tries next person)
4. Creates a booking
5. Removes from waiting list
6. Updates queue positions
7. Sends notification to the user
8. All operations wrapped in a transaction for consistency

**Error Handling:** Comprehensive transaction rollback on failures

---

### 6. Queries (CQRS Pattern)

#### GetSessionWaitingListQuery
**Location:** `EduFlow.Infrastructure/Features/WaitingList/Queries/GetSessionWaitingListQuery.cs`
**Handler:** `GetSessionWaitingListQueryHandler.cs`

Retrieves the complete waiting list for a session with user details.

**Response:**
```json
{
  "sessionId": 1,
  "sessionTitle": "Advanced Mathematics",
  "sessionDateTime": "2025-01-15T10:00:00Z",
  "totalWaiting": 3,
  "entries": [
    {
      "id": 1,
      "studentId": "user-123",
      "studentName": "John Doe",
      "sessionId": 1,
      "sessionTitle": "Advanced Mathematics",
      "requestTime": "2025-01-14T14:30:00Z",
      "queuePosition": 1
    }
  ]
}
```

#### GetStudentWaitingListPositionsQuery
**Location:** `EduFlow.Infrastructure/Features/WaitingList/Queries/GetStudentWaitingListPositionsQuery.cs`
**Handler:** `GetStudentWaitingListPositionsQueryHandler.cs`

Retrieves all sessions where a specific student is on the waiting list.

**Response:**
```json
[
  {
    "sessionId": 1,
    "sessionTitle": "Advanced Mathematics",
    "sessionDateTime": "2025-01-15T10:00:00Z",
    "queuePosition": 2,
    "totalInQueue": 5
  }
]
```

---

### 7. Commands (CQRS Pattern)

#### AddToWaitingListCommand
**Location:** `EduFlow.Infrastructure/Features/WaitingList/Commands/AddToWaitingListCommand.cs`
**Handler:** `AddToWaitingListCommandHandler.cs`

Adds a student to the waiting list for a session.

**Validations:**
- Session must exist and not be canceled
- Student cannot already be on the waiting list
- Student cannot already be booked
- Prevents duplicate entries with unique constraint

**Returns:** Waiting list entry ID

---

### 8. Updated Booking System

#### BookSessionCommandHandler Enhancement
**Location:** `EduFlow.Infrastructure/Features/BookingSystem/Command/BookSessionCommandHandler.cs`

**Changes:**
- When a session is full, instead of throwing an exception, automatically adds student to waiting list
- Returns negative ID to indicate waiting list entry (vs. positive for booking)
- Original validation logic preserved for non-full sessions

**Behavior:**
- Full capacity → Add to waiting list → Return negative entry ID
- Available slots → Create booking → Return positive booking ID

#### CancelBookingCommandHandler Enhancement
**Location:** `EduFlow.Infrastructure/Features/BookingSystem/Command/CancelBookingCommandHandler.cs`

**Changes:**
- After canceling a booking, automatically triggers auto-booking
- `IAutoBookingService` injected as dependency
- Notification still created for cancellation

**Behavior:**
1. Cancel booking
2. Decrement session.BookedCount
3. Create cancellation notification
4. Auto-book next person from waiting list
5. Send notification to newly booked student

---

### 9. Controllers

#### WaitingListController
**Location:** `EduFlow/Controllers/WaitingListController.cs`

**Endpoints:**

##### 1. Join Waiting List
```
POST /api/waiting-list/join
Content-Type: application/json

{
  "sessionId": 1
}
```

**Response (Success):**
```json
{
  "waitingListEntryId": 5,
  "message": "Added to waiting list"
}
```

**Response (Session Full - via BookingController):**
```json
{
  "waitingListEntryId": 5,
  "status": "Waiting",
  "message": "Session is full. You have been added to the waiting list."
}
```

##### 2. Get My Waiting List Positions
```
GET /api/waiting-list/my-positions
Authorization: Bearer <token>
```

**Response:**
```json
[
  {
    "sessionId": 1,
    "sessionTitle": "Advanced Mathematics",
    "sessionDateTime": "2025-01-15T10:00:00Z",
    "queuePosition": 2,
    "totalInQueue": 5
  }
]
```

##### 3. Get Session Waiting List (Admin/Teacher)
```
GET /api/waiting-list/session/{sessionId}
Authorization: Bearer <admin-token>
```

**Response:**
```json
{
  "sessionId": 1,
  "sessionTitle": "Advanced Mathematics",
  "sessionDateTime": "2025-01-15T10:00:00Z",
  "totalWaiting": 3,
  "entries": [...]
}
```

#### AdminController Enhancement
**Location:** `EduFlow/Controllers/AdminController.cs`

Added endpoint for admin viewing of waiting lists:
```
GET /api/admin/waiting-list/session/{sessionId}
```

#### BookingController Enhancement
**Location:** `EduFlow/Controllers/BookingController.cs`

Updated booking response to indicate status:
```json
{
  "bookingId": 10,
  "status": "Booked",
  "message": "Successfully booked the session."
}
// OR
{
  "waitingListEntryId": 5,
  "status": "Waiting",
  "message": "Session is full. You have been added to the waiting list."
}
```

---

## DTOs (Data Transfer Objects)

**Location:** `EduFlow.Infrastructure/Features/WaitingList/Queries/WaitingListDto.cs`

### WaitingListEntryDto
Individual queue entry information

### SessionWaitingListDto
Complete waiting list for a session with all entries

### StudentWaitingListPositionDto
Student's queue position across sessions

### JoinWaitingListDto
Request model for joining waiting list

---

## Dependency Injection Setup

**Location:** `EduFlow/Program.cs`

**Registered Services:**
```csharp
builder.Services.AddScoped<IWaitingListRepository, WaitingListRepository>();
builder.Services.AddScoped<IAutoBookingService, AutoBookingService>();
```

These are registered in the dependency injection container along with other services.

---

## Key Features Implemented

### ✅ Queue Position Tracking
- Queue positions automatically calculated based on request time
- Positions updated when users are removed from queue
- Unique constraint prevents duplicate entries

### ✅ Notification System
- When user moves from waiting → booked:
  - Notification created with message: "Great news! A spot has opened up in '{session.Title}'. Your booking is confirmed!"
  - IsRead flag set to false
  - CreatedAt timestamp recorded

- When user cancels booking:
  - Notification created: "Your booking for session '{session.Title}' has been canceled."

### ✅ Admin View
- GET /api/admin/waiting-list/session/{sessionId}
- Shows complete queue with student names, positions, and request times
- Sorted by queue position

### ✅ Prevent Duplicate Queue Entries
- Database unique constraint on (StudentId, SessionId)
- Repository method `IsInWaitingListAsync()` prevents addition
- CommandHandler validates before adding

### ✅ Automatic Queue Processing
- When session is full: add to waiting list instead of rejecting
- When user cancels: auto-book next in queue
- Transaction safety: all operations in transaction with rollback on failure
- Time conflict detection: removes conflicting entries and tries next

---

## Database Migration

Run the following to apply the migration:
```
dotnet ef database update
```

The migration:
- Creates `WaitingListEntries` table
- Adds foreign keys to Users and Sessions with cascade delete
- Creates indexes for optimal query performance

---

## Error Handling & Validation

**Common Error Scenarios:**

1. **Session not found**
   ```
   "Session not found"
   ```

2. **Already booked for session**
   ```
   "Already booked for this session"
   ```

3. **Already in waiting list**
   ```
   "Already in waiting list for this session"
   ```

4. **Session is canceled**
   ```
   "Session not available"
   ```

5. **Session already started**
   ```
   "Session already started"
   ```

6. **Time conflict**
   ```
   "Time conflict"
   ```

---

## Transaction Safety

All critical operations are wrapped in database transactions:

**BookSessionCommandHandler:**
- Opens transaction before processing
- Closes (commit) after successful booking or waiting list addition
- Rollback on any error

**CancelBookingCommandHandler:**
- Creates cancellation notification
- Saves changes
- Calls auto-booking service (which has its own transaction)

**AutoBookingService:**
- Opens transaction
- Validates capacity
- Creates booking
- Updates queue positions
- Creates notification
- Commits on success, rollback on failure

---

## Performance Optimizations

1. **Indexes:**
   - (StudentId, SessionId): Fast duplicate checking
   - SessionId: Fast queue retrieval
   - RequestTime: Maintains insertion order

2. **Query Optimization:**
   - Include related entities to avoid N+1 queries
   - OrderBy RequestTime ensures correct queue order
   - Single database round-trip per query

3. **Transaction Scoping:**
   - Minimal transaction scope
   - Quick operations to reduce lock contention

---

## API Usage Examples

### Example 1: Student Books a Session (Full)
```bash
# Session is full (capacity = 3, booked = 3)
POST /api/bookings
Authorization: Bearer <student-token>
Content-Type: application/json

{
  "sessionId": 1
}

# Response
{
  "waitingListEntryId": 5,
  "status": "Waiting",
  "message": "Session is full. You have been added to the waiting list."
}
```

### Example 2: View Queue Position
```bash
GET /api/waiting-list/my-positions
Authorization: Bearer <student-token>

# Response
[
  {
    "sessionId": 1,
    "sessionTitle": "Advanced Mathematics",
    "sessionDateTime": "2025-01-15T10:00:00Z",
    "queuePosition": 2,
    "totalInQueue": 5
  }
]
```

### Example 3: Another Student Cancels, Auto-booking Triggers
```bash
DELETE /api/bookings/10
Authorization: Bearer <student-token>

# Flow:
# 1. Booking 10 deleted
# 2. Session.BookedCount decremented from 3 to 2
# 3. Notification created for cancellation
# 4. Auto-booking service finds first in queue (position 1)
# 5. New booking created with auto-booked student
# 6. Student removed from waiting list
# 7. Notification sent to newly booked student
```

### Example 4: Admin Views Waiting List
```bash
GET /api/admin/waiting-list/session/1
Authorization: Bearer <admin-token>

# Response
{
  "sessionId": 1,
  "sessionTitle": "Advanced Mathematics",
  "sessionDateTime": "2025-01-15T10:00:00Z",
  "totalWaiting": 4,
  "entries": [
    {
      "id": 5,
      "studentId": "user-456",
      "studentName": "Jane Smith",
      "sessionId": 1,
      "sessionTitle": "Advanced Mathematics",
      "requestTime": "2025-01-14T14:35:00Z",
      "queuePosition": 1
    },
    ...
  ]
}
```

---

## Testing Recommendations

1. **Unit Tests:**
   - WaitingListRepository methods
   - AutoBookingService logic
   - Command handlers validation

2. **Integration Tests:**
   - Full booking flow with full session
   - Auto-booking on cancellation
   - Queue position updates
   - Notification creation

3. **Edge Cases:**
   - Multiple concurrent requests to join waiting list
   - Time conflicts during auto-booking
   - Cancellation when no one in queue
   - Session cancellation with pending queue

---

## Future Enhancements

1. **Batch Processing:**
   - Process multiple cancellations in one go
   - Optimize queue updates

2. **Priority Queue:**
   - VIP students get priority
   - Weighted queue positions

3. **Notifications:**
   - Email/SMS integration
   - Real-time WebSocket updates
   - Push notifications

4. **Admin Features:**
   - Manual queue reordering
   - Force auto-booking
   - Remove specific students from queue

5. **Analytics:**
   - Queue wait time statistics
   - Session capacity utilization
   - Auto-booking success rate

---

## Conclusion

The Waiting List system is fully integrated with the booking module and provides:
- ✅ Automatic queue management when sessions fill up
- ✅ Transparent queue position tracking for students
- ✅ Automatic booking when slots open up
- ✅ Duplicate entry prevention
- ✅ Admin visibility into all waiting lists
- ✅ Notification system for status changes
- ✅ Transaction safety and data consistency
- ✅ Performance-optimized database queries
