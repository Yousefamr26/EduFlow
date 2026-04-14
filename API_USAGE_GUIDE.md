# Waiting List System - API Usage Guide

## Base URL
```
https://localhost:7001/api
```

## Authentication
All endpoints except public ones require JWT token in header:
```
Authorization: Bearer <your_jwt_token>
```

---

## Booking Endpoints

### 1. Book a Session (with Waiting List Support)

**Endpoint:**
```
POST /bookings
```

**Authentication:** Required (Student Role)

**Request Body:**
```json
{
  "sessionId": 1
}
```

**Response - Booking Successful (200 OK):**
```json
{
  "bookingId": 15,
  "status": "Booked",
  "message": "Successfully booked the session."
}
```

**Response - Added to Waiting List (200 OK):**
```json
{
  "waitingListEntryId": 7,
  "status": "Waiting",
  "message": "Session is full. You have been added to the waiting list."
}
```

**Error Responses:**

- **400 Bad Request** - Already booked:
```json
{
  "error": "Already booked"
}
```

- **400 Bad Request** - Time conflict:
```json
{
  "error": "Time conflict"
}
```

- **400 Bad Request** - Session canceled:
```json
{
  "error": "Session not available"
}
```

- **401 Unauthorized** - Not verified:
```json
{
  "error": "Access code verification required"
}
```

---

## Waiting List Endpoints

### 2. Join Waiting List (Manual)

**Endpoint:**
```
POST /waiting-list/join
```

**Authentication:** Required (Student Role)

**Request Body:**
```json
{
  "sessionId": 1
}
```

**Response - Success (200 OK):**
```json
{
  "waitingListEntryId": 8,
  "message": "Added to waiting list"
}
```

**Error Responses:**

- **400 Bad Request** - Already in list:
```json
{
  "error": "Already in waiting list for this session"
}
```

- **400 Bad Request** - Already booked:
```json
{
  "error": "Already booked for this session"
}
```

- **400 Bad Request** - Session not found:
```json
{
  "error": "Session not found"
}
```

---

### 3. Get My Waiting List Positions

**Endpoint:**
```
GET /waiting-list/my-positions
```

**Authentication:** Required (Student Role)

**Response - Success (200 OK):**
```json
[
  {
    "sessionId": 1,
    "sessionTitle": "Advanced Mathematics",
    "sessionDateTime": "2025-01-15T10:00:00Z",
    "queuePosition": 2,
    "totalInQueue": 5
  },
  {
    "sessionId": 3,
    "sessionTitle": "Physics Fundamentals",
    "sessionDateTime": "2025-01-16T14:00:00Z",
    "queuePosition": 1,
    "totalInQueue": 3
  }
]
```

**Response - No Waiting Lists (200 OK):**
```json
[]
```

---

### 4. Get Session Waiting List (Student View)

**Endpoint:**
```
GET /waiting-list/session/{sessionId}
```

**Authentication:** Required (Student Role)

**Path Parameters:**
- `sessionId` (int): Session ID to view waiting list for

**Example:**
```
GET /waiting-list/session/1
```

**Response - Success (200 OK):**
```json
{
  "sessionId": 1,
  "sessionTitle": "Advanced Mathematics",
  "sessionDateTime": "2025-01-15T10:00:00Z",
  "totalWaiting": 3,
  "entries": [
    {
      "id": 5,
      "studentId": "550e8400-e29b-41d4-a716-446655440000",
      "studentName": "Jane Smith",
      "sessionId": 1,
      "sessionTitle": "Advanced Mathematics",
      "requestTime": "2025-01-14T14:35:00Z",
      "queuePosition": 1
    },
    {
      "id": 6,
      "studentId": "550e8400-e29b-41d4-a716-446655440001",
      "studentName": "Bob Johnson",
      "sessionId": 1,
      "sessionTitle": "Advanced Mathematics",
      "requestTime": "2025-01-14T15:00:00Z",
      "queuePosition": 2
    },
    {
      "id": 7,
      "studentId": "550e8400-e29b-41d4-a716-446655440002",
      "studentName": "Alice Williams",
      "sessionId": 1,
      "sessionTitle": "Advanced Mathematics",
      "requestTime": "2025-01-14T15:30:00Z",
      "queuePosition": 3
    }
  ]
}
```

**Error Responses:**

- **400 Bad Request** - Session not found:
```json
{
  "error": "Session not found"
}
```

---

## Admin Endpoints

### 5. Get Session Waiting List (Admin View)

**Endpoint:**
```
GET /admin/waiting-list/session/{sessionId}
```

**Authentication:** Required (Admin Role)

**Path Parameters:**
- `sessionId` (int): Session ID to view waiting list for

**Example:**
```
GET /admin/waiting-list/session/1
```

