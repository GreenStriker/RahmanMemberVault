# API Reference

## Base URL
`https://<your-domain>/api/v1`

## Authentication
This API uses **Anonymous** access in its current configuration. No authentication is required. Future versions may add token-based auth.

---

## Endpoints

### Retrieve All Members
```
GET /member
```
**Description**: Returns a list of all members.

**Response**  
- **200 OK**  
  ```json
  [
    {
      "id": 1,
      "name": "Test User",
      "email": "test@test.com",
      "phoneNumber": "1234567890",
      "isActive": true,
      "dateJoined": "2025-04-01T12:34:56Z"
    }
    // ...
  ]
  ```

---

### Retrieve Member by ID
```
GET /member/{id}
```
**Description**: Returns the member with the specified ID.

**Parameters**  
- `id` (int, required): Unique identifier of the member.

**Responses**  
- **200 OK**  
  Returns a single `MemberDto` object.  
- **404 Not Found**  
  ```json
  { "error": "Member with ID {id} was not found.", "statusCode": 404 }
  ```

---

### Create a New Member
```
POST /member
```
**Description**: Creates a new member record.

**Request Body**  
A `CreateMemberDto` object:
```json
{
  "name": "John Doe",
  "email": "john.doe@example.com",
  "phoneNumber": "5551234567"
}
```

**Validation Rules**  
- `name`: non-empty, max length 100  
- `email`: valid email format  
- `phoneNumber`: digits only, 10–20 characters and may include spaces or dashes

**Responses**  
- **201 Created**  
  Location header set to `/member/{newId}`. Returns created `MemberDto`.  
- **400 Bad Request**  
  ```json
  {
    "errors": {
      "Email": ["Email must be a valid email address."]
    },
    "statusCode": 400
  }
  ```

---

### Update an Existing Member
```
PUT /member/{id}
```
**Description**: Updates an existing member.

**Parameters**  
- `id` (int, required): ID of the member to update.

**Request Body**  
An `UpdateMemberDto` object:
```json
{
  "id": 1,
  "name": "John Smith",
  "email": "john.smith@example.com",
  "phoneNumber": "5559876543",
  "isActive": true
}
```

**Validation & Behavior**  
- ID in URL must match `id` in body or returns 400.  
- Same validation rules as create.  

**Responses**  
- **200 OK**: Returns updated `MemberDto`.  
- **400 Bad Request**: ID mismatch or validation errors.  
- **404 Not Found**: Member not found.

---

### Delete a Member
```
DELETE /member/{id}
```
**Description**: Deletes a member record.

**Parameters**  
- `id` (int, required): ID of the member to delete.

**Responses**  
- **204 No Content**: Successfully deleted.  
- **404 Not Found**: Member not found.

---

## Error Response Format

All errors use a consistent JSON structure:

```json
{
  "error": "Error message",
  "statusCode": 500,
  "stackTrace": null  // only present in Development
}
```

---

## Data Transfer Objects

### MemberDto
| Property    | Type    | Description                    |
|-------------|---------|--------------------------------|
| `id`        | int     | Unique identifier              |
| `name`      | string  | Full name of the member        |
| `email`     | string  | Email address                  |
| `phoneNumber` | string | Contact phone number           |
| `isActive`  | bool    | Active status                  |
| `DateJoined` | string (DateTime) | Creation timestamp    |

### CreateMemberDto
| Property      | Type   | Description              |
|---------------|--------|--------------------------|
| `name`        | string | Full name (required)     |
| `email`       | string | Email (required)         |
| `phoneNumber` | string | Phone number (required)  |

### UpdateMemberDto
| Property      | Type    | Description                     |
|---------------|---------|---------------------------------|
| `id`          | int     | Unique identifier (required)    |
| `name`        | string  | Full name (required)            |
| `email`       | string  | Email (required)                |
| `phoneNumber` | string  | Phone number (required)         |
| `isActive`    | bool    | Active status (required)        |

---

## Swagger UI
Interactive documentation is available at `/swagger` in both Development and Production environments when enabled.

---
