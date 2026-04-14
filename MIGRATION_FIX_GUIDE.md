# Waiting List System - Migration Fix & Troubleshooting

## 🔧 Error Fixed: Cascade Delete Cycle

### The Problem

When you ran `Update-Database`, you encountered this error:

```
Introducing FOREIGN KEY constraint 'FK_WaitingListEntries_Sessions_SessionId' 
on table 'WaitingListEntries' may cause cycles or multiple cascade paths. 
Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
```

### Why It Happened

SQL Server prevents **cascade delete cycles**. This occurs when:

1. **WaitingListEntry** table had TWO cascade delete foreign keys:
   - `FK_WaitingListEntries_AspNetUsers_StudentId` → Cascade Delete
   - `FK_WaitingListEntries_Sessions_SessionId` → Cascade Delete

2. If a Session is deleted while WaitingListEntries exist, SQL Server cannot determine which delete should happen first, potentially creating a cycle.

### The Solution ✅

Changed the Session foreign key from:
```csharp
onDelete: ReferentialAction.Cascade
```

To:
```csharp
onDelete: ReferentialAction.NoAction
```

**What this means:**
- When a **Student** is deleted → Their waiting list entries are deleted (Cascade)
- When a **Session** is deleted → Waiting list entries are NOT automatically deleted (NoAction)
  - Manual cleanup or business logic handles this
  - Prevents cascade delete cycles

---

## 📋 Files Fixed

### 1. Migration File
**File:** `EduFlow.Infrastructure/Migrations/20260415000001_AddWaitingListSystem.cs`

**Change:**
```csharp
// BEFORE (causes error):
table.ForeignKey(
    name: "FK_WaitingListEntries_Sessions_SessionId",
    column: x => x.SessionId,
    principalTable: "Sessions",
    principalColumn: "Id",
    onDelete: ReferentialAction.Cascade);  // ❌ Changed to NoAction

// AFTER (working):
table.ForeignKey(
    name: "FK_WaitingListEntries_Sessions_SessionId",
    column: x => x.SessionId,
    principalTable: "Sessions",
    principalColumn: "Id",
    onDelete: ReferentialAction.NoAction);  // ✅ Fixed
```

### 2. Configuration File
**File:** `EduFlow.Infrastructure/Persistence/Configurations/WaitingListEntryConfiguration.cs`

**Change:**
```csharp
// BEFORE (matching old migration):
builder.HasOne(w => w.Session)
    .WithMany()
    .HasForeignKey(w => w.SessionId)
    .OnDelete(DeleteBehavior.Cascade);  // ❌ Changed to NoAction

// AFTER (matching new migration):
builder.HasOne(w => w.Session)
    .WithMany()
    .HasForeignKey(w => w.SessionId)
    .OnDelete(DeleteBehavior.NoAction);  // ✅ Fixed
```

---

## 🚀 Next Steps

### Step 1: Remove Old Migration (if applied)
If the migration partially applied, remove it:

```bash
# In Package Manager Console
Update-Database -Migration 20260414165701_InitialCreate

# Or via CLI
dotnet ef database update --target-migration 20260414165701_InitialCreate
```

### Step 2: Apply the Fixed Migration
```bash
# In Package Manager Console
Update-Database

# Or via CLI
dotnet ef database update
```

### Step 3: Verify Success
You should see:
```
Build started...
Build succeeded.
Applying migration '20260415000001_AddWaitingListSystem'.
Done.
```

---

## 🔍 Understanding the Data Model

### Current Foreign Key Strategy

```
WaitingListEntry
├── StudentId (FK to AspNetUsers)
│   └── Delete Behavior: CASCADE
│       (When user is deleted, their queue entries are deleted)
│
└── SessionId (FK to Sessions)
    └── Delete Behavior: NO ACTION
        (When session is deleted, queue entries remain)
        (Can be manually handled or cause referential integrity error)
```

### Why This Design?

| Scenario | Behavior | Reason |
|----------|----------|--------|
| User deletes account | Queue entries deleted | Cleanup: No orphaned entries |
| Session is canceled | Queue entries kept | Preserve history: Can query why entries exist |
| Session is deleted | Queue entries cause error | Safety: Prevents accidental data loss |