**Response - Success (200 OK):**
```json
{
  "sessionId": 1,
  "sessionTitle": "Advanced Mathematics",
  "sessionDateTime": "2025-01-15T10:00:00Z",
  "totalWaiting": 3,
  "entries": [
    {
      "id": 5,
      "studentId": "550e8400-e29b-41d4-a716-446655440000",
      "studentName": "Jane Smith",
      "sessionId": 1,
      "sessionTitle": "Advanced Mathematics",
      "requestTime": "2025-01-14T14:35:00Z",
      "queuePosition": 1
    },
    {
      "id": 6,
      "studentId": "550e8400-e29b-41d4-a716-446655440001",
      "studentName": "Bob Johnson",
      "sessionId": 1,
      "sessionTitle": "Advanced Mathematics",
      "requestTime": "2025-01-14T15:00:00Z",
      "queuePosition": 2
    },
    {
      "id": 7,
      "studentId": "550e8400-e29b-41d4-a716-446655440002",
      "studentName": "Alice Williams",
      "sessionId": 1,
      "sessionTitle": "Advanced Mathematics",
      "requestTime": "2025-01-14T15:30:00Z",
      "queuePosition": 3
    }
  ]
}
```

---

## Booking Management Endpoints

### 6. Cancel Booking (Triggers Auto-booking)

**Endpoint:**
```
DELETE /bookings/{bookingId}
```

**Authentication:** Required (Student Role)

**Path Parameters:**
- `bookingId` (int): ID of booking to cancel

**Example:**
```
DELETE /bookings/15
```

**Response - Success (200 OK):**
```json
{}
```

**Auto-booking Flow:**
1. Booking is deleted
2. Session BookedCount is decremented
3. Cancellation notification is created for canceling student
4. Next person in waiting list is automatically booked (if available)
5. That person is removed from waiting list
6. Queue positions are updated
7. Booking confirmation notification is sent to newly booked student

**Error Responses:**

- **400 Bad Request** - Booking not found:
```json
{
  "error": "Booking not found"
}
```

- **401 Unauthorized** - Not authorized:
```json
{
  "error": "You are not allowed to cancel this booking"
}
```

---

## Notifications Endpoints

### 7. Get User Notifications

**Endpoint:**
```
GET /notifications
```

**Authentication:** Required

**Response - Success (200 OK):**
```json
[
  {
    "id": 1,
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "message": "Great news! A spot has opened up in 'Advanced Mathematics'. Your booking is confirmed!",
    "isRead": false,
    "createdAt": "2025-01-14T15:30:00Z"
  },
  {
    "id": 2,
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "message": "Your booking for session 'Physics Fundamentals' has been canceled.",
    "isRead": true,
    "createdAt": "2025-01-14T14:00:00Z"
  }
]
```

---

## cURL Examples

### Example 1: Book a Session (Session Full - Gets Waiting List)

```bash
curl -X POST https://localhost:7001/api/bookings \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -d '{
    "sessionId": 1
  }'
```

**Expected Response:**
```json
{
  "waitingListEntryId": 7,
  "status": "Waiting",
  "message": "Session is full. You have been added to the waiting list."
}
```

---

### Example 2: Check Waiting List Position

```bash
curl -X GET https://localhost:7001/api/waiting-list/my-positions \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

**Expected Response:**
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

### Example 3: Admin Views Session Queue

```bash
curl -X GET https://localhost:7001/api/admin/waiting-list/session/1 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

**Expected Response:**
```json
{
  "sessionId": 1,
  "sessionTitle": "Advanced Mathematics",
  "sessionDateTime": "2025-01-15T10:00:00Z",
  "totalWaiting": 3,
  "entries": [...]
}
```

---

### Example 4: Cancel Booking (Triggers Auto-booking)

```bash
curl -X DELETE https://localhost:7001/api/bookings/15 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

**Expected Response:**
```json
{}
```

**Behind the scenes:**
- First person in waiting list (position 1) is automatically booked
- They receive notification: "Great news! A spot has opened up..."
- Queue positions are recalculated (position 2 → 1, position 3 → 2, etc.)

---

## Postman Collection

### Import these requests into Postman:

#### Request 1: Book Session (Full - Waiting List)
```
Method: POST
URL: {{base_url}}/api/bookings
Headers:
  - Authorization: Bearer {{token}}
Body (raw JSON):
{
  "sessionId": 1
}
```

#### Request 2: Get My Queue Positions
```
Method: GET
URL: {{base_url}}/api/waiting-list/my-positions
Headers:
  - Authorization: Bearer {{token}}
```

#### Request 3: View Session Queue (Admin)
```
Method: GET
URL: {{base_url}}/api/admin/waiting-list/session/1
Headers:
  - Authorization: Bearer {{admin_token}}
```

#### Request 4: Cancel Booking
```
Method: DELETE
URL: {{base_url}}/api/bookings/{{bookingId}}
Headers:
  - Authorization: Bearer {{token}}
```

#### Request 5: Get Notifications
```
Method: GET
URL: {{base_url}}/api/notifications
Headers:
  - Authorization: Bearer {{token}}
