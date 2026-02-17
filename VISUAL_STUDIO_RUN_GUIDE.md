# ABC College Library Management System
## Complete Visual Studio Guide (Build, Run, Validate, and Package)

This guide explains exactly how to run the application in **Visual Studio on Windows** from a fresh machine.

---

## 1) What you are running

Solution structure:

- `ABCLibrary.Domain` → Entities, enums, domain types
- `ABCLibrary.Application` → DTOs and interfaces
- `ABCLibrary.Infrastructure` → EF Core SQLite, repositories, services
- `ABCLibrary.Wpf` → WPF UI (MVVM)
- `ABCLibrary.sln` → Visual Studio solution entry point

Database target path used by the app:

- `C:\ProgramData\ABCLibrary\library.db`

On first run, app bootstrap logic:

1. Creates DB folder if missing.
2. Creates SQLite DB if missing (`EnsureCreated`).
3. Enables FK constraint enforcement (`PRAGMA foreign_keys = ON`).
4. Seeds default admin user if no users exist.

Default login credentials:

- Username: `admin`
- Password: `Admin@123`

> Change password immediately in production use.

---

## 2) Prerequisites (Windows)

Install the following:

1. **Windows 10/11**
2. **Visual Studio 2022** (17.8+ recommended)
3. Visual Studio workloads/components:
   - **.NET desktop development** (required for WPF)
   - **.NET 8 SDK** targeting pack
4. Optional but recommended:
   - Git for Windows
   - SQLite Browser (for DB inspection)

NuGet packages are restored automatically from project files.

---

## 3) Open and restore in Visual Studio

1. Clone/download repo.
2. Open `ABCLibrary.sln` in Visual Studio.
3. Wait for solution load.
4. Restore packages:
   - Right click solution → **Restore NuGet Packages**.
5. Confirm no restore errors in **Output** window.

If you prefer CLI from Visual Studio Developer Prompt:

```powershell
dotnet restore ABCLibrary.sln
```

---

## 4) Build the full solution

In Visual Studio:

1. Select configuration: **Debug** or **Release**.
2. Build → **Build Solution** (`Ctrl+Shift+B`).
3. Confirm `Build succeeded`.

CLI equivalent:

```powershell
dotnet build ABCLibrary.sln -c Debug
```

> WPF project requires Windows desktop SDK and cannot be fully built on Linux-only environments.

---

## 5) Set startup project and run

1. In Solution Explorer, right-click `ABCLibrary.Wpf` → **Set as Startup Project**.
2. Press `F5` (Debug) or `Ctrl+F5` (Run without debugger).
3. Login with:
   - `admin`
   - `Admin@123`

Expected first-run artifacts:

- Folder: `C:\ProgramData\ABCLibrary`
- File: `C:\ProgramData\ABCLibrary\library.db`

---

## 6) Quick functional verification checklist

After launching the app:

1. **Login** works with default admin.
2. Dashboard loads without crash.
3. Click **Add Sample Book** (creates book + copies + barcodes).
4. Click **Add Sample Student**.
5. Try **Issue** with a created barcode + roll.
6. Try **Return** to compute fine (if overdue).
7. Click **Export Inventory** and confirm Excel output on desktop.
8. Click **Backup Database** and confirm `.db` backup file.

---

## 7) Barcode generation & printing notes

### Barcode generation
- Format: `LIB-0000001`
- Code type: **Code 128** via ZXing
- Generated in service layer (`BarcodeService`)

### Printer (TSC TTP-244 Pro)
Current service sends TSPL commands to a shared printer name:

- Ensure printer installed and shared on Windows.
- Ensure app has permission to access `\\localhost\<PrinterShareName>`.

If printing fails:
1. Verify printer share name.
2. Test command from cmd manually.
3. Check Windows printer queue and permissions.

---

## 8) Database and operational behavior

- DB provider: SQLite
- Main DB file: `C:\ProgramData\ABCLibrary\library.db`
- Soft-delete filters are configured for key entities.
- Auth uses BCrypt hash verification.
- Circulation logic enforces transaction scope and borrow limits.

---

## 9) Common troubleshooting

### A) `Microsoft.NET.Sdk.WindowsDesktop` not found
Cause: missing desktop workload/SDK.

Fix:
1. Open Visual Studio Installer.
2. Modify installation.
3. Add **.NET desktop development** workload.
4. Ensure .NET 8 SDK targeting installed.

### B) NuGet restore errors
Fix:
1. Tools → NuGet Package Manager → Package Sources: enable `nuget.org`.
2. Clear cache:
   ```powershell
   dotnet nuget locals all --clear
   ```
3. Restore again.

### C) DB creation/permission issues
Fix:
1. Ensure user can write to `C:\ProgramData\ABCLibrary`.
2. Run Visual Studio as Administrator once to validate.
3. Verify antivirus is not blocking DB file creation.

### D) Printer command issues
Fix:
1. Confirm exact printer share path.
2. Confirm command shell execution policy is not restricted.
3. Check temp file permissions in `%TEMP%`.

---

## 10) Recommended production hardening before deployment

1. Replace default credentials and add change-password flow.
2. Add global exception handling and user-friendly dialogs.
3. Add structured log viewing UI filters.
4. Add EF migrations pipeline (instead of only `EnsureCreated`) for schema evolution.
5. Add installer packaging (MSIX/WiX/Inno Setup) with:
   - Desktop shortcut
   - Start menu entry
   - first-run checks

---

## 11) Publish build from Visual Studio

1. Right-click `ABCLibrary.Wpf` → **Publish**.
2. Choose target folder profile.
3. Deployment mode:
   - Framework-dependent (smaller, requires .NET runtime), or
   - Self-contained (larger, includes runtime).
4. Publish and test on a clean Windows machine.

CLI sample publish command:

```powershell
dotnet publish ABCLibrary.Wpf/ABCLibrary.Wpf.csproj -c Release -r win-x64 --self-contained false
```

---

## 12) Validation commands (Windows CLI)

Run these in repo root:

```powershell
dotnet --info
dotnet restore ABCLibrary.sln
dotnet build ABCLibrary.sln -c Release
```

Optional tests (if you add test projects later):

```powershell
dotnet test
```

---

## 13) Notes on .NET version planning

Current project target is .NET 8. You can continue with .NET 8 if your environment is standardized there.
If your organization has moved to a newer LTS baseline, perform a coordinated upgrade (TFMs + package versions + regression build on Windows).

---


## 14) Optional CMake wrapper

A top-level `CMakeLists.txt` is included to orchestrate the same .NET commands via CMake targets.

Configure:

```powershell
cmake -S . -B build
```

Run restore:

```powershell
cmake --build build --target abclibrary_restore
```

Run build:

```powershell
cmake --build build --target abclibrary_build
```

Publish WPF executable:

```powershell
cmake --build build --target abclibrary_publish
```

Notes:
- `abclibrary_build` and `abclibrary_publish` require Windows desktop SDK support because the WPF project uses `Microsoft.NET.Sdk.WindowsDesktop`.
- On non-Windows environments, use this wrapper mainly for `abclibrary_restore` and non-WPF project validation.

---

If you want, I can also provide:
- a **step-by-step installer guide** (WiX or Inno Setup), and/or
- a **team onboarding checklist** with screenshots for librarians.