---

## 📊 SQL Server Cascade Rules

### ✅ Allowed (No Cycles)
```
User → WaitingListEntry (Cascade)
Session → WaitingListEntry (No Action)
```

### ❌ Not Allowed (Creates Cycles)
```
User → WaitingListEntry (Cascade)
Session → WaitingListEntry (Cascade)
Session → User (Cascade)  ← Creates potential cycle
```

---

## 🛠️ Common Migration Issues & Solutions

### Issue 1: Migration Failed & Database is in Invalid State

**Solution:**
```bash
# Remove all migrations back to initial
Update-Database -Migration 20260414165701_InitialCreate

# Or delete and recreate database
Drop-Database
Update-Database
```

### Issue 2: "Pending Migrations" Error

**Solution:**
```bash
# View pending migrations
Get-Migration

# Apply all pending
Update-Database

# Or specific migration
Update-Database -Migration 20260415000001_AddWaitingListSystem
```

### Issue 3: Migration Not Found

**Solution:**
```bash
# Set Default Project to Infrastructure
# In Package Manager Console:
# Go to: Tools > NuGet Package Manager > Package Manager Console
# Set Default Project = "EduFlow.Infrastructure"

Update-Database
```

---

## 💡 Best Practices Going Forward

### 1. Test Migrations Locally
```bash
# Apply to local database first
Update-Database

# Verify in SQL Server Management Studio
SELECT * FROM WaitingListEntries
```

### 2. Always Include Down Migration
```csharp
protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropTable(
        name: "WaitingListEntries");
}
```
The Down method is already in the migration! ✅

### 3. Document Delete Behavior
```csharp
// In Entity Configuration
builder.HasOne(w => w.Session)
    .WithMany()
    .HasForeignKey(w => w.SessionId)
    .OnDelete(DeleteBehavior.NoAction);  // ← Clearly documented
```

### 4. Handle Orphaned Data
When using `NoAction`, handle deletion in code:

```csharp
// Before deleting session, clean up waiting list
var waitingEntries = await _dbSet
    .Where(w => w.SessionId == sessionId)
    .ToListAsync();

foreach (var entry in waitingEntries)
{
    _dbSet.Remove(entry);
}

// Then delete session
await _context.SaveChangesAsync();
```

---

## 📚 SQL Server Referential Action Behavior

| Action | Behavior |
|--------|----------|
| **Cascade** | Delete parent → Delete all children |
| **NoAction** | Delete parent → Error if children exist |
| **SetNull** | Delete parent → Set FK to NULL (column must be nullable) |
| **SetDefault** | Delete parent → Set FK to default value |

---

## ✅ Verification Checklist

After applying the fixed migration:

- [ ] `Update-Database` completes without errors
- [ ] New table appears in SQL Server Management Studio
- [ ] Table has correct columns and indexes
- [ ] Foreign keys are properly configured
- [ ] Can create waiting list entries
- [ ] Can query waiting list without errors
- [ ] Build is successful

---

## 🎯 Summary

**The Issue:** Cascade delete cycle prevented by SQL Server  
**The Fix:** Changed Session FK to `NoAction` instead of `Cascade`  
**The Result:** Migration now applies successfully  

**Files Modified:**
1. `20260415000001_AddWaitingListSystem.cs` - Migration file
2. `WaitingListEntryConfiguration.cs` - Configuration file

**Status:** ✅ Ready to run `Update-Database`

---

## 📞 If You Still Have Issues

1. **Check migration history:**
   ```bash
   Get-Migration
   ```

2. **View current database schema:**
   - Open SQL Server Management Studio
   - Navigate to your database
   - Check if WaitingListEntries table exists

3. **Run specific migration:**
   ```bash
   Update-Database -Migration 20260415000001_AddWaitingListSystem
   ```

4. **Complete rollback:**
   ```bash
   Update-Database -Migration 20260414165701_InitialCreate
   Update-Database
   ```

---