```

---

## Status Codes Reference

| Code | Meaning |
|------|---------|
| 200 | OK - Request successful |
| 400 | Bad Request - Invalid input or business logic error |
| 401 | Unauthorized - Missing or invalid token |
| 403 | Forbidden - Insufficient permissions |
| 404 | Not Found - Resource doesn't exist |
| 500 | Internal Server Error |

---

## Flow Diagram: Session Booking with Waiting List

```
┌─────────────────┐
│  Student Books  │
│    Session      │
└────────┬────────┘
         │
         ▼
┌─────────────────────┐
│  Check Validations  │
│ - Not canceled      │
│ - Not started       │
│ - Not already booked│
│ - No conflicts      │
└────────┬────────────┘
         │
         ├─────────────────────────────────┐
         │                                 │
    ✓ PASS                            ✗ FAIL
         │                                 │
         ▼                                 ▼
┌──────────────────┐         ┌─────────────────────┐
│ Check Capacity   │         │   Return Error      │
└────────┬─────────┘         └─────────────────────┘
         │
         ├──────────────────────────┬─────────────────┐
         │                          │                 │
    AVAILABLE              FULL (BookedCount ≥ Capacity)
         │                          │
         ▼                          ▼
    ┌──────────┐       ┌────────────────────────┐
    │  CREATE  │       │ CHECK DUPLICATE IN     │
    │ BOOKING  │       │ WAITING LIST           │
    └────┬─────┘       └──────────┬─────────────┘
         │                        │
         │                   ┌────┴─────┐
         │                   │           │
         │              DUPLICATE    NO DUPLICATE
         │                   │           │
         │              RETURN ERR       ▼
         │                        ┌────────────────┐
         │                        │ CREATE WAITING │
         │                        │ LIST ENTRY     │
         │                        └────────┬───────┘
         │                                 │
         │                            ┌────┴───────┐
         │                            │             │
         ├─────────────────────────────┤             │
         │                             │             │
         ▼                             ▼             ▼
    ┌─────────┐              ┌────────────────┐
    │Increment │              │ RETURN WAITING │
    │BookCount │              │  LIST ENTRY ID │
    └────┬────┘               │   (negative)   │
         │                    └────────────────┘
         │
         ▼
    ┌──────────┐
    │   SAVE   │
    │ CHANGES  │
    └────┬─────┘
         │
         ▼
    ┌──────────────┐
    │RETURN BOOKING│
    │   ID         │
    │(positive)    │
    └──────────────┘


CANCELLATION FLOW (Auto-booking):

┌──────────────────┐
│ Cancel Booking   │
└────────┬─────────┘
         │
         ▼
    ┌──────────┐
    │Decrement │
    │BookCount │
    └────┬─────┘
         │
         ▼
    ┌───────────────────┐
    │ Create Notification│
    │ (Cancellation)    │
    └────────┬──────────┘
             │
             ▼
    ┌──────────────────────────┐
    │ AUTO BOOKING SERVICE:    │
    │ GetFirstInQueue          │
    └────────┬─────────────────┘
             │
         ┌───┴────┐
         │        │
       FOUND    NOT FOUND
         │        │
         ▼        ▼
    ┌────┐    DONE
    │Check
    │Time  │
    │Conflict
    └──┬─┘
       │
    ┌──┴──┐
    │     │
  YES    NO
    │     │
    │     ▼
    │  ┌──────────────┐
    │  │ CREATE NEW   │
    │  │ BOOKING      │
    │  └────┬─────────┘
    │       │
    │       ▼
    │  ┌──────────────────┐
    │  │ INCREMENT BOOKED │
    │  │ COUNT            │
    │  └────┬─────────────┘
    │       │
    │       ▼
    │  ┌──────────────────┐
    │  │ REMOVE FROM      │
    │  │ WAITING LIST     │
    │  └────┬─────────────┘
    │       │
    │       ▼
    │  ┌──────────────────┐
    │  │ UPDATE QUEUE     │
    │  │ POSITIONS        │
    │  └────┬─────────────┘
    │       │
    │       ▼
    │  ┌──────────────────┐
    │  │ CREATE           │
    │  │ NOTIFICATION     │
    │  │ (Auto-booked)    │
    │  └────┬─────────────┘
    │       │
    ▼       ▼
┌─────────────────┐
│   COMMIT        │
│  TRANSACTION    │
└─────────────────┘
```

---

## Troubleshooting

### Issue: "Already in waiting list"
**Cause:** Student is already on the waiting list for this session
**Solution:** Check waiting list position with `/waiting-list/my-positions`

### Issue: "Already booked"
**Cause:** Student already has a booking for this session
**Solution:** Cancel the existing booking first or check existing bookings

### Issue: "Time conflict"
**Cause:** Student has another booking at the same time
**Solution:** Cancel the conflicting booking or choose different session

### Issue: "Session full. Added to waiting list"
**Cause:** Session has reached capacity
**Solution:** Wait for someone to cancel or check queue position regularly

### Issue: Auto-booking didn't trigger
**Cause:** Person at front of queue has time conflict with another booking
**Solution:** That person is automatically removed, next person is booked

---

